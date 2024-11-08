using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using smartshop.user;

namespace smartshop.admin
{
    public partial class report : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = "Selling Report";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }

            }
        }

        private void getreportdata(DateTime fromdate, DateTime todate)
        {
            DataTable dt = new DataTable();
            double grandtotal = 0;

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT ROW_NUMBER() OVER (ORDER BY(SELECT 1)) AS [srno], u.name, u.email, " +
                                  "SUM(o.quantity) AS totalorders, SUM(o.quantity * p.price) AS totalprice " +
                                  "FROM orders o " +
                                  "INNER JOIN products p ON p.productid = o.productid " +
                                  "INNER JOIN users u ON u.userid = o.userid " +
                                  "WHERE CAST(o.orderdate AS date) BETWEEN @fromdate AND @todate " +
                                  "GROUP BY u.name, u.email";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@fromdate", fromdate);
                    cmd.Parameters.AddWithValue("@todate", todate);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    grandtotal += Convert.ToDouble(dr["totalprice"]);
                                }

                                lbltotal.Text = "Sold Cost: ₹" + grandtotal;
                                lbltotal.CssClass = "badge badge-primary";

                                
                                rreport.DataSource = dt;
                                rreport.DataBind();
                            }
                            else
                            {
                                lbltotal.Text = "No data available for the selected dates.";
                                lbltotal.CssClass = "badge badge-warning";
                                rreport.DataSource = null;
                                rreport.DataBind();
                            }
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }


        protected void btnsearch_Click(object sender, EventArgs e)
        {
            DateTime fromdate = Convert.ToDateTime(txtfromdate.Text);
            DateTime todate = Convert.ToDateTime(txttodate.Text);
            if (todate > DateTime.Now)
            {
                Response.Write("<script>alert('ToDate cannot be greater than current date!')</script>");
            }
            else if (fromdate > todate)
            {
                Response.Write("<script>alert('FromDate cannot be greater than Todate!')</script>");
            }
            else
            {
                getreportdata(fromdate, todate);
            }
        }
    }
}