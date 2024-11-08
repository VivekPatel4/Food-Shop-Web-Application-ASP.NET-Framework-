using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static smartshop.connection;

namespace smartshop.user
{
    public partial class index : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.Url.AbsoluteUri.ToString().Contains("index.aspx"))
            {
                form1.Attributes.Add("class", "sub_page");
            }
            else
            {
                form1.Attributes.Remove("class");
                Control silderindexcontrol = (Control)Page.LoadControl("silderindexcontrol.ascx");
                PnlsliderUC.Controls.Add(silderindexcontrol);
            }
            if (Session["userid"] != null)
            {
                lbloginorlogout.Text = "Logout";
                utils utils = new utils();
                Session["cartcount"] = utils.cartcount(Convert.ToInt32(Session["userid"])).ToString();
            }
            else
            {
                lbloginorlogout.Text = "Login";
                Session["cartcount"] = 0;
            }
        }

        protected void lbloginorlogout_Click(object sender, EventArgs e)
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }
            else
            {
                Session.Abandon();
                Response.Redirect("login.aspx");
            }
        }

        protected void lblregorprofile_Click(object sender, EventArgs e)
        {
            if (Session["userid"] != null)
            {
                lblregorprofile.ToolTip = "User Profile";
                Response.Redirect("profile.aspx");
            }
            else
            {
                ;lblregorprofile.ToolTip = "User Profile";
                Response.Redirect("reg.aspx");
            }
        }
    }
}