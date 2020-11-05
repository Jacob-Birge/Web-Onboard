using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Onboard.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Web_Onboard.Pages
{
    public class CustomPageBuilderModel : PageModel
    {
        public CustomAuthenticationStateProvider _authStateProvider { get; set; }
        public int companyId { get; set; }
        public int roleId { get; set; }
        public string name { get; set; }
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
            curCompany = Int32.Parse(e.Value.ToString());
            ((CustomAuthenticationStateProvider)authenticationStateProvider).setCompanyId(curCompany);
            StateHasChanged();
        }
        private void CommonInit()
        {
            roleId = _authStateProvider.getRoleId();
            if (roleId == 0)
            {
                companyListItems = new List<SelectListItem>();
                companyListItems.Add(new SelectListItem
                {
                    Text = "",
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
