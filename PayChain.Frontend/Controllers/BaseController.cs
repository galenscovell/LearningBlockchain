using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayChain.Common.Responses;

namespace PayChain.Frontend.Controllers
{
    [Route("[controller]")]
    public class BaseController : Controller
    {
        private readonly ILogger _logger;

        protected BaseController(ILogger logger)
        {
            _logger = logger;
        }

        protected async Task<JsonResult> ProcessRequest(Func<Task<JsonResult>> processor)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                if (ModelState.ErrorCount > 0)
                {
                    var errorslist = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                    return Json(errorslist);
                }

                return Json(ModelState);
            }

            try
            {
                return await processor();
            }
            catch (Exception ex)
            {
                return GenerateInternalErrorResponse(ex);
            }
        }

        protected JsonResult GenerateOkResponse<T>(T response)
            where T : BaseResponse
        {
            Response.StatusCode = (int)HttpStatusCode.OK;

            return Json(response);
        }

        protected JsonResult GenerateAcceptedResponse(string message)
        {
            Response.StatusCode = (int)HttpStatusCode.Accepted;

            return Json(new BaseResponse { Message = message });
        }

        protected JsonResult GenerateBadRequestResponse(Exception ex)
        {
            _logger.LogInformation(ex.ToString());
            Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return Json(new BaseResponse { Message = ex.Message });
        }

        protected JsonResult GenerateInternalErrorResponse(Exception ex)
        {
            _logger.LogError(ex.ToString());
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return Json(new ErrorResponse(ex.Message));
        }
    }
}
