using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_BULKdelete : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        txtOutput.Text = "";

        foreach (string line in txtUsersToDelete.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
        {
            string userToDelete = line.Trim().TrimStart('0');
            if (userToDelete != "")
            {
                if (userToDelete.ToLower().Contains("admin"))
                    AddMessage(userToDelete, "Må ikke slettes");
                else
                {
                    CustomError error = ADUser.Delete(userToDelete);

                    if (error.HasErrors)
                        AddMessage(userToDelete, error.ToString());
                    else
                        AddMessage(userToDelete, "Slettet");
                }
            }
        }
    }

    protected void AddMessage(string user, string message)
    {
        txtOutput.Text += String.Format("{0}: {1}\n", user, message);
    }
}