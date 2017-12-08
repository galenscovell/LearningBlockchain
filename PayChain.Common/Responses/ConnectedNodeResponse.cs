using PayChain.Common.Entities;

namespace PayChain.Common.Responses
{
    public class ConnectedNodeResponse : BaseResponse
    {
        public Node[] ConnectedNodes { get; set; }

        public ConnectedNodeResponse(Node[] nodes)
        {
            ConnectedNodes = nodes;
        }
    }
}
