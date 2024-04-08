<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="LiveTracking.aspx.cs" Inherits="HSRP.LiveReports.LiveTracking" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript" src="../windowfiles/dhtmlwindow.js"></script>
    <link rel="stylesheet" href="../windowfiles/dhtmlwindow.css" type="text/css" />
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style type="text/css">
        body {
            font-family: Arial;
            font-size: 10pt;
        }

        .center {
            margin: 0 auto;
            width: 60%;
        }


        .front .project_img img {
            width: 92px !important;
            height: 80px !important;
        }


        .card-container {
            display: flex;
            gap: 1rem;
            padding: 2rem;
            overflow: hidden;
            justify-content: center;
            align-items: center;
        }

        .flip-card-back {
            padding: 1rem;
        }




        .card-c {
            padding: 1rem;
        }



        .flip-card, .card-c {
            width: 200px;
            height: 200px;
        }

        .flip-card {
            background-color: transparent;
            perspective: 1000px; /* Remove this if you don't want the 3D effect */
        }

        .flip-card-front, card-c {
        }

        /* This container is needed to position the front and back side */
        .flip-card-inner {
            position: relative;
            width: calc(100%);
            height: calc(100% );
            transition: transform 0.8s;
            transform-style: preserve-3d;
        }



        /* Do an horizontal flip when you move the mouse over the flip box container */
        .flip-card:hover .flip-card-inner {
            transform: rotateY(180deg);
        }

        /* Position the front and back side */
        .flip-card-front, .flip-card-back {
            position: absolute;
            width: calc(100% );
            height: calc(100%);
            -webkit-backface-visibility: hidden; /* Safari */
            backface-visibility: hidden;
        }
        /* Style the front side (fallback if image is missing) */
        .flip-card-front {
            color: black;
            padding: 1rem;
        }

        .flip-card-back {
            background-color: #0354cc;
            color: white;
            transform: rotateY(180deg);
        }

        .flex-item {
            display: flex;
            justify-content: center;
            align-items: center;
            flex-direction: column;
        }

            .flex-item img {
                height: 80px;
                object-fit: contain;
                stroke:#0354cc!important;
            }

        .card-c {
            padding: 1em;
            display: flex;
            align-items: center;
                justify-content: center;
        }

         .card-c:hover{
             background-color:whitesmoke;
         }
    </style>

    <script type="text/javascript">
        if (document.layers) {
            //Capture the MouseDown event.
            document.captureEvents(Event.MOUSEDOWN);

            //Disable the OnMouseDown event handler.
            document.onmousedown = function () {
                return false;
            };
        }
        else {
            //Disable the OnMouseUp event handler.
            document.onmouseup = function (e) {
                if (e != null && e.type == "mouseup") {
                    //Check the Mouse Button which is clicked.
                    if (e.which == 2 || e.which == 3) {
                        //If the Button is middle or right then disable.
                        return false;
                    }
                }
            };
        }

        //Disable the Context Menu event.
        document.oncontextmenu = function () {
            return false;
        };
    </script>

    <div class="container-fluid">
        <div class="rowBorder">

            <div>
                <h3 style="text-align: center; background: linear-gradient(90deg, #fff, #002e72, #fff); color: white; font-family: sans-serif; padding: 10px 0px;">HSRP Order Booking System</h3>

            </div>         

            <div class="card-container">

                <div class="flip-card">
                    <div class="flip-card-inner">
                        <div class="flip-card-front flex-item">
                            <div class=" flex-item">
                                <img style="color:#002e72" src="../images/orderCart.svg" alt="txt" />
                            </div>
                            <div>
                                <h3 style="color:#002e72"><b>Book Order</b></h3>
                            </div>
                        </div>
                        <div class="flip-card-back" >
                            <div class="back_title" style="margin-bottom: 1rem;">
                                <p >Book Order</p>
                            </div>
                            <ul>
                                <li></li>
                                  <li><a href="../Transaction/NewCashReceiptDataEntry.aspx" style="color: white; font-weight: bold;">Book Order</a></li>
                                <li><a href="../Transaction/NewCashReceiptDataEntryDFDRDB.aspx" style="color: white; font-weight: bold;">Book Order (Damage)</a></li>
                                <li><a href="../Transaction/StickerProcess.aspx" style="color: white; font-weight: bold;">Book Only Sticker</a></li>
                               </ul>
                        </div>
                    </div>
                </div>

               <%-- <a href="../Transaction/NewCashReceiptDataEntry.aspx" style="color: black; text-decoration: none;">
                    <div class="card-c">
                        <div class=" flex-item">
                            <div class=" flex-item">
                                <img src="../images/orderCart.svg" alt="txt" />
                            </div>
                            <div>
                                <h3 style="color:#002e72"><b>Book Order</b></h3>
                            </div>
                        </div>

                    </div>
                </a>

                 <a href="../Transaction/StickerProcess.aspx" style="color: black; text-decoration: none;">
                    <div class="card-c">
                        <div class=" flex-item">
                            <div class=" flex-item">
                                <img src="../images/orderCart.svg" alt="txt" />
                            </div>
                            <div>
                                <h3 style="color:#002e72"><b>Only Sticker</b></h3>
                            </div>
                        </div>
                    </div>
                </a>--%>

                 <a href="../Transaction/VahanVerifiedDetails.aspx" style="color: black; text-decoration: none;">
                    <div class="card-c">
                        <div class=" flex-item">
                            <div class=" flex-item">
                                <img style="color:#002e72" src="../images/search.svg" alt="txt" />
                            </div>
                            <div>
                                <h3 style="color:#002e72"><b>Vahan Search</b></h3>
                            </div>
                        </div>

                    </div>
                </a>

               

                <div class="flip-card">
                    <div class="flip-card-inner">
                        <div class="flip-card-front flex-item">
                            <div class=" flex-item">
                                <img style="color:#002e72" src="../images/report.svg" alt="txt" />
                            </div>
                            <div>
                                <h3 style="color:#002e72"><b>Reports</b></h3>
                            </div>
                        </div>
                        <div class="flip-card-back" >
                            <div class="back_title" style="margin-bottom: 1rem;">
                                <p >Reports</p>
                            </div>
                            <ul>
                                <li></li>
                                  <li><a href="../Transaction/DealerLedger.aspx" style="color: white; font-weight: bold;">Dealer Ledger</a></li>
                                <li><a href="../Report/ViewInvoices.aspx" style="color: white; font-weight: bold;">Invoices</a></li>
                                 <li><a href="../Transaction/LaserReceivedDealerwise.aspx" style="color: white; font-weight: bold;">HSRP Plate Receiving Entry</a></li>
                                <li><a href="../Transaction/BMHSRPFittmentEntryImage.aspx" style="color: white; font-weight: bold;">Fitment Entry With Image</a></li>
                                <li><a href="../Transaction/FitterMobile.aspx" style="color: white; font-weight: bold;">Add Fitter Mobile Number</a></li>                               
                                <li><a href="../Report/FitmentLocationReport.aspx" style="color: white; font-weight: bold;">Fitment Location Report</a></li>                               
                               </ul>
                        </div>
                    </div>
                </div>


                  <div class="flip-card">
                    <div class="flip-card-inner">
                        <div class="flip-card-front flex-item">
                            <div class=" flex-item">
                                <img  src="../images/status.svg" alt="txt" />
                            </div>
                            <div>
                                <h3 style="color:#002e72"><b>Status</b></h3>
                            </div>
                        </div>
                        <div class="flip-card-back">
                            <div class="back_title" style="margin-bottom: 1rem;">
                                <p >Status</p>
                            </div>
                            <ul>
                                  
                                    <li><a href="../Transaction/TrackOrder.aspx" style="color: white; font-weight: bold;">Track Order</a></li>
                                <li><a href="../Transaction/DLViewDealerOrder.aspx" style="color: white; font-weight: bold;">View Order</a></li>

                            </ul>
                        </div>
                    </div>
                </div>

            </div>


        </div>

    </div>

</asp:Content>
