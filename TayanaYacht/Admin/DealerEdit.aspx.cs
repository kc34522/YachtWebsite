using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.Admin
{
    public partial class DealerEdit : System.Web.UI.Page
    {
        //待做: 分頁

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountryList();

                if (Request.QueryString["Id"] != null)
                {
                    LabelTitle.Text = "編輯經銷商資訊";
                    LoadDealerViewMode();
                    LoadDealerData();
                    PanelViewMode.Visible = true;
                    PanelEditMode.Visible = false;
                }
                else
                {
                    LabelTitle.Text = "新增經銷商";
                    PanelViewMode.Visible = false;
                    PanelEditMode.Visible = true;
                    ButtonCancel.Visible = false;
                }
                
            }
            ClearMessages();

        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelName.Visible = false;
            LabelImage.Visible = false;
            LabelEditMode.Visible = false;
        }

        // 載入國家下拉選單
        private void BindCountryList()
        {
            string sql = @"SELECT DISTINCT C.Id, C.Name
                            FROM Country C
                            ORDER BY C.Name;";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {

                        ddlCountry.DataSource = sqlDataReader;
                        ddlCountry.DataTextField = "Name";
                        ddlCountry.DataValueField = "Id";
                        ddlCountry.DataBind();
                    }
                }
            }
        }

        // 載入地區下拉選單
        private void LoadRegionDropDown(int countryId)
        {
            string sql = "SELECT Id, Name FROM Region WHERE CountryId = @CountryId ORDER BY Name";
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                conn.Open();
                ddlRegion.DataSource = cmd.ExecuteReader();
                ddlRegion.DataTextField = "Name";
                ddlRegion.DataValueField = "Id";
                ddlRegion.DataBind();
            }
            //ddlRegion.Items.Insert(0, new ListItem("--請選擇地區--", "0"));
        }

        // 載入顯示模式
        private void LoadDealerViewMode()
        {
            string sql = @"SELECT   Country.Name AS CountryName, Region.Name AS RegionName, Dealer.Contact, Dealer.Address, Dealer.Fax, Dealer.Cell, Dealer.Email, Dealer.Website, Dealer.ImagePath, Dealer.IsActive, Dealer.CreatedDate, 
              Dealer.CreatedBy, Dealer.UpdatedAt, Dealer.UpdatedBy, Dealer.Name AS DealerName, Dealer.Tel
FROM     Dealer INNER JOIN
              Region ON Dealer.RegionId = Region.Id INNER JOIN
              Country ON Region.CountryId = Country.Id
 WHERE   (Dealer.Id = @dealerId)";

            int dealerId = Convert.ToInt32(Request.QueryString["Id"]);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@dealerId", dealerId);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        lblCountry.Text = sqlDataReader["CountryName"].ToString();
                        lblRegion.Text = sqlDataReader["RegionName"].ToString();
                        lblName.Text = sqlDataReader["DealerName"].ToString();
                        lblContact.Text = sqlDataReader["Contact"].ToString();
                        lblAddress.Text = sqlDataReader["Address"].ToString();
                        lblTel.Text = sqlDataReader["Tel"].ToString();
                        lblFax.Text = sqlDataReader["Fax"].ToString();
                        lblCell.Text = sqlDataReader["Cell"].ToString();
                        lblEmail.Text = sqlDataReader["Email"].ToString();
                        lblWebsite.Text = sqlDataReader["Website"].ToString();
                        ImageView.ImageUrl = sqlDataReader["ImagePath"].ToString();
                        CheckBoxIsActive.Checked = (Boolean)sqlDataReader["IsActive"];
                        lblCreatedDate.Text = sqlDataReader["CreatedDate"].ToString();
                        lblCreatedBy.Text = sqlDataReader["CreatedBy"].ToString();
                        lblUpdatedAt.Text = sqlDataReader["UpdatedAt"].ToString();
                        lblUpdatedBy.Text = sqlDataReader["UpdatedBy"].ToString();


                    }

                }
            }
        }

        // 載入編輯模式
        private void LoadDealerData()
        {
            string sql = @"SELECT   Country.Name AS CountryName, Region.Name AS RegionName, Dealer.Contact, Dealer.Address, Dealer.Fax, Dealer.Cell, Dealer.Email, Dealer.Website, Dealer.ImagePath, Dealer.IsActive, Dealer.CreatedDate, 
              Dealer.CreatedBy, Dealer.UpdatedAt, Dealer.UpdatedBy, Dealer.Name AS DealerName, Dealer.Tel, Region.CountryId, Dealer.RegionId
FROM     Dealer INNER JOIN
              Region ON Dealer.RegionId = Region.Id INNER JOIN
              Country ON Region.CountryId = Country.Id
 WHERE   (Dealer.Id = @dealerId)";

            int dealerId = Convert.ToInt32(Request.QueryString["Id"]);

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@dealerId", dealerId);
                    sqlConnection.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                    {
                        string countryId = sqlDataReader["CountryId"].ToString();
                        LoadRegionDropDown(Convert.ToInt32(countryId));
                        ddlCountry.SelectedValue = countryId;
                        ddlRegion.SelectedValue = sqlDataReader["RegionId"].ToString();
                        txtName.Text = sqlDataReader["DealerName"].ToString();
                        txtContact.Text = sqlDataReader["Contact"].ToString();
                        txtAddress.Text = sqlDataReader["Address"].ToString();
                        txtTel.Text = sqlDataReader["Tel"].ToString();
                        txtFax.Text = sqlDataReader["Fax"].ToString();
                        txtCell.Text = sqlDataReader["Cell"].ToString();
                        txtEmail.Text = sqlDataReader["Email"].ToString();
                        txtWebsite.Text = sqlDataReader["Website"].ToString();
                        ImageView2.ImageUrl = sqlDataReader["ImagePath"].ToString();
                        chkIsActive.Checked = (Boolean)sqlDataReader["IsActive"];
                    }

                }
            }
        }

        // 編輯按鈕: 觸發顯示Edit Panel
        protected void ButtonEdit_Click(object sender, EventArgs e)
        {
            PanelViewMode.Visible = false;
            PanelEditMode.Visible = true;
        }

        // 國家下拉選單變更
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int countryId = Convert.ToInt32(ddlCountry.SelectedValue);
            LoadRegionDropDown(Convert.ToInt32(countryId));
        }

        // 儲存按鈕: 儲存Edit Panel
        // 待做: 必填欄位, 修改編輯時間, website連結, 圖片上傳, sortorder, 下拉選單應該不要第一個是china
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                LabelName.Text = "必填欄位!";
                LabelName.Visible = true;
                return; // <== 終止執行，避免儲存空值
            }

            string imagePath = ImageView.ImageUrl.ToString();

            if (FileUploadImage.HasFile)
            {
                string ext = Path.GetExtension(FileUploadImage.FileName).ToLower();
                string[] allowExts = { ".jpg", ".jpeg", ".png" };
                if (!allowExts.Contains(ext))
                {
                    LabelImage.Text = "請上傳.jpg/.jpeg/.png 圖片格式檔!";
                    return;
                }

                string newfileName = Guid.NewGuid().ToString() + ext;
                string serverPath = Server.MapPath("~/Upload/Dealers/");
                string savePath = serverPath + newfileName;

                // 若資料夾不存在，自動建立(待學習)
                //if (!Directory.Exists(folderPath))
                //{
                //    Directory.CreateDirectory(folderPath);
                //}

                FileUploadImage.SaveAs(savePath);

                // 更新儲存路徑為虛擬路徑（寫入資料庫用）
                imagePath = "~/Upload/Dealers/" + newfileName;

            }


            


           

            int regionId = Convert.ToInt32(ddlRegion.SelectedValue);
            string dealerName = txtName.Text;
            string contact = txtContact.Text;
            string address = txtAddress.Text;
            string tel = txtTel.Text;
            string fax = txtFax.Text;
            string cell = txtCell.Text;
            string email = txtEmail.Text;
            string website = txtWebsite.Text;
            bool isActive = chkIsActive.Checked;
            //int updatedBy = (int)Session["userId"]; // 待確認登入session
            int dealerId = Convert.ToInt32(Request.QueryString["Id"]);


            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = sqlConnection;
                    if (Request.QueryString["Id"] != null)
                    {
                        sqlCommand.CommandText = @"UPDATE  Dealer
SET        RegionId = @regionId, Name = @dealerName, Contact = @contact, Address = @address, Tel = @tel, Fax = @fax, Cell = @cell, Email = @email, Website = @website, ImagePath = @imagePath, IsActive = @isActive, UpdatedAt = GetDate()
WHERE(Id = @dealerId)";

                        //UpdatedBy = @updatedBy
                        //sqlCommand.Parameters.AddWithValue("@updatedBy", updatedBy); // 待確認登入session
                        sqlCommand.Parameters.AddWithValue("@dealerId", dealerId);

                    }
                    else
                    {
                        sqlCommand.CommandText = @"INSERT INTO Dealer
              (RegionId, Name, Contact, Address, Tel, Fax, Cell, Email, Website, ImagePath, IsActive, CreatedBy)
VALUES  (@regionId,@dealerName,@contact,@address,@tel,@fax,@cell,@email,@website,@imagePath,@isActive,'admin')";
                    }

                        sqlCommand.Parameters.AddWithValue("@regionId", regionId);
                    sqlCommand.Parameters.AddWithValue("@dealerName", dealerName);
                    sqlCommand.Parameters.AddWithValue("@contact", contact);
                    sqlCommand.Parameters.AddWithValue("@address", address);
                    sqlCommand.Parameters.AddWithValue("@tel", tel);
                    sqlCommand.Parameters.AddWithValue("@fax", fax);
                    sqlCommand.Parameters.AddWithValue("@cell", cell);
                    sqlCommand.Parameters.AddWithValue("@email", email);
                    sqlCommand.Parameters.AddWithValue("@website", website);
                    sqlCommand.Parameters.AddWithValue("@imagePath", imagePath);
                    sqlCommand.Parameters.AddWithValue("@isActive", isActive);
                   

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            LoadDealerViewMode();
            if (Request.QueryString["Id"] != null)
            {
                PanelEditMode.Visible = false;
                PanelViewMode.Visible = true;
            }
            else
            {
                LabelEditMode.Text = "儲存成功!";
                LabelEditMode.Visible = true;
            }

            txtName.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            txtTel.Text = "";
            txtFax.Text = "";
            txtCell.Text = "";
            txtEmail.Text = "";
            txtWebsite.Text = "";
            ImageView2.ImageUrl = null;
            chkIsActive.Checked = false;

        }

        // 取消按鈕: 取消編輯Edit Panel
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
            PanelEditMode.Visible = false;
            PanelViewMode.Visible = true;
        }


    }
}