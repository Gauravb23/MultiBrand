using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class FitmentLocationReport : System.Web.UI.Page
    {
        string Query = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        int UserType;
        string strUserID = string.Empty;
        string HSRPStateID = string.Empty;
        string oemid = string.Empty;
        string dealerid = string.Empty;
        string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string ImgoemECUrl = string.Empty;
        string ImgoemCodoUrl = string.Empty;
        DataTable dt = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    UserType = Convert.ToInt32(Session["UserType"].ToString());                   
                    strUserID = Session["UID"].ToString();
                    oemid = Session["oemid"].ToString();
                    dealerid = Session["dealerid"].ToString();
                    if (!IsPostBack)
                    {
                        ddlStateBind();
                    }
                }
            }
            catch (Exception err)
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "Error on Page Load" + err.Message.ToString();
            }

        }

        protected void ddlStateBind()
        {
            Query = "Select HSRPStateName, HSRP_StateId from HSRPState order by HSRPStateName asc";
            dt = Utils.GetDataTable(Query, ConnectionString);
            ddlstate.DataSource = dt;
            ddlstate.DataBind();
            ddlstate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select State --", "0"));
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query = "select RTOLocationID, (RTOLocationName +'  '+ '('+ NAVEMBID +')') as RTOLocationName from rtolocation where HSRP_StateID = '" + ddlstate.SelectedValue + "' and NAVEMBID not like 'CW%' and NAVEMBID not like '%Rej%' order by RTOLocationName asc";
            dt = Utils.GetDataTable(Query, ConnectionString);
            ddlec.DataSource = dt;
            ddlec.DataBind();
            ddlec.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select EC --", "0"));            
        }

        protected void ShowGrid()
        {
            Query = "USP_FitmentLocationReportData '" + ddlstate.SelectedValue + "','" + ddlec.SelectedValue + "'";
            dt = Utils.GetDataTable(Query, ConnectionString);
            if (dt.Rows.Count > 0)
            {
                grdview.DataSource = dt;
                grdview.DataBind();
                grdview.Visible = true;
            }
            else
            {
                grdview.Visible = true;
                llbMSGError.Visible = true;
                llbMSGError.Text = "Record Not Found";
                return;
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            llbMSGError.Visible = false;
            ShowGrid();
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("../LiveReports/LiveTracking.aspx");
        }
    }
}