using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
                case "yachts_overview": return "Yachts1";
                case "yachts_layout": return "Yachts2";
                case "yachts_specification": return "Yachts3";
                case "newslist": return "News";
                case "newsdetail": return "News";
                case "company": return "Company";
                case "dealers": return "Dealers";
                case "contact": return "Contact";
                default: return "";
            }
        }


        private void LoadMenu(string menuType)
        {
            string currentPage = Path.GetFileName(Request.Path); // 自動抓目前頁面

            List<MenuItem> items = new List<MenuItem>();
            switch (menuType)
            {
                case "Yachts1":
                    TitleText = "YACHTS";
                    string yachtSql = @"SELECT   ModelName, YachtID, IsNewBuilding, IsNewDesign

                                    FROM     Yachts
                                    WHERE   (IsActive = 1)
                                    ORDER BY ModelName";
                    using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(yachtSql, sqlConnection))
                        {
                            sqlConnection.Open();
                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                while (sqlDataReader.Read())
                                {
                                    items.Add(new MenuItem { Url = $"Yachts_OverView.aspx?id={sqlDataReader["YachtID"]}", Text = sqlDataReader["ModelName"].ToString() + (Convert.ToBoolean(sqlDataReader["IsNewBuilding"])?" (New Building)":"") + (Convert.ToBoolean(sqlDataReader["IsNewDesign"]) ? " (New Design)" : "")});
                                }
                            }
                        }
                    }
                    break;

                case "Yachts2":
                    TitleText = "YACHTS";
                    string yachtSql2 = @"SELECT   ModelName, YachtID, IsNewBuilding, IsNewDesign

                                    FROM     Yachts
                                    WHERE   (IsActive = 1)
                                    ORDER BY ModelName";
                    using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(yachtSql2, sqlConnection))
                        {
                            sqlConnection.Open();
                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                while (sqlDataReader.Read())
                                {
                                    items.Add(new MenuItem { Url = $"Yachts_Layout.aspx?id={sqlDataReader["YachtID"]}", Text = sqlDataReader["ModelName"].ToString() + (Convert.ToBoolean(sqlDataReader["IsNewBuilding"]) ? " (New Building)" : "") + (Convert.ToBoolean(sqlDataReader["IsNewDesign"]) ? " (New Design)" : "") });
                                }
                            }
                        }
                    }
                    break;
                case "Yachts3":
                    TitleText = "YACHTS";
                    string yachtSql3 = @"SELECT   ModelName, YachtID, IsNewBuilding, IsNewDesign

                                    FROM     Yachts
                                    WHERE   (IsActive = 1)
                                    ORDER BY ModelName";
                    using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(yachtSql3, sqlConnection))
                        {
                            sqlConnection.Open();
                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                while (sqlDataReader.Read())
                                {
                                    items.Add(new MenuItem { Url = $"Yachts_Layout.aspx?id={sqlDataReader["YachtID"]}", Text = sqlDataReader["ModelName"].ToString() + (Convert.ToBoolean(sqlDataReader["IsNewBuilding"]) ? " (New Building)" : "") + (Convert.ToBoolean(sqlDataReader["IsNewDesign"]) ? " (New Design)" : "") });
                                }
                            }
                        }
                    }
                    break;

                case "News":
                    TitleText = "NEWS";
                    items.Add(new MenuItem { Url = "NewsList.aspx", Text = "News & Events" });
                    break;

                case "Company":
                    TitleText = "COMPANY";
                    items.Add(new MenuItem { Url = "Company.aspx", Text = "About Us" });
                    items.Add(new MenuItem { Url = "Company.aspx?Id=2", Text = "Certificate" });
                    break;

                case "Dealers":
                    TitleText = "DEALERS";
                    string sql = @"SELECT Distinct Country.Id, Country.Name
                            FROM     Country 
                            INNER JOIN Region ON Country.Id = Region.CountryId 
                            INNER JOIN Dealer ON Dealer.RegionId = Region.Id
                            WHERE   (Dealer.IsActive = 1)
                            ORDER BY Country.Name";
                    using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                    {
                        using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                        {
                            sqlConnection.Open();
                            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                while (sqlDataReader.Read())
                                {
                                    items.Add(new MenuItem { Url = $"Dealers.aspx?Id={sqlDataReader["Id"]}", Text = sqlDataReader["Name"].ToString() });
                                }
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