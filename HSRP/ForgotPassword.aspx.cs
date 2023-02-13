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
using HSRP;
using DataProvider;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Configuration;
using System.IO;
using System.Net;
//using HSRP.org.mptransport.oltp;
using System.Text.RegularExpressions;
namespace HSRP
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        string CnnString = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            if (!Page.IsPostBack)
            {
                pwdrow.Visible = false;
                btnresetrow.Visible = false;
            }
        }

        protected void btnGetotp_Click(object sender, EventArgs e)
        {
            string userid = string.Empty;
            string MobileNo = string.Empty;
            if (string.IsNullOrEmpty(txtusername.Text.ToString().Trim()))
            {
                lblMsgRed.Text = "Please provide login details.";
                txtusername.Focus();
                return;
            }
            string SQLString = "Select top 1 * From Users where UserLoginName='" + txtusername.Text.ToString().Trim() + "'";
            DataTable dt = Utils.GetDataTable(SQLString, CnnString);
            if (dt.Rows.Count > 0)
            {
                userid = dt.Rows[0]["UserID"].ToString();
                MobileNo = dt.Rows[0]["Mobileno"].ToString();
                if (!string.IsNullOrEmpty(MobileNo))
                {
                    lbluserid.Text = userid;
                    Regex pattern = new Regex(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)");
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
                        lblMsgBlue.Text = "OTP Sent on registered mobileno Please Enter OTP to proceed.";
                        lblMsgBlue.Visible = true;
                        otprow.Visible = false;
                        btnresetrow.Visible = true;
                        pwdrow.Visible = true;
                        return;
                    }
                }
                else
                {
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "Mobile number is not registered.";
                    return;
                }
            }
            else
            {
                lblMsgRed.Visible = true;
                lblMsgRed.Text = "Username does not exist!.";
                txtusername.Focus();
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            lblMsgBlue.Text = "";
            lblMsgBlue.Visible = false;
            try
            {
                if (string.IsNullOrEmpty(txtotp.Text.ToString().Trim()))
                {
                    lblMsgRed.Text = "Please provide otp.";
                    txtusername.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtNewPassword.Text.ToString().Trim()))
                {
                    lblMsgRed.Text = "Please provide password.";
                    txtusername.Focus();
                    return;
                }
                if (string.IsNullOrEmpty(txtCpassword.Text.ToString().Trim()))
                {
                    lblMsgRed.Text = "Please provide confirm password.";
                    txtusername.Focus();
                    return;
                }
                string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
                int count = 0;
                foreach (var item in specialChar)
                {
                    if (txtNewPassword.Text.Trim().Contains(item))
                    {
                        count += 1;
                    }
                }
                if (count == 0)
                {
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "At least one special charecter required";
                    return;
                }
                string SQLString = "OTPVerification '" + lbluserid.Text + "','" + txtotp.Text + "' ";
                DataTable dt = Utils.GetDataTable(SQLString, CnnString);
                if (dt.Rows.Count == 0)
                {
                    txtotp.Text = string.Empty;
                    lblMsgRed.Visible = true;
                    lblMsgRed.Text = "OTP did not match.";
                    txtotp.Focus();
                    return;
                }
                SQLString = "ResetPassword  '" + txtNewPassword.Text.ToString().Trim() + "','" + lbluserid.Text.ToString().Trim() + "'";
                int i = Utils.ExecNonQuery(SQLString, CnnString);
                if (i > 0)
                {
                    lblMsgBlue.Text = "Password updated successfully";
                    lblMsgBlue.Visible = true;
                    lblMsgRed.Text = "";
                    lblMsgRed.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMsgRed.Text = ex.Message.ToString();
                lblMsgRed.Visible = true;
            }
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
                string otp = Otp();
                Session["otp"] = otp;
               // string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "";
                  //string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "";
                string SMSText = "Welcome to UTSAV. Your OTP is:- " + otp.ToString() + "(Team Rosmerta)";
                string result = SMSSend(MobileNo, SMSText);
                int status = Utils.ExecNonQuery("insert into OtpMobile(UserMobileNo,UserLoginName,OTPNo,FromPage,Result) values('" + MobileNo + "','" + UserId + "','" + otp.ToString() + "','forgotpassword','" + result + "')", CnnString1);
                //textBoxMobileNo.Enabled = false; 
                if (status == 1)
                {
                    msg = "1";
                }
            }
            return msg;
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
        public string Otp()
        {
            Random r = new Random();
            var x = r.Next(0, 9999);
            return x.ToString("0000");
        }
    }
}