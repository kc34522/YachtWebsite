using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCrypt.Net;

namespace TayanaYacht.Admin
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["AdminId"] != null)
                {
                    Response.Redirect("YachtsList.aspx"); // 或你預設的後台首頁
                }
            }

            LabelLoginMessage.Visible = false;
        }

        // 登入按鈕
        protected void ButtonLogin_Click(object sender, EventArgs e)
        {
            string userName = TextBoxUserName.Text;
            string password = TextBoxPassword.Text;
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                LabelLoginMessage.Text = "請輸入帳號與密碼!";
                LabelLoginMessage.ForeColor = System.Drawing.Color.Red;
                LabelLoginMessage.Visible = true;
                return;
            }

            string sql = @"SELECT   Id, PasswordHash, DisplayName, Role
                                FROM     [User]
                                WHERE   UserName = @userName AND IsActive = 1";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@userName", userName);
                    int userId = 0;
                    string hashedPassword = null;
                    string displayName = null;
                    string userRole = null;

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        // 讀取資料
                        if (sqlDataReader.Read())
                        {
                            userId = (int)sqlDataReader["Id"];
                            hashedPassword = sqlDataReader["PasswordHash"].ToString();
                            displayName = sqlDataReader["DisplayName"].ToString();
                            userRole = sqlDataReader["Role"].ToString();
                        }
                    }

                    // 驗證用戶
                    if (userId > 0)
                    {
                        // 使用 BCrypt 驗證輸入的密碼是否與資料庫中的 Hash 匹配
                        if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                        {
                            // 更新 LastLoginAt
                            UpdateLastLoginTime(userId, sqlConnection);

                            // 設定Sesion
                            // session只需存userId, 其他是多餘的, 因為userId有值就代表login是true, userId也能透過id搜尋, 只要登出就給它NULL
                            Session["AdminId"] = userId;
                            Session["AdminName"] = displayName;
                            Session["AdminRole"] = userRole;

                            Response.Redirect("YachtsList.aspx");
                        }
                        else
                        {
                            LabelLoginMessage.Text = "您輸入的帳號或密碼有誤!";
                            LabelLoginMessage.ForeColor = System.Drawing.Color.Red;
                            LabelLoginMessage.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        LabelLoginMessage.Text = "您輸入的帳號或密碼有誤!";
                        LabelLoginMessage.ForeColor = System.Drawing.Color.Red;
                        LabelLoginMessage.Visible = true;
                        return;
                    }
                }
            }
            TextBoxUserName.Text = "";
            TextBoxPassword.Text = "";
        }

        // 更新 LastLoginAt 的方法
        private void UpdateLastLoginTime(int userId, SqlConnection connection)
        {
            string sql = @"Update [User] SET LastLoginAt = GetDate() WHERE Id = @Id";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", userId);
                command.ExecuteNonQuery();
            }
        }
    }
}