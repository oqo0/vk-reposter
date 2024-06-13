namespace VkTelegramReposter.Core.MessageFormatters;

public interface IMessageFormatter
{
    public Task<string> FormatAsync(ulong groupId, ulong postId, string newPostText, string[] images);
}