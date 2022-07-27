using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StatInstik.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "ApiScope")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(User.Claims.Select(claim => new { claim.Type, claim.Value }));
        }
    }
}
