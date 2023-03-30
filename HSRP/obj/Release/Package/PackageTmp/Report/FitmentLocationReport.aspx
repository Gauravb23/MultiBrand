<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FitmentLocationReport.aspx.cs" Inherits="HSRP.Report.FitmentLocationReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <link href="../css/Bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../css/Bootstrap/jquery.min.js" type="text/javascript"></script>
    <script src="../css/Bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/Bootstrap/fontawesome.css" rel="stylesheet" />


     <script language="javascript" type="text/javascript">
        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
       
     </script>

     <div class="container" style="width: 100%;margin-bottom:49px;">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                             <div class="clearfix">
                                <h4 style="float:left;"> Fitment Location Report </h4>
                              <asp:Button ID="btnback" runat="server" Text="Home" style="float:right;" CssClass="btn btn-primary" OnClick="btnback_Click"  />     
                            </div>
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-md-1 col-lg-1 col-sm-1"></div>
                                <div class="col-md-1 col-lg-1 col-sm-1">
                                    <b><asp:Label Text=" State :" runat="server" ID="lblState" style="font-size:16px;" /></b>                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:DropDownList ID="ddlstate" runat="server" CssClass="form-control" DataValueField="HSRP_StateID"
                                         DataTextField="HSRPStateName" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Value="0">-- Select State --</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1 col-lg-1 col-sm-1" ><b>
                                    <asp:Label Text=" EC :" runat="server" ID="lblec" style="font-size:16px;" /></b>
                                </div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:DropDownList ID="ddlec" runat="server" CssClass="form-control" DataTextField="RTOLocationName" DataValueField="RTOLocationID">
                                        <asp:ListItem Value="0">-- Select EC --</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                 <div class="col-md-2 col-lg-2 col-sm-2"></div>
                                <div class="col-md-2 col-lg-2 col-sm-2">
                                     <asp:Button ID="btnView" runat="server" Text="View" CssClass="btn btn-primary" OnClick="btnView_Click" />
                                </div>
                            </div>
                            
                            <div class="row">                               
                                <asp:Label ID="llbMSGError" runat="server" ForeColor="Red" Visible="false"></asp:Label> 
                            </div>
                            <div class="row" style="overflow-x: auto; font-size: 14px;">
                                <asp:GridView ID="grdview" Visible="true" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                    AllowPaging="true" PageSize="50" align="center" CellPadding="2" ForeColor="#333333" GridLines="Both" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Width="100%" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" >
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
                                                S.No
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSno" runat="server" Text='<%#Eval("SubDealerId") %>'></asp:Label>                                                                                            
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Address
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAddress" runat="server" Text='<%#Eval("Address") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                City
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCity" runat="server" Text='<%#Eval("City") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                 State
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblState" runat="server" Text='<%#Eval("State") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                              Contact Person
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblContactPerson" runat="server" Text='<%#Eval("ContactPerson") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtContactPerson" runat="server" Text='<%#Eval("ContactPerson") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Contact MobileNo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblContactMobileNo" runat="server" Text='<%#Eval("ContactMobileNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtContactMobileNo" runat="server" MaxLength="10" onkeypress="return isNumberKey(event)" Text='<%#Eval("ContactMobileNo") %>'></asp:TextBox> 
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                               Landmark
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAreaOfDealer" runat="server" Text='<%#Eval("AreaOfDealer") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                       
                                        
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                Action
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Button ID="btnedit" runat="server" Text="Edit" CssClass="btn btn-primary" CommandName="Edit" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="btnupdate" runat="server" Text="Update" CssClass="btn btn-success" CommandName="Update" />
                                                <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-warning" CommandName="Cancel" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div style="align-content:center" >No records found.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
