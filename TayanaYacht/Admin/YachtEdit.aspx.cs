using CKFinder;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.Admin
{
    public partial class YachtEdit : System.Web.UI.Page
    {
        // 方法一：定義一個私有屬性，它的生命週期與整個頁面請求相同。
        private int YachtId { get; set; } = 0;

        // 在頁面載入時，第一件事就是設定這個頁面到底是在處理哪個 ID
        // 使用 TryParse 更安全，如果網址沒有id或id非數字，idFromUrl 會是 0
        protected void Page_Load(object sender, EventArgs e)
        {

            int.TryParse(Request.QueryString["Id"], out int idFromUrl);
            this.YachtId = idFromUrl; // 將從網址取得的id，存入私有屬性中

            if (!IsPostBack)
            {
                if (YachtId > 0)
                {
                    LoadData();
                }

            }
            FileBrowser fileBrowser = new FileBrowser();
            fileBrowser.BasePath = "/ckfinder";
            // 重要：請確保 CKFinder 本身有做身份驗證！
            fileBrowser.SetupCKEditor(CKEditorOverviewContent);
            fileBrowser.SetupCKEditor(CKEditorSpecification);


            ClearMessages();

        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelModelText.Visible = false;
            LabelModelNumber.Visible = false;
            LabelInfoMessage.Visible = false;
            LabelDimUploadMessage.Visible = false;
            LabelDownloadMessage.Visible = false;
            LabelLayoutMessage.Visible = false;
            LabelCarouselMessage.Visible = false;
            LabelGridViewDimMessage.Visible = false;
            LabelDimImageMessage.Visible = false;
            LabelDownloadGridViewMessage.Visible = false;
            LabelGridViewLayout.Visible = false;
            LabelGridViewCarousel.Visible = false;
        }


        private void LoadData()
        {
            string sql = @"SELECT   Yachts.*
FROM     Yachts
WHERE   (YachtID = @yachtId)";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {


                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@yachtId", YachtId);
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtModelText.Text = reader["ModelNameText"].ToString();
                            txtModelNumber.Text = reader["ModelNameNumber"].ToString();
                            CheckBoxIsActive.Checked = Convert.ToBoolean(reader["IsActive"]);
                            chkIsNewBuilding.Checked = Convert.ToBoolean(reader["IsNewBuilding"]);
                            chkIsNewDesign.Checked = Convert.ToBoolean(reader["IsNewDesign"]);
                            CKEditorOverviewContent.Text = reader["OverviewContent"].ToString();
                            CKEditorSpecification.Text = reader["SpecificationsContent"].ToString();
                        }

                    }
                }
                //載入Dimension GridView (無資料時改用DataTable)
                string sqlGridViewDimension = @"SELECT   SpecID, SpecLabel, SpecValue
FROM     YachtSpecs
WHERE   (YachtID = @yachtId)";
                using (SqlCommand sqlCommandGridViewDimension = new SqlCommand(sqlGridViewDimension, sqlConnection))
                {
                    sqlCommandGridViewDimension.Parameters.AddWithValue("@yachtId", YachtId);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommandGridViewDimension))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // 檢查從資料庫撈回來的資料是否為空
                        if (dataTable.Rows.Count == 0)
                        {
                            // 如果沒有資料，就新增一筆空的 DataRow 到 DataTable 中
                            DataRow dataRow = dataTable.NewRow();
                            dataTable.Rows.Add(dataRow);

                            // 將 GridView 的資料來源設為這個「包含一筆空資料」的 DataTable
                            GridViewDimension.DataSource = dataTable;
                            GridViewDimension.DataBind();

                            // 取得 GridView 的第一列 (也就是我們剛剛新增的空資料列) 並將其隱藏
                            // 我們將這一列的狀態設為不會顯示的狀態，這樣使用者就看不到空白列了
                            GridViewDimension.Rows[0].Visible = false;
                            GridViewDimension.Rows[0].Controls.Clear();

                        }
                        else
                        {
                            GridViewDimension.DataSource = dataTable;
                            GridViewDimension.DataBind();
                        }                           


                    }
                }


                //載入dimension圖片
                string sqlDimensionImage = @"SELECT   OverviewImageURL
FROM     Yachts
WHERE   (YachtID = @yachtId)";
                using (SqlCommand sqlCommandDimImage = new SqlCommand(sqlDimensionImage, sqlConnection))
                {
                    sqlCommandDimImage.Parameters.AddWithValue("@yachtId", YachtId);

                    using (SqlDataReader reader = sqlCommandDimImage.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string imageUrl = reader["OverviewImageURL"].ToString();
                            if (!string.IsNullOrWhiteSpace(imageUrl))
                            {
                                PanelViewDimensionImage.Visible = true;
                                PanelUploadDimensionImage.Visible = false;
                                ImageDimensionImage.ImageUrl = reader["OverviewImageURL"].ToString();
                            }
                            else
                            {
                                PanelViewDimensionImage.Visible = false;
                                PanelUploadDimensionImage.Visible = true;
                            }

                        }
                        else
                        {

                            PanelViewDimensionImage.Visible = false;
                            PanelUploadDimensionImage.Visible = true;

                        }
                    }
                }

                // 載入GridViewFile
                string sqlGridViewFile = @"SELECT   DownloadID, DisplayName, FilePath, CreatedByUserID, CreateDate
FROM     YachtDownloads
WHERE   (YachtID = @yachtId)";
                using (SqlCommand sqlCommandGridViewFile = new SqlCommand(sqlGridViewFile, sqlConnection))
                {
                    sqlCommandGridViewFile.Parameters.AddWithValue("@yachtId", YachtId);

                    using (SqlDataReader reader = sqlCommandGridViewFile.ExecuteReader())
                    {
                        GridViewFile.DataSource = reader;
                        GridViewFile.DataBind();
                    }
                }


                // 載入GridViewLayout
                string sqlGridViewLayout = @"SELECT   ImageID, ImagePath, CreateDate, CreatedByUserID, YachtID
FROM     YachtImages
WHERE   (ImageCategory = N'Layout') AND (YachtID = @yachtId)";
                using (SqlCommand sqlCommandGridViewLayout = new SqlCommand(sqlGridViewLayout, sqlConnection))
                {
                    sqlCommandGridViewLayout.Parameters.AddWithValue("@yachtId", YachtId);
                    using (SqlDataReader reader = sqlCommandGridViewLayout.ExecuteReader())
                    {
                        GridViewLayout.DataSource = reader;
                        GridViewLayout.DataBind();
                    }
                }


                // 載入RepeaterCarouselImages
                string sqlRepeaterCarousel = @"SELECT   ImagePath, IsHomepageCarousel, ImageID
FROM     YachtImages
WHERE   (ImageCategory = N'Carousel') AND (YachtID = @yachtId)";
                using (SqlCommand sqlCommandRepeaterCarousel = new SqlCommand(sqlRepeaterCarousel, sqlConnection))
                {
                    sqlCommandRepeaterCarousel.Parameters.AddWithValue("@yachtId", YachtId);

                    using (SqlDataReader reader = sqlCommandRepeaterCarousel.ExecuteReader())
                    {
                        RepeaterCarouselImages.DataSource = reader;
                        RepeaterCarouselImages.DataBind();
                    }
                }


            }


        }

        // 取消按鈕
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            if (YachtId > 0)
            {
                LoadData();
                LabelInfoMessage.Text = "已取消變更!";
                LabelInfoMessage.ForeColor = System.Drawing.Color.Blue;
                LabelInfoMessage.Visible = true;
            }
            else
            {
                txtModelText.Text = "";
                txtModelNumber.Text = "";
                CheckBoxIsActive.Checked = true;
                chkIsNewBuilding.Checked = false;
                chkIsNewDesign.Checked = false;
                CKEditorOverviewContent.Text = "";
                CKEditorSpecification.Text = "";
            }
        }

        // 待做: @user
        // 儲存按鈕
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModelText.Text))
            {
                LabelModelText.Text = "型號系列不得為空白!";
                LabelModelText.ForeColor = System.Drawing.Color.Red;
                LabelModelText.Visible = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(txtModelNumber.Text))
            {
                LabelModelNumber.Text = "型號數字不得為空白!";
                LabelModelNumber.ForeColor = System.Drawing.Color.Red;
                LabelModelNumber.Visible = true;
                return;
            }
            string sql;
            if (YachtId > 0)
            {
                sql = @"UPDATE  Yachts
SET        ModelName = @ModelName, ModelNameText = @ModelNameText, ModelNameNumber = @ModelNameNumber, IsNewBuilding = @IsNewBuilding, IsNewDesign = @IsNewDesign, OverviewContent = @OverviewContent, SpecificationsContent = @SpecificationsContent, IsActive = @IsActive, ModifiedDate = GetDate() 
WHERE   (YachtID = @yachtId)";
            }
            else
            {
                sql = @"INSERT INTO Yachts
              (ModelName, ModelNameText, ModelNameNumber, IsNewBuilding, IsNewDesign, OverviewContent, SpecificationsContent, IsActive)
VALUES  (@ModelName, @ModelNameText, @ModelNameNumber, @IsNewBuilding, @IsNewDesign, @OverviewContent, @SpecificationsContent, @IsActive)
SELECT SCOPE_IDENTITY()";

            }

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ModelName", txtModelText.Text + " " + txtModelNumber.Text);
                    sqlCommand.Parameters.AddWithValue("@ModelNameText", txtModelText.Text);
                    sqlCommand.Parameters.AddWithValue("@ModelNameNumber", txtModelNumber.Text);
                    sqlCommand.Parameters.AddWithValue("@IsNewBuilding", chkIsNewBuilding.Checked);
                    sqlCommand.Parameters.AddWithValue("@IsNewDesign", chkIsNewDesign.Checked);
                    sqlCommand.Parameters.AddWithValue("@IsActive", CheckBoxIsActive.Checked);
                    sqlCommand.Parameters.AddWithValue("@OverviewContent", CKEditorOverviewContent.Text);
                    sqlCommand.Parameters.AddWithValue("@SpecificationsContent", CKEditorSpecification.Text);
                    if (YachtId > 0)
                    {
                        sqlCommand.Parameters.AddWithValue("@yachtId", YachtId);
                    }

                    sqlConnection.Open();
                    // 抓回新插入的 Id (會回傳 SQL 查詢的第一列第一欄，剛好就是我們要的 Id)
                    if (YachtId > 0)
                    {
                        sqlCommand.ExecuteNonQuery(); 
                        LoadData();
                    }
                    else
                    {
                        int newId = Convert.ToInt32(sqlCommand.ExecuteScalar());
                        Response.Redirect($"YachtEdit.aspx?Id={newId}");
                    }


                }
            }
        }


        // 尺寸圖: 圖片上傳按鈕
        protected void ButtonDimensionUpload_Click(object sender, EventArgs e)
        {
            if (!FileUploadDimensionImage.HasFile)
            {
                LabelDimUploadMessage.Text = "請選擇一件圖片檔案!";
                LabelDimUploadMessage.ForeColor = System.Drawing.Color.Red;
                LabelDimUploadMessage.Visible = true;
                return;
            }

            var file = FileUploadDimensionImage.PostedFile;

            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (fileExtension != ".jpeg" && fileExtension != ".jpg")
            {
                LabelDimUploadMessage.Text = "請選擇.jpg/.jpeg圖片檔案!";
                LabelDimUploadMessage.ForeColor = System.Drawing.Color.Red;
                LabelDimUploadMessage.Visible = true;
                return;
            }

            var fileMemory = file.ContentLength;
            if (fileMemory > 1000000)
            {
                LabelDimUploadMessage.Text = "您選擇的圖片超過1MB,不能上傳!";
                LabelDimUploadMessage.ForeColor = System.Drawing.Color.Red;
                LabelDimUploadMessage.Visible = true;
                return;
            }

            var originalName = Path.GetFileName(file.FileName);
            var storedName = Guid.NewGuid().ToString() + fileExtension;
            var imagePath = "/Upload/Yachts/Images/Dimension/" + storedName;



            int id = Convert.ToInt32(Request.QueryString["Id"]);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();
                using (SqlTransaction tran = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        string folderPath = Server.MapPath("~/Upload/Yachts/Images/Dimension/");
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        // 儲存圖片檔案
                        file.SaveAs(folderPath + storedName);

                        // 儲存到資料庫

                        string sql = @"UPDATE  Yachts
SET        OverviewImageURL = @imageUrl
WHERE   (YachtID = @yachtId)";

                        using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection, tran))
                        {
                            sqlCommand.Parameters.AddWithValue("@imageUrl", imagePath);
                            sqlCommand.Parameters.AddWithValue("@yachtId", YachtId);

                            sqlCommand.ExecuteNonQuery();
                        }


                        tran.Commit();
                        LabelDimUploadMessage.Text = "圖片上傳成功!";
                        LabelDimUploadMessage.ForeColor = System.Drawing.Color.Blue;
                        LabelDimUploadMessage.Visible = true;
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        string fullPath = Server.MapPath("~/Upload/Yachts/Images/Dimension/" + storedName);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        LabelDimUploadMessage.Text = "圖片上傳失敗!" + ex.Message;
                        LabelDimUploadMessage.ForeColor = System.Drawing.Color.Red;
                        LabelDimUploadMessage.Visible = true;
                    }
                }
            }
        }

        //  定義檔案CLASS類別 (多檔案上傳用)
        private class FileInfo
        {
            public HttpPostedFile AttachFile { get; set; }
            public string OriginalName { get; set; }
            public string StoredName { get; set; }
            public string FilePath { get; set; }
            public string DisplayName { get; set; }

        }

        // 檔案: 檔案上傳按鈕
        protected void ButtonFileUpload_Click(object sender, EventArgs e)
        {
            if (!FileUploadDownload.HasFiles)
            {
                LabelDownloadMessage.Text = "請選擇至少一件檔案!";
                LabelDownloadMessage.ForeColor = System.Drawing.Color.Red;
                LabelDownloadMessage.Visible = true;
                return;
            }
            else
            {
                var fileInfos = new List<FileInfo>();

                foreach (var file in FileUploadDownload.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (fileExtension != ".pdf")
                    {
                        LabelDownloadMessage.Text = "請選擇.pdf檔案!";
                        LabelDownloadMessage.ForeColor = System.Drawing.Color.Red;
                        LabelDownloadMessage.Visible = true;
                        return;
                    }

                    var fileMemory = file.ContentLength;
                    if (fileMemory > 5 * 1024 * 1024)
                    {
                        LabelDownloadMessage.Text = "您選擇的檔案超過5MB,不能上傳!";
                        LabelDownloadMessage.ForeColor = System.Drawing.Color.Red;
                        LabelDownloadMessage.Visible = true;
                        return;
                    }

                    var originalName = Path.GetFileName(file.FileName);
                    var storedName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = "/Upload/Yachts/Files/" + storedName;


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
                            string folderPath = Server.MapPath("~/Upload/Yachts/Files/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            // 儲存圖片檔案
                            foreach (var file in fileInfos)
                            {
                                file.AttachFile.SaveAs(folderPath + file.StoredName);


                                // 儲存到資料庫
                                string sql = @"INSERT INTO YachtDownloads
              (YachtID, OriginalFileName, StoredFileName, DisplayName, FilePath)
VALUES  (@YachtID, @OriginalFileName, @StoredFileName, @DisplayName, @FilePath)";
                                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection, tran))
                                {
                                    sqlCommand.Parameters.AddWithValue("@YachtID", id);
                                    sqlCommand.Parameters.AddWithValue("@OriginalFileName", file.OriginalName);
                                    sqlCommand.Parameters.AddWithValue("@StoredFileName", file.StoredName);
                                    sqlCommand.Parameters.AddWithValue("@FilePath", file.FilePath);
                                    sqlCommand.Parameters.AddWithValue("@DisplayName", file.DisplayName);

                                    sqlCommand.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                            LabelDownloadMessage.Text = "檔案上傳成功!";
                            LabelDownloadMessage.ForeColor = System.Drawing.Color.Blue;
                            LabelDownloadMessage.Visible = true;
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                            if (tran?.Connection != null)
                            {
                                tran.Rollback(); // 確保交易還活著再 Rollback
                            }
                            foreach (var file in fileInfos)
                            {
                                string fullPath = Server.MapPath("~/Upload/Yachts/Files/" + file.StoredName);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            LabelDownloadMessage.Text = "檔案上傳失敗!" + ex.Message;
                            LabelDownloadMessage.ForeColor = System.Drawing.Color.Red;
                            LabelDownloadMessage.Visible = true;
                        }
                    }
                }
            }
        }

        //  定義圖片CLASS類別 (多圖上傳用)
        private class ImageInfo
        {
            public HttpPostedFile ImageFile { get; set; }
            public string OriginalName { get; set; }
            public string StoredName { get; set; }
            public string ImagePath { get; set; }

        }

        // Layout圖片: 圖片上傳按鈕
        protected void ButtonLayoutUpload_Click(object sender, EventArgs e)
        {
            if (!FileUploadLayout.HasFiles)
            {
                LabelLayoutMessage.Text = "請選擇至少一件圖片檔案!";
                LabelLayoutMessage.ForeColor = System.Drawing.Color.Red;
                LabelLayoutMessage.Visible = true;
                return;
            }
            else
            {
                var imageInfos = new List<ImageInfo>();

                foreach (var file in FileUploadLayout.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (fileExtension != ".jpeg" && fileExtension != ".jpg")
                    {
                        LabelLayoutMessage.Text = "請選擇.jpg/.jpeg圖片檔案!";
                        LabelLayoutMessage.ForeColor = System.Drawing.Color.Red;
                        LabelLayoutMessage.Visible = true;
                        return;
                    }

                    var fileMemory = file.ContentLength;
                    if (fileMemory > 1000000)
                    {
                        LabelLayoutMessage.Text = "您選擇的圖片超過1MB,不能上傳!";
                        LabelLayoutMessage.ForeColor = System.Drawing.Color.Red;
                        LabelLayoutMessage.Visible = true;
                        return;
                    }

                    var originalName = Path.GetFileName(file.FileName);
                    var storedName = Guid.NewGuid().ToString() + fileExtension;
                    var imagePath = "/Upload/Yachts/Images/Layout/" + storedName;


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
                            string folderPath = Server.MapPath("~/Upload/Yachts/Images/Layout/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            // 儲存圖片檔案
                            foreach (var image in imageInfos)
                            {
                                image.ImageFile.SaveAs(folderPath + image.StoredName);


                                // 儲存到資料庫
                                string sql = @"INSERT INTO YachtImages
              (YachtID, ImageCategory, ImagePath)
VALUES  (@Id, N'Layout', @ImagePath)";
                                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection, tran))
                                {
                                    sqlCommand.Parameters.AddWithValue("@Id", id);
                                    sqlCommand.Parameters.AddWithValue("@ImagePath", image.ImagePath);

                                    sqlCommand.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                            LabelLayoutMessage.Text = "圖片上傳成功!";
                            LabelLayoutMessage.ForeColor = System.Drawing.Color.Blue;
                            LabelLayoutMessage.Visible = true;
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            foreach (var image in imageInfos)
                            {
                                string fullPath = Server.MapPath("~/Upload/Yachts/Images/Layout/" + image.StoredName);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            LabelLayoutMessage.Text = "圖片上傳失敗!" + ex.Message;
                            LabelLayoutMessage.ForeColor = System.Drawing.Color.Red;
                            LabelLayoutMessage.Visible = true;
                        }
                    }
                }
            }
        }

        // Carousel圖片: 圖片上傳按鈕
        protected void ButtonCarouselImages_Click(object sender, EventArgs e)
        {
            if (!FileUploadCarouselImages.HasFiles)
            {
                LabelCarouselMessage.Text = "請選擇至少一件圖片檔案!";
                LabelCarouselMessage.ForeColor = System.Drawing.Color.Red;
                LabelCarouselMessage.Visible = true;
                return;
            }
            else
            {
                var imageInfos = new List<ImageInfo>();

                foreach (var file in FileUploadCarouselImages.PostedFiles)
                {
                    string fileExtension = Path.GetExtension(file.FileName).ToLower();

                    if (fileExtension != ".jpeg" && fileExtension != ".jpg")
                    {
                        LabelCarouselMessage.Text = "請選擇.jpg/.jpeg圖片檔案!";
                        LabelCarouselMessage.ForeColor = System.Drawing.Color.Red;
                        LabelCarouselMessage.Visible = true;
                        return;
                    }

                    var fileMemory = file.ContentLength;
                    if (fileMemory > 1000000)
                    {
                        LabelCarouselMessage.Text = "您選擇的圖片超過1MB,不能上傳!";
                        LabelCarouselMessage.ForeColor = System.Drawing.Color.Red;
                        LabelCarouselMessage.Visible = true;
                        return;
                    }

                    var originalName = Path.GetFileName(file.FileName);
                    var storedName = Guid.NewGuid().ToString() + fileExtension;
                    var imagePath = "/Upload/Yachts/Images/Carousel/" + storedName;


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
                            string folderPath = Server.MapPath("~/Upload/Yachts/Images/Carousel/");
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            // 儲存圖片檔案
                            foreach (var image in imageInfos)
                            {
                                image.ImageFile.SaveAs(folderPath + image.StoredName);


                                // 儲存到資料庫
                                string sql = @"INSERT INTO YachtImages
              (YachtID, ImageCategory, ImagePath)
VALUES  (@Id, N'Carousel', @ImagePath)";
                                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection, tran))
                                {
                                    sqlCommand.Parameters.AddWithValue("@Id", id);
                                    sqlCommand.Parameters.AddWithValue("@ImagePath", image.ImagePath);

                                    sqlCommand.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                            LabelCarouselMessage.Text = "圖片上傳成功!";
                            LabelCarouselMessage.ForeColor = System.Drawing.Color.Blue;
                            LabelCarouselMessage.Visible = true;
                            LoadData();
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            foreach (var image in imageInfos)
                            {
                                string fullPath = Server.MapPath("~/Upload/Yachts/Images/Carousel/" + image.StoredName);
                                if (System.IO.File.Exists(fullPath))
                                {
                                    System.IO.File.Delete(fullPath);
                                }
                            }
                            LabelCarouselMessage.Text = "圖片上傳失敗!" + ex.Message;
                            LabelCarouselMessage.ForeColor = System.Drawing.Color.Red;
                            LabelCarouselMessage.Visible = true;
                        }
                    }
                }
            }
        }

        // Dimension Gridview: 新增按鈕
        protected void GridViewDimension_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "AddSpec")
            {
                TextBox txtSpecLabel = GridViewDimension.FooterRow.FindControl("TextBoxSpecLabel") as TextBox;
                TextBox txtSpecValue = GridViewDimension.FooterRow.FindControl("TextBoxSpecValue") as TextBox;

                if (string.IsNullOrWhiteSpace(txtSpecLabel.Text) || string.IsNullOrWhiteSpace(txtSpecValue.Text))
                {
                    LabelGridViewDimMessage.Text = "規格名稱和內容不得為空白！";
                    LabelGridViewDimMessage.ForeColor = System.Drawing.Color.Red;
                    LabelGridViewDimMessage.Visible = true;
                    return;
                }

                string sql = @"INSERT INTO YachtSpecs
              (YachtID, SpecLabel, SpecValue)
VALUES  (@YachtID, @SpecLabel, @SpecValue)";

                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@YachtID", YachtId);
                        sqlCommand.Parameters.AddWithValue("@SpecLabel", txtSpecLabel.Text.Trim());
                        sqlCommand.Parameters.AddWithValue("@SpecValue", txtSpecValue.Text.Trim());

                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                GridViewDimension.EditIndex = -1;
                LoadData();
                LabelGridViewDimMessage.Text = "規格新增成功！";
                LabelGridViewDimMessage.ForeColor = System.Drawing.Color.Blue;
                LabelGridViewDimMessage.Visible = true;
            }
        }

        // Dimension GridView: 編輯按鈕
        protected void GridViewDimension_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            GridViewDimension.EditIndex = rowIndex;
            LoadData();
            
        }

        // Dimension GridView: 取消按鈕
        protected void GridViewDimension_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewDimension.EditIndex = -1;
            LoadData();
            LabelGridViewDimMessage.Text = "規格已取消編輯！";
            LabelGridViewDimMessage.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewDimMessage.Visible = true;
        }

        // Dimension GridView: 更新按鈕
        protected void GridViewDimension_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow gridViewRow = GridViewDimension.Rows[rowIndex];
            TextBox txtSpecLabel = gridViewRow.FindControl("TextBoxSpecLabelEdit") as TextBox;
            TextBox txtSpecValue = gridViewRow.FindControl("TextBoxSpecValueEdit") as TextBox;

            if (string.IsNullOrWhiteSpace(txtSpecLabel.Text) || string.IsNullOrWhiteSpace(txtSpecValue.Text))
            {
                LabelGridViewDimMessage.Text = "規格名稱和內容不得為空白！";
                LabelGridViewDimMessage.ForeColor = System.Drawing.Color.Red;
                LabelGridViewDimMessage.Visible = true;
                return;
            }

            int specId = Convert.ToInt32(GridViewDimension.DataKeys[rowIndex].Value);

            string sql = @"UPDATE  YachtSpecs
SET        SpecLabel = @SpecLabel, SpecValue = @SpecValue
WHERE   (SpecID = @SpecID)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@SpecID", specId);
                    sqlCommand.Parameters.AddWithValue("@SpecLabel", txtSpecLabel.Text.Trim());
                    sqlCommand.Parameters.AddWithValue("@SpecValue", txtSpecValue.Text.Trim());

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewDimension.EditIndex = -1;
            LoadData();
            LabelGridViewDimMessage.Text = "規格更新成功！";
            LabelGridViewDimMessage.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewDimMessage.Visible = true;
        }

        // Dimension GridView: 刪除按鈕
        protected void GridViewDimension_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int specId = Convert.ToInt32(GridViewDimension.DataKeys[rowIndex].Value);

            string sql = @"DELETE FROM YachtSpecs
WHERE   (SpecID = @SpecID)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@SpecID", specId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewDimension.EditIndex = -1;
            LoadData();
            LabelGridViewDimMessage.Text = "規格刪除成功！";
            LabelGridViewDimMessage.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewDimMessage.Visible = true;
        }

        //尺寸圖: 刪除按鈕
        protected void LinkButtonDeleteDimensionImage_Click(object sender, EventArgs e)
        {
            string sql = @"UPDATE  Yachts
SET        OverviewImageURL = NULL
WHERE   (YachtID = @YachtID)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", YachtId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            LoadData();
            LabelDimImageMessage.Text = "尺寸圖刪除成功！";
            LabelDimImageMessage.ForeColor = System.Drawing.Color.Blue;
            LabelDimImageMessage.Visible = true;
        }

        // Download GridView: 編輯按鈕
        protected void GridViewFile_RowEditing(object sender, GridViewEditEventArgs e)
        {
            int rowIndex = e.NewEditIndex;
            GridViewFile.EditIndex = rowIndex;
            LoadData();
        }

        // Download GridView: 取消按鈕
        protected void GridViewFile_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewFile.EditIndex = -1;
            LoadData();
            LabelDownloadGridViewMessage.Text = "已取消編輯！";
            LabelDownloadGridViewMessage.ForeColor = System.Drawing.Color.Blue;
            LabelDownloadGridViewMessage.Visible = true;
        }

        // Download GridView: 更新按鈕
        protected void GridViewFile_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow gridViewRow = GridViewFile.Rows[rowIndex];
            TextBox txtDisplayName = gridViewRow.FindControl("TextBoxDisplayName") as TextBox;

            if (string.IsNullOrWhiteSpace(txtDisplayName.Text))
            {
                LabelDownloadGridViewMessage.Text = "檔案名稱不得為空白!";
                LabelDownloadGridViewMessage.ForeColor = System.Drawing.Color.Red;
                LabelDownloadGridViewMessage.Visible = true;
                return;
            }


            string nameEdit = txtDisplayName.Text.Trim();

            // 在存進 OriginalFileName 前，先確認檔名是否已經有副檔名，避免重複。
            if (!nameEdit.ToLower().EndsWith(".pdf"))
            {
                nameEdit += ".pdf";
            }

            int fileId = Convert.ToInt32(GridViewFile.DataKeys[rowIndex].Value);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();


                string updateSql = @"UPDATE  YachtDownloads
SET        DisplayName = @DisplayName
WHERE   (DownloadID = @DownloadID)";

                using (SqlCommand updateCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    updateCommand.Parameters.AddWithValue("@DisplayName", nameEdit);
                    updateCommand.Parameters.AddWithValue("@DownloadID", fileId);

                    updateCommand.ExecuteNonQuery();
                }


                GridViewFile.EditIndex = -1;
                LoadData();

                LabelDownloadGridViewMessage.Text = "更新成功!";
                LabelDownloadGridViewMessage.ForeColor = System.Drawing.Color.Blue;
                LabelDownloadGridViewMessage.Visible = true;

            }
        }

        // Download GridView: 刪除按鈕
        protected void GridViewFile_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            int rowIndex = e.RowIndex;
            int fileId = Convert.ToInt32(GridViewFile.DataKeys[rowIndex].Value);

            string sql = @"DELETE FROM YachtDownloads
WHERE   (DownloadID = @DownloadID)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@DownloadID", fileId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewFile.EditIndex = -1;
            LoadData();
            LabelDownloadGridViewMessage.Text = "檔案刪除成功！";
            LabelDownloadGridViewMessage.ForeColor = System.Drawing.Color.Blue;
            LabelDownloadGridViewMessage.Visible = true;
        }

        // Layout GridView: 刪除按鈕
        protected void GridViewLayout_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int imageId = Convert.ToInt32(GridViewLayout.DataKeys[rowIndex].Value);

            string sql = @"DELETE FROM YachtImages
WHERE   (ImageID = @ImageId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ImageId", imageId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewLayout.EditIndex = -1;
            LoadData();
            LabelGridViewLayout.Text = "圖片刪除成功！";
            LabelGridViewLayout.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewLayout.Visible = true;
        }

        // 輪播圖Repeater: 刪除按鈕
        protected void RepeaterCarouselImages_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int imageId = Convert.ToInt32(e.CommandArgument);

                string sql = @"DELETE FROM YachtImages
WHERE   (ImageID = @ImageId)";

                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@ImageId", imageId);

                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                LoadData();
                LabelGridViewCarousel.Text = "圖片刪除成功！";
                LabelGridViewCarousel.ForeColor = System.Drawing.Color.Blue;
                LabelGridViewCarousel.Visible = true;
            }
        }

   


        // 輪播圖Repeater: radiobutton更新
        protected void RadioButtonSelectHomepage_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            RepeaterItem item = (RepeaterItem)radioButton.NamingContainer;
            HiddenField hiddenField = (HiddenField)item.FindControl("HiddenFieldImageID");
            int selectedImageID = Convert.ToInt32(hiddenField.Value);
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();

                string sqlReset = @"UPDATE  YachtImages
SET        IsHomepageCarousel = 0
WHERE   (YachtID = @YachtID) AND (ImageCategory = N'Carousel')";

                using (SqlCommand sqlCommand = new SqlCommand(sqlReset, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", YachtId);
                    sqlCommand.ExecuteNonQuery();
                }

                string updateSql = @"UPDATE  YachtImages
SET        IsHomepageCarousel = 1
WHERE   (ImageID = @ImageId)";

                using (SqlCommand sqlCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@ImageId", selectedImageID);
                    sqlCommand.ExecuteNonQuery();
                }

            }
            LoadData();
            LabelGridViewCarousel.Text = "更新成功！";
            LabelGridViewCarousel.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewCarousel.Visible = true;
        }
    }
}