using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.UserControls
{
    public partial class RightMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string page = Path.GetFileName(Request.Path).ToLower();

            // 如果你 不想保留 .aspx 副檔名，可以這樣寫：
            //string page = Path.GetFileNameWithoutExtension(Request.Path).ToLower(); 

            string menuType = GetMenuType(page);
            LoadRightMenu(menuType);
        }

        private string GetMenuType(string page)
        {
            switch (page)
            {
                case "yachts_overview": return "Yachts";
                case "yachts_layout": return "Yachts";
                case "yachts_specification": return "Yachts";
                case "newslist": return "News";
                case "newsdetail": return "News";
                case "company": return "Company";
                case "dealers": return "Dealers";
                case "contact": return "Contact";
                default: return "";
            }
        }

        private int totalItemCount; 



        private void LoadRightMenu(string menuType)
        {
            string id = Request.QueryString["Id"];

            List<MenuItem> items = new List<MenuItem>();
            items.Add(new MenuItem { Url = "Index.aspx", Text = "Home" });


            switch (menuType)
            {
                case "Yachts":
                    items.Add(new MenuItem { Url = "#", Text = "Yachts" });
                    // TODO: 從資料庫取得 Model 名稱

                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        items.Add(new MenuItem { Url = "#", Text = GetYachtName(Convert.ToInt32(id)) });
                    }
                    else
                    {
                        items.Add(new MenuItem { Url = "#", Text = GetYachtName(GetFirstYachtId()) });
                    }

                    break;

                case "News":
                    items.Add(new MenuItem { Url = "NewsList.aspx", Text = "News" });                   
                    items.Add(new MenuItem { Url = "#", Text = "News & Events" });
                    break;

                case "Company":
                    items.Add(new MenuItem { Url = "Company.aspx", Text = "Company" });

                    // 數值待確
                    if (id == "2")
                    {
                        items.Add(new MenuItem { Url = "Company.aspx?Id=2", Text = "Certificate" });
                    }
                    else
                    {
                        items.Add(new MenuItem { Url = "Company.aspx", Text = "About Us" });
                    }
                    break;

                case "Dealers":

                    items.Add(new MenuItem { Url = "Dealers.aspx", Text = "Dealers" });

                    // TODO: 從資料庫取得經銷商名稱
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        //SqlConnection sqlConnection = new SqlConnection();
                        items.Add(new MenuItem { Url = $"Dealers.aspx?Id={id}", Text = GetCountryNameById(Convert.ToInt32(id)) });
                    }
                    else
                    {
                        //SqlConnection sqlConnection = new SqlConnection();
                        items.Add(new MenuItem { Url = $"Dealers.aspx?Id={GetFirstCountryId()}", Text = GetCountryNameById(GetFirstCountryId()) });
                    }

                    break;

                case "Contact":
                    items.Add(new MenuItem { Url = "#", Text = "Contact" });
                    break;
            }
            RepeaterRightMenu.DataSource = items; // Step 1：指定資料來源（只是準備階段）
            totalItemCount = items.Count; // Step 2：這時資料筆數已知，可以記錄下來 ✅
            RepeaterRightMenu.DataBind(); // Step 3：開始資料綁定（真正觸發 ItemDataBound 的時候）


        }

        private class MenuItem
        {
            public string Url { get; set; }
            public string Text { get; set; }
        }

        // 麵包削
        protected void RepeaterRightMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            // 表示 一般資料列，在偶數或奇數位置都會出現
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var menuItem = (MenuItem)e.Item.DataItem;
                var ltlMenuItem = (Literal)e.Item.FindControl("ltlMenuItem");
                var ltlArrow = (Literal)e.Item.FindControl("ltlArrow");

                ltlMenuItem.Text = $"<a href='{menuItem.Url}'>{menuItem.Text}</a>";

                // 如果不是最後一筆，顯示 >>
                if (e.Item.ItemIndex < totalItemCount - 1)
                {
                    ltlArrow.Text = " &gt;&gt; ";
                }
            }
        }

        private string GetCountryNameById(int id)
        {
            string countryName;
            string sql = @"SELECT Name FROM Country WHERE Id = @Id";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlConnection.Open();
                    countryName = sqlCommand.ExecuteScalar().ToString();
                }
            }
            return countryName;
        }

        // 抓左側選單第一個國家Id
        private int GetFirstCountryId()
        {
            int countryId;
            string sql = @"SELECT   Top 1 Country.Id
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
                    countryId = (int)sqlCommand.ExecuteScalar();
                }
            }
            return countryId;
        }

        // 抓左側選單第一個Yacht Id
        private int GetFirstYachtId()
        {
            int yachtId;
            string sql = @"SELECT Top 1  YachtID
FROM     Yachts
WHERE   (IsActive = 1)
ORDER BY ModelName";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    yachtId = (int)sqlCommand.ExecuteScalar();
                }
            }
            return yachtId;
        }

        // 抓YACHT NAME
        private string GetYachtName(int yachtId)
        {
            string yachtName;
            string sql = @"SELECT   ModelName
FROM     Yachts
WHERE   (YachtID = @YachtID)";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", yachtId);
                    sqlConnection.Open();
                    yachtName = sqlCommand.ExecuteScalar().ToString();
                }
            }
            return yachtName;
        }
    }
}