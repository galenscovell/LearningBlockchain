using System.Collections.Generic;
using System.Threading.Tasks;
using PayChain.Common.Entities;
using PayChain.Common.Responses;

namespace PayChain.Backend.Services
{
    public interface IChain
    {
        ConfirmedTxResponse GetConfirmed();

        UnconfirmedTxResponse GetUnconfirmed();

        ConnectedNodeResponse GetConnectedNodes();

        TransactionResponse CreateTransaction(string sender, string recipient, float amount);

        RegisterNodeResponse RegisterNodes(List<Node> nodes);

        Task<MineResponse> Mine();

        Task<ResolveResponse> ReachConsensus();
    }
}
