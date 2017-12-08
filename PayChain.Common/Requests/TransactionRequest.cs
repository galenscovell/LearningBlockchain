namespace PayChain.Common.Requests
{
    public class TransactionRequest
    {
        public float Amount { get; set; }

        public string Sender { get; set; }

        public string Recipient { get; set; }
    }
}
