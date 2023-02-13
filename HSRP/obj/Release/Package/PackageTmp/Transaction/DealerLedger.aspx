<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DealerLedger.aspx.cs" Inherits="HSRP.Transaction.DealerLedger" EnableEventValidation="false"%>
<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/Bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../css/Bootstrap/jquery.min.js" type="text/javascript"></script>
    <script src="../css/Bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/Bootstrap/fontawesome.css" rel="stylesheet" />
    <script type="text/javascript">
        function StartDate_OnDateChange(sender, eventArgs) {
            var fromDate = StartDate.getSelectedDate();
            CalendarStartDate.setSelectedDate(fromDate);
        }

        function StartDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarStartDate.getSelectedDate();
            StartDate.setSelectedDate(fromDate);

        }

        function StartDate_OnClick() {
            if (CalendarStartDate.get_popUpShowing()) {
                CalendarStartDate.hide();
            }
            else {
                CalendarStartDate.setSelectedDate(StartDate.getSelectedDate());
                CalendarStartDate.show();
            }
        }

        function StartDate_OnMouseUp() {
            if (CalendarStartDate.get_popUpShowing()) {
                event.cancelBubble = true;
                event.returnValue = false;
                return false;
            }
            else {
                return true;
            }
        }

        ////>>>>>> Pollution Due Date

        function EndDate_OnDateChange(sender, eventArgs) {
            var fromDate = EndDate.getSelectedDate();
            CalendarEndDate.setSelectedDate(fromDate);

        }

        function EndDate_OnChange(sender, eventArgs) {
            var fromDate = CalendarEndDate.getSelectedDate();
            EndDate.setSelectedDate(fromDate);

        }

        function EndDate_OnClick() {
            if (CalendarEndDate.get_popUpShowing()) {
                CalendarEndDate.hide();
            }
            else {
                CalendarEndDate.setSelectedDate(EndDate.getSelectedDate());
                CalendarEndDate.show();
            }
        }

        function EndDate_OnMouseUp() {
            if (CalendarEndDate.get_popUpShowing()) {
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
                             
                            <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="../images/button/back.png" Style="height: 27px; width: 27px;margin-top:-4px;" OnClientClick="JavaScript: window.history.back(1); return false;" />&nbsp;&nbsp;
                            Dealer Ledger
                             <a href="../LiveReports/LiveTracking.aspx" style="float:right">
                                <img class="" src="../images/button/home.png" alt="logo" Style="height: 27px; width: 27px; margin-top: -4px;">
                            </a>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label Text="Start Date:" runat="server" ID="labelDate" Visible="true" />
                                </div>
                                <div class="col-md-2" onmouseup="StartDate_OnMouseUp()">
                                    <ComponentArt:Calendar ID="StartDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true" Width="78px">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="StartDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                    <img id="calendar_from_button" tabindex="3" alt="" onclick="StartDate_OnClick()"
                                        onmouseup="StartDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Label Text="End Date:" runat="server" ID="label1" Visible="true" />
                                </div>
                                <div class="col-md-2" onmouseup="EndDate_OnMouseUp()">
                                    <ComponentArt:Calendar ID="EndDate" runat="server" PickerFormat="Custom" PickerCustomFormat="dd/MM/yyyy"
                                        ControlType="Picker" PickerCssClass="picker" Visible="true">
                                        <ClientEvents>
                                            <SelectionChanged EventHandler="EndDate_OnDateChange" />
                                        </ClientEvents>
                                    </ComponentArt:Calendar>
                                        <img id="calendar_from_button1" tabindex="3" alt="" onclick="EndDate_OnClick()"
                                        onmouseup="EndDate_OnMouseUp()" class="calendar_button" src="../images/btn_calendar.gif" />
                                </div>
                                <div class="col-md-2">
                                   
                                    <%--<asp:Button ID="btnGO" Width="58px" runat="server" Visible="true" Text="GO" ToolTip="Please Click for Report"
                                        class="button" OnClick="btnGO_Click" />--%>
                                    <%--  OnClientClick=" return validate()"--%>
                                    <asp:ImageButton ID="btnExportExcel" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/Download.jpg" Style="height: 24px; width: 69px;" OnClick="btnExportExcel_Click" OnClientClick="return validate()" />
                                    &nbsp;&nbsp;&nbsp;
                                     <asp:ImageButton ID="btnGO" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px;" OnClick="btnGO_Click" OnClientClick="return validate()" />
                                    <%--<asp:Button ID="btnExportExcel" class="button" Visible="false" runat="server" ToolTip="Download for Grid Report" Text="Download" OnClick="btnExportExcel_Click" />--%>
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
                            </div>
                            <div class="row">
                                <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" ForeColor="#333333" GridLines="Both" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" AutoGenerateColumns="true">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                    </Columns>

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
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0" class="midtable">
        <tr>
            <td>
                <ComponentArt:Calendar runat="server" ID="CalendarStartDate" AllowMultipleSelection="false"
                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                    PopUp="Custom" PopUpExpandControlId="calendar_from_button" CalendarTitleCssClass="title"
                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                    <ClientEvents>
                        <SelectionChanged EventHandler="StartDate_OnChange" />
                    </ClientEvents>
                </ComponentArt:Calendar>
            </td>
            <td>
                <ComponentArt:Calendar runat="server" ID="CalendarEndDate" AllowMultipleSelection="false"
                    AllowWeekSelection="false" AllowMonthSelection="false" ControlType="Calendar"
                    PopUp="Custom" PopUpExpandControlId="calendar_from_button1" CalendarTitleCssClass="title"
                    DayHoverCssClass="dayhover" DisabledDayCssClass="disabledday" DisabledDayHoverCssClass="disabledday"
                    OtherMonthDayCssClass="othermonthday" DayHeaderCssClass="dayheader" DayCssClass="day"
                    SelectedDayCssClass="selectedday" CalendarCssClass="calendar" NextPrevCssClass="nextprev"
                    MonthCssClass="month" SwapSlide="Linear" SwapDuration="300" DayNameFormat="FirstTwoLetters"
                    ImagesBaseUrl="../images" PrevImageUrl="cal_prevMonth.gif" NextImageUrl="cal_nextMonth.gif">
                    <ClientEvents>
                        <SelectionChanged EventHandler="EndDate_OnChange" />
                    </ClientEvents>
                </ComponentArt:Calendar>
            </td>
        </tr>

    </table>


</asp:Content>
