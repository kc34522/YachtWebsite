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
    public partial class YachtsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadYachtList();
            }

            LabelGridViewYacht.Visible = false;

        }

        // 載入YachtList GridView
        private void LoadYachtList()
        {
            string sql = @"SELECT   YachtID, ModelName, IsNewBuilding, IsNewDesign, IsActive
FROM     Yachts Order by IsNewBuilding desc, IsNewDesign desc, ModelName";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.HasRows)
                        {
                            GridViewYachtList.DataSource = sqlDataReader;
                            GridViewYachtList.DataBind();
                        }
                    }
                }
            }
        }

        // YachtList GridView: 刪除按鈕
        protected void GridViewYachtList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int indexRow = e.RowIndex;
            int yachtId = Convert.ToInt32(GridViewYachtList.DataKeys[indexRow].Value);

            string sql = @"DELETE FROM Yachts
WHERE   (YachtID = @YachtID)";

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@YachtID", yachtId);
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery(); // 優化: 判斷比數

                }
            }

            LoadYachtList();

            LabelGridViewYacht.Text = "刪除成功!";
            LabelGridViewYacht.ForeColor = System.Drawing.Color.Blue;
            LabelGridViewYacht.Visible = true;
        }
    }
}