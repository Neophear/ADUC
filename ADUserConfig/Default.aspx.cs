using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.RouteData.Values["page"] != null)
        {
            string strPage = Page.RouteData.Values["page"].ToString();
            int page = 0;

            if (int.TryParse(strPage, out page))
                dpNews.SetPageProperties((page - 1) * this.dpNews.PageSize, this.dpNews.MaximumRows, false);
        }
    }
    protected void sdsNews_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@IsAdmin"].Value = User.IsInRole("Admin");
    }
    protected void sdsNews_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        dpNews.Visible = !(e.AffectedRows <= 5);
    }
    protected void sdsChangeLog_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@IsAdmin"].Value = User.IsInRole("Admin");
        e.Command.Parameters["@OnlyTop"].Value = true;
    }
}