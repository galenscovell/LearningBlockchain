namespace PayChain.Common.Responses
{
    public class TransactionResponse : BaseResponse
    {
        public int Index { get; set; }

        public string Sender { get; set; }

        public string Recipient { get; set; }

        public double Amount { get; set; }


        public TransactionResponse(int idx, string sender, string recipient, double amount)
        {
            Message = "Transaction created";
            Index = idx;
            Sender = sender;
            Recipient = recipient;
            Amount = amount;
        }
    }
}
