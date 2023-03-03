<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="StickerProcess_OLD.aspx.cs" Inherits="HSRP.Transaction.StickerProcess_OLD" EnableEventValidation="false" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    

    <script type="text/javascript">
        function OrderDate_OnDateChange(sender, eventArgs) {
            var fromDate = OrderDate.getSelectedDate();
            CalendarOrderDate.setSelectedDate(fromDate);
        }

        function OrderDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarOrderDate.getSelectedDate();
            OrderDate.setSelectedDate(fromDate);
        }

        function OrderDate_OnClick() {
            if (CalendarOrderDate.get_popUpShowing()) {
                CalendarOrderDate.hide();
            }
            else {
                CalendarOrderDate.setSelectedDate(OrderDate.getSelectedDate());
                CalendarOrderDate.show();
            }
        }

        function OrderDate_OnMouseUp() {
            if (CalendarOrderDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function HSRPAuthDate_OnDateChange(sender, eventArgs) {
            var fromDate = HSRPAuthDate.getSelectedDate();
            CalendarHSRPAuthDate.setSelectedDate(fromDate);

        }

        function HSRPAuthDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarHSRPAuthDate.getSelectedDate();
            HSRPAuthDate.setSelectedDate(fromDate);

        }

        function HSRPAuthDate_OnClick() {
            if (CalendarHSRPAuthDate.get_popUpShowing()) {
                CalendarHSRPAuthDate.hide();
            }
            else {
                CalendarHSRPAuthDate.setSelectedDate(HSRPAuthDate.getSelectedDate());
                CalendarHSRPAuthDate.show();
            }
        }

        function HSRPAuthDate_OnMouseUp() {
            if (CalendarHSRPAuthDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function ischarKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 96 || charCode > 122) && (charCode < 65 || charCode > 90) && (charCode < 31 || charCode > 33))
                return false;

            return true;
        }



    </script>
    <script type="text/javascript">
        function UploadExcelfun() {
            window.location.href = "UploadDealerdata.aspx";
        }

        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_btnSave").hide();


        });

        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 46 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 57))
                return false;

            return true;
        }

        $(document).on("click", ".alert1", function (e) {


            var regno = $('#<%=txtEngineNo.ClientID %>').val()
            if (regno == "") {

                bootbox.alert("Please Enter Engine No. and Click On Go Button !", function () {

                });
            }
            else {


                bootbox.confirm("<B>Please Confirm !</b> <BR><i>Please Ensure Customer Data On Screen Matches With Authorization Slip Before Save. Penalty Will Be Imposed On Generation Of Wrong Cash Receipt.</i>", function (result) {

                    if (result) {
                        $("#save1").hide();
                        $("#ctl00_ContentPlaceHolder1_btnSave").show();
                    }
                });
            }

        });

    </script>
    <style>
        
        .datepick {
            border: 1px solid #ccc;
            border-radius: 4px;
            width: 84%;
        }

        .bookingform {
            margin-bottom: 10px;
        }

        .oprion {
            /* Whatever color  you want */
            background-color: yellow;
        }
    </style>
   
    <div class="container">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="../images/button/back.png" Style="height: 27px; width: 27px; margin-top: -4px;" OnClientClick="JavaScript: window.history.back(1); return false;" />&nbsp;&nbsp;
                            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  Only Sticker "></asp:Label>
                            
<%--                            <table>
                                <tr>

                                </tr>
                            </table>
                            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  3rd Stricker Caution Petrol/CNG "> </asp:Label>
                            <asp:Label ID="TextBox1" class="form-control" runat="server" style="margin-left: 2px; width:50px;background-color:  blue !important; font-weight: bold;" autocomplete="off"></asp:Label>

                            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  Diesel"></asp:Label>
                            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  Hybird/Other"></asp:Label>
                            --%>
                            <a href="../LiveReports/LiveTracking.aspx" style="float: right">
                                <img class="" src="../images/button/home.png" alt="logo" style="height: 27px; width: 27px; margin-top: -4px;">
                            </a>
                            <!--&nbsp;&nbsp;
                             <asp:ImageButton ID="UploadExcel" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="right" ImageUrl="../images/button/upload.jpg" Style="height: 24px; width: 95px; margin-right: 10px;" OnClientClick="UploadExcelfun(); return false;" ValidationGroup="excel" />
                            &nbsp;&nbsp;&nbsp;&nbsp;-->
                        </div>
                        <div class="panel-body">

                            <div class="row">
                                 <div class="col-md-10 col-lg-10 col-sm-10" style="text-align:center;">
                                     <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                    <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                 </div>
                                <br />
                            </div>



                            <div class="row">
                                <div class="col-md-10 col-lg-10 col-sm-10"></div>
                                <div class="col-md-2 col-lg-2 col-sm-2" style="border: 1px ridge; background: #85b8e4; color: white;">Available Balance:&nbsp;&nbsp;&nbsp;<asp:Label ID="lblavailbal" runat="server" Font-Bold="true"></asp:Label></div>
                            </div>

                            <div class="row">
                                
                            </div>
                                                        <br />
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label6" Visible="true"> Oem Name <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        
                                       
                                            <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlOemName" runat="server" class="form-control" DataValueField="OemId" DataTextField="Name"  Enabled="true" >
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                             
                                        </div>

                                   
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label26" Visible="true"> Vehicle Stage Type <span style="color: #FF3300">*</span> <br /></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                             <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlVehicleStateType" runat="server" AutoPostBack="true"   class="form-control"  Enabled="true" OnSelectedIndexChanged="ddlVehicleStateType_SelectedIndexChanged" >
                                                        <asp:ListItem>-Select-</asp:ListItem>
                                                        <asp:ListItem Value="BS4 or Other">BS4 or Other</asp:ListItem>
                                                        <asp:ListItem Value="BS6">BS6</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                               
                            </div>
                            <br />

                            
                            <br />
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label3" Visible="true"> Registration No <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                             <asp:TextBox ID="txtRegNumber" class="form-control" runat="server" Style="text-transform: uppercase" MaxLength="10" autocomplete="off"></asp:TextBox>
                                              <asp:Label ID="lblauthno" Visible="false" runat="server" Text=""></asp:Label>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label14" Visible="true"> Chassis No <span style="color: #FF3300">*</span> <br /></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:TextBox ID="txtChassisno" runat="server" class="form-control" MaxLength="20" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1">
                                    <asp:ImageButton ID="ImgBtnCheck" runat="server" AlternateText="Check Data" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px;" OnClick="ImgBtnCheck_Click" />
                                </div>
                            </div>
                            <br />

                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label13" Visible="true"> Owner Name <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:TextBox ID="txtOwnerName" runat="server" class="form-control" MaxLength="100" autocomplete="off"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>
                                 <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label15" Visible="true"> Mobile No.   <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:TextBox ID="txtmobileno" runat="server" class="form-control" MaxLength="10" autocomplete="off"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            

                            
                            <br />
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label16" Visible="true"> Engine No <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                              <asp:TextBox ID="txtEngineNo" runat="server" class="form-control" MaxLength="20" autocomplete="off"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label18" Visible="true"> Vehicle Type <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlVehicletype" runat="server" class="form-control" DataValueField="VehicleType" DataTextField="VehicleType"  Enabled="False" >
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <br />
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label21" Visible="true"> Order Type <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control" Enabled="False" 
                                                AutoPostBack="True">
                                                <%--<asp:ListItem Value="-Select Order Type-">-Select Order Type-</asp:ListItem>--%>
                                                <asp:ListItem Value="OS">Only Sticker</asp:ListItem>
                                                <%--<asp:ListItem Value="NB">Both Plates (Front and Rear)</asp:ListItem>
                                                <asp:ListItem Value="DF">Damaged (Front)</asp:ListItem>
                                                <asp:ListItem Value="DR">Damaged (Rear)</asp:ListItem>
                                                <asp:ListItem Value="DB">Both Damaged (Front and Rear)</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label19" Visible="true"> Vehicle Class <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlVehicletype" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlVehicleclass" runat="server" CssClass="form-control"  Enabled="False" >
                                                        <asp:ListItem Value="-Select Vehicle Class-">-Select Vehicle Class-</asp:ListItem>
                                                        <asp:ListItem style="background-color: yellow !important; font-weight: bold;">Transport</asp:ListItem>
                                                        <asp:ListItem>Non-Transport</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>

                                        </div>
                                    </div>
                                </div>
                            </div>


                            
                            <br />
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4"">
                                            <asp:Label runat="server" ID="Label20" Visible="true">  Date of Registration<span style="color: #FF3300">*</span><br /> (dd/MM/yyyy) </asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:TextBox ID="txtVehRegDate" runat="server" class="form-control" MaxLength="10" autocomplete="off" placeholder="dd/MM/yyyy"></asp:TextBox>
                                             <%--<table width="100%">
                                                 <tr>
                                                     <td>
                                                         <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" ControlType="Picker" PickerCssClass="picker datepick" Visible="true" style="width: 100%;">
                                                             <ClientEvents>
                                                                 <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                                             </ClientEvents>
                                                         </ComponentArt:Calendar>
                                                     </td>
                                                     <td>   
                                                         <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                                             onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                     </td>
                                                 </tr>
                                             </table>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlVehicleclass" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                <ContentTemplate>
                                                    <asp:Label ID="lblAmount" Text="" runat="server" Visible="false"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>--%>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label22" Visible="true"> Fuel Type <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate >
                                            <asp:DropDownList ID="ddlFuelType" runat="server" CssClass="form-control" Visible="true">
                                                <asp:ListItem Value="-Select Fuel Type-">-Select Fuel type-</asp:ListItem>
                                                <asp:ListItem style="background-color:  blue !important; font-weight: bold;">PETROL/CNG</asp:ListItem>
                                                <asp:ListItem style="background-color: orange !important; font-weight: bold;">DIESEL</asp:ListItem>
                                                <asp:ListItem style="background-color: white !important; font-weight: bold;">HYBRID/OTHER</asp:ListItem>
                                                <%--<asp:ListItem>HYBRID/</asp:ListItem>
                                                <asp:ListItem>PETROL/HYBRID</asp:ListItem>
                                                <asp:ListItem>Electricity</asp:ListItem>--%>
                                            </asp:DropDownList>
                                             </ContentTemplate>
                                                     </asp:UpdatePanel>
                                              <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                <ContentTemplate >
                                             <asp:DropDownList ID="ddlnew" runat="server" CssClass="form-control" Visible="false" 
                                                >
                                                <asp:ListItem Value="-Select Fuel Type-">-Select Fuel type-</asp:ListItem>
                                                <asp:ListItem style="background-color:  blue !important; font-weight: bold;">PETROL/CNG (Blue With Green Strip)</asp:ListItem>
                                                <asp:ListItem style="background-color: orange !important; font-weight: bold;">DIESEL (Orange With Green Strip)</asp:ListItem>
                                                <asp:ListItem style="background-color: white !important; font-weight: bold;">HYBRID/OTHER (Grey With Green Strip)</asp:ListItem>
                                                <%--<asp:ListItem>HYBRID/</asp:ListItem>
                                                <asp:ListItem>PETROL/HYBRID</asp:ListItem>
                                                <asp:ListItem>Electricity</asp:ListItem>--%>
                                            </asp:DropDownList> 
                                                     </ContentTemplate>
                                                     </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <br />
                            <div class="row"  runat="server" visible="false">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label17" Visible="true"> Model <span style="color: #FF3300"></span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                              <asp:TextBox ID="txtmodel" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label23" Visible="true"> Date of manufacture (MM/yyyy)<span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <table width="100%">
                                                 <tr>
                                                     <td>
                                                         <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="MM/yyyy"
                                                             ControlType="Picker" PickerCssClass="picker datepick" Visible="true">
                                                             <ClientEvents>
                                                                 <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                                             </ClientEvents>
                                                         </ComponentArt:Calendar>
                                                     </td>
                                                     <td>   
                                                         <img id="calendar_To_button" tabindex="3" alt="" onclick="HSRPAuthDate_OnClick()"
                                                             onmouseup="HSRPAuthDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                                     </td>
                                                 </tr>
                                             </table>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            
                            
                            <br />
                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label24" Visible="true"> HSRP FrontLaser/ SerialNo. <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                              <asp:TextBox ID="txtfronlaser" runat="server" class="form-control" MaxLength="20" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="HideFLaserTxt" />
                                        </div>
                                        <%--<div class="col-md-3 col-sm-3 col-lg-3">
                                            <asp:CheckBox ID="cbFronlaser" runat="server" Text="&nbsp;&nbsp;Not Valid" AutoPostBack="true" OnCheckedChanged="ChckedChanged" />  
                                        </div>--%>

                                    </div>
                                </div>
                                <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <div class="row">
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:Label runat="server" ID="Label25" Visible="true"> HSRP RearLaser/ SerialNo. <span style="color: #FF3300">*</span></asp:Label>
                                        </div>
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:TextBox ID="txtRearlaser" runat="server" class="form-control" MaxLength="20" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="HideRLaserTxt" />
                                        </div>
                                        <%--<div class="col-md-3 col-sm-3 col-lg-3">
                                            <asp:CheckBox ID="cbRearlaser" runat="server" Text="&nbsp;&nbsp;Not Valid" AutoPostBack="true" OnCheckedChanged="ChckedChanged" />  
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <br />

                            <div class="form-group" runat="server" visible="false" id="DivFrontRearUpload">


                                <div class="row">
                                    <div class="col-md-5 col-sm-5 col-lg-5">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-lg-4">
                                                <asp:Label runat="server" ID="Label1" Visible="true"> Upload Vehicle Registration Certificate<span style="color: #FF3300">*</span></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="Label2" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-lg-8">
                                                <asp:FileUpload runat="server" ID="RcUploader" />
                                                <asp:HiddenField runat="server" ID="HiddenRCPath" />
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                    <div class="col-md-5 col-sm-5 col-lg-5">
                                        <div class="row">
                                            <div class="col-md-3 col-sm-3 col-lg-3">
                                                <asp:Label runat="server" ID="Label4" Visible="true"> Upload Id Proof <span style="color: #FF3300">*</span></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="Label5" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-lg-4">
                                                <asp:DropDownList ID="ddlDocType" runat="server" CssClass="form-control"
                                                    AutoPostBack="True">
                                                    <asp:ListItem Value="-Select ID Proof-">-Select ID Proof-</asp:ListItem>
                                                    <asp:ListItem>Aadhar Card</asp:ListItem>
                                                    <asp:ListItem>PAN Card</asp:ListItem>
                                                    <asp:ListItem>Voter ID</asp:ListItem>
                                                    <asp:ListItem>Passport</asp:ListItem>
                                                    <asp:ListItem>Driving License</asp:ListItem>
                                                    <asp:ListItem>Ration Card</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2 col-sm-2 col-lg-2">
                                                <asp:FileUpload runat="server" ID="IDUploader" />
                                                <asp:HiddenField runat="server" ID="HiddenIDPath" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />

                                <div class="row">
                                    <div class="col-md-5 col-sm-5 col-lg-5">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-lg-4">
                                                <asp:Label runat="server" ID="Label8" Visible="true"> Front Laser Code Image <span style="color: #FF3300">*</span></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="Label9" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-lg-8">
                                                <asp:FileUpload runat="server" ID="FileFrontlaser" />
                                                <asp:HiddenField runat="server" ID="HiddenFlaser" />
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                    <div class="col-md-5 col-sm-5 col-lg-5">
                                        <div class="row">
                                            <div class="col-md-4 col-sm-4 col-lg-4">
                                                <asp:Label runat="server" ID="Label10" Visible="true"> Rear Laser Code Image <span style="color: #FF3300">*</span></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="Label11" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                            </div>
                                            <div class="col-md-8 col-sm-8 col-lg-8">
                                                <asp:FileUpload runat="server" ID="FileRearLaser" />
                                                <asp:HiddenField runat="server" ID="HiddenRearLaser" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <br />

                            </div>

                            <div class="form-group">
                                <div class="row">
                                    <%--<div class="col-md-5 col-sm-5 col-lg-5">
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6 col-lg-6">
                                                <asp:Label runat="server" ID="Label4" Visible="true"> Upload RC/RC Authorization Letter <span style="color: #FF3300">*</span></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="Label5" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                            </div>
                                            <div class="col-md-6 col-sm-6 col-lg-6">
                                                 
                                                <asp:FileUpload runat="server" ID="RcUploader" />
                                            </div>

                                        </div>
                                    </div>--%>
                                    <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                    <div class="col-md-5 col-sm-5 col-lg-5">
                                        <%--<div class="row">
                                            <div class="col-md-8 col-sm-8 col-lg-8">
                                                <asp:Label runat="server" ID="Label8" Visible="true"> Front Laser Code Image <span style="color: #FF3300">*</span></asp:Label>
                                                <br />
                                                <asp:Label runat="server" ID="Label9" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                            </div>
                                            <div class="col-md-4 col-sm-4 col-lg-4">
                                                <asp:FileUpload runat="server" ID="FileUpload1" />
                                            </div>
                                        </div>--%>
                                    </div>

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                    <%--<div class="row">
                                        <div class="col-md-8 col-sm-8 col-lg-8">
                                            <asp:Label runat="server" ID="Label6" Visible="true"> Upload Id Proof <span style="color: #FF3300">*</span></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="Label7" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                        </div>
                                        <div class="col-md-4 col-sm-4 col-lg-4">
                                            <asp:FileUpload runat="server" ID="IDUploader" />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12 col-sm-12 col-lg-12F">
                                            (Any One Aadhar Card,PAN Card,Voter ID,Passport,Driving License,Ration Card)
                                        </div>
                                    </div>--%>
                                </div>
                                    <div class="col-md-1 col-sm-1 col-lg-1"></div>
                                <div class="col-md-5 col-sm-5 col-lg-5">
                                  
                                </div>




                            </div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12 col-sm-12 col-lg-12" style="text-align: right">

                                       <%-- <a href="#" onclick="popupplate();">Click Here For HSRP Plate Description </a>--%>
                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-2 col-sm-2 col-lg-2">
                                        <asp:ImageButton ID="buttonSave" runat="server" Visible="false" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px;" OnClick="btnSave_Click" />
                                        <asp:ImageButton ID="buttonAgainSave" runat="server" Visible="false" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px;" OnClick="buttonAgainSave_Click" />
                                    </div>
                                </div>
                                
                            </div>
                        </div>
                </div>
            </div>
        </div>
    </div>
    <table width="99%" align="center" cellpadding="0" cellspacing="0" class="borderinner">
        <tr>
            <td colspan="6">
                <ComponentArt:Calendar runat="server" ID="CalendarOrderDate" AllowMultipleSelection="false"
                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                    PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                    <ClientEvents>
                        <SelectionChanged EventHandler="OrderDate_OnChange" />
                    </ClientEvents>
                </ComponentArt:Calendar>
            </td>

            <td colspan="7">
                <ComponentArt:Calendar runat="server" ID="CalendarHSRPAuthDate" AllowMultipleSelection="false"
                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                    PopUp="Custom" PopUpExpandControlId="calendar_To_button" CalendarTitleCssClass="title"
                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                    <ClientEvents>
                        <SelectionChanged EventHandler="HSRPAuthDate_OnChange" />
                    </ClientEvents>
                </ComponentArt:Calendar>
            </td>
        </tr>
    </table>
    </div>
    </div>
</asp:Content>
