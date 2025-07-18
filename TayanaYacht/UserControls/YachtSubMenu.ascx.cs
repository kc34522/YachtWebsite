using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.UserControls
{
    public partial class YachtSubMenu : System.Web.UI.UserControl
    {
        public string YachtId { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            DataBind(); // 為了讓 <%# %> 語法能綁定屬性
        }
    }
}