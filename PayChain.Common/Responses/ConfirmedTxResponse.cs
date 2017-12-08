using PayChain.Common.Entities;

namespace PayChain.Common.Responses
{
    public class ConfirmedTxResponse : BaseResponse
    {
        public int Length { get; set; }

        public Block[] FullChain { get; set; }

        public ConfirmedTxResponse(Block[] fullChain)
        {
            Message = "OK";
            Length = fullChain.Length;
            FullChain = fullChain;
        }
    }
}
