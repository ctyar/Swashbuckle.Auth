using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sample;

[Authorize]
[Route("")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("path")]
    public OkResult Get()
    {
        return Ok();
    }
}
