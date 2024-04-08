<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login_OLD.aspx.cs" Inherits="HSRP.Login_OLD" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" href="../images/logo.ico" type="image/x-icon" />
    <title>HSRP Application Ver1.0</title>
    <link rel="stylesheet" type="text/css" href="css/LoginCss/vendor/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/fonts/font-awesome-4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/fonts/Linearicons-Free-v1.0.0/icon-font.min.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/vendor/animate/animate.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/vendor/css-hamburgers/hamburgers.min.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/vendor/animsition/css/animsition.min.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/vendor/select2/select2.min.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/vendor/daterangepicker/daterangepicker.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/css/util.css" />
    <link rel="stylesheet" type="text/css" href="css/LoginCss/css/main.css" />
    <script type="text/javascript" src="windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="windowfiles/dhtmlwindow.css" type="text/css" />
    <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <style>
        .login100-form {
           border-left: 1px solid black;
            height: 550px;
        }
    </style>

   <%-- <style>
        body:before{
    position:absolute;
    top : 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: black;
    content:"";
    pointer-events: none;
    z-index:100;
    opacity: .5

}
</style>--%>

   <%-- <style>
 #ModalPopupExtender1 
{
    height:100%;
    background-color:#EBEBEB;
    filter:alpha(opacity=70);
    opacity:0.7;
}

 .blur {
  -webkit-backdrop-filter: saturate(180%) blur(20px);
  backdrop-filter: saturate(180%) blur(20px);
}

.remove:before {
  content:none
}
 
    </style>--%>

  <%--  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
<script>

    $(document).ready(function () {
        $('#btnClose').on('click', function () {
            $('body').addClass("remove");

        })
    });
</script>--%>

    <script type="text/javascript">
        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes
            googlewin = dhtmlwindow.open("googlebox", "iframe", "ForgotPassword.aspx", "Change Password", "width=700px,height=400px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = 'Login.aspx';
                return true;
            }
        }
        function validateForm() {

            var username = document.getElementById("txtUserID").value;
            var pass = document.getElementById("txtUserPassword").value;
            var Captcha = document.getElementById("txtCaptcha").value;
            if (username == '') {
                swal("Please Provide User Name!", "", "error");
                document.getElementById("txtUserID").focus();
                return false;
            }
            if (pass == '') {
                swal('Please Provide Password.', '', 'error');
                document.getElementById("txtUserPassword").focus();
                return false;
            }
            if (Captcha == '') {
                swal('Please Provide Captcha.', '', 'error');
                document.getElementById("txtCaptcha").focus();
                return false;
            }
            return true;
        }
    </script>

    <%--<script type="text/javascript" language="javascript">

         function DisableBackButton() {
             window.history.forward()
         }
         DisableBackButton();
         window.onload = DisableBackButton;
         window.onpageshow = function (evt) { if (evt.persisted) DisableBackButton() }
         window.onunload = function () { void (0) }
     </script>--%>
</head>
<body>
    <form class="limiter" runat="server">

       <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:ModalPopupExtender  ID="ModalPopupExtender1" runat="server" TargetControlID="hidVal"
            PopupControlID="divMessage" CancelControlID="btnClose" PopupDragHandleControlID="divheader">
        </asp:ModalPopupExtender>
        <input type="hidden" id="hidVal" runat="server" />
        <div runat="server" id="divMessage">
            <table border="0">
                <tr>
                        
                        
                    <td style="text-align:right;"> <asp:Button ID="btnClose" runat="server" ForeColor="Black" Text="Close" /></td>
                </tr>
                <tr>
                    <td>
                        
                        <div>
                            <img src="images/banner.jpg" />
                          </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                       
                    </td>
                </tr>
            </table>
        </div>--%>

        <div class="container-login100">
            <div class="wrap-login100">
                <asp:Label runat="server" ID="Label1" Text="Dealer Support Numbers: 9205180518<span> | </span>9205580558" ForeColor="Green" Style="color: Green; font-size: 14px; font-weight: bold; float: right; position: relative; right: 11px;" Visible="true"></asp:Label>
                <marquee behavior="alternate" direction="left" onmouseover="this.stop();" onmouseout="this.start();">
                    <%--<asp:Label runat="server" ID="msg" Text="Name of the company has changed from Utsav Safety Systems Ltd. to Rosmerta Safety Systems Ltd." ForeColor="Green" style="color: Green;font-size: 18px;font-weight: bold;" Visible="true" ></asp:Label>--%>
                </marquee>
                <div class="login100-form validate-form">
                    <%-- <a href="http://utsavhsrp.com/oemlist.html" style="height: 31px; width: 201px; margin-left: 65%; margin-top: -25%; color: #e45225;" target="_blank">Register as Dealer</a>--%>
                    <span class="login100-form-title p-b-34" style="font-weight: bold; font-size: 27px;">DEALER Login
                    </span>
                    <div class="wrap-input100 rs1-wrap-input100 m-b-20" data-validate="Type user name" id="txtuseriddiv" runat="server">
                        <asp:TextBox ID="txtUserID" class="input100" TabIndex="1" runat="server" placeholder="User name" ValidationGroup="OTP" MaxLength="30" autocomplete="off"></asp:TextBox>
                        <span class="focus-input100"></span>
                    </div>
                    <div class="wrap-input100 rs2-wrap-input100 m-b-20" data-validate="Type password" id="txtpwddiv" runat="server">
                        <asp:TextBox ID="txtUserPassword" class="input100" TabIndex="2" TextMode="Password" placeholder="Password"
                            runat="server" ValidationGroup="OTP" MaxLength="15" autocomplete="off"></asp:TextBox>
                        <span class="focus-input100"></span>
                    </div>
                    <div class="wrap-input100 rs1-wrap-input100 m-b-20" data-validate="Type Cptcha" id="lblcaptchaddiv" runat="server">
                        <asp:Image ID="imgCaptcha" runat="server" Height="41px" Width="122px" />
                        <asp:ImageButton ID="btnRefresh" runat="server" AlternateText="ImageButton 1" ImageAlign="right" ImageUrl="images/button/refreshbtn.jpg" OnClick="btnRefresh_Click1" Style="height: 41px; width: 41px;" ValidationGroup="refresh" />
                        <span class="focus-input100"></span>
                    </div>
                    <div class="wrap-input100 rs2-wrap-input100 m-b-20" data-validate="Type captcha" id="txtcaptchadiv" runat="server">
                        <asp:TextBox ID="txtCaptcha" TabIndex="3" class="input100" runat="server" ValidationGroup="OTP" placeholder="Captcha" MaxLength="6" autocomplete="off"></asp:TextBox>
                        <span class="focus-input100"></span>
                    </div>
                    <div class="rs1-wrap-input100 m-b-20" id="lblotpdiv" runat="server">
                        <asp:Label runat="server" ID="lblotp" Text="Enter OTP" class="input100" Style="height: 41px; padding: 10px 25px;"></asp:Label>
                        <span class="focus-input100"></span>
                    </div>
                    <div class="wrap-input100 rs1-wrap-input100 m-b-20" id="txtotpdiv" runat="server">
                        <asp:TextBox ID="txtOTP" class="input100" TabIndex="2" TextMode="Password" placeholder="OTP"
                            runat="server" ValidationGroup="LOGIN" MaxLength="4"></asp:TextBox>
                        <span class="focus-input100"></span>
                    </div>
                    <div class="container-login100-form-btn">
                        <asp:Button ID="btnLogin" TabIndex="4" runat="server" Text="LOGIN" OnClick="btnLogin_Click" OnClientClick="javascript: return validateForm();" class="login100-form-btn" ValidationGroup="OTP" />
                        <asp:Button ID="btnOTP" runat="server" Text="LOGIN" OnClick="btnOTP_Click" OnClientClick="javascript: return validateForm();" class="login100-form-btn" Visible="false" ValidationGroup="LOGIN" />
                    </div>

                    <div class="w-full text-center p-t-27 p-b-239">
                        <span>
                            <asp:LinkButton ID="lnkBtnresendotp" Visible="false" runat="server" OnClick="lnkBtnresendotp_Click" ForeColor="Blue">Resend OTP</asp:LinkButton>
                        </span>
                        <br />
                        <span class="txt1" style="color: black; font-weight: bold;">Only Registered mobile user can login.
                        </span>
                        <br />
                        <span>
                            <a onclick="AddNewPop();" href="#" style="color: blue;">Forgot Password?
                            </a>
                        </span>
                        <br />
                        <br />
                        <span class="txt1" style="color: black; font-weight: bold;">Dealer Support Numbers 
                        </span>

                        <span class="txt1" style="color: black; font-weight: bold;">9205180518 </span>
                        <span class="txt1" style="color: black; font-weight: bold;">9205580558 </span>



                        <br />

                        <span class="txt1" style="color: black; font-weight: bold; margin-left: -45px;">For Dealer support 
                        </span>
                        <span class="txt1" style="color: black; font-weight: bold;">Please contact - Monday to Saturday 09:30 AM to 06:00 PM </span>
                        <br />
                        <br />
                        <span class="txt1" style="color: black; font-weight: bold;">Dealer Support E-Mail ID:
                        </span>

                        <span class="txt1" style="color: black; font-weight: bold;">appsupport@rosmertasafety.com</span>
                        <span class="txt1" style="color: black; font-weight: bold;"></span>
                        <br />


                        <span class="txt1" style="color: black; font-weight: bold;">Vahan Support E-Mail ID:
                        </span>

                        <span class="txt1" style="color: black; font-weight: bold;">vahansupport@rosmertahsrp.com </span>
                        <br />

                        <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" ForeColor="Blue" Text="Label"></asp:Label>
                        <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label"></asp:Label>
                    </div>

                    <div class="w-full text-center">
                    </div>
                </div>

                <div>
                    <div class="col-md-10 col-lg-4 col-sm-4" style="display: contents">
                        <img src="images/utsav_logo.png" />
                    </div>
                    <div class="col-md-10 col-lg-4 col-sm-4" style="display: contents">
                         <img src="content/dam/doitassets/eesl/images/attention.jpg" style="display: block; margin: 50px auto 0;" height="350" />
                        <%--<img src="images/NewYear.png" style="display: block; margin: 150px auto 0;" height="270" />--%>
                    </div>
                </div>
            </div>
        </div>
    </form> 
    


    <div id="dropDownSelect1"></div>
    <script src="css/LoginCss/vendor/jquery/jquery-3.2.1.min.js"></script>
    <script src="css/LoginCss/vendor/animsition/js/animsition.min.js"></script>
    <script src="css/LoginCss/vendor/bootstrap/js/popper.js"></script>
    <script src="css/LoginCss/vendor/bootstrap/js/bootstrap.min.js"></script>
    <script src="css/LoginCss/vendor/select2/select2.min.js"></script>
    <script>
        $(".selection-2").select2({
            minimumResultsForSearch: 20,
            dropdownParent: $('#dropDownSelect1')
        });
    </script>
    <script src="css/LoginCss/vendor/daterangepicker/moment.min.js"></script>
    <script src="css/LoginCss/vendor/daterangepicker/daterangepicker.js"></script>
    <script src="css/LoginCss/vendor/countdowntime/countdowntime.js"></script>
    <script src="css/LoginCss/js/main.js"></script>
</body>
</html>
