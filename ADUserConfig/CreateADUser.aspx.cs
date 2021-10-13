using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class CreateADUser : Page
{
    static string mailPassword;
    static string mailUsername;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMessage.Visible = false;
        hplPrint.Visible = false;
        pwfPassword.TextBoxWithAccount = txtMANR.ClientID;
        lblInfo.Text = Utilities.GetInfoFromDBOrCache("CreateUserInfo");

        if (!IsPostBack)
        {
            string u = Page.RouteData.Values["username"].ToString();

            if (!String.IsNullOrWhiteSpace(u))
                txtMANR.Text = u;

            pwfPassword.GeneratePassword();
            pwfPassword.AddOnKeyUpJava("checkchkbx(this);");
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        CustomError passwordError = Utilities.CheckPassword(pwfPassword.Password, txtMANR.Text);

        if (!passwordError.HasErrors)
        {
            ADUser user = new ADUser(Utilities.RemoveExcessWhitespaces(txtMANR.Text.ToUpper()), Utilities.RemoveExcessWhitespaces(txtFirstname.Text), Utilities.RemoveExcessWhitespaces(txtLastname.Text), pwfPassword.Password, ddlOU.SelectedValue);

            DateTime? expires = null;
            if (!String.IsNullOrWhiteSpace(txtExpires.Text))
            {
                DateTime dt;
                if (DateTime.TryParseExact(txtExpires.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
                    expires = dt;
            }

            user.DateExpires = expires;

            bool mustChangePassword = User.IsInRole("Admin") ? chkbxForceChangePassword.Checked : !pwfPassword.IsPasswordGenerated;

            CustomError userError = user.Save(mustChangePassword, false);

            if (!userError.HasErrors)
            {
                txtMANR.Text = String.Empty;
                txtFirstname.Text = String.Empty;
                txtLastname.Text = String.Empty;
                txtExpires.Text = String.Empty;
                ddlOU.SelectedIndex = 0;
                pwfPassword.GeneratePassword();
                SetMessage(String.Format("<a href=\"/EditADUser/{0}\">{0}</a> oprettet med password: {1}", user.AccountName, user.Password));
                hplPrint.NavigateUrl = String.Format("~/Print/{0}/{1}", user.AccountName, user.Password);
                hplPrint.Visible = true;
                mailPassword = user.Password;
                mailUsername = user.AccountName;
            }
            else
            {
                switch (userError.ErrorList[0])
                {
                    case CustomError.ErrorType.UserExists:
                        SetMessage("Brugeren eksisterer i forvejen", true);
                        break;
                    case CustomError.ErrorType.OUDoesNotExist:
                        SetMessage("Der skete en fejl med Enhed. Venligst kontakt IT og oplys hvilken enhed du benyttede.", true);
                        break;
                    case CustomError.ErrorType.UnknownError:
                        SetMessage("Der skete en ukendt fejl. Prøv igen og hvis fejlen fortsætter, kontakt da os.", true);
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            switch (passwordError.ErrorList[0])
            {
                case CustomError.ErrorType.PasswordLessThan8:
                    SetMessage("Password skal være på minimum 8 tegn", true);
                    break;
                case CustomError.ErrorType.PasswordIncludesAccount:
                    SetMessage("Password må ikke indeholde dele af navnet", true);
                    break;
                case CustomError.ErrorType.PasswordNotComplex:
                    SetMessage("Passwordet opfylder ikke komplexitetskravene");
                    break;
                default:
                    break;
            }
        }
    }

    public void SetMessage(string message, bool isError = false)
    {
        lblMessage.Text = message + "<br />";
        lblMessage.Visible = true;
        lblMessage.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Empty;
    }

    protected void cvExpires_ServerValidate(object source, ServerValidateEventArgs args)
    {
        DateTime date;
        args.IsValid = String.IsNullOrWhiteSpace(txtExpires.Text) || DateTime.TryParseExact(txtExpires.Text, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date) && date.Date >= DateTime.Today;
    }
}