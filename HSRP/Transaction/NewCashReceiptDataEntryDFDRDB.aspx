<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="NewCashReceiptDataEntryDFDRDB.aspx.cs" Inherits="HSRP.Transaction.NewCashReceiptDataEntryDFDRDB" %>

<%@ Register Assembly="ComponentArt.Web.UI" Namespace="ComponentArt.Web.UI" TagPrefix="ComponentArt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    
    
    <script type="text/javascript" src="../Scripts/jquery.min.js"></script>



      <script type="text/javascript" language="javascript">
          function doClick(btnSave2, e) {
              //the purpose of this function is to allow the enter key to 
              //point to the correct button to click.
              var key;

              if (window.event)
                  key = window.event.keyCode;     //IE
              else
                  key = e.which;     //firefox

              if (key == 13) {
                  //Get the button the user wants to have clicked
                  var btn = document.getElementById(btnSave2);
                  if (btn != null) { //If we find the button click it
                      btn.click();
                      event.keyCode = 0
                  }
              }
          }
      </script>


    <script type="text/javascript" language="javascript">
        function doClick(btnGO2, e) {
            //the purpose of this function is to allow the enter key to 
            //point to the correct button to click.
            var key;

            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13) {
                //Get the button the user wants to have clicked
                var btn = document.getElementById(btnGO2);
                if (btn != null) { //If we find the button click it
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>


    <script type="text/javascript" language="javascript">
        function specialcharecter() {
            var iChars = "!`@#$%^&*()';,.";
            var txtVehicleNo = document.getElementById("<%=txtRegNumber.ClientID%>").value;
        for (var i = 0; i < txtVehicleNo.length; i++) {
            if (iChars.indexOf(txtVehicleNo.charAt(i)) != -1) {
                alert("Special characters in Registration No are not allowed.");
                    document.getElementById("<%=txtRegNumber.ClientID%>").value = "";
                    return false;
                }
            }
        }
    </script>


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
                     var oemid = '<%=Session["oemid"]%>';
            if (oemid == '4') {
                window.location.href = "UploadDealerdata_ford.aspx";
            } else {
                window.location.href = "UploadDealerdata.aspx";
            }


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
            width: 100%;
        }

        .bookingform {
            margin-bottom: 10px;
        }

        .oprion {
            /* Whatever color  you want */
            background-color: yellow;
        }

    </style>
<style type="text/css">

    .modalBackground {
        background-color: Black;
        filter: alpha(opacity=90);
        opacity: 0.8;
    }

    .modalPopup {
        background-color: #FFFFFF;
        border-width: 3px;
        border-style: solid;
        border-color: black;
        padding-top: 10px;
        padding-left: 10px;
        width: 300px;
        height: 140px;
    }
</style>


     <div class="container">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="../images/button/back.png" Style="height: 27px; width: 27px; margin-top: -4px;" OnClientClick="JavaScript: window.history.back(1); return false;" />&nbsp;&nbsp;
                            <asp:Label runat="server" Style="margin-left: 10px; height: 75px; margin-top: 50px; font-size: medium; color: Black;" Text="  HSRP Booking Form"></asp:Label>
                            <a href="../LiveReports/LiveTracking.aspx" style="float: right">
                                <img class="" src="../images/button/home.png" alt="logo" style="height: 27px; width: 27px; margin-top: -4px;">
                            </a>
                           

                        </div>
                        <div class="panel-body">
                            <div class="row bookingform">
                                <div class="col-md-10 col-lg-10 col-sm-10"></div>
                                <div class="col-md-2 col-lg-2 col-sm-2" runat="server" visible="false" style="border: 1px ridge; background: #0d0076 border-box; color: white;">Available Balance:&nbsp;&nbsp;&nbsp;<asp:Label ID="lblavailbal" runat="server" Font-Bold="true"></asp:Label></div>

                            </div>
                            <div class="row bookingform">
                                <div class="col-md-2 col-lg-2 col-sm-2">Registration No <span style="color: #FF3300">*</span></div>
                                <div class="col-md-2 col-lg-2 col-sm-2">                                    
                                    <asp:TextBox ID="txtRegNumber" onblur="specialcharecter()" class="form-control" runat="server" Style="text-transform: uppercase" MaxLength="10" autocomplete="off" OnTextChanged="txtRegNumber_TextChanged" AutoPostBack="True"></asp:TextBox>
                                      <asp:AutoCompleteExtender ServiceMethod="GetCompletionList" MinimumPrefixLength="1"
                                        CompletionInterval="10" EnableCaching="false" CompletionSetCount="1" TargetControlID="txtRegNumber"
                                        ID="AutoCompleteExtender1" runat="server" FirstRowSelected="false">
                                    </asp:AutoCompleteExtender>

                                    <asp:Label ID="lblauthno" Visible="false" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" id="divvehclass" Visible="false" runat="server">
                                    Vehicle Class <span style="color: #FF3300">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divvehclassddl" Visible="false" runat="server">                                  
                                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlVehicletype" EventName="SelectedIndexChanged" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlVehicleclass" runat="server" CssClass="form-control">
                                                <asp:ListItem Value="-Select Vehicle Class-">-Select Vehicle Class-</asp:ListItem>
                                                <asp:ListItem style="background-color: yellow !important; font-weight: bold;">Transport</asp:ListItem>
                                                <asp:ListItem>Non-Transport</asp:ListItem>
                                                <asp:ListItem>Rent A CAB</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                 <div class="col-md-2 col-sm-2 col-lg-2" id="divregdate" Visible="false" runat="server">                                   
                                    Date of Registration <span style="color: #FF3300">*</span>                                 
                                </div>
                                
                                 <div class="col-md-2 col-sm-2 col-lg-2" id="divregdate2" Visible="false" runat="server">                                   
                                   <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy" ControlType="Picker"
                                       PickerCssClass="picker datepick" Visible="true" >
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                    <img id="calendar_from_button" runat="server" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" Visible="false" />                                                                    
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="ddlVehicleclass" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Label ID="lblAmount" Text="" runat="server" Visible="false"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                          
                          <div class="row bookingform">
                               <div class="col-md-2 col-lg-2 col-sm-2">
                                    Chassis No <span style="color: #FF3300">*</span>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:TextBox ID="txtChassisno" runat="server" class="form-control" MaxLength="35" autocomplete="off" ></asp:TextBox>
                                </div>
                               
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divfuel" Visible="false" runat="server">
                                    Fuel Type <span style="color: #FF3300">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divfuel2" Visible="false" runat="server">                                    
                                     <asp:DropDownList ID="ddlFuelType" runat="server" CssClass="form-control"
                                        AutoPostBack="false">
                                        <asp:ListItem Value="-Select Fuel Type-">-Select Fuel type-</asp:ListItem>
                                        <asp:ListItem>PETROL</asp:ListItem>
                                        <asp:ListItem>CNG/PETROL</asp:ListItem>
                                        <asp:ListItem>DIESEL</asp:ListItem>
                                        <asp:ListItem>DIESEL/HYBRID</asp:ListItem>
                                        <asp:ListItem>PETROL/HYBRID</asp:ListItem>
                                        <asp:ListItem>Electricity</asp:ListItem>
                                        <asp:ListItem>LPG/PETROL</asp:ListItem>
                                        <asp:ListItem>CNG</asp:ListItem>
                                    </asp:DropDownList>

                                </div>
                               
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divvehstage" Visible="false" runat="server">
                                   Vehicle Stage Type <span style="color: #FF3300">*</span>
                                </div>

                                <div class="col-md-2 col-sm-2 col-lg-2" id="divvehstage2" Visible="false" runat="server">
                                     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlVehicleStateType" runat="server" class="form-control"  Enabled="true" >
                                                        <asp:ListItem>-Select Vehicle Stage Type-</asp:ListItem>
                                                        <asp:ListItem Value="BS4">BS4 or Other</asp:ListItem>
                                                         <asp:ListItem Value="BS6">BS6</asp:ListItem>
                                                         <asp:ListItem Value="BS3">BS3</asp:ListItem>
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-lg-2 col-sm-2">Engine No <span style="color: #FF3300">*</span></div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                     <asp:TextBox ID="txtEngineNo" runat="server" class="form-control" MaxLength="35" autocomplete="off"></asp:TextBox>
                                </div>
                              
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divvehtype" Visible="false" runat="server">
                                    Vehicle Type <span style="color: #FF3300">*</span>
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divvehtype2" Visible="false" runat="server">
                                    <asp:DropDownList ID="ddlVehicletype" runat="server" class="form-control" DataValueField="VehicleType" DataTextField="Vehicletypenew" OnSelectedIndexChanged="ddlVehicletype_SelectedIndexChanged" AutoPostBack="True">
                                     <asp:ListItem>-Select Vehicle Type-</asp:ListItem>
                                    </asp:DropDownList>  
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divmodel" Visible="false" runat="server">
                                    Order Type <span style="color: #FF3300">*</span>
                                     <%--Model <span style="color: #FF3300">*</span>--%>
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2" id="divmodel2" visible="false" runat="server">
                                     <asp:DropDownList ID="ddlOrderType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlOrderType_SelectedIndexChanged"
                                        AutoPostBack="true">
                                         <asp:ListItem Value="0">-- Select Order Type -- </asp:ListItem>
                                  <asp:ListItem Value="DB">Both Damage Plates (Front and Rear)</asp:ListItem>
                                  <asp:ListItem Value="DF">Front Damage Plate</asp:ListItem>
                                  <asp:ListItem Value="DR">Rear Damage Plate</asp:ListItem>
                                         </asp:DropDownList>
                                    <asp:TextBox ID="txtmodel" Visible="false" runat="server" class="form-control" autocomplete="off"></asp:TextBox> 
                                </div>

                            </div>

                          <div class="row" style="margin-top:10px;">
                                <div class="col-md-2 col-sm-2 col-lg-2">
                                    <asp:Label runat="server" ID="lbldom" Visible="false"> Date of manufacture  <span style="color: #FF3300">*</span></asp:Label>
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2">
                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="MM/yyyy"
                                        ControlType="Picker" PickerCssClass="picker datepick" Visible="false">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                   <%-- <img id="calendar_To_button" tabindex="3" alt="" onclick="HSRPAuthDate_OnClick()"
                                        onmouseup="HSRPAuthDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />--%>
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2">                                  
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2">                                                                                                                       
                                </div>                              
                            </div>
                            <div class="row">
                                <div class="col-md-2 col-sm-2 col-lg-2">                                                                                                                       
                                </div>
                                <div class="col-md-2 col-sm-2 col-lg-2">
                                     <%--<asp:ImageButton ID="btnGO" runat="server" AlternateText="ImageButton 1" TabIndex="4" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px; position:relative; top: 0px; left: 1px;" OnClick="btnGO_Click" OnClientClick="return validate()"  />--%>
                                    <asp:Button Text="Go" ID="btnGO2" CssClass="btn btn-primary" runat="server"  OnClick="btnGO2_Click" OnClientClick="return validate()" />

                                </div>
                            </div>
                                  <hr id="hr1" runat="server" visible="false" />          
                             <div class="row" visible="false" id="divfitment" runat="server">
                                <div class="col-md-4 col-sm-4 col-lg-4">
                               <b><u>Fitment Location Details</u></b> 
                                    </div>
                            </div>
                            <div class="row" id="divdevtype" runat="server" Visible="false" style="margin-top:10px;">
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                     Fitment Address<span style="color: #FF3300">*</span>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                     <asp:DropDownList ID="ddlAffixationType" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlAffixationType_SelectedIndexChanged">
                                        <asp:ListItem Value="0">- Select Type -</asp:ListItem>
                                        <asp:ListItem Value="1">At Your Location</asp:ListItem>
                                        <asp:ListItem Value="2">At Home</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2 ">                                    
                                   <asp:Label ID="lblstate" runat="server" Visible="false">State<span style="color: #FF3300">*</span></asp:Label>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" >                                   
                                     <asp:DropDownList ID="ddlstate" runat="server" CssClass="form-control" Visible="false" DataValueField="HSRP_StateID"
                                         DataTextField="HSRPStateName" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged" AutoPostBack="true">
                                         <asp:ListItem Value="0">-- Select State</asp:ListItem>
                                     </asp:DropDownList>
                                </div>  
                                <div class="col-md-2 col-lg-2 col-sm-2" >
                                    <asp:Label ID="lblLocationAddress" runat="server" Visible="false">Fitment Location<span style="color: #FF3300">*</span></asp:Label>
                                    <asp:Label ID="lbldistrict" runat="server" Visible="false">District<span style="color: #FF3300">*</span></asp:Label>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" >
                                    <asp:DropDownList ID="ddlLocationAddress" runat="server" CssClass="form-control" Visible="false">
                                        <asp:ListItem Value="0">-- Select Fitment Location --</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:DropDownList ID="ddldistrict" runat="server" Visible="false" CssClass="form-control" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                        <asp:ListItem Value="0">-- Select District --</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row" style="margin-top:10px;">
                                <div class="col-md-2 col-lg-2 col-sm-2" >
                                     <asp:Label ID="lblHomeAddress" runat="server" Visible="false">Home Location Address<span style="color: #FF3300">*</span></asp:Label>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" >
                                    <asp:TextBox ID="txtHomeAddress" runat="server" class="form-control" Visible="false"></asp:TextBox>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" >
                                    <asp:Label ID="lblpincode" runat="server" Visible="false">Pincode<span style="color: #FF3300">*</span></asp:Label>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" >
                                    <asp:TextBox ID="txtpincode" runat="server" Visible="false" MaxLength="6" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                            <div class="row justify-content-center" style="margin-top:12px;">
                                <div style="text-align: center;">
                                 <asp:Button ID="btnAdd2" runat="server" Text="Add Location" CssClass="btn btn-primary" OnClick="btnAdd2_Click" Visible="false" />
                                </div>   
                            </div>
                            <hr id="hr2" runat="server" visible="false"/>
                            <div class="row" id="divdocument" runat="server" visible="false">
                                <div class="col-md-4 col-sm-4 col-lg-4">
                               <b><u>Documents Upload</u></b> 
                                    </div>
                            </div>
                            <div class="row" style="margin-top:10px;" visible="false" id="divdocument2" runat="server">
                                <div class="col-md-3 col-sm-3 col-lg-3">
                                    <asp:Label runat="server" ID="lblRC" Visible="true"> Upload Vehicle Registration Certificate<span style="color: #FF3300">*</span></asp:Label>
                                      <br />
                                    <asp:Label runat="server" ID="lblRCmax" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-3 col-lg-3">
                                     <asp:FileUpload runat="server" CssClass="form-control" ID="RcUploader" />
                                     <asp:HiddenField runat="server" ID="HiddenRCPath" />
                                </div>
                                 <div class="col-md-3 col-sm-3 col-lg-3">
                                    <asp:Label runat="server" ID="lblFIR" Visible="true"> Upload FIR<span style="color: #FF3300">*</span></asp:Label>
                                      <br />
                                    <asp:Label runat="server" ID="lblFIRmax" ForeColor="Red" Visible="true"> Max File Size 200Kb</asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-3 col-lg-3">
                                     <asp:FileUpload runat="server" CssClass="form-control" ID="FIRUploader" />
                                     <asp:HiddenField runat="server" ID="HiddenFIR" />
                                </div>
                            </div>

                            <div class="row" style="margin-top:10px;" visible="false" id="divdocument3" runat="server">
                                 <div class="col-md-3 col-sm-3 col-lg-3" id="divflaser" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblfront"> Front Laser Code Image <span style="color: #FF3300">*</span></asp:Label>
                                      <br />
                                    <asp:Label runat="server" ID="lblfrontmax" ForeColor="Red"> Max File Size 200Kb</asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-3 col-lg-3" id="divflaser2" runat="server" visible="false">
                                    <asp:FileUpload runat="server" CssClass="form-control" ID="FileFrontlaser" />
                                    <asp:HiddenField runat="server" ID="HiddenFlaser" />
                                </div>
                                <div class="col-md-3 col-sm-3 col-lg-3" id="divrlaser" runat="server" visible="false">
                                    <asp:Label runat="server" ID="lblrear"> Rear Laser Code Image <span style="color: #FF3300">*</span></asp:Label>
                                      <br />
                                    <asp:Label runat="server" ID="lblrearmax" ForeColor="Red"> Max File Size 200Kb</asp:Label>
                                </div>
                                <div class="col-md-3 col-sm-3 col-lg-3" id="divrlaser2" runat="server" visible="false">
                                    <asp:FileUpload runat="server" CssClass="form-control" ID="FileRearLaser" />
                                    <asp:HiddenField runat="server" ID="HiddenRearLaser" />
                                </div>
                            </div>
                            <div class="row justify-content-center" style="margin-top:12px;" >
                              
                                <div style="text-align: center;">
                                    <%--<asp:ImageButton ID="btnAdd" Visible="true" runat="server" AlternateText="Add" TabIndex="2" ImageAlign="Right" ImageUrl="../images/Button/AddLocation.jpg" Style="height: 24px; width: 69px;" OnClick="btnAdd_Click" />--%>
                                    <%--<asp:ImageButton ID="buttonSave" Visible="false" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/save.jpg" Style="height: 24px; width: 69px;" OnClick="btnSave_Click" OnClientClick="return validate()"  />--%>
                                    <asp:Button ID="btnSave2" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave2_Click" OnClientClick="return validate()" Visible="false" />
                                   
                                    <asp:Button ID="btnPrint" runat="server" Text="Print Receipt" CssClass="btn btn-primary" OnClick="btnPrint_Click" Visible="false" />
                                </div>
                            </div>
                            <div class="row" style="margin-left:5px;">
                                <asp:Label ID="lblSucMess" runat="server" Font-Size="18px" ForeColor="Blue"></asp:Label>
                                <asp:Label ID="lblErrMess" runat="server" Font-Size="18px" ForeColor="Red"></asp:Label>
                                <asp:Label ID="lblVehicleType" runat="server" Text="Label" Visible="false"></asp:Label>
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
                    PopUp="Custom" PopUpExpandControlId="calendar_To_button" CalendarTitleCssClass="title"
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

</asp:Content>
