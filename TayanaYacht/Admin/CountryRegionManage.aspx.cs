using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.Admin
{
    public partial class CountryRegionManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountryList();
                LoadRegionList(0);
            }

            int countryId = string.IsNullOrEmpty(ddlCountry.SelectedValue) ? 0 : Convert.ToInt32(ddlCountry.SelectedValue);
            if (countryId == 0)
            {
                PanelAddRegion.Visible = false;
            }
            else
            {
                PanelAddRegion.Visible = true;
            }
            ClearMessages();

        }

        // 統一清除Label訊息
        private void ClearMessages()
        {
            LabelAddCountry.Visible = false;
            LabelCountryList.Visible = false;
            LabelRegionList.Visible = false;
            LabelAddRegion.Visible = false;
        }


        // 載入國家列表 & 國家下拉選單
        private void BindCountryList()
        {
            string sql = @"SELECT   *
                            FROM     Country
                            ORDER BY Name;";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        DataTable dataTable = new DataTable();
                        dataTable.Load(sqlDataReader);

                        GridViewCountry.DataSource = dataTable;
                        GridViewCountry.DataBind();

                        ddlCountry.DataSource = dataTable;
                        ddlCountry.DataTextField = "Name";
                        ddlCountry.DataValueField = "Id";
                        ddlCountry.DataBind();
                        ddlCountry.Items.Insert(0, new ListItem("選擇一國家", "0"));  // 預設選項
                    }
                }
            }
        }

        // 下拉選單篩選時自動刷新Dealer清單
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            int countryId = string.IsNullOrEmpty(ddlCountry.SelectedValue) ? 0 : Convert.ToInt32(ddlCountry.SelectedValue);

            LoadRegionList(countryId);
        }



        // 載入地區列表(根據國家下拉選單)
        private void LoadRegionList(int countryId)
        {
            string sql = "SELECT Id, Name FROM Region WHERE (CountryId = @CountryId) ORDER BY Name";
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@CountryId", countryId);
                conn.Open();
                GridViewRegion.DataSource = cmd.ExecuteReader();
                GridViewRegion.DataBind();
            }
        }

        // 國家GridView-編輯按鈕
        protected void GridViewCountry_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewCountry.EditIndex = e.NewEditIndex;
            BindCountryList();
        }

        // 國家GridView-取消按鈕
        protected void GridViewCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewCountry.EditIndex = -1;
            BindCountryList();
        }

        // 國家GridView-更新按鈕
        protected void GridViewCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int indexRow = e.RowIndex;

            GridViewRow targetRow = GridViewCountry.Rows[indexRow];

            TextBox textBox = targetRow.FindControl("TextBoxCountry") as TextBox;

            string countryName = textBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(countryName))
            {
                LabelCountryList.Text = "國家名稱請勿空白!";
                LabelCountryList.ForeColor = System.Drawing.Color.Red;
                LabelCountryList.Visible = true;
                return;
            }

            string idKey = GridViewCountry.DataKeys[indexRow].Value.ToString();



            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();

                string checkSql = @"SELECT   count(*)
                            FROM     Country where Lower(Name) = Lower(@countryName) AND ID <> @id;";

                using (SqlCommand checkCommand = new SqlCommand(checkSql, sqlConnection))
                {
                    checkCommand.Parameters.AddWithValue("@countryName", countryName);
                    checkCommand.Parameters.AddWithValue("@id", idKey);

                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        LabelCountryList.Text = "該國家已存在，請勿重複新增!";
                        LabelCountryList.ForeColor = System.Drawing.Color.Red;
                        LabelCountryList.Visible = true;
                        return;
                    }
                }

                string updateSql = @"UPDATE  Country
                            SET        Name = @CountryName
                            WHERE Id = @Id";

                using (SqlCommand sqlCommand = new SqlCommand(updateSql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@CountryName", countryName);
                    sqlCommand.Parameters.AddWithValue("@Id", idKey);

                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewCountry.EditIndex = -1;
            BindCountryList();
            LabelCountryList.Text = "更新成功!";
            LabelCountryList.ForeColor = System.Drawing.Color.Blue;
            LabelCountryList.Visible = true;
        }

        // 國家GridView-刪除按鈕
        protected void GridViewCountry_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int indexRow = e.RowIndex;

            string idKey = GridViewCountry.DataKeys[indexRow].Value.ToString();

            string sql = @"DELETE FROM Country
                            WHERE   (Id = @Id)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", idKey);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewCountry.EditIndex = -1;
            BindCountryList();
            LabelCountryList.Text = "刪除成功!";
            LabelCountryList.ForeColor = System.Drawing.Color.Blue;
            LabelCountryList.Visible = true;
        }



        // 新增國家按鈕
        protected void ButtonAddCountry_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxAddCountry.Text))
            {
                LabelAddCountry.Text = "請輸入國家名稱!";
                LabelAddCountry.ForeColor = System.Drawing.Color.Red;
                LabelAddCountry.Visible = true;
                return;
            }

            string countryName = TextBoxAddCountry.Text.Trim();

            string checkSql = @"SELECT   count(*)
                            FROM     Country where Lower(Name) = Lower(@countryName);";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand checkCommand = new SqlCommand(checkSql, sqlConnection))
                {
                    checkCommand.Parameters.AddWithValue("@countryName", countryName);

                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        LabelAddCountry.Text = "該國家已存在，請勿重複新增!";
                        LabelAddCountry.ForeColor = System.Drawing.Color.Red;
                        LabelAddCountry.Visible = true;
                        return;
                    }
                }

                string insertSql = @"INSERT INTO Country
                                                          (Name)
                                            VALUES  (@countryName)";

                using (SqlCommand insertCommand = new SqlCommand(insertSql, sqlConnection))
                {
                    insertCommand.Parameters.AddWithValue("@countryName", countryName);

                    insertCommand.ExecuteNonQuery();
                }


                LabelAddCountry.Text = "新增成功!";
                LabelAddCountry.ForeColor = System.Drawing.Color.Blue;
                LabelAddCountry.Visible = true;
                TextBoxAddCountry.Text = "";
                BindCountryList();

            }
        }

        // 清除按鈕
        protected void ButtonCancelCountry_Click(object sender, EventArgs e)
        {
            TextBoxAddCountry.Text = "";
        }

        // 地區GridView-編輯按鈕
        protected void GridViewRegion_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewRegion.EditIndex = e.NewEditIndex;

            int countryId = string.IsNullOrEmpty(ddlCountry.SelectedValue) ? 0 : Convert.ToInt32(ddlCountry.SelectedValue);

            LoadRegionList(countryId);
        }

        // 地區GridView-取消按鈕
        protected void GridViewRegion_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewRegion.EditIndex = -1;
            int countryId = string.IsNullOrEmpty(ddlCountry.SelectedValue) ? 0 : Convert.ToInt32(ddlCountry.SelectedValue);

            LoadRegionList(countryId);

        }

        // 地區GridView-更新按鈕
        protected void GridViewRegion_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int indexRow = e.RowIndex;

            GridViewRow targetRow = GridViewRegion.Rows[indexRow];

            TextBox textBox = targetRow.FindControl("TextBoxRegion") as TextBox;

            string regionName = textBox.Text.Trim();

            string idKey = GridViewRegion.DataKeys[indexRow].Value.ToString();

            int countryId = Convert.ToInt32(ddlCountry.SelectedValue);


            if (string.IsNullOrWhiteSpace(regionName))
            {
                LabelRegionList.Text = "請輸入地區名稱!";
                LabelRegionList.ForeColor = System.Drawing.Color.Red;
                LabelRegionList.Visible = true;
                return;
            }

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();


                string sqlCount = @"select Count(*) from Region where lower(Name) = lower(@regionName) AND CountryId = @countryId AND Id <> @id";

                using (SqlCommand countCommand = new SqlCommand(sqlCount, sqlConnection))
                {

                    countCommand.Parameters.AddWithValue("@regionName", regionName);
                    countCommand.Parameters.AddWithValue("@countryId", countryId);
                    countCommand.Parameters.AddWithValue("@id", idKey);


                    int count = (int)countCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        LabelRegionList.Text = "該地區已存在，請勿重複新增!";
                        LabelRegionList.ForeColor = System.Drawing.Color.Red;
                        LabelRegionList.Visible = true;
                        return;
                    }
                }

                string sqlUpdate = @"UPDATE  Region
                            SET        Name = @regionName
                            WHERE   (Id = @regionId)";


            
                using(SqlCommand sqlCommand = new SqlCommand(sqlUpdate, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@regionName", regionName);
                    sqlCommand.Parameters.AddWithValue("@regionId", idKey);
                    sqlCommand.ExecuteNonQuery();

                }
            }

            GridViewRegion.EditIndex = -1;

            LabelRegionList.Text = "更新成功!";
            LabelRegionList.ForeColor = System.Drawing.Color.Blue;
            LabelRegionList.Visible = true;

            LoadRegionList(countryId);

        }

        // 地區GridView-刪除按鈕
        protected void GridViewRegion_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int rowIndex = e.RowIndex;
            string idKey = GridViewRegion.DataKeys[rowIndex].Value.ToString();
            string sql = @"DELETE FROM Region
                        WHERE   (Id = @regionId)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using(SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@regionId", idKey);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }

            GridViewRegion.EditIndex = -1;

            LabelRegionList.Text = "刪除成功!";
            LabelRegionList.ForeColor = System.Drawing.Color.Blue;
            LabelRegionList.Visible = true;

            int countryId = Convert.ToInt32(ddlCountry.SelectedValue);
            LoadRegionList(countryId);

        }

        // 新增地區按鈕
        protected void ButtonAddRegion_Click(object sender, EventArgs e)
        {
            string regionName = TextBoxAddRegion.Text.Trim();
            if (string.IsNullOrWhiteSpace(regionName))
            {
                LabelAddRegion.Text = "請輸入地區名稱!";
                LabelAddRegion.ForeColor = System.Drawing.Color.Red;
                LabelAddRegion.Visible = true;
                return;
            }
            int countryId = Convert.ToInt32(ddlCountry.SelectedValue);

            

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();

                string sqlCount = @"select Count(*) from Region where lower(Name) = lower(@regionName) AND CountryId = @countryId";

                using (SqlCommand countCommand = new SqlCommand(sqlCount,sqlConnection))
                {

                    countCommand.Parameters.AddWithValue("@regionName", regionName);
                    countCommand.Parameters.AddWithValue("@countryId", countryId);

                    int count = (int)countCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        LabelAddRegion.Text = "該地區已存在，請勿重複新增!";
                        LabelAddRegion.ForeColor = System.Drawing.Color.Red;
                        LabelAddRegion.Visible = true;
                        return;
                    }
                }

                string sqlInsert = @"INSERT INTO Region
              (CountryId, Name)
                VALUES  (@countryId, @regionName)";

                using (SqlCommand insertCommand = new SqlCommand(sqlInsert, sqlConnection))
                {
                    insertCommand.Parameters.AddWithValue("@countryId", countryId);
                    insertCommand.Parameters.AddWithValue("@regionName", regionName);

                    insertCommand.ExecuteNonQuery();
                }

                LabelAddRegion.Text = "新增成功!";
                LabelAddRegion.ForeColor = System.Drawing.Color.Blue;
                LabelAddRegion.Visible = true;

                TextBoxAddRegion.Text = "";

                LoadRegionList(countryId);

            }
        }

        // 清空地區按鈕
        protected void ButtonCancelRegion_Click(object sender, EventArgs e)
        {
            TextBoxAddRegion.Text = "";
        }
    }
}