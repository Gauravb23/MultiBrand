using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace HSRP.Transaction
{
    public partial class AffixAddressUpdate : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string dealerid = string.Empty;
        string userid = string.Empty;
        string oemid = string.Empty;
        string query = string.Empty;
        DataTable dt = new DataTable();
        int i = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            oemid = Session["Oemid"].ToString();
            dealerid = Session["DealerID"].ToString();
            userid = Session["UID"].ToString();
            try
            {
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {

                }
            }
            catch
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                getdata();
                bindState();
            }
        }

        public void getdata()
        {
            string SQLString = "select Name,Vehicletype from oemmaster with(nolock) where oemid=" + oemid + "";
            DataTable dt = Utils.GetDataTable(SQLString, ConnectionString);
            if (dt.Rows.Count > 0)
            {
                lbloemname.Text = dt.Rows[0]["Name"].ToString();
                lblclass.Text = dt.Rows[0]["VehicleType"].ToString();
            }
            string SQLString2 = "select DealerName,concat(Address,city) as Address,dealercode,ContactPerson from dealermaster with(nolock) where dealerid=" + dealerid + "";
            DataTable dt2 = Utils.GetDataTable(SQLString2, ConnectionString);
            if (dt2.Rows.Count > 0)
            {
                lbldealername.Text = dt2.Rows[0]["DealerName"].ToString();
                lbldealercode.Text = dt2.Rows[0]["dealercode"].ToString();
                lbladdress.Text = dt2.Rows[0]["Address"].ToString();
                // lblcontectperson.Text = dt2.Rows[0]["ContactPerson"].ToString();
            }
            string SQLString3 = "select UserLoginName,EmailID,MobileNo from users with(nolock) where dealerid=" + dealerid + "";
            DataTable dt3 = Utils.GetDataTable(SQLString3, ConnectionString);
            if (dt3.Rows.Count > 0)
            {
                lbluserid.Text = dt3.Rows[0]["UserLoginName"].ToString();
                //lblemailid.Text = dt3.Rows[0]["EmailID"].ToString();
                //lblMobileNo.Text = dt3.Rows[0]["MobileNo"].ToString();

            }

            //string SQLString4 = "select DealerAffixationID from DealerAffixationCenter with(nolock) where dealerid=" + dealerid + "";
            //DataTable dt4 = Utils.GetDataTable(SQLString4, ConnectionString);
            //if (dt4.Rows.Count > 0)
            //{
            //    lblaff.Text = dt4.Rows[0]["DealerAffixationID"].ToString();
            //}
        }

        protected void bindState()
        {
            query = "select HSRPStateName, HSRP_StateId from HSRPState order by HSRPStateName asc";
            dt = Utils.GetDataTable(query,ConnectionString);
            ddlstate.DataSource = dt;
            ddlstate.DataBind();
            ddlstate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select State --", "0"));
        }

        //protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    query = "select Emb_Center_id, EmbCenterName from EmbossingCentersNew where State_ID = '" + ddlstate.SelectedValue + "' and embcentername not like '%Rej%' and EmbCenterName not like '%CW%' and EmbCenterName not like '%Covid%' and EmbCenterName not like '%OLA%' order by EmbCenterName asc";
        //    dt = Utils.GetDataTable(query, ConnectionString);
        //    ddlembcenter.DataSource = dt;
        //    ddlembcenter.DataBind();
        //    ddlembcenter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select State --", "0"));
        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if(ddlstate.SelectedValue == "0")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please select state!";
                lblsucmsg.Visible = false;
                return;
            }

            if (txtAddress.Text == "")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please enter address!";
                lblsucmsg.Visible = false;
                return;
            }
           
            if (txtCity.Text == "")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please enter city!";
                lblsucmsg.Visible = false;
                return;
            }     
            
            if (txtPincode.Text == "")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please enter pincode!";
                lblsucmsg.Visible = false;
                return;
            }

            if (txtContact.Text == "")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please enter contact person name!";
                lblsucmsg.Visible = false;
                return;
            }

            if (txtmobile.Text == "")
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Please enter mobile number!";
                lblsucmsg.Visible = false;
                return;
            }

            string strContact = txtmobile.Text.Trim().ToString();
            if (strContact.Length < 10)
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Mobile number cannot be less then 10 digits!";
                lblsucmsg.Visible = false;
                return;
            }

            string specialCharCE = @" \|{}%_!'#$%&'()+,./:;<=>?@<>@^£§€";
            
            if (txtmobile.Text.Trim() != "")
            {
                strContact = txtmobile.Text.Trim();
                foreach (var special in specialCharCE)
                {
                    if (strContact.Contains(special))
                    {
                        lblerrmsg.Visible = true;
                        lblsucmsg.Visible = false;
                        lblerrmsg.Text = "Special characters like (|,{,},%,_,!,',#,$,%,&,',(,),+,.,/,:,;,<,=,>,?,@,<,>,@,^,£,§,€) are not allowed in Mobile Number!";
                        return;
                    }
                }
            }
    
            query = "USP_AddAffixationAddressAllOem '" + lbldealername.Text + "','" + lbloemname.Text.ToString() + "','" + txtAddress.Text.ToString() + "','" + txtCity.Text.ToString() + "','" + ddlstate.SelectedItem.Text.ToString() + "','" + txtContact.Text.ToString() + "','" + txtmobile.Text.ToString() + "','" + txtPincode.Text.ToString() + "','" + txtLandmark.Text + "','" + ddlstate.SelectedValue + "','" + oemid + "','" + dealerid + "'";
            i = Utils.ExecNonQuery(query,ConnectionString);
            
            if(i > 0)
            {
                lblerrmsg.Visible = false;
                lblsucmsg.Visible = true;
                lblsucmsg.Text = "Added Successfully.";
                clearData();
                return;
            }
            else
            {
                lblerrmsg.Visible = true;
                lblerrmsg.Text = "Unable to add new address!";
                return;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transaction/NewCashReceiptDataEntry.aspx");
        }
        protected void clearData()
        {
            txtAddress.Text = "";
            txtCity.Text = "";
            txtContact.Text = "";
            txtmobile.Text = "";
            txtPincode.Text = "";
            txtLandmark.Text = "";
            ddlstate.ClearSelection();
        }
    }
}