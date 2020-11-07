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
            LoginValidateInvalidUser();
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
        private async void LoginValidateInvalidUser()
        {
            Login login = new Login();
            login.user.username = "noone";
            login.user.password = "m23123";
            bool loggedIn = await login.ValidateUser(true);
            if (!loggedIn)
            {
                outputs.Add("Success: LoginValidateInvalidUser");
            }
            else
            {
                outputs.Add("Failed: LoginValidateInvalidUser");
            }
        }

        private async void CompanyManagerTest()
        {
            CompanyManager companyManager = new CompanyManager();
            byte[] a = { 181, 235, 45 };
            string v = await companyManager.ToByte(a);
            if (v == "??-")
            {
                outputs.Add("Success: CompanyManagerTest" + v);
            }
            else
            {
                outputs.Add("Failed: CompanyManagerTest");
            }

        }
        private async void UserManagerAddUserTest()
        {
            UserManager userManager = new UserManager();
            userManager.user.username = "TestUser";
            userManager.user.firstname = "TestUser";
            userManager.user.lastname = "TestUser";
            userManager.user.email = "TestUser";
            userManager.user.password = "TestUser";
            userManager.user.role_id = "1";



            int UserID = await userManager.UserAddedComplete(true);
            if (userManager.errorMessage.Length > 1)
            {
                outputs.Add("Failed: UserAddedComplete");
            }
            else
            {
                outputs.Add("Success:UserAddedComplete");
            }
            userManager.editedName = "Test";
            userManager.editedRoleID = 1;
            userManager.editedEmail = "Test@test.com";
            userManager.UserEdited(UserID, true);
            if (userManager.errorMessage.Length > 1)
            {
                outputs.Add("Failed: User Edit");
            }
            else
            {
                outputs.Add("Success: User Edit");
            }
            Functions.GetDataTableFromSQL($"Delete from users where id = {UserID}");

        }
     
    }
}
