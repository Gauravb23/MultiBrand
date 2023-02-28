<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AffixAddressUpdate.aspx.cs" Inherits="HSRP.Transaction.AffixAddressUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
       

        div[class^="col-"]:not(:first-child) {   padding-bottom:10px; } div[class^="col-"]:not(:last-child) {   padding-right: 70px; }
    
    </style>

     <script type="text/javascript">
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
     </script>
 
        
    <div class="container">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">

                        <div class="container">
                            
                        <h3 style="text-align: center; background: linear-gradient(90deg, #fff, #434f6a, #fff); color: white; font-family: sans-serif; padding: 10px 0px;">Affixation Address Update</h3>

                        <div class="row">
                            <div class="col-sm-6">
                                <div class="row">
                                    <div class="col-sm-6">
                                        State:
                                    </div>
                                    <div class="col-sm-6">

                                        <asp:DropDownList ID="ddlstate" runat="server" AutoPostBack="true" CssClass="form-control" DataTextField="HSRPStateName"
                                            DataValueField="HSRP_StateId">
                                            <asp:ListItem Value="0">--Select State --</asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        City:
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtCity" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-6">
                                        Pincode:
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" CssClass="form-control" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        Name:
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="row">
                                    <div class="col-sm-6">
                                        Address:
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtAddress" runat="server" MaxLength="300" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        Landmark:
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtLandmark" runat="server" MaxLength="300" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-6">
                                        Mobile No:
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtmobile" runat="server" MaxLength="10" CssClass="form-control" onkeypress="return isNumber(event)"></asp:TextBox>
                                    </div>
                                </div>

                            </div>


                        </div>

                        <div class="row" style="margin:1rem;">
                            <div style="text-align:end">
                                     <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="Add" OnClick="btnAdd_Click" />
                                     <asp:Button ID="btnBack" runat="server" class="btn btn-success" Text="Back" OnClick="btnBack_Click" />
                            </div>
                        </div>


                            <div style="margin:auto;width:90%;display:none;">

                            <div>
                                <h3 style="font-family: sans-serif; margin: 20px 0 8px;">Dealer Details</h3>
                                <table>
                                    <tr>
                                        <td>OEM Name</td>
                                        <td>
                                            <asp:Label ID="lbloemname" runat="server" Text=""></asp:Label></td>
                                        <td>Dealership Name</td>
                                        <td>
                                            <asp:Label ID="lbldealername" runat="server" Text=""></asp:Label></td>
                                        <td>Address</td>
                                        <td>
                                            <asp:Label ID="lbladdress" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>User ID</td>
                                        <td>
                                            <asp:Label ID="lbluserid" runat="server" Text=""></asp:Label></td>
                                        <td>Dealer Code by OEM</td>
                                        <td>
                                            <asp:Label ID="lbldealercode" runat="server" Text=""></asp:Label></td>
                                       <td>Vehicle Class</td>
                                        <td>
                                            <asp:Label ID="lblclass" runat="server" Text=""></asp:Label></td>
                                    </tr>                                   
                                </table>
                            </div>

                            <br>

                      <%--      <div>
                                <h3 style="font-family: sans-serif; margin: 20px 0 8px;">New Address Details</h3>
                                <table>
                                    <tr>
                                        <td>State</td>
                                        <td>
                                            <asp:DropDownList ID="ddlstate" runat="server" AutoPostBack="true" CssClass="form-control" DataTextField="HSRPStateName"
                                                DataValueField="HSRP_StateId" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged">
                                                <asp:ListItem Value="0">--Select State --</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>Loaction</td>
                                        <td>
                                            <asp:DropDownList ID="ddlembcenter" runat="server" CssClass="form-control" DataValueField="Emb_Center_id" DataTextField="EmbCenterName">
                                                <asp:ListItem Value="0">-- Select Location --</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>Address</td>
                                        <td>
                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="300" CssClass="form-control"></asp:TextBox>
                                        </td>                                      
                                        
                                        <td>City</td>
                                        <td>
                                            <asp:TextBox ID="txtCity" runat="server" MaxLength="100" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td>Pincode</td>
                                        <td>
                                            <asp:TextBox ID="txtPincode" runat="server" MaxLength="6" CssClass="form-control"></asp:TextBox>
                                        </td>                                                                             
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>Contact Person Name</td>
                                        <td>
                                            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                        </td>
                                        <td>Mobile Number</td>
                                         <td>
                                            <asp:TextBox ID="txtmobile" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                                        </td>                                       
                                       <td>
                                           <asp:Button ID="btnAdd" runat="server" class="forlastcolumn" Text="Add" OnClick="btnAdd_Click" />
                                       </td>
                                    </tr>
                                </table>
                            </div>--%>

                            <br>
                           
                            <br>
                        </div>
                        <asp:Label ID="lblerrmsg" runat="server" Text="" ForeColor="Red" Style="margin-left: 65px;" Font-Size="18px"></asp:Label>
                        <asp:Label ID="lblsucmsg" runat="server" Text="" ForeColor="Green" Style="margin-left: 65px;" Font-Size="18px"></asp:Label>
                     
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
