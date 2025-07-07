using CKFinder;
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
    public partial class Certificate : System.Web.UI.Page
    {
        //優化:把圖片拆開, 加入資料庫
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCertificateContent();
            }

            FileBrowser fileBrowser = new FileBrowser();
            fileBrowser.BasePath = "/ckfinder";
            // 重要：請確保 CKFinder 本身有做身份驗證！
            fileBrowser.SetupCKEditor(CKEditorControl1);

            ClearMessages();


            //Literal1.Text = HttpUtility.HtmlEncode(CKEditorControl1.Text);

        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LiteralMessage.Text = "";
            //LabelLastUpdatedInfo.Text = "";
        }

        // 待優化: 載入時間和人
        // 載入資料庫Certificate Content
        private void LoadCertificateContent()
        {
            string sql = @"SELECT   [Content], LastUpdated, LastUpdatedUser
                            FROM     CompanyContent
                            WHERE   (PageName = N'Certificate')";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read()) // 如果有讀到資料
                    {
                        CKEditorControl1.Text = sqlDataReader["Content"].ToString();
                        LabelLastUpdatedInfo.Text = $"最後更新時間為: {sqlDataReader["LastUpdated"]}由{sqlDataReader["LastUpdatedUser"]}更新";
                    }

                }
            }
        }

        // 優化: 補上上傳者資料
        // Ckeditor儲存按鈕
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string htmlContent = CKEditorControl1.Text;
            string sql = @"IF EXISTS (
    SELECT 1  
    FROM CompanyContent
    WHERE PageName = N'Certificate'
)
BEGIN
    UPDATE CompanyContent
    SET [Content] = @Content, LastUpdated = GETDATE()
    WHERE PageName = N'Certificate'
END
ELSE
BEGIN
    INSERT INTO CompanyContent (PageName, [Content], LastUpdated)
    VALUES (N'Certificate', @Content, GETDATE())
END
";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Content", htmlContent);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    LiteralMessage.Text = "儲存成功!";
                }
            }

            LoadCertificateContent();
        }
    }
}