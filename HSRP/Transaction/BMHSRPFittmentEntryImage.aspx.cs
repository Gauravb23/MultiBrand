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
using System.Data.SqlClient;

namespace HSRP.Transaction
{
    public partial class BMHSRPFittmentEntryImage : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static string FileRequestPath = ConfigurationManager.AppSettings["RequestFolder"].ToString();
        SqlConnection con = new SqlConnection(ConnectionString);
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
        int oemid = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            // Utils.GZipEncodePage();
            if (Session["UID"] == null)
            {
                Response.Redirect("~/error.html", true);
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
                oemid = Convert.ToInt32(Session["oemid"].ToString());


                if (!IsPostBack)
                {

                    try
                    {
                        HSRPStateID = Session["UserHSRPStateID"].ToString();
                        RTOLocationID = Session["UserRTOLocationID"].ToString();
                        USERID = Session["UID"].ToString();
                        dealerid = Session["DealerID"].ToString();

                    }
                    catch (Exception err)
                    {
                        lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
                    }
                }
            }
        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("../LiveReports/LiveTracking.aspx");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string specialChar = @"\ |{}%_!'#$%&'()+,‐./:;<=>?@<>@^£§€";
            string FinYear = string.Empty;
            try
            {

                string strgetdate = System.DateTime.Now.ToString();

                try
                {
                    FinYear = Utils.getScalarValue("SELECT (CASE WHEN (MONTH('" + strgetdate + "')) <= 3 THEN convert(varchar(4), YEAR('" + strgetdate + "')-1) + '-' + convert(varchar(4), YEAR('" + strgetdate + "')%100) ELSE convert(varchar(4),YEAR('" + strgetdate + "'))+ '-' + convert(varchar(4),(YEAR('" + strgetdate + "')%100)+1)END)", CnnString);
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }


                string fileLocation = System.Configuration.ConfigurationManager.AppSettings["Upload"].ToString() + "/" + oemid + "/" + FinYear + "/" + dealerid+"/";

                LblMessage.Text = "";
                lblErrMsg.Text = "";

                if (txtRegNumber.Text == "")
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please Enter VehicleRegno";
                    return;

                }

                if (txtRegNumber.Text.Trim() != "")
                {
                    string strVehicleNo = txtRegNumber.Text.Trim();
                    if (strVehicleNo.Length < 4 || strVehicleNo.Length > 10)
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.Text = "Please enter valid vehicle registration no!";
                        return;
                    }
                    foreach (var item in specialChar)
                    {
                        if (strVehicleNo.Contains(item))
                        {
                            lblErrMsg.Visible = true;
                            lblErrMsg.Text = "Special characters not allowed in vehicle registration no!";
                            return;
                        }
                    }
                }

                if (FId.HasFile == false)
                {

                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please upload front lasercode image file.";
                    return;
                }

                if (RId.HasFile == false)
                {

                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "Please upload rear lasercode image file.";
                    return;
                }


                int invoiceflag = 0;
                int annextureflag = 0;

                #region Upload ID
                if (FId.HasFile)
                {

                    int _size = 3072;// equal 3 mb
                    string _fileExt = System.IO.Path.GetExtension(FId.FileName);


                    if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg")
                    {
                        if ((FId.PostedFile.ContentLength / 1024) <= _size)
                        {
                            string FileName = System.IO.Path.GetFileName(FId.FileName);

                            string filename1 = "FImage" + "-" + Convert.ToInt32(dealerid) + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + _fileExt.ToLower();


                            if (!Directory.Exists(fileLocation))
                            {
                                Directory.CreateDirectory(fileLocation);
                            }
                            string Filepath = fileLocation + filename1;

                            FId.SaveAs(Filepath);
                            HiddenFId.Value = filename1;
                            invoiceflag = 1;

                        }
                        else
                        {
                            lblErrMsg.Visible = true;
                            lblErrMsg.ForeColor = Color.Red;
                            lblErrMsg.Text = "Front Image  File Should be in png,jpg,jpeg format or less than 3 mb";
                            return;
                        }


                    }
                    else
                    {
                        lblErrMsg.Visible = true;
                        lblErrMsg.ForeColor = Color.Red;
                        lblErrMsg.Text = "Front Image  File Should be in png,jpg,jpeg format or less than 3 mb";
                        return;
                    }




                }
                else
                {
                    lblErrMsg.Visible = true;
                    lblErrMsg.ForeColor = Color.Red;
                    lblErrMsg.Text = "Please upload Front Image  File  file in png,jpg,jpeg Format";
                    return;

                }

                #endregion



                #region Upload Annexure1
                if (RId.HasFile)
                {

                    int _size = 3072;// equal 3 mb
                    string _fileExt = System.IO.Path.GetExtension(RId.FileName);
                    string _fileExt1 = "RImage " + "-" + Convert.ToInt32(dealerid) + "-" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + _fileExt.ToLower();

                    if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg")
                    {
                        if ((RId.PostedFile.ContentLength / 1024) <= _size)
                        {
                            string FileName = System.IO.Path.GetFileName(RId.FileName);

                            //FileName = txtRegNumber.Text + "-RC-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + _fileExt;
                            //string path = ConfigurationManager.AppSettings["ScannedRcBr"].ToString();
                            if (!Directory.Exists(fileLocation))
                            {
                                Directory.CreateDirectory(fileLocation);
                            }
                            string Filepath = fileLocation + _fileExt1;

                            RId.SaveAs(Filepath);
                            HiddenRId.Value = _fileExt1;

                            annextureflag = 1;
                        }
                        else
                        {
                            LblMessage.Visible = true;
                            LblMessage.ForeColor = Color.Maroon;
                            LblMessage.Text = "Rear Image Should be in png,jpg,jpeg format or less than 3 mb";
                            return;
                        }


                    }
                    else
                    {
                        LblMessage.Visible = true;
                        LblMessage.ForeColor = Color.Maroon;
                        LblMessage.Text = "Rear Image  File Should be in png,jpg,jpeg format or less than 3 mb";
                        return;
                    }




                }
                else
                {
                    LblMessage.Visible = true;
                    LblMessage.ForeColor = Color.Maroon;
                    LblMessage.Text = "Please upload Rear Image  File  file in png,jpg,jpeg Format";
                    return;

                }
                #endregion



                string orderStatus = string.Empty;
                string RecievedAffixationStatus = string.Empty;
                string Query = "select vehicleregno, RecievedAffixationStatus, OrderStatus from hsrprecords where vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "' and Dealerid='" + dealerid + "'";

                DataTable dtregno = Utils.GetDataTable(Query, ConnectionString);
                if (dtregno.Rows.Count > 0)
                {
                    orderStatus = dtregno.Rows[0]["OrderStatus"].ToString().Trim();
                    RecievedAffixationStatus = dtregno.Rows[0]["RecievedAffixationStatus"].ToString().Trim();
                }

                else
                {
                    LblMessage.Text = "";
                    lblErrMsg.Text = "HSRP Order Not Exist With This Dealer Id/Registration No.";
                    //lblErrMsg.Visible = true;
                    return;
                }

                if (orderStatus == "Closed")
                {
                    LblMessage.Text = "";
                    lblErrMsg.Text = "Registration No. Affixation Entry  Already Done";
                    //lblErrMsg.Visible = true;
                    return;
                }
               
                if ((orderStatus == "Embossing Done") && (RecievedAffixationStatus == "Y"))
                {

                    string SQLCheckRecorQuery = "select HSRPRecordID,orderno, HSRP_stateId, VehicleRegNo, isnull(ChassisNo,'') ChassisNo,  isnull(OrderType,'') OrderType, isnull(convert(varchar,getDate(),105),'') ClosedDate, isnull(HSRP_Front_LaserCode,'') HSRP_Front_LaserCode, isnull(HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode from hsrprecords where vehicleregno='"+ txtRegNumber.Text.ToString() +"' and  Dealerid='"+ dealerid +"'  order by HSRPRecordID desc";
                    DataTable dtRecords = Utils.GetDataTable(SQLCheckRecorQuery, ConnectionString);

                    if (dtRecords.Rows.Count == 1)
                    {
                        string UpdateQuery = string.Empty;
                        string HSRPRecordID = dtRecords.Rows[0]["HSRPRecordID"].ToString();
                        string HSRP_stateId = dtRecords.Rows[0]["HSRP_stateId"].ToString();
                        string VehicleRegNo = dtRecords.Rows[0]["VehicleRegNo"].ToString();
                        string ChassisNo = dtRecords.Rows[0]["ChassisNo"].ToString();
                        string OrderType = dtRecords.Rows[0]["OrderType"].ToString();
                        string HSRP_Front_LaserCode = dtRecords.Rows[0]["HSRP_Front_LaserCode"].ToString();
                        string HSRP_Rear_LaserCode = dtRecords.Rows[0]["HSRP_Rear_LaserCode"].ToString();                      

                        if (RecievedAffixationStatus == "Y")
                        {
                            //string UpdateQuery = "Update HSRPRecords set orderStatus= 'Closed' ,OrderClosedDate=Getdate() where  vehicleregno='" + txtRegNumber.Text.Trim().ToString() + "' and  HSRPRecordID='" + HSRPRecordID + "'  and IsBookMyHsrpRecord='Y' ";
                            UpdateQuery = "Update HSRPRecords set orderStatus= 'Closed' ,OrderClosedDate=Getdate() where  vehicleregno='" + txtRegNumber.Text.Trim().ToString() + "' and  Dealerid='" + dealerid + "'";
                        }

                        int i = Utils.ExecNonQuery(UpdateQuery, ConnectionString);
                        if (i > 0)
                        {
                            string insertQuery = "insert into hsrpimagedata (Imgname, HSRPREcordid,Dealerid,Hsrp_stateid,OemID,imageRear)values ('" + HiddenFId.Value + "','" + HSRPRecordID + "','" + dealerid + "','" + HSRP_stateId + "','" + oemid + "','" + HiddenRId.Value + "')";

                            int J = Utils.ExecNonQuery(insertQuery, ConnectionString);

                            LblMessage.Visible = true;
                            LblMessage.Text = "Registration No. Affixation Entry Saved Sucessfully";
                            lblErrMsg.Text = "";
                            txtRegNumber.Text = "";
                            return;
                        }
                    }
                    else if (dtRecords.Rows.Count > 1)
                    {
                        LblMessage.Visible = true;
                        LblMessage.Text = "Duplicate Order found.";
                        lblErrMsg.Text = "";
                        txtRegNumber.Text = "";
                    }
                    else
                    {
                        LblMessage.Visible = true;
                        LblMessage.Text = "Something went wrong. Try sometime later..";
                        lblErrMsg.Text = "";
                        txtRegNumber.Text = "";
                    }
                }
                else
                {
                    LblMessage.Visible = false;
                    lblErrMsg.Visible = true;
                    lblErrMsg.Text = "HSRP receiving entry pending  from dealer";
                    return;
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }


    }


}