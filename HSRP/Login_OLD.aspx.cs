using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Text;
using System.Web;
using HSRP;
using DataProvider;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;
using System.Text;
using System.Net;
//using HSRP.org.mptransport.oltp;
using System.Text.RegularExpressions;
namespace HSRP
{
    public partial class Login_OLD : System.Web.UI.Page
    {
        string CnnString = String.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {


           
            //Response.Redirect("http://43.242.124.23/HSRPALL/login.aspx?X=" + Request.QueryString["X"].ToString(),true);
            //Response.Redirect("http://203.122.58.217/vts/login.aspx", true);
            //Utils.GZipEncodePage();

            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

            //HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            //HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            //HttpContext.Current.Response.AddHeader("Expires", "0");
            
            
            //ModalPopupExtender1.Show();


            if (!IsPostBack)
            {


                lblotpdiv.Visible = false;
                txtotpdiv.Visible = false;
                FillCapctha();
                txtUserID.Focus();
                Session["LoginAttempts"] = string.Empty;

            }
            lblMsgBlue.Text = String.Empty;
            lblMsgRed.Text = String.Empty;
        }

        DataSet kk;

        DataTable dt = new DataTable();

        public void FillCapctha()
        {
            try
            {
                Random random = new Random();
                string combination = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZ";
                StringBuilder captcha = new StringBuilder();
                for (int i = 0; i < 5; i++)
                    captcha.Append(combination[random.Next(combination.Length)]);

                Session["captcha"] = captcha.ToString();
                imgCaptcha.ImageUrl = "GenerateCaptcha.aspx?" + DateTime.Now.Ticks.ToString();
            }
            catch
            {
                throw;
            }
        }

        protected void btnLogin_Click(object sender, System.EventArgs e)
        {
            try
            {
                string onlinebanking = string.Empty;
                string strDefaultPage = string.Empty;
                string strUserName = string.Empty;
                string strPassword = string.Empty;
                string SQLString = string.Empty;
                string ActiveStatus = string.Empty;
                string userid = string.Empty;
                string HSRPStateID = string.Empty;
                string MobileNo = string.Empty;
                string UserType = string.Empty;
                string UserName = string.Empty;
                string RTOLocationID = string.Empty;
                string RTOLocationName = string.Empty;
                string dbPassword = string.Empty;
                string dbUserName = string.Empty;
                string FirstLoginStatus = string.Empty;
                string macbaseflag = string.Empty;
                strUserName = txtUserID.Text.ToString();
                string macb = string.Empty;
                string dealerid = string.Empty;
                string oemid = string.Empty;
                string accessType = string.Empty;
                string EmailID = string.Empty;
                string SQLStringCodeId = string.Empty;
                string OemCodeId = System.Configuration.ConfigurationManager.AppSettings["OemCodeId"].ToString();


                strPassword = txtUserPassword.Text.ToString();

                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    lblMsgRed.Text = "Please provide login details.";
                    txtUserID.Focus();
                    return;
                }

                if (txtCaptcha.Text.ToString().Trim() == "")
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "swal('Please Enter Captcha!','','error');", true);
                    txtCaptcha.Focus();
                    return;
                }
                else if (string.Equals(Session["captcha"], txtCaptcha.Text.ToString()) == false)
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "swal('Captcha validation is required!','','error');", true);
                    txtCaptcha.Focus();
                    return;
                }

                SQLString = "Select * From Users where UserLoginName='" + strUserName + "'  and Password='" + strPassword + "' ";

                dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    if (string.IsNullOrEmpty(Session["LoginAttempts"].ToString()))
                    {
                        Session["LoginAttempts"] = 1;
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not match.";
                        txtUserPassword.Focus();
                    }
                    if (Session["LoginAttempts"].ToString().Equals("5"))
                    {
                        SQLString = "Update Users Set ActiveStatus='N' where UserLoginName='" + strUserName + "'";
                        Utils.ExecNonQuery(SQLString, CnnString);
                        lblMsgRed.Text = "Your Account is blocked : Contact Administrator.";
                        return;
                    }
                    else
                    {
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not match.";
                        Session["LoginAttempts"] = Convert.ToInt32(Session["LoginAttempts"].ToString()) + 1;
                        txtUserPassword.Focus();
                    }
                    txtUserPassword.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Your credential did not match.";
                    txtUserPassword.Focus();
                    return;
                }

                if (dt.Rows[0]["withoutmac"].ToString().ToUpper() == "Y")
                {
                    macb = "CAF8DA35332B";
                }
                else
                {
                    macb = Request.QueryString["X"].ToString();
                }



                SQLString = "Select * From Users  where  UserLoginName='" + strUserName + "'";
               
              
                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 600;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();


                if (PReader.HasRows)
                {
                    while (PReader.Read())
                    {
                        ActiveStatus = PReader["ActiveStatus"].ToString();
                        userid = PReader["UserID"].ToString();
                        HSRPStateID = PReader["HSRP_StateID"].ToString();
                        RTOLocationID = PReader["RTOLocationID"].ToString();
                        dealerid = PReader["dealerid"].ToString();
                        UserType = PReader["UserType"].ToString();
                        dbUserName = PReader["UserFirstName"].ToString() + " " + PReader["UserLastName"].ToString();
                        dbPassword = PReader["Password"].ToString();
                        FirstLoginStatus = PReader["FirstLoginStatus"].ToString();
                        strDefaultPage = PReader["DefaultPage"].ToString();
                        macbaseflag = PReader["withoutMAC"].ToString();
                        Session["macbaseflag"] = macbaseflag;
                        MobileNo = PReader["Mobileno"].ToString();
                        EmailID = PReader["EmailID"].ToString();
                        oemid = PReader["oemid"].ToString();
                         onlinebanking = PReader["Banking_Type"].ToString();
                        //if (oemid != "4")
                        //{
                        //    lblMsgRed.Text = "You are not authorized to login.";
                        //    return;
                        //}
                        try
                        {
                            accessType = PReader["AccessType"].ToString();
                        }
                        catch (Exception ev)
                        {
                            //lblMsgRed.Text = "You are not authorized to login.";
                            //return;
                        }

                        if (UserType != "0")
                        {
                            if (string.IsNullOrEmpty(PReader["dealerid"].ToString()) && string.IsNullOrEmpty(PReader["oemid"].ToString()))
                            {
                                lblMsgRed.Text = "You are not authorized to login.";
                                return;
                            }
                            //if (PReader["dealerid"].ToString() == "0")
                            //{
                            //    lblMsgRed.Text = "You are not authorized to login.";
                            //    return;
                            //}
                        }


                        string RTOlocationStateID = string.Empty;
                        SQLString = "select  E.State_Id as  HSRPStateID   from RTOlocation R Join EmbossingCentersNew E  On R.NAVEMBCode=E.Emb_Center_id where RTOLocationID='" + RTOLocationID + "'";
                        DataTable dts = Utils.GetDataTable(SQLString, CnnString);

                        if (dts.Rows.Count > 0)
                        {
                            RTOlocationStateID = dts.Rows[0]["HSRPStateID"].ToString();
                        }

                        SQLString = "select HSRP_StateID as Prefixcount from Prefixotherinvoice  where HSRP_StateId='" + RTOlocationStateID + "' and oemid='" + oemid + "'";
                        DataTable dtPrefix = Utils.GetDataTable(SQLString, CnnString);

                        if (dtPrefix.Rows.Count == 0)
                        {
                            txtUserPassword.Text = string.Empty;
                            lblMsgRed.Visible = true;
                            lblMsgRed.Text = "Prefix is not maintained ! kindly contact Account Dept.";
                            txtUserPassword.Focus();
                            //return;

                        }

                        SQLStringCodeId = "select OEMCodeId from OemMaster    where  oemid='" + oemid + "'";
                        DataTable dtOemCodeid = Utils.GetDataTable(SQLStringCodeId, CnnString);
                        string fetchOemCodeiD = dtOemCodeid.Rows[0]["OEMCodeId"].ToString();
                        if (OemCodeId != fetchOemCodeiD)
                        {
                            txtUserPassword.Text = string.Empty;
                            lblMsgRed.Visible = true;
                            lblMsgRed.Text = "Url is Invalid";
                            txtUserPassword.Focus();
                            return;

                        }

                    }
                }
                else
                {
                    txtUserPassword.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Your credential did not match.";
                    txtUserPassword.Focus();
                    return;
                }

                PReader.Close();
                dbLink.CloseConnection();

                //Modified by Ashok Dated - 08/10/2021
                //Start

                if (strPassword.Equals(dbPassword))
                {
                    if (!string.IsNullOrEmpty(MobileNo) && FirstLoginStatus == "N")
                    {
                        string LastOTPVerified = fnLastOTPVerified(userid);
                        if (LastOTPVerified  != "1")
                        {
                            Regex pattern = new Regex(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)");
                            //Regex pattern = new Regex(@"^((\+){0,1}91(\s){0,1}(\-){0,1}(\s){0,1}){0,1}9[0-9](\s){0,1}(\-){0,1}(\s){0,1}[1-9]{1}[0-9]{7}$");
                            if (!pattern.IsMatch(MobileNo.Trim()))
                            {
                                lblMsgRed.Visible = true;
                                lblMsgRed.Text = "Mobile No is not valid.";
                                return;
                            }
                            string OTPStatus = GenerateOTP(MobileNo, userid);
                            lblMsgBlue.Visible = false;
                            if (OTPStatus == "1")
                            {
                                Session["UID"] = userid;
                                lblMsgBlue.Text = "OTP Sent on registered mobileno Please Enter OTP to proceed.";
                                //ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('OTP Sent Please Enter OTP to proceed');", true);
                                lnkBtnresendotp.Visible = true;
                                lblMsgBlue.Visible = true;
                                txtOTP.Visible = true;
                                btnOTP.Visible = true;
                                btnLogin.Visible = false;
                                //txtUserID.Visible = false;
                                //txtUserPassword.Visible = false;
                                lblotpdiv.Visible = true;
                                txtotpdiv.Visible = true;
                                txtuseriddiv.Visible = false;
                                txtpwddiv.Visible = false;
                                txtcaptchadiv.Visible = false;
                                lblcaptchaddiv.Visible = false;
                                return;
                            }
                            else
                            {
                                lblMsgBlue.Text = "Please Contact to Administrator";
                                //ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('OTP Sent Please Enter OTP to proceed');", true);
                                lblMsgRed.Visible = true;
                                return;
                            }
                        }
                    }

                    //END
                    if (ActiveStatus.Equals("N"))
                    {
                        lblMsgRed.Text = "Please Contact System Administrator.";
                        return;
                    }

                    else
                    {
                        SQLString = "select RTOLocationName from rtolocation where hsrp_stateid='" + HSRPStateID + "' and rtolocationid='" + RTOLocationID + "'";
                        DataTable dtrtoname = Utils.GetDataTable(SQLString, CnnString);
                        RTOLocationName = dtrtoname.Rows[0]["RTOLocationName"].ToString();

                        Session["UID"] = userid;
                        Session["dealerid"] = dealerid;
                        Session["UserHSRPStateID"] = HSRPStateID;
                        Session["UserRTOLocationID"] = RTOLocationID;
                        Session["UserType"] = UserType;
                        Session["UserName"] = dbUserName;
                        Session["RTOLocationName"] = RTOLocationName;
                        Session["MobileNo"] = MobileNo;
                        Session["EmailID"] = EmailID;
                        Session["oemid"] = oemid;
                        Session["accessType"] = accessType;
                        Session["onlinebanking"] = onlinebanking;
                        if (macbaseflag.Trim() == "N" || macbaseflag.Trim() == "Y")
                        {
                            String MacAddress = String.Empty;
                            if (macbaseflag == "Y")
                            {
                                MacAddress = "CAF8DA35332B";
                            }
                            else
                            {
                                MacAddress = Request.QueryString["X"].ToString();
                            }
                            SQLString = "Select ActiveStatus,SaveMacAddress From MACBase where MacAddress ='" + MacAddress + "'";
                            kk = Utils.getDataSet(SQLString, CnnString);

                            //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='0', LastLoggedInDatetime ='1900-01-01 00:00:00.000' where  loggedinUserID is null and LastLoggedInDatetime is null", CnnString);

                            SQLString = "select count(*) as Records from MACBASE where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30";
                            int getcounts = Utils.getScalarCount(SQLString, CnnString);
                            if (getcounts > 0)
                            {
                                //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30", CnnString);
                            }

                            Session["SaveMacAddress"] = kk.Tables[0].Rows[0]["SaveMacAddress"].ToString();
                            if (macbaseflag == "Y")
                            {
                                Session["MacAddress"] = "CAF8DA35332B";
                            }
                            else
                            {
                                Session["MacAddress"] = Request.QueryString["X"].ToString();
                            }
                            if (string.IsNullOrEmpty(MobileNo) || FirstLoginStatus == "Y")
                            {
                                Response.Redirect("~/FIrstLoginChangePassword.aspx", true);
                            }

                            string BrowserName = HttpContext.Current.Request.Browser.Type;
                            string ClientOSName = HttpContext.Current.Request.Browser.Platform;

                            Utils.ExecNonQuery("UPDATE users set lastLoginDatetime=GetDate() where UserLoginName='" + strUserName + "'", CnnString);

                            String Sq = "select  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) as record, MacAddress from MACBASE where LoggedInUserID='" + userid + "'";

                            DataTable checkLogin = Utils.GetDataTable(Sq, CnnString);
                            if (checkLogin.Rows.Count > 0)
                            {
                                int csd = Convert.ToInt16(checkLogin.Rows[0]["record"].ToString());
                                String MacDataUser = checkLogin.Rows[0]["MacAddress"].ToString();
                                if (csd < 10)
                                {
                                    if (MacDataUser == MacAddress)
                                    {
                                        //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                        //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                        Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                        //Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                        if (!string.IsNullOrEmpty(dealerid) && dealerid != "0" && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                        {
                                            Response.Redirect(strDefaultPage, true);
                                        }
                                        else if ((string.IsNullOrEmpty(dealerid) || dealerid == "0") && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                        {
                                            Response.Redirect("~/LiveReports/oemdashboard.aspx", true);
                                        }
                                        lblMsgRed.Text = string.Empty;
                                    }
                                    else
                                    {
                                        CreateDuplicateLogin(userid);
                                        lblMsgRed.Text = "You are Already Loggged In";
                                        lblMsgRed.Visible = true;
                                    }
                                }
                                else
                                {
                                    //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                    //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                    Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                    //Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                    if (!string.IsNullOrEmpty(dealerid) && dealerid != "0" && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                    {
                                        Response.Redirect(strDefaultPage, true);
                                    }
                                    else if ((string.IsNullOrEmpty(dealerid) || dealerid == "0") && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                    {
                                        Response.Redirect("~/liveReports/oemdashboard.aspx", true);
                                    }
                                    lblMsgRed.Text = string.Empty;
                                }
                            }
                            else
                            {
                                //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                //Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                if (!string.IsNullOrEmpty(dealerid) && dealerid != "0" && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                {
                                    Response.Redirect(strDefaultPage, true);
                                }
                                else if ((string.IsNullOrEmpty(dealerid) || dealerid == "0") && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                {
                                    Response.Redirect("~/liveReports/oemdashboard.aspx", true);
                                }
                                lblMsgRed.Text = string.Empty;
                            }
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(Session["LoginAttempts"].ToString()))
                    {
                        Session["LoginAttempts"] = 1;
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not match.";
                        txtUserPassword.Focus();
                    }
                    if (Session["LoginAttempts"].ToString().Equals("5"))
                    {
                        SQLString = "Update Users Set ActiveStatus='N' where UserLoginName='" + strUserName + "'";
                        Utils.ExecNonQuery(SQLString, CnnString);

                        lblMsgRed.Text = "Your Account is blocked : Contact Admin.";

                    }
                    else
                    {
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not match.";
                        Session["LoginAttempts"] = Convert.ToInt32(Session["LoginAttempts"].ToString()) + 1;
                        txtUserPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsgRed.Text = "Contact Administrator : " + ex.Message.ToString();
            }
        }

        protected string fnLastOTPVerified(string userid)
        {
            string msg = "0";
            string SQLString = "LastOtpVerification '" + userid + "'";
            dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0].ToString() == "1")
                {
                    msg = "1";
                }
            }
            return msg;
        }

        private string Achknowledge(string userid, string HSRPStateID)
        {
            string status = "";
            try
            {
                string SQLString = "AcknowledgeStatusUsers'" + userid + "','" + HSRPStateID + "'";
                DataTable dtrtoname = Utils.GetDataTable(SQLString, CnnString);
                if (dtrtoname.Rows.Count > 0)
                {
                    status = dtrtoname.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                status = "N";
            }
            return status;
        }

        string logmacbaseID;

        private void CreateDuplicateLogin(string userid)
        {
            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
            {
                logmacbaseID = "CAF8DA35332B";
            }
            else
            {
                logmacbaseID = Request.QueryString["X"].ToString();
            }

            string sqle = "Insert into DuplicateLoginLog (userID,UserName,Password,IP,MacbaseID) values ('" + userid + "','" + txtUserID.Text + "','" + txtUserPassword.Text + "','" + 11 + "','" + logmacbaseID + "')";
            Utils.ExecNonQuery(sqle, CnnString);
        }

        protected void LinkButtonForget_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPass.aspx", false);
        }
        public static String SMSSend(string mobile, string SMSText)
        {
            //txtAuthKey.Text="343817AaX3yb5BY4rI5f967427P1";
            //txtSenderId.Text ="BMHSRP;    // "dlhsrp";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string result;
            //Your authentication key
            string authKey = "343817AaX3yb5BY4rI5f967427P1";
            //Multiple mobiles numbers separated by comma
            string mobileNumber = mobile;
            //Sender ID,While using route4 sender id should be 6 characters long.
            string senderId = "DLHSRP";
            //Your message to send, Add URL encoding here.
            string message = HttpUtility.UrlEncode(SMSText);
            string country = "91";
            string DLT_TE_ID = "1007857264069489695";
            //Prepare you post parameters
            StringBuilder sbPostData = new StringBuilder();
            sbPostData.AppendFormat("authkey={0}", authKey);
            sbPostData.AppendFormat("&mobiles={0}", mobileNumber);
            sbPostData.AppendFormat("&message={0}", message);
            sbPostData.AppendFormat("&sender={0}", senderId);
            sbPostData.AppendFormat("&country={0}", country);
            sbPostData.AppendFormat("&DLT_TE_ID={0}", DLT_TE_ID);
            sbPostData.AppendFormat("&route={0}", "4");

            try
            {
                //Call Send SMS API
                string sendSMSUri = "https://api.msg91.com/api/sendhttp.php";
                //Create HTTPWebrequest
                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(sendSMSUri);
                //Prepare and Add URL Encoded data
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(sbPostData.ToString());
                //Specify post method
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/x-www-form-urlencoded";
                httpWReq.ContentLength = data.Length;
                using (Stream stream = httpWReq.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                //Get the response
                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string responseString = reader.ReadToEnd();

                //Close the response
                reader.Close();
                response.Close();
                result = responseString;
            }
            catch (SystemException ex)
            {
                result = ex.Message.ToString();
            }

            return result;
        }
        protected string GenerateOTP(string MobileNo, string UserId)
        {


            string msg = "";
            if (MobileNo.Trim().ToString().Length < 10 || MobileNo.Trim().ToString().Length > 10)
            {
                lblMsgRed.Visible = true;
                msg = "Please Enter valid Mobile No .";
            }

            if (MobileNo.ToString() == "")
            {
                lblMsgRed.Visible = true;
                msg = "Please Enter Mobile No.";
            }


            else
            {
                string CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                btnLogin.Enabled = true;
                //txtOTP.Visible = true;
                //txtotp.Visible = true;s
                string otp = Otp();
                Session["otp"] = otp;
                //string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "";
                //string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "";
                string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "(Team Rosmerta)";
                string result = SMSSend(MobileNo, SMSText);

                int status = Utils.ExecNonQuery("insert into OtpMobile(UserMobileNo,UserLoginName,OTPNo,FromPage,Result) values('" + MobileNo + "','" + UserId + "','" + otp.ToString() + "','Login','" + result + "')", CnnString1);
                //textBoxMobileNo.Enabled = false; 
                if (status == 1)
                {
                    msg = "1";
                }
            }
            return msg;
        }

        public string GetRandomNumber()
        {
            Random r = new Random();
            var x = r.Next(0, 99999999);
            return x.ToString("00000000");
        }

        public string Otp()
        {
            Random r = new Random();
            var x = r.Next(0, 9999);
            return x.ToString("0000");
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            FillCapctha();
        }

        protected void btnOTP_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    string strDefaultPage = string.Empty;
                    string strUserName = string.Empty;
                    string strPassword = string.Empty;
                    string SQLString = string.Empty;
                    string ActiveStatus = string.Empty;
                    string userid = string.Empty;
                    string HSRPStateID = string.Empty;
                    string MobileNo = string.Empty;
                    string UserType = string.Empty;
                    string UserName = string.Empty;
                    string RTOLocationID = string.Empty;
                    string RTOLocationName = string.Empty;
                    string dbPassword = string.Empty;
                    string dbUserName = string.Empty;
                    string FirstLoginStatus = string.Empty;
                    string macbaseflag = string.Empty;
                    strUserName = txtUserID.Text.ToString();
                    string macb = string.Empty;
                    string dealerid = string.Empty;
                    string oemid = string.Empty;

                    strPassword = txtUserPassword.Text.ToString();

                    if (string.IsNullOrEmpty(txtOTP.Text))
                    {
                        lblMsgRed.Text = "Please provide OTP.";
                        txtUserID.Focus();
                        return;
                    }

                    SQLString = "OTPVerification '" + Session["UID"].ToString() + "','" + txtOTP.Text + "' ";

                    dt = Utils.GetDataTable(SQLString, CnnString);
                    if (dt.Rows.Count == 0)
                    {
                        txtOTP.Text = string.Empty;
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "OTP did not match.";
                        txtOTP.Focus();
                        return;
                    }

                    if (dt.Rows[0]["withoutmac"].ToString().ToUpper() == "Y")
                    {
                        macb = "CAF8DA35332B";
                    }
                    else
                    {
                        macb = Request.QueryString["X"].ToString();
                    }



                    SQLString = "Select * From Users where UserId='" + Session["UID"].ToString() + "'";

                    Utils dbLink = new Utils();
                    dbLink.strProvider = CnnString;
                    dbLink.CommandTimeOut = 600;
                    dbLink.sqlText = SQLString.ToString();
                    SqlDataReader PReader = dbLink.GetReader();


                    if (PReader.HasRows)
                    {
                        while (PReader.Read())
                        {
                            ActiveStatus = PReader["ActiveStatus"].ToString();
                            userid = PReader["UserID"].ToString();
                            HSRPStateID = PReader["HSRP_StateID"].ToString();
                            RTOLocationID = PReader["RTOLocationID"].ToString();
                            dealerid = PReader["dealerid"].ToString();
                            UserType = PReader["UserType"].ToString();
                            dbUserName = PReader["UserFirstName"].ToString() + " " + PReader["UserLastName"].ToString();
                            dbPassword = PReader["Password"].ToString();
                            FirstLoginStatus = PReader["FirstLoginStatus"].ToString();
                            strDefaultPage = PReader["DefaultPage"].ToString();
                            macbaseflag = PReader["withoutMAC"].ToString();
                            Session["macbaseflag"] = macbaseflag;
                            MobileNo = PReader["ContactNo"].ToString();
                            oemid = PReader["oemid"].ToString();
                            //if (UserType != "0")
                            //{
                            //    if (string.IsNullOrEmpty(PReader["dealerid"].ToString()))
                            //    {
                            //        lblMsgRed.Text = "You are not authorized to login.";
                            //        return;
                            //    }
                            //    if (PReader["dealerid"].ToString() == "0")
                            //    {
                            //        lblMsgRed.Text = "You are not authorized to login.";
                            //        return;
                            //    }
                            //}
                        }
                    }
                    else
                    {

                        txtUserPassword.Text = string.Empty;
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not match.";
                        txtUserPassword.Focus();

                        return;
                    }

                    PReader.Close();
                    dbLink.CloseConnection();

                    //if (strPassword.Equals(dbPassword))
                    //{
                    if (ActiveStatus.Equals("N"))
                    {

                        lblMsgRed.Text = "Please Contact System Administrator.";
                        return;
                    }
                    else
                    {
                        SQLString = "select RTOLocationName from rtolocation where hsrp_stateid='" + HSRPStateID + "' and rtolocationid='" + RTOLocationID + "'";
                        DataTable dtrtoname = Utils.GetDataTable(SQLString, CnnString);
                        RTOLocationName = dtrtoname.Rows[0]["RTOLocationName"].ToString();

                        Session["UID"] = userid;
                        Session["dealerid"] = dealerid;
                        Session["UserHSRPStateID"] = HSRPStateID;
                        Session["UserRTOLocationID"] = RTOLocationID;
                        Session["UserType"] = UserType;
                        Session["UserName"] = dbUserName;
                        Session["RTOLocationName"] = RTOLocationName;
                        Session["MobileNo"] = MobileNo;
                        Session["oemid"] = oemid;
                        if (FirstLoginStatus == "Y")
                        {
                            Response.Redirect("~/FirstLoginChangePassword.aspx", true);
                        }

                        if (macbaseflag.Trim() == "N" || macbaseflag.Trim() == "Y")
                        {
                            String MacAddress = String.Empty;
                            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
                            {
                                MacAddress = "CAF8DA35332B";
                            }
                            else
                            {
                                MacAddress = Request.QueryString["X"].ToString();
                            }
                            Session["MacAddress"] = MacAddress;
                            SQLString = "Select ActiveStatus,SaveMacAddress From MACBase where MacAddress ='" + MacAddress + "'";
                            kk = Utils.getDataSet(SQLString, CnnString);

                            //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='0', LastLoggedInDatetime ='1900-01-01 00:00:00.000' where  loggedinUserID is null and LastLoggedInDatetime is null", CnnString);

                            SQLString = "select count(*) as Records from MACBASE where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30";
                            int getcounts = Utils.getScalarCount(SQLString, CnnString);
                            if (getcounts > 0)
                            {
                                //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) >30", CnnString);

                            }

                            Session["SaveMacAddress"] = kk.Tables[0].Rows[0]["SaveMacAddress"].ToString();
                            if (dt.Rows[0]["withoutmac"].ToString() == "Y")
                            {
                                Session["MacAddress"] = "CAF8DA35332B";
                            }
                            else
                            {
                                Session["MacAddress"] = Request.QueryString["X"].ToString();
                            }
                            if (FirstLoginStatus == "Y")
                            {
                                Response.Redirect("~/FIrstLoginChangePassword.aspx", true);
                            }
                            string BrowserName = HttpContext.Current.Request.Browser.Type;
                            string ClientOSName = HttpContext.Current.Request.Browser.Platform;


                            Utils.ExecNonQuery("UPDATE users set lastLoginDatetime=GetDate() where UserLoginName='" + strUserName + "'", CnnString);


                            String Sq = "select  DATEDIFF(mi, LastLoggedInDatetime, GETDATE()) as record, MacAddress from MACBASE where LoggedInUserID='" + userid + "'";

                            DataTable checkLogin = Utils.GetDataTable(Sq, CnnString);
                            if (checkLogin.Rows.Count > 0)
                            {
                                int csd = Convert.ToInt16(checkLogin.Rows[0]["record"].ToString());
                                String MacDataUser = checkLogin.Rows[0]["MacAddress"].ToString();
                                if (csd < 10)
                                {
                                    if (MacDataUser == MacAddress)
                                    {

                                        //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                        //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                        Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                        //Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                        if (!string.IsNullOrEmpty(dealerid) && dealerid != "0" && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                        {
                                            Response.Redirect(strDefaultPage, true);
                                        }
                                        else if ((string.IsNullOrEmpty(dealerid) || dealerid == "0") && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                        {
                                            Response.Redirect("~/liveReports/oemdashboard.aspx", true);
                                        }
                                        lblMsgRed.Text = string.Empty;
                                    }
                                    else
                                    {
                                        CreateDuplicateLogin(userid);
                                        lblMsgRed.Text = "You are Already Loggged In";
                                        lblMsgRed.Visible = true;
                                    }
                                }
                                else
                                {

                                    //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                    //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                    Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                    //Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                    if (!string.IsNullOrEmpty(dealerid) && dealerid != "0" && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                    {
                                        Response.Redirect(strDefaultPage, true);
                                    }
                                    else if ((string.IsNullOrEmpty(dealerid) || dealerid == "0") && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                    {
                                        Response.Redirect("~/liveReports/oemdashboard.aspx", true);
                                    }
                                    lblMsgRed.Text = string.Empty;
                                }
                            }
                            else
                            {

                                //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where  LoggedInUserID='" + userid + "'", CnnString);
                                //Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='" + userid + "', LastLoggedInDatetime =GetDate() where MacAddress='" + MacAddress + "'", CnnString);
                                Utils.user_log(userid, "Login Page", Request.UserHostAddress.ToString(), "Login", MacAddress, BrowserName, ClientOSName, CnnString);
                                //Utils.ExecNonQuery("UPDATE MACBase set HSRP_StateID='" + HSRPStateID + "',RTOLocationID='" + RTOLocationID + "' where MacAddress='" + Session["MacAddress"] + "'", CnnString);
                                if (!string.IsNullOrEmpty(dealerid) && dealerid != "0" && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                {
                                    Response.Redirect(strDefaultPage, true);
                                }
                                else if ((string.IsNullOrEmpty(dealerid) || dealerid == "0") && !string.IsNullOrEmpty(oemid) && oemid != "0")
                                {
                                    Response.Redirect("~/liveReports/oemdashboard.aspx", true);
                                }
                                lblMsgRed.Text = string.Empty;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsgRed.Text = "Contact Administrator : " + ex.Message.ToString();
                }
            }
        }

        protected void btnRefresh_Click1(object sender, ImageClickEventArgs e)
        {
            FillCapctha();
        }

        protected void lnkBtnresendotp_Click(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                string MobileNo = Utils.getScalarValue("select top 1 Mobileno from users where userid='" + Session["UID"].ToString() + "'", CnnString);
                string OTPStatus = GenerateOTP(MobileNo, Session["UID"].ToString());
                lblMsgBlue.Visible = false;
                if (OTPStatus == "1")
                {
                    //Session["UID"] = userid;
                    lblMsgBlue.Text = "OTP re-sent on your registered mobileno Please Enter OTP to proceed.";
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('OTP Sent Please Enter OTP to proceed');", true);
                    lnkBtnresendotp.Visible = true;
                    lblMsgBlue.Visible = true;
                    txtOTP.Visible = true;
                    btnOTP.Visible = true;
                    btnLogin.Visible = false;
                    //txtUserID.Visible = false;
                    //txtUserPassword.Visible = false;
                    lblotpdiv.Visible = true;
                    txtotpdiv.Visible = true;
                    txtuseriddiv.Visible = false;
                    txtpwddiv.Visible = false;
                    txtcaptchadiv.Visible = false;
                    lblcaptchaddiv.Visible = false;
                    return;
                }
                else
                {
                    lblMsgBlue.Text = "Please Contact to Administrator";
                    //ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('OTP Sent Please Enter OTP to proceed');", true);
                    lblMsgRed.Visible = true;
                    return;
                }
            }
        }
    }
}