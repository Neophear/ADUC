using Stiig;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditADUser : Page
{
    ADUser user;
    static string mailPassword;
    static int lstItemCount = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Text = String.Empty;
        hplPrint.Visible = false;
        pwfPassword.TextBoxWithAccount = txtMANR.ClientID;
        lblInfo.Text = Utilities.GetInfoFromDBOrCache("EditUserInfo");

        if (!IsPostBack)
        {
            string u = Page.RouteData.Values["username"].ToString().Trim();

            if (!String.IsNullOrWhiteSpace(u))
            {
                try
                {
                    user = ADUser.Find(u);

                    if (user != null)
                    {
                        ViewState["MANR"] = user.AccountName;
                        txtName.Text = user.FullName;
                        chkbxUnlock.Checked = user.Locked;
                        int count = 0;
                        foreach (var item in user.Groups)
                        {
                            //txtGroups.Text += item;
                            //txtGroups.Text += Environment.NewLine;
                            lstGroups.Items.Add(item);
                            lstGroups.Items[count].Selected = true;
                            lstGroups.Items[count].Attributes["Title"] = item;
                            count++;
                        }

                        foreach (var item in ADUser.GetGroups())
                        {
                            if (!user.Groups.Contains(item.Name))
                            {
                                lstGroups.Items.Add(item.Name);
                                lstGroups.Items[count].Attributes["Title"] = item.Name;
                                count++;
                            }
                        }
                        lstItemCount = count;
                        EnableFields(true);
                    }
                    else
                    {
                        ViewState["MANR"] = null;
                        EnableFields(false);

                        SetMessage("Brugeren blev ikke fundet", true);
                    }
                }
                catch (ADUser.NoAccessToADUserException)
                {
                    ViewState["MANR"] = null;
                    EnableFields(false);
                    SetMessage("Ingen adgang til denne bruger", true);
                    txtMANR.Focus();
                }

                txtMANR.Text = u;
            }
            else
                txtMANR.Focus();
        }
        else
        {
            //Tooltip forsvinder åbenbart hvis det ikke er postback
            if (lstGroups.Items.Count != 0)
                for (int i = 0; i < lstItemCount; i++)
                    lstGroups.Items[i].Attributes["Title"] = lstGroups.Items[i].Value;
        }
    }

    public void SetMessage(string message, bool isError = false)
    {
        lblMessage.Text = message + "<br />";
        lblMessage.Visible = true;
        lblMessage.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Empty;
    }

    private void EnableFields(bool enable)
    {
        if (!enable)
        {
            txtName.Text = String.Empty;
            pwfPassword.Password = String.Empty;
        }
        else
        {
            if (user.LastLogon != null && user.LastLogon > DateTime.Today.AddDays(-30))
            {
                lblPasswordWarning.Visible = true;
                lblPasswordWarning.Text = String.Format("Brugeren har været logget på en INET-PC inden for den sidste måned<br />");
            }
            else
            {
                lblPasswordWarning.Visible = false;
            }

            pwfPassword.GeneratePassword();

            if (user.DateExpires.HasValue)
            {
                txtExpires.Text = user.DateExpires.Value.ToString("dd-MM-yyyy");
                txtExpires.CssClass = "warning";
            }
            else
            {
                txtExpires.Text = String.Empty;
                txtExpires.CssClass = "";
            }

            lblLockedStatus.Text = user.Locked ? "Låst" : "Ikke låst";
            lblEnabledStatus.Text = user.Enabled ? "Aktiv" : "Deaktiveret";
            chkbxUnlock.Enabled = chkbxUnlock.Checked = user.Locked;
        }

        pnlEdit.Visible = enable;

        if (enable && User.IsInRole("Admin"))
        {
            txtOU.Text = user.OU.Replace(",", ",\n");
            pwfPassword.AddOnKeyUpJava("checkchkbx(this);");
        }

        pnlAdminOnly.Visible = enable && User.IsInRole("Admin");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EditADUser/" + txtMANR.Text, true);
    }

    private void ChangePassword()
    {
        string newPassword = pwfPassword.Password;
        bool mustChangePass = User.IsInRole("Admin") ? chkbxForceChangePassword.Checked : !pwfPassword.IsPasswordGenerated;
        string MANR = ViewState["MANR"].ToString();

        CustomError userErrors = ADUser.ChangePassword(MANR, newPassword, mustChangePass, pwdGenerated: pwfPassword.IsPasswordGenerated);

        if (!userErrors.HasErrors)
        {
            pwfPassword.GeneratePassword();
            mailPassword = newPassword;
            SetMessage(String.Format("Password ændret til: {0}", newPassword));
            hplPrint.NavigateUrl = String.Format("~/Print/{0}/{1}", MANR, newPassword);
            hplPrint.Visible = true;
            txtMANR.Focus();
            chkbxChangePassword.Checked = false;
            chkbxForceChangePassword.Checked = false;
        }
        else
            SetMessage(userErrors.ToString(), true);
    }

    private void SetExpired()
    {
        string MANR = ViewState["MANR"].ToString();

        DateTime? expires = null;
        if (!String.IsNullOrWhiteSpace(txtExpires.Text))
        {
            DateTime dt;
            if (DateTime.TryParseExact(txtExpires.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                expires = dt;
        }

        ADUser.SetExpires(MANR, expires);

        chkbxExpires.Checked = false;
    }

    private void MoveOU()
    {
        if (User.IsInRole("Admin") && chkbxMoveOU.Checked && ddlOU.SelectedIndex != 0)
        {
            string MANR = ViewState["MANR"].ToString();
            CustomError errors = ADUser.Move(MANR, ddlOU.SelectedValue, ddlOU.SelectedItem.Text);

            if (!errors.HasErrors)
            {
                //SetMessage("Bruger flyttet til " + ddlOU.SelectedItem.Text);
                txtOU.Text = ddlOU.SelectedValue.Replace(",", ",\n");
                ddlOU.SelectedIndex = 0;
                chkbxMoveOU.Checked = false;
                cvOU.Validate();
            }
            else
            {
                CustomValidator cv = new CustomValidator();
                cv.ValidationGroup = "Edit";

                switch (errors.ErrorList[0])
                {
                    case CustomError.ErrorType.NoAccess:
                        cv.ErrorMessage = "Ingen adgang til brugeren";
                        break;
                    case CustomError.ErrorType.UserDoesNotExist:
                        cv.ErrorMessage = "Brugeren findes ikke";
                        break;
                    case CustomError.ErrorType.OUDoesNotExist:
                        cv.ErrorMessage = "Der skete en fejl. Muligvis findes OU'en ikke.";
                        break;
                }

                Page.Validators.Add(cv);
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string MANR = ViewState["MANR"].ToString();
        user = ADUser.Find(MANR);
        if (chkbxChangePassword.Checked)
            ChangePassword();

        if (chkbxUnlock.Checked)
        {
            ADUser.Unlock(MANR);
            chkbxUnlock.Checked = false;
            chkbxUnlock.Enabled = false;
            lblLockedStatus.Text = "Ikke låst";
        }

        List<string> groups = new List<string>();
        List<string> removeFromGroups = new List<string>();

        for (int i = 0; i < lstItemCount; i++)
        {
            if (lstGroups.Items[i].Selected)
            {
                groups.Add(lstGroups.Items[i].Value);
            }
            else
            {
                removeFromGroups.Add(lstGroups.Items[i].Value);
            }
        }

        //if (user.Groups.Count != groups.Count)
        //{
            ADUser.AddUserToGroup(MANR, groups, removeFromGroups);
        //}

        if (chkbxExpires.Checked)
            SetExpired();

        if (chkbxMoveOU.Checked)
            MoveOU();
    }

    protected void cvOU_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (chkbxMoveOU.Checked)
            args.IsValid = args.Value == "0";
    }

    protected void cvPassword_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (chkbxChangePassword.Checked)
            args.IsValid = !Utilities.CheckPassword(pwfPassword.Password, txtMANR.Text).HasErrors;
    }

    protected void cvExpires_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (chkbxExpires.Checked)
        {
            DateTime date;
            args.IsValid = String.IsNullOrWhiteSpace(txtExpires.Text) || DateTime.TryParseExact(txtExpires.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date) && date.Date >= DateTime.Today;
        }
    }

    protected void btnDeleteUser_Click(object sender, EventArgs e)
    {
        string MANR = ViewState["MANR"].ToString();
        CustomError errors = ADUser.Delete(MANR);

        if (!errors.HasErrors)
        {
            SetMessage(String.Format("{0} slettet!", MANR));
            EnableFields(false);
        }
        else
            SetMessage(errors.ToString());
    }
}