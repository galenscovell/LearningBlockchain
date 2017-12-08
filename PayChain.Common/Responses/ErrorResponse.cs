namespace PayChain.Common.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(string error)
        {
            Message = error;
        }
    }
}
