namespace PayChain.Common.Responses
{
    public class ResolveResponse : BaseResponse
    {
        public ResolveResponse(bool replaced)
        {
            var status = replaced ? "was replaced" : "is authoritive";
            Message = $"Local chain {status}";
        }
    }
}
