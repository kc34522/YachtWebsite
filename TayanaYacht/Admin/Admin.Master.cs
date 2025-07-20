using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminId"] != null && Session["AdminName"] != null)
            {
                LabelAdminName.Text = Session["AdminName"].ToString();
            }
            else
            {
                // 若沒有登入，導回登入頁
                Response.Redirect("Login.aspx");
            }
        }

        protected void ButtonLogout_Click(object sender, EventArgs e)
        {
            Session.Clear(); // 清除所有 Session
            Session.Abandon(); // 放棄目前的 Session（通知伺服器這個 Session 不再使用
            Response.Redirect("Login.aspx");
        }
    }
}