using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_Log : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtDateFrom.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtDateTo.Text = DateTime.Today.ToString("yyyy-MM-dd");

            sdsLog.SelectParameters["DateFrom"] = new Parameter("DateFrom", System.Data.DbType.Date, DateTime.Today.ToShortDateString());
            sdsLog.SelectParameters["DateTo"] = new Parameter("DateTo", System.Data.DbType.Date, DateTime.Today.ToShortDateString());
            sdsLog.SelectParameters["CategoryID"] = new Parameter("CategoryID", System.Data.DbType.Int32, "");
            sdsLog.SelectParameters["Username"] = new Parameter("Username", System.Data.DbType.String, String.Empty);
            sdsLog.SelectParameters["SearchTerm"] = new Parameter("SearchTerm", System.Data.DbType.String, String.Empty);
            DataAccessLayer dal = new DataAccessLayer();

            ddlTypes.DataSource = dal.ExecuteDataTable("SELECT * FROM [LogAction]");
            ddlTypes.DataValueField = "ID";
            ddlTypes.DataTextField = "Description";
            ddlTypes.DataBind();
        }
    }

    protected void btnLoadLog_Click(object sender, EventArgs e)
    {
        string searchTerm = txtSearchTerm.Text;

        if (chkbxWithDates.Checked)
        {
            DateTime dateFrom = DateTime.Parse(txtDateFrom.Text);
            DateTime dateTo = DateTime.Parse(txtDateTo.Text);

            txtDateFrom.Text = dateFrom.ToString("yyyy-MM-dd");
            txtDateTo.Text = dateTo.ToString("yyyy-MM-dd");

            if (dateFrom > dateTo)
                ShowError("Slutdatoen kan ikke komme før startdatoen");
            else
            {
                sdsLog.SelectParameters["DateFrom"] = new Parameter("DateFrom", System.Data.DbType.Date, dateFrom.ToShortDateString());
                sdsLog.SelectParameters["DateTo"] = new Parameter("DateTo", System.Data.DbType.Date, dateTo.ToShortDateString());
            }
        }
        else
        {
            sdsLog.SelectParameters["DateFrom"] = new Parameter("DateFrom", System.Data.DbType.Date, null);
            sdsLog.SelectParameters["DateTo"] = new Parameter("DateTo", System.Data.DbType.Date, null);
        }

        sdsLog.SelectParameters["CategoryID"] = new Parameter("CategoryID", System.Data.DbType.Int32, ddlTypes.SelectedValue);
        sdsLog.SelectParameters["Username"] = new Parameter("Username", System.Data.DbType.String, chkbxOnlyCurrentUser.Checked ? User.Identity.Name : String.Empty);
        sdsLog.SelectParameters["SearchTerm"] = new Parameter("SearchTerm", System.Data.DbType.String, searchTerm);
    }

    private void ShowError(string msg)
    {
        lblError.Text = msg;
        lblError.Visible = true;
    }

    protected void gvLog_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Logging.Action action = (Logging.Action)DataBinder.Eval(e.Row.DataItem, "ActionID");
            string user = DataBinder.Eval(e.Row.DataItem, "Username").ToString();

            string color = "";
            bool strong = user.Equals(User.Identity.Name);

            switch (action)
            {
                case Logging.Action.Create:
                    color = strong ? "#99FF99" : "#E6FFE6";//"#CCFFCC";
                    break;
                case Logging.Action.Delete:
                    color = strong ? "#FF9999" : "#FFE6E6";//"#FFCCCC";
                    break;
                case Logging.Action.Unlock:
                    color = strong ? "#FFFF99" : "#FFFFE6";//"#FFFFCC";
                    break;
                case Logging.Action.ChangePassword:
                    color = strong ? "#FF99C2" : "#FFE6F0";//"#FFCCE0";
                    break;
                case Logging.Action.Edit:
                    color = strong ? "#9999FF" : "#E6E6FF";//"#CCCCFF";
                    break;
                case Logging.Action.Expires:
                    color = strong ? "#CCCCCC" : "#F2F2F2";
                    break;
                case Logging.Action.Mail:
                    color = strong ? "#FF9B44" : "#F9C598";
                    break;
                default:
                    color = "#FFFFFF";
                    break;
            }

            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(color);
        }
    }
}