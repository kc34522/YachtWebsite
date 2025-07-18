using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht
{
    public partial class Yachts_Specification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int yachtId;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]) && int.TryParse(Request.QueryString["id"], out yachtId))
                {
                    LoadCarousel(yachtId);
                    GetSpecification(yachtId);
                }
                else
                {
                    yachtId = GetFirstYachtId();
                    LoadCarousel(yachtId);
                    GetSpecification(yachtId);
                }

            }
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

        // 載入輪播圖
        private void LoadCarousel(int yachtId)
        {
            string sql = @"SELECT   ImagePath
FROM     YachtImages
WHERE   (YachtID = @YachtID) AND (ImageCategory = N'Carousel')";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", yachtId);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterCarousel.DataSource = reader;
                        RepeaterCarousel.DataBind();
                    }
                }
            }
        }

        // 載入specification內容
        private void GetSpecification(int yachtId)
        {
            string sql = @"SELECT   SpecificationsContent
FROM     Yachts
WHERE   (YachtID = @YachtID)";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", yachtId);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LiteralSpecification.Text = reader["SpecificationsContent"].ToString();                        
                        }


                    }
                }
            }
        }
    }
}