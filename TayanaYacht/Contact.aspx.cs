using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace TayanaYacht
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropDownList();
                ClearLabelMessage();
            }

        }

        // 統一清除Label訊息
        private void ClearLabelMessage()
        {
            LabelName.Visible = false;
            LabelEmail.Visible = false;
            LabelPhone.Visible = false;
        }


        // 載入表單內2個下拉選單
        private void LoadDropDownList()
        {
            string sqlCountry = @"SELECT   Country.*
                                FROM     Country";
            string sqlBrochure = @"SELECT   Id, Name, IsActive
                                    FROM     Yachts
                                    WHERE   (IsActive = 1)";   

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommandCountry = new SqlCommand(sqlCountry, sqlConnection))
                {
                    using (SqlDataReader sqlData = sqlCommandCountry.ExecuteReader())
                    {
                        DropDownListCountry.DataSource = sqlData;
                        DropDownListCountry.DataTextField = "Name"; // 代表「使用者在畫面上會看到的文字」
                        DropDownListCountry.DataValueField = "Id"; // 代表「這個選項實際送出的值（value）」
                        DropDownListCountry.DataBind();

                    }
                }

                // ⚠️ 注意：DataReader 只能用一次，結束後要重新開 Reader

                using (SqlCommand sqlCommandBrochure = new SqlCommand(sqlBrochure, sqlConnection))
                {
                    using (SqlDataReader sqlData = sqlCommandBrochure.ExecuteReader())
                    {
                        DropDownListBrochure.DataSource = sqlData;
                        DropDownListBrochure.DataTextField = "Name"; // 代表「使用者在畫面上會看到的文字」
                        DropDownListBrochure.DataValueField = "Id"; // 代表「這個選項實際送出的值（value）」
                        DropDownListBrochure.DataBind();

                    }
                }
            }
            
        }



        // 待優化: 使用 Nuget 套件管理安裝 MailKit(微軟官網建議使用)
        // 建立寄信功能
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            // TODO: 收集資料
            string name = TextBoxName.Text;
            string email = TextBoxEmail.Text;
            string phone = TextBoxPhone.Text;
            int countryId = Convert.ToInt32(DropDownListCountry.SelectedValue);
            int brochureId = Convert.ToInt32(DropDownListBrochure.SelectedValue);
            string comments = TextBoxComments.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                LabelName.Text = "Required";
                LabelName.Visible = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                LabelEmail.Text = "Required";
                LabelEmail.Visible = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                LabelPhone.Text = "Required";
                LabelPhone.Visible = true;
                return;
            }

            string sql = @"
              INSERT INTO ContactRecord
              (Name, Email, Phone, CountryId, BrochureId, Comments)
              VALUES  (@name, @email, @phone, @countryId, @brochureId, @comments)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@name", name.Trim());
                    sqlCommand.Parameters.AddWithValue("@email", email.Trim());
                    sqlCommand.Parameters.AddWithValue("@phone", phone.Trim());
                    sqlCommand.Parameters.AddWithValue("@countryId", countryId);
                    sqlCommand.Parameters.AddWithValue("@brochureId", brochureId);
                    sqlCommand.Parameters.AddWithValue("@comments", comments.Trim());

                    sqlConnection.Open();

                    sqlCommand.ExecuteNonQuery();
                }
            }


            // TODO: 寄出信件
            string body = $@"
            <b>Name:</b> {name}<br/>
            <b>Email:</b> {email}<br/>
            <b>Phone:</b> {phone}<br/>
            <b>Country:</b> {DropDownListCountry.SelectedItem.Text}<br/>
            <b>Brochure:</b> {DropDownListBrochure.SelectedItem.Text}<br/>
            <b>Comments:</b> {comments}<br/>
            ";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("kelly556320@gmail.com"); // 寄件者（需是SMTP設定的帳號）
            mail.To.Add("kellychiang123@gmail.com"); // 收件者
            mail.Subject = "【Tayana 聯絡表單】有新留言 ";
            mail.Body = body;
            mail.IsBodyHtml = true; //m這封信的內容是 HTML 格式，而不是純文字。

            // 建立 SMTP 用來寄信
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // SMTP 伺服器主機（這裡是 Gmail）
            smtp.Port = 587; // Gmail 用 587 這個 Port（支援加密）
            smtp.EnableSsl = true; // 啟用 SSL 加密（很重要）


            // 登入帳號密碼（使用寄件者的帳密）
            smtp.Credentials = new NetworkCredential("kelly556320@gmail.com", "btgx xiyy aryc wawz");

            // 寄出信件
            smtp.Send(mail);

            TextBoxName.Text = "";
            TextBoxEmail.Text = "";
            TextBoxPhone.Text = "";
            DropDownListCountry.SelectedIndex = 1;
            DropDownListBrochure.SelectedIndex = 1;
            TextBoxComments.Text = "";

        }
    }
}
