<%@ Page Title="" Language="C#" MasterPageFile="~/OEM.Master" AutoEventWireup="true" CodeBehind="OemDashboard.aspx.cs" Inherits="HSRP.LiveReports.OemDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <section class="section full-colum" id="section9">
        <div class="container">
            <div class="row">
                <div class="energy-eff-section">
                    <div class="rowBorder">
                        <div class="col-lg-12">
                            <div class="subHeader">
                                HSRP
                            </div>
                        </div>
              
     
                        <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12" id="MIS" runat="server">
                            <section id="cardContainer">
                                <div id="card">
                                    <figure class="front">
                                        <div class="project_img">
                                            <img src="../content/dam/doitassets/eesl/images/dashboardscheme/misreports.png" style="height: 100px; width: 100px">
                                        </div>
                                        <div class="project_details">
                                            <div class="project_label">
                                                OEM Business MIS
                                            </div>
                                        </div>
                                    </figure>
                                    <figure class="back">
                                        <div class="back_holder" style="height:200px;">
                                            <div class="back_title">
                                                <p>Reports</p>
                                            </div>
                                            <ul style="color:white;font-weight:bold;">
                                                
                                                
                                                <li><a href="../Report/DealersList.aspx" style="color:white;font-weight:bold;">Dealers</a></li>
                                                <li><a href="../Report/DealerOrderSummary.aspx" style="color:white;font-weight:bold;">Dealer wise orders summary</a></li>
                                                <li><a href="../Report/DealerWiseOrderDetails.aspx" style="color:white;font-weight:bold;">Dealer wise orders Details</a></li>
                                              <li id="other" runat="server"><a href="../Report/ActiveInactiveUsers.aspx" style="color:white;font-weight:bold;">Active/Inactive users</a></li>
                                                <li><a href="../Report/DealerTransationSummary.aspx" style="color:white;font-weight:bold;">Dealer Transation Summary</a></li>
                                                <li><a href="../Report/ViewSearch_Engingno_MIS.aspx" style="color:white;font-weight:bold;">Search VehicleRegno/Chassis No</a></li>
                                                <li id="sml" runat="server"><a href="../Report/ActiveInactivechanges.aspx" style="color:white;font-weight:bold;"> Active/Inactive Dealers</a></li>
                                                <li id="lihero" runat="server"><%--<a href="../Report/DealerOutStandingOemWise.aspx" style="color:white;font-weight:bold;"> Dealer Ledger </a>--%></li>
                                                
                                                   <%--<li><a href="../Report/BookMyHSRPOemSummaryReport.aspx" style="color:white;font-weight:bold;">BookMyHSRP Summary Report</a></li> --%>
                                            </ul>
                                        </div> 
                                    </figure>
                                </div>
                            </section>
                        </div>


                         <div id="divmis" runat="server" class="col-lg-2 col-md-2 col-sm-6 col-xs-12">
                            <section id="cardContainer">
                                <div id="card">
                                    <figure class="front">
                                        <div class="project_img">
                                            <img src="../content/dam/doitassets/eesl/images/dashboardscheme/hsrpreports11.png" style="height: 100px; width: 300px">
                                        </div>
                                        <div class="project_details">
                                            <div class="project_label">
                                               BookMyHSRP MIS
                                            </div>
                                        </div>
                                    </figure>
                                    <figure class="back">
                                        <div class="back_holder" style="height:290px; width:400px ">
                                            <div class="back_title">
                                                <p>BookMyHSRP Reports</p>
                                            </div>
                                            <ul style="color:white;font-weight:bold;">
                                                
                                                 <li><a href="../Report/TrackBookingOrder.aspx" style="color:white;font-weight:bold;">1. Order Summary Report </a></li> 
                                                <li><a href="../Report/BookMyHSRPOemSummaryReport.aspx" style="color:white;font-weight:bold;">2. Dealer Wise Appointments Summary-HSRP Sets</a></li> 
                                               <li id="HyundaiId" runat="server"><a href="../Report/BookMyHSRPOemSummaryReportHyundai.aspx" style="color:white;font-weight:bold;">2.1 Dealer Wise Appointments Summary-HSRP&Sticker</a></li> 
                                                <li><a href="../Report/Dealeroemwisereport.aspx" style="color:white;font-weight:bold;">3. Dealer Wise Orders-HSRP and Stickers </a></li> 
                                                <li><a href="../Report/DealerOemWiseReportDetails.aspx" style="color:white;font-weight:bold;">4. Dealer Wise Appointments Details-HSRP and Sticker </a></li> 
                                                  <li><a href="../Report/ActiveInactiveUsersBMHSRP.aspx" style="color:white;font-weight:bold;">5. Active/Inactive Status-Dealers</a></li>
                                                 <li><a href="../Transaction/AffixationReport.aspx" style="color:white;font-weight:bold;">6. Affixation Report</a></li>
                                                <div id="RId" runat="server"> <li><a href="../Report/CheckStatusHSRP.aspx" style="color:white;font-weight:bold;">6. Details Report </a></li>  </div> 
                                               <div id="HRId"  runat="server"> <li><a href="../Report/DealerOemWiseReportDetailsHome.aspx" style="color:white;font-weight:bold;">7. Overall Summary Report (Home Visit) </a></li>  </div>
                                                <div  id="DRId" runat="server"> <li><a href="../Report/DealerOemWiseReportDealerDetails.aspx" style="color:white;font-weight:bold;">8. Dealer wise Order Summary Report </a></li>  </div>
                                                 <div id="divMISDealer" runat="server"> <%--<li><a href="../Report/VehicleDealerMIS.aspx" style="color:white;font-weight:bold;">9. Vehicle Dealers Report</a></li>--%>  </div> 
                                                <div id="divHero" runat="server">  <%--<li><a href="../Report/VahanStatus.aspx" style="color:white;font-weight:bold;">10. Vahan Status Report</a></li>--%></div> 
                                                 <div id="divFitment" runat="server">  <%--<li><a href="../Report/VehicleFitmentImage.aspx" style="color:white;font-weight:bold;">11. Vahan Fitment Photos</a></li>--%></div> 
                                                <%-- <li><a href="../Report/TrackBookingOrder.aspx" style="color:white;font-weight:bold;">1. Order Summary Report </a></li> 
                                                <li><a href="../Report/BookMyHSRPOemSummaryReport.aspx" style="color:white;font-weight:bold;">2. Dealer Wise Appointments Summary-HSRP Sets</a></li> 
                                               <li id="HyundaiId" runat="server"><a href="../Report/BookMyHSRPOemSummaryReportHyundai.aspx" style="color:white;font-weight:bold;">2.1 Dealer Wise Appointments Summary-HSRP&Sticker</a></li> 
                                                <li><a href="../Report/Dealeroemwisereport.aspx" style="color:white;font-weight:bold;">3. Dealer Wise Orders-HSRP and Stickers </a></li> 
                                                <li><a href="../Report/DealerOemWiseReportDetails.aspx" style="color:white;font-weight:bold;">4. Dealer Wise Appointments Details-HSRP and Sticker </a></li> 
                                                  <li><a href="../Report/ActiveInactiveUsersBMHSRP.aspx" style="color:white;font-weight:bold;">5. Active/Inactive Status-Dealers</a></li>
                                                 <li><a href="../Transaction/AffixationReport.aspx" style="color:white;font-weight:bold;">6. Affixation Report</a></li>                                              
                                                <div id="RId" runat="server"> <li><a href="../Report/CheckStatusHSRP.aspx" style="color:white;font-weight:bold;">6. Details Report </a></li>  </div> 
                                                   
                                               <div id="HRId"  runat="server"> <li><a href="../Report/DealerOemWiseReportDetailsHome.aspx" style="color:white;font-weight:bold;">8. Overall Summary Report (Home Visit) </a></li>  </div> 
                                                <div  id="DRId" runat="server"> <li><a href="../Report/DealerOemWiseReportDealerDetails.aspx" style="color:white;font-weight:bold;">9. Dealer wise Order Summary Report </a></li>  </div>
                                                  --%>
                                                  <%--<li><a href="../Report/BookMyHSRPOemSummaryReport.aspx" style="color:white;font-weight:bold;">BookMyHSRP Summary Report</a></li> --%>

                                                  
                                            </ul>
                                        </div> 
                                    </figure>
                                </div>
                            </section>
                        </div>
                    <%--    <div class="col-lg-2 col-md-2 col-sm-6 col-xs-12" >
                            <a onclick="AddNewPop();" href="#">      
                              <figure class="front">
                                        <div class="project_img">
                                            <img src="../content/dam/doitassets/eesl/images/dashboardscheme/Hsrp.jpg" style="height: 100px; width: 100px;border-radius: 50%;">
                                        </div>
                                           <div class="project_details">
                                            <div class="project_label">
                                            </div>
                                        </div>
                                    </figure>
                                </a>
                        </div>--%>
                   
                    </div>
                    <div class="row">

                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>

