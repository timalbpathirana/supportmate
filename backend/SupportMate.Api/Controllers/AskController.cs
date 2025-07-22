using Microsoft.AspNetCore.Mvc;
using SupportMate.Api.Services;

namespace SupportMate.Api.Controllers
{
    [ApiController]
    [Route("ask")]
    public class AskController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;

        public AskController(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public class AskRequest
        {
            public string? Question { get; set; }
        }

        public class AskResponse
        {
            public string? Answer { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Ask([FromBody] AskRequest request)
        {
            var answer = await _openAIService.AskOpenAIAsync(request.Question);
            var response = new AskResponse
            {
                Answer = answer
            };

            return Ok(response);
        }
    }
}