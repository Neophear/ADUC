using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;

public partial class BULK : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblBULKMsg.Visible = false;
        string[] files = Directory.GetFiles(Server.MapPath("~/Files"), "BULK*.xlsx");

        if (files.Length == 1)
        {
            string BULKFilePath = "~/Files/" + Path.GetFileName(files[0]);
            hplBULKFile.NavigateUrl = BULKFilePath;
        }

        hplBULKFile.Visible = files.Length == 1;
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
            else if (fuBULKFile.FileContent.Length > 2000000)
                SetMessage("Filen er over 2MB", true);
            else
            {
                string localPath = Server.MapPath("~/Files/TEMP/");

                if (!Directory.Exists(localPath))
                    Directory.CreateDirectory(localPath);

                string filename = String.Format("{0}{1}{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Membership.GetUser().UserName, ext);
                string filePath = localPath + filename;
                fuBULKFile.SaveAs(filePath);
                
                int BULKID = Logging.CreateNewBULK(chkbxChangePassword.Checked);

                Logging.WriteLog(Logging.Action.Create, Logging.ObjectType.BULK, BULKID.ToString());

                if (ReadExcelIntoDB(BULKID, filePath))
                    Response.Redirect("~/BULK");
            }
        }
        else
            SetMessage("Kunne ikke finde filen", true);
    }

    private bool ReadExcelIntoDB(int bulkid, string filepath)
    {
        var package = new ExcelPackage(new FileInfo(filepath));

        ExcelWorksheet ws = package.Workbook.Worksheets[1];

        if (!CheckColumnNames(ws))
        {
            SetMessage("Excel-arket er ikke validt", true);
            return false;
        }
        else
        {
            for (int i = 2;i <= ws.Dimension.End.Row;i++)
            {
                string adusername = ws.Cells[i, 1].Text.Trim();
                string firstname = ws.Cells[i, 2].Text.Trim();
                string lastname = ws.Cells[i, 3].Text.Trim();
                string password = ws.Cells[i, 4].Text;
                string ouname = ws.Cells[i, 5].Text.Trim();

                if (adusername != String.Empty || firstname != String.Empty || lastname != String.Empty)
                    Logging.WriteBULKDetail(bulkid, (i + 2), adusername, firstname, lastname, password, ouname);
            }
            return true;
        }
    }

    private bool CheckColumnNames(ExcelWorksheet ws)
    {
        if (!ws.Cells["A1"].Text.StartsWith("MANR"))
            return false;

        if (!ws.Cells["B1"].Text.StartsWith("Fornavn"))
            return false;

        if (!ws.Cells["C1"].Text.StartsWith("Efternavn"))
            return false;

        if (!ws.Cells["D1"].Text.StartsWith("Password"))
            return false;

        if (!ws.Cells["E1"].Text.StartsWith("Enhed"))
            return false;

        return true;
    }

    protected void sdsBULKs_Selecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters["@CreatedBy"].Value = User.Identity.Name;
    }
    protected void sdsBULKs_Deleting(object sender, SqlDataSourceCommandEventArgs e)
    {
        e.Command.Parameters["@DeletedBy"].Value = User.Identity.Name;
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
}