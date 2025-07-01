using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht
{
    public partial class LeftMenu : System.Web.UI.UserControl
    {
        //public string MenuType { get; set; } // 由外部頁面設定
        public string TitleText { get; set; } // 左上標題


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string page = Path.GetFileName(Request.Path).ToLower();
                string menuType = GetMenuType(page);
                LoadMenu(menuType);
            }
          

        }

        private string GetMenuType(string page)
        {
            switch (page)
            {
                case "yachts": return "Yachts";
                case "news": return "News";
                case "company": return "Company";
                case "dealers": return "Dealers";
                case "contact": return "Contact";
                default: return ""; 
            }
        }
        private void LoadMenu(string menuType)
        {
            List<MenuItem> items = new List<MenuItem>();
            switch (menuType)
            {
                case "Yachts":
                    TitleText = "YACHTS";
                    SqlConnection sqlConnection = new SqlConnection();
                    break;

                case "News":
                    TitleText = "NEWS";
                    items.Add(new MenuItem { Url = "#", Text= "News & Events" });
                    break;

                case "Company":
                    TitleText = "COMPANY";
                    items.Add(new MenuItem { Url = "#", Text = "About Us" });
                    items.Add(new MenuItem { Url = "#", Text = "Certificate" });
                    break;

                case "Dealers":
                    TitleText = "DEALERS";
                    string sql = @"SELECT   Name
                                    FROM     Country
                                    WHERE   (IsForDealer = 1)";
                    using(SqlConnection sqlConnection1 = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                    {
                        using(SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection1))
                        {
                            sqlConnection1.Open();
                            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                            while (sqlDataReader.Read())
                            {
                                items.Add(new MenuItem { Url = "Dealers.aspx?Id="+ sqlDataReader["Id"].ToString(), Text = sqlDataReader["Name"].ToString() });
                            }
                        }
                    }
                    break;

                case "Contact":
                    TitleText = "CONTACT";
                    items.Add(new MenuItem { Url = "#", Text = "Contact" });
                    break;

                //default:
            }

            RepeaterLeftMenu.DataSource = items;
            RepeaterLeftMenu.DataBind();
        }

        private class MenuItem
        {
            public string Url { get; set; }
            public string Text { get; set; }

        }
    }
}