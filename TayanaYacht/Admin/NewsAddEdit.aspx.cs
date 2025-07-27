using CKEditor.NET;
using CKFinder;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using static System.Net.WebRequestMethods;

namespace TayanaYacht.Admin
{
    public partial class NewsAddEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasQueryString())
                {
                    LabelContentTitle.Text = "編輯新聞";
                    LoadEditContent();
                    LoadImages();
                    LoadFiles();
                }
                else
                {
                    txtTitle.Attributes.Add("required", "required");
                    LabelContentTitle.Text = "新增新聞";
                    txtPublishDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }

            FileBrowser fileBrowser = new FileBrowser();
            fileBrowser.BasePath = "/ckfinder";
            // 重要：請確保 CKFinder 本身有做身份驗證！
            fileBrowser.SetupCKEditor(CKEditorControlContent);

            ClearMessages();
        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelSaveMessage.Visible = false;
            LabelImageMessage.Visible = false;
            LabelFileMessage.Visible = false;
            LabelFileGridView.Visible = false;
            LabelImageGridView.Visible = false;           
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

        // 載入EditMode Content
        private void LoadEditContent()
        {
            string sql = @"SELECT 
                       News.*,
                       U1.DisplayName AS CreatedUserName, -- 建議用 DisplayName 較佳
                       U2.DisplayName AS ModifiedUserName
                   FROM News
                   LEFT JOIN [User] AS U1 ON News.CreatedBy = U1.Id
                   LEFT JOIN [User] AS U2 ON News.ModifiedBy = U2.Id
                   WHERE News.Id = @Id";

            string id = Request.QueryString["Id"].Trim();

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlConnection.Open();


                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.Read())
                        {
                            txtTitle.Text = sqlDataReader["Title"].ToString();
                            CheckBoxIsTopEdit.Checked = (Boolean)sqlDataReader["IsTop"];
                            CheckBoxIsVisibleEdit.Checked = (Boolean)sqlDataReader["IsVisible"];
                            txtPublishDate.Text = Convert.ToDateTime(sqlDataReader["PublishDate"]).ToString("yyyy-MM-dd");
                            CKEditorControlContent.Text = sqlDataReader["Content"].ToString();
                            LabelCreatedDateAdd.Text = Convert.ToDateTime(sqlDataReader["CreatedDate"]).ToString();
                            LabelCreatedByAdd.Text = sqlDataReader["CreatedUserName"].ToString();
                            if (sqlDataReader["ModifiedDate"] != DBNull.Value)
                            {
                                LabelModifiedDateAdd.Text = Convert.ToDateTime(sqlDataReader["ModifiedDate"]).ToString();
                            }
                            if (sqlDataReader["ModifiedUserName"] != DBNull.Value)
                            {
                                LabelModifiedByAdd.Text = sqlDataReader["ModifiedUserName"].ToString();
                            }

                        }
                    }
                }
            }
        }

        // SQL @ModifiedBy @CreatedBy 要改
        // 儲存按鈕-新聞內文
        protected void btnSave_Click(object sender, EventArgs e)
        {
            // 判斷標題是否為空白
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                LabelSaveMessage.Text = "標題不得為空白!";
                LabelSaveMessage.ForeColor = System.Drawing.Color.Red;
                LabelSaveMessage.Visible = true;
                return;
            }

            if (HasQueryString() == true)
            {
                string sqlUpdate = @"UPDATE News 
                               SET Title = @Title, [Content] = @Content, IsTop = @IsTop, IsVisible = @IsVisible, 
                                   PublishDate = @PublishDate, ModifiedDate = GETDATE(), ModifiedBy = @ModifiedBy
                             WHERE (Id = @Id)";
                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand updateCommand = new SqlCommand(sqlUpdate, sqlConnection))
                    {
                        updateCommand.Parameters.AddWithValue("@Title", txtTitle.Text);
                        updateCommand.Parameters.AddWithValue("@Content", CKEditorControlContent.Text);
                        updateCommand.Parameters.AddWithValue("@IsTop", CheckBoxIsTopEdit.Checked);
                        updateCommand.Parameters.AddWithValue("@IsVisible", CheckBoxIsVisibleEdit.Checked);
                        updateCommand.Parameters.AddWithValue("@PublishDate", txtPublishDate.Text);
                        
                        updateCommand.Parameters.AddWithValue("@ModifiedBy", (int)Session["AdminId"]);
                        updateCommand.Parameters.AddWithValue("@Id", Request.QueryString["Id"]);

                        sqlConnection.Open();
                        updateCommand.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                string sqlInsert = @"INSERT INTO News 
                               (Title, [Content], IsTop, IsVisible, PublishDate, CreatedBy, ModifiedBy) 
                             VALUES 
                               (@Title, @Content, @IsTop, @IsVisible, @PublishDate, @CreatedBy, @ModifiedBy);
                             SELECT SCOPE_IDENTITY()"; // 加這行取得新 Id

                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand insertCommand = new SqlCommand(sqlInsert, sqlConnection))
                    {
                        insertCommand.Parameters.AddWithValue("@Title", txtTitle.Text);
                        insertCommand.Parameters.AddWithValue("@Content", CKEditorControlContent.Text);
                        insertCommand.Parameters.AddWithValue("@IsTop", CheckBoxIsTopEdit.Checked);
                        insertCommand.Parameters.AddWithValue("@IsVisible", CheckBoxIsVisibleEdit.Checked);
                        insertCommand.Parameters.AddWithValue("@PublishDate", txtPublishDate.Text);
                        // 要改
                        insertCommand.Parameters.AddWithValue("@CreatedBy", (int)Session["AdminId"]);
                        insertCommand.Parameters.AddWithValue("@ModifiedBy", (int)Session["AdminId"]); // 新增時，兩者可設為相同
                        sqlConnection.Open();

                        // 抓回新插入的 Id (會回傳 SQL 查詢的第一列第一欄，剛好就是我們要的 Id)
                        int newId = Convert.ToInt32(insertCommand.ExecuteScalar());
                        Response.Redirect($"NewsAddEdit.aspx?Id={newId}");
                    }
                }
            }
            LabelSaveMessage.Text = "儲存成功!";
            LabelSaveMessage.Visible = true;
            LoadEditContent();
        }

        //  定義圖片CLASS類別 (多圖上傳用)
        private class ImageInfo
        {
            public HttpPostedFile ImageFile { get; set; }
            public string OriginalName { get; set; }
            public string StoredName { get; set; }
            public string ImagePath { get; set; }

        }

        // 圖片上傳按鈕
        protected void ButtonAddImage_Click(object sender, EventArgs e)
        {
            if (!FileUploadImage.HasFiles)
            {
                LabelImageMessage.Text = "請選擇至少一件圖片檔案!";
                LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                LabelImageMessage.Visible = true;
                return;
            }
            else
            {
                var imageInfos = new List<ImageInfo>();

                foreach (var file in FileUploadImage.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (fileExtension != ".jpeg" && fileExtension != ".jpg")
                    {
                        LabelImageMessage.Text = "請選擇.jpg/.jpeg圖片檔案!";
                        LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                        LabelImageMessage.Visible = true;
                        return;
                    }

                    var fileMemory = file.ContentLength;
                    if (fileMemory > 1000000)
                    {
                        LabelImageMessage.Text = "您選擇的圖片超過1MB,不能上傳!";
                        LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                        LabelImageMessage.Visible = true;
                        return;
                    }

                    var originalName = Path.GetFileName(file.FileName);
                    var storedName = Guid.NewGuid().ToString() + fileExtension;
                    var imagePath = "/Upload/News/Images/" + storedName;


                    imageInfos.Add(new ImageInfo
                    {
                        ImageFile = file,
                        OriginalName = originalName,
                        StoredName = storedName,
                        ImagePath = imagePath
                    });

                }

                int id = Convert.ToInt32(Request.QueryString["Id"]);

                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlTransaction tran = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            string folderPath = Server.MapPath("~/Upload/News/Images/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            string checkCoverSql = @"SELECT COUNT(*) FROM NewsImage WHERE NewsID = @Id AND IsCover = 1";
                            bool hasCover = false;
                            using (SqlCommand checkCoverCommand = new SqlCommand(checkCoverSql, sqlConnection, tran))
                            {
                                checkCoverCommand.Parameters.AddWithValue("@Id", id);
                                hasCover = Convert.ToInt32(checkCoverCommand.ExecuteScalar()) > 0;
                            }

                            // 儲存圖片檔案
                            foreach (var image in imageInfos)
                            {
                                image.ImageFile.SaveAs(folderPath + image.StoredName);


                                // 儲存到資料庫
                                string sql = @"INSERT INTO NewsImage
              (NewsID, OriginalImageName, StoredFileName, ImagePath, IsCover)
VALUES  (@Id, @OriginalName, @StoredName, @ImagePath, @IsCover)";
                                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection, tran))
                                {
                                    sqlCommand.Parameters.AddWithValue("@Id", id);
                                    sqlCommand.Parameters.AddWithValue("@OriginalName", image.OriginalName);
                                    sqlCommand.Parameters.AddWithValue("@StoredName", image.StoredName);
                                    sqlCommand.Parameters.AddWithValue("@ImagePath", image.ImagePath);
                                    sqlCommand.Parameters.AddWithValue("@IsCover", !hasCover ? 1 : 0);

                                    sqlCommand.ExecuteNonQuery();
                                }
                                hasCover = true;
                            }

                            tran.Commit();
                            LabelImageMessage.Text = "圖片上傳成功!";
                            LabelImageMessage.ForeColor = System.Drawing.Color.Blue;
                            LabelImageMessage.Visible = true;
                            LoadImages();
                        }
                        catch
                        {
                            tran.Rollback();
                            foreach (var image in imageInfos)
                            {
                                string fullPath = Server.MapPath("~/Upload/News/Images/" + image.StoredName);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            LabelImageMessage.Text = "圖片上傳失敗，請稍後再試!";
                            LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                            LabelImageMessage.Visible = true;
                        }
                    }
                }
            }


        }

        // 載入圖片GridView
        private void LoadImages()
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            string sql = @"SELECT   NewsImage.*
FROM     NewsImage
WHERE   (NewsID = @id)";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@id", id);

                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    GridViewImage.DataSource = sqlDataReader;
                    GridViewImage.DataBind();
                }
            }
        }

        // 圖片GridView: 刪除按鈕
        protected void GridViewImage_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int imageId = Convert.ToInt32(GridViewImage.DataKeys[rowIndex].Value);
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction tran = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string checkImageCountSql = @"SELECT COUNT(*) FROM NewsImage WHERE NewsID = @Id";
                        int imageCount;
                        using (SqlCommand checkImageCountCommand = new SqlCommand(checkImageCountSql, sqlConnection, tran))
                        {
                            checkImageCountCommand.Parameters.AddWithValue("@Id", id);
                            imageCount = Convert.ToInt32(checkImageCountCommand.ExecuteScalar());
                        }

                        if (imageCount <= 1)
                        {
                            tran.Rollback();
                            LabelImageMessage.Text = "至少需要保留一張圖片作為封面！";
                            LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                            LabelImageMessage.Visible = true;
                            return;
                        }

                        string checkCoverSql = @"SELECT IsCover FROM NewsImage WHERE Id = @ImageId";
                        bool isCover = false;
                        using (SqlCommand checkCoverCommand = new SqlCommand(checkCoverSql, sqlConnection, tran))
                        {
                            checkCoverCommand.Parameters.AddWithValue("@ImageId", imageId);
                            isCover = Convert.ToBoolean(checkCoverCommand.ExecuteScalar());
                        }

                        string deleteSql = @"DELETE FROM NewsImage WHERE Id = @ImageId";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteSql, sqlConnection, tran))
                        {
                            deleteCommand.Parameters.AddWithValue("@ImageId", imageId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        if (isCover)
                        {
                            string selectNewCoverSql = @"SELECT TOP 1 Id FROM NewsImage WHERE NewsID = @Id";
                            using (SqlCommand selectNewCoverCommand = new SqlCommand(selectNewCoverSql, sqlConnection, tran))
                            {
                                selectNewCoverCommand.Parameters.AddWithValue("@Id", id);
                                object newCoverId = selectNewCoverCommand.ExecuteScalar();
                                if (newCoverId != null)
                                {
                                    string updateNewCoverSql = @"UPDATE NewsImage SET IsCover = 1 WHERE Id = @NewCoverId";
                                    using (SqlCommand updateNewCoverCommand = new SqlCommand(updateNewCoverSql, sqlConnection, tran))
                                    {
                                        updateNewCoverCommand.Parameters.AddWithValue("@NewCoverId", newCoverId);
                                        updateNewCoverCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        tran.Commit();
                        LabelImageMessage.Text = "刪除成功!";
                        LabelImageMessage.ForeColor = System.Drawing.Color.Blue;
                        LabelImageMessage.Visible = true;
                        LoadImages();
                    }
                    catch
                    {
                        tran.Rollback();
                        LabelImageMessage.Text = "刪除失敗!";
                        LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                        LabelImageMessage.Visible = true;
                    }
                }
            }
        }

        private class FileInfo
        {
            public HttpPostedFile AttachFile { get; set; }
            public string OriginalName { get; set; }
            public string StoredName { get; set; }
            public string FilePath { get; set; }
            public string DisplayName { get; set; }

        }

        //檔案上傳按鈕
        protected void ButtonAddFile_Click(object sender, EventArgs e)
        {
            if (!FileUploadFile.HasFiles)
            {
                LabelFileMessage.Text = "請選擇至少一件檔案!";
                LabelFileMessage.ForeColor = System.Drawing.Color.Red;
                LabelFileMessage.Visible = true;
                return;
            }
            else
            {
                var fileInfos = new List<FileInfo>();

                foreach (var file in FileUploadFile.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (fileExtension != ".pdf")
                    {
                        LabelFileMessage.Text = "請選擇.pdf檔案!";
                        LabelFileMessage.ForeColor = System.Drawing.Color.Red;
                        LabelFileMessage.Visible = true;
                        return;
                    }

                    var fileMemory = file.ContentLength;
                    if (fileMemory > 5 * 1024 * 1024)
                    {
                        LabelFileMessage.Text = "您選擇的檔案超過5MB,不能上傳!";
                        LabelFileMessage.ForeColor = System.Drawing.Color.Red;
                        LabelFileMessage.Visible = true;
                        return;
                    }

                    var originalName = Path.GetFileName(file.FileName);
                    var storedName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = "/Upload/News/Files/" + storedName;


                    fileInfos.Add(new FileInfo
                    {
                        AttachFile = file,
                        OriginalName = originalName,
                        StoredName = storedName,
                        FilePath = filePath,
                        DisplayName = originalName
                    });

                }

                int id = Convert.ToInt32(Request.QueryString["Id"]);

                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    sqlConnection.Open();
                    using (SqlTransaction tran = sqlConnection.BeginTransaction())
                    {
                        try
                        {
                            string folderPath = Server.MapPath("~/Upload/News/Files/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            // 儲存圖片檔案
                            foreach (var file in fileInfos)
                            {
                                file.AttachFile.SaveAs(folderPath + file.StoredName);


                                // 儲存到資料庫
                                string sql = @"INSERT INTO NewsFile
              (NewsID, OriginalFileName, StoredFileName, FilePath, DisplayName)
VALUES  (@Id, @OriginalName, @StoredName, @FilePath, @DisplayName)";
                                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection, tran))
                                {
                                    sqlCommand.Parameters.AddWithValue("@Id", id);
                                    sqlCommand.Parameters.AddWithValue("@OriginalName", file.OriginalName);
                                    sqlCommand.Parameters.AddWithValue("@StoredName", file.StoredName);
                                    sqlCommand.Parameters.AddWithValue("@FilePath", file.FilePath);
                                    sqlCommand.Parameters.AddWithValue("@DisplayName", file.DisplayName);

                                    sqlCommand.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                            LabelFileMessage.Text = "檔案上傳成功!";
                            LabelFileMessage.ForeColor = System.Drawing.Color.Blue;
                            LabelFileMessage.Visible = true;
                        }
                        catch (Exception ex)
                        {
                            if (tran?.Connection != null)
                            {
                                tran.Rollback(); // 確保交易還活著再 Rollback
                            }
                            foreach (var file in fileInfos)
                            {
                                string fullPath = Server.MapPath("~/Upload/News/Files/" + file.StoredName);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            LabelFileMessage.Text = "檔案上傳失敗!" + ex.Message;
                            LabelFileMessage.ForeColor = System.Drawing.Color.Red;
                            LabelFileMessage.Visible = true;
                        }
                        LoadFiles();
                    }
                }
            }
        }

        // 載入檔案GridView
        private void LoadFiles()
        {
            int id = Convert.ToInt32(Request.QueryString["Id"]);
            string sql = @"SELECT   NewsFile.*
FROM     NewsFile
WHERE   (NewsID = @id)";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@id", id);

                sqlConnection.Open();
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    GridViewFile.DataSource = sqlDataReader;
                    GridViewFile.DataBind();
                }
            }
        }

        // 檔案GridView: 編輯按鈕
        protected void GridViewFile_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            GridViewFile.EditIndex = rowIndex;
            LoadFiles();
        }

        // 檔案GridView: 取消按鈕
        protected void GridViewFile_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewFile.EditIndex = -1;
            LoadFiles();
        }

        // 檔案GridView: 更新按鈕
        protected void GridViewFile_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow gridViewRow = GridViewFile.Rows[rowIndex];
            TextBox updateTextBox = gridViewRow.FindControl("TextBoxName") as TextBox;

            if (string.IsNullOrWhiteSpace(updateTextBox.Text))
            {
                LabelFileGridView.Text = "顯示前台檔案名稱不得為空白!";
                LabelFileGridView.ForeColor = System.Drawing.Color.Red;
                LabelFileGridView.Visible = true;
                return;
            }


                string nameEdit = updateTextBox.Text.Trim();
            // 在存進 OriginalFileName 前，先確認檔名是否已經有副檔名，避免重複。
            if (!nameEdit.ToLower().EndsWith(".pdf"))
            {
                nameEdit += ".pdf";
            }

            int fileId = Convert.ToInt32(GridViewFile.DataKeys[rowIndex].Value);
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();


                string updateSql = @"UPDATE  NewsFile
SET        DisplayName = @DisplayName
WHERE   (Id = @FileId)";

                using (SqlCommand updateCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    updateCommand.Parameters.AddWithValue("@DisplayName", nameEdit);
                    updateCommand.Parameters.AddWithValue("@FileId", fileId);

                    updateCommand.ExecuteNonQuery();
                }


                GridViewFile.EditIndex = -1;
                LoadFiles();

                LabelFileGridView.Text = "更新成功!";
                LabelFileGridView.ForeColor = System.Drawing.Color.Blue;
                LabelFileGridView.Visible = true;

            }

        }
        // 檔案GridView: 刪除按鈕
        protected void GridViewFile_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int fileId = Convert.ToInt32(GridViewFile.DataKeys[rowIndex].Value);

            string sql = @"Delete From NewsFile
WHERE   (Id = @FileId)";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@FileId", fileId);

                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            LoadFiles();
            LabelFileGridView.Text = "刪除成功!";
            LabelFileGridView.ForeColor = System.Drawing.Color.Blue;
            LabelFileGridView.Visible = true;
        }

        protected void RadioButtonIsCover_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)rb.NamingContainer;
            int imageId = Convert.ToInt32(GridViewImage.DataKeys[row.RowIndex].Value);
            int id = Convert.ToInt32(Request.QueryString["Id"]);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction tran = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string clearCoverSql = @"UPDATE NewsImage SET IsCover = 0 WHERE NewsID = @Id";
                        using (SqlCommand clearCoverCommand = new SqlCommand(clearCoverSql, sqlConnection, tran))
                        {
                            clearCoverCommand.Parameters.AddWithValue("@Id", id);
                            clearCoverCommand.ExecuteNonQuery();
                        }

                        string updateCoverSql = @"UPDATE NewsImage SET IsCover = 1 WHERE Id = @ImageId";
                        using (SqlCommand updateCoverCommand = new SqlCommand(updateCoverSql, sqlConnection, tran))
                        {
                            updateCoverCommand.Parameters.AddWithValue("@ImageId", imageId);
                            updateCoverCommand.ExecuteNonQuery();
                        }

                        tran.Commit();
                        LabelImageMessage.Text = "封面選擇成功!";
                        LabelImageMessage.ForeColor = System.Drawing.Color.Blue;
                        LabelImageMessage.Visible = true;
                        LoadImages();
                    }
                    catch
                    {
                        tran.Rollback();
                        LabelImageMessage.Text = "封面選擇失敗!";
                        LabelImageMessage.ForeColor = System.Drawing.Color.Red;
                        LabelImageMessage.Visible = true;
                    }
                }
            }
        }
    }
}

