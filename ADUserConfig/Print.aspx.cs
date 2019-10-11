using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Print : System.Web.UI.Page
{
    protected string footer = String.Empty;
    protected string wifiname = String.Empty;
    protected int counter = 0;
    protected PrintForm form = PrintForm.List;
    string printpage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cache["VoucherFooter"] == null)
        {
            DataAccessLayer dal = new DataAccessLayer();

            dal.AddParameter("@Name", "VoucherFooter", System.Data.DbType.String);
            string footertext = dal.ExecuteScalar("SELECT dbo.GetInfo(@Name)").ToString();
            dal.ClearParameters();

            Cache.Insert("VoucherFooter", footertext, null, DateTime.MaxValue, TimeSpan.FromSeconds(30));
        }

        footer = Cache["VoucherFooter"].ToString();

        if (Cache["WiFiName"] == null)
        {
            DataAccessLayer dal = new DataAccessLayer();

            dal.AddParameter("@Name", "WiFiName", System.Data.DbType.String);
            string wifinametext = dal.ExecuteScalar("SELECT dbo.GetInfo(@Name)").ToString();
            dal.ClearParameters();

            Cache.Insert("WiFiName", wifinametext, null, DateTime.MaxValue, TimeSpan.FromSeconds(30));
        }

        wifiname = Cache["WiFiName"].ToString();

        switch (Page.RouteData.Values["form"].ToString().ToLower())
        {
            case "list":
                form = PrintForm.List;
                break;
            case "a5":
                form = PrintForm.A5;
                break;
            case "single":
                form = PrintForm.Single;
                break;
            default:
                break;
        }

        ChangeForm(form);

        if (form == PrintForm.Single)
        {
            lblSingleUsername.Text = Page.RouteData.Values["username"].ToString();
            lblSinglePassword.Text = Page.RouteData.Values["password"].ToString();
            lblSingleFooter.Text = footer;
            lblWiFiName.Text = wifiname;
        }

        Page.Header.Controls.Add(
            new LiteralControl(@"<style type='text/css'>@media print{@page {size: " + (form == PrintForm.List ? "A4" : "A5") + "}}</style>"));
    }

    private void ChangeForm(PrintForm form)
    {
        rptA4.Visible = form == PrintForm.List;
        rptA5.Visible = form == PrintForm.A5;
        pnlBulk.Visible = form != PrintForm.Single;
        pnlSingle.Visible = form == PrintForm.Single;
    }
    protected void sdsBULK_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string id = Page.RouteData.Values["id"].ToString();

        if (!String.IsNullOrEmpty(id))
            e.Command.Parameters["@ID"].Value = id;

        e.Command.Parameters["@CreatedBy"].Value = User.Identity.Name;
        e.Command.Parameters["@IsAdmin"].Value = User.IsInRole("Admin");
    }

    protected enum PrintForm
    {
        List,
        A5,
        Single
    }
}