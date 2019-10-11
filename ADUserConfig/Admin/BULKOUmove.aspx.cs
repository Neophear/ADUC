using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Stiig;

public partial class Admin_BULKOUmove : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnRun_Click(object sender, EventArgs e)
    {
        lblOutput.Text = "";
        List<OU> ous = OU.GetOUs();

        foreach (string line in txtInput.Text.Split(new char[] { '\r', '\n' }))
        {
            if (line != "")
            {
                if (line.Count(x => x == ';') == 1)
                {
                    string manr = line.Split(';')[0];
                    string stabsnummer = line.Split(';')[1].ToUpper().Trim();

                    ADUser au = ADUser.Find(manr);

                    if (au != null)
                    {
                        bool foundOU = false;

                        if (stabsnummer == "")
                        {
                            OU ext = OU.GetOU("EKSTERN MYN");

                            if (ext != null)
                            {
                                foundOU = true;

                                if (au.OU != ext.DistinguishedName)
                                {
                                    ADUser.Move(au.AccountName, ext.DistinguishedName, ext.DisplayName);
                                    lblOutput.Text += manr + " flyttet til " + ext.DisplayName + "<br />";
                                }
                                else
                                    lblOutput.Text += manr + " allerede i korrekt OU; " + ext.DisplayName + "<br />";
                            }
                        }
                        else
                        {
                            foreach (OU o in ous)
                            {
                                if (o.Regex != null && o.Regex.IsMatch(stabsnummer))
                                {
                                    foundOU = true;

                                    if (au.OU != o.DistinguishedName)
                                    {
                                        ADUser.Move(au.AccountName, o.DistinguishedName, o.DisplayName);
                                        lblOutput.Text += manr + " flyttet til " + o.DisplayName + "<br />";
                                    }
                                    else
                                        lblOutput.Text += manr + " allerede i korrekt OU; " + o.DisplayName + "<br />";

                                    break;
                                }
                            }
                        }

                        if (!foundOU)
                            lblOutput.Text += manr + " ikke flyttet da ingen OU matchede " + stabsnummer + "<br />";
                    }
                    else
                        lblOutput.Text += manr + " findes ikke<br />";
                }
                else
                    lblOutput.Text += "Fejl på linje: " + line + "<br />";
            }
        }
    }
    class OU
    {
        public string DistinguishedName { get; set; }
        public string DisplayName { get; set; }
        public Regex Regex { get; set; }

        public static List<OU> GetOUs()
        {
            List<OU> l = new List<OU>();
            DataAccessLayer dal = new DataAccessLayer();

            DataTable dt = dal.ExecuteDataTable("SELECT [DistinguishedName], [DisplayName], [Regex] FROM [OUs] WHERE ([Enabled] = 1) ORDER BY [DisplayName]");

            foreach (DataRow row in dt.Rows)
            {
                OU o = new OU();
                o.DistinguishedName = row["DistinguishedName"].ToString();
                o.DisplayName = row["DisplayName"].ToString();

                if (row["Regex"] != DBNull.Value)
                    o.Regex = new Regex(row["Regex"].ToString());

                l.Add(o);
            }

            return l;
        }

        public static OU GetOU(string displayname)
        {
            OU o = null;

            DataAccessLayer dal = new DataAccessLayer();
            dal.AddParameter("@DisplayName", displayname, DbType.String);
            DataTable dt = dal.ExecuteDataTable("SELECT [DistinguishedName], [Regex] FROM [OUs] WHERE ([Enabled] = 1) AND [DisplayName] = @DisplayName ORDER BY [DisplayName]");

            if (dt.Rows.Count == 1)
            {
                o = new OU();
                o.DisplayName = displayname;
                o.DistinguishedName = dt.Rows[0]["DistinguishedName"].ToString();
                o.Regex = new Regex(dt.Rows[0]["Regex"].ToString());
            }

            return o;
        }
    }
}