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
using System.Text.RegularExpressions;

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
            Query = "select RTOLocationID, (RTOLocationName +'  '+ '('+ NAVEMBID +')') as RTOLocationName from rtolocation where HSRP_StateID = '" + ddlstate.SelectedValue + "' and ActiveStatus = 'Y' and NAVEMBID not like 'CW%' and NAVEMBID not like '%Rej%' order by RTOLocationName asc";
            dt = Utils.GetDataTable(Query, ConnectionString);
            ddlec.DataSource = dt;
            ddlec.DataBind();
            ddlec.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select EC --", "0"));
        }

        protected void ShowGrid()
        {
            if(ddlstate.SelectedValue == "0")
            {
                grdview.Visible = false;
                llbMSGError.Visible = true;
                llbMSGError.Text = "Please select state!";
                llbMSGsucss.Visible = false;
                return;
            }
            if(ddlec.SelectedValue == "0")
            {
                grdview.Visible = false;
                llbMSGError.Visible = true;
                llbMSGError.Text = "Please select EC!";
                llbMSGsucss.Visible = false;
                return;
            }
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
                grdview.Visible = false;
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
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdview.EditIndex = e.NewEditIndex;
            ShowGrid();
        }
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {           
            Label id = grdview.Rows[e.RowIndex].FindControl("lblSno") as Label;
            int subdealerid = Convert.ToInt32(id.Text);
            TextBox name = grdview.Rows[e.RowIndex].FindControl("txtContactPerson") as TextBox;
            string strname = name.Text;
            TextBox number = grdview.Rows[e.RowIndex].FindControl("txtContactMobileNo") as TextBox;
            string strnumber = number.Text;

            if (strname == "")
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "Please enter name!";
                llbMSGsucss.Visible = false;
                return;
            }

            string specialChar = @"'";
            foreach (var item in specialChar)
            {
                if (strname.Contains(item))
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Special characters not allowed in name!";
                    llbMSGsucss.Visible = false;
                    return;
                }
            }

            if (strnumber == "")
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "Please enter mobile number!";
                llbMSGsucss.Visible = false;
                return;
            }

            if (!isMobileNoValid(strnumber))
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "Please enter valid mobile number!";
                llbMSGsucss.Visible = false;
                return;
            }


            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[USP_FitmentLocationreport_Update]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@id", subdealerid));
                cmd.Parameters.Add(new SqlParameter("@name", strname));
                cmd.Parameters.Add(new SqlParameter("@number", strnumber));
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                con.Close();                
                grdview.EditIndex = -1;
                llbMSGError.Visible = false;
                llbMSGsucss.Text = "Updated Successfully";
                llbMSGsucss.Visible = true;
                ShowGrid();
            }

        }
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdview.EditIndex = -1;
            ShowGrid();
        }

        protected Boolean isMobileNoValid(string mobileNo)
        {
            Regex panreg = new Regex(@"(^[1-9]{1}[0-9]{9}$)");
            if (!(mobileNo.Length == 10))
            {
                return false;
            }
            if (!panreg.IsMatch(mobileNo))
            {
                return false;
            }
            return true;
        }
    }
}