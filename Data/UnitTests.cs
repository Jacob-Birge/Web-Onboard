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
            CompanyManagerTest();
            UserManagerAddUserTest();
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

        private async void CompanyManagerTest()
        {
            CompanyManager companyManager = new CompanyManager();
            byte[] a = { 0,1,1,0,1,0,0,0 };
            string v = await companyManager.ToByte(a);
            if (v.Length > 0)
            {
                outputs.Add("Success: LoginValidateValidUser");
            }
            else
            {
                outputs.Add("Failed: LoginValidateValidUser");
            }

        }
        private void UserManagerAddUserTest()
        {
            UserManager userManager = new UserManager();
            userManager.user.username = "TestUser";
            userManager.user.firstname = "TestUser";
            userManager.user.lastname = "TestUser";
            userManager.user.email = "TestUser";
            userManager.user.password = "TestUser";
            userManager.user.role_id = "1";



            userManager.UserAddedComplete();
            if (userManager.errorMessage.Length > 1)
            {
                outputs.Add("Failed: UserAddedComplete");
            }
            else
            {
                outputs.Add("Success:UserAddedComplete");
            }
          
        }
        private void UserManagerUserNameExistTest()
        {
            UserManager userManager = new UserManager();
            userManager.user.username = "TestUser";
            UserManagerAddUserTest();
            if (userManager.usernameExists(userManager.user.username, -1))
            {
                outputs.Add("Failed: User Name Exists");

            }
            else
            {
                outputs.Add("Success: User Name Uniqe");
            }
            

        }
    }
}
