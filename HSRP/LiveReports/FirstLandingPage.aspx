<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="FirstLandingPage.aspx.cs" Inherits="HSRP.LiveReports.FirstLandingPage" EnableEventValidation="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <script type="text/javascript">
        function AddNewPop() { //Define arbitrary function to run desired DHTML Window widget codes
            googlewin = dhtmlwindow.open("googlebox", "iframe", "../Transaction/FitterMobile.aspx", "Add New Fitter", "width=700px,height=400px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = '../LiveReports/LiveTracking.aspx';
                return true;
            }
        }
        function ComplaintsPopup() { //Define arbitrary function to run desired DHTML Window widget codes
            googlewin = dhtmlwindow.open("googlebox", "iframe", "../Complaints.aspx", "Add New Complaint", "width=800px,height=600px,resize=1,scrolling=1,center=1", "recal")
            googlewin.onclose = function () {
                window.location = '../LiveReports/LiveTracking.aspx';
                return true;
            }
        }
        function isNumberKey(evt) {
            //debugger;
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
  
    <section class="section full-colum" id="section9">
          <div class="energy-eff-section">
                    <div class="rowBorder">
        <div class="container">
            <div class="row" runat="server" id="ADiv" visible="true">
              <div class="col-md-1" style="text-align:right">
                  <h2>A. </h2>  
              </div>
                <div class="col-md-11">
                <h2><a href="../Transaction/NewCashReceiptDataEntry.aspx"> Order Booking </a></h2> 
              </div>
            </div>
           
            <br />

           <%--  <div class="row" runat="server" id="DDiv" visible="false">
              <div class="col-md-1" style="text-align:right">
                  <h2>AA.</h2> 
              </div>
                  <div class="col-md-11">
                  <h2><a href="../Transaction/CashReceiptDFDRDB.aspx">Only DF/DR/DB(Veh. Reg. Date 1 April 2019 Onwards)</a></h2> 
              </div>
            </div>
              <br />
           <div class="row"  runat="server" id="BDiv" visible="false">
              <div class="col-md-1" style="text-align:right">
                  <h2>B. </h2>
              </div>
               <div class="col-md-11">
                  <h2><a href="../Transaction/CashReceiptOldDataEntry.aspx">Old Vehicle (Veh. Reg. Date prior 1 April 2019)</a></h2> 
              </div>
            </div>
            
            <br />
             <div class="row" runat="server" id="CDiv" visible="false">
              <div class="col-md-1" style="text-align:right">
                  <h2>C. </h2> 
              </div>
                  <div class="col-md-11">
                  <h2><a href="../Transaction/StickerProcess.aspx">Only Sticker</a></h2> 
              </div>
            </div>--%>

            
           
        </div>
                        </div>
              </div>
    </section>
</asp:Content>
