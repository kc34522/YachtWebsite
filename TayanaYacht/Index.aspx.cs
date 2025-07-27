using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCarousel();
                LoadNews();
            }

        }

        private void LoadCarousel()
        {
            string sql = @"SELECT   YachtImages.ImagePath, Yachts.IsActive, Yachts.ModelNameText, Yachts.ModelNameNumber, Yachts.IsNewBuilding, Yachts.IsNewDesign
FROM     Yachts INNER JOIN
              YachtImages ON Yachts.YachtID = YachtImages.YachtID
WHERE   (Yachts.IsActive = 1) AND (YachtImages.IsHomepageCarousel = 1)
order by Yachts.IsNewBuilding desc, Yachts.IsNewDesign desc, Yachts.ModelNameNumber asc";

            // 使用 DataTable，這是一個可以重複讀取的記憶體資料表
            DataTable dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    // 使用 SqlDataAdapter 來當作橋梁，把資料庫的資料填入 DataTable
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                }
            }

            // 現在 dt 裡面有完整的資料了，它可以被重複使用
            if (dataTable.Rows.Count > 0)
            {
                RepeaterBanner.DataSource = dataTable;
                RepeaterBanner.DataBind();

                RepeaterLittleBanner.DataSource = dataTable;
                RepeaterLittleBanner.DataBind();
            }
        }


        private void LoadNews()
        {
            string sql = @"WITH CoverImages AS (
    SELECT 
        News.Id AS NewsId,
        News.Title,
        News.PublishDate,
        News.IsTop,
        NewsImage.ImagePath,
        ROW_NUMBER() OVER (PARTITION BY News.Id ORDER BY NewsImage.IsCover DESC) AS rn
    FROM News
    LEFT JOIN NewsImage ON News.Id = NewsImage.NewsID
    WHERE News.PublishDate <= GETDATE()
)
SELECT TOP 3 *
FROM CoverImages
WHERE rn = 1
ORDER BY IsTop DESC, PublishDate DESC;
";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterNews.DataSource = reader;
                        RepeaterNews.DataBind();
                    }
                }
            }
        }

    }
}