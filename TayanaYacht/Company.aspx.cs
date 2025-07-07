using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht
{
    public partial class Company : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadAboutUsContent();
        }
        private void LoadAboutUsContent()
        {
            string id = Request.QueryString["Id"];
            if(id != null && id.Trim() == "2")
            {
                string sqlCertificate = @"SELECT   [Content]
                            FROM     CompanyContent
                            WHERE   (PageName = N'Certificate')";
                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sqlCertificate, sqlConnection))
                    {
                        sqlConnection.Open();
                        object result = sqlCommand.ExecuteScalar();
                        if (result != null) // 如果有讀到資料
                        {
                            LiteralContent.Text = result.ToString();
                        }

                    }
                }
            }
            else
            {
                string sql = @"SELECT   [Content]
                            FROM     CompanyContent
                            WHERE   (PageName = N'AboutUs')";
                using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                    {
                        sqlConnection.Open();
                        object result = sqlCommand.ExecuteScalar();
                        if (result != null) // 如果有讀到資料
                        {
                            LiteralContent.Text = result.ToString();
                        }

                    }
                }
            }
                
        }
    }
}