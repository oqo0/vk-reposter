namespace VkReposter.Vk;

public class LongPollUrl(string serverUrl, string key, ulong timeStamp)
{
    public ulong TimeStamp { get; set; } = timeStamp;

    public string Form()
    {
        return $"{serverUrl}?act=a_check&key={key}&ts={TimeStamp}&wait=25";
    }
}