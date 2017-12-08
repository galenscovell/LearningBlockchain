using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using PayChain.Common.Entities;

namespace PayChain.Backend
{
    public static class Utility
    {
        public static int CreateProofOfWork(int lastProof, string previousHash)
        {
            var proof = 0;

            while (!IsValidProof(lastProof, proof, previousHash))
            {
                proof++;
            }

            return proof;
        }

        public static string GetHash(Block block)
        {
            var blockText = JsonConvert.SerializeObject(block);

            return GetSha256(blockText);
        }

        public static bool IsValidChain(IReadOnlyCollection<Block> chain)
        {
            var lastBlock = chain.First();
            var currentIndex = 1;

            while (currentIndex < chain.Count)
            {
                var block = chain.ElementAt(currentIndex);
                if (block.PreviousHash != GetHash(lastBlock))
                {
                    return false;
                }

                if (!IsValidProof(lastBlock.Proof, block.Proof, lastBlock.PreviousHash))
                {
                    return false;
                }

                lastBlock = block;
                currentIndex++;
            }

            return true;
        }

        private static bool IsValidProof(int lastProof, int proof, string previousHash)
        {
            var guess = $"{lastProof}{proof}{previousHash}";
            var result = GetSha256(guess);

            return result.StartsWith("0000");
        }

        private static string GetSha256(string data)
        {
            var sha256 = new SHA256Managed();
            var hashBuilder = new StringBuilder();

            var bytes = Encoding.Unicode.GetBytes(data);
            var hash = sha256.ComputeHash(bytes);

            foreach (var x in hash)
            {
                hashBuilder.Append($"{x:x2}");
            }

            return hashBuilder.ToString();
        }
    }
}
