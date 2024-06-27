using System.Text;
using VkTelegramReposter.Utils;

namespace VkTelegramReposter.Core.MessageFormatters;

public class NormalMessageFormatter : IMessageFormatter
{
    public Task<string> FormatAsync(ulong groupId, ulong postId, string newPostText, long ownerId, string[] images)
    {
        var stringBuilder = new StringBuilder();
        
        string vkPostLink = new VkLinkBuilder().WithGroupId(groupId)
            .WithPostId(postId)
            .Build();
        
        stringBuilder.Append(newPostText);
        
        if (images.Length <= 0)
            return Task.FromResult(stringBuilder.ToString());
        
        stringBuilder.Append("\n \n");
        stringBuilder.Append(string.Join("\n", images));
        stringBuilder.Append("\n \n");
        stringBuilder.Append(vkPostLink);

        return Task.FromResult(stringBuilder.ToString());
    }
}