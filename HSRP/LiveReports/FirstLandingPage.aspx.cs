using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using BarcodeLib;
using BarcodeLib.Barcode.ASP;
using BarcodeLib.Barcode;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace HSRP.LiveReports
{
    public partial class FirstLandingPage : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConnectionString);
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string ProductivityID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string VIP = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string dealerid = string.Empty;
        string macbase = string.Empty;
        string sql = string.Empty;
        string sql1 = string.Empty;
        string Dealeardetail = string.Empty;
        string totcoll = "0";
        decimal inttotcoll = 0;

        string accessType = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {
                    UserType = Session["UserType"].ToString();
                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            HSRPStateID = Session["UserHSRPStateID"].ToString();
            RTOLocationID = Session["UserRTOLocationID"].ToString();
            UserName = Session["UID"].ToString();
            USERID = Session["UID"].ToString();
            macbase = Session["MacAddress"].ToString();
            dealerid = Session["DealerID"].ToString();
            accessType = Session["accessType"].ToString();
            //Session["DealerChargingType"] = dtdealergstin.Rows[0]["ChargingType"].ToString(); 
            if (!Page.IsPostBack)
            {

                try
                {
                    string[] accessArray = accessType.Split(new Char[] {'^'});

                    for(int aind = 0; aind < accessArray.Length; aind++)
                    {
                        string accessValue = accessArray[aind];
                        if(accessValue == "A")
                        {
                            ADiv.Visible = true;
                        }

                        if (accessValue == "B")
                        {
                            //BDiv.Visible = true;
                        }

                        if (accessValue == "C")
                        {
                            //CDiv.Visible = true;
                        }
                        if (accessValue == "AA")
                        {
                            //DDiv.Visible = true;
                        }

                        
                    }

                    
                }
                catch(Exception ev)
                {

                }


            }

        }
       
        protected void redirect_Click(object sender, EventArgs e)
        {
            try
            {


                string strUrl = "";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            }
            catch (Exception ex)
            {

            }

        }

       
    }
}