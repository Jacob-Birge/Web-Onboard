using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Web_Onboard.Data
{
    public class Functions
    {
        public static string strConn = Startup.Configuration.GetConnectionString("conn_str");
        public const string app_version = "1.0.0";


        public static DataTable GetDataTableFromSQL(string mqry)
        {
            SqlConnection con = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(mqry, con);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public static void ExecuteProcedure(string spName, List<SqlParameter> spParams, SqlConnection dbConn)
        {
            SqlCommand dbCmd = new SqlCommand();
            dbCmd.Connection = dbConn;
            dbCmd.CommandText = spName;
            dbCmd.CommandType = CommandType.StoredProcedure;
            dbCmd.CommandTimeout = 6000;
            SqlDataAdapter sqlDa = new SqlDataAdapter(dbCmd);
            try
            {
                foreach (SqlParameter param in spParams)
                {
                    dbCmd.Parameters.Add(param);
                }
                DataSet dsQuery = new DataSet();
                dbCmd.ExecuteNonQuery();
            }
            finally
            {
                dbCmd.Dispose();
            }
        }


        public static bool PagePermission(int role_id, string url)
        {
            if (role_id == 0)
            {
                return true;
            }

            DataTable dt = GetDataTableFromSQL("SELECT [admin_access], [user_access], [guest_access] FROM [pages] WHERE [url] = '" + url + "'");

            if (dt.Rows.Count > 0)
            {
                if (role_id == 1 && (bool)dt.Rows[0]["admin_access"] == true)
                {
                    return true;
                }
                else if (role_id == 2 && (bool)dt.Rows[0]["user_access"] == true)
                {
                    return true;
                }
                else if ((bool)dt.Rows[0]["guest_access"] == true)
                {
                    return true;
                }
            }

            return false;
        }

        public static int GetRoleID(int user_id)
        {
            DataTable dt = GetDataTableFromSQL("SELECT [role_id] FROM [users] WHERE [id] = " + user_id.ToString());

            if (dt.Rows.Count > 0)
            {
                return (int)dt.Rows[0]["role_id"];
            }
            else
            {
                return 3; // Guest
            }
        }

        public static string GetUserName(int user_id)
        {
            DataTable dt = GetDataTableFromSQL("SELECT [user_name] FROM [users] WHERE [id] = " + user_id.ToString());

            if (dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["user_name"];
            }
            else
            {
                return ""; // Guest
            }
        }

        public static string GetFirstName(int user_id)
        {
            DataTable dt = GetDataTableFromSQL("SELECT [first_name] FROM [users] WHERE [id] = " + user_id.ToString());

            if (dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["first_name"];
            }
            else
            {
                return ""; // Guest
            }
        }
        public static string GetLastName(int user_id)
        {
            DataTable dt = GetDataTableFromSQL("SELECT [last_name] FROM [users] WHERE [id] = " + user_id.ToString());

            if (dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["last_name"];
            }
            else
            {
                return ""; // Guest
            }
        }

        public static int GetCompanyID(int user_id)
        {
            DataTable dt = GetDataTableFromSQL("SELECT [company_id] FROM [users] WHERE [id] = " + user_id.ToString());

            if (dt.Rows.Count > 0)
            {
                return (int)dt.Rows[0]["company_id"];
            }
            else
            {
                return 0; // Guest
            }
        }
    }
}