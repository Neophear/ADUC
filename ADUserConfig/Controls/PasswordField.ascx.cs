using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PasswordField : UserControl
{
    private RandomPassword rndPassword = new RandomPassword(1, -1, 1, 1, 0, 0, 1, 2);
    public string Password { get { return txtPassword.Text; } set { txtPassword.Text = value; } }

    /// <summary>
    /// ClientID's of the textboxes which contain names. Fx. txtFirstname.ClientID. This MUST be set to work
    /// </summary>
    public string TextBoxWithAccount { get; set; }

    //protected string txtbxNamesToJavaArray()
    //{
    //    string result = "";

    //    foreach (string name in TextBoxesWithName)
    //        result += String.Format("'{0}', ", name);

    //    return result.Remove(result.Length - 2);
    //}

    public string ValidationGroup { get { return rfvPassword.ValidationGroup; } set { rfvPassword.ValidationGroup = value; } }
    
    public string txtClientID { get { return txtPassword.ClientID; } }

    private bool enabled = true;
    public bool Enabled
    {
        get { return enabled; }
        set
        {
            btnGeneratePassword.Enabled = txtPassword.Enabled = enabled = value;

            if (!value)
                Password = String.Empty;
            else
                GeneratePassword();
        }
    }

    public bool IsPasswordGenerated
    {
        get { return Convert.ToString(ViewState["generatedPassword"] ?? "") == txtPassword.Text; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void AddOnKeyUpJava(string java)
    {
        txtPassword.Attributes["OnKeyUp"] += java;
    }

    protected void btnGeneratePassword_Click(object sender, EventArgs e)
    {
        GeneratePassword();
    }

    public void GeneratePassword()
    {
        string password = rndPassword.GeneratePassword(10);

        ViewState["generatedPassword"] = password;
        txtPassword.Text = password;
        txtPassword.Attributes["data-password"] = password;
    }
}