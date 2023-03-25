<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="LaserReceivedDealerwise.aspx.cs" Inherits="HSRP.Transaction.LaserReceivedDealerwise" EnableEventValidation="false" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/Bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../css/Bootstrap/jquery.min.js" type="text/javascript"></script>
    <script src="../css/Bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/Bootstrap/fontawesome.css" rel="stylesheet" />

  
    <script type="text/javascript">
        function checkreceive(id) {
            debugger;
            var row = id.parentNode.parentNode.rowIndex;
            var gv = document.getElementById('<%=GridView1.ClientID %>');
                var tags = gv.rows[row].cells[1].getElementsByTagName("input");
                for (var i = 0; i < tags.length; i++) {
                    if (tags[i].type == "checkbox") {
                        if ($(id).prop("checked") == true) {
                            var rejid = tags[i].id;
                            var chk = document.getElementById(rejid);
                            chk.disabled = true;
                            //$('#' + rejid).css("display","none");
                        }
                        else {
                            var rejid = tags[i].id;
                            var chk = document.getElementById(rejid);
                            chk.disabled = false;
                            //$('#' + rejid).css("display","");
                        }
                    }
                }
        }
            function checkreject(id, row) {
                debugger;
                var row = id.parentNode.parentNode.rowIndex;
                var gv = document.getElementById('<%=GridView1.ClientID %>');
                var tags = gv.rows[row].cells[0].getElementsByTagName("input");
                for (var i = 0; i < tags.length; i++) {
                    if (tags[i].type == "checkbox") {
                        if ($(id).prop("checked") == true) {
                            var rejid = tags[i].id;
                            var chk = document.getElementById(rejid);
                            chk.disabled = true;
                            //$('#' + rejid).css("display","none");
                        }
                        else {
                            var rejid = tags[i].id;
                            var chk = document.getElementById(rejid);
                            chk.disabled = false;
                            //$('#' + rejid).css("display","");
                        }
                    }
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
                                <h4 style="float:left;"> HSRP Plate Received At Affixation Center </h4>
                              <asp:Button ID="btnback" runat="server" Text="Home" style="float:right;" CssClass="btn btn-primary" OnClick="btnback_Click"  />     
                            </div>
                           
                        </div>
                        <div class="panel-body">
                            <div class="row" id="TR2" runat="server">
                            </div>
                            <div class="row" id="TRRTOHide" runat="server">
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label Text="Challan No:" runat="server"
                                        ID="labelOrderStatus" />
                                </div>
                                <div class="col-md-2">
                                    <asp:DropDownList DataTextField="ChallanNo" DataValueField="ChallanNo"
                                        AutoPostBack="true" ID="dropdownChallanNo" CausesValidation="false"
                                        runat="server" class="form-control"
                                        OnSelectedIndexChanged="dropdownDuplicateFIle_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                
                            </div>
                           
                            <div class="row" style="margin-left:10px;">
                                <asp:Label ID="LblMessage" runat="server" Font-Names="Arial" Font-Size="15pt" ForeColor="Blue" Text=""></asp:Label>
                                &nbsp;&nbsp;
                                <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="15pt" ForeColor="#FF3300" />
                            </div>
                            <div class="row" style="overflow-x: auto; font-size: 12px;">
                                <asp:GridView ID="GridView1" runat="server" BackColor="White" AutoGenerateColumns="false"
                                    OnPageIndexChanging="GridView1_PageIndexChanging" AllowPaging="true" PageSize="50"
                                    align="center" CellPadding="4" ForeColor="#333333" GridLines="Both" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Width="100%"
                                    DataKeyNames="hsrprecordid">
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
                                                Receive
                                                        <asp:CheckBox ID="CHKSelect1" runat="server" AutoPostBack="true" OnCheckedChanged="CHKSelect1_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CHKSelect" runat="server" onclick="checkreceive(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                                Reject
                                                        <asp:CheckBox ID="CHKreject" runat="server" AutoPostBack="true" OnCheckedChanged="CHKreject_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CHKreject1" runat="server" onclick="checkreject(this);" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Vehicle Reg No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblVehicleRegNo" runat="server" Text='<%#Eval("VehicleRegNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                F Laser Code
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtFLaserCode" runat="server" Text='<%#Eval("hsrp_front_lasercode") %>'
                                                    Enabled="false"></asp:TextBox>

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                R Laser Code
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtRlaserCode" runat="server" Text='<%#Eval("hsrp_rear_lasercode") %>'
                                                    Enabled="false"></asp:TextBox>

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField Visible="false">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="id" runat="server" Text='<%#Eval("hsrprecordid") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Order Status
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderStatus" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                ChassisNo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblchassisno" runat="server" Text='<%#Eval("ChassisNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                EngineNo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblengine" runat="server" Text='<%#Eval("EngineNo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                OrderDate
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblorderdate" runat="server" Text='<%#Eval("OrderDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="row" style="float:right;">
                               <%-- <asp:ImageButton ID="btnUpdate" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/save.jpg" Style="height: 24px; width: 69px;" OnClick="btnUpdate_Click" OnClientClick="return validate()" Visible="false" />--%>
                               <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="return validate()" Visible="false" />                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>


