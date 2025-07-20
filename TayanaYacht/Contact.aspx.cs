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
            }
            ClearLabelMessage();

        }

        // 統一清除Label訊息
        private void ClearLabelMessage()
        {
            LabelName.Visible = false;
            LabelEmail.Visible = false;
            LabelPhone.Visible = false;
            LabelRecaptcha.Visible = false;
            LabelSubmit.Visible = false;
        }


        // 載入表單內2個下拉選單
        private void LoadDropDownList()
        {
            string sqlCountry = @"SELECT   Country.*
                                FROM     Country";
            string sqlBrochure = @"SELECT   YachtID, ModelName, IsActive
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
                        DropDownListBrochure.DataTextField = "ModelName"; // 代表「使用者在畫面上會看到的文字」
                        DropDownListBrochure.DataValueField = "YachtID"; // 代表「這個選項實際送出的值（value）」
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
                LabelName.Text = "Required.";
                LabelName.Visible = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                LabelEmail.Text = "Required.";
                LabelEmail.Visible = true;
                return;
            }
            if (string.IsNullOrWhiteSpace(phone))
            {
                LabelPhone.Text = "Required.";
                LabelPhone.Visible = true;
                return;
            }
            if (string.IsNullOrEmpty(RecaptchaWidget1.Response))
            {
                LabelRecaptcha.Text = "請勾選「我不是機器人」。";
                LabelRecaptcha.Visible = true;
                return;
            }
            else
            {
                var result = RecaptchaWidget1.Verify();
                if (!result.Success)
                {
                    LabelRecaptcha.Text = "驗證失敗，請重試!";
                    LabelRecaptcha.Visible = true;
                    return;
                }
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

            // --- 準備信件 1: 給網站管理員的通知信 ---
            string adminBody = $@"
            <p>您有一封新的網站表單留言：</p>
            <hr>
            <b>Name:</b> {name}<br/>
            <b>Email:</b> {email}<br/>
            <b>Phone:</b> {phone}<br/>
            <b>Country:</b> {DropDownListCountry.SelectedItem.Text}<br/>
            <b>Brochure:</b> {DropDownListBrochure.SelectedItem.Text}<br/>
            <b>Comments:</b> {comments}<br/>
            <hr>
            <p>這封信由系統自動發送。</p>";

            // --- 準備信件 2: 給使用者的自動回覆確認信 ---
            string userSubject = "【Tayana Yachts】我們已收到您的聯絡訊息";
            string userBody = $@"
        <p>親愛的 {name} 您好,</p>
        <p>感謝您與 Tayana Yachts 聯絡，我們已經成功收到您的訊息。<br>
        我們的團隊將會盡快與您聯繫。</p>
        <p>以下是您填寫的資訊備份：</p>
        <ul>
            <li><b>姓名:</b> {name}</li>
            <li><b>Email:</b> {email}</li>
            <li><b>電話:</b> {phone}</li>
            <li><b>國家:</b> {DropDownListCountry.SelectedItem.Text}</li>
            <li><b>感興趣的船型:</b> {DropDownListBrochure.SelectedItem.Text}</li>
            <li><b>備註:</b> {comments}</li>
        </ul>
        <hr>
        <p>敬祝 順心</p>
        <p><b>Tayana Yachts</b><br>
        <a href='http://www.tayanaworld.com/'>www.tayanaworld.com</a></p>";                       

            // 建立 SMTP 用來寄信
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; // SMTP 伺服器主機（這裡是 Gmail）
            smtp.Port = 587; // Gmail 用 587 這個 Port（支援加密）
            smtp.EnableSsl = true; // 啟用 SSL 加密（很重要）
            // 登入帳號密碼（使用寄件者的帳密）
            smtp.Credentials = new NetworkCredential("~~~~~~~~~~~~~~~~", "~~~~~~~~~~~~~~~~~~"); 

            try
            {
                // --- 寄送第一封信 (給管理員) ---
                MailMessage mailtoAdmin = new MailMessage();
                mailtoAdmin.From = new MailAddress("~~~~~~~~~~~~~~~~"); // 寄件者（需是SMTP設定的帳號）
                mailtoAdmin.To.Add("~~~~~~~~~~~~"); // 收件者
                mailtoAdmin.Subject = $"【Tayana 聯絡表單】有來自{name}的新留言 ";
                mailtoAdmin.Body = adminBody;
                mailtoAdmin.IsBodyHtml = true; //m這封信的內容是 HTML 格式，而不是純文字。
                                               // 寄出信件
                smtp.Send(mailtoAdmin);

                // --- 寄送第二封信 (給使用者) ---
                MailMessage mailToUser = new MailMessage();
                mailToUser.From = new MailAddress("~~~~~~~~~~~~~~~~", "Tayana Yachts"); // 可以加上寄件人名稱
                mailToUser.To.Add(email); // 收件者是填表單的人
                mailToUser.Subject = userSubject;
                mailToUser.Body = userBody;
                mailToUser.IsBodyHtml = true;
                smtp.Send(mailToUser);

                LabelSubmit.Text = "訊息已成功寄出！我們將盡快與您聯繫。";
                LabelSubmit.ForeColor = System.Drawing.Color.Blue;
                LabelSubmit.Visible = true;
            }
            catch
            {
                LabelSubmit.Text = "發生未預期的錯誤，請稍後再試。";
                LabelSubmit.ForeColor = System.Drawing.Color.Red;
                LabelSubmit.Visible = true;
            }          

            TextBoxName.Text = "";
            TextBoxEmail.Text = "";
            TextBoxPhone.Text = "";
            DropDownListCountry.SelectedIndex = 1;
            DropDownListBrochure.SelectedIndex = 1;
            TextBoxComments.Text = "";

        }
    }
}
