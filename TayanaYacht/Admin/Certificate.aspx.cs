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
            string sql = @"SELECT   C.[Content], C.LastUpdated, U.DisplayName
                   FROM     CompanyContent AS C
                   LEFT JOIN [User] AS U ON C.LastUpdatedUser = U.Id
                   WHERE    (C.PageName = N'Certificate')";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read()) // 如果有讀到資料
                    {
                        CKEditorControl1.Text = sqlDataReader["Content"].ToString();
                        // 讀取更新時間和更新者名稱
                        string lastUpdated = sqlDataReader["LastUpdated"].ToString();

                        // 檢查 DisplayName 是否為 DBNull (因為使用 LEFT JOIN)
                        string updatedUser = sqlDataReader["DisplayName"] == DBNull.Value
                                                ? "未知"
                                                : sqlDataReader["DisplayName"].ToString();

                        LabelLastUpdatedInfo.Text = $"最後更新時間為: {lastUpdated} 由 {updatedUser} 更新";
                    }

                }
            }
        }

        // 優化: 補上上傳者資料
        // Ckeditor儲存按鈕
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            int userId = (int)Session["AdminId"];
            string htmlContent = CKEditorControl1.Text;
            string sql = @"
IF EXISTS (
    SELECT 1
    FROM CompanyContent
    WHERE PageName = N'Certificate'
)
BEGIN
    UPDATE CompanyContent
    SET [Content] = @Content, LastUpdated = GETDATE(), LastUpdatedUser = @LastUpdatedUser
    WHERE PageName = N'Certificate'
END
ELSE
BEGIN
    INSERT INTO CompanyContent (PageName, [Content], LastUpdated, LastUpdatedUser)
    VALUES (N'Certificate', @Content, GETDATE(), @LastUpdatedUser)
END
";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Content", htmlContent);
                    sqlCommand.Parameters.AddWithValue("@LastUpdatedUser", userId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    // --- 修改後的訊息提示 ---
                    // 1. 讓包著訊息的整個 div 顯示出來
                    MessageContainer.Visible = true;
                    // 2. 設定訊息內容
                    LiteralMessage.Text = "資料已成功儲存！";
                }
            }

            LoadCertificateContent();
        }
    }
}