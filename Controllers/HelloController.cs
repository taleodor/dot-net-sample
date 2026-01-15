using Microsoft.AspNetCore.Mvc;

namespace MinDotNetSample.Controllers;

[ApiController]
[Route("hello")]
public sealed class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("Hello, World!");
}
