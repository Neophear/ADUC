using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_AdvancedTextbox : System.Web.UI.UserControl
{
    public string Text
    {
        get { return txtText.Text; }
        set { txtText.Text = value; }
    }

    private int textWidth;
    public int Width
    {
        get { return textWidth; }
        set { txtText.Width = txtHelpbox.Width = textWidth = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}