using System.Collections.Generic;
using PayChain.Common.Entities;

namespace PayChain.Common.Requests
{
    public class RegisterNodeRequest
    {
        public List<Node> Nodes { get; set; }
    }
}
