using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht
{
    public partial class NewsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 從 QueryString 取得當前頁數，預設為第 1 頁
                int currentPage = 1;
                if (!string.IsNullOrEmpty(Request.QueryString["page"]))
                {
                    int.TryParse(Request.QueryString["page"], out currentPage);
                }

                // 載入指定頁數的新聞列表
                LoadNewsList(currentPage);
            }
        }

        // 載入經銷商列表(Repeater)+頁數篩選
        private void LoadNewsList(int currentPage)
        {
            //舊sql語法
            //            string sql = @"SELECT 
            //    News.Id, News.Title, News.[Content], News.IsTop, News.IsVisible, 
            //    News.PublishDate, News.CreatedDate, News.CreatedBy, 
            //    News.ModifiedDate, News.ModifiedBy, 
            //    NewsImage.ImagePath, NewsImage.AltText
            //FROM News
            //LEFT JOIN NewsImage 
            //    ON News.Id = NewsImage.NewsID AND NewsImage.IsCover = 1
            //WHERE News.IsVisible = 1 AND (News.PublishDate <= GETDATE())
            //ORDER BY News.IsTop DESC, News.PublishDate DESC";


            // 設定每頁要顯示的資料筆數
            int pageSize = 5;

            // 取得新聞總筆數
            int totalItems = GetTotalNewsCount();

            // 計算起訖行號
            int startRow = (currentPage - 1) * pageSize + 1;
            int endRow = currentPage * pageSize;

            // 新的 SQL 語法 (使用 CTE)
            string sql = @"
WITH NewsCTE as (
SELECT 
    News.Id, News.Title, News.[Content], News.IsTop, News.IsVisible, 
    News.PublishDate, News.CreatedDate, News.CreatedBy, 
    News.ModifiedDate, News.ModifiedBy, 
    NewsImage.ImagePath, NewsImage.AltText,
ROW_NUMBER() OVER (ORDER BY News.IsTop DESC, News.PublishDate DESC) AS RowNumber
FROM News
LEFT JOIN NewsImage 
    ON News.Id = NewsImage.NewsID AND NewsImage.IsCover = 1
WHERE News.IsVisible = 1 AND (News.PublishDate <= GETDATE())
)
SELECT * FROM NewsCTE
WHERE RowNumber >= @startRow AND RowNumber <= @endRow";


            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {

                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@startRow", startRow);
                    sqlCommand.Parameters.AddWithValue("@endRow", endRow);

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterNews.DataSource = reader;
                        RepeaterNews.DataBind();
                    }
                }
            }
            // 產生分頁控制項
            ltlPager.Text = GeneratePager(totalItems, pageSize, currentPage);
        }

        // 取得新聞總筆數的方法
        private int GetTotalNewsCount()
        {
            // 取得總資料筆數的 SQL
            string countSql = @"Select COUNT(Id) FROM News WHERE News.IsVisible = 1 AND (News.PublishDate <= GETDATE())";

            int totalItems = 0;

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(countSql, sqlConnection))
                {
                    sqlConnection.Open();

                    totalItems = Convert.ToInt32(sqlCommand.ExecuteScalar());
                }
            }
            return totalItems;
        }

        //產生分頁 HTML 的方法
        private string GeneratePager(int totalitems, int pageSize, int currentPage)
        {
            // 計算總頁數
            int totalPages = (int)Math.Ceiling((double)totalitems / pageSize);

            // 使用 StringBuilder 來組合 HTML 字串，效能較好
            StringBuilder pagerHtml = new StringBuilder();

            if (totalPages > 1)
            {
                pagerHtml.Append("<ul class='pagination'>");

                // 上一頁按鈕
                if (currentPage > 1)
                {
                    pagerHtml.Append($"<li><a href='NewsList.aspx?page={currentPage - 1}'>&laquo; Prev</a></li>");
                }

                // 頁碼
                for (int i = 1; i <= totalPages; i++)
                {
                    if (i == currentPage)
                    {
                        pagerHtml.Append($"<li class='active'><a href='NewsList.aspx?page={i}'>{i}</a></li>");
                    }
                    else
                    {
                        pagerHtml.Append($"<li><a href='NewsList.aspx?page={i}'>{i}</a></li>");
                    }
                }

                // 下一頁按鈕
                if (currentPage < totalPages)
                {
                    pagerHtml.Append($"<li><a href='NewsList.aspx?page={currentPage + 1}'>Next &raquo;</a></li>");

                }

                pagerHtml.Append("</ul>");

            }

            return pagerHtml.ToString();

        }
    }
}