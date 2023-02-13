using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace HSRP
{
    public partial class ChangePassword1 : System.Web.UI.Page
    {
        string UserID = string.Empty;
        string HSRPStateID = string.Empty;
        string dealerid = string.Empty;
        string oemid = string.Empty;
        string url = string.Empty;
        string CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Utils.GZipEncodePage();
                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                UserID = Session["UID"].ToString();
                HSRPStateID = Session["UserHSRPStateID"].ToString();
                dealerid = Session["dealerid"].ToString();
                oemid = Session["oemid"].ToString();
                if (!Page.IsPostBack)
                {
                    txtusername.Text = Session["UserName"].ToString();
                  //  lblotp.Visible = false;
                   // txtotp.Visible = false;
                    btnProceed.Visible = false;
                    lnkBtnresendotp.Visible = false;
                    fillDDl_Question();
                }
            }
            catch(Exception ex){
                lblErrMess.Text = ex.Message.ToString();
                lblErrMess.Visible = true;
            }
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            try
            {
                lblSuccMess.Visible = false;
                lblErrMess.Visible = false;
                string CnnString = string.Empty;
                String SQLString = string.Empty;
                string SQLStringChkPwd = string.Empty;
                string specialChar = @"'€";
                string straddress = txtAddress.Text.Trim();
                url = HttpContext.Current.Request.Url.AbsoluteUri;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                foreach (var item in specialChar)
                {
                    if (straddress.Contains(item))
                    {
                        
                        lblErrMess.Visible = true;
                        lblErrMess.Text = "Special charecters like (') not allowed in Address!";
                        return;
                    }
                }

                string gstin = txtGstNo.Text.ToString().Trim().ToUpper();
                if(gstin.Length != 15)
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "GSTNO. cannot be less than 15 Characters.";
                    return;
                }    

                if (!isPanNoValid(txtPan.Text.ToString().Trim()))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = "PAN No. is not valid!!!";
                    return;
                }


                //Regex regGst = new Regex(@"[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}");
                //if (!regGst.IsMatch(txtGstNo.Text.Trim()))
                //{
                //    lblErrMess.Visible = true;
                //    lblErrMess.Text = "Please enter valid GSTIN!!!";
                //    return;
                //}               
                

                SQLString = "select StateCode from HSRPState where HSRP_StateID = '" + HSRPStateID + "'";
                DataTable stcode = Utils.GetDataTable(SQLString, CnnString);
                string statecode = stcode.Rows[0]["StateCode"].ToString();
                Dictionary<string, string> gstinValidation = checkGstNumber(statecode, txtGstNo.Text.ToString().Trim(), txtPan.Text.ToString().Trim());
                if (!gstinValidation["message"].Equals("valid"))
                {
                    lblErrMess.Visible = true;
                    lblErrMess.Text = gstinValidation["message"];
                    return;
                }



                if ((!String.IsNullOrEmpty(ddlSecurityQuestion.Text)) && (!String.IsNullOrEmpty(txtSecurityAnswer.Text)) && (!String.IsNullOrEmpty(txtpassword.Text)) && (!String.IsNullOrEmpty(txtNewPassword.Text)) && (!String.IsNullOrEmpty(txtCpassword.Text)))
                {
                    if (string.IsNullOrEmpty(txtotp.Text.ToString()))
                    {
                        lblErrMess.Text = "Please enter OTP!";
                        lblErrMess.Visible = true;
                        lblSuccMess.Visible = false;
                        return;
                    }

                    if (otpverification() == "1")
                    {
                   
                        SQLStringChkPwd = "Select Password From Users where UserID='" + UserID + "'";
                        if (Utils.getDataSingleValue(SQLStringChkPwd, CnnString, "Password") == txtpassword.Text)
                        {                            
                            SQLString = "UpdateDealerFirstLogin '" + Convert.ToInt32(ddlSecurityQuestion.SelectedValue) + "','" + txtSecurityAnswer.Text + "','" + txtNewPassword.Text + "','" + txtMobilenumber.Text.ToString() + "','" + txtemail.Text.ToString() + "','" + txtAddress.Text + "','" + txtAddress.Text + "','" + txtOemgivendealercode.Text + "','" + UserID + "','" + dealerid + "','" + txtPan.Text + "'";
                            //SQLString = "Update Users Set  SecurityQuestionID='" + Convert.ToInt32(ddlSecurityQuestion.SelectedValue) + "' , SecurityQuestionAnswer ='" + txtSecurityAnswer.Text + "', Password ='" + txtNewPassword.Text + "', FirstLoginStatus ='N', MobileNo='" + txtMobilenumber.Text.ToString() + "', ActiveStatus='N', Emailid='" + txtemail.Text.ToString() + "'  where UserID='" + UserID + "'";
                            int j = Utils.ExecNonQuery(SQLString, CnnString);
                            string query = "insert into DealerMasterLog (DealerID, Address, Pincode, Mobileno, EmailID, oemid, UserID, CreatedDate, GSTIn, pageName) values('" + dealerid + "','" + txtAddress.Text.ToString() + "','" + txtPinCode.Text.ToString() + "'," + txtMobilenumber.Text.ToString() + ",'" + txtemail.Text.ToString() + "','" + oemid + "','" + UserID + "',getdate(), '" + txtGstNo.Text.ToString() + "','" + url + "') ";
                            Utils.ExecNonQuery(query, CnnString);
                            if (j > 0)
                            {
                                SQLString = "Update dealermaster Set  gstin=upper('" + txtGstNo.Text.ToString().Trim() + "') where dealerid='" + dealerid + "'";
                                if (Utils.ExecNonQuery(SQLString, CnnString) > 0)
                                {
                                    Response.Redirect("~/Login.aspx");
                                }
                            }
                            else
                            {
                                lblErrMess.Text = " Some DB Level Error is their in Execution. your New Password not saved </br> ";
                            }
                        }

                        else
                        {
                            lblErrMess.Text = "The Password provided by You is Incorrect.";
                            lblErrMess.Visible = true;
                            lblSuccMess.Visible = false;
                            BlankField();
                        }
                    }
                    else
                    {
                        lblErrMess.Text = "Otp is not valid.";
                        lblErrMess.Visible = true;
                    }
                }
                else
                {
                    lblErrMess.Text = "Please fill all the required fields.";
                    lblErrMess.Visible = true;
                    lblSuccMess.Visible = false;
                }
            }
            catch (Exception ee)
            {
                lblErrMess.Text = ee.Message.ToString();
                lblErrMess.Visible = true;
            }
        }

        private void BlankField()
        {
            txtpassword.Text = string.Empty;
        }

        public string otpverification()
        {
            try
            {
                string SQLString = "OTPVerification '" + Session["UID"].ToString() + "','" + txtotp.Text + "' ";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    lblErrMess.Text = "OTP did not match.";
                    lblErrMess.Visible = true;
                    lblSuccMess.Visible = false;
                    return "0";
                }
                else
                {
                    lblErrMess.Visible = false;
                    lblSuccMess.Visible = true;
                    return "1";
                }
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message.ToString();
                lblErrMess.Visible = true;
                return "0";
            }
        }

        private void fillDDl_Question()
        {
            try
            {
                string CnnString = string.Empty;
                CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                Utils.PopulateDropDownList(ddlSecurityQuestion, "select QuestionID,QuestionText from SecurityQuestion", CnnString, "--Select Question--");
            }
            catch (Exception ex)
            {
                lblErrMess.Text = ex.Message;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Login.aspx");
        }

        protected void buttonLogout_Click(object sender, EventArgs e)
        {
            if (Session["UID"] == null)
            {
                string login = "~/LogOut.aspx";
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);
            }
            else
            {
                string login = string.Empty;
                string wmac = Session["macbaseflag"].ToString();
                if (wmac == "N")
                {
                    login = "~/Login.aspx?X=" + Session["MacAddress"].ToString();
                }
                else
                {
                    Utils.ExecNonQuery("UPDATE MACBASE set LoggedInUserID='', LastLoggedInDatetime ='' where MacAddress='" + Session["MacAddress"].ToString() + "'", CnnString); 
                    login = "~/Login.aspx";
                }
                Session.Abandon();
                Session.Clear();
                Response.CacheControl = "no-cache";
                Response.Redirect(login);
            }
        }

        protected void btngetotp_Click(object sender, ImageClickEventArgs e)
        {
            string msg = string.Empty;
            lblErrMess.Visible = false;
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (txtemail.Text == string.Empty)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Email Id cannot be Blank";
                return;
            }
            if (!regex.IsMatch(txtemail.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Email-id is not valid.";
                return;
            }

            string MobileNo = txtMobilenumber.Text.ToString();
            if (txtMobilenumber.Text == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Mobile No, cannot be blank.";
                return;
            }
            if (!isMobileNoValid(txtMobilenumber.Text.ToString()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Mobile No, is not valid.";
                return;
            }

            else
            {
                string CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();

                txtotp.Visible = true;
                Random r = new Random();
                var otp = r.Next(0, 9999);
                Session["otp"] = otp;
                //string SMSText = "Welcome to UTSAV. Your OTP is : '" + otp.ToString() + "'";

                //  string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "";

                string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "(Team Rosmerta)";

                string result = SMSSend(MobileNo, SMSText);

                int status = Utils.ExecNonQuery("insert into OtpMobile(UserMobileNo,UserLoginName,OTPNo,FromPage,Result) values('" + MobileNo + "','" + UserID + "','" + otp.ToString() + "','FirstLogin','" + result + "')", CnnString1);
                txtMobilenumber.Enabled = false;
                if (status == 1)
                {
                    btnProceed.Visible = true;
                    btngetotp.Visible = false;
                    txtMobilenumber.ReadOnly = true;
                    txtotp.Visible = true;
                    lblotp.Visible = true;
                    lblSuccMess.Text = "Otp sent! Please enter otp to proceed!";
                    lblsecurityanswer.Visible = true;
                    lblsecurityque.Visible = true;
                    ddlSecurityQuestion.Visible = true;
                    txtSecurityAnswer.Visible = true;
                    lblgstno.Visible = true;
                    txtGstNo.Visible = true;
                    lblPan.Visible = true;
                    txtPan.Visible = true;
                    lbloldpwd.Visible = true;
                    txtpassword.Visible = true;
                    lblnewpwd.Visible = true;
                    txtNewPassword.Visible = true;
                    lblconfpwd.Visible = true;
                    txtCpassword.Visible = true;
                    lblAddress.Visible = true;
                    txtAddress.Visible = true;
                    lblPincode.Visible = true;
                    txtPinCode.Visible = true;
                    lblOemgivendealercode.Visible = true;
                    txtOemgivendealercode.Visible = true;
                    msg = "1";
                }
            }
            
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
        protected void lnkBtnresendotp_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            lblErrMess.Visible = false;
            Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);
            if (txtemail.Text == string.Empty)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Email Id cannot be Blank";
                return;
            }
            if (!regex.IsMatch(txtemail.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Email-id is not valid.";
                return;
            }

            Regex pattern = new Regex(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)");

            string MobileNo = txtMobilenumber.Text;

            if (MobileNo.Trim().ToString().Length < 10 || MobileNo.Trim().ToString().Length > 10)
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please Enter valid Mobile No .";
                return;
            }

            if (MobileNo.ToString() == "")
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Please Enter Mobile No.";
                return;
            }
            if (!pattern.IsMatch(txtMobilenumber.Text.Trim()))
            {
                lblErrMess.Visible = true;
                lblErrMess.Text = "Mobile No is not valid.";
                return;
            }
            else
            {
                string CnnString1 = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                //txtOTP.Visible = true;
                txtotp.Visible = true;
                Random r = new Random();
                var otp = r.Next(0, 9999);
                Session["otp"] = otp;
                //string SMSText = "Welcome to UTSAV. Your OTP is : '" + otp.ToString() + "'";

                //  string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "";

                string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "(Team Rosmerta)";

                string result = SMSSend(MobileNo, SMSText);

                int status = Utils.ExecNonQuery("insert into OtpMobile(UserMobileNo,UserLoginName,OTPNo,FromPage,Result) values('" + MobileNo + "','" + UserID + "','" + otp.ToString() + "','FirstLogin','" + result + "')", CnnString1);
                //textBoxMobileNo.Enabled = false; 
                if (status == 1)
                {
                    btnProceed.Visible = true;
                    btngetotp.Visible = false;
                    txtMobilenumber.ReadOnly = true;
                    txtotp.Visible = true;
                    lblotp.Visible = true;
                    lblSuccMess.Text = "Otp sent! Please enter otp to proceed!";
                    lblsecurityanswer.Visible = true;
                    lblsecurityque.Visible = true;
                    ddlSecurityQuestion.Visible = true;
                    txtSecurityAnswer.Visible = true;
                    //lblgstno.Visible = true;
                    //txtGstNo.Visible = true;
                    lbloldpwd.Visible = true;
                    txtpassword.Visible = true;
                    lblnewpwd.Visible = true;
                    txtNewPassword.Visible = true;
                    lblconfpwd.Visible = true;
                    txtCpassword.Visible = true;
                    lblAddress.Visible = true;
                    txtAddress.Visible = true;
                    lblPincode.Visible = true;
                    txtPinCode.Visible = true;
                    lblOemgivendealercode.Visible = true;
                    txtOemgivendealercode.Visible = true;
                    msg = "1";
                }
            }
        }

        protected Dictionary<string,string> checkGstNumber(string stateCode, string gstNum, string panNum)

        {
            Dictionary<string, string> errorMap = new Dictionary<string,string>();
            var GstateCode = gstNum.Substring(0, 2);
            var GpanNum = gstNum.Substring(2, 10);
            var GEnd = gstNum.Substring(13, 1);

            if (!string.Equals(stateCode, GstateCode, StringComparison.OrdinalIgnoreCase))
            {
                errorMap.Add("message", "User state is different from GSTIN state!");
                return errorMap;
               
            }
            if (!string.Equals(panNum, GpanNum, StringComparison.OrdinalIgnoreCase))
            {
                errorMap.Add("message", "PAN No not matching with GSTIN!");
                return errorMap;
            }
            if (!string.Equals(GEnd, "Z", StringComparison.OrdinalIgnoreCase))
            {
                errorMap.Add("message", "Invalid GSTIN!");
                return errorMap;
            }
            if (gstNum.Length != 15)
            {
                errorMap.Add("message", "Invalid GSTIN, GSTIN should be of max 15 character!");
                return errorMap;
            }

            errorMap.Add("message", "valid");
            return errorMap;
        }

        protected Boolean isPanNoValid(string panNo)
        {
            Regex panreg = new Regex(@"^[A-Za-z]{5}[0-9]{4}[A-Za-z]{1}");
            if (!panreg.IsMatch(panNo))
            {
                return false;
            }
            return true;
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

    }
}