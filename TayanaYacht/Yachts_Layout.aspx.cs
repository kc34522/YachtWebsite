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
    public partial class Yachts_Layout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                int yachtId;
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]) && int.TryParse(Request.QueryString["id"], out yachtId))
                {
                    LoadCarousel(yachtId);
                    GetLayoutImage(yachtId);
                }
                else
                {
                    yachtId = GetFirstYachtId();
                    LoadCarousel(yachtId);
                    GetLayoutImage(yachtId);
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

        // 載入layout圖片
        private void GetLayoutImage(int yachtId)
        {
            string sql = @"SELECT   ImagePath
FROM     YachtImages
WHERE   (YachtID = @YachtID) AND (ImageCategory = N'Layout')";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", yachtId);
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterLayout.DataSource = reader;
                        RepeaterLayout.DataBind();
                    }
                }
            }
        }
    }
}