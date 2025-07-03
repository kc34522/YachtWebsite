using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.UserControls
{
    public partial class MainContentTitle : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string menuType = Path.GetFileName(Request.Path).ToLower();

            LoadContentTitle(menuType);
        }

        public string ContentTitle { get; set; }


        private void LoadContentTitle(string menuType)
        {
            string id = Request.QueryString["Id"];
            switch (menuType)
            {
                case "yachts":
                    ContentTitle = "XXX";
                    //SqlConnection sqlConnection = new SqlConnection();
                    break;

                case "news":
                    ContentTitle = "News & Events";
                    break;

                case "company":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        if (id == "2")
                        {
                            ContentTitle = "Certificate";
                        }


                    }
                    else
                    {
                        ContentTitle = "About Us";
                    }

                    break;

                case "dealers":

                    int countryId;
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["Id"]) && int.TryParse(Request.QueryString["Id"], out countryId))
                    {
                        ContentTitle = GetCountryNameById(countryId);
                    }
                    else
                    {
                        countryId = GetFirstCountryId();
                        ContentTitle = GetCountryNameById(countryId);
                    }
                    break;

                case "contact":
                    ContentTitle = "Contact";
                    break;

                    //default:
            }

        }

        private string GetCountryNameById(int id)
        {
            string countryName;
            string sql = @"SELECT Name FROM Country WHERE Id = @Id";

            using(SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using(SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
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

    }
}