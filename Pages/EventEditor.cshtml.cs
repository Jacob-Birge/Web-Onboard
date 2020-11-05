using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Onboard.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Web;
using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Web_Onboard.Pages
{
    public class EventEditorModel : PageModel
    {
        public CustomAuthenticationStateProvider _authStateProvider { get; set; }
        public int companyId { get; set; }
        public int roleId { get; set; }
        public string name { get; set; }
        public List<SelectListItem> companyListItems { get; set; }
        public List<SelectListItem> Events { get; set; }
        public List<SelectListItem> AllEvents { get; set; }
        public List<SelectListItem> Colors { get; set; }

        public EventEditorModel(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = (CustomAuthenticationStateProvider)authStateProvider;
        }

        private int curCompany = -1;
        private int curRole = -1;

        public string js = "";

        private DataTable dt;
        private DataTable companies_dt;
        private DataTable colors_dt;
        private DataTable events;
        private DataTable allevents;

        public void OnGet()
        {
            int? tempComp = _authStateProvider.getCompanyId();
            if (tempComp.HasValue)
            {
                curCompany = tempComp.Value;
            }
            int? tempRole = _authStateProvider.getRoleId();
            if (tempRole.HasValue)
            {
                curRole = tempRole.Value;
            }
            if (curRole == 0)
            {
                companies_dt = Functions.GetDataTableFromSQL("SELECT [id], [name] FROM [companies]");
                dt = Functions.GetDataTableFromSQL("SELECT u.[id], u.[user_name], u.[role_id], u.[company_id], u.[email], c.[name] FROM [users] u left join [companies] c on u.company_id=c.id");

            }
            else
            {
                dt = Functions.GetDataTableFromSQL("SELECT [id], [user_name], [role_id], [company_id], [email] FROM [users] WHERE [company_id]=" + curCompany.ToString());
            }

            colors_dt = Functions.GetDataTableFromSQL("SELECT [background_color], [id] FROM [item_color_schemes] ORDER BY [id]");

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
            bind_lbData("", -1, false);
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
                foreach (SelectListItem item in companyListItems)
                {
                    if (item.Value == companyId.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

                Colors = new List<SelectListItem>();
                foreach (DataRow dr in colors_dt.Rows)
                {
                    Colors.Add(new SelectListItem
                    {
                        Text = dr[0].ToString(),
                        Value = dr[1].ToString()
                    });
                }
            }
        }
        private void CompanyChange(string compId)
        {

        }
        public async Task OnPostButton()
        {
            
        }

        private void bind_lbData(string ss, int v, bool to_top)
        {
            DataTable dt = Functions.GetDataTableFromSQL("SELECT [id], [title] FROM [items] WHERE [company_id] = " + companyId.ToString() + " ORDER BY [title]");

            AllEvents = new List<SelectListItem>();
            Events = new List<SelectListItem>();

            foreach (DataRow dri in dt.Rows)
            {
                AllEvents.Add(new SelectListItem
                {
                    Text = (string)dri[1],
                    Value = ((int)dri[0]).ToString()
                });
            }

            SqlConnection conn = new SqlConnection(Functions.strConn);
            SqlCommand cmd = new SqlCommand();
            js = "var lbColors = []; ";

            cmd.CommandText = "sp_SearchItems";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("a", ss);
            cmd.Parameters.AddWithValue("company_id", companyId.ToString());

            conn.Open();
            cmd.Connection = conn;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 0;

            adapter.Fill(dt);

            foreach (DataRow dri in dt.Rows)
            {
                Events.Add(new SelectListItem
                {
                    Text = (string)dri[1],
                    Value = ((int)dri[0]).ToString()
                });
            }

            if (v >= 0)
            {
                foreach (SelectListItem item in Events)
                {
                    if (item.Value == v.ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }

            if (to_top)
            {
                SelectListItem sitem = new SelectListItem();

                int index = 0;
                foreach (SelectListItem item in Events)
                {
                    if (item.Selected == true)
                    {
                        sitem = item;
                        break;
                    }

                    index++;
                }

                DataRow drn = dt.NewRow();

                drn.ItemArray = dt.Rows[index].ItemArray;

                dt.Rows.RemoveAt(index);
                dt.Rows.InsertAt(drn, 0);

                Events.Remove(sitem);
                Events.Insert(0, sitem);

                Events[0].Selected = true;
            }

            foreach(DataRow r in colors_dt.Rows)
            {
                //js += "lbColors.push('" + r["background_color"] + "');";
            }
        }

    }
}
