<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FitterMobile.aspx.cs" Inherits="HSRP.Transaction.FitterMobile1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="../css/Bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../css/Bootstrap/jquery.min.js" type="text/javascript"></script>
    <script src="../css/Bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/Bootstrap/fontawesome.css" rel="stylesheet" />
    <script src="windowfiles/dhtmlwindow.js" type="text/javascript"></script>
    <script>
        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>

      <div class="container"  runat="server" align="center">
            <div class="row">
                <div class="col-md-12 col-lg-12 col-sm-12">
                    <div class="panel-group">
                        <div class="panel panel-default">
                           <div class="panel-heading">                           
                            <div class="clearfix">
                                <h4 style="float:left;"> Add Fitter Contacts </h4>
                             <asp:Button ID="btnback" runat="server" Text="Home" style="float:right;" CssClass="btn btn-primary" OnClick="btnback_Click"  />     
                            </div>
                           
                        </div>
                            <br />
                             <div class="row" style="overflow-x: auto; font-size: 12px;">
                                    <asp:GridView ID="grdid" runat="server" align="center" CellPadding="4" EmptyDataText="No records has been added." ForeColor="#333333" GridLines="Both" 
                                        BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" Width="750px" AutoGenerateColumns="false" 
                                        OnRowUpdating="grdid_RowUpdating" OnRowCancelingEdit="grdid_RowCancelingEdit" OnRowEditing="grdid_RowEditing">
                                        <AlternatingRowStyle BackColor="White" />
                                        <EditRowStyle BackColor="#a8bde4" />
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
                                                    <asp:Label ID="lblfitterid" runat="server" Visible="false" Text='<%#Eval("FitterId") %>'></asp:Label>
                                                    <asp:Label ID="lbldealerid" runat="server" Visible="false" Text='<%#Eval("Dealerid") %>'></asp:Label>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Fitter Name
                                                </HeaderTemplate>
                                                <ItemTemplate>

                                                    <asp:Label ID="lblfittername" runat="server" Text='<%#Eval("FitterName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" Width="150px" class="form-control" ID="txtgrdFitterName" Text='<%#Eval("FitterName") %>' placeholder="Name" MaxLength="50"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Fitter Mobile No
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblfittermobile" runat="server" Text='<%#Eval("FitterMobile") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" Width="150px" class="form-control" ID="txtgrdFitterMobile" Text='<%#Eval("FitterMobile") %>' placeholder="Mobileno" onkeypress="return isNumberKey(event)" MaxLength="10"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ButtonEdit" CommandName="Edit" runat="server" AlternateText="ImageButton 1" ImageAlign="Middle" ImageUrl="../images/button/edit.png" Style="height: 30px; width: 30px;" />
                                                    <%--<asp:ImageButton ID="ButtonDelete" CommandName="Delete" runat="server" AlternateText="ImageButton 1" ImageAlign="Middle" ImageUrl="../images/button/delete.png" Style="height: 30px; width: 30px;" />--%>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="ButtonUpdate" CommandName="Update" runat="server" AlternateText="ImageButton 1" ImageAlign="Middle" ImageUrl="../images/button/update.png" Style="height: 30px; width: 30px;" />
                                                    <asp:ImageButton ID="ButtonCancel" CommandName="Cancel" runat="server" AlternateText="ImageButton 1" ImageAlign="Middle" ImageUrl="../images/button/cancel.png" Style="height: 30px; width: 30px;" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            <div class="panel-body">
                                <div class="row">
                                    <div class="col-md-2 col-lg-2 col-sm-2"></div>
                                    <div class="col-md-4 col-lg-4 col-sm-4">
                                        <b>Fitter Name :</b>                                     
                                    </div>
                                    <div class="col-md-2 col-lg-2 col-sm-2">
                                        <asp:TextBox ID="txtFitterName" class="form-control" runat="server" MaxLength="50" ></asp:TextBox>
                                    </div>                                                                      
                                </div>  
                                <div class="row" style="margin-top: 10px;">
                                     <div class="col-md-2 col-lg-2 col-sm-2"></div>
                                     <div class="col-md-4 col-lg-4 col-sm-4">
                                         <b>Fitter Mobile no :</b>
                                     </div>
                                     <div class="col-md-2 col-lg-2 col-sm-2">
                                         <asp:TextBox ID="txtFitterMobile" class="form-control" runat="server" MaxLength="10"  onkeypress="return isNumberKey(event)"></asp:TextBox>
                                     </div>
                                </div>
                                <div class="row" style="margin-top: 10px;">
                                     <div style="align-items:center;">
                                        <%--<asp:ImageButton ID="buttonSave" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageUrl="../images/button/save.jpg" Style="height: 24px; width: 69px;" OnClick="buttonSave_Click" OnClientClick="return validate()" ValidationGroup="fittermobile" />--%>
                                         <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                    </div>
                                </div>
                                <div class="row" style="margin-left: 20px;">
                                     <asp:Label ID="lblerr" runat="server" ForeColor="Red"></asp:Label>  
                                </div>
                            </div>
                          
                        </div>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>
