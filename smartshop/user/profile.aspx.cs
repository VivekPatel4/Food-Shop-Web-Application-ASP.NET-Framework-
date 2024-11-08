using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace smartshop.user
{
    public partial class profile : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userid"] == null)
                {
                    Response.Redirect("login.aspx");
                }
                else
                {
                    getuserdetails();
                    getparchasehistory();

                }
                if (Session["imageurl"] != null && !string.IsNullOrEmpty(Session["imageurl"].ToString()))
                {
                    imgProfile.ImageUrl = Session["imageurl"].ToString();
                }
                else
                {
                    // Set a default image if session variable is empty or not set
                    imgProfile.ImageUrl = "~/images/default-profile.png";
                }

            }
        }

        void getuserdetails()
        {
            // Initialize connection
            con = new SqlConnection(connection.getconnectionString());

            // SQL Query to fetch user details
            string sqlQuery = "SELECT * " +
                              "FROM users WHERE userid = @userid";

            cmd = new SqlCommand(sqlQuery, con);
            cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);
            ruserprofile.DataSource = dt;
            ruserprofile.DataBind();
            if (dt.Rows.Count == 1)
            {
                // Populate user details
                Session["name"] = dt.Rows[0]["name"].ToString();
                Session["mobile"] = dt.Rows[0]["mobile"].ToString();
                Session["email"] = dt.Rows[0]["email"].ToString();
                Session["imageurl"] = dt.Rows[0]["imageurl"].ToString();
                Session["createddate"] = dt.Rows[0]["createddate"];

               
             

            }
        }
        public string GetImageUrl()
        {
            // Check if the session variable "imageurl" is set and not null
            if (Session["imageurl"] != null && !string.IsNullOrEmpty(Session["imageurl"].ToString()))
            {
                return Session["imageurl"].ToString();
            }
            else
            {
                // If the session variable is null or empty, return a default image path
                return "~/images/default-profile.png";
            }
        }

        void getparchasehistory()
        {

            string query = @"SELECT DISTINCT o.paymentid,p.paymentmode,p.cardno
                     FROM orders o
                     INNER JOIN payment p ON p.paymentid = o.paymentid
                     WHERE o.userid = @userid";

            int sr = 1;
            con = new SqlConnection(connection.getconnectionString());
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();

            try
            {
                sda.Fill(dt);
                dt.Columns.Add("srno", typeof(Int32));
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow datarow in dt.Rows) 
                    {
                        datarow["srno"]=sr;  
                        sr++;
                    }
                }
                
                if (dt.Rows.Count == 0)
                {

                    rparchasehistory.FooterTemplate = null;
                    rparchasehistory.FooterTemplate = new customtemplate(ListItemType.Footer);// Assuming CustomTemplate is defined elsewhere
                }

                rparchasehistory.DataSource = dt;
                rparchasehistory.DataBind();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error - " + ex.Message + "')</script>");
            }
            finally
            {
                con.Close();
            }
        }

        protected void rparchasehistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string query = @"
            SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [srno],
                   o.orderno,
                   p.name,
                   p.price,
                   o.quantity,
                   (p.price * o.quantity) AS totalprice,
                   o.orderdate,
                   o.status
            FROM orders o
            INNER JOIN products p ON p.productid = o.productid
            WHERE o.paymentid = @paymentid AND o.userid = @userid";

                double grandtotal = 0;
                HiddenField paymentid = e.Item.FindControl("hdnpaymentid") as HiddenField;
                Repeater reporders = e.Item.FindControl("rorder") as Repeater;
                con = new SqlConnection(connection.getconnectionString());
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@paymentid", Convert.ToInt32(paymentid.Value));
                cmd.Parameters.AddWithValue("@userid", Session["userid"]);
                sda = new SqlDataAdapter(cmd);
                dt = new DataTable();

                try
                {
                    sda.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow datarow in dt.Rows)
                        {
                            grandtotal += Convert.ToDouble(datarow["totalprice"]);

                        }
                    }
                    DataRow dr = dt.NewRow();
                    dr["totalprice"] = grandtotal;
                    dt.Rows.Add(dr);
                    reporders.DataSource = dt;
                    reporders.DataBind();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error - " + ex.Message + "')</script>");
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private sealed class customtemplate : ITemplate
        {
            private ListItemType ListItemType { get; set; }

            public customtemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {
                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td><b>Hungry! Why not order food for you. </b> <a href='menu.aspx' class='badge badge-info ml-2'>Click to Order</a></td></tr>");
                    container.Controls.Add(footer);
                }
            }
        }

       
    }
}