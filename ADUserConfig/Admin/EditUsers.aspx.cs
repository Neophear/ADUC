using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_EditUsers : System.Web.UI.Page
{
    private RandomPassword rndPassword = new RandomPassword(1, -1, 1, 1, 0, 0, 1, 2);
    
    protected void Page_Load(object sender, EventArgs e)
    {
        lblEditMsg.Visible = false;
        
        if (!IsPostBack)
        {
            //MembershipUserCollection allUsers = Membership.GetAllUsers();
            MembershipUserCollection allUsersWithoutCurrent = Membership.GetAllUsers();
            allUsersWithoutCurrent.Remove(User.Identity.Name);

            List<ADUCUser> list = ADUCUser.GetList(allUsersWithoutCurrent, Profile);
            ddlEditUser.DataSource = list;
            ddlEditUser.DataBind();

            ((TextBox)cuwCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("Password")).Text = rndPassword.GeneratePassword(10);
        }

        if (HttpContext.Current.Items["username"] != null && HttpContext.Current.Items["password"] != null)
        {
            string username = HttpContext.Current.Items["username"].ToString();
            string password = HttpContext.Current.Items["password"].ToString();

            tcUserManagement.ActiveTabIndex = 1;
            cuwCreateUser.ActiveStepIndex = 1;
            cuwCreateUser.CompleteSuccessText = String.Format("{0} oprettet med password: {1}", username, password); ;
        }
    }

    protected void ddlEditUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEditUser.SelectedIndex != 0)
        {
            ADUCUser user = new ADUCUser(Membership.GetUser(ddlEditUser.SelectedValue), Profile.GetProfile(ddlEditUser.SelectedValue));
            EnableEditFields(true, user.Firstname, user.Lastname, user.IsApproved, Roles.IsUserInRole(user.MANR, "Admin"), user.IsLockedOut);
        }
        else
            EnableEditFields(false);
    }

    protected void btnEditUser_Click(object sender, EventArgs e)
    {
        string username = ddlEditUser.SelectedValue;
        string firstname = txtEditFirstname.Text;
        string lastname = txtEditLastname.Text;
        string password = txtEditPassword.Text;
        bool admin = chkbxEditAdmin.Checked;
        bool enabled = chkbxEditEnabled.Checked;
        MembershipUser user = Membership.GetUser(username);
        ProfileCommon profile = Profile.GetProfile(username);
        
        profile.Firstname = firstname;
        profile.Lastname = lastname;
        profile.Save();

        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@Username", username, System.Data.DbType.String);
        dal.AddParameter("@Firstname", firstname, System.Data.DbType.String);
        dal.AddParameter("@Lastname", lastname, System.Data.DbType.String);
        dal.AddParameter("@By", Membership.GetUser().UserName, System.Data.DbType.String);
        dal.ExecuteNonQuery("EXEC dbo.UserDetailsUpdate @Username, @Firstname, @Lastname, @By");
        dal.ClearParameters();

        UpdateUserAdmin(username, admin);

        if (user.IsApproved != enabled)
        {
            user.IsApproved = enabled;
            Membership.UpdateUser(user);

            Logging.WriteLog(Logging.Action.Edit, Logging.ObjectType.ADUCUser, username, enabled ? "Enabled" : "Disabled");
        }

        if (chkbxEditPassword.Checked)
        {
            if (user.IsLockedOut)
                user.UnlockUser();

            btnEditUnlock.Visible = false;

            user.ChangePassword(user.ResetPassword(), txtEditPassword.Text);
            Logging.WriteLog(Logging.Action.ChangePassword, Logging.ObjectType.ADUCUser, user.UserName);
        }

        SetMessage(String.Format("{0} blev opdateret{1}", username, (chkbxEditPassword.Checked ? " med password " + txtEditPassword.Text : String.Empty)));
    }

    private void EnableEditFields(bool enable, string firstname = "", string lastname = "", bool isEnabled = false, bool isAdmin = false, bool isLocked = false)
    {
        txtEditFirstname.Enabled = enable;
        txtEditLastname.Enabled = enable;
        txtEditPassword.Enabled = enable;
        chkbxEditAdmin.Enabled = enable;
        chkbxEditEnabled.Enabled = enable;
        btnEditDelete.Enabled = enable;
        btnEditUser.Enabled = enable;
        chkbxEditPassword.Enabled = enable;
        chkbxEditPassword.Checked = false;
        btnEditUnlock.Visible = isLocked;

        txtEditFirstname.Text = firstname;
        txtEditLastname.Text = lastname;
        chkbxEditAdmin.Checked = isAdmin;
        chkbxEditEnabled.Checked = isEnabled;
        txtEditPassword.Text = enable ? rndPassword.GeneratePassword(10) : String.Empty;
    }

    private void UpdateUserAdmin(string username, bool setAdmin)
    {
        if (Roles.IsUserInRole(username, "Admin") && !setAdmin)
        {
            Roles.RemoveUserFromRole(username, "Admin");
            Logging.WriteLog(Logging.Action.Edit, Logging.ObjectType.ADUCUser, username, "Removed Admin");
        }
        else if (!Roles.IsUserInRole(username, "Admin") && setAdmin)
        {
            Roles.AddUserToRole(username, "Admin");
            Logging.WriteLog(Logging.Action.Edit, Logging.ObjectType.ADUCUser, username, "Added Admin");
        }
    }

    private void SetMessage(string message, bool isError = false)
    {
        lblEditMsg.Text = message;
        lblEditMsg.Visible = true;
        lblEditMsg.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Empty;
    }

    protected void cuwCreateUser_CreatedUser(object sender, EventArgs e)
    {
        ProfileCommon userProfile = Profile.GetProfile(cuwCreateUser.UserName);
        string firstname = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtFirstname")).Text;
        string lastname = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtLastname")).Text;
        userProfile.Firstname = firstname;
        userProfile.Lastname = lastname;

        DataAccessLayer dal = new DataAccessLayer();

        dal.AddParameter("@Username", cuwCreateUser.UserName.Trim(), System.Data.DbType.String);
        dal.AddParameter("@Firstname", firstname, System.Data.DbType.String);
        dal.AddParameter("@Lastname", lastname, System.Data.DbType.String);
        dal.AddParameter("@By", Membership.GetUser().UserName, System.Data.DbType.String);
        dal.ExecuteNonQuery("EXEC dbo.UserDetailsUpdate @Username, @Firstname, @Lastname, @By");
        dal.ClearParameters();
        userProfile.Save();

        Logging.WriteLog(Logging.Action.Create, Logging.ObjectType.ADUCUser, cuwCreateUser.UserName);

        HttpContext.Current.Items["username"] = cuwCreateUser.UserName.Trim();
        HttpContext.Current.Items["password"] = ((TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("Password")).Text;
        Server.Transfer("EditUsers.aspx");
    }
    protected void btnEditDelete_Click(object sender, EventArgs e)
    {
        string username = ddlEditUser.SelectedValue;
        Membership.DeleteUser(username);
        Logging.WriteLog(Logging.Action.Delete, Logging.ObjectType.ADUCUser, username);

        Server.Transfer("EditUsers.aspx");
    }
    protected void btnEditUnlock_Click(object sender, EventArgs e)
    {
        string username = ddlEditUser.SelectedValue;
        Membership.GetUser(username).UnlockUser();
        Logging.WriteLog(Logging.Action.Unlock, Logging.ObjectType.ADUCUser, username);

        btnEditUnlock.Visible = false;
    }
    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            bool isAdmin = (bool)DataBinder.Eval(e.Row.DataItem, "IsAdmin");
            bool isApproved = (bool)DataBinder.Eval(e.Row.DataItem, "IsApproved");

            if (!isApproved)
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCCCC");
            else if (isAdmin)
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCFFCC");
        }
    }
    protected void lnkbtnShowUsers_Click(object sender, EventArgs e)
    {
        gvUsers.DataSource = ADUCUser.GetList(Membership.GetAllUsers(), Profile);
        gvUsers.DataBind();
        lnkbtnShowUsers.Visible = false;
    }
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }
    protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        //re-run the query, use linq to sort the objects based on the arg.
        //perform a search using the constraints given 
        //you could have this saved in Session, rather than requerying your datastore
        List<ADUCUser> users = ADUCUser.GetList(Membership.GetAllUsers(), Profile);

        if (users != null)
        {
            var param = Expression.Parameter(typeof(ADUCUser), e.SortExpression);
            var sortExpression = Expression.Lambda<Func<ADUCUser, object>>(Expression.Convert(Expression.Property(param, e.SortExpression), typeof(object)), param);

            if (GridViewSortDirection == SortDirection.Ascending)
            {
                gvUsers.DataSource = users.AsQueryable<ADUCUser>().OrderBy(sortExpression);
                GridViewSortDirection = SortDirection.Descending;
            }
            else
            {
                gvUsers.DataSource = users.AsQueryable<ADUCUser>().OrderByDescending(sortExpression);
                GridViewSortDirection = SortDirection.Ascending;
            }

            gvUsers.DataBind();
        }
    }
}