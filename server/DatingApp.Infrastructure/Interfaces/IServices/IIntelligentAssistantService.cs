namespace DatingApp.Infrastructure.Interfaces.IServices;

public interface IIntelligentAssistantService
{
    Task<string> GenerateIntroductionMessageAsync(string interests, string lookingFor);
}
