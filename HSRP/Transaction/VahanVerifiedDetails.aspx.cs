using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP.Transaction
{
    public partial class VahanVerifiedDetails : System.Web.UI.Page
    {
        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {            
                                   
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                string specialCharCE = @"'€";
                if (txtRegNumber.Text == "")
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Please Enter Valid Vehicle Registration No.";
                    return;
                }
                if (txtRegNumber.Text.Trim() != "")
                {
                    string strVehicleNo = txtRegNumber.Text.Trim();
                    if (strVehicleNo.Length < 4 || strVehicleNo.Length > 10)
                    {
                        llbMSGError.Visible = true;
                        llbMSGError.Text = "Please enter valid vehicle registration no.";
                        return;
                    }
                    foreach (var item in specialCharCE)
                    {
                        if (strVehicleNo.Contains(item))
                        {
                            llbMSGError.Visible = true;
                            llbMSGError.Text = "Special characters not allowed in vehicleregno!";
                            return;
                        }
                    }
                }

                if (txtChassisno.Text.Length < 5)
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Please enter valid chassis no.";
                    return;
                }
                if (string.IsNullOrEmpty(txtChassisno.Text.Trim()))
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Please enter valid chassis no.";
                    return;
                }
                if (txtEngineNo.Text.Length < 5)
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Please enter valid engine no.";
                    return;
                }
                if (string.IsNullOrEmpty(txtEngineNo.Text.Trim()))
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Please enter valid engine no.";
                    return;
                }
                foreach (var item in specialCharCE)
                {
                    if (txtEngineNo.Text.Trim().Contains(item))
                    {
                        llbMSGError.Visible = true;
                        llbMSGError.Text = "Special characters not allowed in engine number!";
                        return;
                    }
                }

                string VehicleRego = txtRegNumber.Text.ToString().Trim();
                string Chassisno = txtChassisno.Text.Trim().ToString();
                string strChassisno = Chassisno.Substring(Chassisno.Length - 5, 5);
                string Engineno = txtEngineNo.Text.ToString().Trim();
                string strEngineno = Engineno.Substring(Engineno.Length - 5, 5);

                string responseJson = rosmerta_API_2(VehicleRego.ToUpper().Trim(), strChassisno.ToUpper().Trim(), strEngineno.ToUpper().Trim(), "5UwoklBqiW");

                VehicleDetails _vd = JsonConvert.DeserializeObject<VehicleDetails>(responseJson);
                if (_vd != null)
                {
                    if(_vd.message == "Vehicle Not Found")
                    {
                        llbMSGError.Visible = true;
                        llbMSGError.Text = "Details Not Found!";
                        return;
                    }
                    else
                    {
                        divmsg.Visible = true;
                        lblmsg.Text = _vd.message;
                        lblmaker.Text = _vd.maker;
                        lblFuel.Text = _vd.fuel;
                        lblVehType.Text = _vd.vchCatg;
                        lblStageType.Text = _vd.norms;
                        lblVehClass.Text = _vd.vchType;
                        CultureInfo culture = new CultureInfo("en-US");
                        DateTime regDate = Convert.ToDateTime(_vd.regnDate, culture);
                        lblregDate.Text = regDate.ToString("dd/MM/yyyy");
                        lblstateCode.Text = _vd.stateCd;
                        lblfCode.Text = _vd.hsrpFrontLaserCode;
                        lblrCode.Text = _vd.hsrpRearLaserCode;
                        llbMSGError.Visible = false;
                    }
                   
                }
                else
                {
                    llbMSGError.Visible = true;
                    llbMSGError.Text = "Details Not Found!";
                    return;
                }
            }
            catch (Exception ex)
            {
                llbMSGError.Visible = true;
                llbMSGError.Text = "Something went wrong!";
                return;
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            txtRegNumber.Text = "";
            txtChassisno.Text = "";
            txtEngineNo.Text = "";
            lblmsg.Text = "";
            lblmsg.Visible = false;
            llbMSGError.Visible = false;
            divmsg.Visible = false;           
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
            public string stateCd { get; set; }
            public string hsrpFrontLaserCode { get; set; }
            public string hsrpRearLaserCode { get; set; }

        }
    }
}