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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Text;
using HSRP.SMLService;
using System.Security.Cryptography;
using iTextSharp.text.html.simpleparser;
using System.Drawing;

namespace HSRP.Transaction
{
    public partial class NewCashReceiptDataEntryDFDRDB : System.Web.UI.Page
    {
        static String ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        static String ConnStringBMHSRP = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringBMHSRP"].ToString();
        static String ConnectionStringHR = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringHR"].ToString();
        static string FileRequestPath = ConfigurationManager.AppSettings["RequestFolder"].ToString();
        [Obsolete]
        string ParivahanAPI = ConfigurationSettings.AppSettings["ParivahanAPI"].ToString();
        [Obsolete]
        SqlConnection con = new SqlConnection(ConnectionString);
        SqlConnection conHR = new SqlConnection(ConnectionStringHR);
        string HSRPStateID = string.Empty;
        string RTOLocationID = string.Empty;
        string UserType = string.Empty;
        string UserName = string.Empty;
        string Sticker = string.Empty;
        string USERID = string.Empty;
        DataTable dt = new DataTable();
        string dealerid = string.Empty;
        string macbase = string.Empty;
        string strFrmDateString = string.Empty;
        string strToDateString = string.Empty;
        string oemid = string.Empty;
        string lsOriginalValue = string.Empty;
        string checkEngineno = string.Empty;
        string url = string.Empty;
        string Query = string.Empty;
        string hsrprecord_authorizationno;
        string GSTIN = string.Empty;
        int length;
        string delaerId = string.Empty;
        string specialChar = @"'€";
        string specialCharCE = @"'€";
        string checkQuery = string.Empty;
        string maker1 = string.Empty;
        string IS_OEM_Name_Match = string.Empty;
        string QryFindOEMName = string.Empty;
        string sqlstring = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
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
            DataTable dtdealergstin = Utils.GetDataTable("Select top 1 gstin from dealerMaster  where DealerId ='" + dealerid + "' and  HSRP_StateID= '" + HSRPStateID + "'", ConnectionString);


            if (!IsPostBack)
            {

                txtRegNumber.Attributes.Add("onKeyPress", "doClick('" + btnSave2.ClientID + "',event)");
                txtChassisno.Attributes.Add("onKeyPress", "doClick('" + btnSave2.ClientID + "',event)");
                txtEngineNo.Attributes.Add("onKeyPress", "doClick('" + btnSave2.ClientID + "',event)");
                txtmodel.Attributes.Add("onKeyPress", "doClick('" + btnSave2.ClientID + "',event)");


                InitialSetting();

                string strIsvahan = @"select isVahan from Dealermaster where dealerid = '" + dealerid + "'";

                DataTable dtchkIsvahan = Utils.GetDataTable(strIsvahan, ConnectionString);

                string SkipVehicleType = @"select VEHICLETYPE from OEMMaster where OEMID = '" + oemid + "'";

                DataTable dtchkVehicleType = Utils.GetDataTable(SkipVehicleType, ConnectionString);


                if ((HSRPStateID == "10") || (dtchkVehicleType.Rows[0]["VEHICLETYPE"].ToString().Trim() == "Trailer & Trolley Supplement") || (dtchkIsvahan.Rows[0]["IsVahan"].ToString() == "N"))
                {
                    btnGO2.Visible = false;
                    ddlVehicleStateType.Enabled = true;
                    ddlVehicleclass.Enabled = true;
                    ddlFuelType.Enabled = true;
                    OrderDate.Enabled = true;
                    calendar_from_button.Visible = true;
                }

                else
                {
                    btnGO2.Visible = true;
                    ddlVehicleStateType.Enabled = false;
                    ddlVehicleclass.Enabled = true;
                    ddlFuelType.Enabled = false;
                    calendar_from_button.Visible = false;
                    OrderDate.Enabled = false;
                }

                BindVehicleDropdownlistwithSP(oemid);
                //DropdownAffixation();
                bindState();
            }
        }

        private void BindVehicleDropdownlistwithSP(string OemIdSP)
        {
            string SQLString = "USP_BindVehicleTypeDropDownlist " + OemIdSP + " "; //"select distinct  ChallanNo as  ChallanNo, ChallanNo as  ChallanName from    hsrprecords h join RTOLocation R on h.RTOLocationID=R.RTOLocationID where H.HSRP_StateID='" + stateid + "' and R.Navembcode='" + EmbossingLocation + "' and Challanno not like '%TVS%' and  ChallanNo not in(select distinct Challanno from  HSRPDispatch)  "; //and ChallanDate >='2019-06-17' 

            DataTable dtable = Utils.GetDataTable(SQLString, ConnectionString);
            if (dtable.Rows.Count > 0)
            {

                ddlVehicletype.DataSource = dtable;
                ddlVehicletype.DataBind();

                ddlVehicletype.DataValueField = "VehicleType";
                ddlVehicletype.DataTextField = "VehicleTypeNew";
                ddlVehicletype.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Vehicle Type-", "-Select Vehicle Type-"));
                // ddlVehicletype.Items.Remove("MCV/HCV/Trailers");

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
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        public void Cleardatasave()
        {
            txtEngineNo.Text = "";
            txtmodel.Text = "";
            txtRegNumber.Text = "";
            txtChassisno.Text = "";
            txtmodel.ReadOnly = false;
            txtRegNumber.ReadOnly = false; ;
            txtEngineNo.ReadOnly = false;
            txtChassisno.ReadOnly = false;
            ddlVehicletype.Enabled = true;
            ddlVehicleclass.Enabled = true;
            ddlVehicletype.ClearSelection();
            ddlVehicleclass.ClearSelection();
            ddlFuelType.ClearSelection();
            ddlVehicleStateType.ClearSelection();
            ddlAffixationType.ClearSelection();
            ddlLocationAddress.ClearSelection();
            lblHomeAddress.Visible = false;
            txtHomeAddress.Visible = false;
            txtHomeAddress.Text = "";
            ddlLocationAddress.Visible = false;
            lblLocationAddress.Visible = false;
            divdevtype.Visible = false;
            divvehclass.Visible = false;
            divvehclassddl.Visible = false;
            divregdate.Visible = false;
            divregdate2.Visible = false;
            ddlstate.ClearSelection();
            ddldistrict.ClearSelection();
            txtpincode.Text = "";
            divfuel.Visible = false;
            divfuel2.Visible = false;
            divvehstage.Visible = false;
            divvehstage2.Visible = false;
            divvehtype.Visible = false;
            divvehtype2.Visible = false;
            divmodel.Visible = false;
            divmodel2.Visible = false;
            lblErrMess.Visible = false;
            divdevtype.Visible = false;
            btnGO2.Visible = true;
            btnAdd2.Visible = false;
            btnSave2.Visible = false;
            divfitment.Visible = false;
            divdocument.Visible = false;
            divdocument2.Visible = false;
            divdocument3.Visible = false;
            hr1.Visible = false;
            hr2.Visible = false;
            //btnPrint.Visible = true;
            lblpincode.Visible = false;
            txtpincode.Visible = false;
            InitialSetting();
            HiddenFIR.Value = null;
            HiddenRCPath.Value = null;
            HiddenFlaser.Value = null;
            HiddenRearLaser.Value = null;

            VehicleDetails _vd = new VehicleDetails();
            _vd.message = "";
            _vd.fuel = "";
            _vd.maker = "";
            _vd.vchType = "";
            _vd.norms = "";

            _vd.vchCatg = "";
            _vd.regnDate = "";

            ddlVehicleclass.Items.Clear();
            ddlVehicleclass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Vehicle Class-", "-Select Vehicle Class-"));
            ddlVehicleclass.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Transport", "Transport"));
            ddlVehicleclass.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Non-Transport", "Non-Transport"));
            ddlVehicleclass.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Rent A CAB", "Rent A CAB"));


            ddlFuelType.Items.Clear();
            ddlFuelType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Fuel type-", "-Select Fuel type-"));
            ddlFuelType.Items.Insert(1, new System.Web.UI.WebControls.ListItem("PETROL", "PETROL"));
            ddlFuelType.Items.Insert(2, new System.Web.UI.WebControls.ListItem("CNG/PETROL", "CNG/PETROL"));
            ddlFuelType.Items.Insert(3, new System.Web.UI.WebControls.ListItem("DIESEL", "DIESEL"));
            ddlFuelType.Items.Insert(4, new System.Web.UI.WebControls.ListItem("DIESEL/HYBRID", "DIESEL/HYBRID"));

            ddlFuelType.Items.Insert(5, new System.Web.UI.WebControls.ListItem("PETROL/HYBRID", "PETROL/HYBRID"));
            ddlFuelType.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Electricity", "Electricity"));
            ddlFuelType.Items.Insert(7, new System.Web.UI.WebControls.ListItem("LPG/PETROL", "LPG/PETROL"));
            ddlFuelType.Items.Insert(8, new System.Web.UI.WebControls.ListItem("CNG", "CNG"));


            ddlVehicleStateType.Items.Clear();
            ddlVehicleStateType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Vehicle Stage Type-", "-Select Vehicle Stage Type-"));
            ddlVehicleStateType.Items.Insert(1, new System.Web.UI.WebControls.ListItem("BS4 or Other", "BS4"));
            ddlVehicleStateType.Items.Insert(2, new System.Web.UI.WebControls.ListItem("BS6", "BS6"));
            ddlVehicleStateType.Items.Insert(3, new System.Web.UI.WebControls.ListItem("BS3", "BS3"));

            string SkipVehicleType = @"select VEHICLETYPE from OEMMaster where OEMID = '" + oemid + "'";

            DataTable dtchkVehicleType = Utils.GetDataTable(SkipVehicleType, ConnectionString);

            string strIsvahan = @"select isVahan from Dealermaster where dealerid = '" + dealerid + "'";
            DataTable dtchkIsvahan = Utils.GetDataTable(strIsvahan, ConnectionString);

            if ((HSRPStateID == "10") || (dtchkVehicleType.Rows[0]["VEHICLETYPE"].ToString().Trim() == "Trailer & Trolley Supplement") || (dtchkIsvahan.Rows[0]["IsVahan"].ToString() == "N"))
            {
                btnGO2.Visible = false;
                ddlVehicleStateType.Enabled = true;
                ddlVehicleclass.Enabled = true;
                ddlFuelType.Enabled = true;
                OrderDate.Enabled = true;
                calendar_from_button.Visible = true;
            }

            else
            {
                btnGO2.Visible = true;
                ddlVehicleStateType.Enabled = false;
                ddlVehicleclass.Enabled = true;
                ddlFuelType.Enabled = false;
                calendar_from_button.Visible = false;
                OrderDate.Enabled = false;
            }

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

        protected void ddlOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlOrderType.SelectedValue == "DB")
            {
                divflaser.Visible = false;
                divflaser2.Visible = false;
                divrlaser.Visible = false;
                divrlaser2.Visible = false;
            }
            if (ddlOrderType.SelectedValue == "DF")
            {
                divflaser.Visible = false;
                divflaser2.Visible = false;
                divrlaser.Visible = true;
                divrlaser2.Visible = true;
            }
            if (ddlOrderType.SelectedValue == "DR")
            {
                divflaser.Visible = true;
                divflaser2.Visible = true;
                divrlaser.Visible = false;
                divrlaser2.Visible = false;
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

        protected void btnSave2_Click(object sender, EventArgs e)
        {
            #region Feilds Validation

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
                        lblErrMess.Text = "Special characters not allowed in vehicle registration no!";
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

            foreach (var item in specialCharCE)
            {
                if (txtChassisno.Text.Trim().Contains(item))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Special characters not allowed in chassis no!";
                    return;
                }
            }


            if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please enter valid engine no!";
                return;
            }

            checkEngineno = txtEngineNo.Text.ToString().Trim();
            if (checkEngineno.Length <= 4)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please enter valid engine no!";
                return;
            }

            foreach (var item in specialCharCE)
            {
                if (txtEngineNo.Text.Trim().Contains(item))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Special characters not allowed in engine no!";
                    return;
                }
            }


            if (ddlVehicleclass.SelectedItem.Text.ToString() == "-Select Vehicle Class-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select vehicle class!";
                return;
            }
            if (ddlFuelType.SelectedItem.Text.ToString() == "-Select Fuel type-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select fuel type!";
                return;
            }
            if (ddlVehicleStateType.SelectedItem.Text.ToString() == "-Select Vehicle Stage Type-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select vehicle stage type!";
                return;
            }
            if (ddlVehicletype.SelectedItem.Text.ToString() == "-Select Vehicle Type-")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select vehicle type!";
                return;
            }
            if (ddlOrderType.SelectedValue == "0")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select order type!";
                return;
            }

            //if (string.IsNullOrEmpty(txtmodel.Text.Trim()))
            //{
            //    lblErrMess.Visible = true;
            //    lblErrMess.Text = "Please enter model!";
            //    return;
            //}          

            if (ddlAffixationType.SelectedValue == "0")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please select fitment address!";
                return;
            }

            if (ddlAffixationType.SelectedValue == "1")
            {
                if (ddlstate.SelectedValue == "0")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select state!";
                    return;
                }

                if (ddlLocationAddress.SelectedValue == "0")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select fitment location!";
                    return;
                }

            }

            if (ddlAffixationType.SelectedValue == "2")
            {

                if (ddlstate.SelectedValue == "0")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select state!";
                    return;
                }

                if (ddldistrict.SelectedValue == "0")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please select district!";
                    return;
                }

                if (txtHomeAddress.Text == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter address!";
                    return;
                }

                if (txtpincode.Text == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter pincode!";
                    return;
                }

            }

            #endregion


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


            #region Upload FIR
            if (FIRUploader.HasFile)
            {
                int _size = 3072;// equal 3 mb
                string _fileExt = System.IO.Path.GetExtension(FIRUploader.FileName);

                if (_fileExt.ToLower() == ".png" || _fileExt.ToLower() == ".jpg" || _fileExt.ToLower() == ".jpeg" || _fileExt.ToLower() == ".pdf")
                {
                    if ((FIRUploader.PostedFile.ContentLength / 1024) <= _size)
                    {
                        string FileName = System.IO.Path.GetFileName(FIRUploader.FileName);

                        FileName = txtRegNumber.Text + "-ID-" + System.DateTime.Now.Day.ToString() + System.DateTime.Now.Month.ToString() + System.DateTime.Now.Year.ToString() + System.DateTime.Now.Hour.ToString() + System.DateTime.Now.Minute.ToString() + _fileExt;
                        //string path = FileUploadDir + "";
                        if (!Directory.Exists(FileRequestPath))
                        {
                            Directory.CreateDirectory(FileRequestPath);
                        }
                        string Filepath = FileRequestPath + FileName;

                        FIRUploader.SaveAs(Filepath);
                        HiddenFIR.Value = FileName;

                    }
                    else
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.ForeColor = Color.Maroon;
                        lblErrMess.Text = "FIR Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                        return;
                    }
                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.ForeColor = Color.Maroon;
                    lblErrMess.Text = "FIR Image Should be in png/jpg/jpeg/pdf format or less than 3 mb";
                    return;
                }
            }
            else
            {
                lblErrMess.Visible = true;
                lblErrMess.ForeColor = Color.Maroon;
                lblErrMess.Text = "Please upload FIR Image..";
                return;
            }

            #endregion


            #region Upload Front Laser
            //if((ddlOrderType.SelectedValue == "DB") || (ddlOrderType.SelectedValue == "DR"))
            if(ddlOrderType.SelectedValue == "DR")
            {
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
            }

            #endregion


            #region Upload Rear Laser

            //if((ddlOrderType.SelectedValue == "DB") || (ddlOrderType.SelectedValue == "DF")) 
            if(ddlOrderType.SelectedValue == "DF")
            {
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
            }
           
            #endregion


            url = HttpContext.Current.Request.Url.AbsoluteUri;

            DataTable dtdealergstin = Utils.GetDataTable("Select top 1 gstin from dealerMaster  where DealerId ='" + dealerid + "' ", ConnectionString);

            GSTIN = dtdealergstin.Rows[0]["gstin"].ToString();

            if ((GSTIN == "") || (GSTIN == null))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Your GSTIN cannot be blank, kindly update the GSTIN.";
                return;
            }

            length = GSTIN.Length;

            if (length != 15)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Your GSTIN is incorrect, kindly correct the GSTIN.";
                return;
            }

            string strDate = OrderDate.SelectedDate.ToString("dd/MM/yyyy");
            String[] StringAuthDate = strDate.Replace("-", "/").Split('/');
            string MonTo = ("0" + StringAuthDate[0]);
            string MonthdateTO = MonTo.Replace("00", "0").Replace("01", "1");
            String ReportDateFrom = StringAuthDate[1] + "/" + MonthdateTO + "/" + StringAuthDate[2].Split(' ')[0];
            String From = StringAuthDate[0] + "/" + StringAuthDate[1] + "/" + StringAuthDate[2].Split(' ')[0];
            string AuthorizationDate = From + " 00:00:00";

            String[] StringOrderDate = HSRPAuthDate.SelectedDate.ToString().Replace("-", "/").Split('/');
            string Mon = ("0" + StringOrderDate[0]);
            string Monthdate = Mon.Replace("00", "0").Replace("01", "1");
            String FromDate = StringOrderDate[1] + "-" + Monthdate + "-" + StringOrderDate[2].Split(' ')[0];
            String ReportDateTo = StringOrderDate[1] + "/" + Monthdate + "/" + StringOrderDate[2].Split(' ')[0];
            String From1 = StringOrderDate[0] + "/" + StringOrderDate[1] + "/" + StringOrderDate[2].Split(' ')[0];
            string ToDate = From1 + " 23:58:00";

            strFrmDateString = OrderDate.SelectedDate.ToShortDateString() + " 00:00:00";
            strToDateString = HSRPAuthDate.SelectedDate.ToShortDateString() + " 00:00:00";

            //string specialCharCE = @" \|{}%_!'#$%&'()+,‐./:;<=>?@<>@^£§€";
            string validDatetime = "2019-04-01";

            try
            {
                #region Registration Date Validation Check

                DateTime CurrentDatetime = System.DateTime.Now;
                if (Convert.ToDateTime(strFrmDateString) > CurrentDatetime)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Date of registration can not be greater than current date!";
                    return;
                }

                if (Convert.ToDateTime(strToDateString) > CurrentDatetime)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Date of Manufacture can not be greater than current date!";
                    return;
                }

                #endregion

               
                string VehicleRego = txtRegNumber.Text.ToString().Trim();
                string Chassisno = txtChassisno.Text.Trim().ToString();
                string strChassisno = Chassisno.Substring(Chassisno.Length - 5, 5);
                string Engineno = txtEngineNo.Text.ToString().Trim();
                string strEngineno = string.Empty;
                strEngineno = Engineno.Substring(Engineno.Length - 5, 5);

                string responseJson = rosmerta_API_2(txtRegNumber.Text.ToString().Trim().ToUpper(), strChassisno, strEngineno, "5UwoklBqiW");
                VehicleDetails _vd = JsonConvert.DeserializeObject<VehicleDetails>(responseJson);


                QryFindOEMName = @"select OEMID,OemName from oemvahanmapping WHERE OemName='" + _vd.maker + "'";
                DataTable dt = Utils.GetDataTable(QryFindOEMName, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    oemid = dt.Rows[0]["OEMID"].ToString();
                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Message :" + _vd.message;
                    return;
                }

                #region Rosmerta API commented
                if ((txtChassisno.Text.Trim().ToString() != "") && (txtRegNumber.ToString().Trim() != "") && (txtEngineNo.Text.Trim() != ""))
                {
                    string response = string.Empty;
                    string query3 = string.Empty;

                    if (Engineno != "NA")
                    {
                        strEngineno = Engineno.Substring(Engineno.Length - 5, 5);
                    }
                    else
                    {
                        strEngineno = "NA";
                    }

                    delaerId = Session["dealerid"].ToString();

                    //response = Utils.ValidationVehicleCHassisEngineno(VehicleRego, strChassisno, strEngineno, oemId, delaerId);

                    //if (response != "")
                    //{
                    //    if (response != "Vehicle Present and you are authorized vendor for this vehicle")
                    //    {
                    //        lblErrMess.Visible = true;
                    //        lblErrMess.Text = response + " -Validation Failed. ";
                    //        return;
                    //    }
                    //}
                }
                #endregion

                #region SMLISUZU API

                if ((txtChassisno.Text.Trim().ToString() != "") && (txtRegNumber.ToString().Trim() != "") && (txtEngineNo.Text.Trim() != "") && ((oemid == "23") || (oemid == "1241")))
                {

                    string response = string.Empty;
                    if (Engineno != "NA")
                    {
                        strEngineno = Engineno.Substring(Engineno.Length - 5, 5);
                    }
                    else
                    {
                        strEngineno = "NA";
                    }


                    delaerId = Session["dealerid"].ToString();

                    string responseJsonSML = SMLISUZU_API_2(Chassisno, Engineno);

                    if (responseJsonSML != "")
                    {

                        string query = "insert into SMLISUZULog(Vehicleregno,Chassisno,EngineNo,Resposnse, Dealerid,CreatedBy) values('" + VehicleRego + "','" + Chassisno + "','" + Engineno + " ','" + responseJson + "', '" + delaerId + "', '" + delaerId + "'  )";
                        SqlConnection conn = new SqlConnection(ConnectionString);
                        conn.Open();
                        SqlCommand cmd2 = new SqlCommand(query, conn);
                        cmd2.ExecuteNonQuery();

                        conn.Close();

                        if (responseJson != "VALID")
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Invalid Chassis & Engine no!";
                            return;
                        }


                    }
                }

                #endregion

                #region Vahan Validation checking (code edit by Ashok:22-01-2022)

                string strIsvahan = @"select isVahan from Dealermaster where dealerid = '" + dealerid + "'";

                DataTable dtchkIsvahan = Utils.GetDataTable(strIsvahan, ConnectionString);

                string VehicleRegoE = txtRegNumber.Text.ToString().Substring(0, 2).Trim();

                if ((HSRPStateID != "10") && (VehicleRegoE != "TS") && (dtchkIsvahan.Rows[0]["IsVahan"].ToString() == "Y"))
                {
                    try
                    {
                        if (checkEngineno != "NA")
                        {
                            strEngineno = Engineno.Substring(Engineno.Length - 5, 5);
                        }
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

                            strquery = "insert into VahanLog values('" + dealerid + "','" + oemid + "','" + vahanstatus + "','" + fueltype + "','" + maker + "','" + vehicletype + "','" + norms + "','" + vehiclecatg + "','" + regdate + "','" + USERID + "', getdate(),'" + txtRegNumber.Text + "','" + txtChassisno.Text + "','" + txtEngineNo.Text + "')";
                            Utils.ExecNonQuery(strquery, ConnectionString);

                            string InPutRegdate = Convert.ToDateTime(strFrmDateString).ToString("yyyy-MM-dd");

                            string vehcatg = _vd.vchCatg;



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

                                            if ((_vd.regnDate != InPutRegdate) && (_vd.regnDate != ""))
                                            {

                                                lblErrMess.Visible = true;
                                                lblErrMess.Text = "Kindly  select  correct Regdate!";
                                                return;
                                            }

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
                    catch (Exception EX)
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Soemthing went wrong while validating with vahan!";
                        throw;
                    }
                }

                #endregion

                #region Check Vehicle No. previous order not closed yet

                string Query = "select ApprovedStatus from hsrprecords_ApprovalMB where VehicleRegNo = '"+ VehicleRego + "' and ApprovedStatus = 'N'";
                dt = Utils.GetDataTable(Query,ConnectionString);
                if(dt.Rows.Count > 0)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Your order is pending for approval!";
                    return;
                }

                string Chassisno1 = txtChassisno.Text.Trim().ToString();                
                string Engineno1 = txtEngineNo.Text.ToString().Trim();
                string strEngineno1 = Engineno1.Substring(Engineno1.Length - 5, 5);
                DataTable dtregno = new DataTable();
                DataTable dtregnoHR = new DataTable();
                DataTable dtregnoHRDemo = new DataTable();
                string QueryHR = string.Empty;
                string QueryHRDemo = string.Empty;
                
                Query = "select vehicleregno from hsrprecords where vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "'";
                dtregno = Utils.GetDataTable(Query, ConnectionString);
                QueryHR = "select vehicleregno from hsrprecords_HR where vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "'";
                dtregnoHR = Utils.GetDataTable(QueryHR, ConnectionString);
                QueryHRDemo = "select vehicleregno from hsrprecords where vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "'";
                dtregnoHRDemo = Utils.GetDataTable(QueryHRDemo, ConnectionStringHR);

                if ((dtregno.Rows.Count == 0) && (dtregnoHR.Rows.Count == 0) && (dtregnoHRDemo.Rows.Count == 0))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please book order through NB screen.";
                    return;
                }

                Query = "select vehicleregno, orderstatus, challandate from hsrprecords where vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "' and orderstatus <>'Closed' order by HSRPRecordId desc";
                QueryHR = "select vehicleregno, orderstatus, challandate from hsrprecords_HR where vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "' and orderstatus <>'Closed' order by HSRPRecordId desc";
                dtregno = Utils.GetDataTable(Query, ConnectionString);
                dtregnoHR = Utils.GetDataTable(QueryHR, ConnectionString);
                
                if ((dtregno.Rows.Count > 0) || (dtregnoHR.Rows.Count > 0))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Check Vehicle No. previous order not closed yet!";
                    return;
                }

                //Query = "select top 1 vehicleregno from hsrprecords h join Dealermaster d on  h.dealerid=d.dealerid  where (vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "' or chassisno='" + txtChassisno.Text.Trim().ToString() + "' )  ";
                //QueryHR = "select top 1 vehicleregno from hsrprecords_HR h join Dealermaster d on  h.dealerid=d.dealerid  where (vehicleregno ='" + txtRegNumber.Text.Trim().ToString() + "' or chassisno='" + txtChassisno.Text.Trim().ToString() + "' ) ";
                //dtregno = Utils.GetDataTable(Query, ConnectionString);
                //dtregnoHR = Utils.GetDataTable(QueryHR, ConnectionString);
                //if ((dtregno.Rows.Count == 0) && (dtregnoHR.Rows.Count == 0))
                //{
                //    lblErrMess.Visible = true;
                //    lblErrMess.Text = "New Order is  not available  for this Dealer!";
                //    return;
                //}

                #endregion

                #region Amount Validation Check
                string fixcharge = "0.00";
                string SQLfixcharge = " select top 1 Dealerid  from dealermaster where dealerid='" + dealerid.ToString() + "'";
                DataTable dt11 = Utils.GetDataTable(SQLfixcharge, ConnectionString);
                if (dt11.Rows.Count > 0)
                {

                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "You are not a dealer!";
                    return;
                }

                string sticker1 = Sticker;
                DataTable dt5 = new DataTable();
                string cashrc = string.Empty;
                string authdate = string.Empty;
                string DC = string.Empty;

                DC = "AA111";
                cashrc = "ABC11";

                //sqlstring = "select (FitmentCharges+HomeDeliveryCharges+convenienceCharges+MRDCharges) as Charges from bmhsrpchargesmaster where typeofdelivery = '" + ddlAffixationType.SelectedValue + "'";
                sqlstring = "select cast(((FitmentCharges+HomeDeliveryCharges+convenienceCharges+MRDCharges)+(FitmentCharges+HomeDeliveryCharges+convenienceCharges+MRDCharges)*18/100 ) as  decimal(10,2)) as Charges from bmhsrpchargesmaster where typeofdelivery = '" + ddlAffixationType.SelectedValue + "'";
                DataTable dtCharges = Utils.GetDataTable(sqlstring, ConnectionString);
                decimal fitmentCharges = Convert.ToDecimal(dtCharges.Rows[0]["Charges"].ToString());
                string newStateid = "";
                DataTable dtrates = new DataTable();
                VehicleRegoE = txtRegNumber.Text.ToString().Substring(0, 2).Trim().ToUpper();
               
                try
                {
                    Query = "select oemid from OEMVahanMapping where OemName = '" + _vd.maker + "' and Oemid != 1005";
                    dt = Utils.GetDataTable(Query, ConnectionString);
                    oemid = dt.Rows[0]["oemid"].ToString();

                    if (Convert.ToDateTime(strFrmDateString) < Convert.ToDateTime(validDatetime))
                    {
                        if (VehicleRegoE == "HR")
                        {
                            newStateid = "4";
                            SqlConnection conn = new SqlConnection(ConnectionString);
                            SqlCommand cmdd = new SqlCommand("usp_BMHSRPOemRatesAndSize", conn);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.Parameters.AddWithValue("@Oemid", oemid);
                            cmdd.Parameters.AddWithValue("@Hsrp_stateid", newStateid);
                            cmdd.Parameters.AddWithValue("@OrderType", ddlOrderType.SelectedValue.ToString());
                            cmdd.Parameters.AddWithValue("@VehicleClass", ddlVehicleclass.SelectedItem.Text.ToString());
                            cmdd.Parameters.AddWithValue("@VehicleType", lblVehicleType.Text.ToString());
                            cmdd.Parameters.AddWithValue("@ShortVehicleregno", VehicleRegoE);
                            SqlDataAdapter da = new SqlDataAdapter(cmdd);
                            da.Fill(dtrates);
                            if (dtrates.Rows.Count == 0)
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.Text = "error amount, ref:('" + oemid + "'/'" + ddlVehicleclass.SelectedItem.Text.ToString() + "'/'" + ddlVehicletype.SelectedItem.Text.ToString() + "')";
                                return;
                            }
                        }
                        else
                        {
                            sqlstring = "select HsrpStateName from hsrpstate where hsrp_stateid = '" + HSRPStateID + "' and isbookmyhsrpstate = 'Y'";
                            dt = Utils.GetDataTable(Query, ConnectionString);

                            if (dt.Rows.Count > 0)
                            {
                                Query = "Select distinct FrontPlateSize,RearPlateSize,GstBasic_Amt,cgstper,cgstamt,sgstper,sgstamt,roundoff_netamount from OemRates where OemId = '" + oemid + "' and VehicleCLass = '" + ddlVehicleclass.SelectedItem.Text.ToString() + "' and VehicleType = '" + ddlVehicletype.SelectedItem.Text.ToString() + "' and Ordertype = '" + ddlOrderType.SelectedValue + "' ";
                                dtrates = Utils.GetDataTable(Query, ConnStringBMHSRP);

                                if (dtrates.Rows.Count == 0)
                                {
                                    lblErrMess.Visible = true;
                                    lblErrMess.Text = "error amount, ref:('" + oemid + "'/'" + ddlVehicleclass.SelectedItem.Text.ToString() + "'/'" + lblVehicleType.Text.ToString() + "')";
                                    return;
                                }
                            }
                            else
                            {
                                lblErrMess.Visible = true;
                                lblErrMess.Text = "You are not authorised to book the order!";
                                return;
                            }
                        }
                    }

                    else
                    {

                        SqlConnection conn = new SqlConnection(ConnectionString);
                        SqlCommand cmdd = new SqlCommand("OemRatesAndSize", conn);
                        cmdd.CommandType = CommandType.StoredProcedure;
                        cmdd.Parameters.AddWithValue("@Oemid", oemid);
                        cmdd.Parameters.AddWithValue("@Hsrp_stateid", HSRPStateID);
                        cmdd.Parameters.AddWithValue("@OrderType", ddlOrderType.SelectedValue.ToString());
                        cmdd.Parameters.AddWithValue("@VehicleClass", ddlVehicleclass.SelectedItem.Text.ToString());
                        cmdd.Parameters.AddWithValue("@VehicleType", lblVehicleType.Text.ToString());


                        SqlDataAdapter da = new SqlDataAdapter(cmdd);
                        da.Fill(dtrates);
                        if (dtrates.Rows.Count == 0)
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "error amount, ref:('" + oemid + "'/'" + ddlVehicleclass.SelectedItem.Text.ToString() + "'/'" + ddlVehicletype.SelectedItem.Text.ToString() + "')";
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = ex.Message.ToString();
                    return;
                }

                decimal newRates = Convert.ToDecimal(dtrates.Rows[0]["roundoff_netamount"].ToString()) + fitmentCharges;
                #endregion

                //int availableamount = Convert.ToInt32(availableBlanace());
                //if (availableamount < Convert.ToInt32(Math.Round(decimal.Parse(dtrates.Rows[0]["roundoff_netamount"].ToString()), 0) + fitmentCharges))
                //{ 
                //    lblSucMess.Visible = false;
                //    lblErrMess.Visible = true;
                //    lblErrMess.Text = "Available Balance is low, Please Contact to Administrator.";
                //    return;
                //}

                string StickerMandatory = string.Empty;
                sqlstring = "select VehicleType from VehicleTypeStickerMaster where VehicleType = '" + ddlVehicletype.SelectedItem.Text.Trim() + "'";
                dt = Utils.GetDataTable(sqlstring, ConnectionString);
                if (dt.Rows.Count > 0)
                {
                    StickerMandatory = "Y";
                }
                else
                {
                    StickerMandatory = "N";
                }

                if (!string.IsNullOrEmpty(lblauthno.Text.ToString()))
                {
                    hsrprecord_authorizationno = lblauthno.Text.ToString();
                }
                else
                {
                    hsrprecord_authorizationno = "0";
                }

                #region Database Insertion

                string rcFileName = HiddenRCPath.Value;
                string firFileName = HiddenFIR.Value;
                string flFileName = HiddenFlaser.Value;
                string rlFileName = HiddenRearLaser.Value;
                DataTable dt1 = new DataTable();
                string query1 = string.Empty;
                SqlCommand cmd = new SqlCommand();
               
                cmd = new SqlCommand("HRCashcollection_DealerManualPrepaidAllOemDamageForApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@hsrprecord_authorizationno", hsrprecord_authorizationno);
                cmd.Parameters.AddWithValue("@hsrprecord_authorizationdate", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@SaveMacAddress", macbase);
                cmd.Parameters.AddWithValue("@DeliveryChallanNo", DC);
                cmd.Parameters.AddWithValue("@ISFrontPlateSize", 'N');
                cmd.Parameters.AddWithValue("@ISRearPlateSize", 0);
                cmd.Parameters.AddWithValue("@HSRPRecord_CreationDate", DateTime.Now.ToString());

                if (ddlAffixationType.SelectedValue == "1")
                {
                    Query = "select State_Id from dealeraffixation where SubDealerId = '" + ddlLocationAddress.SelectedValue + "'";
                    dt = Utils.GetDataTable(Query, ConnectionString);

                    query1 = "select top 1 rtolocationid, NAVEMBID from rtolocation where RTOLocationID in (select RTOLocationID from DealerAffixation where SubDealerId = " + ddlLocationAddress.SelectedValue + ")";
                    dt1 = Utils.GetDataTable(query1, ConnectionString);

                    cmd.Parameters.AddWithValue("@HSRP_StateID", dt.Rows[0]["State_Id"].ToString());
                    cmd.Parameters.AddWithValue("@RTOLocationID", dt1.Rows[0]["rtolocationid"].ToString());
                }
                if (ddlAffixationType.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@HSRP_StateID", ddlstate.SelectedValue);
                    cmd.Parameters.AddWithValue("@RTOLocationID", ddldistrict.SelectedValue);
                }

                cmd.Parameters.AddWithValue("@VehicleClass", ddlVehicleclass.SelectedItem.ToString());                   
                cmd.Parameters.AddWithValue("@OrderType", ddlOrderType.SelectedValue);
                cmd.Parameters.AddWithValue("@NetAmount", newRates);
                cmd.Parameters.AddWithValue("@VehicleType", lblVehicleType.Text.ToString());
                cmd.Parameters.AddWithValue("@OrderStatus", "New Order");
                cmd.Parameters.AddWithValue("@CashReceiptNo", cashrc);
                cmd.Parameters.AddWithValue("@ChassisNo", txtChassisno.Text.Trim());
                cmd.Parameters.AddWithValue("@EngineNo", txtEngineNo.Text.Trim());
                if(newStateid == "4")
                {
                    cmd.Parameters.AddWithValue("@frontplatesize", DBNull.Value);
                    cmd.Parameters.AddWithValue("@rearplatesize", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@frontplatesize", dtrates.Rows[0]["FrontPlateSize"].ToString());
                    cmd.Parameters.AddWithValue("@rearplatesize", dtrates.Rows[0]["RearPlateSize"].ToString());
                }
               
                cmd.Parameters.AddWithValue("@CreatedBy", USERID);
                cmd.Parameters.AddWithValue("@vehicleref", "New");
                cmd.Parameters.AddWithValue("@FrontplatePrize", 0);
                cmd.Parameters.AddWithValue("@RearPlatePrize", 0);
                cmd.Parameters.AddWithValue("@StickerPrize", 0);
                cmd.Parameters.AddWithValue("@ScrewPrize", 0);
                //cmd.Parameters.AddWithValue("@TotalAmount", (dtrates.Rows[0]["roundoff_netamount"].ToString()+fitmentCharges));
                cmd.Parameters.AddWithValue("@TotalAmount", newRates);
                cmd.Parameters.AddWithValue("@VAT_Amount", 0);
                //cmd.Parameters.AddWithValue("@RoundOff_NetAmount", Math.Round(decimal.Round(decimal.Parse(dtrates.Rows[0]["roundoff_netamount"].ToString()), 2, MidpointRounding.AwayFromZero))+ Math.Round(decimal.Parse(fitmentCharges.ToString()),2, MidpointRounding.AwayFromZero));
                cmd.Parameters.AddWithValue("@RoundOff_NetAmount", newRates);
                cmd.Parameters.AddWithValue("@VAT_Percentage", 0);
                cmd.Parameters.AddWithValue("@PlateAffixationDate", "");
                cmd.Parameters.AddWithValue("@DateOfInsurance", Convert.ToDateTime(strFrmDateString.ToString()));
                cmd.Parameters.AddWithValue("@ReceiptNo", "");
                cmd.Parameters.AddWithValue("@vehicleregno", txtRegNumber.Text.Replace(" ", "").Trim());
                cmd.Parameters.AddWithValue("@dealerid", dealerid);
                cmd.Parameters.AddWithValue("@addrecordby", "Dealer");
                cmd.Parameters.AddWithValue("@fixingcharge", fixcharge);
                cmd.Parameters.AddWithValue("@StickerMandatory", StickerMandatory);
                cmd.Parameters.AddWithValue("@userrtolocationid", Session["UserRTOLocationID"].ToString());
                cmd.Parameters.AddWithValue("@SGSTPer", dtrates.Rows[0]["sgstper"].ToString());
                cmd.Parameters.AddWithValue("@SGSTAmount", dtrates.Rows[0]["sgstamt"].ToString());
                cmd.Parameters.AddWithValue("@CGSTPer", dtrates.Rows[0]["cgstper"].ToString());
                cmd.Parameters.AddWithValue("@CGSTAmount", dtrates.Rows[0]["cgstamt"].ToString());
                cmd.Parameters.AddWithValue("@gstbasicamount", dtrates.Rows[0]["GstBasic_Amt"].ToString());
                cmd.Parameters.AddWithValue("@GSTRoundoff_value", "0.00");
                cmd.Parameters.AddWithValue("@typeofapplication", ddlFuelType.SelectedItem.Text.ToString());
                cmd.Parameters.AddWithValue("@ManufactureDate", Convert.ToDateTime(strToDateString.ToString()));
                cmd.Parameters.AddWithValue("@ManufacturerModel", txtmodel.Text.ToString());
                cmd.Parameters.AddWithValue("@VehicleTypeNew", ddlVehicletype.SelectedItem.ToString());
                if ((ddlVehicleStateType.SelectedItem.Text.Contains("VI")) || (ddlVehicleStateType.SelectedValue == "BS6"))
                {
                    cmd.Parameters.AddWithValue("@VehicleStagingType", "BS6");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@VehicleStagingType", "BS4");
                }

                if (ddlAffixationType.SelectedValue == "1")
                {
                    string navembidquery = "select top 1 rtolocationid, NAVEMBID from rtolocation where RTOLocationID in (select RTOLocationID from DealerAffixation where SubDealerId = " + ddlLocationAddress.SelectedValue + ")";
                    dt1 = Utils.GetDataTable(navembidquery, ConnectionString);
                    cmd.Parameters.AddWithValue("@Affix_Id", ddlLocationAddress.SelectedItem.Value.ToString());
                    cmd.Parameters.AddWithValue("@Address1", ddlLocationAddress.SelectedItem.Text.ToString());
                    cmd.Parameters.AddWithValue("@navembid", dt1.Rows[0]["NAVEMBID"].ToString());
                }

                if (ddlAffixationType.SelectedValue == "2")
                {
                    cmd.Parameters.AddWithValue("@Affix_Id", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address1", txtHomeAddress.Text.ToString());
                    string navembidquery = "select NAVEMBID from rtolocation where rtolocationid = '" + ddldistrict.SelectedValue + "'";
                    dt1 = Utils.GetDataTable(navembidquery, ConnectionString);
                    cmd.Parameters.AddWithValue("@navembid", dt1.Rows[0]["NAVEMBID"].ToString());
                }

                cmd.Parameters.AddWithValue("@address2", url);
                cmd.Parameters.AddWithValue("@Pinno", txtpincode.Text.ToString());
                cmd.Parameters.AddWithValue("@TypeOfDelivery", ddlAffixationType.SelectedValue);
                cmd.Parameters.AddWithValue("@oemid", oemid);

                cmd.Parameters.AddWithValue("@RCFileName", rcFileName);
                cmd.Parameters.AddWithValue("@FIRFileName", firFileName);
                cmd.Parameters.AddWithValue("@FLFileName", flFileName);
                cmd.Parameters.AddWithValue("@RLFileName", rlFileName);
                

                //cmd.Parameters.Add("@NewHsrpR", SqlDbType.Char, 1);
                cmd.Parameters.Add("@Message", SqlDbType.Char, 1);
                cmd.Parameters["@Message"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                                             
                string message = cmd.Parameters["@Message"].Value.ToString();
                con.Close();

                if (message == "Y")
                {
                    lblSucMess.Visible = true;
                    lblErrMess.Visible = false;
                    lblSucMess.Text = "Order Saved Successfully, Rate Charged : " + newRates;
                    //lblavailbal.Text = availableBlanace();                                                          
                    //GenerateInvoice(txtRegNumber.Text.ToString(), newStateid);
                    Cleardatasave();
                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Something went wrong, Please contact administrator.";
                    return;
                }

                #endregion

            }
            catch (Exception ex)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Something went wrong, please contact administrator.";
                return;
            }
        }

        protected void ddlVehicletype_SelectedIndexChanged(object sender, EventArgs e)
        {
            int liStartIndex = ddlVehicletype.SelectedItem.Value.IndexOf(":") + 1;
            int liLength = ddlVehicletype.SelectedItem.Value.Length - liStartIndex;
            if (liStartIndex > 1)
            {
                lsOriginalValue = ddlVehicletype.SelectedItem.Value.Substring(liStartIndex, liLength);
                lblVehicleType.Text = lsOriginalValue;
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

        public static string SMLISUZU_API_2(string Chassisno, string Engineno)
        {
            string Status = "";
            string SOAP_Prefix = "http://125.19.77.151/SMLWebServices/RssplService.asmx";
            string apiResponse = string.Empty;
            string serverUserName = "rsspl";
            string serverPassword = "kjhuiu567@";
            string html = string.Empty;
            //string responseJson = "";


            string decryptedString = string.Empty;


            SMLService.RssplService mService = new SMLService.RssplService();
            mService.Url = SOAP_Prefix;
            mService.UseDefaultCredentials = false;
            mService.Credentials = new System.Net.NetworkCredential(serverUserName, serverPassword);

            try
            {

                var str = mService.VinEngineNoValidation(serverUserName, serverPassword, Chassisno, Engineno);

                Status = str.InnerText;

            }
            catch (Exception ev)
            {
                apiResponse = ev.Message;
                Console.WriteLine(ev.Message);
            }

            return Status;
        }

        protected void btnGO2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRegNumber.Text.Trim() == "")
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter vehicle registration no!";
                    return;
                }

                string specialChar = @"'€";
                string specialCharCE = @"'€";
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
                            lblErrMess.Text = "Special characters not allowed in vehicle registration no!";
                            return;
                        }
                    }
                }
                if (txtChassisno.Text.Length < 5)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter valid chassis no!";
                    return;
                }
                if (string.IsNullOrEmpty(txtChassisno.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter valid chassis no!";
                    return;
                }
                if (txtEngineNo.Text.Length < 5)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter valid engine no!";
                    return;
                }
                if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please enter valid engine no!";
                    return;
                }
                foreach (var item in specialCharCE)
                {
                    if (txtEngineNo.Text.Trim().Contains(item))
                    {
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Special characters not allowed in engine number!";
                        return;
                    }
                }

                url = HttpContext.Current.Request.Url.AbsoluteUri;
                string VehicleRego = txtRegNumber.Text.ToString().Trim();
                string Chassisno = txtChassisno.Text.Trim().ToString();
                string strChassisno = Chassisno.Substring(Chassisno.Length - 5, 5);
                string Engineno = txtEngineNo.Text.ToString().Trim();
                string strEngineno = Engineno.Substring(Engineno.Length - 5, 5);

                string responseJson = rosmerta_API_2(VehicleRego.ToUpper().Trim(), strChassisno.ToUpper().Trim(), strEngineno.ToUpper().Trim(), "5UwoklBqiW");

                HttpContext.Current.Session["SessionFilePath"] = "";
                VehicleDetails _vd = JsonConvert.DeserializeObject<VehicleDetails>(responseJson);

                string maker1 = string.Empty;

                if (_vd != null)
                {
                    if ((_vd.regnDate != "") && (_vd.regnDate != "01/01/0001 12:00:00 AM") && (_vd.regnDate != null))
                    {
                        string orderType = GetOrderType(_vd.regnDate);
                        if (orderType == "OB")
                        {
                            string VehicleRegoE = txtRegNumber.Text.ToString().Substring(0, 2).Trim();
                            if (VehicleRegoE == "AP")
                            {
                                string strquery = string.Empty;
                                strquery = "insert into HSRPrecordsAPLog values('" + dealerid + "','" + oemid + "','" + txtRegNumber.Text + "','" + txtChassisno.Text + "','" + txtEngineNo.Text + "','" + _vd.message + "','" + _vd.fuel + "','" + _vd.maker + "','" + _vd.vchType + "','" + _vd.norms + "','" + _vd.vchCatg + "','" + _vd.regnDate + "','" + USERID + "', getdate(),'" + url + "')";
                                Utils.ExecNonQuery(strquery, ConnectionString);
                                Response.Redirect("https://aprtacitizen.epragathi.org/#!/Hsrpbookingslot");
                            }
                        }

                        OrderDate.SelectedDate = Convert.ToDateTime(_vd.regnDate);

                        ddlVehicletype.SelectedItem.Text = _vd.vchCatg;
                        ddlVehicleclass.SelectedValue = _vd.vchType;

                        Query = "select oemid from OEMVahanMapping where OemName = '" + _vd.maker + "' and Oemid != 1005";
                        dt = Utils.GetDataTable(Query, ConnectionString);
                        if (dt.Rows.Count > 0)
                        {
                            oemid = dt.Rows[0]["oemid"].ToString();
                        }
                        else
                        {
                            lblErrMess.Visible = true;
                            lblErrMess.Text = "Oem not mapped, please contact administrator.";
                            return;
                        }

                        BindVehicleDropdownlistwithSP(oemid);
                        //ddlVehicleStateType.SelectedItem.Text = _vd.norms;
                        if ((_vd.norms == "Not Available") || (_vd.norms == ""))
                        {
                            ddlVehicleStateType.Enabled = true;
                        }
                        else
                        {
                            ddlVehicleStateType.SelectedItem.Text = _vd.norms;
                            ddlVehicleStateType.Enabled = false;
                        }

                        if (_vd.fuel == "NOT APPLICABLE")
                        {
                            ddlFuelType.SelectedItem.Text = _vd.fuel;
                            ddlFuelType.Enabled = true;
                        }
                        else
                        {
                            ddlFuelType.SelectedItem.Text = _vd.fuel;
                            ddlFuelType.Enabled = false;
                        }

                        divvehclass.Visible = true;
                        divvehclassddl.Visible = true;
                        divregdate.Visible = true;
                        divregdate2.Visible = true;
                        divfuel.Visible = true;
                        divfuel2.Visible = true;
                        divvehstage.Visible = true;
                        divvehstage2.Visible = true;
                        divvehtype.Visible = true;
                        divvehtype2.Visible = true;
                        divmodel.Visible = true;
                        divmodel2.Visible = true;
                        divdevtype.Visible = true;
                        btnGO2.Visible = false;
                        btnSave2.Visible = true;
                        btnPrint.Visible = false;
                        hr1.Visible = true;
                        hr2.Visible = true;
                        divfitment.Visible = true;
                        divdocument.Visible = true;
                        divdocument2.Visible = true;
                        divdocument3.Visible = true;
                        DisableControl();
                    }
                    else
                    {
                        divvehclass.Visible = false;
                        divvehclassddl.Visible = false;
                        divregdate.Visible = false;
                        divregdate2.Visible = false;
                        divfuel.Visible = false;
                        divfuel2.Visible = false;
                        divvehstage.Visible = false;
                        divvehstage2.Visible = false;
                        divvehtype.Visible = false;
                        divvehtype2.Visible = false;
                        divmodel.Visible = false;
                        divmodel2.Visible = false;
                        lblErrMess.Visible = true;
                        lblErrMess.Text = _vd.message;
                        divdevtype.Visible = false;
                        btnGO2.Visible = true;
                        btnPrint.Visible = false;
                        lblErrMess.Text = _vd.message.ToString();
                        InitialSetting();
                        ddlVehicleclass.ClearSelection();
                        ddlFuelType.SelectedItem.Text = "-Select Fuel Type-";
                        ddlVehicleStateType.SelectedItem.Text = "-Select Stage Type-";
                        txtmodel.Text = "";
                        ddlVehicletype.ClearSelection();
                        hr1.Visible = false;
                        hr2.Visible = false;
                        divfitment.Visible = false;
                        divdocument.Visible = false;
                        divdocument2.Visible = false;
                        divdocument3.Visible = false;
                        return;
                    }

                }
                else
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Vehicle Not Found";
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Something went wrong while validating with vahan!";
                return;
            }
        }

        private void DisableControl()
        {
            //ddlFuelType.Enabled = false;
            ddlVehicleclass.Enabled = false;
            //ddlOrderType.Enabled = false;
            txtmodel.Text = "";
        }

        protected void txtRegNumber_TextChanged(object sender, EventArgs e)
        {

            string VehicleRegoE = "";
            string strIsvahan = @"select isVahan from Dealermaster where dealerid = '" + dealerid + "'";

            DataTable dtchkIsvahan = Utils.GetDataTable(strIsvahan, ConnectionString);

            string SkipVehicleType = @"select VEHICLETYPE from OEMMaster where OEMID = '" + oemid + "'";

            DataTable dtchkVehicleType = Utils.GetDataTable(SkipVehicleType, ConnectionString);
            if (txtRegNumber.Text.Trim() != "")
            {
                string strVehicleNo = txtRegNumber.Text.Trim();
                if (strVehicleNo.Length < 4 || strVehicleNo.Length > 10)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "Please Enter Valid Vehicle Registration No.";
                    return;
                }
            }

            if (txtRegNumber.Text != "")
            {
                VehicleRegoE = txtRegNumber.Text.ToString().Substring(0, 2).Trim().ToUpper();
            }

            if ((txtRegNumber.Text != "") && (VehicleRegoE == "TS") || (dtchkVehicleType.Rows[0]["VEHICLETYPE"].ToString().Trim() == "Trailer & Trolley Supplement") || (dtchkIsvahan.Rows[0]["IsVahan"].ToString() == "N"))
            {
                btnGO2.Visible = false;
                ddlVehicleclass.Items.Clear();
                ddlVehicleclass.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Vehicle Class-", "-Select Vehicle Class-"));
                ddlVehicleclass.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Transport", "Transport"));
                ddlVehicleclass.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Non-Transport", "Non-Transport"));
                ddlVehicleclass.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Rent A CAB", "Rent A CAB"));


                ddlFuelType.Items.Clear();
                ddlFuelType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Fuel type-", "-Select Fuel type-"));
                ddlFuelType.Items.Insert(1, new System.Web.UI.WebControls.ListItem("PETROL", "PETROL"));
                ddlFuelType.Items.Insert(2, new System.Web.UI.WebControls.ListItem("CNG/PETROL", "CNG/PETROL"));
                ddlFuelType.Items.Insert(3, new System.Web.UI.WebControls.ListItem("DIESEL", "DIESEL"));
                ddlFuelType.Items.Insert(4, new System.Web.UI.WebControls.ListItem("DIESEL/HYBRID", "DIESEL/HYBRID"));

                ddlFuelType.Items.Insert(5, new System.Web.UI.WebControls.ListItem("PETROL/HYBRID", "PETROL/HYBRID"));
                ddlFuelType.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Electricity", "Electricity"));
                ddlFuelType.Items.Insert(7, new System.Web.UI.WebControls.ListItem("LPG/PETROL", "LPG/PETROL"));
                ddlFuelType.Items.Insert(8, new System.Web.UI.WebControls.ListItem("CNG", "CNG"));


                ddlVehicleStateType.Items.Clear();
                ddlVehicleStateType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-Select Vehicle Stage Type-", "-Select Vehicle Stage Type-"));
                ddlVehicleStateType.Items.Insert(1, new System.Web.UI.WebControls.ListItem("BS4 or Other", "BS4"));
                ddlVehicleStateType.Items.Insert(2, new System.Web.UI.WebControls.ListItem("BS6", "BS6"));
                ddlVehicleStateType.Items.Insert(3, new System.Web.UI.WebControls.ListItem("BS3", "BS3"));
                ddlVehicleStateType.Enabled = true;
                ddlVehicleclass.Enabled = true;
                ddlFuelType.Enabled = true;
                OrderDate.Enabled = true;
                calendar_from_button.Visible = true;
            }
            else
            {
                btnGO2.Visible = true;
                ddlVehicleStateType.Enabled = false;
                ddlVehicleclass.Enabled = true;
                ddlFuelType.Enabled = false;
                calendar_from_button.Visible = false;
                OrderDate.Enabled = false;
            }

        }

        protected void btnAdd2_Click(object sender, EventArgs e)
        {
            Response.Redirect("../Transaction/AffixAddressUpdate.aspx");
        }

        protected string GetOrderType(string date)
        {
            string validDatetime = "2019-04-01";
            return (Convert.ToDateTime(date) >= Convert.ToDateTime(validDatetime)) ? "NB" : "OB";
        }

        private void GenerateInvoice(string VehicleNo, string stateid)
        {
            try
            {
                string checkReceiptQuery = "";
                DataTable dtReceipt = new DataTable();
                if (stateid == "4")
                {
                    checkReceiptQuery = "PaymentPlateReceiptMultiBrand_HR '" + VehicleNo + "' ";
                    dtReceipt = Utils.GetDataTable(checkReceiptQuery, ConnectionString);
                }
                else
                {
                    checkReceiptQuery = "PaymentPlateReceiptMultiBrand '" + VehicleNo + "' ";
                    dtReceipt = Utils.GetDataTable(checkReceiptQuery, ConnectionString);
                }

                if (dtReceipt.Rows.Count > 0)
                {
                    string Status = dtReceipt.Rows[0]["status"].ToString();
                    if (Status == "1")
                    {
                        #region
                        string HSRPRecordID = dtReceipt.Rows[0]["HSRPRecordID"].ToString();
                        string OrderDate = dtReceipt.Rows[0]["OrderDate"].ToString();
                        string fuelType = dtReceipt.Rows[0]["TypeOfApplication"].ToString();
                        string VehicleClass = dtReceipt.Rows[0]["VehicleClass"].ToString();
                        string VehicleType = dtReceipt.Rows[0]["VehicleType"].ToString();
                        string AppointmentType = dtReceipt.Rows[0]["AppointmentType"].ToString();
                        decimal GstBasic_Amt = Convert.ToDecimal(dtReceipt.Rows[0]["BasicAamount"]) + Convert.ToDecimal(dtReceipt.Rows[0]["FitmentCharge"]);

                        string BasicAamount = dtReceipt.Rows[0]["BasicAamount"].ToString();
                        string FitmentCharge = dtReceipt.Rows[0]["FitmentCharge"].ToString();
                        string ConvenienceFee = dtReceipt.Rows[0]["ConvenienceFee"].ToString();
                        string HomeDeliveryCharge = dtReceipt.Rows[0]["HomeDeliveryCharge"].ToString();
                        string MRDCharges = dtReceipt.Rows[0]["MRDCharges"].ToString();
                        string TotalAmount = dtReceipt.Rows[0]["TotalAmount"].ToString();
                        string CGSTPer = dtReceipt.Rows[0]["CGSTPer"].ToString();
                        string SGSTPer = dtReceipt.Rows[0]["SGSTPer"].ToString();
                        decimal CGSTAmount = Convert.ToDecimal(dtReceipt.Rows[0]["CGSTAmount"]);
                        decimal SGSTAmount = Convert.ToDecimal(dtReceipt.Rows[0]["SGSTAmount"]);
                        string NetAmount = dtReceipt.Rows[0]["NetAmount"].ToString();
                        string OrderStatus = dtReceipt.Rows[0]["OrderStatus"].ToString();

                        string MonthYears = DateTime.Now.ToString("MMM-yyyy");
                        string folderpath = ConfigurationManager.AppSettings["ReceiptPath"].ToString() + "/Plate/" + MonthYears + "/";

                        if (!Directory.Exists(folderpath))
                        {
                            Directory.CreateDirectory(folderpath);
                        }

                        string filename = VehicleNo + DateTime.Now.ToString("ddMMyyyyHHmmssfff") + ".pdf";
                        string PdfFolder = folderpath + filename;
                        string filepathtosave = "Plate/" + MonthYears + "/" + filename; //for saving into database

                        string ReceiptPathQRCode = "https://chart.googleapis.com/chart?chs=80x80&cht=qr&chl=" + ConfigurationManager.AppSettings["ReceiptDirectory"].ToString() + filepathtosave + "&chld=L|1&choe=UTF-8";

                        StringBuilder sbTable = new StringBuilder();
                        sbTable.Clear();

                        sbTable.Append("<table>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td valign='bottom' width='50%' ><b><h4>Receipt of Payment & Appointment</h4></b></td>");
                        sbTable.Append("<td rowspan=2><img src='" + ReceiptPathQRCode + "' id='img_qrcode'></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr><td><p>HSRP Online Appointment Transaction Reciept <br> Rosmerta Safety Systems Ltd.</p></td></tr>");
                        sbTable.Append("</table>");

                        sbTable.Append("<table>");
                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Order Date :</td>");
                        sbTable.Append("<td>" + OrderDate + "</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Vehicle Reg No :</td>");
                        sbTable.Append("<td><b>" + VehicleNo + "</b></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Vehicle Fuel :</td>");
                        sbTable.Append("<td>" + fuelType + "</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Vehicle Class :</td>");
                        sbTable.Append("<td>" + VehicleClass + "</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Vehicle Type :</td>");
                        sbTable.Append("<td>" + VehicleType + "</td>");
                        sbTable.Append("</tr>");


                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Order Status :</td>");
                        sbTable.Append("<td><b>" + OrderStatus + "</b></td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>HSRP set :</td>");
                        sbTable.Append("<td>" + string.Format("{0:0.00}", BasicAamount) + " INR</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Fitment Charges :</td>");
                        sbTable.Append("<td>" + FitmentCharge + " INR</td>");
                        sbTable.Append("</tr>");

                        if (AppointmentType.ToString().Contains("Home"))
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>Home Delivery :</td>");
                            sbTable.Append("<td>" + HomeDeliveryCharge + " INR</td>");
                            sbTable.Append("</tr>");
                        }
                        else
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>Convenience Fee :</td>");
                            sbTable.Append("<td>" + ConvenienceFee + " INR</td>");
                            sbTable.Append("</tr>");
                        }


                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Gross Total :</td>");
                        sbTable.Append("<td>" + GstBasic_Amt + " INR</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>CGST Rate(%) :</td>");
                        sbTable.Append("<td>" + CGSTPer + " </td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>SGST Rate(%) :</td>");
                        sbTable.Append("<td>" + SGSTPer + " </td>");
                        sbTable.Append("</tr>");

                        if (CGSTAmount != 0)
                        {
                            sbTable.Append("<tr>");
                            sbTable.Append("<td>CGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", CGSTAmount) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        if (SGSTAmount != 0)
                        {

                            sbTable.Append("<tr>");
                            sbTable.Append("<td>SGST Amount :</td>");
                            sbTable.Append("<td>" + string.Format("{0:0.00}", SGSTAmount) + " INR</td>");
                            sbTable.Append("</tr>");
                        }

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>MRDCharges :</td>");
                        sbTable.Append("<td>" + MRDCharges + " INR</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>Grand Total :</td>");
                        sbTable.Append("<td>" + NetAmount + " INR</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("</table>");
                        //sbTable.Append("<br><br><p align='left'><b>Note:</b><br><font color='red'>1. Carry this receipt, SMS and RC copy at the time of appointment.</font></br>");

                        sbTable.Append("<table>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>1. Carry this receipt and RC copy at the time of appointment.</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>2. For queries, Please contact Toll Free 9205180518, Email ID : appsupport@rosmertasafety.com </td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("<tr>");
                        sbTable.Append("<td>3. Calling time (9:30 AM to 6:00 PM) and day (Monday to Saturday) or Email at appsupport@rosmertasafety.com</td>");
                        sbTable.Append("</tr>");

                        sbTable.Append("</table>");

                        //createpdfreceipt(sbTable.ToString(), BookingHistoryID);
                        try
                        {
                            Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                            #region

                            PdfWriter.GetInstance(pdfDoc, new FileStream(PdfFolder, FileMode.Create));
                            pdfDoc.Open();
                            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(sbTable.ToString()), null);
                            foreach (var htmlElement in parsedHtmlElements)
                                pdfDoc.Add(htmlElement as IElement);
                            pdfDoc.Close();

                            try
                            {
                                string updateReceiptQuery = "update multibrandhsrprecords set ReceiptPath = '" + filepathtosave + "' where HSRPRecordID = '" + HSRPRecordID + "' ";
                                Utils.ExecNonQuery(updateReceiptQuery, ConnectionString);
                            }
                            catch (Exception ev)
                            {

                            }
                            string ReceiptPathForQRCode = ConfigurationManager.AppSettings["ReceiptDirectory"].ToString() + filepathtosave;

                            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "BarCodeDisplay('" + ReceiptPathForQRCode + "')", true);
                            ViewState["HSRPRecordID"] = HSRPRecordID;
                            //PrintReceipt(HSRPRecordID);

                            #endregion
                        }
                        catch (Exception ev)
                        {
                            lblErrMess.Text = ev.Message;
                            lblErrMess.Visible = true;
                            return;
                        }

                        #endregion

                    }
                    else
                    {
                        lblErrMess.Text = "Error:: " + dtReceipt.Rows[0]["message"].ToString();
                        lblErrMess.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblErrMess.Text = "Error: Invalid VehicleRegno.";
                    lblErrMess.Visible = true;
                    return;
                }

            }
            catch (Exception ex)
            {
                lblErrMess.Text = "Receipt : Your Session has been expired, Please fill the details again.";
                lblErrMess.Visible = true;
                return;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string checkReceiptQuery = "select isnull(ReceiptPath,'') FileName, HSRPRecordID from multibrandhsrprecords where HSRPRecordID = '" + ViewState["HSRPRecordID"] + "'";
                DataTable dtFileName = Utils.GetDataTable(checkReceiptQuery, ConnectionString);
                if (dtFileName.Rows.Count > 0)
                {
                    #region
                    string FileName = dtFileName.Rows[0]["FileName"].ToString();
                    string folderpath = ConfigurationManager.AppSettings["ReceiptPathDownalod"].ToString();
                    string PdfFolder = folderpath + "/" + FileName;
                    if (FileName.Length > 0 && File.Exists(PdfFolder))
                    {
                        string[] arr = FileName.ToString().Split('/');
                        int i = arr.Length;

                        //string PdfSignedFolder = signFolderPath + arr[i - 1].ToString();
                        string filename = arr[i - 1].ToString();
                        #region
                        try
                        {
                            HttpContext context = HttpContext.Current;
                            context.Response.ContentType = "Application/pdf";
                            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
                            context.Response.WriteFile(PdfFolder);
                            context.Response.Flush(); // Sends all currently buffered output to the client.
                            context.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                            context.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                        }
                        catch (Exception ev)
                        {
                            lblErrMess.Text = ev.Message;
                            lblErrMess.Visible = true;
                        }
                        #endregion
                    }
                    else
                    {
                        lblErrMess.Text = "Receipt not Generated.";
                        lblErrMess.Visible = true;
                        return;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message;
                lblErrMess.Visible = true;
                return;
            }

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