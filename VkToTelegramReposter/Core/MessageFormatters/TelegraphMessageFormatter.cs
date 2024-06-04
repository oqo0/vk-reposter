using System.Text;
using Telegraph.Sharp;
using Telegraph.Sharp.Types;

namespace VkTelegramReposter.Core.MessageFormatters;

public class TelegraphMessageFormatter(TelegraphClient telegraphClient) : IMessageFormatter
{
    public async Task<string> FormatAsync(string newPostText, string[] images)
    {
        var pageContent = new List<Node>();
        var textLines = newPostText.Split('\n');
        var outerTags = new StringBuilder();

        foreach (var line in textLines)
        {
            if (line.StartsWith('#'))
                outerTags.Append(line);
            
            pageContent.Add(Node.P(line));
        }

        pageContent.AddRange(images.Select(Node.Img));

        var page = await telegraphClient.CreatePageAsync("Анкета", pageContent);

        return outerTags + "\n \n" + page.Url;
    }
}