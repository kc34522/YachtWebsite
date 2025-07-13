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
    public partial class NewsDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!HasQueryString())
                {
                    Response.Redirect("NewsList.aspx");
                }
                else
                {
                    int newsId = Convert.ToInt32(Request.QueryString["Id"]);
                    GetContent(newsId);
                    GetDownload(newsId);
                    GetImages(newsId);
                }
            }
        }

        // 判斷是否有querystring
        private bool HasQueryString()
        {
            string result = Request.QueryString["Id"];
            if (string.IsNullOrWhiteSpace(result))
            {
                return false;
            }
            else
            {
                string sql = @"SELECT   Count(Id)
FROM     News
WHERE   (Id = @Id)";
                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@Id", result.Trim());
                        sqlConnection.Open();
                        int number = Convert.ToInt32(sqlCommand.ExecuteScalar());
                        if (number == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
        }

        // 載入news 資料
        private void GetContent(int newsId)
        {
            string sql = @"SELECT   Title, [Content]
FROM     News
WHERE   (Id = @NewsId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@NewsId", newsId);

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            LabelTitle.Text = reader["Title"].ToString();
                            LiteralContent.Text = reader["Content"].ToString();
                        }
                        else
                        {
                            Response.Redirect("NewsList.aspx");
                        }

                    }
                }
            }
        }

        // 載入news 圖片資料
        private void GetImages(int newsId)
        {
            string sql = @"SELECT   AltText, ImagePath
FROM     NewsImage
WHERE   (NewsID = @NewsId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@NewsId", newsId);

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterImages.DataSource = reader;
                        RepeaterImages.DataBind();
                    }
                }
            }
        }

        // 載入news 檔案下載資料
        private void GetDownload(int newsId)
        {
            string sql = @"SELECT   FilePath, DisplayName
FROM     NewsFile
WHERE   (NewsID = @NewsId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@NewsId", newsId);

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterDownloads.DataSource = reader;
                        RepeaterDownloads.DataBind();
                    }
                }
            }
        }


    }
}