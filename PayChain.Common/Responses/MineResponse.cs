using PayChain.Common.Entities;

namespace PayChain.Common.Responses
{
    public class MineResponse : BaseResponse
    {
        public int Index { get; set; }

        public Transaction[] Transactions { get; set; }

        public int Proof { get; set; }

        public string PreviousHash { get; set; }

        public MineResponse(int index, Transaction[] transactions, int proof, string previousHash)
        {
            Message = "New Block Forged";
            Index = index;
            Transactions = transactions;
            Proof = proof;
            PreviousHash = previousHash;
        }
    }
}
