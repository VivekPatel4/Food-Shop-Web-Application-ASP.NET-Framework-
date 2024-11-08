using System;
using System.Collections.Generic;
using System.Linq;
using System.util;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static smartshop.connection;

namespace smartshop.admin
{
    public partial class index1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = " ";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }
                else
                {
                    // Instantiate the class
                    // Instantiate the class
                    dashboardcount dashboard = new dashboardcount();

                    // Fetch different counts and store them in Session
                    Session["categorycount"] = dashboard.count("categories");
                    Session["productcount"] = dashboard.count("products");
                    Session["ordercount"] = dashboard.count("orders");
                    Session["deliveredorderCount"] = dashboard.count("deliveredOrders");
                    Session["pendingdispatchedOrderCount"] = dashboard.count("pendingDispatchedOrders");
                    Session["usercount"] = dashboard.count("users");
                    Session["contactcount"] = dashboard.count("contacts");

                    // Get total sold value and store it in Session
                    Session["TotalSoldValue"] = dashboard.getTotalSoldValue();



                }
            }
        }
    }
}