<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="MainEntry.aspx.cs" Inherits="HSRP.LiveReports.MainEntry" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    

    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    
     
      <section class="section full-colum" id="section9">
        <div class="container">
            <div class="row">
                <div class="energy-eff-section">
                    <div class="rowBorder">
                        <div class="col-lg-12">
                            <marquee behavior="Scroll" direction="left" onmouseover="this.stop();" onmouseout="this.start();" class="nav-item"><asp:Label runat="server" ID="paymentmsg" ForeColor="Red" style="color: Red;font-size: 18px;font-weight: bold;" Visible="true"></asp:Label></marquee>
                            <br />
                           
                            <div class="row" style="padding-bottom: 39px;">
                                <div class="col-md-6 col-lg-6 col-sm-6" ><asp:Label ID="Label1" runat="server" style="font-size: 30px;color: #253b7c !important;padding-top: 39px;" Text="HSRP Order Booking System"></asp:Label>&nbsp; &nbsp; &nbsp; &nbsp;
                                
                                <div class="col-md-6 col-lg-6 col-sm-6" style="text-align:right;padding-top: 39px;"><asp:Button runat="server" ID="btnonlinepayment" OnClick="btnonlinepayment_Click" Text="Online Payment" CssClass="btn btn-group-lg warning" BackColor="#ff9933" ForeColor="#000000" Width="200PX" /></div>
                            </div>                          
                        </div>
                        
                        <div class="col col-md-1 col-sm-2 col-xs-2">
                            <section id="cardContainer">
                                <div id="card" style="width:122px;">
                                    <figure class="front">
                                        <div class="project_img">
                                            <img src="../content/dam/doitassets/eesl/images/dashboardscheme/ordernow.png" style="height: 300px; width: 150px">
                                        </div>
                                        <div class="project_details">
                                            <div class="project_label" style="color:black; font-weight:bold;">
                                                <span style="font-size:15px;"> Order Booking </span> 
                                            </div>
                                        </div>
                                    </figure>                                
                                </div>
                            </section>
                        </div>

                        <div class="col-lg-2 col-md-1 col-sm-2 col-xs-2" style="margin-left:50px; position:relative;left:16px;" >
                            <section id="cardContainer">
                                <div id="card">
                                    <figure class="front">
                                        <div class="project_img">
                                            <img src="../content/dam/doitassets/eesl/images/dashboardscheme/misreports.png" style="height:250px; width: 300px">
                                        </div>
                                        <div class="project_details">
                                            <div class="project_label" style="color:black; font-weight:bold;">
                                              <span style="font-size:15px;">   Reports</span>
                                            </div>
                                        </div>
                                    </figure>
                                    <figure class="back">
                                        <div class="back_holder" style="height:195px">
                                            <div class="back_title">
                                                <p>Reports</p>
                                            </div>
                                            <ul>   
                                                 <li><a href="../Transaction/DealerLedger.aspx"style="color:white;font-weight:bold;">Dealer Ledger</a></li>
                                                  <li><a href ="../Report/ViewInvoices.aspx" style="color:white;font-weight:bold;">Invoices</a></li>
                                                 <li><asp:LinkButton runat="server" ID="redirect" Text="Online payment"  ForeColor="white" OnClick="redirect_Click"></asp:LinkButton></li>                                           
                                                <li><a href="../Transaction/trackOrder.aspx" style="color:white;font-weight:bold;">Track Order</a></li>
                                                <li> <a href="../Transaction/DLViewDealerOrder.aspx"  style="color:white;font-weight:bold;">View Order</a></li>
                                                 <li><a href ="../Transaction/VahanVerifiedDetails.aspx" style="color:white;font-weight:bold;">Check Vahan Details</a></li>                                                  
                                            </ul>
                                        </div>
                                    </figure>
                                </div>
                            </section>
                        </div>
                       </div>                                                                           
                    </div>    
                </div>
            </div>            
    </section>  
</asp:Content>
