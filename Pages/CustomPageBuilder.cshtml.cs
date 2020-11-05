using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Onboard.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Web_Onboard.Pages
{
    public class CustomPageBuilderModel : PageModel
    {
        public CustomAuthenticationStateProvider _authStateProvider { get; set; }
        public int companyId { get; set; }
        public int roleId { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public List<SelectListItem> companyListItems { get; set; }

        public CustomPageBuilderModel(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        }

        public void OnGet()
        {
            companyId = _authStateProvider.getCompanyId();
            string first = _authStateProvider.getFirstName();
            string last = _authStateProvider.getLastName();
            if (string.IsNullOrWhiteSpace(first) && string.IsNullOrWhiteSpace(last))
            {
                name = "No Name";
            }
            else if (string.IsNullOrWhiteSpace(first) || string.IsNullOrWhiteSpace(last))
            {
                name = (string.IsNullOrWhiteSpace(first)) ? last : first;
            }
            else
            {
                name = first + " " + last;
            }
            CommonInit();
        }
        public void OnGetCompanyChange(string compId)
        {
            companyId = int.Parse(compId);
            CommonInit();
        }
        private void CommonInit()
        {
            message = "Welcome to the custom page builder!";
            roleId = _authStateProvider.getRoleId();
            if (roleId == 0)
            {
                companyListItems = new List<SelectListItem>();
                companyListItems.Add(new SelectListItem
                {
                    Selected = (companyId == -1),
                    Text = "Not Selected",
                    Value = "-1"
                });
                DataTable dt = Functions.GetDataTableFromSQL("SELECT [id], [name] FROM [companies]");
                foreach (DataRow dr in dt.Rows)
                {
                    companyListItems.Add(new SelectListItem
                    {
                        Text = (string)dr[1],
                        Value = ((int)dr[0]).ToString()
                    });
                }
                foreach(SelectListItem item in companyListItems)
                {
                    if (item.Value == companyId.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
    }
}
