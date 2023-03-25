<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DLViewDealerOrder.aspx.cs" Inherits="HSRP.Transaction.DLViewDealerOrder" EnableEventValidation="false" %>

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

    <div class="container" style="width: 100%">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                          <div class="clearfix">
                                <h4 style="float:left;"> View Orders</h4>
                            <asp:Button ID="btnback" runat="server" Text="Home" style="float:right;" CssClass="btn btn-primary" OnClick="btnback_Click"  />     
                            </div>
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
                                    <img id="calendar_from_button" tabindex="1" alt="" onclick="OrderDate_OnClick()"
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
                                    <img id="calendar_To_button" tabindex="2" alt="" onclick="HSRPAuthDate_OnClick()"
                                        onmouseup="HSRPAuthDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:Button ID="btnGo2" runat="server" Visible="true" Text="View" ToolTip="Please Click for Report" CssClass="btn btn-primary" OnClick="btnGO2_Click" />
                                    &nbsp;&nbsp;&nbsp;<asp:Button ID="btnExcel" runat="server" Visible="true" Text="Download" ToolTip="Download for Grid Report" CssClass="btn btn-secondary" OnClick="btnExcel_Click" />
                                </div>

                            </div>
                            <div class="row" style="margin-left:3px;">                                
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red"></asp:Label>                               
                            </div>
                            <div class="row" style="overflow-x:auto;font-size: 12px;">
                                <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="Both" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Width="100%" AutoGenerateColumns="false">
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
