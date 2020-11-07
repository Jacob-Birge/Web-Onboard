using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Onboard.Pages;
using System.Data;

namespace Web_Onboard.Data
{
    //class used for implementing and calling unit tests
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

        private void CompanyManagerTest()
        {
            CompanyManager companyManager = new CompanyManager();
            byte[] a = { 181, 235, 45 };
            string v = companyManager.ToByte(a);
            if (v == "??-")
            {
                outputs.Add("Success: CompanyManagerTest");
            }
            else
            {
                outputs.Add("Failed: CompanyManagerTest");
            }

        }
        private void UserManagerAddUserTest()
        {
            UserManager userManager = new UserManager();
            userManager.user.username = "TestUser";
            userManager.user.password = "TestUser";
            userManager.user.role_id = "1";

            int UserID = userManager.UserAddedComplete(true);
            DataTable testDt = Functions.GetDataTableFromSQL($"select id from users where id = {UserID}");
            if (testDt.Rows.Count > 0)
            {
                outputs.Add("Success: UserAddedComplete");
            }
            else
            {
                outputs.Add("Failed: UserAddedComplete");
            }
            userManager.editedName = "Test";
            userManager.UserEdited(UserID, true);
            testDt = Functions.GetDataTableFromSQL($"select [user_name] from users where id = {UserID}");
            if (testDt.Rows.Count > 0 && testDt.Rows[0][0].ToString() == "Test")
            {
                outputs.Add("Success: User Edit");
            }
            else
            {
                outputs.Add("Failed: User Edit");
            }
            Functions.GetDataTableFromSQL($"Delete from users where id = {UserID}");
        }
     
    }
}
