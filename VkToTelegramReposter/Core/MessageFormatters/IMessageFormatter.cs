namespace VkTelegramReposter.Core.MessageFormatters;

public interface IMessageFormatter
{
    public Task<string> FormatAsync(string newPostText, string[] images);
}