using IPSTemplate.Dal.Models.Identity;

namespace IPSTemplate.BusinessLibrary.Interfaces
{
    public interface IUserService
    {
        IList<TEIdentityUser> GetUsers();
        Task<(IList<TEIdentityUser> Users, int TotalCount)> GetUsersAsync(UserFilter filter);
        Task<TEIdentityUser> GetUserByIdAsync(Guid id);
        Task<TEIdentityUser> GetUserByEmailAsync(string email);
        Task AddUserAsync(TEIdentityUser user);
        Task UpdateUserAsync(TEIdentityUser user);
        Task<bool> PasswordMatchesAsync(Guid userId, string password);
        Task SetUserPassword(Guid userId, string password);
        Task ResetUserPassword(Guid userId, string password);
        Task<string> SetPasswordResetToken(Guid userId);
        Task<TEIdentityUser> GetUserByResetToken(string token);
        Task<List<string>> GetUsersRoles(Guid userId);
        Task<List<string>> GetUsersRoles(TEIdentityUser user);
        Task SaveUsersRoles(Guid userId, List<string> roles);
        IList<string> GetAllRoles();
        Task CreateNewRole(string roleName);
        Task LogUserOut(Guid userId);
        //bool AuthenticateNightjobUser();
    }

    public record UserFilter(
        bool? Active,
        int? Page,
        int? PageSize,
        string? Username,
        string? Email,
        string? PhoneNumber,
        string? Name,
        string? Surname,
        string? Role,
        string? Nationality,
        DateTime? BirthDate
    );
}
