using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TayanaYacht
{
    public partial class Dealers : System.Web.UI.Page
    {
        //public int CurrentPage
        //{
        //    get
        //    {
        //        return ViewState["CurrentPage"] != null ? (int)ViewState["CurrentPage"] : 1;
        //    }
        //    set
        //    {
        //        ViewState["CurrentPage"] = value;
        //    }
        //}

        //int pageSize = 5;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int countryId;
                if(!string.IsNullOrWhiteSpace(Request.QueryString["Id"]) && int.TryParse(Request.QueryString["Id"], out countryId))
                {
                    LoadDealerList(countryId);

                }
                else
                {
                    countryId = GetFirstCountryId();
                    LoadDealerList(countryId);
                }
            }
        }


        // 抓左側選單第一個國家Id
        private int GetFirstCountryId()
        {
            int countryId;
            string sql = @"SELECT   Top 1 Country.Id
                            FROM     Country 
                            INNER JOIN Region ON Country.Id = Region.CountryId 
                            INNER JOIN Dealer ON Dealer.RegionId = Region.Id
                            WHERE   (Dealer.IsActive = 1)
                            ORDER BY Country.Name";
            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlConnection.Open();
                    countryId = (int)sqlCommand.ExecuteScalar();                   
                }
            }
            return countryId;
        }

        // 載入經銷商列表(Repeater)
        private void LoadDealerList(int countryId)
        {
            string sql = @"SELECT   Dealer.ImagePath, Region.Name AS RegionName, Dealer.Name AS DealerName, Dealer.Contact, Dealer.Address, Dealer.Tel, Dealer.Fax, Dealer.Cell, Dealer.Email, Dealer.Website
                        FROM     Dealer INNER JOIN
                                      Region ON Dealer.RegionId = Region.Id 
                        WHERE   (Dealer.IsActive = 1) AND (Region.CountryId = @CountryId)
                        ORDER BY RegionName, DealerName";

            

            using (SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["MyDb"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@CountryId", countryId);

                    sqlConnection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        RepeaterDealerList.DataSource = reader;
                        RepeaterDealerList.DataBind();
                    }
                }
            }
        }

        // 資料內容隱藏某些區塊PlaceHolder (每顯示一個項目，就會跑一次你寫的程式碼)
        protected void RepeaterDealerList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PlaceHolder phContact = (PlaceHolder)e.Item.FindControl("PlaceHolderContact");
                string contact = DataBinder.Eval(e.Item.DataItem, "Contact") as string;
                phContact.Visible = !string.IsNullOrWhiteSpace(contact);

                PlaceHolder phAddress = (PlaceHolder)e.Item.FindControl("PlaceHolderAddress");
                string address = DataBinder.Eval(e.Item.DataItem, "Address") as string;
                phAddress.Visible = !string.IsNullOrWhiteSpace(address);

                PlaceHolder phTel = (PlaceHolder)e.Item.FindControl("PlaceHolderTel");
                string tel = DataBinder.Eval(e.Item.DataItem, "Tel") as string;
                phTel.Visible = !string.IsNullOrWhiteSpace(tel);

                PlaceHolder phFax = (PlaceHolder)e.Item.FindControl("PlaceHolderFax");
                string fax = DataBinder.Eval(e.Item.DataItem, "Fax") as string;
                phFax.Visible = !string.IsNullOrWhiteSpace(fax);

                PlaceHolder phCell = (PlaceHolder)e.Item.FindControl("PlaceHolderCell");
                string cell = DataBinder.Eval(e.Item.DataItem, "Cell") as string;
                phCell.Visible = !string.IsNullOrWhiteSpace(cell);

                PlaceHolder phEmail = (PlaceHolder)e.Item.FindControl("PlaceHolderEmail");
                string email = DataBinder.Eval(e.Item.DataItem, "Email") as string;
                phEmail.Visible = !string.IsNullOrWhiteSpace(email);

                PlaceHolder phWebsite = (PlaceHolder)e.Item.FindControl("PlaceHolderWebsite");
                string website = DataBinder.Eval(e.Item.DataItem, "Website") as string;
                phWebsite.Visible = !string.IsNullOrWhiteSpace(website);
            }
        }

        

       
    }
}