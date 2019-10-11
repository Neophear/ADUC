using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_News : System.Web.UI.Page
{
    protected bool AdminOnly = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        Parameter usernameParameter = new Parameter("Username", System.Data.DbType.String, User.Identity.Name);
        sdsNews.InsertParameters.Add(usernameParameter);
        sdsNews.UpdateParameters.Add(usernameParameter);
        sdsNews.DeleteParameters.Add(usernameParameter);
    }
    protected void dvNews_ItemInserting(object sender, DetailsViewInsertEventArgs e)
    {
        e.Values["Text"] = Utilities.StripHTML(e.Values["Text"].ToString());
    }
    protected void dvNews_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
    {
        e.NewValues["Text"] = Utilities.StripHTML(e.NewValues["Text"].ToString());
    }
    protected void dvNews_DataBound(object sender, EventArgs e)
    {
        if (dvNews.DataItemCount == 1)
            UpdateExample();
    }

    private void UpdateExample()
    {

        lblTitle.Text = DataBinder.Eval(dvNews.DataItem, "Title").ToString();
        lblText.Text = Utilities.BBToHTML(DataBinder.Eval(dvNews.DataItem, "Text").ToString());
        AdminOnly = (bool)DataBinder.Eval(dvNews.DataItem, "AdminOnly");
        lblTimeStamp.Text = Utilities.GetFriendlyTime((DateTime)DataBinder.Eval(dvNews.DataItem, "Timestamp"));
    }
    protected void sdsNews_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        Response.Redirect("~/Admin/News/" + e.Command.Parameters["@NewID"].Value.ToString());
    }
    protected void sdsNews_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        dvNews.DataBind();
        UpdateExample();
    }
    protected void sdsNews_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        Response.Redirect("~/");
    }
    protected void sdsNews_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        string id = Page.RouteData.Values["id"].ToString();

        if (!String.IsNullOrWhiteSpace(id))
        {
            e.Command.Parameters["@ID"].Value = id;
            ChangeMode(DetailsViewMode.Edit);
            ddlNews.SelectedValue = id;
        }
        else
            ChangeMode(DetailsViewMode.Insert);
    }
    protected void sdsNews_Selected(object sender, SqlDataSourceStatusEventArgs e)
    {
        if (Page.RouteData.Values["id"].ToString() != "")
        {
            if (e.AffectedRows == 1)
                ChangeMode(DetailsViewMode.Edit);
            else
                Response.Redirect("~/Admin/News");
        }
        else
            ChangeMode(DetailsViewMode.Insert);
    }
    protected void ChangeMode(DetailsViewMode mode)
    {
        dvNews.ChangeMode(mode);
        pnlShow.Visible = mode == DetailsViewMode.Edit;
    }
    protected void ddlNews_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlNews.SelectedIndex != 0)
            Response.Redirect("~/Admin/News/" + ddlNews.SelectedValue);
        else
            Response.Redirect("~/Admin/News");
    }
}