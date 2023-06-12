namespace IPSTemplate.BusinessLibrary.StandardCollections;

public class TEGender
{
    public const string Male = "M";
    public const string Female = "F";
    public const string Other = "Other";
    public static List<string> All => new()
    {
        Male,
        Female,
        Other
    };

    public static Dictionary<string, string> OrderedUserFriendlyNameMap => new Dictionary<string, string>
    {
        { Male, GetUserFriendlyName(Male) },
        { Female , GetUserFriendlyName(Female) },
        { Other , GetUserFriendlyName(Other) }    }.OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

    public static string GetUserFriendlyName(string value) => value switch
    {
        Male => "M",
        Female => "F",
        Other => "Other",
        _ => ""
    };
}
