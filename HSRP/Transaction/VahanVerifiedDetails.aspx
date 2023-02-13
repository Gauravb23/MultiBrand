<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VahanVerifiedDetails.aspx.cs" Inherits="HSRP.Transaction.VahanVerifiedDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        div[class^="col-"]:not(:first-child) {   padding-bottom:10px; } div[class^="col-"]:not(:last-child) {   padding-right: 70px; }
    </style>

 

    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>

    <div class="container">
        <div class="row g-5">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:ImageButton  UseSubmitBehavior="false" ID="ImageButton1" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="../images/button/back.png" Style="height: 27px; width: 27px; margin-top: -4px;" OnClientClick="JavaScript: window.history.back(1); return false;" />&nbsp;&nbsp;
                            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="Vahan Details"></asp:Label>
                            <a href="../LiveReports/LiveTracking.aspx" style="float: right"  UseSubmitBehavior="false" >
                                <img class="" src="../images/button/home.png" alt="logo" style="height: 27px; width: 27px; margin-top: -4px;">
                            </a>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-4 col-lg-4 col-sm-4">

                                <div class="row">
                                    <div class="col-sm-6">
                                        Registration No <span style="color: #FF3300">*</span>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtRegNumber" class="form-control" runat="server" Style="text-transform: uppercase" MaxLength="10" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        Chassis No <span style="color: #FF3300">*</span>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtChassisno" runat="server" class="form-control" MaxLength="25" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        Engine No <span style="color: #FF3300">*</span>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtEngineNo" runat="server" class="form-control" MaxLength="35" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row ">
                                    <div class="col-sm-6">
                                        <%--<asp:ImageButton ID="btnGO" runat="server" AlternateText="ImageButton 1" TabIndex="4"  ImageUrl="../images/button/btngo.jpg" style="height:30px;object-fit:contain;" OnClick="btnView_Click" />--%>
                                        <asp:Button runat="server" ID="btnView" Text="GO" CssClass="btn btn-primary" OnClick="btnView_Click" />
                                        <asp:Button runat="server" ID="btnRefresh" Text="Refresh" CssClass="btn btn-warning" OnClick="btnRefresh_Click" />
                                    </div>
                                </div>

                            </div>
                            <div id="divmsg" runat="server" visible="false" class="col-md-8 col-lg-8 col-sm-8">
                                <div class="row">
                                    <div class="row">
                                        <div class="col-sm-6">
                                            Response : 
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblmsg" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            Maker :  
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblmaker" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            Fuel Type : 
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblFuel" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            Vehicle Type : 
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblVehType" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            Vehicle Stage Type :  
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblStageType" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-6">
                                            Vehicle Class :     
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblVehClass" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-6">
                                            Registration Date :  
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblregDate" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="row">

                                        <div class="col-sm-6">
                                            State Code : 
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblstateCode" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>


                                    <div class="row">
                                        <div class="col-sm-6">
                                            HSRP Front Laser Code :     
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblfCode" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-6">
                                            HSRP Rear Laser Code : 
                                        </div>
                                        <div class="col-sm-6">
                                            <asp:Label ID="lblrCode" runat="server" Font-Size="18px"></asp:Label>
                                        </div>
                                    </div>

                                </div>
                            </div>

                        </div>
                        <%--Panel Body Div--%>

                        <div class="row" style="margin-left: 10px;">
                            <asp:Label ID="llbMSGError" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </div>

       <script type="text/javascript" language="javascript">

           var input = document.getElementsByTagName('input')
           for (i = 0; i <= input.length; i++) {
               input[i].addEventListener("keypress", function (event) {
                   debugger;
                
                   if (event.keyCode === 13) {
                     
                       event.preventDefault();
                       document.getElementById("ctl00_ContentPlaceHolder1_btnView").focus();
                       document.getElementById("ctl00_ContentPlaceHolder1_btnView").click();
                   }
               });

           }
        


       </script>
</asp:Content>
