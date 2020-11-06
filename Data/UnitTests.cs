using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Onboard.Pages;

namespace Web_Onboard.Data
{
    public class UnitTests
    {
        public List<string> outputs = new List<string>();
        public UnitTests()
        {
            LoginValidateValidUser();
        }

        private async void LoginValidateValidUser()
        {
            Login login = new Login();
            login.user.username = "andy";
            login.user.password = "maddog";
            bool loggedIn = await login.ValidateUser(true);
            if (loggedIn)
            {
                outputs.Add("Success: LoginValidateValidUser");
            }
            else
            {
                outputs.Add("Failed: LoginValidateValidUser");
            }
        }
    }
}
