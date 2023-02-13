using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace HSRP
{
    public partial class OEM : System.Web.UI.MasterPage
    {
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string StateID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["oemid"].ToString() == "1280")
                {
                    spkubota.Visible = true;
                }
                else
                {
                    spkubota.Visible = false;
                }
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/LogOut.aspx", true);
                }
                else
                {
                    if (Session["Dealerid"] != null && Session["oemid"] != null)
                    {
                        if ((Session["Dealerid"].ToString() == "" || Session["Dealerid"].ToString() == "0") && Session["oemid"].ToString() != "" && Session["oemid"].ToString() != "0")
                        {
                            StateID = Session["UserHSRPStateID"].ToString();
                            String UserID = string.Empty;
                            UserID = Session["UID"].ToString();
                            if (firstLoginStatus() == "Y")
                            {
                                Response.Redirect("~/Firstloginchangepassword.aspx");
                            }
                            lblUser.Text = Session["UserName"].ToString();
                            string mac = Session["MacAddress"].ToString();
                            Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + Session["UID"].ToString() + "', LastLoggedInDatetime =GetDate() where MacAddress='" + Session["MacAddress"].ToString() + "'", CnnString);
                        }
                        else if ((Session["Dealerid"].ToString() != "" || Session["Dealerid"].ToString() != "0") && Session["oemid"].ToString() != "" && Session["oemid"].ToString() != "0")
                        {
                            Response.Redirect("~/Livereports/LiveTracking.aspx");
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
        
    }
}