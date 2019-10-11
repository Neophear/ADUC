using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_Default : Page
{
    private int countAll = 0;
    private DataTable table;
    protected void Page_Load(object sender, EventArgs e)
    {
        DataAccessLayer dal = new DataAccessLayer();
        DataTable dt = dal.ExecuteDataTable("SELECT * FROM [OUs]");
        dt.Columns.Add("UserCount");

        foreach (DataRow row in dt.Rows)
        {
            int count = ADUser.GetUserCount(row["DistinguishedName"].ToString());
            row["UserCount"] = count;
            countAll += count;
        }

        DataRow rowAll = dt.NewRow();
        rowAll["DisplayName"] = "<b>I alt</b>";
        rowAll["UserCount"] = String.Format("<b>{0}</b>", countAll);

        dt.Rows.Add(rowAll);

        gvUserCount.DataSource = dt;
        gvUserCount.DataBind();
    }
}