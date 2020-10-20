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
            int roleId = await _sessionStorageService.GetItemAsync<int>("role");
            Roles userRole;
            if (roleId > (int)Roles.BaseUser || roleId < (int)Roles.Owner)
            {
                userRole = Roles.BaseUser;
            }
            else
            {
                userRole = (Roles)roleId;
            }
            ClaimsIdentity identity;
            if (username != null)
            {
                identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Roles), userRole))
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(string username, int roleId = 2)
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
                new Claim(ClaimTypes.Role, Enum.GetName(typeof(Roles), userRole))
            }, "apiauth_type");
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _sessionStorageService.RemoveItemAsync("username");
            _sessionStorageService.RemoveItemAsync("role");

            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
