using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Xml;
using System.Web;
using System.Data;
using System.Net;

namespace HSRP
{
    public class ECOMApi
    {
        string EComUid = ConfigurationManager.AppSettings["EComUid"].ToString();
        string EComPwd = ConfigurationManager.AppSettings["EComPwd"].ToString();
        //PinCodeApi is used to get list of deliverable locations 
        public bool PinCodeAPI(string DealerPincode)
        {
            var PinCodeUrl = ConfigurationManager.AppSettings["PinCodeUrl"].ToString();
            var PinCodePostdata = "";
            var pincodeResult = APIMethod(PinCodeUrl, "POST", EComUid, EComPwd, PinCodePostdata);
            if (pincodeResult != "")
            {
                List<PinCodes> objPinCodes = JsonConvert.DeserializeObject<List<PinCodes>>(pincodeResult);
                foreach (var pincode in objPinCodes)
                {
                    if (DealerPincode.Trim() == pincode.pincode.ToString())
                        return true;

                    //var a = pincode.status;
                    //var b = pincode.city_type;
                    //var c = pincode.pincode;
                    //var d = pincode.active;
                    //var m = pincode.state_code.ToString();
                    //var f = pincode.city;
                    //var g = pincode.dccode;
                    //var h = pincode.route;
                    //var i = pincode.state;
                    //var j = pincode.date_of_discontinuance;
                    //var k = pincode.city_code;
                }
            }
            return false;
        }

        //WayBillsAPI is used to get single/multiple AWB number to store into database for future use
        public int WayBillsAPI(string type, int count)
        {           
            //int count = 20;
            //var type = "PPD";
            var awbsno = 0;
            var WaybillsPostdata = "count=" + count + "&type=" + type + "";
            var WaybillsUrl = ConfigurationManager.AppSettings["WaybillsUrl"].ToString();
            var WaybillResult = APIMethod(WaybillsUrl, "POST", EComUid, EComPwd, WaybillsPostdata);

            if (WaybillResult != "")
            {
                Waybills objWaybills = JsonConvert.DeserializeObject<Waybills>(WaybillResult);
                var a = objWaybills.reference_id;
                var b = objWaybills.success;
                if (b.ToLower() == "yes")
                {
                    for (int i = 0; i < objWaybills.awb.Count; i++)
                    {
                        awbsno = objWaybills.awb[i];
                    }
                }

            }

            return awbsno;
           
        }

        //ForwardManifestAPI is used to book the orders for a single AWB number. With single order multiple items can be booked. 
        public string ForwardManifestAPI(ForwardManifest fw)
        {
            //ForwardManifest fw = new ForwardManifest();
            //fw.AWB_NUMBER = "114719794";
            //fw.ORDER_NUMBER = "107249072-001";
            //fw.PRODUCT = "PPD";
            //fw.CONSIGNEE = "Anish Kumar Dubey";
            //fw.CONSIGNEE_ADDRESS1 = "H. No. A10";
            //fw.CONSIGNEE_ADDRESS2 = "Block";
            //fw.CONSIGNEE_ADDRESS3 = "Sector";
            //fw.DESTINATION_CITY = "GURGAON";
            //fw.PINCODE = "111111";
            //fw.STATE = "DL";
            //fw.MOBILE = "1111111111";
            //fw.TELEPHONE = "0123456789";
            //fw.ITEM_DESCRIPTION = "Bicycle";
            //fw.PIECES = 1;
            //fw.COLLECTABLE_VALUE = 0;
            //fw.DECLARED_VALUE = 1000.0;
            //fw.ACTUAL_WEIGHT = 0.5;
            //fw.VOLUMETRIC_WEIGHT = 0;
            //fw.LENGTH = 12;
            //fw.BREADTH = 1;
            //fw.HEIGHT = 2;
            //fw.PICKUP_NAME = "Pickup";
            //fw.PICKUP_ADDRESS_LINE1 = "";
            //fw.PICKUP_ADDRESS_LINE2 = "";
            //fw.PICKUP_PINCODE = "111111";
            //fw.PICKUP_PHONE = "1111111111";
            //fw.PICKUP_MOBILE = "1234567890";
            //fw.RETURN_NAME = "anish";
            //fw.RETURN_ADDRESS_LINE1 = "mzp";
            //fw.RETURN_ADDRESS_LINE2 = "";
            //fw.RETURN_PINCODE = "111111";
            //fw.RETURN_PHONE = "1111111111";
            //fw.RETURN_MOBILE = "0123456789";
            //fw.ADDONSERVICE = new List<string>();
            //fw.DG_SHIPMENT = "false";

            //List<ADDITIONALINFORMATION> itemcollection = new List<ADDITIONALINFORMATION>();
            //for (int i = 0; i < 2; i++)
            //{
            //    ADDITIONALINFORMATION adiObj = new ADDITIONALINFORMATION();
            //    adiObj.essentialProduct = "Y";
            //    adiObj.OTP_REQUIRED_FOR_DELIVERY = "Y";
            //    adiObj.DELIVERY_TYPE = "";
            //    adiObj.SELLER_TIN = "SELLER_TIN_1234";
            //    adiObj.INVOICE_NUMBER = "InvoiceTin";
            //    adiObj.INVOICE_DATE = "21-03-2021";
            //    adiObj.ESUGAM_NUMBER = "";
            //    adiObj.ITEM_CATEGORY = "";
            //    adiObj.PACKING_TYPE = "Box";
            //    adiObj.PICKUP_TYPE = "WH";
            //    adiObj.RETURN_TYPE = "WH";
            //    adiObj.CONSIGNEE_ADDRESS_TYPE = "WH";
            //    adiObj.PICKUP_LOCATION_CODE = "";
            //    adiObj.SELLER_GSTIN = "GISTN988787";
            //    adiObj.GST_HSN = "123456";
            //    adiObj.GST_ERN = "123456";
            //    adiObj.GST_TAX_NAME = "DELHI GST";
            //    adiObj.GST_TAX_BASE = 900.0;
            //    adiObj.DISCOUNT = 0.0;
            //    adiObj.GST_TAX_RATE_CGSTN = 5.0;
            //    adiObj.GST_TAX_RATE_SGSTN = 1;
            //    adiObj.GST_TAX_RATE_IGSTN = 0.0;
            //    adiObj.GST_TAX_TOTAL = 1;
            //    adiObj.GST_TAX_CGSTN = 1;
            //    adiObj.GST_TAX_SGSTN = 1;
            //    adiObj.GST_TAX_IGSTN = 1;

            //    itemcollection.Add(adiObj);

            //}

            //fw.ADDITIONAL_INFORMATION = itemcollection;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonStr = serializer.Serialize(fw);

            var ForwardManifestPostdata = "username=" + EComUid + "&password=" + EComPwd + "&json_input=[" + jsonStr + "]";
            var ForwardManifestUrl = ConfigurationManager.AppSettings["ForwordManifestUrl"].ToString();
            var ForwardManifestResult = APIMethod(ForwardManifestUrl, "POST", EComUid, EComPwd, ForwardManifestPostdata);

            if (ForwardManifestResult != "")
            {
                ApiResponse objFwr = JsonConvert.DeserializeObject<ApiResponse>(ForwardManifestResult);

                for (int i = 0; i < objFwr.shipments.Count; i++)
                {
                    Shipment spm = objFwr.shipments[i];

                    var a = spm.reason;
                    var b = spm.order_number;
                    var c = spm.awb;
                    var d = spm.success;
                    if (spm.success == false)
                    {
                        return spm.reason;
                    }
                }
            }
            return "";
        }

        //ShipmentCancelationAPI is used to cancel the single/multiple orders based on awbs number on the demand of customer
        //Can be shared with customer to cancel the order
        private void ShipmentCancelationAPI()
        {
            var awbs1 = "114719792";
            var awbs2 = "114719793";

            var ShpmCancUrl = ConfigurationManager.AppSettings["ShpmCancUrl"].ToString();
            var ShpmCancPostdata = "username=" + EComUid + "&password=" + EComPwd + "&awbs=" + awbs1 + "," + awbs2 + "";
            var ShpmCancResult = APIMethod(ShpmCancUrl, "POST", EComUid, EComPwd, ShpmCancPostdata);
            if (ShpmCancResult != "")
            {
                List<Shipment> objFwr = JsonConvert.DeserializeObject<List<Shipment>>(ShpmCancResult);

                for (int i = 0; i < objFwr.Count; i++)
                {
                    Shipment spm = objFwr[i];

                    var a = spm.reason;
                    var b = spm.order_number;
                    var c = spm.awb;
                    var d = spm.success;
                }
            }
        }

        //ShipmentTrackingAPI is used to track the order status based on one or more AWBs number 
        //Return data in xml therefore no. of columns are not fixed and can not store into table
        public StringBuilder ShipmentTrackingAPI(string awb)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Field", typeof(string));
            table.Columns.Add("Value", typeof(string));            

            StringBuilder sb = new StringBuilder();
            var ShipmentTrackingFilePath = HttpContext.Current.Server.MapPath("ShipmentTrackingFile.xml");
            var ShipTrackPostdata = "awb="+ awb +"";
            var ShipTrackUrl = ConfigurationManager.AppSettings["TrackingApiUrl"].ToString();
            var ShipTrackResult = APIMethod(ShipTrackUrl, "POST", EComUid, EComPwd, ShipTrackPostdata);

            if (ShipTrackResult != "")
            {
                using (StreamWriter outputFile = new StreamWriter(ShipmentTrackingFilePath, false))
                {
                    outputFile.WriteLine(ShipTrackResult);
                }

                Ecomexpressobjects shpobj = DeserializeToObject<Ecomexpressobjects>(ShipmentTrackingFilePath);
                List<Object> obj = shpobj.Object;
                for (int i = 0; i < obj.Count; i++)
                {
                    List<Field> fld = obj[0].Field;
                    for (int j = 0; j < fld.Count; j++)
                    {

                        var columns = fld[j].Name;
                        if (columns != "scans" && (columns == "tracking_status" || columns == "last_update_datetime"))
                        {
                            var value = fld[j].Text;
                            sb.Append(columns.ToUpper() + ": " + value + "  ");
                           // sb.AppendFormat("Current Stage: {0} ({1}).\n", columns, value);

                            //table.Rows.Add(columns, value);
                        }
                        else if (columns == "scans")
                        {
                            List<Object> obj2 = fld[j].Object;                           
                            for (int k = 0; k < obj2.Count; k++)
                            {
                                List<Field> fld2 = obj2[k].Field;
                                for (int l = 0; l < fld2.Count; l++)
                                {
                                    var col = fld2[l].Name;
                                    if (col == "updated_on" || col == "scan_status")
                                    {
                                        var colval = fld2[l].Text;
                                        //sb.Append(col + " : " + colval + ",");
                                        //sb.AppendFormat("ScanStage "+ k + 1 +" {0} ({1}).\n", col, colval);

                                        //table.Rows.Add(col, colval);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sb;
        }

        //To Reverse the orders
        private void ReverseManifestAPI()
        {
            ReverseManifest revmnf = new ReverseManifest();
            ECOMEXPRESSOBJECTS ecmexp = new ECOMEXPRESSOBJECTS();
            SHIPMENT shpm = new SHIPMENT();

            shpm.AWB_NUMBER = "114719793";
            shpm.ORDER_NUMBER = "107249072-001";
            shpm.PRODUCT = "REV";
            shpm.REVPICKUP_NAME = "Manish";
            shpm.REVPICKUP_ADDRESS1 = "Change Address 1";
            shpm.REVPICKUP_ADDRESS2 = "Change Address 2";
            shpm.REVPICKUP_ADDRESS3 = "Change Address 3";
            shpm.REVPICKUP_CITY = "Delhi";
            shpm.REVPICKUP_PINCODE = "231001";
            shpm.REVPICKUP_STATE = "DL";
            shpm.REVPICKUP_MOBILE = "9999999999";
            shpm.REVPICKUP_TELEPHONE = "0123456789";
            shpm.PIECES = "1";
            shpm.COLLECTABLE_VALUE = "1000";
            shpm.DECLARED_VALUE = "1000";
            shpm.ACTUAL_WEIGHT = "0.5";
            shpm.VOLUMETRIC_WEIGHT = "0.8";
            shpm.LENGTH = "140";
            shpm.BREADTH = "130";
            shpm.HEIGHT = "120";
            shpm.VENDOR_ID = "";
            shpm.DROP_NAME = "mzp";
            shpm.DROP_ADDRESS_LINE1 = "Drop Change Address 1";
            shpm.DROP_ADDRESS_LINE2 = "Drop Change Address 2,";
            shpm.DROP_PINCODE = "231001";
            shpm.DROP_MOBILE = "1111111111";
            shpm.ITEM_DESCRIPTION = "MI4I";
            shpm.DROP_PHONE = "0123456789";
            shpm.EXTRA_INFORMATION = "test-info";
            shpm.DG_SHIPMENT = "false";

            ADDITIONALINFORMATIONRev addInfoRev = new ADDITIONALINFORMATIONRev();
            addInfoRev.SELLER_TIN = null;
            addInfoRev.INVOICE_NUMBER = "NV-34894C89";
            addInfoRev.INVOICE_DATE = "11-MAR-2021";
            addInfoRev.ESUGAM_NUMBER = "";
            addInfoRev.ITEM_CATEGORY = null;
            addInfoRev.PACKING_TYPE = null;
            addInfoRev.PICKUP_TYPE = null;
            addInfoRev.RETURN_TYPE = null;
            addInfoRev.PICKUP_LOCATION_CODE = "WH";
            addInfoRev.SELLER_GSTIN = "GSTTEST01G456ST";
            addInfoRev.GST_HSN = null;
            addInfoRev.GST_ERN = null;
            addInfoRev.GST_TAX_NAME = "delhi gst";
            addInfoRev.GST_TAX_BASE = 9000.0;
            addInfoRev.GST_TAX_RATE_CGSTN = 0.5;
            addInfoRev.GST_TAX_RATE_SGSTN = 0.5;
            addInfoRev.GST_TAX_RATE_IGSTN = 0.0;
            addInfoRev.GST_TAX_TOTAL = 100;
            addInfoRev.GST_TAX_CGSTN = null;
            addInfoRev.GST_TAX_SGSTN = null;
            addInfoRev.GST_TAX_IGSTN = null;
            addInfoRev.DISCOUNT = null;

            shpm.ADDITIONAL_INFORMATIONRev = addInfoRev;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonStr = serializer.Serialize(shpm);

            var RevManifestUrl = ConfigurationManager.AppSettings["RevManifestUrl"].ToString();
            var RevManifestPostdata = "username=" + EComUid + "&password=" + EComPwd + "&json_input=" + jsonStr;
            var RevManifestResult = APIMethod(RevManifestUrl, "POST", EComUid, EComPwd, RevManifestPostdata);
            if (RevManifestResult != "")
            {
                ReverseManifestResponse objFwr = JsonConvert.DeserializeObject<ReverseManifestResponse>(RevManifestResult);


            }
        }


        //NDRDataAPI is used to re-attemp&delivery(RAD): if order is not delivered due to unavailability of customer
        //NDRDataAPI is also used to Cancel(RTO): only used by company to cancel if amount is mismatched due to technical fault/faulty address 
        private void NDRDataAPI()
        {
            var instruction = "RAD";  //or RTO 
            var delivery_slot = "1"; //or 2 or 3
            List<NDRData> ndrItemcollection = new List<NDRData>();
            //suppose to two items are scheduled for reorder, cancel
            for (int i = 0; i < 2; i++)
            {
                NDRData ndr = new NDRData();
                ndr.awb = "114719938";
                ndr.comments = "deliver it today asap";
                ndr.instruction = instruction;
                ndr.scheduled_delivery_date = "2021-03-25"; // or 25-MAR-2021, Not required for RTO
                ndr.scheduled_delivery_slot = delivery_slot; // Not required for RTO

                ndrItemcollection.Add(ndr);
            }


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jsonStr = serializer.Serialize(ndrItemcollection);

            var NDRDataApiUrl = ConfigurationManager.AppSettings["NDRDataApiUrl"].ToString();
            var NDRApiPostdata = "json_input=" + jsonStr;
            var NDRApiResult = APIMethod(NDRDataApiUrl, "POST", EComUid, EComPwd, NDRApiPostdata);
            if (NDRApiResult != "")
            {
                List<NDRDataResponse> objNdr = JsonConvert.DeserializeObject<List<NDRDataResponse>>(NDRApiResult);
                for (int i = 0; i < objNdr.Count; i++)
                {
                    var a = objNdr[i].status;
                    var b = objNdr[i].awb;
                    var c = objNdr[i].error;
                }
            }
        }
        public T DeserializeToObject<T>(string filepath) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StreamReader sr = new StreamReader(filepath))
            {
                return (T)ser.Deserialize(sr);
            }
        }

        private string APIMethod(string URL, string methodtype, string uid, string pwd, string postData)
        {
            var result = "";
            var authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(uid + ":" + pwd));
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(URL);
            webRequest.Method = methodtype;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Headers["Authorization"] = "Basic " + authInfo;
            Stream reqStream = webRequest.GetRequestStream();
            byte[] postArray = Encoding.ASCII.GetBytes(postData);
            reqStream.Write(postArray, 0, postArray.Length);
            reqStream.Close();
            using (StreamReader sr = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            return result;
        }



    }


    public class PinCodes
    {
        public int status { get; set; }
        public string city_type { get; set; }
        public int pincode { get; set; }
        public bool active { get; set; }
        public string state_code { get; set; }
        public string city { get; set; }
        public string dccode { get; set; }
        public string route { get; set; }
        public string state { get; set; }
        public string date_of_discontinuance { get; set; }
        public string city_code { get; set; }
    }

    public class Waybills
    {
        public int reference_id { get; set; }
        public string success { get; set; }
        public List<int> awb { get; set; }
    }

    public class ADDITIONALINFORMATION
    {
        public string essentialProduct { get; set; }
        public string OTP_REQUIRED_FOR_DELIVERY { get; set; }
        public string DELIVERY_TYPE { get; set; }
        public string SELLER_TIN { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string INVOICE_DATE { get; set; }
        public string ESUGAM_NUMBER { get; set; }
        public string ITEM_CATEGORY { get; set; }
        public string PACKING_TYPE { get; set; }
        public string PICKUP_TYPE { get; set; }
        public string RETURN_TYPE { get; set; }
        public string CONSIGNEE_ADDRESS_TYPE { get; set; }
        public string PICKUP_LOCATION_CODE { get; set; }
        public string SELLER_GSTIN { get; set; }
        public string GST_HSN { get; set; }
        public string GST_ERN { get; set; }
        public string GST_TAX_NAME { get; set; }
        public double GST_TAX_BASE { get; set; }
        public double DISCOUNT { get; set; }
        public double GST_TAX_RATE_CGSTN { get; set; }
        public double GST_TAX_RATE_SGSTN { get; set; }
        public double GST_TAX_RATE_IGSTN { get; set; }
        public double GST_TAX_TOTAL { get; set; }
        public double GST_TAX_CGSTN { get; set; }
        public double GST_TAX_SGSTN { get; set; }
        public double GST_TAX_IGSTN { get; set; }
    }

    public class ForwardManifest
    {
        public string AWB_NUMBER { get; set; }
        public string ORDER_NUMBER { get; set; }
        public string PRODUCT { get; set; }
        public string CONSIGNEE { get; set; }
        public string CONSIGNEE_ADDRESS1 { get; set; }
        public string CONSIGNEE_ADDRESS2 { get; set; }
        public string CONSIGNEE_ADDRESS3 { get; set; }
        public string DESTINATION_CITY { get; set; }
        public string PINCODE { get; set; }
        public string STATE { get; set; }
        public string MOBILE { get; set; }
        public string TELEPHONE { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public int PIECES { get; set; }
        public int COLLECTABLE_VALUE { get; set; }
        public double DECLARED_VALUE { get; set; }
        public double ACTUAL_WEIGHT { get; set; }
        public int VOLUMETRIC_WEIGHT { get; set; }
        public int LENGTH { get; set; }
        public int BREADTH { get; set; }
        public int HEIGHT { get; set; }
        public string PICKUP_NAME { get; set; }
        public string PICKUP_ADDRESS_LINE1 { get; set; }
        public string PICKUP_ADDRESS_LINE2 { get; set; }
        public string PICKUP_PINCODE { get; set; }
        public string PICKUP_PHONE { get; set; }
        public string PICKUP_MOBILE { get; set; }
        public string RETURN_NAME { get; set; }
        public string RETURN_ADDRESS_LINE1 { get; set; }
        public string RETURN_ADDRESS_LINE2 { get; set; }
        public string RETURN_PINCODE { get; set; }
        public string RETURN_PHONE { get; set; }
        public string RETURN_MOBILE { get; set; }
        public List<string> ADDONSERVICE { get; set; }
        public string DG_SHIPMENT { get; set; }
        public List<ADDITIONALINFORMATION> ADDITIONAL_INFORMATION { get; set; }
    }

    public class Shipment
    {
        public string reason { get; set; }
        public string order_number { get; set; }
        public string awb { get; set; }
        public bool success { get; set; }
    }

    public class ApiResponse
    {
        public List<Shipment> shipments { get; set; }
    }

    public class NDRData
    {
        public string awb { get; set; }
        public string comments { get; set; }
        public string instruction { get; set; }
        public string scheduled_delivery_date { get; set; }
        public string scheduled_delivery_slot { get; set; }
    }

    public class NDRDataResponse
    {
        public bool status { get; set; }
        public string awb { get; set; }
        public List<string> error { get; set; }
    }



    [XmlRoot(ElementName = "field")]
    public class Field
    {

        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlText]
        public string Text { get; set; }

        [XmlElement(ElementName = "object")]
        public List<Object> Object { get; set; }
    }

    [XmlRoot(ElementName = "object")]
    public class Object
    {

        [XmlElement(ElementName = "field")]
        public List<Field> Field { get; set; }

        [XmlAttribute(AttributeName = "pk")]
        public int Pk { get; set; }

        [XmlAttribute(AttributeName = "model")]
        public string Model { get; set; }

        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "ecomexpress-objects")]
    public class Ecomexpressobjects
    {

        [XmlElement(ElementName = "object")]
        public List<Object> Object { get; set; }

        [XmlAttribute(AttributeName = "version")]
        public double Version { get; set; }

        [XmlText]
        public string Text { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class ADDITIONALINFORMATIONRev
    {
        public object SELLER_TIN { get; set; }
        public string INVOICE_NUMBER { get; set; }
        public string INVOICE_DATE { get; set; }
        public object ESUGAM_NUMBER { get; set; }
        public object ITEM_CATEGORY { get; set; }
        public object PACKING_TYPE { get; set; }
        public object PICKUP_TYPE { get; set; }
        public object RETURN_TYPE { get; set; }
        public string PICKUP_LOCATION_CODE { get; set; }
        public string SELLER_GSTIN { get; set; }
        public object GST_HSN { get; set; }
        public object GST_ERN { get; set; }
        public string GST_TAX_NAME { get; set; }
        public double GST_TAX_BASE { get; set; }
        public double GST_TAX_RATE_CGSTN { get; set; }
        public double GST_TAX_RATE_SGSTN { get; set; }
        public double GST_TAX_RATE_IGSTN { get; set; }
        public double GST_TAX_TOTAL { get; set; }
        public object GST_TAX_CGSTN { get; set; }
        public object GST_TAX_SGSTN { get; set; }
        public object GST_TAX_IGSTN { get; set; }
        public object DISCOUNT { get; set; }
    }

    public class SHIPMENT
    {
        public string AWB_NUMBER { get; set; }
        public string ORDER_NUMBER { get; set; }
        public string PRODUCT { get; set; }
        public string REVPICKUP_NAME { get; set; }
        public string REVPICKUP_ADDRESS1 { get; set; }
        public string REVPICKUP_ADDRESS2 { get; set; }
        public string REVPICKUP_ADDRESS3 { get; set; }
        public string REVPICKUP_CITY { get; set; }
        public string REVPICKUP_PINCODE { get; set; }
        public string REVPICKUP_STATE { get; set; }
        public string REVPICKUP_MOBILE { get; set; }
        public string REVPICKUP_TELEPHONE { get; set; }
        public string PIECES { get; set; }
        public string COLLECTABLE_VALUE { get; set; }
        public string DECLARED_VALUE { get; set; }
        public string ACTUAL_WEIGHT { get; set; }
        public string VOLUMETRIC_WEIGHT { get; set; }
        public string LENGTH { get; set; }
        public string BREADTH { get; set; }
        public string HEIGHT { get; set; }
        public string VENDOR_ID { get; set; }
        public string DROP_NAME { get; set; }
        public string DROP_ADDRESS_LINE1 { get; set; }
        public string DROP_ADDRESS_LINE2 { get; set; }
        public string DROP_PINCODE { get; set; }
        public string DROP_MOBILE { get; set; }
        public string ITEM_DESCRIPTION { get; set; }
        public string DROP_PHONE { get; set; }
        public string EXTRA_INFORMATION { get; set; }
        public string DG_SHIPMENT { get; set; }
        public ADDITIONALINFORMATIONRev ADDITIONAL_INFORMATIONRev { get; set; }
    }

    public class ECOMEXPRESSOBJECTS
    {
        public SHIPMENT SHIPMENT { get; set; }
    }

    public class ReverseManifest
    {
        [JsonProperty("ECOMEXPRESS-OBJECTS")]
        public ECOMEXPRESSOBJECTS ECOMEXPRESSOBJECTS { get; set; }
    }


    public class AIRWAYBILL
    {
        public string success { get; set; }
        public string order_id { get; set; }
        public string airwaybill_number { get; set; }
    }

    public class AIRWAYBILLOBJECTS
    {
        public AIRWAYBILL AIRWAYBILL { get; set; }
    }

    public class RESPONSEOBJECTS
    {
        [JsonProperty("AIRWAYBILL-OBJECTS")]
        public AIRWAYBILLOBJECTS AIRWAYBILLOBJECTS { get; set; }

        [JsonProperty("RESPONSE-COMMENT")]
        public string RESPONSECOMMENT { get; set; }
    }

    public class ReverseManifestResponse
    {
        [JsonProperty("RESPONSE-OBJECTS")]
        public RESPONSEOBJECTS RESPONSEOBJECTS { get; set; }
    }

}
