﻿using System;
using System.Collections;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ComponentArt.Web.UI;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
namespace HSRP
{
    public class Utils
    {

        public string strProvider = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public string sqlText;
        public int CommandTimeOut = 0;
        public SqlConnection objConnection;

        public SqlDataReader GetReader()
        {
            SqlDataReader dr;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = 0;
            MakeConnection();
            OpenConnection();
            cmd = GetCommand();
            cmd.CommandTimeout = 0;
            dr = cmd.ExecuteReader();
            return dr;
        }
        SqlCommand GetCommand()
        {
            return new SqlCommand(sqlText, objConnection);
        }
        public void MakeConnection()
        {

            objConnection = new SqlConnection(strProvider);
           // objConnection.= 200000;
        }
        public void OpenConnection()
        {
            
            
            objConnection.Open();
        }
        public void CloseConnection()
        {
            objConnection.Close();
        }
        public static string ConvertDateTimeToString(DateTime oDatatime)
        {
            string str = "";
            str = oDatatime.Month.ToString() + "/" + oDatatime.Day.ToString() + "/" + oDatatime.Year.ToString() + " " + oDatatime.Hour.ToString() + ":" + oDatatime.Minute.ToString() + ":" + oDatatime.Second.ToString();
            return str;
        }
        public static int getScalarCount(string SQLString, string CnnString)
        {
            int ReturnValue = 0;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);

                conn.Open();
                ReturnValue = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return ReturnValue;
        }

        public static string getScalarValue(string SQLString, string CnnString)
        {

            string ReturnValue = string.Empty;
            using (SqlConnection conn = new SqlConnection(CnnString))
            {
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                conn.Open();
                ReturnValue = cmd.ExecuteScalar().ToString();;
            }
            return ReturnValue;
        }
        //============================ComponentArt Menu===NEW-------------------------------//
        //===============================Menu Component art
        public static void buildMenu(ComponentArt.Web.UI.Menu Menu1, string UserId)
        {
            SqlConnection dbCon = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ToString());
            SqlCommand cmd = new SqlCommand();
            try
            {
                //string TestMenu = "SELECT MenuRelation.ParentMenuID, Menu.MenuName as MenuLabel, Menu.MenuID, Menu.MenuLabel, Menu.MenuNavigateURL, Menu.ClientSideEvent, Menu.IconImage, Menu.IconHoverImage, Menu.MenuLookID, Menu.[Look-RightIconUrl] FROM  Menu ,MenuRelation,UserMenuRelation where Menu.MenuID = MenuRelation.MenuID and MenuRelation.MenuRelationID = UserMenuRelation.MenuRelationID and UserMenuRelation.UserID = '" + UserId + "' order by MenuRelation.MenuOrderBy";
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT MenuRelation.ParentMenuID, Menu.MenuName as MenuLabel, Menu.MenuID, Menu.MenuLabel, Menu.MenuNavigateURL, Menu.ClientSideEvent, Menu.IconImage, Menu.IconHoverImage, Menu.MenuLookID, Menu.[Look-RightIconUrl] FROM  Menu ,MenuRelation,UserMenuRelation where Menu.MenuID = MenuRelation.MenuID and MenuRelation.MenuRelationID = UserMenuRelation.MenuRelationID and UserMenuRelation.UserID = '" + UserId + "' order by MenuRelation.MenuOrderBy", dbCon);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                string s = ds.Tables[0].Columns["MenuID"].ToString();
                string s1 = ds.Tables[0].Columns["ParentMenuID"].ToString();

                ds.Relations.Add("NodeRelation", ds.Tables[0].Columns["MenuID"], ds.Tables[0].Columns["ParentMenuID"]);
                foreach (DataRow dbRow in ds.Tables[0].Rows)
                {
                    if (dbRow.IsNull("ParentMenuID"))
                    {
                        ComponentArt.Web.UI.MenuItem newItem = CreateItem(dbRow, UserId);
                        Menu1.Items.Add(newItem);

                        PopulateSubMenu(dbRow, newItem, UserId);
                    }
                }
                dbCon.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void PopulateSubMenu(DataRow dbRow, ComponentArt.Web.UI.MenuItem item, string UserId)
        {
            try
            {
                foreach (DataRow childRow in dbRow.GetChildRows("NodeRelation"))
                {
                    ComponentArt.Web.UI.MenuItem childItem = CreateItem(childRow, UserId);
                    item.Items.Add(childItem);
                    PopulateSubMenu(childRow, childItem, UserId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ComponentArt.Web.UI.MenuItem CreateItem(DataRow dbRow, string UserId)
        {
            try
            {
                ComponentArt.Web.UI.MenuItem item = new ComponentArt.Web.UI.MenuItem();
                item.Text = dbRow["MenuLabel"].ToString();
                item.ID = dbRow["MenuID"].ToString().Trim();
                item.NavigateUrl = dbRow["MenuNavigateURL"].ToString().Trim();
                item.LookId = dbRow["MenuLookID"].ToString();

                if (dbRow["IconImage"].ToString() != "")
                {
                    item.Look.LeftIconUrl = dbRow["IconImage"].ToString();
                }
                else
                {
                    item.Look.LeftIconUrl = "arrow.gif";
                }

                if (dbRow["ParentMenuID"].ToString() != "")
                {
                    if (dbRow["MenuNavigateURL"].ToString() != "")
                    {
                        item.NavigateUrl = dbRow["MenuNavigateURL"].ToString().Trim();
                    }
                   
                }
                else
                {
                    item.ClientSideCommand = "javascript:void(0);";
                }

                return item;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //===================================================================================//


        public static int ExecNonQuery(string SQLString, string CnnString)
        {
            try
            {
                int count = 0;
                using (SqlConnection connection = new SqlConnection(CnnString))
                {
                    SqlCommand command = new SqlCommand(SQLString, connection);
                    command.Connection.Open();
                    count = command.ExecuteNonQuery();
                    command.Connection.Close();
                }
                return count;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    return ex.Number;
                }
                throw ex;
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }
        public static void PopulateDropDownList(DropDownList DropDownName, string SQLString, string CnnString, string DefaultValue)
        {
            try
            {
                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 0;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();
                DropDownName.DataSource = PReader;
                DropDownName.DataBind();
                ListItem li = new ListItem(DefaultValue, DefaultValue);
                DropDownName.Items.Insert(0, li);
                PReader.Close();
                dbLink.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void DealerNameDropDownList(DropDownList DropDownName, string SQLString, string CnnString, string DefaultValue)
        {
            try
            {
                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 0;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();
                DropDownName.DataSource = PReader;
                DropDownName.DataBind();
                ListItem li = new ListItem(DefaultValue, DefaultValue);
                DropDownName.Items.Insert(0, li);
                PReader.Close();
                dbLink.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void PopulateListBox(ListBox ListBoxName, string SQLString, string CnnString, string DefaultValue)
        {
            try
            {
                Utils dbLink = new Utils();
                dbLink.strProvider = CnnString;
                dbLink.CommandTimeOut = 0;
                dbLink.sqlText = SQLString.ToString();
                SqlDataReader PReader = dbLink.GetReader();
                ListBoxName.DataSource = PReader;
                ListBoxName.DataBind();
                ListBoxName.Items.Add(DefaultValue);
                PReader.Close();
                dbLink.CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataSet getDataSet(string SQLString, string CnnString)
        {
            SqlConnection Conn = new SqlConnection(CnnString);
            SqlDataAdapter DA = new SqlDataAdapter(SQLString, Conn);
            DA.SelectCommand.CommandTimeout = 0;
            Conn.Open();
            DataSet ReturnDs = new DataSet();
            DA.Fill(ReturnDs, "Table1");
            Conn.Close();
            return ReturnDs;
        }
        public static string getDataSingleValue(string SQLString, string CnnString, string colname)
        {
            string SingleValue = "";
            try
            {
                SqlConnection conn = new SqlConnection(CnnString);
                SqlCommand cmd = new SqlCommand(SQLString, conn);
                SqlDataAdapter returnVal = new SqlDataAdapter(SQLString, conn);
                conn.Close();
                conn.Open();
                DataTable dt = new DataTable("CharacterInfo");
                returnVal.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    SingleValue = dt.Rows[0][colname].ToString();
                }
                if (SingleValue == "")
                {
                    SingleValue = "0";
                }
                returnVal.Dispose();
                return SingleValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static DataTable GetDataTable(string SQLString, string CnnString)
        {
            Utils dbLink = new Utils();
            dbLink.strProvider = CnnString.ToString();
            dbLink.CommandTimeOut = 0;
            dbLink.sqlText = SQLString.ToString();
            SqlDataReader dr = dbLink.GetReader();
            DataTable tb = new DataTable();
            tb.Load(dr);
            dr.Close();
            dbLink.CloseConnection();
            return tb;

        }



        //for compression
        public static bool IsGZipSupported()
        {

            string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

            if (!string.IsNullOrEmpty(AcceptEncoding) &&

                 AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate"))

                return true;

            return false;

        }
        public static void GZipEncodePage()
        {

            if (IsGZipSupported())
            {

                HttpResponse Response = HttpContext.Current.Response;



                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

                if (AcceptEncoding.Contains("gzip"))
                {

                    Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,

                                              System.IO.Compression.CompressionMode.Compress);

                    Response.AppendHeader("Content-Encoding", "gzip");

                }

                else
                {
                    Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
                    Response.AppendHeader("Content-Encoding", "deflate");
                }

            }

        }

        public static void user_log(string userid, string formname, string ClientLocalIP, string eventname, string MACAddress, string BrowserName, string ClientOSName, string CnnString)
        {
            string sdate = DateTime.Now.ToString();
            string sql = "INSERT INTO [USERLOG]([UserID],[formname],[eventname],[clientip],[MACAddress],[BrowserName],[ClientOSName]) VALUES ('" + userid + "','" + formname + "','" + eventname + "','" + ClientLocalIP + "','" + MACAddress + "','" + BrowserName + "','" + ClientOSName + "')";
            Utils.ExecNonQuery(sql, CnnString);

        }


        public static void Exception_log(string userid, string Formname, string computername, string exeption, string CnnString)
        {
            string strDate = DateTime.Now.ToString();
            string strQuery = "insert into Exceptionlog(LoginId,FormName,UpdateDateTime,ComputerIP,ExceptionName)values('" + userid + "','" + Formname + "','" + strDate + "','" + computername + "','" + exeption + "')";
            Utils.ExecNonQuery(strQuery, CnnString);
        }
        public static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //>>>> Export to Excel tanuj 24/12/2011
        public static void ExportToSpreadsheet(DataTable table, string name)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            foreach (DataColumn column in table.Columns)
            {
                context.Response.Write(column.ColumnName + ",");
            }
            context.Response.Write(Environment.NewLine);
            foreach (DataRow row in table.Rows)
            {
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    context.Response.Write(row[i].ToString().Replace(",", string.Empty) + ",");
                }
                context.Response.Write(Environment.NewLine);
            }
            context.Response.ContentType = "text/csv";
            context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + name + ".csv");
            context.Response.End();
        }

        //public static System.Boolean IsNumeric(System.Object Expression)
        //{
        //    if (Expression == null || Expression is DateTime)
        //        return false;

        //    if (Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
        //        return true;

        //    try
        //    {
        //        if (Expression is string)
        //            Double.Parse(Expression as string);
        //        else
        //            Double.Parse(Expression.ToString());
        //        return true;
        //    }
        //    catch { } // just dismiss errors but return false
        //    return false;
        //}

        public DataTable GetRecords(string query, SqlParameter[] sqlParameter, SqlConnection con)
        {
            SqlCommand sqlCommand = new SqlCommand(query, con);
            DataTable records = null;
            try
            {
                con.Open();
                records = new DataTable();
                sqlCommand.CommandType = CommandType.StoredProcedure;
                if (sqlParameter != null)
                    sqlCommand.Parameters.AddRange(sqlParameter);
                records.Load(sqlCommand.ExecuteReader());
                con.Close();
            }
            catch (SqlException ex)
            {
                con.Close();
                throw ex;
            }
            catch (Exception ex)
            {
                con.Close();
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return records;
        }
        public static string ValidationVehicleCHassisEngineno(string VehicleRego, string Chassisno, string Engineno)
        {
            string responseJson = null;

            //string url = "http://103.197.122.35/ParivahanVehStatus/VehStatus.aspx?VehRegNo="+VehicleRego+"&ChassisNo="+Chassisno+"&EngineNo="+Engineno+"&X=5UwoklBqiW";
            string url = "http://103.197.122.35/ParivahanVehStatus/VehStatus.aspx?VehRegNo=" + VehicleRego + "&ChassisNo=" + Chassisno + "&EngineNo=" + Engineno + "&X=5UwoklBqiW";
            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseJson = sr.ReadToEnd();
                // more stuff
            }

            String[] splitArr = responseJson.Split(new[] { "lblVehStatusResponse" }, StringSplitOptions.None);
            String[] SplitArr1 = splitArr[1].Split(new[] { "</span>" }, StringSplitOptions.None);
            //string resString = SplitArr1[0].

            responseJson = Regex.Replace(SplitArr1[0], @">", "");
            responseJson = responseJson.Substring(1);

            //string[] rowValues = responseJson.Split(',');
            return responseJson;



        }

        public static string ValidationVehicleCHassisEngineno(string VehicleRego, string Chassisno, string Engineno, string OEMID, string Dealerid)
        {
            string responseJson = null;
            string ParivahanAPI = ConfigurationSettings.AppSettings["ParivahanAPI"].ToString();
            //string url = "http://103.197.122.35/ParivahanVehStatus/VehStatus.aspx?VehRegNo="+VehicleRego+"&ChassisNo="+Chassisno+"&EngineNo="+Engineno+"&X=5UwoklBqiW";
            string url = ParivahanAPI + "CheckHSRPVehicleStatus?VehRegNo=" + VehicleRego + "&ChassisNo=" + Chassisno + "&EngineNo=" + Engineno + "&Oemid=" + OEMID + "&Dealerid=" + Dealerid + "&X=5UwoklBqiW";


            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseJson = sr.ReadToEnd();
                // more stuff
            }


            return responseJson;



        }


        public static string ParivahanVehicleFitmentEntry(string HSRPRecordID, string VehicleRegNo, string ChassisNo, string OrderType, string HSRP_Front_LaserCode, string HSRP_Rear_LaserCode, string fixationDate)
        {
            string responseJson = null;
            string ParivahanAPI = ConfigurationSettings.AppSettings["ParivahanAPI"].ToString();
            //string url = "http://103.197.122.35/ParivahanVehStatus/VehStatus.aspx?VehicleEntry="+VehicleRego+"&ChassisNo="+Chassisno+"&EngineNo="+Engineno+"&X=5UwoklBqiW";


            string url = ParivahanAPI + "VehicleEntry?HSRPRecordID=" + HSRPRecordID + "&VehicleRegNo=" + VehicleRegNo + "&ChassisNo=" + ChassisNo + "&OrderType=" + OrderType + "&HSRP_Front_LaserCode=" + HSRP_Front_LaserCode + "&HSRP_Rear_LaserCode=" + HSRP_Rear_LaserCode + "&fixationDate=" + fixationDate + "&X=5UwoklBqiW";


            HttpWebRequest http = (HttpWebRequest)HttpWebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)http.GetResponse();
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                responseJson = sr.ReadToEnd();
                // more stuff
            }


            return responseJson;



        }


    }
}