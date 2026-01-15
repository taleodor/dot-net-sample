using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace MinDotNetSample.Controllers;

[ApiController]
[Route("hello")]
public sealed class HelloController : ControllerBase
{
    private readonly ILogger<HelloController> _logger;

    public HelloController(ILogger<HelloController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        foreach (KeyValuePair<string, StringValues> header in Request.Headers)
        {
            _logger.LogInformation("Header {HeaderName}: {HeaderValue}", header.Key, header.Value.ToString());
        }

        return Ok("Hello, World!");
    }
}
