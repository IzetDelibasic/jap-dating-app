using DatingApp.Infrastructure.Interfaces.IServices;
using Microsoft.Extensions.Configuration;
using OpenAI_API;

namespace DatingApp.Services.Services;

public class IntelligentAssistantService(IConfiguration configuration) : IIntelligentAssistantService
{
    public async Task<string> GenerateIntroductionMessageAsync(string interests, string lookingFor)
    {
        var authentication = new APIAuthentication(configuration["OpenAI:ApiKey"]);
        var api = new OpenAIAPI(authentication);

        var prompt = $"Generate a short and friendly introduction message for dating. " +
             $"Interests: {interests}. Looking for: {lookingFor}.";

        var conversation = api.Chat.CreateConversation();
        conversation.AppendUserInput(prompt);

        var response = await conversation.GetResponseFromChatbotAsync();

        return response;
    }
}