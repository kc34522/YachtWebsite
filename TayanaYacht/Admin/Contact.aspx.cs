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
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearMessages();

            if (!IsPostBack)
            {
                LoadContactList();
            }
        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelContactListMessage.Visible = false;
            LabelSearchResult.Visible = false;
        }

        // 載入Contact清單
        private void LoadContactList(string keyword = "")
        {
            int currentPageIndex = GridViewContact.PageIndex; // ✅ 先記住目前頁數

            string sql = @"SELECT   ContactRecord.Id, ContactRecord.Name, ContactRecord.Email, ContactRecord.Phone, Country.Name AS CountryName, Yachts.ModelName, ContactRecord.Comments, ContactRecord.CreatedTime
FROM     ContactRecord INNER JOIN
              Country ON ContactRecord.CountryId = Country.Id INNER JOIN
              Yachts ON ContactRecord.BrochureId = Yachts.YachtID
WHERE 1 = 1";  // ← 方便加條件


            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql += @"AND (
                    ContactRecord.Name LIKE @kw OR 
                    ContactRecord.Email LIKE @kw OR 
                    ContactRecord.Phone LIKE @kw OR 
                    Country.Name LIKE @kw OR
                    Yachts.ModelName LIKE @kw OR
                    ContactRecord.Comments LIKE @kw
                 )";
            }

            sql += "ORDER BY ContactRecord.CreatedTime DESC;";


            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    if (!string.IsNullOrWhiteSpace(keyword))
                    {
                        sqlCommand.Parameters.AddWithValue("@kw", "%" + keyword + "%");
                    }
                    sqlConnection.Open();

                    // ❌ 這裡 GridView 要求可分頁的資料來源，SqlDataReader 不行
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        GridViewContact.DataSource = dataTable;
                        GridViewContact.PageIndex = currentPageIndex; // ✅ 設回剛剛記下來的頁數
                        GridViewContact.DataBind();
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
                    LabelContactListMessage.Text = "已成功刪除!";
                    LabelContactListMessage.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    LabelContactListMessage.Text = "刪除失敗，請稍後再試。";
                    LabelContactListMessage.ForeColor = System.Drawing.Color.Red;
                }

                LabelContactListMessage.Visible = true;
            }

            // 如果有搜尋條件，就帶入關鍵字
            String keyword = ViewState["SearchKeyword"] as string;
            LoadContactList(keyword);// ⚠️ 移出 using 區塊，確保連線已釋放後再執行
        }

        // 分頁控制
        protected void GridViewContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            GridViewContact.PageIndex = e.NewPageIndex;

            // 如果有搜尋條件，就帶入關鍵字
            String keyword = ViewState["SearchKeyword"] as string;
            LoadContactList(keyword);
        }

        // 搜尋按鈕
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            string keyword = TextBoxSearch.Text.Trim();
            ViewState["SearchKeyword"] = keyword; // 存起來讓分頁也能用

            GridViewContact.PageIndex = 0; // 回到第一頁
            LoadContactList(keyword);

            if (!string.IsNullOrEmpty(keyword))
            {
                LabelSearchResult.Text = $"搜尋結果：包含「{keyword}」的結果如下：";
                LabelSearchResult.Visible = true;
            }
        }

        // 搜尋清除按鈕
        protected void ButtonClearSearch_Click(object sender, EventArgs e)
        {
            TextBoxSearch.Text = "";
            ViewState["SearchKeyword"] = null;
            LabelSearchResult.Visible = false;

            GridViewContact.PageIndex = 0; // 回到第一頁
            LoadContactList(); // 載入全部
        }

        protected void GridViewContact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = e.Row.RowIndex;
                int pageIndex = GridViewContact.PageIndex;
                int pageSize = GridViewContact.PageSize;
                int rowNumber = (pageIndex * pageSize) + rowIndex + 1;

                Label lblRowNumber = (Label)e.Row.FindControl("LabelRowNumber");
                if (lblRowNumber != null)
                {
                    lblRowNumber.Text = rowNumber.ToString();
                }

                // 若要除錯觀察值，也可暫時加上
                // e.Row.Cells[0].Text = $"PageIndex: {pageIndex}, RowIndex: {rowIndex}";
            }
        }
    }
}

