using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

namespace HSRP.Transaction
{
    public partial class FitterMobile1 : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string USERID = string.Empty;
        string specialChar = @"'€";
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (Session["UID"] == null)
            {
                Response.Redirect("/error.aspx", true);
            }
            USERID = Session["dealerid"].ToString();
            if (!Page.IsPostBack)
            {
               
                GridFitterMobileNumber();
            }
        }

        public void GridFitterMobileNumber()
        {
            try
            {
                //lblerrr.Visible = false;
                string sql = "select * from FitterMobile where DealerId='" + USERID + "'";
                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    grdid.DataSource = dt;
                    grdid.DataBind();
                }
                // txtfitterMobileno.Text = Fitternumber;
            }
            catch (Exception ex)
            {
                //lblerrr.Text = "Fitter mobile does not exists!";
                //lblerrr.Visible = true;
            }
        }
        //protected void buttonSave_Click(object sender, ImageClickEventArgs e)
        //{
        //    lblerr.Visible = false;
        //    if (txtFitterName.Text == "")
        //    {
        //        lblerr.Text = "Please enter name";
        //        lblerr.Visible = true;
        //        return;
        //    }
        //    if (txtFitterMobile.Text == "")
        //    {
        //        lblerr.Text = "Please enter mobile number";
        //        lblerr.Visible = true;
        //        return;
        //    }
        //    try
        //    {
        //        string sql = "AddFitterDetails '" + USERID + "','" + txtFitterName.Text + "','" + txtFitterMobile.Text + "'";
        //        DataTable dt = Utils.GetDataTable(sql, ConnectionString);
        //        if (dt.Rows.Count > 0)
        //        {
        //            if (dt.Rows[0][0].ToString() == "1")
        //            {
        //                GridFitterMobileNumber();
        //            }
        //            else
        //            {
        //                lblerr.Text = dt.Rows[0][1].ToString();
        //                lblerr.Visible = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblerr.Text = ex.Message.ToString();
        //        lblerr.Visible = true;
        //    }
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            
            lblerr.Visible = false;
            if (txtFitterName.Text == "")
            {
                lblerr.Text = "Please enter name!";
                lblerr.Visible = true;
                return;
            }
            string strFitterName = txtFitterName.Text.Trim();
            foreach (var item in specialChar)
            {
                if (strFitterName.Contains(item))
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Special characters not allowed in fitter name!";
                    return;
                }
            }
            if (txtFitterMobile.Text == "")
            {
                lblerr.Text = "Please enter mobile number!";
                lblerr.Visible = true;
                return;
            }

            if (!isMobileNoValid(txtFitterMobile.Text.ToString()))
            {
                lblerr.Visible = true;
                lblerr.Text = "Please enter valid mobile number!";
                return;
            }

            try
            {
                string sql = "AddFitterDetails '" + USERID + "','" + txtFitterName.Text + "','" + txtFitterMobile.Text + "'";
                DataTable dt = Utils.GetDataTable(sql, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        GridFitterMobileNumber();
                        txtFitterMobile.Text = "";
                        txtFitterName.Text = "";
                    }
                    else
                    {
                        lblerr.Text = dt.Rows[0][1].ToString();
                        lblerr.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message.ToString();
                lblerr.Visible = true;
            }
        }
        protected void grdid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                lblerr.Visible = false;
                Label lblfitterid = (Label)grdid.Rows[e.RowIndex].FindControl("lblfitterid");
                Label lbldealerid = (Label)grdid.Rows[e.RowIndex].FindControl("lbldealerid");
                TextBox txtFitterName = (TextBox)grdid.Rows[e.RowIndex].FindControl("txtgrdFitterName");
                TextBox txtMobileNo = (TextBox)grdid.Rows[e.RowIndex].FindControl("txtgrdFitterMobile");
                if (txtFitterName.Text == "")
                {
                    lblerr.Text = "Please enter fitter name!";
                    lblerr.Visible = true;
                    return;
                }
                string strFitterName = txtFitterName.Text.Trim();
                foreach (var item in specialChar)
                {
                    if (strFitterName.Contains(item))
                    {
                        lblerr.Visible = true;
                        lblerr.Text = "Special characters not allowed in fitter name!";
                        return;
                    }
                }
                if (txtMobileNo.Text == "")
                {
                    lblerr.Text = "Please enter mobile number!";
                    lblerr.Visible = true;
                    return;
                }
                
                if (!isMobileNoValid(txtMobileNo.Text.ToString()))
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Please enter valid mobile number!";
                    return;
                }

                string SQLString = "UpdateFitterDetails '" + lblfitterid.Text + "','" + lbldealerid.Text + "','" + txtFitterName.Text + "','" + txtMobileNo.Text + "'";
                //string SQLString = "update fittermobile set fittername='" + txtFitterName.Text + "', FitterMobile='" + txtMobileNo.Text + "' where fitterid='" + lblfitterid.Text + "'";
                DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        //GridFitterMobileNumber();
                    }
                    else
                    {
                        lblerr.Text = dt.Rows[0][1].ToString();
                        lblerr.Visible = true;
                    }
                }
                grdid.EditIndex = -1;
                this.GridFitterMobileNumber();
            }
            catch (Exception ex)
            {
                lblerr.Text = ex.Message.ToString();
            }
        }
        protected void grdid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdid.EditIndex = -1;
            this.GridFitterMobileNumber();
        }
        protected void grdid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdid.EditIndex = e.NewEditIndex;
            this.GridFitterMobileNumber();
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

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("../LiveReports/LiveTracking.aspx");
        }
    }
}