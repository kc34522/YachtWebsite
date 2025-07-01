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
    public partial class DealerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountryList();
                LoadDealerList();
            }
            ClearMessages();

        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelDealerListMessage.Visible = false;
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


        // 載入國家下拉選單
        private void BindCountryList()
        {
            string sql = @"SELECT DISTINCT C.Id, C.Name
                            FROM Dealer D
                            JOIN Region R ON D.RegionId = R.Id
                            JOIN Country C ON R.CountryId = C.Id
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
                        ddlCountry.Items.Insert(0, new ListItem("全部國家", "0"));  // 預設選項
                    }
                }
            }
        }

        // 下拉選單篩選時自動刷新Dealer清單
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDealerList();
        }

        // 載入Dealer清單
        private void LoadDealerList()
        {
            int countryId = Convert.ToInt32(ddlCountry.SelectedValue);
            string sql = @"
                        SELECT   
                            D.Id,
                            R.Name AS RegionName,
                            D.Name,
                            D.Tel, 
                            D.Email, 
                            D.Website, 
                            D.IsActive
                        FROM Dealer D
                        JOIN Region R ON D.RegionId = R.Id
                        JOIN Country C ON R.CountryId = C.Id
                        WHERE (@countryId = 0 OR C.Id = @countryId)
                        ORDER BY R.SortOrder, D.SortOrder";



            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@countryId", countryId);

                    sqlConnection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {

                        GridViewDealer.DataSource = sqlDataReader;
                        GridViewDealer.DataBind();
                    }
                }
            }
        }



        // 刪除按鈕
        protected void GridViewConctact_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewDealer.EditIndex = -1; // ❗先清除任何編輯狀態

            int rowIndex = e.RowIndex;
            int dealerId = Convert.ToInt32(GridViewDealer.DataKeys[rowIndex].Value);

            string sql = @"DELETE FROM Dealer
                            WHERE   (Id = @dealerId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
            {
                sqlCommand.Parameters.AddWithValue("@dealerId", dealerId);

                sqlConnection.Open();
                int rowsAffected = sqlCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    LabelDealerListMessage.Text = "已成功刪除經銷商!";
                    LabelDealerListMessage.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    LabelDealerListMessage.Text = "刪除失敗，請稍後再試。";
                    LabelDealerListMessage.ForeColor = System.Drawing.Color.Red;
                }

                LabelDealerListMessage.Visible = true;
            }

            LoadDealerList(); // ⚠️ 移出 using 區塊，確保連線已釋放後再執行
        }
    }
}