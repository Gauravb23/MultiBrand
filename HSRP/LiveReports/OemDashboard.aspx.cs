using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.LiveReports
{
    public partial class OemDashboard : System.Web.UI.Page
    {

        string UserID = string.Empty;
        Utils bl = new Utils();
        string HSRPStateID = string.Empty;
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string userdealerid = string.Empty;
        string oemid = string.Empty;
        int UserType;
        string strOemType = string.Empty;

        int icount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!IsPostBack)
            {
                oemid = Session["oemid"].ToString();
                if (oemid == "7")
                {
                    HyundaiId.Visible = true;
                    DRId.Visible = true;
                    HRId.Visible = true;
                }
                else
                {
                    HyundaiId.Visible = false;
                    DRId.Visible = true;
                    HRId.Visible = true;
                }
                if(oemid == "272")
                {
                    divMISDealer.Visible = true;
                    divHero.Visible = true;
                    lihero.Visible = true;
                    divFitment.Visible = true;
                }
                else
                {
                    divMISDealer.Visible = false;
                    divHero.Visible = false;
                    lihero.Visible = false;
                    divFitment.Visible = false;

                }
                if (oemid == "23")
                {
                    other.Visible = false;
                    sml.Visible = true;
                }
                else
                {
                    other.Visible = true;
                    sml.Visible = false;
                }
                if (oemid == "1280")
                {
                    divmis.Visible = false;
                }
                else
                {                   
                    divmis.Visible = true;                    
                }
                if (Session["UID"] != null)
                {
                    UserID = Session["UID"].ToString();

                    string sqlOemQuery = "select top 1 oemtype from users where userid='" + UserID + "'";
                    DataTable dtOem = Utils.GetDataTable(sqlOemQuery, CnnString);

                    if (dtOem.Rows.Count > 0)
                    {
                       
                        strOemType = dtOem.Rows[0]["oemtype"].ToString();
                        if(strOemType=="Y")
                        {
                            MIS.Visible = false;
                        }
                        else
                        {
                            MIS.Visible = true;
                        }

                        if(oemid=="36")
                        {
                            RId.Visible = true;
                        }
                        else
                        {
                            RId.Visible = false;
                        }
                       
                    }
                  

                }

            }
        }
    }
}