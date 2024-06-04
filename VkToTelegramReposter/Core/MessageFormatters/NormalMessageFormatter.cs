using System.Text;

namespace VkTelegramReposter.Core.MessageFormatters;

public class NormalMessageFormatter : IMessageFormatter
{
    public Task<string> FormatAsync(string newPostText, string[] images)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(newPostText);
        
        if (images.Length <= 0)
            return Task.FromResult(stringBuilder.ToString());
        
        stringBuilder.Append("\n \n");
        stringBuilder.Append(string.Join("\n", images));

        return Task.FromResult(stringBuilder.ToString());
    }
}