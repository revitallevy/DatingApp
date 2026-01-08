using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")] // localhost:5001/api/members
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
