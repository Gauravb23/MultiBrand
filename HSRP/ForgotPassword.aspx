<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="HSRP.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="Scripts/Dashboard/publish.min.css" rel="stylesheet" />
    <link href="css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/gridStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <link href="windowfiles/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script src="windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <link href="css/button.css" rel="stylesheet" />
    <script>
        function checkusername() {
            if (document.getElementById('txtusername').value == "") {
                alert('Username cannot be Blank..');
                document.getElementById('txtpassword').focus();     
                return false;
            }
        }
        function checkpass() {
            if (document.getElementById('txtotp').value == "") {
                alert('OTP cannot be Blank..');
                document.getElementById('txtotp').focus();
                return false;
            }
            if (document.getElementById('txtNewPassword').value == "") {
                alert('Password cannot be Blank..');
                document.getElementById('txtNewPassword').focus();
                return false;
            }
            if (document.getElementById('txtCpassword').value == "") {
                alert('Confirm password cannot be Blank..');
                document.getElementById('txtCpassword').focus();
                return false;
            }
            if (document.getElementById('txtCpassword').value != document.getElementById('txtNewPassword').value) {
                alert('Confirm password and New password are not same..');
                return false;
            }
        }
    </script>
    <style>
        .alert{
            color:red;
        }
        .bookingform {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-12 col-lg-12 col-sm-12">
                    <div class="panel-group">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Reset Password
                            </div>
                            <div class="panel-body">
                                <div class="row bookingform" id="otprow" runat="server">
                                    <div class="col-md-4 col-lg-4 col-sm-12 col-xs-12">
                                        User Name <span class="alert">* </span>
                                        <asp:TextBox ID="txtusername" runat="server" MaxLength="20" CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lbluserid" runat="server" Visible="false"></asp:Label>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-12 col-xs-12 bookingform" style="text-align:center;">
                                        <asp:Label ID="temp" runat="server">&nbsp;</asp:Label>
                                        <asp:Button runat="server" ID="btnGetotp" Text="Get Otp" class="btn btn-success btn-sm" OnClick="btnGetotp_Click" OnClientClick="return checkusername();" />
                                    </div>
                                </div>
                                <div class="row bookingform" id="pwdrow" runat="server">
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        OTP <span class="alert">* </span>
                                        <asp:TextBox ID="txtotp" runat="server" MaxLength="4" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        New Password <span class="alert">* </span>
                                        <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="15" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                        <span class="alert">At least one special charecter.</span>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        Confirm Password <span class="alert">* </span>
                                        <asp:TextBox ID="txtCpassword" runat="server" MaxLength="15" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row bookingform" id="btnresetrow" runat="server" style="text-align:center;">
                                    <asp:Button runat="server" ID="btnReset" Text="Reset Password" class="btn btn-success btn-sm" OnClick="btnReset_Click" OnClientClick="return checkpass();"/>
                                </div>
                                <div class="row bookingform">
                                    <asp:Label ID="lblMsgBlue" CssClass="bluelink" runat="server" ForeColor="Blue" Text="Label" Visible="false"></asp:Label>
                                    <asp:Label ID="lblMsgRed" CssClass="alert2" runat="server" ForeColor="Red" Text="Label" Visible="false"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
