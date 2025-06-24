using MediatR;
using Microsoft.Extensions.Configuration;
using OpenAI_API;

public class GenerateMessageHandler(IConfiguration configuration) : IRequestHandler<GenerateMessageCommand, string>
{
    public async Task<string> Handle(GenerateMessageCommand request, CancellationToken cancellationToken)
    {
        var authentication = new APIAuthentication(configuration["OpenAI:ApiKey"]);
        var api = new OpenAIAPI(authentication);

        var prompt = $"Generate a short and friendly introduction message for dating. " +
                     $"Interests: {request.Interests}. Looking for: {request.LookingFor}.";

        var conversation = api.Chat.CreateConversation();
        conversation.AppendUserInput(prompt);

        var response = await conversation.GetResponseFromChatbotAsync();

        return response;
    }
}