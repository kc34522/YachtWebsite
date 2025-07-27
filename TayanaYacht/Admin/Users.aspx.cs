using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminRole"] == null || Session["AdminRole"].ToString() != "SuperAdmin")
            {
                Response.Redirect("YachtsList.aspx");
            }

            if (!IsPostBack)
            {
                GetUserList();
                ResetForm();
            }

            LabelPanelMessage.Visible = false;
            LabelGridView.Visible = false;
        }

        // 載入使用者列表
        private void GetUserList()
        {
            string sql = @"SELECT   Id, DisplayName, UserName, Role, IsActive, LastLoginAt
FROM     [User]";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            GridViewUser.DataSource = sqlDataReader;
                            GridViewUser.DataBind();
                        }
                    }
                }
            }
        }

        // GridView 的命令事件處理
        protected void GridViewUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditUser")
            {
                int userId = Convert.ToInt32(e.CommandArgument);
                LoadUserForEditing(userId);
                LabelTitle.Text = "編輯使用者";
            }
            else if (e.CommandName == "DeleteUser")
            {
                string currentUserId = Session["AdminId"].ToString();
                string userId = e.CommandArgument.ToString();

                // 【核心保護機制】檢查是否在刪除自己
                if (currentUserId == userId)
                {
                    LabelGridView.Text = "";
                    LabelGridView.ForeColor = Color.Red;
                    LabelGridView.Visible = true;
                    return;
                }

                string sql = @"DELETE FROM [User]
WHERE   (Id = @userId)";

                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@userId", userId);
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery(); // 優化: 判斷比數

                    }
                }

                GetUserList();

                LabelGridView.Text = "刪除成功!";
                LabelGridView.ForeColor = System.Drawing.Color.Blue;
                LabelGridView.Visible = true;
            }
        }

        // 【核心】將使用者資料載入到右側表單以供編輯
        private void LoadUserForEditing(int userId)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM [User] WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", userId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 將資料填入表單
                            HiddenFieldUserId.Value = reader["Id"].ToString();
                            TextBoxUserName.Text = reader["UserName"].ToString();
                            TextBoxDisplayName.Text = reader["DisplayName"].ToString();
                            TextBoxEmail.Text = reader["Email"].ToString();
                            DropDownListRole.SelectedValue = reader["Role"].ToString();
                            CheckBoxIsActive.Checked = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]);

                            // 顯示唯讀資訊
                            infoRow.Visible = true;
                            LiteralCreatedAt.Text = Convert.ToDateTime(reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm");
                            LiteralLastLoginAt.Text = reader["LastLoginAt"] == DBNull.Value ? "從未登入" : Convert.ToDateTime(reader["LastLoginAt"]).ToString("yyyy-MM-dd HH:mm");

                            // 更新 UI 狀態為「編輯模式」
                            LabelTitle.Text = "編輯用戶：" + reader["DisplayName"].ToString();
                            ButtonSave.Text = "更新";

                            // 保護機制：僅對自己的帳號禁用角色和啟用狀態
                            bool isCurrentUser = (userId.ToString() == Session["AdminId"].ToString());
                            DropDownListRole.Enabled = !isCurrentUser;
                            CheckBoxIsActive.Disabled = isCurrentUser;
                        }
                    }
                }
            }
        }


        // 儲存按鈕
        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            bool isNewUser = string.IsNullOrEmpty(HiddenFieldUserId.Value);
            if (string.IsNullOrWhiteSpace(TextBoxUserName.Text))
            {
                ShowMessage("帳號為必填。", Color.Red);
                return;
            }

            // 如果是新增用戶，密碼為必填
            if (isNewUser && string.IsNullOrWhiteSpace(TextBoxPassword.Text))
            {
                ShowMessage("新增用戶時，密碼為必填。", Color.Red);
                return;
            }

            // 檢查兩次密碼輸入是否一致
            if (TextBoxPassword.Text != TextBoxConfirmPassword.Text)
            {
                ShowMessage("兩次密碼輸入不一致。", Color.Red);
                return;
            }

            // 【核心檢查】在執行儲存前，檢查帳號是否已存在
            string userName = TextBoxUserName.Text.Trim();
            int userId = 0;
            if (!string.IsNullOrEmpty(HiddenFieldUserId.Value))
            {
                userId = Convert.ToInt32(HiddenFieldUserId.Value);
            }

            if (IsUserNameExists(userName, userId))
            {
                ShowMessage("錯誤：這個帳號名稱已經被使用了，請換一個。", Color.Red);
                return; // 中斷儲存操作
            }

            string sql;
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                if (isNewUser)
                {
                    sql= @"INSERT INTO [User]
              (UserName, PasswordHash, DisplayName, Email, Role, IsActive)
VALUES  (@UserName, @PasswordHash, @DisplayName, @Email, @Role, @IsActive)";
                    
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {

                        sqlConnection.Open();
                        sqlCommand.Parameters.AddWithValue("@UserName", TextBoxUserName.Text.Trim());
                        sqlCommand.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(TextBoxPassword.Text));
                        sqlCommand.Parameters.AddWithValue("@DisplayName", TextBoxDisplayName.Text.Trim());
                        sqlCommand.Parameters.AddWithValue("@Email", TextBoxEmail.Text.Trim());
                        sqlCommand.Parameters.AddWithValue("@Role", DropDownListRole.SelectedValue);
                        sqlCommand.Parameters.AddWithValue("@IsActive", CheckBoxIsActive.Checked);

                        sqlCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    // 動態組合 SQL 字串，只有在輸入新密碼時才更新 PasswordHash
                    if (!string.IsNullOrWhiteSpace(TextBoxPassword.Text))
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(TextBoxPassword.Text);
                        sql = @"UPDATE [User] SET UserName=@UserName, PasswordHash=@PasswordHash, DisplayName=@DisplayName, 
                            Email=@Email, Role=@Role, IsActive=@IsActive WHERE Id=@Id";
                    }
                    else
                    {
                        sql = @"UPDATE [User] SET UserName=@UserName, DisplayName=@DisplayName, 
                            Email=@Email, Role=@Role, IsActive=@IsActive WHERE Id=@Id";
                    }

                    using (SqlCommand cmd = new SqlCommand(sql, sqlConnection))
                    {
                        sqlConnection.Open();
                        cmd.Parameters.AddWithValue("@Id", HiddenFieldUserId.Value);
                        cmd.Parameters.AddWithValue("@UserName", TextBoxUserName.Text);
                        if (!string.IsNullOrWhiteSpace(TextBoxPassword.Text))
                        {
                            cmd.Parameters.AddWithValue("@PasswordHash", BCrypt.Net.BCrypt.HashPassword(TextBoxPassword.Text));
                        }
                        cmd.Parameters.AddWithValue("@DisplayName", TextBoxDisplayName.Text);
                        cmd.Parameters.AddWithValue("@Email", TextBoxEmail.Text);
                        cmd.Parameters.AddWithValue("@Role", DropDownListRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@IsActive", CheckBoxIsActive.Checked);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            GetUserList();
            ResetForm();
            ShowMessage("儲存成功！", Color.Blue);

        }


        // 新增編輯區清空
        private void ResetForm()
        {
            HiddenFieldUserId.Value = "";
            TextBoxUserName.Text = "";
            TextBoxPassword.Text = "";
            TextBoxConfirmPassword.Text = "";
            TextBoxDisplayName.Text = "";
            TextBoxEmail.Text = "";
            DropDownListRole.SelectedIndex = 0;
            CheckBoxIsActive.Checked = true;
            CheckBoxIsActive.Disabled = false;

            LabelTitle.Text = "新增用戶";
            ButtonSave.Text = "新增";

            infoRow.Visible = false;
        }

        // 訊息顯示
        private void ShowMessage(string message, Color color)
        {
            LabelPanelMessage.Text = message;
            LabelPanelMessage.ForeColor = color;
            LabelPanelMessage.Visible = true;
        }

        // 取消按鈕
        protected void ButtonCancel_Click(object sender, EventArgs e)
        {
       
                // 如果是空的，代表現在是「新增模式」
                // 行為：清空表單
                ResetForm();
                ShowMessage("已清空表單，可新增用戶!", Color.Blue);
            LabelTitle.Text = "新增使用者";


        }

        // 建立一個檢查帳號是否存在的輔助方法
        private bool IsUserNameExists(string userName, int currentUserId)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString;
            // 在編輯模式下，查詢時需要排除自己
            string sql = (currentUserId > 0)
                ? "SELECT COUNT(*) FROM [User] WHERE UserName = @UserName AND Id != @Id"
                : "SELECT COUNT(*) FROM [User] WHERE UserName = @UserName";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", userName);
                    if (currentUserId > 0)
                    {
                        // 只有在編輯模式下才需要傳入 Id 參數
                        cmd.Parameters.AddWithValue("@Id", currentUserId);
                    }

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar(); // ExecuteScalar 用於執行查詢並只返回結果集的第一行第一列
                    return (count > 0);
                }
            }
        }
    }
}