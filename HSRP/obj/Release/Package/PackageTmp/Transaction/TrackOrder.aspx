<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TrackOrder.aspx.cs" Inherits="HSRP.Transaction.TrackOrder" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/Bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../css/Bootstrap/jquery.min.js" type="text/javascript"></script>
    <script src="../css/Bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/Bootstrap/fontawesome.css" rel="stylesheet" />
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
    <script>
        function DispatchDetailModal(i) {
            googlewin = dhtmlwindow.open("googlebox", "iframe", "DispatchDetails.aspx?id=" + i, "" + i + "", "width=500px,height=250px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                //window.location.href = "AssignLaserCode.aspx";
                return true;
            }
        }
    </script>
    <div class="container" style="width: 100%">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="../images/button/back.png" Style="height: 27px; width: 27px; margin-top: -4px;" OnClientClick="JavaScript: window.history.back(1); return false;" />&nbsp;&nbsp;Track  Orders
                             <a href="../LiveReports/LiveTracking.aspx" style="float: right">
                                 <img class="" src="../images/button/home.png" alt="logo" style="height: 27px; width: 27px; margin-top: -4px;">
                             </a>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:Label Text=" From Date:" runat="server" ID="labelDate" Visible="true" />
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" onmouseup="OrderDate_OnMouseUp()">
                                    <ComponentArt:Calendar ID="OrderDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="OrderDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="OrderDate_OnClick()"
                                        onmouseup="OrderDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:Label Text=" To Date:" runat="server" ID="label1" Visible="true" />
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2" onmouseup="HSRPAuthDate_OnMouseUp()">
                                    <ComponentArt:Calendar ID="HSRPAuthDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="HSRPAuthDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                    <img id="calendar_To_button" tabindex="3" alt="" onclick="HSRPAuthDate_OnClick()"
                                        onmouseup="HSRPAuthDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-3">
                                    <%-- <asp:Button ID="btnGO" Width="58px" runat="server" Visible="true" Text="GO" ToolTip="Please Click for Report"
                                        class="button" OnClick="btnGO_Click" />
                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnExportExcel" class="button" Visible="false" runat="server" ToolTip="Download for Grid Report" Text="Download" OnClick="btnExportExcel_Click" />--%>

                                    <asp:ImageButton ID="btnExportExcel" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/ExcelDownload.jpg" Style="height: 24px; width: 69px;" OnClick="btnExportExcel_Click" OnClientClick="return validate()" />
                                    &nbsp;&nbsp;&nbsp;
                                   <%-- <asp:ImageButton ID="ImageButton2" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/PdfDownload.jpg" Style="height: 24px; width: 69px;" OnClick="btnExportExcel_Click" OnClientClick="return validate()" />
                                    &nbsp;&nbsp;&nbsp;--%>
                                    <asp:ImageButton ID="btnGO" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px;" OnClick="btnGO_Click" OnClientClick="return validate()" />
                                </div>
                            </div>
                            <div class="row">
                                <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300" />
                                <asp:Label ID="lblSucMess" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="#FF3300"></asp:Label>
                                <asp:Label ID="llbMSGSuccess" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:Label ID="llbMSGError0" runat="server" ForeColor="Red"></asp:Label>
                                <br />
                                <asp:Label ID="lblVehicleRegNo" runat="server" Font-Bold="True"
                                    ForeColor="Blue" Text="VehicleRegNo=" Visible="False"></asp:Label>
                            </div>
                            <div class="row" style="overflow-x: auto; font-size: 12px;">
                                <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="Both" BorderColor="#CCCCCC" BorderStyle="None"
                                    BorderWidth="1px" Width="100%" AutoGenerateColumns="false" OnRowDataBound="grdid_RowDataBound" OnRowCommand="grdid_RowCommand">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Sr. No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Vehicle Reg No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblembname" runat="server" Text='<%#Eval("vehicleregno") %>'></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Vehicle Type
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblvehicletype" runat="server" Text='<%#Eval("VehicleType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Vehicle Class
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="VehicleClass" runat="server" Text='<%#Eval("VehicleClass") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Chassis No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ChassisNo" runat="server" Text='<%#Eval("ChassisNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Engione No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="EngineNo" runat="server" Text='<%#Eval("EngineNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Order Type
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="OrderType" runat="server" Text='<%#Eval("OrderType") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Front Laser
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="frontlaser" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Rear Laser
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="rearlaser" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                ProductionStatus
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="Productionstatus" runat="server" Text='<%#Eval("Productionstatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Dispatch Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%--<asp:LinkButton ID="lnkDispatchStatus" runat="server" OnClientClick=<%# "DispatchDetailModal('" + Eval("hsrprecordid") + "')" %> Text='<%#Eval("DispatchStatus") %>'></asp:LinkButton>--%>
                                                <asp:Label ID="DispatchStatus" runat="server" Text='<%#Eval("DispatchStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                Dispatch Date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbldispatch" runat="server" Text='<%#Eval("DispatchDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Receiving Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ReceivingStatus" runat="server" Text='<%#Eval("ReceivingStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Affixation Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="AffixationStatus" runat="server" Text='<%#Eval("AffixationStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                           <asp:TemplateField>
                                            <HeaderTemplate>
                                               Vahan Sync Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="ParivahanAPI" runat="server" Text='<%#Eval("ParivahanAPIStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField>
                                            <HeaderTemplate>
                                               API Response Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="APIResponse" runat="server" Text='<%#Eval("APIResponse") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Courier Name
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCourier" runat="server" Text='<%#Eval("CourierName") %>'></asp:Label>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                               AWB No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAWBNo" CommandName="AWBNo" runat="server" Visible="false" Text='<%#Eval("AWBNo") %>'>LinkButton</asp:Label>  
                                                <asp:LinkButton ID="linkAWBNo" CommandName="AWBNo" runat="server" Visible="false" Text='<%#Eval("AWBNo") %>'>LinkButton</asp:LinkButton>                                               
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
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
</asp:Content>
