using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers;

[ApiController]
[Route("ask")]
public class AskController : ControllerBase
{
    [HttpPost]
    public IActionResult Ask([FromBody] AskRequest request)
    {
        var response = new AskResponse
        {
            Answer = $"You asked: {request.Question} - I will answer you in a moment"
        };

        return Ok(response);
    }
}