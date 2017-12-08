using PayChain.Common.Entities;

namespace PayChain.Common.Responses
{
    public class RegisterResponse : BaseResponse
    {
        public Node[] AddedNodes { get; set; }

        public RegisterResponse(Node[] nodes)
        {
            Message = "Registered new node connections";
            AddedNodes = nodes;
        }
    }
}
