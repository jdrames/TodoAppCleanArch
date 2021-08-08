using Application.Common.DTOs.UserInput;
using Application.Common.Models;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Result> AuthenticateUser(LoginRequestDto loginRequest, string ipAddress);

        Task<ApplicationUser> GetUserAsync(string userId);

        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string username, string password);

        Task<Result> DeleteUserAsync(string userId);
    }
}
