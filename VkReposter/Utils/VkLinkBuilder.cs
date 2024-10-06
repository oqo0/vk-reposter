namespace VkTelegramReposter.Utils;

public class VkLinkBuilder
{
    private const string VkUrl = "https://vk.com/";

    private string _groupType = "club";
    private ulong _groupId;
    private ulong _postId;

    public VkLinkBuilder WithGroupType(string groupType)
    {
        _groupType = groupType;
        return this;
    }
    
    public VkLinkBuilder WithGroupId(ulong groupId)
    {
        _groupId = groupId;
        return this;
    }
    
    public VkLinkBuilder WithPostId(ulong postId)
    {
        _postId = postId;
        return this;
    }
    
    public string Build()
    {
        if (_groupId == default || _postId == default)
            throw new InvalidDataException("Group id and post id values should be set");
            
        return VkUrl + _groupType + _groupId + "?w=wall-" + _groupId + "_" + _postId;
    }
}