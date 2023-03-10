using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using HSRP;
using System.Data;
using DataProvider;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace HSRP.Transaction
{
    public partial class LaserReceivedDealerwise : System.Web.UI.Page
    {
        string CnnString = string.Empty;
        string SQLString = string.Empty;
        string strUserID = string.Empty;
        string ComputerIP = string.Empty;
        string UserType = string.Empty;
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        int IResult;
        string sendURL = string.Empty;
        string SMSText = string.Empty;
        string SqlQuery = string.Empty;

        string AllLocation = string.Empty;
        string OrderStatus = string.Empty;
        int iChkCount = 0;
        string vehicle = string.Empty;
        DataProvider.BAL bl = new DataProvider.BAL();
        BAL obj = new BAL();
        string StickerManditory = string.Empty;

        string USERID = string.Empty;
        string dealerid = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx", true);
            }
            else
            {

                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                HSRPStateID = Session["UserHSRPStateID"].ToString();
                RTOLocationID = Session["UserRTOLocationID"].ToString();
                USERID = Session["UID"].ToString();
                dealerid = Session["DealerID"].ToString();

                lblErrMsg.Text = string.Empty;
                strUserID = Session["UID"].ToString();
                ComputerIP = Request.UserHostAddress;


                if (!IsPostBack)
                {
                    try
                    {
                        HSRPStateID = Session["UserHSRPStateID"].ToString();
                        RTOLocationID = Session["UserRTOLocationID"].ToString();
                        USERID = Session["UID"].ToString();
                        dealerid = Session["DealerID"].ToString();
                        ChallandropDown();

                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        public void ChallandropDown()
        {
            try
            {
                //and rtolocationID='" + RTOLocationID + "' 
                ///    Change by receiving Rtolocation by amit through by ravi  05-july-19
                SQLString = "USP_ReceivingEntryMultiBrand '"+ dealerid +"'";
                Utils.PopulateDropDownList(dropdownChallanNo, SQLString.ToString(), CnnString, "-- Select Challan No --");
            }
            catch (Exception e)
            {
                lblErrMsg.Text = e.Message.ToString();
            }
        }

        private void ShowGrid()
        {
            try
            {
                // and rtolocationID='" + RTOLocationID + "'
                if (dropdownChallanNo.SelectedItem.Text != "--Select Challan No--")
                {
                    string SQLString = "USP_ReceivingEntryGridMultiBrand '"+ dealerid +"','"+ dropdownChallanNo.SelectedValue.ToString() +"'";
                    dt = Utils.GetDataTable(SQLString, CnnString);

                    if (dt.Rows.Count > 0)
                    {
                        btnUpdate.Visible = true;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        GridView1.Visible = true;
                    }
                    else
                    {

                        lblErrMsg.Text = "Record Not Found";
                        btnUpdate.Visible = false;
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                    }
                }
                else
                {
                    lblErrMsg.Text = String.Empty;
                    lblErrMsg.Text = "Please Select Challan no.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
            }
        }
        StringBuilder sb = new StringBuilder();
        CheckBox chk;
        protected void CHKSelect1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
            if (chk1.Checked == true)
            {
                CheckBox chk2 = GridView1.HeaderRow.FindControl("CHKreject") as CheckBox;
                chk2.Enabled = false;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = true;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKreject1") as CheckBox;
                    chk.Enabled = false;
                }
            }
            else if (chk1.Checked == false)
            {
                CheckBox chk2 = GridView1.HeaderRow.FindControl("CHKreject") as CheckBox;
                chk2.Enabled = true;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Checked = false;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKreject1") as CheckBox;
                    chk.Enabled = true;
                }
            }

        }

       


        DataTable dt = new DataTable();

        protected void dropdownDuplicateFIle_SelectedIndexChanged(object sender, EventArgs e)
        {
            LblMessage.Text = "";
            lblErrMsg.Text = "";
            string SQLString = "select hsrprecordid,vehicleregno,hsrp_rear_lasercode,hsrp_Front_lasercode,OrderStatus from hsrprecords where OrderStatus='Embossing Done' and ChallanNo='" + dropdownChallanNo.SelectedItem.ToString() + "' and hsrp_StateID='" + HSRPStateID + "' and RecievedAffixationStatus='N' and  Dealerid = '" + dealerid + "'  ";
            dt = Utils.GetDataTable(SQLString, CnnString);
            ShowGrid();
            //and rtolocationID='" + RTOLocationID + "'
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            ShowGrid();

        }

        DataTable ds = new DataTable();
        StringBuilder sbinsert = new StringBuilder();
        StringBuilder sbupdate = new StringBuilder();

        protected void btnUpdate_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string rtolocationid = RTOLocationID;
                LblMessage.Text = "";
                lblErrMsg.Text = "";
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    CheckBox chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    CheckBox chkrej = GridView1.Rows[i].Cells[0].FindControl("CHKreject1") as CheckBox;
                    if (chk.Checked == true)
                    {
                        string strRecordId = GridView1.DataKeys[i]["hsrprecordid"].ToString();
                        string Query = "select vehicleregno from hsrprecords where hsrprecord = '"+ strRecordId +"'";
                        dt = Utils.GetDataTable(Query,CnnString);

                        string QueryHR = "select vehicleregno from hsrprecords_HR where hsrprecord = '" + strRecordId + "'";
                        DataTable dtHR = Utils.GetDataTable(QueryHR, CnnString);

                        if(dt.Rows.Count > 0)
                        {
                            sbupdate.Append("update hsrprecords set ReceivedAtAffixationCenterID ='" + RTOLocationID + "',RecievedAtAffixationDateTime=getdate(),RecievedAffixationCenterByUserID='" + strUserID + "',RecievedAffixationStatus='Y' where  hsrprecordid ='" + strRecordId + "' ;");
                        }

                        if(dtHR.Rows.Count > 0)
                        {
                            sbupdate.Append("update hsrprecords set ReceivedAtAffixationCenterID ='" + RTOLocationID + "',RecievedAtAffixationDateTime=getdate(),RecievedAffixationCenterByUserID='" + strUserID + "',RecievedAffixationStatus='Y' where  hsrprecordid ='" + strRecordId + "' ;");
                        }

                    }
                    if (chkrej.Checked == true)
                    {
                        string strRecordId = GridView1.DataKeys[i]["hsrprecordid"].ToString();
                        sbupdate.Append("update hsrprecords set hsrp_flag='R' where  hsrprecordid ='" + strRecordId + "' ;");
                    }
                }
                if (!string.IsNullOrEmpty(sbupdate.ToString().Trim()))
                {
                    IResult = Utils.ExecNonQuery(sbupdate.ToString(), CnnString);
                    if (IResult > 0)
                    {
                        LblMessage.Visible = true;
                        lblErrMsg.Text = "";
                        LblMessage.Text = "";

                        LblMessage.Text = "Received Successfully......";
                        ChallandropDown();
                        GridView1.Visible = false;
                        //ShowGrid();
                        //return;
                    }
                    else
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "";
                        lblErrMsg.Text = "Record Not Saved.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
            }
        }

        protected void CHKreject_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk1 = GridView1.HeaderRow.FindControl("CHKreject") as CheckBox;
            if (chk1.Checked == true)
            {
                CheckBox chk2 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
                chk2.Enabled = false;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKreject1") as CheckBox;
                    chk.Checked = true;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Enabled = false;
                }
            }
            else if (chk1.Checked == false)
            {
                CheckBox chk2 = GridView1.HeaderRow.FindControl("CHKSelect1") as CheckBox;
                chk2.Enabled = true;
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKreject1") as CheckBox;
                    chk.Checked = false;
                    chk = GridView1.Rows[i].Cells[0].FindControl("CHKSelect") as CheckBox;
                    chk.Enabled = true;
                }
            }
        }
    }
}