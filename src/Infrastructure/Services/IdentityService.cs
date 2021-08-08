using Application.Common.DTOs.UserInput;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        #region Private Properties
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IConfiguration Configuration;
        #endregion

        #region Constructor
        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            Configuration = configuration;
        }
        #endregion

        #region Public Methods
        public async Task<Result> AuthenticateUser(LoginRequestDto loginRequest, string ipAddress)
        {
            // TODO: Setup Token authentication
            throw new NotImplementedException();
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            var result = await _authorizationService.AuthorizeAsync(principal, policyName);
            return result.Succeeded;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = username
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user != null)
                return await DeleteUserAsync(user);

            return Result.Success();
        }

        public async Task<ApplicationUser> GetUserAsync(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return user;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return user?.UserName;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return await _userManager.IsInRoleAsync(user, role);
        }
        #endregion

        #region Private Methods
        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.ToApplicationResult();
        }
        #endregion

    }
}
