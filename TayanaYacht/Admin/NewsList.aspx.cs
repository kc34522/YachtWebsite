using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.Admin
{
    public partial class NewsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadNewsList();
            }

            ClearMessages();
        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelGridViewMessage.Visible = false;

        }

        // 載入News清單
        private void LoadNewsList()
        {
            string sql = @"SELECT   Id, Title, [Content], IsTop, IsVisible, PublishDate
FROM     News";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            GridViewNews.DataSource = sqlDataReader;
                            GridViewNews.DataBind();
                        }
                    }
                }
            }
        }

        // 刪除按鈕
        protected void GridViewNews_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int indexRow = e.RowIndex;
            int newsId = Convert.ToInt32(GridViewNews.DataKeys[indexRow].Value);

            string sql = @"DELETE FROM News
                            WHERE   (Id = @NewsId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@NewsId", newsId);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery(); // 優化: 判斷比數
                    
                }
            }

            LoadNewsList();

            LabelGridViewMessage.Text = "刪除成功!";
            LabelGridViewMessage.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewMessage.Visible = true;

        }
    }
}