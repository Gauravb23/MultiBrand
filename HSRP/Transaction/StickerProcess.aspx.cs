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
using Newtonsoft.Json;

namespace HSRP.Transaction
{
    public partial class StickerProcess : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static String ConnectionStringDL = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringDL"].ToString();
        static String ConnectionStringHR = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringHR"].ToString();
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
        string checkSQL = string.Empty;
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
                bindState();
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

        protected void ddlAffixationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlAffixationType.SelectedValue == "1")
            {
                lblHomeAddress.Visible = false;
                txtHomeAddress.Visible = false;
                lblstate.Visible = true;
                ddlstate.Visible = true;
                ddlstate.ClearSelection();
                ddlLocationAddress.ClearSelection();
                ddlLocationAddress.Items.Clear();
                ddlLocationAddress.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Fitment Location--", "0"));
                ddldistrict.ClearSelection();
                ddldistrict.Items.Clear();
                ddldistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select District -", "0"));
                lbldistrict.Visible = false;
                ddldistrict.Visible = false;
                lblpincode.Visible = false;
                txtpincode.Visible = false;
                ddlLocationAddress.Visible = true;
                lblLocationAddress.Visible = true;
                btnAdd2.Visible = true;
            }

            if (ddlAffixationType.SelectedValue == "2")
            {
                lblHomeAddress.Visible = true;
                txtHomeAddress.Visible = true;
                lblstate.Visible = true;
                ddlstate.Visible = true;
                ddlstate.ClearSelection();
                ddlLocationAddress.ClearSelection();
                ddlLocationAddress.Items.Clear();
                ddlLocationAddress.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Fitment Location--", "0"));
                ddldistrict.ClearSelection();
                ddldistrict.Items.Clear();
                ddldistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select District -", "0"));
                lbldistrict.Visible = true;
                ddldistrict.Visible = true;
                lblpincode.Visible = true;
                txtpincode.Visible = true;
                ddlLocationAddress.Visible = false;
                lblLocationAddress.Visible = false;
                btnAdd2.Visible = false;
            }
        }

        protected void bindState()
        {
            Query = "Select HSRPStateName, HSRP_StateId from HSRPState order by HSRPStateName asc";
            dt = Utils.GetDataTable(Query, ConnectionString);
            ddlstate.DataSource = dt;
            ddlstate.DataBind();
            ddlstate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select State --", "0"));
        }

        protected void ddlstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Query = "select RTOLocationID, (RTOLocationName +'  '+ '('+ NAVEMBID +')') as RTOLocationName from rtolocation where HSRP_StateID = '" + ddlstate.SelectedValue + "' and NAVEMBID not like 'CW%' and NAVEMBID not like '%Rej%' and ActiveStatus = 'Y' order by RTOLocationName asc";
            dt = Utils.GetDataTable(Query, ConnectionString);
            ddldistrict.DataSource = dt;
            ddldistrict.DataBind();
            ddldistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select District --", "0"));
            DropdownAffixation(ddlstate.SelectedValue.ToString());
        }

        public void DropdownAffixation(string stateid)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand("AffixationDropdown", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Userid", USERID);
                cmd.Parameters.AddWithValue("@Dealerid", dealerid);
                cmd.Parameters.AddWithValue("@OemId", oemid);
                cmd.Parameters.AddWithValue("@StateId", stateid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                ddlLocationAddress.DataSource = dt;
                ddlLocationAddress.DataTextField = "Name";
                ddlLocationAddress.DataValueField = "Value";
                ddlLocationAddress.DataBind();
                ddlLocationAddress.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Fitment Location--", "0"));
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
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

        protected void ImgBtnCheck_Click(object sender, ImageClickEventArgs e)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_, ";
            if (ddlOemName.SelectedItem.Text.Trim() == "-Select Oem-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select oem!";
                return;
            }

            if (ddlVehicleStateType.SelectedItem.Text.Trim() == "-Select-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select vehicle stage type!";
                return;
            }

            if (txtRegNumber.Text.Trim() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please enter vehicle registration no!";
                return;
            }

            if (txtRegNumber.Text.Trim() != "")
            {
                string strVehicleNo = txtRegNumber.Text.Trim();
                if (strVehicleNo.Length < 4 || strVehicleNo.Length > 10)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter valid vehicle registration no!";
                    return;
                }

                foreach (var item in specialChar)
                {
                    if (strVehicleNo.Contains(item))
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Special characters not allowed in vehicleregno!";
                        return;
                    }
                }
            }

            if (string.IsNullOrEmpty(txtChassisno.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please enter valid chassis no!";
                return;
            }

            foreach (var item in specialChar)
            {
                if (txtChassisno.Text.Trim().Contains(item))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Special characters not allowed in chassis number!";
                    return;
                }
            }

            if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please enter valid engine no!";
                return;
            }

            foreach (var item in specialChar)
            {
                if (txtEngineNo.Text.Trim().Contains(item))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Special characters not allowed in engine number!";
                    return;
                }
            }


            string vehRegNo = txtRegNumber.Text.ToString().Trim();
            string chassisNo = txtChassisno.Text.ToString().Trim();
            string engineno = txtEngineNo.Text.ToString().Trim();

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
             * 
             *   */

            #region Vahan Validation checking (code edit by Ashok:22-01-2022)

            string VehicleRego = txtRegNumber.Text.ToString().Trim();
            string Chassisno = txtChassisno.Text.Trim().ToString();
            string strChassisno = Chassisno.Substring(Chassisno.Length - 5, 5);
            string Engineno = txtEngineNo.Text.ToString().Trim();
            string strEngineno = string.Empty;
            strEngineno = Engineno.Substring(Engineno.Length - 5, 5);
            string maker1 = string.Empty;
            string IS_OEM_Name_Match = string.Empty;
            string QryFindOEMName = string.Empty;

            string responseJson = rosmerta_API_2(txtRegNumber.Text.ToString().Trim().ToUpper(), strChassisno, strEngineno, "5UwoklBqiW");
            VehicleDetails _vd = JsonConvert.DeserializeObject<VehicleDetails>(responseJson);

            QryFindOEMName = @"select OEMID,OemName from oemvahanmapping WHERE OemName='" + _vd.maker + "'";
            DataTable dt = Utils.GetDataTable(QryFindOEMName, ConnectionString);

            if (dt.Rows.Count > 0)
            {
                oemid = dt.Rows[0]["OEMID"].ToString();
            }
            string strIsvahan = @"select isVahan from Dealermaster where dealerid = '" + dealerid + "'";

            DataTable dtchkIsvahan = Utils.GetDataTable(strIsvahan, ConnectionString);

            //if((HSRPStateID != "10" && HSRPStateID != "5" && HSRPStateID != "9") &&(dtchkIsvahan.Rows[0]["IsVahan"].ToString()=="Y"))
            string VehicleRegoE = txtRegNumber.Text.ToString().Substring(0, 2).Trim();

            if ((HSRPStateID != "10") && (VehicleRegoE != "TS") && (dtchkIsvahan.Rows[0]["IsVahan"].ToString() == "Y"))
            {
                try
                {

                    string SkipOemName = SkipOemName = @"select VEHICLETYPE from OEMMaster where OEMID = '" + oemid + "'";

                    DataTable dtchk = Utils.GetDataTable(SkipOemName, ConnectionString);
                    if (dtchk.Rows[0]["VEHICLETYPE"].ToString().Trim() != "Trailer & Trolley Supplement")
                    {

                        string maker = _vd.maker;
                        string fueltype = _vd.fuel;
                        string vahanstatus = _vd.message;
                        string norms = _vd.norms;
                        string vehicletype = _vd.vchType;
                        string vehiclecatg = _vd.vchCatg;
                        string regdate = _vd.regnDate;
                        string strquery = string.Empty;
                        if(regdate == "")
                        {
                            txtVehRegDate.Text = "";
                            txtVehRegDate.Enabled = true;
                        }
                        else
                        {
                            txtVehRegDate.Text = regdate;
                            txtVehRegDate.Enabled = false;
                        }
                       
                        strquery = "insert into VahanLog values('" + dealerid + "','" + oemid + "','" + vahanstatus + "','" + fueltype + "','" + maker + "','" + vehicletype + "','" + norms + "','" + vehiclecatg + "','" + regdate + "','" + USERID + "', getdate(),'" + txtRegNumber.Text + "','" + txtChassisno.Text + "','" + txtEngineNo.Text + "')";
                        Utils.ExecNonQuery(strquery, ConnectionString);


                        // maker1 = dt.Rows[0]["OemName"].ToString();

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i <= dt.Rows.Count - 1; i++)
                            {
                                maker1 = dt.Rows[i]["OemName"].ToString().ToUpper();

                                if (maker1 == _vd.maker.ToUpper())
                                {
                                    IS_OEM_Name_Match = "Y";
                                    break;

                                }
                                else
                                {
                                    IS_OEM_Name_Match = "N";

                                }

                            }
                        }

                        // Debug.WriteLine(_vd.message);
                        //  Console.WriteLine(_vd.fuel + "-" + _vd.maker + "_" + _vd.message + "-" + _vd.norms + "-" + _vd.vchCatg + "-" + _vd.vchType);

                        if (_vd.fuel == "" || _vd.maker == "" || _vd.message == "" || _vd.norms == "" || _vd.vchCatg == "" || _vd.vchType == "")
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Vehicle details not available in Vahan";
                            return;
                        }


                        else
                        {
                            if (IS_OEM_Name_Match == "Y")
                            {

                                if (_vd.message == "Vehicle details available in Vahan")
                                {


                                    if ((_vd.message == "Vehicle details available in Vahan") && (_vd.maker.ToUpper() == maker1))
                                    {

                                        //if (Convert.ToDateTime(_vd.regnDate) <= Convert.ToDateTime(validDatetime))
                                        //{
                                        //    lblErrMess.Visible = true;
                                        //    lblErrMess.Text = "Kindly book order in B Option";
                                        //    return;
                                        //}

                                        ////if ((_vd.regnDate != InPutRegdate) && (_vd.regnDate != ""))                                                        
                                        //{

                                        //    lblErrMess.Visible = true;
                                        //    lblErrMess.Text = "Kindly  select  correct Regdate!";
                                        //    return;
                                        //}

                                    }
                                    else
                                    {
                                        lblErrMess.Visible = true;
                                        lblErrMess.Text = "Maker is different form vahan and you are not AUTHORIZED! ";
                                        return;

                                    }
                                }
                                else
                                {
                                    lblErrMess.Visible = true;
                                    lblErrMess.Text = _vd.message;
                                    return;
                                }
                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.Text = "Maker is different form vahan and you are not AUTHORIZED!  ";
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Soemthing went wrong while validating with vahan!";
                    return;
                }
            }

            #endregion


            checkSQL = "select top 1 OwnerName, MobileNo, EngineNo, VehicleType,ManufacturerModel , VehicleClass, '' RegDate, ManufacturingYear, isnull(HSRP_Front_LaserCode,'') HSRP_Front_LaserCode, isnull(HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode " +
             " from HSRPRecords where vehicleRegNo='" + vehRegNo + "'  and orderStatus in ('Embossing Done', 'Closed')  order by HSRPRecordID desc";
            dt = Utils.GetDataTable(checkSQL, ConnectionStringDL);

            if (dt.Rows.Count == 0)
            {
                checkSQL = "select top 1 OwnerName, MobileNo, EngineNo, VehicleType,ManufacturerModel , VehicleClass, '' RegDate, ManufacturingYear, isnull(HSRP_Front_LaserCode,'') HSRP_Front_LaserCode, isnull(HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode " +
                   " from HSRPRecords where vehicleRegNo='" + vehRegNo + "'  and orderStatus in ('Embossing Done', 'Closed')  order by HSRPRecordID desc";
                dt = Utils.GetDataTable(checkSQL, ConnectionString);
            }

            if (dt.Rows.Count == 0)
            {
                checkSQL = "select top 1 OwnerName, MobileNo, EngineNo, VehicleType,ManufacturerModel , VehicleClass, '' RegDate, ManufacturingYear, isnull(HSRP_Front_LaserCode,'') HSRP_Front_LaserCode, isnull(HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode " +
                            " from HSRPRecords where vehicleRegNo='" + vehRegNo + "'  and orderStatus in ('Embossing Done', 'Closed')  order by HSRPRecordID desc";
                dt = Utils.GetDataTable(checkSQL, ConnectionStringHR);

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
                //txtEngineNo.Text = EngineNo;
                //txtEngineNo.Enabled = false;
                ddlVehicletype.SelectedItem.Text = VehicleType;
                txtmodel.Text = ManufacturerModel;
                ddlVehicleclass.SelectedItem.Text = VehicleClass;
                //OrderDate.SelectedDate = RegDate

                //txtfronlaser.Text = FrontLaser;
                //txtRearlaser.Text = RearLaser;

                //buttonSave.Visible = true;
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
            ddlVehicleclass.ClearSelection();
            ddlOemName.ClearSelection();
            ddlOemName.SelectedIndex = 0;
            ddlVehicleStateType.ClearSelection();
            ddlVehicleStateType.SelectedIndex = 0;
            ddlVehicletype.ClearSelection();
            lblHomeAddress.Visible = false;
            txtHomeAddress.Visible = false;
            lblstate.Visible = false;
            ddlstate.Visible = false;
            ddlstate.ClearSelection();
            ddlLocationAddress.ClearSelection();
            ddlLocationAddress.Items.Clear();
            ddlLocationAddress.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Fitment Location--", "0"));
            ddldistrict.ClearSelection();
            ddldistrict.Items.Clear();
            ddldistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("- Select District -", "0"));
            lbldistrict.Visible = false;
            ddldistrict.Visible = false;
            lblpincode.Visible = false;
            txtpincode.Visible = false;
            ddlLocationAddress.Visible = false;
            lblLocationAddress.Visible = false;
            ddlAffixationType.ClearSelection();
            btnAdd2.Visible = false;
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
                    DateTime regDate = DateTime.ParseExact(txtVehRegDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    string savedate = regDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    string fuelType = ddlFuelType.SelectedItem.Text.ToString();
                    string fuelType2 = ddlnew.SelectedItem.Text.ToString();

                    string orderType = ddlOrderType.SelectedItem.Text.ToString();
                    string manuDate = HSRPAuthDate.SelectedDate.ToString();

                    string vehFLaserCode = txtfronlaser.Text.ToString().Trim();
                    string vehRLaserCode = txtRearlaser.Text.ToString().Trim();
                    string fitmentType = ddlAffixationType.SelectedValue.ToString();
                    string fitmentState = string.Empty;
                    string fitmentLocation = string.Empty;
                    string fitmentLocationID = string.Empty;
                    string fitmentHome = string.Empty;
                    string fitmentHomePin = string.Empty;
                    
                    if (vehFLaserCode != "" && vehRLaserCode != "")
                    {
                        try
                        {
                            /*
                             * Start validation from DB
                             */
                           

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


                            string ordertype = string.Empty;
                            string query = string.Empty;
                            
                            if ((vehFLaserCode.Substring(0, 2) != "AA") || (vehRLaserCode.Substring(0, 2) != "AA"))
                            {
                                Response.Redirect("https://www.bookmyhsrp.com/", true);
                            }

                            if ((vehFLaserCode.Length < 11))
                            {
                                Response.Redirect("https://www.bookmyhsrp.com/", true);
                            }                           

                            DataTable dt1 = new DataTable();
                            string strstickerPrinterFacility = string.Empty;
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


                            DataTable ds = new DataTable();
                           
                            query = "select a.HSRPRecordID, a.RegistrationDate, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,isnull(a.HSRP_Front_LaserCode,'') HSRP_Front_LaserCode,isnull(a.HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID " +
                              "where  a.vehicleRegNo='" + vehRegNo + "'  and a.HSRP_Front_LaserCode = '" + vehFLaserCode + "'  and a.HSRP_Rear_LaserCode = '" + vehRLaserCode + "' ";
                           
                            ds = Utils.GetDataTable(query, ConnectionStringDL);

                            if (ds.Rows.Count == 0)
                            {
                                query = "select a.HSRPRecordID, a.RegistrationDate, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,isnull(a.HSRP_Front_LaserCode,'') HSRP_Front_LaserCode,isnull(a.HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID " +
                                   "where  a.vehicleRegNo='" + vehRegNo + "'  " +
                                   "and a.HSRP_Front_LaserCode = '" + vehFLaserCode + "'  and a.HSRP_Rear_LaserCode = '" + vehRLaserCode + "'   ";
                                ds = Utils.GetDataTable(query, ConnectionString);                               
                            }                            

                            if(ds.Rows.Count == 0)
                            {
                                query = "select a.HSRPRecordID, a.RegistrationDate, a.HSRP_Sticker_LaserCode, a.StickerMandatory, a.VehicleRegNo,a.EngineNo,a.ChassisNo,isnull(a.HSRP_Front_LaserCode,'') HSRP_Front_LaserCode,isnull(a.HSRP_Rear_LaserCode,'') HSRP_Rear_LaserCode,b.RTOLocationname ,b.RTOLocationCode , HSRPState.HSRPStateName ,HSRPState.statetext from HSRPRecords a inner join rtolocation  as b on a.RTOLocationID =b.RTOLocationID inner join HSRPState  on HSRPState.HSRP_StateID= b.HSRP_StateID " +
                                   "where  a.vehicleRegNo='" + vehRegNo + "'  " +
                                   "and a.HSRP_Front_LaserCode = '" + vehFLaserCode + "'  and a.HSRP_Rear_LaserCode = '" + vehRLaserCode + "'   ";

                                ds = Utils.GetDataTable(query, ConnectionStringHR);
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
                                string registrationDate = ds.Rows[0]["RegistrationDate"].ToString();
                                string ValidDateTime = "01/04/2019";                               
                                string VehicleRegNo = txtRegNumber.Text.ToString();
                                string vehHR = VehicleRegNo.Substring(0, 2).Trim();
                                
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

                                        string address = "";
                                        string navembid = "";
                                        string state_id = "";
                                        string pincode = "";
                                        string rtolocationid = "";
                                        DataTable dtadd = new DataTable(); 
                                        

                                        string isBookMyHSRP = string.Empty;
                                        string queryfetchstatusofBookMyHSRP = "select Count(HSRPrecordId) as BookMyHSRPExist from HSRPRecords where  Vehicleregno = '" + vehRegNo + "' and  IsBookMyHsrpRecord='Y'";
                                        DataTable dtstatusofBookMyHSRP = Utils.GetDataTable(queryfetchstatusofBookMyHSRP, ConnectionString);

                                        isBookMyHSRP = dtstatusofBookMyHSRP.Rows[0]["BookMyHSRPExist"].ToString();

                                        if (fuelType != "-Select Fuel type-")
                                        {
                                            fulesticker = fuelType;
                                        }
                                        if (fuelType2 != "-Select Fuel type-")
                                        {
                                            fulesticker = fuelType2;
                                        }

                                        DataTable dtt = new DataTable();

                                        if (ddlAffixationType.SelectedValue == "1")
                                        {
                                            string navembidquery = "select top 1 rtolocationid, NAVEMBID from rtolocation where RTOLocationID in (select RTOLocationID from DealerAffixation where SubDealerId = " + ddlLocationAddress.SelectedValue + ")";
                                            dtt = Utils.GetDataTable(navembidquery, ConnectionString);
                                            navembid = dtt.Rows[0]["NAVEMBID"].ToString();
                                            address = ddlLocationAddress.SelectedItem.Text.ToString();
                                            state_id = ddlstate.SelectedValue.ToString();
                                            pincode = null;
                                            rtolocationid = dtt.Rows[0]["rtolocationid"].ToString();
                                        }
                                        if (ddlAffixationType.SelectedValue == "2")
                                        {
                                            string navembidquery = "select top 1 NAVEMBID from rtolocation where RTOLocationID = '" + ddldistrict.SelectedValue + "'";
                                            dtt = Utils.GetDataTable(navembidquery, ConnectionString);
                                            navembid = dtt.Rows[0]["NAVEMBID"].ToString();
                                            address = txtHomeAddress.Text.ToString();
                                            state_id = ddlstate.SelectedValue.ToString();
                                            pincode = txtpincode.Text.ToString();
                                            rtolocationid = ddldistrict.SelectedValue;
                                        }

                                        if(registrationDate == "")
                                        {
                                            registrationDate = txtVehRegDate.Text.ToString();
                                        }
                                      
                                        if ((vehHR == "HR") && (DateTime.Parse(registrationDate, CultureInfo.InvariantCulture) < DateTime.Parse(ValidDateTime, CultureInfo.InvariantCulture)))
                                        {
                                            sqlQuery = "StrickerOnlyEntryFRLaserSuccess_HR '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" +
                                            ChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" +
                                            savedate + "', '" + fulesticker + "', '" + orderType + "', '" + manuDate + "', '" +
                                            vehFLaserCode + "', '" + vehRLaserCode + "', '" + oemid + "', '" + dealerid + "', '" + state_id + "', '" + USERID + "','" + VehicleStateType + "','N','" + address + "','" + navembid + "','" + pincode + "','" + ddlAffixationType.SelectedValue + "','"+ rtolocationid +"' ";
                                        }
                                        else
                                        {
                                            sqlQuery = "USP_StrickerOnlyEntryFRLaserSuccess '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" +
                                            ChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" +
                                            savedate + "', '" + fulesticker + "', '" + orderType + "', '" + manuDate + "', '" +
                                            vehFLaserCode + "', '" + vehRLaserCode + "', '" + oemid + "', '" + dealerid + "', '" + state_id + "', '" + USERID + "','" + VehicleStateType + "','N','" + address + "','" + navembid + "','" + pincode + "','" + ddlAffixationType.SelectedValue + "','"+ rtolocationid +"' ";

                                        }


                                        dt = Utils.GetDataTable(sqlQuery, ConnectionString);

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

                                        string Vehicleregno1 = txtRegNumber.Text.Trim();

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


                                        string VehicleRegDate = savedate.Replace("/", "-");
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
                                        lblSucMess.Text = "Record saved successfully. Delivery from EC. Fee charged 50+GST";
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
                DateTime regDate = DateTime.ParseExact(txtVehRegDate.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string savedate = regDate.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string fuelType = string.Empty;
                if (ddlVehicleStateType.SelectedItem.Text.ToString() == "BS6")
                {
                    fuelType = ddlnew.SelectedItem.Text.ToString();
                }
                else
                {
                    fuelType = ddlFuelType.SelectedItem.Text.ToString();
                }
                            
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

                    #region Upload ID Proof
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

                    string address = "";
                    string navembid = "";
                    string state_id = "";
                    string pincode = "";
                    string rtolocationid = "";
                    DataTable dtt = new DataTable();

                    if (ddlAffixationType.SelectedValue == "1")
                    {
                        string navembidquery = "select top 1 rtolocationid, NAVEMBID from rtolocation where RTOLocationID in (select RTOLocationID from DealerAffixation where SubDealerId = " + ddlLocationAddress.SelectedValue + ")";
                        dtt = Utils.GetDataTable(navembidquery, ConnectionString);
                        navembid = dtt.Rows[0]["NAVEMBID"].ToString();
                        address = ddlLocationAddress.SelectedItem.Text.ToString();
                        state_id = ddlstate.SelectedValue.ToString();
                        pincode = null;
                        rtolocationid = dtt.Rows[0]["rtolocationid"].ToString();
                    }
                    if (ddlAffixationType.SelectedValue == "2")
                    {
                        string navembidquery = "select top 1 NAVEMBID from rtolocation where RTOLocationID = '" + ddldistrict.SelectedValue + "'";
                        dtt = Utils.GetDataTable(navembidquery, ConnectionString);
                        navembid = dtt.Rows[0]["NAVEMBID"].ToString();
                        address = txtHomeAddress.Text.ToString();
                        state_id = ddlstate.SelectedValue.ToString();
                        pincode = txtpincode.Text.ToString();
                        rtolocationid = ddldistrict.SelectedValue;
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

                        sqlQuery = "USP_StrickerOnlyEntry_FRLaserEmpty '" + ownerName + "', '" + mobileNo + "', '" + vehRegNo + "', '" + vehChassisNo + "', '" + vehEngineNo + "', '" + vehicleType + "', '" + vehicleClass + "', '" + vehModel + "', '" + savedate + "', '" + fuelType + "', '" + orderType + "', '" + manuDate + "', '" + vehFLaserCode + "', '" + vehRLaserCode + "', '" + rcFileName + "', '" + idFileName + "', '" + flFileName + "' , '" + rlFileName + "' , '" + docType + "','" + oemid + "', '" + dealerid + "', '" + state_id + "', '" + USERID + "','" + VehicleStateType + "','" + address + "','" + navembid + "','" + pincode + "', '"+ ddlAffixationType.SelectedValue +"','"+ rtolocationid +"'";
                                             
                        DataTable dt = Utils.GetDataTable(sqlQuery, ConnectionString);

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["status"].ToString() == "1")
                            {
                                string msg = "Records saved successfully.";
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
                
                if (ddlOemName.SelectedItem.Text.Trim() == "-Select Oem-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select oem!";
                    return false;
                }
              
                if (ddlVehicleStateType.SelectedItem.Text.Trim() == "-Select-")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select vehicle stage type!";
                    return false;
                }

                if (txtRegNumber.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter vehicle reg no!";
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

                if (txtChassisno.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter chassis no!";
                    return false;
                }

                if (txtOwnerName.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter owner name!";
                    return false;
                }

                if (txtmobileno.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter mobile no!";
                    return false;
                }

                if (txtmobileno.Text.Trim() != "")
                {
                    string strMobileNo = txtmobileno.Text.Trim();
                    if (strMobileNo.Length != 10)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please enter valid mobile no!";
                        return false;
                    }
                    foreach (var item in specialChar)
                    {
                        if (strMobileNo.Contains(item))
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Special characters not allowed in mobile no!";
                            return false;
                        }
                    }
                }
                             
                if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter valid engine no!";
                    return false;
                }

                foreach (var item in specialChar)
                {
                    if (txtEngineNo.Text.Trim().Contains(item))
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Special characters not allowed in engine no!";
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

                if (txtVehRegDate.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter registration date!";
                    return false;
                }


                //try
                //{
                //    //Verify whether date entered in dd/MM/yyyy format.
                //    //bool isValid = regexDate.IsMatch(OrderDate.SelectedDate.ToString());
                //    bool isValid = regexDate.IsMatch(txtVehRegDate.Text.ToString());

                //    //Verify whether entered date is Valid date.
                //    DateTime dt;
                //    //isValid = DateTime.TryParseExact(OrderDate.SelectedDate.ToString(), "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
                //    isValid = DateTime.TryParseExact(txtVehRegDate.Text.ToString(), "dd/MM/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out dt);
                //    DateTime regMDate = new DateTime();
                //    DateTime todate = new DateTime();

                //    if (isValid)
                //    {
                //        //regMDate = DateTime.Parse(Convert.ToDateTime(OrderDate.SelectedDate.ToString()).ToShortDateString());
                //        regMDate = DateTime.Parse(Convert.ToDateTime(txtVehRegDate.Text.ToString()).ToShortDateString());
                //        todate = DateTime.Parse(Convert.ToDateTime("01/04/2019").ToShortDateString());
                //        if (regMDate > todate)
                //        {
                //            lblErrMess.Visible = true;
                //            lblErrMess.Text = "Choose Vehicle Reg. date before 01/04/2019";
                //            return false;
                //        }
                //    }
                //    else
                //    {
                //        lblErrMess.Visible = true;
                //        lblErrMess.Text = "Vehicle Reg. date invalid";
                //        return false;
                //    }


                //    //regMDate = DateTime.Parse(Convert.ToDateTime(OrderDate.SelectedDate.ToString()).ToShortDateString());
                //    regMDate = DateTime.Parse(Convert.ToDateTime(txtVehRegDate.Text.ToString()).ToShortDateString());
                //    todate = DateTime.Parse(Convert.ToDateTime("01/04/2019").ToShortDateString());
                //    if (regMDate > todate)
                //    {
                //        lblErrMess.Visible = true;
                //        lblErrMess.Text = "Choose Vehicle Reg. date before 01/04/2019";
                //        return false;
                //    }

                //}
                //catch (Exception ev)
                //{

                //}



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
                    lblErrMess.Text = "Please enter front laser code!";
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
                            lblErrMess.Text = "Special characters not allowed in Front Laser Code. !";
                            return false;
                        }
                    }
                }

                if (txtRearlaser.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter rear laser code!";
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
                            lblErrMess.Text = "Special characters not allowed in Rear Laser Code. !";
                            return false;
                        }
                    }
                }

                if (ddlAffixationType.SelectedValue == "0")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select fitment address!";
                    return false;
                }

                if (ddlAffixationType.SelectedValue == "1")
                {
                    if (ddlstate.SelectedValue == "0")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please select state!";
                        return false;
                    }

                    if (ddlLocationAddress.SelectedValue == "0")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please select fitment location!";
                        return false;
                    }

                }

                if (ddlAffixationType.SelectedValue == "2")
                {

                    if (ddlstate.SelectedValue == "0")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please select state!";
                        return false;
                    }

                    if (ddldistrict.SelectedValue == "0")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please select district!";
                        return false;
                    }

                    if (txtHomeAddress.Text == "")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please enter address!";
                        return false;
                    }

                    if (txtpincode.Text == "")
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Please enter pincode!";
                        return false;
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

        protected void btnAdd2_Click(object sender, EventArgs e)
        {
            string sticker = "sticker";
            Response.Redirect("../Transaction/AffixAddressUpdate.aspx?Through="+sticker,true);
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

        public string rosmerta_API_2(string vehRegNo, string chasiNo, string EngineNo, string Key)
        {
            string html = string.Empty;


            string decryptedString = string.Empty;

            try
            {

                string url = @"" + ConfigurationManager.AppSettings["VehicleStatusAPI2"].ToString() + "?VehRegNo=" + vehRegNo + "&ChassisNo=" + chasiNo + "&EngineNo=" + EngineNo + "&X=" + Key + "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();

                }

            }
            catch (Exception ev)
            {
                html = "Error While Calling Vahan Service - " + ev.Message;
            }


            return html;
        }

        class VehicleDetails
        {
            public string message { get; set; }
            public string fuel { get; set; }
            public string maker { get; set; }
            public string vchType { get; set; }
            public string norms { get; set; }
            public string vchCatg { get; set; }
            public string regnDate { get; set; }
        }
    }
}