using PayChain.Common.Entities;

namespace PayChain.Common.Responses
{
    public class RegisterNodeResponse : BaseResponse
    {
        public Node[] AddedNodes { get; set; }

        public RegisterNodeResponse(Node[] nodes)
        {
            Message = "Registered new node connections";
            AddedNodes = nodes;
        }
    }
}
