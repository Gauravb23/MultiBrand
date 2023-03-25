using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace HSRP.Report
{
    public partial class ViewInvoices : System.Web.UI.Page
    {
        string SQLString = string.Empty;
        string CnnString = string.Empty;
        string HSRP_StateID = string.Empty, RTOLocationID = string.Empty;
        int UserType;
        string strUserID = string.Empty;
        string HSRPStateID = string.Empty;
        string oemid = string.Empty;
        string dealerid = string.Empty;
        string CnnStringupload = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        string ImgoemECUrl = string.Empty;
        string ImgoemCodoUrl = string.Empty;
        DataTable datatable =null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["UID"] == null)
                {
                    Response.Redirect("~/Login.aspx", true);
                }
                else
                {
                    HSRPStateID = Session["UserHSRPStateID"].ToString();
                    UserType = Convert.ToInt32(Session["UserType"].ToString());
                    CnnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                    strUserID = Session["UID"].ToString();
                    oemid = Session["oemid"].ToString();
                    dealerid = Session["dealerid"].ToString();
                    if (!IsPostBack)
                    {
                        grdDealerOrderSummary();
                    }
                }
            }
            catch (Exception err)
            {
                lblErrMsg.Text = "Error on Page Load" + err.Message.ToString();
            }
        }

        private void InitialSetting()
        {
            string TodayDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            string MaxDate = System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString();
            //
            HSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            HSRPAuthDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarHSRPAuthDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarHSRPAuthDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            //
            OrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            OrderDate.MaxDate = DateTime.Parse(MaxDate);
            CalendarOrderDate.SelectedDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
            CalendarOrderDate.VisibleDate = (DateTime.Parse(TodayDate)).AddDays(0.00);
        }

        protected void btnGO_Click(object sender, ImageClickEventArgs e)
        {
            grdDealerOrderSummary();
        }

        protected void grdDealerOrderSummary()
        {
            try
            {
                ViewState["dtAfixationData"] = null;
                ViewState["dtAffixationWithImageUrl"] = null;
                string SQLString = string.Empty;

                lblErrMsg.Visible = false;
                SqlConnection con = new SqlConnection(CnnString);
                SqlCommand cmd = new SqlCommand("ViewInvoiceOnDealerPortal20082021", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@dealerid", dealerid);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //dt = Utils.GetDataTable(SQLString, CnnString);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                //da.Fill(dt);

                DataColumn dcImageUrl = new DataColumn("ImageUrl");
                dcImageUrl.ReadOnly = false;
                dt.Columns.Add(dcImageUrl);
                da.Fill(dt);
                ViewState["dtInvoice"] = dt;
                grddealers.DataSource = dt;
                grddealers.DataBind();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
                lblErrMsg.Visible = true;
            }
        }

        protected void grddealers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grddealers.PageIndex = e.NewPageIndex;
            grdDealerOrderSummary();
            grddealers.DataBind();
            grddealers.Visible = true;
        }

        protected void lnkPreview_Click(object sender, EventArgs e)
        {
            string url = "";
            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;
            LinkButton gvimgBtn = row.FindControl("lnkPreview") as LinkButton;
            string gvimgurl = gvimgBtn.CommandArgument;
            url = gvimgurl;
            if (url != "")
            {
                string s = "window.open('" + url + "', 'popup_window', 'width=800,height=600,left=100,top=100,resizable=yes');";
                ClientScript.RegisterStartupScript(this.GetType(), "script", s, true);
            }

        }

        /*protected void LinkButtonDownloadPdf_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer as GridViewRow;
                int row = grdrow.RowIndex;
                Label lblfile = (Label)grddealers.Rows[row].FindControl("lblFileName");
                string filename = lblfile.Text;
                Label lblInvoiceNo = (Label)grddealers.Rows[row].FindControl("lblInvoiceNo");
                string invoiceno = lblInvoiceNo.Text;
                string invoicepath = System.Configuration.ConfigurationManager.AppSettings["InvoicePath"].ToString();
                Response.ContentType = "Application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + invoiceno + ".pdf");
                Response.TransmitFile(invoicepath + filename);
                Response.End();
            }
            catch (Exception ex)
            {
                lblErrMsg.Text = ex.Message.ToString();
            }
        }*/


        protected string PreviewImage(string ImgParam)
        {
           
         
            ImgoemECUrl = ConfigurationManager.AppSettings["ImgoemECUrl"].ToString();
            ImgoemCodoUrl = ConfigurationManager.AppSettings["ImgoemCodoUrl"].ToString();
            if (ViewState["dtInvocieImageUrl"] == null)
            {
                datatable = (DataTable)ViewState["dtInvoice"];
            }

            
            string imageUrl = ""; lblErrMsg.Text = String.Empty; ViewState["dtAffixationWithImageUrl"] = null;

            string path = "";
            if (!string.IsNullOrEmpty(ImgParam))
            {
             
                int Salesinvoiceid = 0;
                string navEmbId = "";
                string filename = "";
                string[] imageparm = ImgParam.Split('^');
                if (imageparm[0] != "")
                {
                    filename = imageparm[0].ToString();
                }
                if (imageparm[1] != "")
                {
                    Salesinvoiceid = Convert.ToInt32(imageparm[1]);
                }
                if (imageparm[2] != "")
                {
                    navEmbId = imageparm[2];
                }
                



                if (navEmbId.ToUpper().Contains("CODO") == true)
                {

                    if ((ImgParam != null) && (ImgParam != ""))
                    {

                        path = ImgoemCodoUrl + filename;

                        DataRow dr = datatable.AsEnumerable().Where(r => (r["Salesinvoiceid"]).Equals(Salesinvoiceid)).First();

                       // DataRow dr = datatable.AsEnumerable().Where(r => (r["hsrprecordid"]).Equals(hsrprecordid)).First();
                        //dr["ImageParam"] = imageUrl;



                        dr["ImageParam"] = path;

                    }
                }
                else
                {
                    path = ImgoemECUrl + filename;
                    DataRow dr = datatable.AsEnumerable().Where(r => (r["Salesinvoiceid"]).Equals(Salesinvoiceid)).First();

                    dr["ImageParam"] = path;

                }
                ViewState["dtInvoice"] = datatable;
                ViewState["dtInvocieImageUrl"] = datatable;
            }
            return path;
        }

        protected void grddealers_DataBound(object sender, GridViewRowEventArgs e)
        {

            DataTable dt = null;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkPreview = (LinkButton)e.Row.FindControl("lnkPreview");

                string StrPreview = lnkPreview.CommandArgument;
                if (StrPreview != string.Empty)
                {
                    lnkPreview.Text = "DownLoad";
                }
            }


        }

        protected void btnback_Click(object sender, EventArgs e)
        {
            Response.Redirect("../LiveReports/LiveTracking.aspx");
        }
    }
}