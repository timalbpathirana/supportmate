using SupportMate.Api.Models;

namespace SupportMate.Api.Services
{
    public interface IOpenAIService
    {
        Task<string> AskOpenAIAsync(string userQuestion);
    }
}