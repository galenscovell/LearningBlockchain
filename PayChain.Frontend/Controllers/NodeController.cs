using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayChain.Backend.Services;
using PayChain.Common.Requests;

namespace PayChain.Frontend.Controllers
{
    [ApiVersion("1.0")]
    public class NodeController : BaseController
    {
        private readonly IChain _localBlockChain;

        public NodeController(ILogger logger, IChain blockChain)
            : base(logger)
        {
            _localBlockChain = blockChain;
        }

        /// <summary>
        /// Attempt to complete the next block on the blockchain.
        /// </summary>
        [HttpGet]
        [Route("mine")]
        public async Task<JsonResult> Mine()
        {
            return await ProcessRequest(async () =>
            {
                var response = await _localBlockChain.Mine();

                return response == null
                    ? GenerateAcceptedResponse("No unconfirmed transactions on chain")
                    : GenerateOkResponse(response);
            });
        }

        /// <summary>
        /// Query all connected nodes and reach consensus on the state of the blockchain.
        /// </summary>
        [HttpGet]
        [Route("resolve")]
        public async Task<JsonResult> Resolve()
        {
            return await ProcessRequest(async () =>
            {
                var response = await _localBlockChain.ReachConsensus();

                return GenerateOkResponse(response);
            });
        }

        /// <summary>
        /// Get all completed transactions on the blockchain.
        /// </summary>
        [HttpGet]
        [Route("details")]
        public async Task<JsonResult> Completed()
        {
            return await ProcessRequest(async () =>
            {
                var response = _localBlockChain.GetConfirmed();

                return GenerateOkResponse(response);
            });
        }

        /// <summary>
        /// Get all currently unconfirmed transactions on the blockchain.
        /// </summary>
        [HttpGet]
        [Route("unconfirmed")]
        public async Task<JsonResult> Unconfirmed()
        {
            return await ProcessRequest(async () =>
            {
                var response = _localBlockChain.GetUnconfirmed();

                return GenerateOkResponse(response);
            });
        }

        /// <summary>
        /// Get all currently connected nodes.
        /// </summary>
        [HttpGet]
        [Route("connections")]
        public async Task<JsonResult> Connections()
        {
            return await ProcessRequest(async () =>
            {
                var response = _localBlockChain.GetConnectedNodes();

                return GenerateOkResponse(response);
            });
        }



        /// <summary>
        /// Establish connections with other nodes.
        /// </summary>
        [HttpPost]
        [Route("register")]
        public async Task<JsonResult> Register([FromBody] RegisterRequest request)
        {
            // TODO: Check that all required values are in request

            return await ProcessRequest(async () =>
            {
                var response = _localBlockChain.RegisterNodes(request.Nodes);

                return GenerateOkResponse(response);
            });
        }

        /// <summary>
        /// Create a new transaction on the blockchain.
        /// </summary>
        [HttpPost]
        [Route("transaction")]
        public async Task<JsonResult> Transaction([FromBody] TransactionRequest request)
        {
            // TODO: Check that all required values are in request

            return await ProcessRequest(async () =>
            {
                var response = _localBlockChain.CreateTransaction(request.Sender, request.Recipient, request.Amount);

                return GenerateOkResponse(response);
            });
        }
    }
}
