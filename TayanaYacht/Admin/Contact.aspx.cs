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
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadContactList();
            }

            ClearMessages();

        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelContactListMessage.Visible = false;
           
        }

        //// 搜尋按鈕
        //protected void ButtonSearch_Click(object sender, EventArgs e)
        //{
        //    string keyword = TextBoxSearch.Text.Trim();
        //    LoadVideoList(keyword);

        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        LabelSearchResult.Text = "搜尋結果：包含「" + keyword + "」的影片如下:";
        //        LabelSearchResult.Visible = true;
        //    }
        //}

        //// 清除搜尋按鈕
        //protected void ButtonClearSearch_Click(object sender, EventArgs e)
        //{
        //    TextBoxSearch.Text = "";
        //    LabelSearchResult.Visible = false;
        //    LoadVideoList();
        //}


        // 載入Contact清單
        private void LoadContactList()
        {
            string sql = @"
                            SELECT   ContactRecord.Id, ContactRecord.Name, ContactRecord.Email, ContactRecord.Phone, Country.Name AS CountryName, Yachts.Name AS Yacht, ContactRecord.Comments, ContactRecord.CreatedTime
                            FROM     ContactRecord INNER JOIN
              Country ON ContactRecord.CountryId = Country.Id INNER JOIN
              Yachts ON ContactRecord.BrochureId = Yachts.Id";
          
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            GridViewContact.DataSource = sqlDataReader;
                            GridViewContact.DataBind();
                        }
                    }
                }
            }
        }

        // 刪除按鈕
        protected void GridViewConctact_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewContact.EditIndex = -1; // ❗先清除任何編輯狀態

            int rowIndex = e.RowIndex;
            int contactId = Convert.ToInt32(GridViewContact.DataKeys[rowIndex].Value);

            string sql = @"DELETE FROM ContactRecord
                            WHERE   (Id = @contactId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@contactId", contactId);

                sqlConnection.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    LabelContactListMessage.Text = "已成功刪除影片!";
                    LabelContactListMessage.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    LabelContactListMessage.Text = "刪除失敗，請稍後再試。";
                    LabelContactListMessage.ForeColor = System.Drawing.Color.Red;
                }

                LabelContactListMessage.Visible = true;
            }

            LoadContactList(); // ⚠️ 移出 using 區塊，確保連線已釋放後再執行
        }
    }
}