using System.Text;
using Telegraph.Sharp;
using Telegraph.Sharp.Types;
using VkTelegramReposter.Configuration;
using VkTelegramReposter.Utils;

namespace VkTelegramReposter.Core.MessageFormatters;

public class TelegraphMessageFormatter(
    TelegraphClient telegraphClient,
    Config config) : IMessageFormatter
{
    public async Task<string> FormatAsync(ulong groupId, ulong postId, string newPostText, string[] images)
    {
        var pageContent = new List<Node>();
        var textLines = newPostText.Split('\n');
        var outerTags = new StringBuilder();
        
        foreach (var line in textLines)
        {
            if (line.StartsWith('#'))
                outerTags.Append(line + " ");
            
            pageContent.Add(Node.P(line));
        }

        pageContent.AddRange(images.Select(Node.Img));

        string vkPostLink = new VkLinkBuilder().WithGroupId(groupId)
            .WithPostId(postId)
            .Build();
        
        pageContent.Add(Node.A(vkPostLink, "Vk"));
        
        var page = await telegraphClient.CreatePageAsync(config.Telegraph.PageName, pageContent);

        return outerTags + "\n \n" + page.Url;
    }
}