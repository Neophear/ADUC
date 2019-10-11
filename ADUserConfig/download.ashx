<%@ WebHandler Language="C#" Class="download" %>

using System;
using System.Data;
using System.Web.Security;
using System.Text;
using System.Web;
using Stiig;

public class download : IHttpHandler {
    
    public void ProcessRequest (HttpContext context)
    {
        string strID = context.Request.QueryString["id"];
        int id = 0;
        
        if (!String.IsNullOrEmpty(strID) && int.TryParse(strID, out id))
        {
            DataAccessLayer dal = new DataAccessLayer();
            dal.AddParameter("@ID", id, System.Data.DbType.Int32);
            dal.AddParameter("@CreatedBy", context.User.Identity.Name, System.Data.DbType.String);
            dal.AddParameter("@IsAdmin", Roles.IsUserInRole("Admin"), System.Data.DbType.Boolean);
            DataTable dt = dal.ExecuteDataTable("EXEC dbo.GetBULKPrint @ID, @CreatedBy, @IsAdmin");
            dal.ClearParameters();
            
            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Content-Disposition", String.Format("attachment; filename=BULK_{0}.csv;", id));

            StringBuilder sb = new StringBuilder();
            
            foreach (DataRow row in dt.Rows)
                sb.AppendLine(String.Format("{0};{1};{2};{3}", row["ADUsername"], row["Firstname"], row["Lastname"], row["Password"]));

            // the most easy way as you have type it
            context.Response.Write(sb.ToString());

            context.Response.Flush();
            context.Response.End();
        }
        else
            throw new Exception();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}