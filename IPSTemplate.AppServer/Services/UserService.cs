using Core.Library.Extensions;
using Csla;
using IPSTemplate.BusinessLibrary.Interfaces;
using IPSTemplate.Dal.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IPSTemplate.AppServer.Services;

public class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly AppSettings _appSettings;
    private readonly ApplicationContext _appContext;
    private readonly UserManager<TEIdentityUser> _userManager;
    private readonly RoleManager<TEIdentityRole> _roleManager;

    public UserService(IOptions<AppSettings> appSettings, ILogger<UserService> logger, UserManager<TEIdentityUser> userManager, RoleManager<TEIdentityRole> roleManager, ApplicationContext appContext)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }

    public IList<TEIdentityUser> GetUsers()
    {
        return _userManager.Users.OrderBy(u => u.UserName).ToList();
    }

    public async Task<(IList<TEIdentityUser> Users, int TotalCount)> GetUsersAsync(UserFilter filter)
    {
        var filteredUsers = _userManager.Users;

        if (!string.IsNullOrEmpty(filter.Username))
        {
            filteredUsers = filteredUsers.Where(u => u.UserName.ToLower().Contains(filter.Username.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.Email))
        {
            filteredUsers = filteredUsers.Where(u => u.Email.ToLower().Contains(filter.Email.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.PhoneNumber))
        {
            filteredUsers = filteredUsers.Where(u => u.PhoneNumber.ToLower().Contains(filter.PhoneNumber.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.Name))
        {
            filteredUsers = filteredUsers.Where(u => u.Name.ToLower().Contains(filter.Name.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.Surname))
        {
            filteredUsers = filteredUsers.Where(u => u.Surname.ToLower().Contains(filter.Surname.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.Nationality))
        {
            filteredUsers = filteredUsers.Where(u => u.Nationality.ToLower().Contains(filter.Nationality.ToLower()));
        }
        if (!string.IsNullOrEmpty(filter.Role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(filter.Role);
            filteredUsers = filteredUsers.Where(u => usersInRole.Select(ur => ur.Id).Contains(u.Id));
        }
        if (filter.Active.HasValue)
        {
            filteredUsers = filteredUsers.Where(u => u.Active == filter.Active.Value);
        }
        if (filter.BirthDate.HasValue)
        {
            filteredUsers = filteredUsers.Where(u => u.BirthDate == filter.BirthDate.Value);
        }
        if (filter.Page.HasValue || filter.PageSize.HasValue)
        {
            int page = filter.Page ?? 1;
            int pageSize = filter.PageSize ?? 10;

            int skip = Math.Max((page - 1) * pageSize, 0);
            int take = Math.Max(pageSize, 0);

            filteredUsers = filteredUsers.Skip(skip).Take(take);
        }

        return (filteredUsers.OrderBy(u => u.UserName).ToList(), filteredUsers.Count());
    }

    public async Task<TEIdentityUser> GetUserByIdAsync(Guid id)
    {
        return await _userManager.FindByIdAsync(id.ToString());
    }

    public async Task<TEIdentityUser> GetUserByEmailAsync(string email)
    {
        return _userManager.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<Guid?> GetUsersSupplierRights(Guid userId)
    {
        Guid? supplierId = null;
        Guid supplierParse;
        string suppId = (_appContext.User as ClaimsPrincipal).Claims.FirstOrDefault(c => c.Type == "SupplierId")?.Value;
        if (!string.IsNullOrEmpty(suppId) && Guid.TryParse(suppId, out supplierParse))
            supplierId = supplierParse;

        return supplierId;
    }

    public async Task AddUserAsync(TEIdentityUser user)
    {
        try
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existingUser != null)
                throw new Exception($"New User could not be added because Id {user.Id} already exists!");

            await _userManager.CreateAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserService - Error creating user {UserId}", user.Id);
            throw;
        }
    }

    public async Task UpdateUserAsync(TEIdentityUser user)
    {
        try
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existingUser == null)
                throw new Exception($"User could not be updated because Id {user.Id} does not exists!");

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.EmailConfirmed = user.EmailConfirmed;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            existingUser.Name = user.Name;
            existingUser.Surname = user.Surname;
            existingUser.PasswordResetToken = null;
            existingUser.ResetTokenExpireUtc = null;
            existingUser.Active = user.Active;
            existingUser.Nationality = user.Nationality;
            existingUser.BirthDate = user.BirthDate;
            var status = await _userManager.UpdateAsync(existingUser);
            if (!status.Succeeded)
                throw new Exception("User could not be updated! " + string.Join(", ", status.Errors.Select(e => e.Description)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserService - Error updating user {UserId}", user.Id);
            throw;
        }
    }

    public async Task<List<string>> GetUsersRoles(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<List<string>> GetUsersRoles(TEIdentityUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task SaveUsersRoles(Guid userId, List<string> roles)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var uRoles = await _userManager.GetRolesAsync(user);

            var newRoles = roles.FindAll(r => !uRoles.Contains(r));
            var deletedRoles = uRoles.ToList().FindAll(r => !roles.Contains(r));

            if (newRoles.Any())
            {
                var status = await _userManager.AddToRolesAsync(user, newRoles);
                if (!status.Succeeded)
                    throw new Exception("Added Roles could not be saved! " + string.Join(", ", status.Errors.Select(e => e.Description)));
            }
            if (deletedRoles.Any())
            {
                var status = await _userManager.RemoveFromRolesAsync(user, deletedRoles);
                if (!status.Succeeded)
                    throw new Exception("Removed Roles could not be saved! " + string.Join(", ", status.Errors.Select(e => e.Description)));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserService - Error saving UserRoles for user {UserId}", userId);
            throw;
        }
    }

    public IList<string> GetAllRoles()
    {
        try
        {
            return _roleManager.Roles.Select(r => r.Name).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all user roles");
            throw;
        }
    }

    public async Task CreateNewRole(string roleName)
    {
        try
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                throw new Exception($"UserRole {roleName} could not be added because it already exists!");

            var newRole = new TEIdentityRole { Id = Guid.NewGuid(), Name = roleName };
            await _roleManager.CreateAsync(newRole);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new role {RoleName}", roleName);
            throw;
        }
    }

    public async Task SetUserPassword(Guid userId, string password)
    {
        //if (userId.ToString().Equals(_appSettings.NightjobUserId, StringComparison.InvariantCultureIgnoreCase))
        //{
        //    throw new Exception($"Nightjob user password can not be changed!");
        //}

        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception($"Could not set user password because Id {userId} does not exists!");
            if (!user.Active)
                throw new Exception($"Could not set user password because user was deactivated!");

            //resetting token if it was generated
            user.PasswordResetToken = null;
            user.ResetTokenExpireUtc = null;
            await _userManager.UpdateAsync(user);
            //resetting password
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, password);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserService - Error setting user password for {UserId}", userId);
            throw;
        }
    }

    public async Task ResetUserPassword(Guid userId, string newPassword)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new Exception($"Could not set user password because Id {userId} does not exists!");
            if (!user.Active)
                throw new Exception($"Could not set user password because user was deactivated!");

            user.PasswordResetToken = null;
            user.ResetTokenExpireUtc = null;

            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UserService - Error setting user password for {UserId}", userId);
            throw;
        }
    }

    public async Task<string> SetPasswordResetToken(Guid userId)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        if (existingUser == null)
            throw new Exception($"Password reset token could not be generated because Id {userId} does not exists!");

        existingUser.PasswordResetToken = Guid.NewGuid().ToString();
        existingUser.ResetTokenExpireUtc = DateTime.UtcNow.AddHours(1); //Default token expiration is 1 hour!

        await _userManager.UpdateAsync(existingUser);

        return existingUser.PasswordResetToken;
    }

    public async Task<TEIdentityUser> GetUserByResetToken(string token)
    {
        var existingUser = _userManager.Users.FirstOrDefault(u => u.PasswordResetToken == token);
        if (existingUser == null)
            return null;

        if ((existingUser.ResetTokenExpireUtc ?? DateTime.MinValue) < DateTime.UtcNow)
        {
            //reset token has expired!
            existingUser.ResetTokenExpireUtc = null;
            existingUser.PasswordResetToken = null;
            await _userManager.UpdateAsync(existingUser);

            return null; //if token has expired, we dont return the user!
        }

        return existingUser;
    }

    public async Task<bool> PasswordMatchesAsync(Guid userId, string password)
    {
        var existingUser = await _userManager.FindByIdAsync(userId.ToString());
        return existingUser is null ? false : await _userManager.CheckPasswordAsync(existingUser, password);
    }

    public async Task LogUserOut(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        await _userManager.UpdateSecurityStampAsync(user);
    }

    private ClaimsIdentity GetIdentity()
    {
        var identity = new ClaimsIdentity("CslaIdentity");
        //identity.AddClaim(new Claim(ClaimTypes.Sid, _appSettings.NightjobUserId));
        //identity.AddClaim(new Claim(ClaimTypes.Name, "Nightjob"));
        return identity;
    }

    private ClaimsPrincipal GetPrincipal()
    {
        return new ClaimsPrincipal(GetIdentity());
    }
}
