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
        private static string _username = "";
        private static int _userId = -1;
        private static int _roleId = -1;
        private static int _companyId = -1;

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
            _username = username;
            int? userId = await _sessionStorageService.GetItemAsync<int>("userId");
            if (userId.HasValue)
                _userId = userId.Value;
            else
                _userId = -1;
            int? roleId = await _sessionStorageService.GetItemAsync<int>("roleId");
            if (roleId.HasValue)
                _roleId = roleId.Value;
            else
                _roleId = -1;
            int? companyId = await _sessionStorageService.GetItemAsync<int>("companyId");
            if (companyId.HasValue)
                _companyId = companyId.Value;
            else
                _companyId = -1;

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

        public void MarkUserAsAuthenticated()
        {
            MarkUserAsAuthenticated(_username, _userId, _roleId, _companyId);
        }

        public async void MarkUserAsAuthenticated(string username, int userId, int roleId, int companyId)
        {
            _username = username;
            _userId = userId;
            _roleId = roleId;
            _companyId = companyId;

            await _sessionStorageService.SetItemAsync("username", _username);
            await _sessionStorageService.SetItemAsync("userId", _userId);
            await _sessionStorageService.SetItemAsync("roleId", _roleId);
            await _sessionStorageService.SetItemAsync("companyId", _companyId);

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

        public async void HoldForReset(string username, int userId, int roleId, int companyId)
        {
            _username = username;
            await _sessionStorageService.SetItemAsync("username", username);
            _userId = userId;
            await _sessionStorageService.SetItemAsync("userId", userId);
            _roleId = roleId;
            await _sessionStorageService.SetItemAsync("roleId", roleId);
            _companyId = companyId;
            await _sessionStorageService.SetItemAsync("companyId", companyId);
        }

        public void MarkUserAsLoggedOut()
        {
            _sessionStorageService.RemoveItemAsync("username");
            _username = "";
            _sessionStorageService.RemoveItemAsync("userId");
            _userId = -1;
            _sessionStorageService.RemoveItemAsync("roleId");
            _roleId = -1;
            _sessionStorageService.RemoveItemAsync("companyId");
            _companyId = -1;

            ClaimsIdentity identity = new ClaimsIdentity();
            ClaimsPrincipal user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
        public string getUsername()
        {
            return _username;
        }
        public async void setUsername(string username)
        {
            _username = username;
            await _sessionStorageService.SetItemAsync("username", username);
        }
        public int getUserId()
        {
            return _userId;
        }
        public async void setUserId(int userId)
        {
            _userId = userId;
            await _sessionStorageService.SetItemAsync("userId", userId);
        }
        public int getRoleId()
        {
            return _roleId;
        }
        public async void setRoleId(int roleId)
        {
            _roleId = roleId;
            await _sessionStorageService.SetItemAsync("roleId", roleId);
        }
        public int getCompanyId()
        {
            return _companyId;
        }
        public async void setCompanyId(int companyId)
        {
            _companyId = companyId;
            await _sessionStorageService.SetItemAsync("companyId", companyId);
        }
    }
}
