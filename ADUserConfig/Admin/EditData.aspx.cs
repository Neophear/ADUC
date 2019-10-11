using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EditData : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblBULKMsg.Visible = false;

        Parameter usernameParameter = new Parameter("Username", System.Data.DbType.String, Membership.GetUser().UserName);
        sdsOUs.DeleteParameters.Add(usernameParameter);
        sdsOUs.UpdateParameters.Add(usernameParameter);
        sdsOUs.InsertParameters.Add(usernameParameter);
    }
    protected void sdsOUs_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        Server.Transfer("EditData.aspx");
    }

    public void SetMessage(string message, bool isError = false)
    {
        lblBULKMsg.Text = message;
        lblBULKMsg.Visible = true;
        lblBULKMsg.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Empty;
    }
    protected void btnBULKUpload_Click(object sender, EventArgs e)
    {
        if (fuBULKFile.HasFile)
        {
            string ext = Path.GetExtension(fuBULKFile.FileName);

            if (ext.ToLower() != ".xlsx")
                SetMessage("Filen er ikke en XLSX-fil", true);
            else
            {
                string localPath = Server.MapPath("~/Files/");

                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                string[] files = Directory.GetFiles(Server.MapPath("~/Files"), "BULK*.xlsx");

                foreach (string oldFile in files)
                    File.Delete(oldFile);

                string filePath = localPath + String.Format("BULK{0}.xlsx", DateTime.Now.ToString("yyyyMMddHHmm"));
                fuBULKFile.SaveAs(filePath);
            }
        }
    }

    protected void gvInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        e.NewValues["Value"] = ((TextBox)gvInfo.Rows[e.RowIndex].FindControl("txtValue")).Text.Replace("\n", "<br />");
    }
}