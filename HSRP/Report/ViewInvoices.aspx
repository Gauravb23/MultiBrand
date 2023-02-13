<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ViewInvoices.aspx.cs" Inherits="HSRP.Report.ViewInvoices" %>
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
    <div class="container" style="width: 100%;margin-bottom:49px;">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <asp:ImageButton ID="ImageButton1" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="left" ImageUrl="../images/button/back.png" Style="height: 27px; width: 27px; margin-top: -4px;" OnClientClick="JavaScript: window.history.back(1); return false;" />&nbsp;&nbsp;View Invoice
                             <a href="../LiveReports/oemdashboard.aspx" style="float:right">
                                <img class="" src="../images/button/home.png" alt="logo" Style="height: 27px; width: 27px; margin-top: -4px;">
                            </a>
                        </div>
                        <div class="panel-body">
                            <div class="row" style="display:none;">
                                <div class="col-md-1 col-lg-1 col-sm-1"></div>
                                <div class="col-md-1 col-lg-1 col-sm-1">
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
                                <div class="col-md-1 col-lg-1 col-sm-1">
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
                            </div>
                            <div class="row">
                                <div class="col-md-3 col-lg-3 col-sm-3"></div>
                                <div class="col-md-3 col-lg-3 col-sm-3">
                                     <%--<asp:ImageButton ID="btnGO" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/btngo.jpg" Style="height: 24px; width: 69px;" OnClick="btnGO_Click" OnClientClick="return validate()" />--%>
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
                            <div class="row" style="overflow-x: auto; font-size: 14px;">
                                <asp:GridView ID="grddealers" Visible="true" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                    AllowPaging="true" PageSize="6000" align="center" CellPadding="2" ForeColor="#333333" GridLines="Both" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Width="100%" >
                                    <AlternatingRowStyle BackColor="White" />
                                    <FooterStyle BackColor="White" ForeColor="#000066" />
                                    <PagerSettings Visible="False" Mode="NextPreviousFirstLast" PageButtonCount="20" />
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
                                                Sno
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text='<%#Eval("Sno") %>'
                                                    Enabled="false"></asp:Label>
                                                <asp:Label ID="lblsalesinvoiceid" runat="server" Text='<%#Eval("Salesinvoiceid") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblapprovedstatus" runat="server" Text='<%#Eval("ApprovedStatus") %>'
                                                    Visible="false"></asp:Label>
                                                <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("ImageParam") %>'
                                                    Visible="false"></asp:Label>
                                                
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Invoice No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#Eval("InvoiceNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Invoice Date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblinvoicedate" runat="server" Text='<%#Eval("invoicedate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                               Invoice Period
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDealername" runat="server" Text='<%#Eval("InvoicePeriod") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                               Orders
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrders" runat="server" Text='<%#Eval("Orders") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Rate
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblRate" runat="server" Text='<%#Eval("Rate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                               BasicAmount
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBasicAmount" runat="server" Text='<%#Eval("BasicAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                CGST Amount
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblcgstamount" runat="server" Text='<%#Eval("CGSTamount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                SGST Amount
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblsgstamount" runat="server" Text='<%#Eval("SGSTamount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                IGST Amount
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbligstamount" runat="server" Text='<%#Eval("IGSTamount") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField>
                                            <HeaderTemplate>
                                                Total Amount
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbltotalamount" runat="server" Text='<%#Eval("Total") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField>
                                            <HeaderTemplate>
                                                Excel
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkexcel" runat="server" Text="Excel" OnClick="lnkexcel_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Download Invoice
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                 <asp:LinkButton ID="lnkPreview" runat="server" OnClick="lnkPreview_Click" Text="DownLoad" CommandArgument='<%# PreviewImage(Eval("ImageParam").ToString()) %>'></asp:LinkButton>
                                                <%--<asp:linkbutton id="LinkButtonDownloadPdf" runat="server" text="Download" style="color: Navy;
            font-weight: bold;" onclick="LinkButtonDownloadPdf_Click" />--%>
                                               <%-- <a href=http://103.197.122.35/OemInvoice/<%#Eval("FileName") %> target="_blank" download="<%#Eval("FileName") %>">Download</a>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div align="center">No records found.</div>
                                    </EmptyDataTemplate>
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

