<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="BMHSRPFittmentEntryImage.aspx.cs" Inherits="HSRP.Transaction.BMHSRPFittmentEntryImage" EnableEventValidation="false" %>

<%@ Register TagPrefix="ComponentArt" Namespace="ComponentArt.Web.UI" Assembly="ComponentArt.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link href="../css/Bootstrap/bootstrap.min.css" rel="stylesheet" />
    <script src="../css/Bootstrap/jquery.min.js" type="text/javascript"></script>
    <script src="../css/Bootstrap/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="../css/Bootstrap/fontawesome.css" rel="stylesheet" />

   
    
    <div class="container" style="width: 100%">
        <div class="row">
            <div class="col-md-12 col-lg-12 col-sm-12">
                <div class="panel-group">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4>BookMyHSRP Vehicle Fittment Entry</h4> <asp:Button ID="btnback" runat="server" Text="Home" style="float:right;" CssClass="btn btn-primary" OnClick="btnback_Click"  />                            
                        </div>
                        <div class="panel-body">
                            <div class="row" id="TR2" runat="server">
                            </div>
                            <div class="row" id="TRRTOHide" runat="server">
                            </div>
                            <div class="row">
                                <div class="col-md-2">
                                    <asp:Label Text="Vehicle Reg No:" runat="server"
                                        ID="labelOrderStatus" />
                                    <span style="color: #FF3300">*</span>  
                                </div>
                                <div class="col-md-2">
                                  
                                     <asp:TextBox ID="txtRegNumber" runat="server" class="form-control" MaxLength="10"></asp:TextBox>
                                    
                                </div>

                                 
                                  </div>


                             <div class="row">
                                      <div class="col-md-2 col-lg-2 col-sm-2">
                                   <asp:Label ID="Label1" runat="server" Text="Front LaserCode Affix Image" >   </asp:Label> 
                                          <span style="color: #FF3300">*</span>  
                                         
                                               
                                </div>
                                <div class="col-md-6 col-lg-6 col-sm-6" style="margin-top:10px;">
                                    <asp:FileUpload runat="server" ID="FId" />
                                                <asp:HiddenField runat="server" ID="HiddenFId" />
                                        <br />

                                </div>
                                 </div>

                                      <div class="row">
                                      <div class="col-md-2 col-lg-2 col-sm-2">
                                    <asp:Label ID="Label3" runat="server" Text="Rear LaserCode Affix Image">     </asp:Label>  
                                          <span style="color: #FF3300">*</span>  
                                         
                                               
                                </div>
                                <div class="col-md-6 col-lg-6 col-sm-6" style="margin-top:10px;">
                                      <asp:FileUpload runat="server" ID="RId" />
                                              <asp:HiddenField runat="server" ID="HiddenRId" />
                                        <br />

                                </div>

                         
                                <div class="col-md-6 col-lg-6 col-sm-6">
                                 
                                     <br />

                                </div>
                                    </div>
                                <div class="col-md-2">
                                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-success" Text="Confirm Fitment" OnClick="Button1_Click" />
                                        <%--<asp:ImageButton ID="buttonSave" runat="server" AlternateText="ImageButton 1" TabIndex="2" ImageAlign="Right" ImageUrl="../images/button/save.jpg" Style="height: 24px; width: 69px;" onClick="btnGO_Click" OnClientClick="return validate()" />--%>

                                </div>
                                <div>
                                  <%--  <a href="https://play.google.com/store/apps/details?id=com.app.hsrp"  >Mobile App Link</a>--%>
                                     
                                </div>
                            </div>
                           
                            <div class="row">
                               &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;  <asp:Label ID="LblMessage" runat="server" Font-Names="Arial" Font-Size="10pt" ForeColor="Blue" Text=""></asp:Label>
                                &nbsp;&nbsp;
                                        <asp:Label ID="lblErrMsg" runat="server" Font-Names="Arial" Font-Size="15pt" ForeColor="#FF3300" />
                            </div>
                          
                        </div>
                    </div>
                </div>
            </div>
        </div>
    

    
    <br />
    <asp:HiddenField ID="hiddenUserType" runat="server" />
</asp:Content>


