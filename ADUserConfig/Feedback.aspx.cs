using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Feedback : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Visible = false;
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@Username", User.Identity.Name, System.Data.DbType.String);
        dal.AddParameter("@Text", txtText.Text, System.Data.DbType.String);
        dal.ExecuteNonQuery("EXEC dbo.FeedbackCreate @Username, @Text");
        dal.ClearParameters();

        txtText.Text = String.Empty;
        lblMsg.Text = "Tak for din feedback!";
        lblMsg.Visible = true;
        pnlFeedback.Visible = false;
    }
}