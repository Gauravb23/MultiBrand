<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FirstLoginChangePassword.aspx.cs"
    Inherits="HSRP.ChangePassword1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <link href="Scripts/Dashboard/publish.min.css" rel="stylesheet" />
    <link href="css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/gridStyle.css" rel="stylesheet" type="text/css" />
    <link href="css/calendarStyle.css" rel="stylesheet" type="text/css" />
    <link href="windowfiles/dhtmlwindow.css" rel="stylesheet" type="text/css" />
    <script src="windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <link href="css/button.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="https://fonts.googleapis.com/css?family=Work+Sans:100,200,300,400,500,600,700,800,900" />
    <link href="netdna-bootstrapcdn-com/font-awesome/3-2-1/css/font-awesome.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Satisfy" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Work+Sans:400,500,700" rel="stylesheet" />
    <script language="javascript" type="text/jscript">
        function checkPass() {
            debugger;            
            if (document.getElementById('txtpassword').value == "") {
                alert('Old Password cannot be Blank..');
                document.getElementById('txtpassword').focus();
                return false;
            }
            else if (document.getElementById('txtNewPassword').value == "") {
                alert('New Password cannot be Blank..');
                document.getElementById('txtNewPassword').focus();
                return false;
            }
            else if (document.getElementById('txtCpassword').value == "") {
                alert('Confirm password cannot be Blank..');
                return false;
            }
            else if (document.getElementById('txtCpassword').value != document.getElementById('txtNewPassword').value) {
                alert('Confirm password and New password are not same..');
                return false;
            }
            else if (document.getElementById('txtNewPassword').value == document.getElementById('txtpassword').value) {
                alert('Old password and Confirm password are same..');
                return false;
            }
            else if (document.getElementById('txtNewPassword').value.length < 8) {
                alert('Password is less than eight character');
                document.getElementById('txtNewPassword').value = "";
                document.getElementById('txtCpassword').value = "";
                document.getElementById('txtNewPassword').focus();
                return false;
            }
            else if (document.getElementById('ddlSecurityQuestion').value == "") {
                alert('Please select Security Question !!!!');
                document.getElementById('ddlSecurityQuestion').focus();
                return false;
            }
            else if (document.getElementById('txtSecurityAnswer').value == "") {
                alert('Security Answer cannot be blank!!!!');                
                document.getElementById('txtSecurityAnswer').focus();
                return false;
            }
            else if (document.getElementById('txtGstNo').value == "") {
                alert('GSTIN cannot be Blank..');
                document.getElementById('txtGstNo').focus();
                return false;
            }
            else if (document.getElementById('txtPan').value == "") {
                alert('PAN No. cannot be Blank..');
                document.getElementById('txtPan').focus();
                return false;
            }
            else if (document.getElementById('txtAddress').value == "") {
                alert('Address cannot be blank!!!!');
                document.getElementById('txtAddress').focus();
                return false;
            }
            else if (document.getElementById('txtPinCode').value == "") {
                alert('Pincode cannot be blank!!!!');
                document.getElementById('txtPinCode').focus();
                return false;
            }
            else if (document.getElementById('txtOemgivendealercode').value == "") {
                alert('Oem Given Dealer Code cannot be blank!!!!');
                document.getElementById('txtOemgivendealercode').focus();
                return false;
            }
           
            else {
                return true;
                 }
        }

        //function checkPass1() {
        //    debugger;
        //    if (document.getElementById('txtemail').value == "") {
        //        alert('Email cannot be Blank..');
        //        document.getElementById('txtemail').focus();
        //        return false;
        //    }
        //    else if (document.getElementById('txtMobilenumber').value == "") {
        //        alert('Mobile number cannot be Blank..');
        //        document.getElementById(' txtMobilenumber').focus();
        //        return false;
        //    }
        //}
    </script>

    <script type="text/javascript" language="javascript">
        function specialcharecter() {
            var iChars = "!`@#$%^&*()';,.";
            var txtVehicleNo = document.getElementById("<%=txtOemgivendealercode.ClientID%>").value;
        for (var i = 0; i < txtVehicleNo.length; i++) {
            if (iChars.indexOf(txtVehicleNo.charAt(i)) != -1) {
                alert("Oem Given Dealer Code is not valid.");
                    document.getElementById("<%=txtOemgivendealercode.ClientID%>").value = "";
                    return false;
                }
            }
        }

    </script>

    <script type="text/javascript" language="javascript">
        function specialcharecter() {
            var iChars = "!`@#$%^&*()';,.";
            var pincode = document.getElementById("<%=txtPinCode.ClientID%>").value;
            for (var i = 0; i < pincode.length; i++) {
                if (iChars.indexOf(pincode.charAt(i)) != -1) {
                alert("Pin Code is not valid.");
                    document.getElementById("<%=txtPinCode.ClientID%>").value = "";
                    return false;
                }
            }
        }

    </script>

    <style>
        .bookingform {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="iparys_inherited">
            <div class="section">
                <link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet">
                <div class="utilityContainer1">
                    <div class="container">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="top-header">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="header-bluebg">
                                                <div class="guideline">
                                                    <div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!--end of class col-md-12-->
                                            <div class="col-md-5">
                                                <figure class="logo">
                                                    <a href="LiveReports/LiveTracking.aspx">
                                                        <img class="" src="content/dam/doitassets/eesl/images/logo-en-main.png" alt="logo">
                                                    </a>
                                                </figure>
                                            </div>
                                            <div class="col-md-2"></div>
                                            <div class="col-md-3" style="margin-top: 23px;">
                                                <span class="whiteh">Welcome </span>
                                                <asp:Label ID="lblUser" runat="server" />
                                            </div>
                                            <div class="col-md-2" style="margin-top: 23px;">
                                                <asp:LinkButton Style="background-color: transparent; float: right;" runat="server" CausesValidation="false" CssClass="button_logout" ID="buttonLogout" Text="Logout" OnClick="buttonLogout_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div class="container" style="margin-bottom:150px">
            <div class="row">
                <div class="col-md-12 col-lg-12 col-sm-12">
                    <div class="panel-group">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                <asp:ImageButton ID="btnBack" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="images/button/back.png" Style="height: 24px; width: 50px;" OnClick="btnBack_Click" />
                                <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  Change Password"></asp:Label>
                            </div>
                            <div class="panel-body">
                                <div class="row">
                                    <span class="heading1" style="color: Blue">You are forced to change your Password. Since your password expired or You are logging
                                                        in for the first time.
                                    </span>
                                </div>
                                <div class="row bookingform">
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblusername" runat="server"> User Name <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtusername" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                                        <span controltovalidate="txtName" errormessage="Please Enter Name" display="Dynamic"
                                            id="reqName" class="header" evaluationfunction="RequiredFieldValidatorEvaluateIsValid"
                                            initialvalue="" style="color: Red; display: none;">Please                               
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        Email <span class="alert" style="color: #FF3300">* </span>
                                        <asp:TextBox ID="txtemail" runat="server" MaxLength="50" CssClass="form-control" autocomplete="off" placeholder="example@gmail.com"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        Mobile Number <span class="alert" style="color: #FF3300">* </span>
                                        <asp:TextBox ID="txtMobilenumber" runat="server" MaxLength="10" CssClass="form-control" onkeypress="return isNumberKey(event)"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="row bookingform">
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lbloldpwd" runat="server" Visible="false"> Old Password <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtpassword" runat="server" MaxLength="15" TextMode="Password" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblnewpwd" runat="server" Visible="false"> New Password <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="15" TextMode="Password"
                                            CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblconfpwd" runat="server" Visible="false"> <span class="form_text">Confirm Password <span class="alert" style="color: #FF3300">* </span></span></asp:Label>
                                        <asp:TextBox CssClass="form-control" ID="txtCpassword" runat="server" MaxLength="15"
                                            TextMode="Password" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row bookingform">
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblsecurityque" runat="server" Visible="false">  Security Questions <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:DropDownList ID="ddlSecurityQuestion" runat="server" CssClass="form-control" DataTextField="QuestionText" Visible="false"
                                            DataValueField="QuestionID">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblsecurityanswer" runat="server" Visible="false">  Security Answer <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtSecurityAnswer" runat="server" CssClass="form-control" TextMode="Password" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblgstno" runat="server" Visible="false">  GST No <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtGstNo" runat="server" MaxLength="15" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                     
                                </div>
                                <div class="row bookingform">
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        

                                        <asp:Label ID="lblAddress" runat="server" Visible="false">Address<span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="200" CssClass="form-control" Visible="false"></asp:TextBox>

                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                         <asp:Label ID="lblPincode" runat="server" Visible="false">PinCode<span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtPinCode" runat="server" MaxLength="6" CssClass="form-control" Visible="false" onkeypress="return isNumberKey(event)" onBlur="specialcharecter()"></asp:TextBox>

                                        
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">                                       

                                        <asp:Label ID="lblOemgivendealercode" runat="server" Visible="false">Oem Given Dealer Code<span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtOemgivendealercode" runat="server" MaxLength="15" CssClass="form-control" Visible="false" onBlur="specialcharecter()"></asp:TextBox>

                                    </div>
                                </div>
                                <div class="row bookingform">
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <asp:Label ID="lblPan" runat="server" Visible="false">  PAN No <span class="alert" style="color: #FF3300">* </span></asp:Label>
                                        <asp:TextBox ID="txtPan" runat="server" MaxLength="10" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                      <asp:Label ID="lblotp" runat="server" Visible="false">Enter OTP<span class="alert" style="color: #FF3300">* </span></asp:Label> 
                                        <asp:TextBox ID="txtotp" runat="server" MaxLength="4" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">

                                        <asp:ImageButton ID="btnProceed" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="images/button/save.jpg" Style="height: 24px; width: 69px;" OnClick="btnProceed_Click" OnClientClick="return checkPass();" />

                                        <%--<asp:Button ID="btnProceed" runat="server" class="button" OnClick="btnProceed_Click"
                                    OnClientClick="return checkPass();" Text="Proceed" />--%>
                                         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                      <asp:ImageButton ID="btngetotp" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="images/button/getotp.jpg" Style="height: 24px; width: 69px;" OnClick="btngetotp_Click" OnClientClick="return checkPass1();" />
                                        <%--<asp:Button ID="btnBack" runat="server" class="button" Text="Back" OnClick="btnBack_Click" />--%>
                                    </div>
                                    <div class="col-md-4"></div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">
                                        <asp:LinkButton ID="lnkBtnresendotp" runat="server" OnClick="lnkBtnresendotp_Click" ForeColor="Blue">Resend OTP</asp:LinkButton>
                                    </div>
                                    <div class="col-md-4"></div>

                                </div>                          

                                <div class="row">                                   
                                   
                                    <asp:Label ID="lblSuccMess" runat="server" ForeColor="Blue" style="margin-left:10px;" Font-Size="18px"></asp:Label>
                                    <asp:Label ID="lblErrMess" ForeColor="Red" Font-Size="18px" style="margin-left:10px;" runat="server"></asp:Label>
                                   
                                </div>
                             </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="iparys_inherited">
            <div class="section">
                <div class="footer-outer" style="bottom: 0; position: fixed">
                    <div class="container">
                        <div class="row">
                            <div class="col-lg-12">
                                <footer id="footer">
                                    <div class="footerRow">
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                           <%-- <ul class='linklist'>
                                                <li><a href="#" target="_self">USS Policies</a></li>
                                                <li><a href="#" target="_self">Feedback</a></li>
                                                <li><a href="#" target="_self">Sitemap</a></li>
                                                <li><a href="#" target="_self">Help</a></li>
                                            </ul>--%>
                                            <%--<span class="main-hdg1">Related Links:</span><br>
                                            <ul id="extlink">
                                                <script>
                                                    $(document).ready(function () {
                                                        var url = "/bin/nexteon/eesl/menuData";
                                                        $.ajax({
                                                            url: url, type: "POST", datatype: "application/json", data: { "menuId": "10" }, success: function (result) {
                                                                if (jQuery.isEmptyObject(result)) {
                                                                    $(".menuData").html("<h2 style='text-align:center;color: red;'>UNDER MODIFICATION</h2>");
                                                                } else {
                                                                    var html = "";
                                                                    for (var key in result) {
                                                                        var record = result[key];
                                                                        var sno = parseInt(key) + 1;
                                                                        html += "<li>" +
                                                                            "<a href='" + record.linkUrl + "' target='_blank' rel='noopener noreferrer'>" + record.subject + "</a>" +
                                                                            "</li>";
                                                                    }
                                                                    $("#extlink").append(html);
                                                                }
                                                            }
                                                        });
                                                    });
                                                </script>
                                            </ul>--%>
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12 text-right">
                                            <%--<ul class="socialLink">
                                                <ul class='socialLink'>
                                                    <li>
                                                        <li><a href="#"><i class="fa fa-facebook"></i></a></li>
                                                    </li>
                                                    <li>
                                                        <li><a href="#"><i class="fa fa-twitter"></i></a></li>
                                                    </li>
                                                    <li>
                                                        <li><a href="#"><i class="fa fa-youtube"></i></a></li>
                                                    </li>
                                                    <li>
                                                        <li><a href="#"><i class="fa fa-linkedin"></i></a></li>
                                                    </li>
                                                </ul>
                                            </ul>
                                            <div class="pull-right1">
                                                <span class="counter" id="counter">
                                                    <span>
                                                        <a href="#" target="_blank" rel="noopener noreferrer">
                                                            <!--  <em>Visitor No.</em> <img src="http://www.cutercounter.com/hit.php?id=gaoxac&nd=8&style=64" border="0" alt="hit counter"> -->
                                                        </a>
                                                    </span>
                                                </span>
                                            </div>--%>
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-12 col-xs-12">
                                            <div class="footerLink">
                                                <ul>
                                                    <li>
                                                        <div class="baseRichText">
                                                            <p>UTSAV SAFETY SYSTEMS PVT. LTD.</p>
                                                        </div>
                                                    </li>

                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </footer>
                            </div>
                        </div>
                    </div>
                </div>
                <style>
                    .gandhi1 {
                        position: absolute;
                        bottom: 0;
                        z-index: 999;
                        width: 140px;
                        right: 300px;
                </style>
                <%--<div class="contactus_button">
                    <a href="javascript:;" data-toggle="modal" data-target="#helpdesk-model" title="Helpdesk (Working days from 10 AM to 6 PM)">
                        <img src="content/dam/doitassets/eesl/images/landline.png" alt="">
                        <div class="contactus_button_text">HELPDESK</div>
                    </a>
                </div>
                <!-- Modal -->
                <div id="helpdesk-model" class="modal fade" role="dialog">
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal">&times;</button>
                                <h4 class="modal-title">EESL Helpdesk</h4>
                            </div>
                            <div class="modal-body">
                                <p><img src="content/dam/doitassets/eesl/images/phone-icon.jpg" alt=""> <span>EESL Corporate Office: 011- 45801260</span></p>
                                <p><img src="content/dam/doitassets/eesl/images/phone-icon.jpg" alt=""> <span>Toll- Free no. for complaints:  1800 180 3580</span></p>
                                <p><img src="content/dam/doitassets/eesl/images/email-icon.jpg" alt=""> <span>helpline@eesl.co.in</span></p>
                                <p><img src="content/dam/doitassets/eesl/images/logde-comp-icon.jpg" alt=""> <span>Lodge a Complaint</span>  <span class="anclink_2"><a href="raj.support_subdomain/index.html" target="_blank">Rajasthan</a></span>  <span class="anclink_2"><a href="support_subdomain/index.html" target="_blank">Other States</a></span></p>
                            </div>
                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
    </form>
</body>

</html>
