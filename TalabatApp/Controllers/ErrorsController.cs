using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Errors;

namespace TalabatApp.Controllers
{
    [Route("Errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {

        public ActionResult NotFoundEndpointOrUnAuthorized(int code)
        {

            if(code == 401)
            {
                return Unauthorized(new ApiErrorResponse(401));

            }else if(code == 404)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            else
            {
                return StatusCode(code);
            }

           
        }
    }
}
