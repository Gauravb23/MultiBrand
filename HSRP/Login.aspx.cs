using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HSRP
{
    public partial class Login : System.Web.UI.Page
    {
        string CnnString = String.Empty;
        DataTable dt = new DataTable();
        DataSet kk;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Session["LoginAttempts"] = string.Empty;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
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
                strUserName = txtUsername.Text.ToString();
                string macb = string.Empty;
                string dealerid = string.Empty;
                string oemid = string.Empty;
                string accessType = string.Empty;
                string EmailID = string.Empty;
                string SQLStringCodeId = string.Empty;
                string OemCodeId = System.Configuration.ConfigurationManager.AppSettings["OemCodeId"].ToString();


                strPassword = txtpassword.Text.ToString();

                if (string.IsNullOrEmpty(strUserName) || string.IsNullOrEmpty(strPassword))
                {
                    lblMsgRed.Text = "Please provide login details.";
                    txtUsername.Focus();
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
                        txtpassword.Focus();
                    }
                    if (Session["LoginAttempts"].ToString().Equals("3"))
                    {
                        SQLString = "Update Users Set ActiveStatus='N' where UserLoginName='" + strUserName + "'";
                        Utils.ExecNonQuery(SQLString, CnnString);
                        lblMsgRed.Text = "Your Account is blocked : Contact Administrator and mail at (test@rosmertatech.com).";
                        return;
                    }
                    else
                    {
                        lblMsgRed.Visible = true;
                        lblMsgRed.Text = "Your credential did not match.";
                        Session["LoginAttempts"] = Convert.ToInt32(Session["LoginAttempts"].ToString()) + 1;
                        txtpassword.Focus();
                    }
                    txtpassword.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Your credential did not match.";
                    txtpassword.Focus();
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
                      
                        SQLStringCodeId = "select OEMCodeId from OemMaster    where  oemid='" + oemid + "'";
                        DataTable dtOemCodeid = Utils.GetDataTable(SQLStringCodeId, CnnString);
                        string fetchOemCodeiD = dtOemCodeid.Rows[0]["OEMCodeId"].ToString();
                        if (OemCodeId != fetchOemCodeiD)
                        {
                            txtpassword.Text = string.Empty;
                            lblMsgRed.Visible = true;
                            lblMsgRed.Text = "Url is Invalid";
                            txtpassword.Focus();
                            return;

                        }

                    }
                }
                else
                {
                    txtpassword.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Your credential did not match.";
                    txtpassword.Focus();
                    return;
                }

                PReader.Close();
                dbLink.CloseConnection();

                //Modified by Ashok Dated - 08/10/2021
                //Start

                if (strPassword.Equals(dbPassword))
                {
                    //if (!string.IsNullOrEmpty(MobileNo) && FirstLoginStatus == "N")
                    //{
                    //    string LastOTPVerified = fnLastOTPVerified(userid);
                    //    if (LastOTPVerified != "1")
                    //    {
                    //        Regex pattern = new Regex(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)");
                    //        //Regex pattern = new Regex(@"^((\+){0,1}91(\s){0,1}(\-){0,1}(\s){0,1}){0,1}9[0-9](\s){0,1}(\-){0,1}(\s){0,1}[1-9]{1}[0-9]{7}$");
                    //        if (!pattern.IsMatch(MobileNo.Trim()))
                    //        {
                    //            lblMsgRed.Visible = true;
                    //            lblMsgRed.Text = "Mobile No is not valid.";
                    //            return;
                    //        }
                    //        string OTPStatus = GenerateOTP(MobileNo, userid);
                    //        lblMsgBlue.Visible = false;
                    //        if (OTPStatus == "1")
                    //        {
                    //            Session["UID"] = userid;
                    //            lblMsgBlue.Text = "OTP Sent on registered mobileno Please Enter OTP to proceed.";
                    //            //ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('OTP Sent Please Enter OTP to proceed');", true);
                    //            lnkBtnresendotp.Visible = true;
                    //            lblMsgBlue.Visible = true;
                    //            txtOTP.Visible = true;
                    //            btnOTP.Visible = true;
                    //            btnLogin.Visible = false;
                    //            //txtUserID.Visible = false;
                    //            //txtpassword.Visible = false;
                    //            lblotpdiv.Visible = true;
                    //            txtotpdiv.Visible = true;
                    //            txtuseriddiv.Visible = false;
                    //            txtpwddiv.Visible = false;
                    //            txtcaptchadiv.Visible = false;
                    //            lblcaptchaddiv.Visible = false;
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            lblMsgBlue.Text = "Please Contact to Administrator";
                    //            //ScriptManager.RegisterStartupScript(Page, GetType(), "MyScript", "alert('OTP Sent Please Enter OTP to proceed');", true);
                    //            lblMsgRed.Visible = true;
                    //            return;
                    //        }
                    //    }
                    //}

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
                                            //Response.Redirect("~/LiveReports/oemdashboard.aspx", true);
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
                                        //Response.Redirect("~/liveReports/oemdashboard.aspx", true);
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
                                    //Response.Redirect("~/liveReports/oemdashboard.aspx", true);
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
                        txtpassword.Focus();
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
                        txtpassword.Focus();
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

        string logmacbaseID = "";
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

            string sqle = "Insert into DuplicateLoginLog (userID,UserName,Password,IP,MacbaseID) values ('" + userid + "','" + txtUsername.Text + "','" + txtpassword.Text + "','" + 11 + "','" + logmacbaseID + "')";
            Utils.ExecNonQuery(sqle, CnnString);
        }

    }
}