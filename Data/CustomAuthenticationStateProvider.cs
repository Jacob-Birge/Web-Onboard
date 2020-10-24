using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.SessionStorage;

namespace Web_Onboard.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private enum Roles
        {
            Owner,
            Manager,
            BaseUser
        }

        private ISessionStorageService _sessionStorageService;

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
            _sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string username = await _sessionStorageService.GetItemAsync<string>("username");
            int? userId = await _sessionStorageService.GetItemAsync<int>("userId");
            int? roleId = await _sessionStorageService.GetItemAsync<int>("roleId");
            int? companyId = await _sessionStorageService.GetItemAsync<int>("companyId");

            Roles userRole;
            if (roleId.HasValue && (roleId > (int)Roles.BaseUser || roleId < (int)Roles.Owner))
            {
                userRole = Roles.BaseUser;
            }
            else
            {
                userRole = (Roles)roleId;
            }

            if (!companyId.HasValue)
            {
                companyId = -1;
            }

            ClaimsIdentity identity;
            if (username != null && userId.HasValue)
            {
                identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Roles), userRole)),
                new Claim("CompanyId", companyId.ToString())
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(string username, int userId, int roleId, int companyId)
        {
            Roles userRole;
            if (roleId > (int)Roles.BaseUser || roleId < (int)Roles.Owner)
            {
                userRole = Roles.BaseUser;
            }
            else
            {
                userRole = (Roles)roleId;
            }

            ClaimsIdentity identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("UserId", userId.ToString()),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Roles), userRole)),
                new Claim("CompanyId", companyId.ToString())
            }, "apiauth_type");
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _sessionStorageService.RemoveItemAsync("username");
            _sessionStorageService.RemoveItemAsync("userId");
            _sessionStorageService.RemoveItemAsync("roleId");
            _sessionStorageService.RemoveItemAsync("companyId");

            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
