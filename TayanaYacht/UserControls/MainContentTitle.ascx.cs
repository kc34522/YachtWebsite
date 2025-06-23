using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht.UserControls
{
    public partial class MainContentTitle : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string menuType = Path.GetFileName(Request.Path).ToLower();

            LoadContentTitle(menuType);
        }

        public string ContentTitle { get; set; }


        private void LoadContentTitle(string menuType)
        {
            string id = Request.QueryString["Id"];
            switch (menuType)
            {
                case "yachts":
                    ContentTitle = "XXX";
                    //SqlConnection sqlConnection = new SqlConnection();
                    break;

                case "news":
                    ContentTitle = "News & Events";
                    break;

                case "company":
                    if (!string.IsNullOrWhiteSpace(id))
                    {
                        if (id == "2")
                        {
                            ContentTitle = "Certificate";
                        }


                    }
                    else
                    {
                        ContentTitle = "About Us";
                    }

                    break;

                case "dealers":
                    ContentTitle = "XXX";
                    //SqlConnection sqlConnection = new SqlConnection();
                    break;

                case "contact":
                    ContentTitle = "Contact";
                    break;

                    //default:
            }

        }

    }
}