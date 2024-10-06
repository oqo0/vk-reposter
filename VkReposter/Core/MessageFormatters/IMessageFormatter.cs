namespace VkReposter.Core.MessageFormatters;

public interface IMessageFormatter
{
    public Task<string> FormatAsync(ulong groupId, ulong postId, string newPostText, long ownerId, string[] images);
}