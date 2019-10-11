using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class public_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Login1_LoginError(object sender, EventArgs e)
    {
        MembershipUser userInfo = Membership.GetUser(Login1.UserName);

        if (userInfo != null && userInfo.IsLockedOut)
            Login1.FailureText = "Brugeren er låst og kan ikke logges på.";
        else
            Login1.FailureText = "Du blev ikke logget ind. Prøv igen.";
    }
}