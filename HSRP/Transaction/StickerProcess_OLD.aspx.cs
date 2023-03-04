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
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace HSRP.Transaction
{
    public partial class StickerProcess_OLD : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static String ConnectionStringDL = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringDL"].ToString();
        //static String ConnectionStringLink = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringLink"].ToString();
        static string FileRequestPath = ConfigurationManager.AppSettings["RequestFolder"].ToString();

        SqlConnection con = new SqlConnection(ConnectionString);
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string dealerid = string.Empty;
        string macbase = string.Empty;
        string Dealeardetail = string.Empty;
        string oemid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {

            //Session["Save"] = "saved";

            try
            {
                lblSucMess.Visible = false;
                lblErrMess.Visible = false;
                if (Session["UserType"].ToString() == null)
                {
                    Response.Redirect("~/Login.aspx");
                }
                else
                {

                    UserType = Session["UserType"].ToString();
                }
                InitialSetting();
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
            oemid = Session["oemid"].ToString();
            lblavailbal.Text = availableBlanace();
            //DataTable dtdealergstin = Utils.GetDataTable("select  gstin from dealerMaster  where DealerId ='" + dealerid + "' and  HSRP_StateID= '" + HSRPStateID + "'", ConnectionString);
            if (!IsPostBack)
            {
                BindOemnameDropdown();
                BindVehicleTypeDropdown();

                if (Session["Save"] != null && Session["Save"].ToString().ToLower().Contains("saved".ToLower()))
                {
                    lblSucMess.Visible = true;
                    lblErrMess.Visible = false;
                    Session.Contents.Remove("Save");

                    //string viewMsg = "Successfully downloaded file.";

                    //lblSucMess.Text = viewMsg;
                    //lblSucMess.ForeColor = Color.Green;

                    if (Request.QueryString["msg"] != null && Request.QueryString["msg"].ToString() != "")
                    {
                        //string vehRegNo = Request.QueryString["msg"].ToString();
                        lblSucMess.Text = Request.QueryString["msg"].ToString(); ;
                        lblSucMess.ForeColor = Color.Green;

                    }
                }
            }
        }


        private void BindOemnameDropdown()
        {
            try
            {
                lblErrMess.Visible = false;
                SqlCommand cmd = new SqlCommand("Select Distinct Oemid,Name from oemmaster where oemid='" + oemid + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlOemName.DataSource = dt;
                ddlOemName.DataBind();
                ddlOemName.DataValueField = "Oemid";
                ddlOemName.DataTextField = "Name";
                ddlOemName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Oem-", "-Select Oem-"));
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
                lblErrMess.Visible = true;
            }
        }
        private void BindVehicleTypeDropdown()
        {
            try
            {
                lblErrMess.Visible = false;
                SqlCommand cmd = new SqlCommand("Select Distinct VehicleType from Oemrates where oemid='" + oemid + "'", con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlVehicletype.DataSource = dt;
                ddlVehicletype.DataBind();
                ddlVehicletype.DataValueField = "VehicleType";
                ddlVehicletype.DataTextField = "VehicleType";
                ddlVehicletype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Vehicle Type-", "-Select Vehicle Type-"));
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
                lblErrMess.Visible = true;
            }
        }

        private string availableBlanace()
        {
            try
            {
                string SqlString = "DealerAvailableBalance '" + USERID + "','" + HSRPStateID + "'";
                string Amount = Utils.getScalarValue(SqlString, ConnectionString);
                return Amount;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //
            //HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //
            //OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        public void Cleardatasave()
        {
            //txtAddress.Text = "";
            //txtChassisno.Text = "";
            //txtEmail.Text = "";
            txtEngineNo.Text = "";
            //txtexprice.Text = "";
            //txtMobileNo.Text = "";
            txtmodel.Text = "";
            //txtOwnerName.Text = "";
            // txtrecno.Text = "";
            txtRegNumber.Text = "";

            txtChassisno.Text = "";
            //txtEmail.ReadOnly = false;
            //txtOwnerName.ReadOnly = false;
            //txtAddress.ReadOnly = false;
            txtmodel.ReadOnly = false;
            txtRegNumber.ReadOnly = false; ;
            txtEngineNo.ReadOnly = false;
            txtChassisno.ReadOnly = false;
            ddlVehicletype.Enabled = true;
            ddlVehicleclass.Enabled = true;

            ddlFuelType.Enabled = true;
            ddlOrderType.Enabled = true;
            //ddlPlantCode.Enabled = true;

            //ddlPlantCode.ClearSelection();
            ddlOrderType.ClearSelection();
            ddlFuelType.ClearSelection();
            ddlnew.ClearSelection();
            ddlVehicletype.ClearSelection();
            ddlVehicleclass.ClearSelection();
        }

        string Query = string.Empty;

        //string hsrprecord_authorizationno;



        protected void ImgBtnCheck_Click(object sender, ImageClickEventArgs e)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_, ";

            if (txtRegNumber.Text.Trim() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please Enter Vehicle Registration No.";
                return;
            }


            if (ddlOemName.SelectedItem.Text.Trim() == "-Select Oem-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select Oem.";
                return;
            }
            if (ddlVehicleStateType.SelectedItem.Text.Trim() == "-Select-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select Vehicle Stage Type.";
                return;
            }


            if (ddlOemName.SelectedItem.Text.Trim() == "-Select Oem-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select Oem.";
                return;
            }

            if (txtRegNumber.Text.Trim() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please Enter Vehicle Registration No.";
                return;
            }
            if (txtRegNumber.Text.Trim() != "")
            {
                string strVehicleNo = txtRegNumber.Text.Trim();
                if (strVehicleNo.Length < 4 || strVehicleNo.Length > 10)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                    return;
                }

                foreach (var item in specialChar)
                {
                    if (strVehicleNo.Contains(item))
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Special charecters not allowed in vehicleregno!";
                        return;
                    }
                }
            }

            if (string.IsNullOrEmpty(txtChassisno.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please Enter Valid Chassis No.";
                return;
            }


            foreach (var item in specialChar)
            {
                if (txtChassisno.Text.Trim().Contains(item))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Special charecters not allowed in chassis number!";
                    return;
                }
            }


            string vehRegNo = txtRegNumber.Text.ToString().Trim();
            string chassisNo = txtChassisno.Text.ToString().Trim();

            /*
             * Start DB working
             */

            /*
             * commented by 
             * from: Ravi Kumar <ravi.kumar@utsavhsrp.com>
                to:	rtlitdeveloper <rtlitdeveloper@gmail.com>
                cc:	MMM <mukesh2705@yahoo.com>,
                CEO <ceo@utsavhsrp.com>,
                Himanshu Tyagi <projectmanager@utsavhsrp.com>,
                Amit Bhargava <amitbhargavain@gmail.com>
                date:	29 Jul 2020, 10:40
             * 
             *   */
            string checkSQL = string.Empty;
            DataTable dt = new DataTable();
            checkSQL = "select top 1 OwnerName, MobileNo, EngineNo, VehicleType,ManufacturerModel , VehicleClass, '' RegDate, ManufacturingYear, isnull(HSRP_Front_LaserCode,'') HSRP_Front_LaserCode, isnull(HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode " +
              " from HSRPRecords where vehicleRegNo='" + vehRegNo + "'  and orderStatus in ('Embossing Done', 'Closed')  order by HSRPRecordID desc";


            dt = Utils.GetDataTable(checkSQL, ConnectionStringDL);

            if (dt.Rows.Count == 0)
            {

                checkSQL = "select top 1 OwnerName, MobileNo, EngineNo, VehicleType,ManufacturerModel , VehicleClass, '' RegDate, ManufacturingYear, isnull(HSRP_Front_LaserCode,'') HSRP_Front_LaserCode, isnull(HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode " +
              " from HSRPRecords where vehicleRegNo='" + vehRegNo + "'   " +
              "and orderStatus in ('Embossing Done', 'Closed')  order by HSRPRecordID desc";

                dt = Utils.GetDataTable(checkSQL, ConnectionString);
            }

            if (dt.Rows.Count > 0)
            {
                string OwnerName = dt.Rows[0]["OwnerName"].ToString();
                string MobileNo = dt.Rows[0]["MobileNo"].ToString();
                string EngineNo = dt.Rows[0]["EngineNo"].ToString();
                string VehicleType = dt.Rows[0]["VehicleType"].ToString();
                string ManufacturerModel = dt.Rows[0]["ManufacturerModel"].ToString();
                string VehicleClass = dt.Rows[0]["VehicleClass"].ToString();
                string RegDate = dt.Rows[0]["RegDate"].ToString();
                string ManufacturingYear = dt.Rows[0]["ManufacturingYear"].ToString();
                string FrontLaser = dt.Rows[0]["HSRP_Front_LaserCode"].ToString();
                string RearLaser = dt.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                //HideFLaserTxt.Value = FrontLaser;
                //HideRLaserTxt.Value = RearLaser;

                if (FrontLaser == "" || RearLaser == "")
                {
                    DivFrontRearUpload.Visible = true;
                    buttonAgainSave.Visible = true;
                    buttonSave.Visible = false;
                }
                else
                {
                    DivFrontRearUpload.Visible = false;
                    buttonAgainSave.Visible = false;
                    buttonSave.Visible = true;
                }


                txtOwnerName.Text = OwnerName;
                txtmobileno.Text = MobileNo;
                txtEngineNo.Text = EngineNo;

                ddlVehicletype.SelectedItem.Text = VehicleType;
                txtmodel.Text = ManufacturerModel;

                ddlVehicleclass.SelectedItem.Text = VehicleClass;
                //OrderDate.SelectedDate = RegDate

                //txtfronlaser.Text = FrontLaser;
                //txtRearlaser.Text = RearLaser;

                buttonSave.Visible = true;


            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Invalid Vehicle RegNo. or Chassis No.";
                return;
            }



        }

        private void resetPage()
        {
            txtRegNumber.Text = "";
            txtChassisno.Text = "";
            txtOwnerName.Text = "";
            txtmobileno.Text = "";
            txtEngineNo.Text = "";
            ddlVehicletype.SelectedIndex = 0;
            ddlVehicleclass.SelectedIndex = 0;
            txtVehRegDate.Text = "";
            ddlFuelType.SelectedIndex = 0;
            ddlnew.SelectedIndex = 0;
            txtfronlaser.Text = "";
            txtRearlaser.Text = "";
            buttonSave.Visible = false;
            buttonAgainSave.Visible = false;
        }
        string fulesticker;
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                if (ValidateField())
                {
                    string ownerName = txtOwnerName.Text.ToString().Trim();
                    string mobileNo = txtmobileno.Text.ToString().Trim();

                    string vehRegNo = txtRegNumber.Text.ToString().Trim();
                    string vehChassisNo = txtChassisno.Text.ToString().Trim();
                    string vehEngineNo = txtEngineNo.Text.ToString().Trim();

                    string vehicleType = ddlVehicletype.SelectedItem.Text.ToString();
                    string vehModel = txtmodel.Text.ToString().Trim();
                    string vehicleClass = ddlVehicleclass.SelectedItem.Text.ToString();
                    string regDate = txtVehRegDate.Text.ToString();// OrderDate.SelectedDate.ToString();
                    string fuelType = ddlFuelType.SelectedItem.Text.ToString();
                    string fuelType2 = ddlnew.SelectedItem.Text.ToString();

                    string orderType = ddlOrderType.SelectedItem.Text.ToString();
                    string manuDate = HSRPAuthDate.SelectedDate.ToString();

                    string vehFLaserCode = txtfronlaser.Text.ToString().Trim();
                    string vehRLaserCode = txtRearlaser.Text.ToString().Trim();


                    string validDatetime = "2019-04-01";
                    string day = "";
                    string month="";
                     string  year = "";
                     string[] regDateparm = regDate.Split('/');
                     if (regDateparm[0] != "")
                    {
                        day = regDateparm[0].ToString();
                    }
                     if (regDateparm[1] != "")
                    {
                        month = regDateparm[1].ToString();
                    }
                     if (regDateparm[2] != "")
                    {
                        year = regDateparm[2].ToString();
                    }
                    string inputdate = year + "-" + month + "-" + day;

                    string SQLString1 = "select   HSRPStateShortName  from  HSRpState  where HSRP_StateID='" + HSRPStateID + "'";
                    string HSRpShortState = "";

                    DataTable ds1 = Utils.GetDataTable(SQLString1, ConnectionString);

                    if (ds1.Rows.Count > 0)
                    {
                        HSRpShortState = ds1.Rows[0]["HSRPStateShortName"].ToString();
                    }
                    string Vehicleregno = txtRegNumber.Text;
                    if ((Convert.ToDateTime(inputdate))<Convert.ToDateTime(validDatetime))
                    {
                    if (Vehicleregno != string.Empty)
                    {
                        if (HSRpShortState != Vehicleregno.Substring(0, 2))
                        {
                            if (((HSRpShortState == "DL") && (Vehicleregno.Substring(0, 2) != "UP")) || ((HSRpShortState == "UP") && (Vehicleregno.Substring(0, 2) != "DL")))
                            {

                                lblErrMess.Visible = true;
                                lblErrMess.Text = "You are  not authorized to book the sticker";
                                return ;

                            }
                        }
                    }
                    }

                    if (vehFLaserCode != "" && vehRLaserCode != "")
                    {
                        try
                        {
                            /*
                             * Start validation from DB
                             */
                            if ((vehFLaserCode.Substring(0, 2) != "AA") || (vehRLaserCode.Substring(0, 2) != "AA"))
                            {
                                Response.Redirect("https://www.bookmyhsrp.com/", true);
                            }

                            if ((vehFLaserCode.Length < 11))
                            {
                                Response.Redirect("https://www.bookmyhsrp.com/", true);
                            }
                            #region


                            //string SQLString = "select a.HSRPRecordID, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,isnull(a.HSRP_Front_LaserCode,'') HSRP_Front_LaserCode,isnull(a.HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID " +
                            //    "where  a.vehicleRegNo='" + vehRegNo + "' and right(a.ChassisNo,5)='" + vehChassisNo + "' and a.HSRP_Front_LaserCode = '" + vehFLaserCode + "'  and a.HSRP_Rear_LaserCode = '" + vehRLaserCode + "' ";

                            //DataTable ds = Utils.GetDataTable(SQLString, ConnectionStringDL);

                            /*
                             * commented by 
                             * from: Ravi Kumar <ravi.kumar@utsavhsrp.com>
                                to:	rtlitdeveloper <rtlitdeveloper@gmail.com>
                                cc:	MMM <mukesh2705@yahoo.com>,
                                CEO <ceo@utsavhsrp.com>,
                                Himanshu Tyagi <projectmanager@utsavhsrp.com>,
                                Amit Bhargava <amitbhargavain@gmail.com>
                                date:	29 Jul 2020, 10:40
                             * 
                             *  */

                            DataTable dt1 = new DataTable();
                            string strstickerPrinterFacility =string.Empty;
                            strstickerPrinterFacility = "select stickerPrinterFacility from dealermaster where dealerid = '" + dealerid + "'";


                            dt1 = Utils.GetDataTable(strstickerPrinterFacility, ConnectionString);
                            string StickerPrintStatus = dt1.Rows[0]["stickerPrinterFacility"].ToString();

                            if (StickerPrintStatus == "N") 
                            {
                                int availableamount = Convert.ToInt32(availableBlanace());
                                if (availableamount < 0)
                                {
                                    //lblSucMess.Visible = false;
                                    //lblErrMess.Visible = true;
                                    //lblErrMess.Text = "Available Balance is low, Please Contact to Administrator.";
                                    //return;
                                }
                            }

                            string SQLString = string.Empty;

                            DataTable ds = new DataTable();
                            SQLString = "select a.HSRPRecordID, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,isnull(a.HSRP_Front_LaserCode,'') HSRP_Front_LaserCode,isnull(a.HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID " +
                              "where  a.vehicleRegNo='" + vehRegNo + "'  and a.HSRP_Front_LaserCode = '" + vehFLaserCode + "'  and a.HSRP_Rear_LaserCode = '" + vehRLaserCode + "' ";


                            ds = Utils.GetDataTable(SQLString, ConnectionStringDL);

                            if (ds.Rows.Count == 0)
                            {
                                SQLString = "select a.HSRPRecordID, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,isnull(a.HSRP_Front_LaserCode,'') HSRP_Front_LaserCode,isnull(a.HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID " +
                                   "where  a.vehicleRegNo='" + vehRegNo + "'  " +
                                   "and a.HSRP_Front_LaserCode = '" + vehFLaserCode + "'  and a.HSRP_Rear_LaserCode = '" + vehRLaserCode + "'   ";

                                ds = Utils.GetDataTable(SQLString, ConnectionString);
                            }



                            //string oemName = ddlOemName.SelectedValue;
                            string VehicleStateType = ddlVehicleStateType.SelectedValue;
                            if (ds.Rows.Count > 0)
                            {
                                DivFrontRearUpload.Visible = false;
                                string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                                string FrontLCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString();
                                string RearLaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString();
                                string ChassisNo = ds.Rows[0]["ChassisNo"].ToString();
                                if (Stricker != "")
                                {

                                    //lblSucMess.Text = "Successfully download Stricker" ;
                                    //lblSucMess.ForeColor = Color.Green;

                                    string stickerPrinterFacility = "N";
                                    string InvoiceNo = string.Empty;
                                    string sqlQuery = string.Empty;
                                    try
                                    {
                                        /* Commented by Dhiru
                                         * Start Inserting into table
                                         * Create new table as per column defined into below commented SQL statement
                                         * After creating table declare table name into sql insert statement
                                         * Finally uncomment code lineNo 458 to 462
                                         */

                                        string isBookMyHSRP = string.Empty;
                                        string queryfetchstatusofBookMyHSRP = "select Count(HSRPrecordId) as BookMyHSRPExist from HSRPRecords where  Vehicleregno = '" + vehRegNo + "' and  IsBookMyHsrpRecord='Y'";
                                        DataTable dtstatusofBookMyHSRP = Utils.GetDataTable(queryfetchstatusofBookMyHSRP, ConnectionString);

                                        isBookMyHSRP = dtstatusofBookMyHSRP.Rows[0]["BookMyHSRPExist"].ToString();

                                        if (fuelType != "-Select Fuel type-")
                                        {
                                            fulesticker  = fuelType;
                                        }
                                        if (fuelType2 != "-Select Fuel type-")
                                        {
                                            fulesticker = fuelType2;
                                        }
                                        if ((oemid != "1005") && (isBookMyHSRP == "0"))
                                        {
                                            sqlQuery = "StrickerOnlyEntryFRLaserSuccess '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" +
                                                ChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" +
                                                regDate + "', '" + fulesticker + "', '" + orderType + "', '" + manuDate + "', '" +
                                                vehFLaserCode + "', '" + vehRLaserCode + "', '" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','N' ";
                                        }

                                        if ((oemid != "1005") && (isBookMyHSRP == "1"))
                                        {
                                            sqlQuery = "StrickerOnlyEntryFRLaserSuccess '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" +
                                               ChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" +
                                               regDate + "', '" + fulesticker + "', '" + orderType + "', '" + manuDate + "', '" +
                                               vehFLaserCode + "', '" + vehRLaserCode + "', '" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','Y' ";
                                        }
                                        if ((oemid == "1005") && (isBookMyHSRP == "0"))
                                        {
                                            sqlQuery = "StrickerOnlyEntryFRLaserSuccess '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" +
                                               ChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" +
                                               regDate + "', '" + fulesticker + "', '" + orderType + "', '" + manuDate + "', '" +
                                               vehFLaserCode + "', '" + vehRLaserCode + "', '" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','Y' ";
                                        }
                                        if ((oemid == "1005") && (isBookMyHSRP == "1"))
                                        {
                                            sqlQuery = "StrickerOnlyEntryFRLaserSuccess '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" +
                                               ChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" +
                                               regDate + "', '" + fulesticker + "', '" + orderType + "', '" + manuDate + "', '" +
                                               vehFLaserCode + "', '" + vehRLaserCode + "', '" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','Y' ";
                                        }

                                        DataTable dt = Utils.GetDataTable(sqlQuery, ConnectionString);

                                        if (dt.Rows.Count > 0)
                                        {
                                            if (dt.Rows[0]["status"].ToString() == "1")
                                            {
                                                //string msg = "successfully saved.";
                                                //Session["Save"] = "saved";
                                                //Response.Redirect("StickerProcess.aspx?msg=" + msg);
                                                stickerPrinterFacility = dt.Rows[0]["StickerPrinterFacility"].ToString();
                                                InvoiceNo = dt.Rows[0]["InvoiceNo"].ToString();
                                                lblErrMess.Visible = true;
                                                lblErrMess.Text = dt.Rows[0]["msg"].ToString();
                                            }
                                            else
                                            {
                                                lblErrMess.Visible = true;
                                                lblErrMess.Text = dt.Rows[0]["msg"].ToString();
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            lblErrMess.Visible = true;
                                            lblErrMess.Text = "Something wrong try again!!!";
                                            return;
                                        }
                                        /*
                                         * End Inserting into table
                                         */
                                    }
                                    catch (Exception ev)
                                    {
                                        lblErrMess.Visible = true;
                                        lblErrMess.Text = ev.Message;
                                        //return;
                                    }

                                    if (stickerPrinterFacility == "Y")
                                    {
                                        #region

                                        string filename = "Sticker" + vehRegNo + "-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";


                                        String StringField = String.Empty;
                                        String StringAlert = String.Empty;

                                        //StringBuilder bb = new StringBuilder();

                                        Document document = new Document(PageSize.A4, 0, 0, 212, 0);
                                        document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
                                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                                        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                                        //string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                                        //PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));

                                        string folderpath = ConfigurationManager.AppSettings["PdfFolder"].ToString() + "Sticker/";
                                        if (!Directory.Exists(folderpath))
                                        {
                                            Directory.CreateDirectory(folderpath);
                                        }

                                        string PdfFolder = folderpath + filename;
                                        //string filepathtosave = FinYear + "/" + OemID + "/" + DealerID + "/" + filename;
                                        PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));


                                        //Opens the document:
                                        document.Open();

                                        PdfPTable table = new PdfPTable(1);

                                        table.TotalWidth = 300f;

                                        StringBuilder sbtrnasportname = new StringBuilder();
                                        string trnasportname = "TRANSPORT DEPARTMENT";
                                        trnasportname = "TRANSPORT DEPARTMENT";

                                        for (int i = trnasportname.Length - 1; i >= 0; i--)
                                        {
                                            sbtrnasportname.Append(trnasportname[i].ToString());
                                        }

                                        string fontpath = ConfigurationManager.AppSettings["DataFolder"].ToString();
                                        BaseFont basefont = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, true);
                                        //BaseFont basefont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                                        PdfPCell cell6 = new PdfPCell(new Phrase(sbtrnasportname.ToString(), new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                        // cell.Colspan = 4;
                                        cell6.PaddingTop = 8f;
                                        cell6.BorderColor = BaseColor.WHITE;
                                        cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell6);

                                        StringBuilder sb = new StringBuilder();
                                        StringBuilder sb1 = new StringBuilder();

                                        //string statename = "NATIONAL CAPITAL TERRITORY OF";// ds.Rows[0]["statetext"].ToString().ToUpper();
                                        ///string statename = ds.Rows[0]["statetext"].ToString().ToUpper();
                                        ///
                                        string statename = "";// ds.Rows[0]["statetext"].ToString().ToUpper();
                                        string HSRP_StateId = HSRPStateID;

                                        string  Vehicleregno1=txtRegNumber.Text.Trim();

                                        if (Vehicleregno1.Substring(0, 2) == "DL")
                                        {
                                            statename = "NATIONAL CAPITAL TERRITORY OF";
                                        }
                                        else
                                        {
                                            statename = ds.Rows[0]["statetext"].ToString().ToUpper();
                                        }

                                        string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();


                                        for (int i = statename.Length - 1; i >= 0; i--)
                                        {
                                            sb.Append(statename[i].ToString());
                                        }

                                        for (int i = HSRPStateName.Length - 1; i >= 0; i--)
                                        {
                                            sb1.Append(HSRPStateName[i].ToString());
                                        }

                                        if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                            // cell.Colspan = 4;
                                            cell.PaddingTop = 8f;
                                            cell.BorderColor = BaseColor.WHITE;
                                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                            table.AddCell(cell);
                                        }

                                        else if (HSRPStateName == "HARYANA")
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                            // cell.Colspan = 4;
                                            cell.PaddingTop = 8f;
                                            cell.BorderColor = BaseColor.WHITE;
                                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                            table.AddCell(cell);
                                        }
                                        else
                                        {

                                            PdfPCell cell = new PdfPCell(new Phrase(sb1.ToString() + " " + sb.ToString(), new iTextSharp.text.Font(basefont, 10f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                            // cell.Colspan = 4;
                                            cell.PaddingTop = 8f;
                                            cell.BorderColor = BaseColor.WHITE;
                                            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                            table.AddCell(cell);
                                        }


                                        //StringBuilder sbVehicleRegNo = new StringBuilder();

                                        string VehicleRegNo = string.Empty;
                                        StringBuilder sbVehicleRegNo = new StringBuilder();
                                        try
                                        {
                                            VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();
                                            string[] tokens = VehicleRegNo.Split(new[] { " " }, StringSplitOptions.None);
                                            VehicleRegNo = tokens[0];
                                        }
                                        catch (Exception ev)
                                        {

                                        }
                                        for (int i = VehicleRegNo.Length - 1; i >= 0; i--)
                                        {
                                            sbVehicleRegNo.Append(VehicleRegNo[i].ToString());
                                        }


                                        PdfPCell cell2 = new PdfPCell(new Phrase(sbVehicleRegNo.ToString(), new iTextSharp.text.Font(basefont, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                        // cell.Colspan = 4;
                                        cell2.PaddingTop = 8f;
                                        cell2.BorderColor = BaseColor.WHITE;
                                        cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell2);
                                        StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                                        StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();



                                        string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();
                                        string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();

                                        for (int i = HSRP_Front_LaserCode.Length - 1; i >= 0; i--)
                                        {
                                            sbHSRP_Front_LaserCode.Append(HSRP_Front_LaserCode[i].ToString());
                                        }

                                        for (int i = HSRP_Rear_LaserCode.Length - 1; i >= 0; i--)
                                        {
                                            sbHSRP_Rear_LaserCode.Append(HSRP_Rear_LaserCode[i].ToString());
                                        }

                                        PdfPCell cell3 = new PdfPCell(new Phrase("" + sbHSRP_Rear_LaserCode.ToString() + " - " + sbHSRP_Front_LaserCode.ToString(), new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                        // cell.Colspan = 4;
                                        cell3.PaddingTop = 8f;
                                        //cell3.PaddingLeft = 193f;
                                        cell3.BorderColor = BaseColor.WHITE;
                                        cell3.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell3);


                                        string VehicleRegDate = regDate.Replace("/", "-");
                                        StringBuilder sbVehicleRegDate = new StringBuilder();
                                        for (int i = VehicleRegDate.Length - 1; i >= 0; i--)
                                        {
                                            sbVehicleRegDate.Append(VehicleRegDate[i].ToString());
                                        }

                                        PdfPCell cell4 = new PdfPCell(new Phrase(sbVehicleRegDate.ToString(), new iTextSharp.text.Font(basefont, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                                        // cell.Colspan = 4;
                                        cell4.PaddingTop = 8f;
                                        cell4.BorderColor = BaseColor.WHITE;
                                        cell4.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                        table.AddCell(cell4);






                                        document.Add(table);

                                        document.Close();

                                       
                                
                                        lblErrMess.Visible = false;
                                        lblSucMess.Visible = true;
                                        lblSucMess.Text = "Sticker Downloaded Successfully. Invoice Generted."; //Record saved successfully. Fee charged 50+GST
                                        lblSucMess.ForeColor = Color.Green;
                                        resetPage();

                                        string yourUrl = "StickerPdf.aspx?FileName=" + filename;
                                        Response.Write("<script> window.open( '" + yourUrl + "','_blank' ); </script>");


                                       // string stickerInvoiceUrl = "StickerInvoicePdf.aspx?InvoiceNo=" + InvoiceNo;
                                       // Response.Write("<script> window.open( '" + stickerInvoiceUrl + "','_blank' ); </script>");

                                    

                                        #endregion
                                    }
                                    else
                                    {
                                        lblErrMess.Visible = false;
                                        lblSucMess.Visible = true;
                                        lblSucMess.Text = "Record saved successfully. Devlivery from EC. Fee charged 50+GST";
                                        lblSucMess.ForeColor = Color.Green;
                                        resetPage();
                                    }

                                }
                                else
                                {
                                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                                }
                            }
                            else
                            {
                                DivFrontRearUpload.Visible = true;
                                buttonAgainSave.Visible = true;
                                buttonSave.Visible = false;
                            }



                            #endregion
                            /*
                             * End validation from DB
                             */

                        }
                        catch (Exception ev)
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = ev.Message;
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                lblSucMess.Text = "Message : " + ex;
                return;
            }
        }

        //protected void ChckedChanged(object sender, EventArgs e)
        //{
        //    if(cbFronlaser.Checked || cbRearlaser.Checked)
        //    {
        //        DivFrontRearUpload.Visible = true;
        //        buttonAgainSave.Visible = true;
        //        buttonSave.Visible = false;
        //    }
        //    else
        //    {
        //        DivFrontRearUpload.Visible = false;
        //        buttonAgainSave.Visible = false;
        //        buttonSave.Visible = true;
        //    }
        //}


        protected void buttonAgainSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ValidateField())
            {

                string ownerName = txtOwnerName.Text.ToString().Trim();
                string mobileNo = txtmobileno.Text.ToString().Trim();

                string vehRegNo = txtRegNumber.Text.ToString().Trim();
                string vehChassisNo = txtChassisno.Text.ToString().Trim();
                string vehEngineNo = txtEngineNo.Text.ToString().Trim();

                string vehicleType = ddlVehicletype.SelectedItem.Text.ToString();
                string vehModel = txtmodel.Text.ToString().Trim();
                string vehicleClass = ddlVehicleclass.SelectedItem.Text.ToString();
                string regDate = txtVehRegDate.Text.ToString();//OrderDate.SelectedDate.ToString();
                string fuelType = ddlFuelType.SelectedItem.Text.ToString();
                string fuelType2 = ddlnew.SelectedItem.Text.ToString();

                string orderType = ddlOrderType.SelectedItem.Text.ToString();
                string manuDate = HSRPAuthDate.SelectedDate.ToString();

                string vehFLaserCode = txtfronlaser.Text.ToString().Trim();
                string vehRLaserCode = txtRearlaser.Text.ToString().Trim();

                if (vehRLaserCode != "" || vehRLaserCode != "")
                {


                    #region Upload RC
                    if (RcUploader.HasFile)
                    {

                        int _size = 3072;// equal 3 mb
                        string _fileExt = System.IO.Path.GetExtension(RcUploader.FileName);


                        if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg" || _fileExt.ToLower() == ".pdf")
                        {
                            if ((RcUploader.PostedFile.ContentLength / 1024) <= _size)
                            {
                                string FileName = System.IO.Path.GetFileName(RcUploader.FileName);

                                FileName = txtRegNumber.Text + "-RC-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + _fileExt;
                                //string path = ConfigurationManager.AppSettings["ScannedRcBr"].ToString();
                                if (!Directory.Exists(FileRequestPath))
                                {
                                    Directory.CreateDirectory(FileRequestPath);
                                }
                                string Filepath = FileRequestPath + FileName;

                                RcUploader.SaveAs(Filepath);
                                HiddenRCPath.Value = FileName;


                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.ForeColor = Color.Maroon;
                                lblErrMess.Text = "Vehicle Registration Certificate Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                                return;
                            }


                        }
                        else
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.ForeColor = Color.Maroon;
                            lblErrMess.Text = "Vehicle Registration Certificate Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                            return;
                        }




                    }
                    else
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.ForeColor = Color.Maroon;
                        lblErrMess.Text = "Please upload Vehicle Registration Certificate Image..";
                        return;

                    }
                    #endregion

                    #region Upload RC
                    if (IDUploader.HasFile)
                    {

                        int _size = 3072;// equal 3 mb
                        string _fileExt = System.IO.Path.GetExtension(IDUploader.FileName);


                        if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg" || _fileExt.ToLower() == ".pdf")
                        {
                            if ((IDUploader.PostedFile.ContentLength / 1024) <= _size)
                            {
                                string FileName = System.IO.Path.GetFileName(IDUploader.FileName);

                                FileName = txtRegNumber.Text + "-ID-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + _fileExt;
                                //string path = FileUploadDir + "";
                                if (!Directory.Exists(FileRequestPath))
                                {
                                    Directory.CreateDirectory(FileRequestPath);
                                }
                                string Filepath = FileRequestPath + FileName;

                                IDUploader.SaveAs(Filepath);
                                HiddenIDPath.Value = FileName;


                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.ForeColor = Color.Maroon;
                                lblErrMess.Text = "Id Proof Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                                return;
                            }


                        }
                        else
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.ForeColor = Color.Maroon;
                            lblErrMess.Text = "Id Proof Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                            return;
                        }




                    }
                    else
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.ForeColor = Color.Maroon;
                        lblErrMess.Text = "Please upload Id Proof Image..";
                        return;

                    }

                    #endregion


                    #region Upload Front Laser
                    if (FileFrontlaser.HasFile)
                    {

                        int _size = 3072;// equal 3 mb
                        string _fileExt = System.IO.Path.GetExtension(FileFrontlaser.FileName);


                        if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg" || _fileExt.ToLower() == ".pdf")
                        {
                            if ((FileFrontlaser.PostedFile.ContentLength / 1024) <= _size)
                            {
                                string FileName = System.IO.Path.GetFileName(FileFrontlaser.FileName);

                                FileName = txtRegNumber.Text + "-FrontLaser-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + _fileExt;
                                //string path = ConfigurationManager.AppSettings["ScannedRcBr"].ToString();
                                if (!Directory.Exists(FileRequestPath))
                                {
                                    Directory.CreateDirectory(FileRequestPath);
                                }
                                string Filepath = FileRequestPath + FileName;

                                FileFrontlaser.SaveAs(Filepath);
                                HiddenFlaser.Value = FileName;


                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.ForeColor = Color.Maroon;
                                lblErrMess.Text = "Vehicle Front Laser Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                                return;
                            }


                        }
                        else
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.ForeColor = Color.Maroon;
                            lblErrMess.Text = "Vehicle Front Laser Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                            return;
                        }




                    }
                    else
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.ForeColor = Color.Maroon;
                        lblErrMess.Text = "Please upload Vehicle Front Laser Image..";
                        return;

                    }
                    #endregion


                    #region Upload Rear Laser
                    if (FileRearLaser.HasFile)
                    {

                        int _size = 3072;// equal 3 mb
                        string _fileExt = System.IO.Path.GetExtension(FileRearLaser.FileName);


                        if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg" || _fileExt.ToLower() == ".pdf")
                        {
                            if ((FileRearLaser.PostedFile.ContentLength / 1024) <= _size)
                            {
                                string FileName = System.IO.Path.GetFileName(FileRearLaser.FileName);

                                FileName = txtRegNumber.Text + "-RearLaser-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + _fileExt;
                                //string path = ConfigurationManager.AppSettings["ScannedRcBr"].ToString();
                                if (!Directory.Exists(FileRequestPath))
                                {
                                    Directory.CreateDirectory(FileRequestPath);
                                }
                                string Filepath = FileRequestPath + FileName;

                                FileRearLaser.SaveAs(Filepath);
                                HiddenRearLaser.Value = FileName;


                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.ForeColor = Color.Maroon;
                                lblErrMess.Text = "Vehicle Rear Laser Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                                return;
                            }


                        }
                        else
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.ForeColor = Color.Maroon;
                            lblErrMess.Text = "Vehicle Rear Laser Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                            return;
                        }




                    }
                    else
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.ForeColor = Color.Maroon;
                        lblErrMess.Text = "Please upload Vehicle Rear Laser Image..";
                        return;

                    }
                    #endregion


                    if (ddlDocType.SelectedItem.Text.ToString() == "-Select ID Proof-")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Select ID Proof.";
                        return;
                    }


                    string rcFileName = HiddenRCPath.Value;
                    string idFileName = HiddenIDPath.Value;
                    string flFileName = HiddenFlaser.Value;
                    string rlFileName = HiddenRearLaser.Value;

                    string docType = ddlDocType.SelectedItem.Text.ToString();
                    string VehicleStateType = ddlVehicleStateType.SelectedItem.Text;
                    try
                    {
                        /* Commented by Dhiru
                         * Start Inserting into table
                         * Create new table as per column defined into below commented SQL statement
                         * After creating table declare table name into sql insert statement
                         * Finally uncomment code lineNo 458 to 462
                         */

                        string sqlQuery = string.Empty;

                        //string isBookMyHSRP = string.Empty;
                        //string queryfetchstatusofBookMyHSRP = "select Count(HSRPrecordId) as BookMyHSRPExist from HSRPRecords where  vehRegNo = '" + vehRegNo + "' and  IsBookMyHsrpRecord='Y'";
                        //DataTable dtstatusofBookMyHSRP = Utils.GetDataTable(queryfetchstatusofBookMyHSRP, ConnectionString);

                        //isBookMyHSRP = dtstatusofBookMyHSRP.Rows[0]["BookMyHSRPExist"].ToString();
                        //if ((oemid != "1005") && (isBookMyHSRP == "0"))
                        //{
                            sqlQuery = "StrickerOnlyEntry_FRLaserEmpty '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" + vehChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" + regDate + "', '" + fuelType + "', '" + orderType + "', '" + manuDate + "', '" + vehFLaserCode + "', '" + vehRLaserCode + "', '" + rcFileName + "', '" + idFileName + "', '" + flFileName + "' , '" + rlFileName + "' , '" + docType + "','" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "'";
                       // }
                        //if ((oemid != "1005") && (isBookMyHSRP == "1"))
                        //{
                        //    sqlQuery = "StrickerOnlyEntry_FRLaserEmpty '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" + vehChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" + regDate + "', '" + fuelType + "', '" + orderType + "', '" + manuDate + "', '" + vehFLaserCode + "', '" + vehRLaserCode + "', '" + rcFileName + "', '" + idFileName + "', '" + flFileName + "' , '" + rlFileName + "' , '" + docType + "','" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','Y' ";
                        //}
                        //if ((oemid != "1005") && (isBookMyHSRP == "0"))
                        //{
                        //    sqlQuery = "StrickerOnlyEntry_FRLaserEmpty '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" + vehChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" + regDate + "', '" + fuelType + "', '" + orderType + "', '" + manuDate + "', '" + vehFLaserCode + "', '" + vehRLaserCode + "', '" + rcFileName + "', '" + idFileName + "', '" + flFileName + "' , '" + rlFileName + "' , '" + docType + "','" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','N' ";
                        //}
                        //if ((oemid != "1005") && (isBookMyHSRP == "0"))
                        //{
                        //    sqlQuery = "StrickerOnlyEntry_FRLaserEmpty '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" + vehChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" + regDate + "', '" + fuelType + "', '" + orderType + "', '" + manuDate + "', '" + vehFLaserCode + "', '" + vehRLaserCode + "', '" + rcFileName + "', '" + idFileName + "', '" + flFileName + "' , '" + rlFileName + "' , '" + docType + "','" + oemid + "', '" + dealerid + "', '" + HSRPStateID + "', '" + USERID + "','" + VehicleStateType + "','N' ";
                        //}

                        DataTable dt = Utils.GetDataTable(sqlQuery, ConnectionString);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["status"].ToString() == "1")
                            {
                                string msg = "Records saved successfull";
                                Session["Save"] = "saved";
                                Response.Redirect("StickerProcess.aspx?msg=" + msg);
                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.Text = dt.Rows[0]["msg"].ToString();
                                return;
                            }
                        }
                        else
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Something wrong try again!!!";
                            return;
                        }
                        /*
                         * End Inserting into table
                         */
                    }
                    catch (Exception ev)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = ev.Message;
                        return;
                    }


                }



            }
        }


        private bool ValidateField()
        {
            try
            {
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_, ";
                Regex regexDate = new Regex(@"(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$");
                string HSRpShortState = string.Empty;
                string SQLString = "select   HSRPStateShortName  from  HSRpState  where HSRP_StateID='" + HSRPStateID + "'";

                
                DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
         
                if (ds.Rows.Count > 0)
                {
                    HSRpShortState = ds.Rows[0]["HSRPStateShortName"].ToString();
                }
                string Vehicleregno=txtRegNumber.Text;
                //if (Vehicleregno != string.Empty)
                //{
                //    if (HSRpShortState != Vehicleregno.Substring(0, 2))
                //    {
                //        if (((HSRpShortState == "DL") && (Vehicleregno.Substring(0, 2) != "UP")) || ((HSRpShortState == "UP") && (Vehicleregno.Substring(0, 2) != "DL")))
                //        {

                //            lblErrMess.Visible = true;
                //            lblErrMess.Text = "You are  not authorized to book the sticker";
                //            return false;

                //        }
                //    }
                //}


                if (ddlOemName.SelectedItem.Text.Trim() == "-Select Oem-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select Oem.";
                    return false;
                }

                if (ddlOemName.SelectedItem.Text.Trim() == "-Select Oem-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select Oem.";
                    return false;
                }
                if (ddlVehicleStateType.SelectedItem.Text.Trim() == "-Select-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select Vehicle Stage Type.";
                    return false;
                }

                if (txtOwnerName.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Owner Name";
                    return false;
                }
                if (txtmobileno.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Mobile No.";
                    return false;
                }

                if (txtmobileno.Text.Trim() != "")
                {
                    string strMobileNo = txtmobileno.Text.Trim();
                    if (strMobileNo.Length != 10)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Enter Valid Mobile No.";
                        return false;
                    }
                    foreach (var item in specialChar)
                    {
                        if (strMobileNo.Contains(item))
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Special charecters not allowed in Mobile No. !";
                            return false;
                        }
                    }
                }

                if (txtRegNumber.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Vehicle Registration No.";
                    return false;
                }

                if (txtRegNumber.Text.Trim() != "")
                {
                    string strVehicleNo = txtRegNumber.Text.Trim();
                    if (strVehicleNo.Length < 4 || strVehicleNo.Length > 10)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                        return false;
                    }
                    foreach (var item in specialChar)
                    {
                        if (strVehicleNo.Contains(item))
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Special charecters not allowed in vehicleregno!";
                            return false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(txtChassisno.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Chassis No.";
                    return false;
                }
                //foreach (var item in specialChar)
                //{
                //    if (txtChassisno.Text.Trim().Contains(item))
                //    {
                //        lblErrMess.Visible = true;
                //        lblErrMess.Text = "Special charecters not allowed in chassis number!";
                //        return false;
                //    }
                //}
                if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Engine No.";
                    return false;
                }
                foreach (var item in specialChar)
                {
                    if (txtEngineNo.Text.Trim().Contains(item))
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Special charecters not allowed in engine number!";
                        return false;
                    }
                }
                if (ddlVehicletype.SelectedItem.Text.ToString() == "-Select Vehicle Type-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Select Vehicle Type.";
                    return false;
                }
                if (ddlVehicleclass.SelectedItem.Text.ToString() == "-Select Vehicle Class-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Select Vehicle Class.";
                    return false;
                }


                try
                {
                    //Verify whether date entered in dd/MM/yyyy format.
                    //bool isValid = regexDate.IsMatch(OrderDate.SelectedDate.ToString());

                    ////Verify whether entered date is Valid date.
                    //DateTime dt;
                    //isValid = DateTime.TryParseExact(OrderDate.SelectedDate.ToString(), "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);

                    //if (isValid)
                    //{
                    //    DateTime regMDate = DateTime.Parse(Convert.ToDateTime(OrderDate.SelectedDate.ToString()).ToShortDateString());
                    //    DateTime todate = DateTime.Parse(Convert.ToDateTime("01/04/2019").ToShortDateString());
                    //    if (regMDate > todate)
                    //    {
                    //        lblErrMess.Visible = true;
                    //        lblErrMess.Text = "Choose Vehicle Reg. date before 01/04/2019";
                    //        return;
                    //    }
                    //}
                    //else
                    //{
                    //    lblErrMess.Visible = true;
                    //    lblErrMess.Text = "Vehicle Reg. date invalid";
                    //    return;
                    //}
                    //txtVehRegDate.Text.ToString();//
                    //DateTime regMDate = DateTime.Parse(Convert.ToDateTime(OrderDate.SelectedDate.ToString()).ToShortDateString());
                    //DateTime regMDate = DateTime.Parse(Convert.ToDateTime(txtVehRegDate.Text.ToString()).ToShortDateString());
                    //DateTime todate = DateTime.Parse(Convert.ToDateTime("01/04/2019").ToShortDateString());
                    //if (regMDate > todate)
                    //{
                    //    lblErrMess.Visible = true;
                    //    lblErrMess.Text = "Choose Vehicle Reg. date before 01/04/2019";
                    //    return false;
                    //}


                }
                catch (Exception ev)
                {

                }



                if (ddlFuelType.SelectedItem.Text.ToString() == "-Select Fuel type-" && ddlnew.SelectedItem.Text.ToString() == "-Select Fuel type-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Select Fuel Type.";
                    return false;
                }
                if (ddlOrderType.SelectedItem.Text.ToString() == "-Select Order Type-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Select Order Type.";
                    return false;
                }

                if (txtfronlaser.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Front Laser Code.";
                    return false;
                }

                if (txtfronlaser.Text.Trim() != "")
                {
                    string strFrontLaser = txtfronlaser.Text.Trim();

                    foreach (var item in specialChar)
                    {
                        if (strFrontLaser.Contains(item))
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Special charecters not allowed in Front Laser Code. !";
                            return false;
                        }
                    }
                }

                if (txtRearlaser.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Rear Laser Code.";
                    return false;
                }

                if (txtRearlaser.Text.Trim() != "")
                {
                    string strRearLaser = txtRearlaser.Text.Trim();

                    foreach (var item in specialChar)
                    {
                        if (strRearLaser.Contains(item))
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Special charecters not allowed in Rear Laser Code. !";
                            return false;
                        }
                    }
                }

            }
            catch (Exception ev)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = ev.Message;
                return false;
            }

            return true;
        }

        private void genertaeStricker()
        {


            //LinkButton lnkInvc = (LinkButton)Grid1.FindControl("LinkButtonSticker");
            //string str = lnkInvc.CommandName.ToString();
            string SQLString = "select a.HSRPRecordID,a.vehicleRegNo, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,a.HSRP_Front_LaserCode,a.HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID where  HSRPRecordID='' and  a.HSRP_Front_LaserCode is not null and a.HSRP_Front_LaserCode!=''";
            DataTable ds = Utils.GetDataTable(SQLString, ConnectionString);
            if (ds.Rows.Count > 0)
            {
                string Stricker = ds.Rows[0]["StickerMandatory"].ToString();
                if (Stricker == "Y")
                {
                    string filename = "Sticker" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + ".pdf";


                    String StringField = String.Empty;
                    String StringAlert = String.Empty;

                    StringBuilder bb = new StringBuilder();

                    Document document = new Document(PageSize.A4, 0, 0, 212, 0);
                    document.SetPageSize(new iTextSharp.text.Rectangle(500, 400));
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    BaseFont bfTimes = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
                    string PdfFolder = ConfigurationManager.AppSettings["PdfFolder"].ToString() + filename;
                    PdfWriter.GetInstance(document, new FileStream(PdfFolder, FileMode.Create));



                    //Opens the document:
                    document.Open();

                    PdfPTable table = new PdfPTable(1);

                    table.TotalWidth = 300f;

                    StringBuilder sbtrnasportname = new StringBuilder();
                    string trnasportname = "TRANSPORT DEPARTMENT";

                    BaseFont basefont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

                    PdfPCell cell6 = new PdfPCell(new Phrase(trnasportname, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    // cell.Colspan = 4;
                    cell6.PaddingTop = 8f;
                    cell6.BorderColor = BaseColor.WHITE;
                    cell6.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell6);
                    StringBuilder sb = new StringBuilder();
                    StringBuilder sb1 = new StringBuilder();


                    string statename = ds.Rows[0]["statetext"].ToString().ToUpper();
                    string HSRPStateName = ds.Rows[0]["HSRPStateName"].ToString().ToUpper();
                    if (HSRPStateName == "HIMACHAL PRADESH" || HSRPStateName == "MADHYA PRADESH" || HSRPStateName == "ANDHRA PRADESH" || HSRPStateName == "UTTRAKHAND")
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell.PaddingTop = 8f;
                        cell.BorderColor = BaseColor.WHITE;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }

                    else if (HSRPStateName == "HARYANA")
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 16f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell.PaddingTop = 8f;
                        cell.BorderColor = BaseColor.WHITE;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    else
                    {

                        PdfPCell cell = new PdfPCell(new Phrase(statename + " " + HSRPStateName, new iTextSharp.text.Font(basefont, 20f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                        // cell.Colspan = 4;
                        cell.PaddingTop = 8f;
                        cell.BorderColor = BaseColor.WHITE;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }


                    StringBuilder sbVehicleRegNo = new StringBuilder();

                    string VehicleRegNo = ds.Rows[0]["VehicleRegNo"].ToString().ToUpper();



                    PdfPCell cell2 = new PdfPCell(new Phrase(VehicleRegNo, new iTextSharp.text.Font(basefont, 30f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    // cell.Colspan = 4;
                    cell2.PaddingTop = 8f;
                    cell2.BorderColor = BaseColor.WHITE;
                    cell2.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell2);
                    StringBuilder sbHSRP_Front_LaserCode = new StringBuilder();
                    StringBuilder sbHSRP_Rear_LaserCode = new StringBuilder();



                    string HSRP_Front_LaserCode = ds.Rows[0]["HSRP_Front_LaserCode"].ToString().ToUpper();
                    string HSRP_Rear_LaserCode = ds.Rows[0]["HSRP_Rear_LaserCode"].ToString().ToUpper();
                    PdfPCell cell3 = new PdfPCell(new Phrase("" + HSRP_Front_LaserCode + " - " + HSRP_Rear_LaserCode, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    // cell.Colspan = 4;
                    cell3.PaddingTop = 8f;
                    cell3.PaddingLeft = 193f;
                    cell3.BorderColor = BaseColor.WHITE;
                    cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell3);

                    StringBuilder sbEngineNo = new StringBuilder();
                    //string EngineNo = "ENGINE NO - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();
                    string RegistraionDate = "Reg. Date - " + ds.Rows[0]["EngineNo"].ToString().ToUpper();


                    PdfPCell cell4 = new PdfPCell(new Phrase(RegistraionDate, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    // cell.Colspan = 4;
                    cell4.PaddingTop = 8f;
                    cell4.PaddingLeft = 193f;
                    cell4.BorderColor = BaseColor.WHITE;
                    cell4.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    table.AddCell(cell4);

                    //StringBuilder sbChassisNo = new StringBuilder();
                    //string ChassisNo = "CHASIS NO - " + ds.Rows[0]["ChassisNo"].ToString().ToUpper();
                    //PdfPCell cell5 = new PdfPCell(new Phrase(ChassisNo, new iTextSharp.text.Font(basefont, 13f, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK)));
                    //// cell.Colspan = 4;
                    //cell5.PaddingTop = 8f;
                    //cell5.PaddingLeft = 193f;
                    //cell5.BorderColor = BaseColor.WHITE;
                    //cell5.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                    //table.AddCell(cell5);

                    document.Add(table);

                    document.Close();

                    //SAVEStickerLog(ds.Rows[0]["vehicleRegNo"].ToString().ToUpper().Trim(), ds.Rows[0]["HSRPRecordID"].ToString());

                    HttpContext context = HttpContext.Current;

                    context.Response.ContentType = "Application/pdf";
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                    context.Response.WriteFile(PdfFolder);
                    context.Response.End();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(typeof(Page), "alert", "<script language=JavaScript>alert('Sticker Not Allow !!');</script>");
                }
            }


        }

        protected void ddlVehicleStateType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlVehicleStateType.SelectedValue == "BS4 or Other")
            {
                ddlnew.Visible = false;
                ddlFuelType.Visible = true;
            }
            if (ddlVehicleStateType.SelectedValue == "BS6")
            {
                ddlnew.Visible = true;
                ddlFuelType.Visible = false;
            }
        }
    }
}