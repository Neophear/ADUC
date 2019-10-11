using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_BULKs : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void gvBULKS_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            object objApproved = DataBinder.Eval(e.Row.DataItem, "Approved");
            bool? approved = objApproved == DBNull.Value ? null : (bool?)objApproved;

            if (approved == false)
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFCCCC");
            else if (approved == true)
                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCFFCC");
        }
    }
    protected void gvBULKS_SelectedIndexChanged(object sender, EventArgs e)
    {
        mpeShowBULK.Show();
    }
    protected void lnkbtnApprove_Click(object sender, EventArgs e)
    {
        DataAccessLayer dal = new DataAccessLayer();
        RandomPassword rndpassword = new RandomPassword(1, -1, 1, 1, 0, 0, 1, 2);
        int BULKID = int.Parse(((LinkButton)sender).CommandArgument);
        string message = ((TextBox)dvShowBULK.FindControl("txtMessage")).Text;
        message = (message == ((AjaxControlToolkit.TextBoxWatermarkExtender)dvShowBULK.FindControl("wmeMessage")).WatermarkText) ? String.Empty : message;

        dal.AddParameter("@ID", BULKID, DbType.Int32);
        bool changePasswordIfExist = (bool)dal.ExecuteScalar("SELECT [ChangePasswordIfExist] FROM [BULKs] WHERE [ID] = @ID");
        dal.ClearParameters();

        dal.AddParameter("@ID", BULKID, DbType.Int32);
        DataTable dt = dal.ExecuteDataTable("SELECT * FROM [BULKsView] WHERE [BULKRefID] = @ID");
        dal.ClearParameters();

        foreach (DataRow row in dt.Rows)
        {
            int id = (int)row["ID"];
            string ADUsername = row["ADUsername"].ToString().Trim();
            string firstname = row["Firstname"].ToString().Trim();
            string lastname = row["Lastname"].ToString().Trim();
            string ouName = row["OUName"].ToString().Trim();
            string ouDN = row["DistinguishedName"] == DBNull.Value ? "" : row["DistinguishedName"].ToString();
            string password = row["Password"].ToString();
            bool mustChangePassword = password != String.Empty;
            CustomError errors = new CustomError();
            string detailMessage = String.Empty;

            if (!mustChangePassword)
            {
                for (int i = 0; i < 10; i++)
                {
                    password = rndpassword.GeneratePassword(10);
                    if (!Utilities.CheckPassword(password, ADUsername).HasErrors)
                        break;
                }
            }

            errors += Utilities.CheckPassword(password, ADUsername);

            if (ouDN == String.Empty)
                errors.Add(CustomError.ErrorType.OUDoesNotExist);

            if (!errors.HasErrors)
                errors += CreateADUser(ADUsername, firstname, lastname, ouDN, password, mustChangePassword);

            if (errors.HasErrors)
            {
                if (errors.ErrorList.Contains(CustomError.ErrorType.UserExists))
                {
                    bool isLocked = ADUser.Find(ADUsername).Locked;
                    if (isLocked)
                        ADUser.Unlock(ADUsername, false);

                    if (changePasswordIfExist)
                    {
                        ADUser.ChangePassword(ADUsername, password, mustChangePassword, true);
                        Logging.UpdateBULKDetail(id, true, password, mustChangePassword, "MANR findes i forvejen. Password skiftet." + (isLocked ? " Låst op" : ""));
                    }
                    else
                        Logging.UpdateBULKDetail(id, "MANR findes i forvejen. Password IKKE skiftet." + (isLocked ? " Låst op" : ""));
                }
                else
                    Logging.UpdateBULKDetail(id, errors.ToString());
            }
            else
                Logging.UpdateBULKDetail(id, true, password, mustChangePassword);
        }

        Logging.UpdateBULK(BULKID, message);
        Response.Redirect("~/Admin/BULKs");
    }
    protected void lnkbtnReject_Click(object sender, EventArgs e)
    {
        int BULKID = int.Parse(((LinkButton)sender).CommandArgument);
        string message = ((TextBox)dvShowBULK.FindControl("txtMessage")).Text;
        message = (message == ((AjaxControlToolkit.TextBoxWatermarkExtender)dvShowBULK.FindControl("wmeMessage")).WatermarkText) ? String.Empty : message;
        Logging.UpdateBULK(BULKID, message, false);
        Response.Redirect("~/Admin/BULKs");
    }
    protected CustomError CreateADUser(string adusername, string firstname, string lastname, string oudn, string password, bool mustChangePassword)
    {
        CustomError errors = new CustomError();

        if (adusername == String.Empty)
            errors.Add(CustomError.ErrorType.NoUsername);

        if (firstname == String.Empty)
            errors.Add(CustomError.ErrorType.NoFirstname);

        if (lastname == String.Empty)
            errors.Add(CustomError.ErrorType.NoLastname);

        if (!errors.HasErrors)
        {
            ADUser user = new ADUser(adusername, firstname, lastname, password, oudn);
            errors = user.Save(mustChangePassword, true);
        }

        return errors;
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.Clear();
        Response.ContentType = "text/plain";
        Response.AddHeader("Content-Disposition", "attachment; filename=DownloadedData.txt;");

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 40; i++)
        {
            sb.AppendLine(i.ToString());
        }

        // the most easy way as you have type it
        Response.Write(sb.ToString());

        Response.Flush();
        Response.End();
    }
}