namespace PayChain.Common.Responses
{
    public class TransactionResponse : BaseResponse
    {
        public int Index { get; set; }

        public string Sender { get; set; }

        public string Recipient { get; set; }

        public float Amount { get; set; }


        public TransactionResponse(int idx, string sender, string recipient, float amount)
        {
            Message = "Transaction created";
            Index = idx;
            Sender = sender;
            Recipient = recipient;
            Amount = amount;
        }
    }
}
