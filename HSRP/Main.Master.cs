using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP
{
    public partial class Main : System.Web.UI.MasterPage
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string StateID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/LogOut.aspx", true);
                }
                else
                {
                    if (Session["Dealerid"] != null && Session["oemid"] != null)
                    {
                        if (Session["Dealerid"].ToString() != "" && Session["Dealerid"].ToString() != "0" && Session["oemid"].ToString() != "" && Session["oemid"].ToString() != "0")
                        {
                            StateID = Session["UserHSRPStateID"].ToString();
                            //lblmobile.Text = Session["MobileNo"].ToString();
                            //lblEmail.Text = Session["EmailID"].ToString();
                            String UserID = string.Empty;
                            UserID = Session["UID"].ToString();
                            if (firstLoginStatus() == "Y")
                            {
                                Response.Redirect("~/Firstloginchangepassword.aspx");
                            }
                            lblUser.Text = Session["UserName"].ToString();
                            lbldatetime.Text = System.DateTime.Now.ToString();

                            string mac = Session["MacAddress"].ToString();
                            Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + Session["UID"].ToString() + "', LastLoggedInDatetime =GetDate() where MacAddress='" + Session["MacAddress"].ToString() + "'", CnnString);
                        }
                        else if ((Session["Dealerid"].ToString() == "" || Session["Dealerid"].ToString() == "0") && Session["oemid"].ToString() != "" && Session["oemid"].ToString() != "0")
                        {
                            Response.Redirect("~/LiveReports/Oemdashboard.aspx");
                        }
                    }
                }
            }
        }
        private string firstLoginStatus()
        {
            string status = Utils.getScalarValue("Select top 1 firstloginstatus from users where userid='" + Session["UID"].ToString() + "'", CnnString);
            return status;
        }
        protected void buttonLogout_Click(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                string login = "~/LogOut.aspx";
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);
            }
            else
            {
                string login = string.Empty;
                string wmac = Session["macbaseflag"].ToString();
                if (wmac == "N")
                {
                    login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                }
                else
                {
                    Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where MacAddress='" + Session["MacAddress"].ToString() + "'", CnnString);
                    login = "~/Login.aspx";
                }
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);
            }
        }

        protected void btnonlinepayment_Click(object sender, EventArgs e)
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

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}