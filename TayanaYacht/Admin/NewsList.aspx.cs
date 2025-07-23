using System;
using System.Collections.Generic;
using System.Data;
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
            LabelSearchResult.Visible = false;

        }

        // 載入News清單
        private void LoadNewsList(string keyword="")
        {
            int currentPageIndex = GridViewNews.PageIndex;

            string sql = @"SELECT   Id, Title, [Content], IsTop, IsVisible, PublishDate
FROM     News
WHERE 1=1";

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += @"AND
(Title like @kw or
[content] like @kw or
PublishDate like @kw)";
            }

            sql += "Order By IsTop Desc, PublishDate Desc;";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        sqlCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }
                    sqlConnection.Open();

                    using(SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        GridViewNews.DataSource = dataTable;
                        GridViewNews.PageIndex = currentPageIndex;
                        GridViewNews.DataBind();
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
                    int rowsAffected = sqlCommand.ExecuteNonQuery(); // 優化: 判斷比數
                    if(rowsAffected > 0)
                    {
                        LabelGridViewMessage.Text = "已成功刪除1筆新聞!";
                        LabelGridViewMessage.ForeColor = System.Drawing.Color.Blue;
                        LabelGridViewMessage.Visible = true;
                    }
                    else
                    {
                        LabelGridViewMessage.Text = "刪除失敗，請稍後再試。";
                        LabelGridViewMessage.ForeColor = System.Drawing.Color.Red;
                        LabelGridViewMessage.Visible = true;
                    }
                }
            }
            // 如果有搜尋條件，就帶入關鍵字
            String keyword = ViewState["SearchKeyword"] as string;
            LoadNewsList(keyword);// ⚠️ 移出 using 區塊，確保連線已釋放後再執行
        }

        // 搜尋按鈕
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string keyword = TextBoxSearch.Text.Trim();
            ViewState["SearchKeyword"] = keyword; // 存起來讓分頁也能用

            GridViewNews.PageIndex = 0; // 回到第一頁
            LoadNewsList(keyword);

            if (!string.IsNullOrEmpty(keyword))
            {
                LabelSearchResult.Text = $"搜尋結果：包含「{keyword}」的結果如下：";
                LabelSearchResult.Visible = true;
            }
        }

        // 清除搜尋按鈕
        protected void ButtonClearSearch_Click(object sender, EventArgs e)
        {
            TextBoxSearch.Text = "";
            ViewState["SearchKeyword"] = null;
            LabelSearchResult.Visible = false;

            GridViewNews.PageIndex = 0; // 回到第一頁
            LoadNewsList(); // 載入全部
        }

        // GridView換頁
        protected void GridViewNews_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewNews.PageIndex = e.NewPageIndex;

            // 如果有搜尋條件，就帶入關鍵字
            String keyword = ViewState["SearchKeyword"] as string;
            LoadNewsList(keyword);
        }

        //GridView產生序號
        protected void GridViewNews_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = e.Row.RowIndex;
                int pageIndex = GridViewNews.PageIndex;
                int pageSize = GridViewNews.PageSize;
                int rowNumber = (pageIndex * pageSize) + rowIndex + 1;

                Label lblRowNumber = (Label)e.Row.FindControl("LabelRowNumber");
                if (lblRowNumber != null)
                {
                    lblRowNumber.Text = rowNumber.ToString();
                }
            }
        }
    }
}