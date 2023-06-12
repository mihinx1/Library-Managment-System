namespace IPSTemplate.BusinessLibrary.StandardCollections;

public class TEEntityType
{
    public const string Type1 = "Type1";
    public const string Type2 = "Type2";
    public const string Type3 = "Type3";

    public static List<string> All => new()
    {
        Type1,
        Type2,
        Type3,
    };

    public static Dictionary<string, string> OrderedUserFriendlyNameMap => new Dictionary<string, string>
    {
        { Type1, GetUserFriendlyName(Type1) },
        { Type2, GetUserFriendlyName(Type2) },
        { Type3 , GetUserFriendlyName(Type3) },
    }.OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

    public static string GetUserFriendlyName(string value) => value switch
    {
        Type1 => "Type 1",
        Type2 => "Type 2",
        Type3 => "Type 3",
        _ => ""
    };
}
