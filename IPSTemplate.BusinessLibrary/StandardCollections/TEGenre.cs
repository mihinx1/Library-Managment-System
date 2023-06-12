namespace IPSTemplate.BusinessLibrary.StandardCollections;

public class TEGenre
{
    public const string Drama = "Drama";
    public const string Comedy = "Comedy";
    public const string Action = "Action";
    public const string Adventure = "Adventure";
    public const string CrimeAndMystery = "Crime and Mystery";
    public const string Fantasy = "Fantasy";
    public const string Historical = "Historical";
    public const string Other = "Other";
    public static List<string> All => new()
    {
        Drama,
        Comedy,
        Action,
        Adventure,
        CrimeAndMystery,
        Fantasy,
        Historical,
        Other
    };

    public static Dictionary<string, string> OrderedUserFriendlyNameMap => new Dictionary<string, string>
    {
        { Drama, GetUserFriendlyName(Drama) },
        { Comedy, GetUserFriendlyName(Comedy) },
        { Action , GetUserFriendlyName(Action) },
        { Adventure , GetUserFriendlyName(Adventure) },
        { CrimeAndMystery , GetUserFriendlyName(CrimeAndMystery) },
        { Fantasy , GetUserFriendlyName(Fantasy) },
        { Historical , GetUserFriendlyName(Historical) },
        { Other , GetUserFriendlyName(Other) }    }.OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

    public static string GetUserFriendlyName(string value) => value switch
    {
        Drama => "Drama",
        Comedy => "Comedy",
        Action => "Action",
        Adventure => "Adventure",
        CrimeAndMystery => "Crime and Mystery",
        Fantasy => "Fantasy",
        Historical => "Historical",
        Other => "Other",
        _ => ""
    };
}
