using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        hdlnkCDD.Href = String.Format("~/Content/StyleSheet.css?d={0}", File.GetLastWriteTime(Server.MapPath("~/Content/StyleSheet.css")).ToString("yyyymmddHHMM"));
        DataAccessLayer dal = new DataAccessLayer();

        lblFooter.Text = Utilities.GetInfoFromDBOrCache("SiteFooter");
        hplFeedback.Visible = HttpContext.Current.User.Identity.IsAuthenticated;
        hplFeedbackToRead.Visible = HttpContext.Current.User.Identity.Name == "417317" && dal.ExecuteScalar("SELECT COUNT(*) FROM [Feedback] WHERE [Viewed] = 0").ToString() != "0";
        
        if (Page.User.IsInRole("Admin"))
        {
            int count = (int)dal.ExecuteScalar("SELECT COUNT(*) FROM [BULKs] WHERE [Approved] IS NULL");
            pnlBULKs.Visible = count > 0;
        }
        else
            pnlBULKs.Visible = false;
    }
    protected void btnCreateOrEdit_Click(object sender, EventArgs e)
    {
        string search = ((TextBox)lgnvwHeader.FindControl("txtCreateOrEdit")).Text.Trim();

        if (search != String.Empty)
        {
            if (ADUser.DoesUserExist(search))
                Response.Redirect("~/EditADUser/" + search);
            else
                Response.Redirect("~/CreateADUser/" + search);
        }
    }
}