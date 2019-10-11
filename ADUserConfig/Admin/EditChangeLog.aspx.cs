using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_EditChangeLog : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        
    }
    protected void gvChangeLog_SelectedIndexChanged(object sender, EventArgs e)
    {
        dvChangeLog.ChangeMode(DetailsViewMode.Edit);
    }
    protected void dvChangeLog_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
    {
        gvChangeLog.DataBind();
    }
    protected void dvChangeLog_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        gvChangeLog.DataBind();
    }
}