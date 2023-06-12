namespace IPSTemplate.BusinessLibrary.StandardCollections
{
    public class TEUserRole
    {
        public const string Administrator = "Administrator";
        public const string Member = "Member";

        public static List<string> All => new List<string>()
        {
            Administrator,
            Member,
        };

        public static string GetUserFriendlyName(string value) => value switch
        {
            Administrator => "",
            Member => "",
            _ => ""
        };

        public static Dictionary<string, string> GetUserFriendlyNameMap => new Dictionary<string, string>
        {
            { Administrator, GetUserFriendlyName(Administrator) },
            { Member, GetUserFriendlyName(Member) },
        }.OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);

        public static Dictionary<string, string> SupplierRolesDictionary => new Dictionary<string, string>
        {
            { Administrator, GetUserFriendlyName(Administrator) },
            { Member, GetUserFriendlyName(Member) },
        }.OrderBy(c => c.Value).ToDictionary(c => c.Key, c => c.Value);
    }
}
