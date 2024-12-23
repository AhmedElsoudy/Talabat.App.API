using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TalabatApp.Errors;
using TalabatApp.Repository.Data;

namespace TalabatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreContext _dbcontext;

        public BuggyController(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }


        [HttpGet("notfound")]                // GET : api/Buggy/notfound
        public ActionResult GetNotFound()    
        {
            var product = _dbcontext.Products.Find(100);
            if(product == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            return Ok(product);
        }

        [HttpGet("servererror")]            // GET : api/Buggy/servererror
        public ActionResult GetServerError()
        {
            var product = _dbcontext.Products.Find(100);
            var productDto = product.ToString();
            return Ok(productDto);

        }

        [HttpGet("badrequest")]            // GET : api/Buggy/badrequest
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiErrorResponse(400));
        }


        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }


    }
}
