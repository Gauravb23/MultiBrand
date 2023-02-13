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
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace HSRP.LiveReports
{
    public partial class LiveTracking : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        //static string dashboardCGUrl = ConfigurationManager.AppSettings["dashboardCGUrl"].ToString();
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
        string str = string.Empty;
        string query = string.Empty;
        string emailid = string.Empty;
        string strsubject = string.Empty;
        string dealername = string.Empty;
        int oemid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["UserType"] == null)
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
            dealername = Session["UserName"].ToString();
            oemid = Convert.ToInt32(Session["oemid"].ToString());

            if (!Page.IsPostBack)
            {

                PaymentOverDueMessage();
                
            }

        }
        public void PaymentOverDueMessage()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("PaymentOverdueMessage", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@dealerid", dealerid);
                cmd.Parameters.AddWithValue("@userid", USERID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    //paymentmsg.Text = dt.Rows[0]["Message"].ToString();
                }
            }
            catch (Exception ex)
            {
                //lblerrr.Text = "Fitter mobile does not exists!";
                //lblerrr.Visible = true;
            }
        }

        protected void redirect_Click(object sender, EventArgs e)
        {
            try
            {
                string strUrl = "https://utsavhsrponline.com/online/OnlinePaymentRazorPay.aspx?" + "UI=" + HttpUtility.UrlEncode(Encrypt(Session["UID"].ToString())) + "&DI=" + HttpUtility.UrlEncode(Encrypt(Session["DealerID"].ToString()));
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + strUrl + "','_blank')", true);
            }
            catch (Exception ex)
            {

            }

        }
        protected void ImageButton1_Click1(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Transaction/NewCashReceiptDataEntry.aspx");
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}