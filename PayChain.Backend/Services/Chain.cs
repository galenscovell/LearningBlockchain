﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PayChain.Common.Entities;
using PayChain.Common.Responses;

namespace PayChain.Backend.Services
{
    public class Chain : IChain
    {
        private readonly IRequestClient _requestClient;

        private readonly List<Transaction> _currentTransactions = new List<Transaction>();
        private readonly List<Node> _nodes = new List<Node>();

        private List<Block> _chain = new List<Block>();
        private Block LastBlock => _chain.Last();

        public string NodeId { get; }

        public Chain(IRequestClient requestClient)
        {
            _requestClient = requestClient;

            NodeId = Guid.NewGuid().ToString().Replace("-", "");
            CreateNewBlock(100, "1");
        }

        public ConfirmedTxResponse GetConfirmed()
        {
            return new ConfirmedTxResponse(_chain.ToArray());
        }

        public UnconfirmedTxResponse GetUnconfirmed()
        {
            return new UnconfirmedTxResponse(_currentTransactions.ToArray());
        }

        public TransactionResponse CreateTransaction(string sender, string recipient, float amount)
        {
            var transaction = new Transaction
            {
                Sender = sender,
                Recipient = recipient,
                Amount = amount
            };

            _currentTransactions.Add(transaction);

            return new TransactionResponse(LastBlock?.Index + 1 ?? 0, sender, recipient, amount);
        }

        public ConnectedNodeResponse GetConnectedNodes()
        {
            return new ConnectedNodeResponse(_nodes.ToArray());
        }

        public RegisterNodeResponse RegisterNodes(List<string> nodeAddresses)
        {
            var addedNodes = new List<Node>();
            foreach (var address in nodeAddresses)
            {
                var newNode = new Node { Address = new Uri($"http://{address}") };
                _nodes.Add(newNode);
                addedNodes.Add(newNode);
            }

            return new RegisterNodeResponse(addedNodes.ToArray());
        }

        public async Task<MineResponse> Mine()
        {
            if (_currentTransactions.Count > 0)
            {
                var proof = await Task.Run(() => Utility.CreateProofOfWork(LastBlock.Proof, LastBlock.PreviousHash));

                CreateTransaction("0", NodeId, 1);
                var block = CreateNewBlock(proof);

                return new MineResponse(block.Index, block.Transactions.ToArray(), block.Proof, block.PreviousHash);
            }

            return null;
        }

        public async Task<ResolveResponse> ReachConsensus()
        {
            var replaced = await ResolveConflicts();

            return new ResolveResponse(replaced);
        }

        private async Task<bool> ResolveConflicts()
        {
            List<Block> newChain = null;
            var maxLength = _chain.Count;

            foreach (var node in _nodes)
            {
                var url = new Uri(node.Address, "/node/confirmed");
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await _requestClient.MakeRequest(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var model = new
                    {
                        fullChain = new List<Block>(),
                        length = 0
                    };

                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeAnonymousType(json, model);

                    if (data.fullChain.Count > maxLength && Utility.IsValidChain(data.fullChain))
                    {
                        maxLength = data.fullChain.Count;
                        newChain = data.fullChain;
                    }
                }
            }

            if (newChain != null)
            {
                _chain = newChain;
                return true;
            }

            return false;
        }

        private Block CreateNewBlock(int proof, string previousHash = null)
        {
            var block = new Block
            {
                Index = _chain.Count,
                Timestamp = DateTime.UtcNow,
                Transactions = _currentTransactions.ToList(),
                Proof = proof,
                PreviousHash = previousHash ?? Utility.GetHash(_chain.Last())
            };

            _currentTransactions.Clear();
            _chain.Add(block);

            return block;
        }
    }
}
