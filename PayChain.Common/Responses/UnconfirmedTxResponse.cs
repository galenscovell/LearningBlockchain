using PayChain.Common.Entities;

namespace PayChain.Common.Responses
{
    public class UnconfirmedTxResponse : BaseResponse
    {
        public int Length { get; set; }

        public Transaction[] UnconfirmedTransactions { get; set; }

        public UnconfirmedTxResponse(Transaction[] currentTx)
        {
            Message = "OK";
            Length = currentTx.Length;
            UnconfirmedTransactions = currentTx;
        }
    }
}
