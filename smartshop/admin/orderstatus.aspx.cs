using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


namespace smartshop.admin
{
    public partial class orderstatus : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = "order status";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }
                else
                {
                    getorderstatus();
                }
            }
            lblMsg.Visible = false;
            pupdateorderstatus.Visible = false;
        }

        private void getorderstatus()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                // Properly formatted SQL query with spaces between clauses
                string sqlQuery = "SELECT o.orderdetailsid, o.orderno, (pr.price * o.quantity) AS totalprice, " +
                                  "o.status, o.orderdate, p.paymentmode, pr.name " +
                                  "FROM orders o " +
                                  "INNER JOIN payment p ON p.paymentid = o.paymentid " +
                                  "INNER JOIN products pr ON pr.productid = o.productid";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);

                            // Check if the DataTable has rows before binding to avoid potential errors
                            if (dt.Rows.Count > 0)
                            {
                                rorderstatus.DataSource = dt;
                                rorderstatus.DataBind();
                            }
                            else
                            {
                                lblMsg.Text = "No records found.";
                                lblMsg.ForeColor = System.Drawing.Color.Orange;
                                lblMsg.CssClass = "alert alert-warning";
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            lblMsg.Text = "SQL error occurred: " + sqlEx.Message;
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.CssClass = "alert alert-danger";
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Text = "An error occurred: " + ex.Message;
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.CssClass = "alert alert-danger";
                        }
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }


        protected void rorderstatus_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false; // Initially hide the message label
            string connectionString = connection.getconnectionString();

            if (e.CommandName == "Edit")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT orderdetailsid, status FROM orders WHERE orderdetailsid = @orderdetailsid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@orderdetailsid", e.CommandArgument);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string status = dt.Rows[0]["status"].ToString();
                                hdnId.Value = dt.Rows[0]["orderdetailsid"].ToString();

                                // Check if the value exists in the dropdown list
                                if (ddlorderstatus.Items.FindByValue(status) != null)
                                {
                                    ddlorderstatus.SelectedValue = status; // Set the status if it exists in the dropdown
                                }
                                else
                                {
                                    // Optionally, add the status value to the dropdown if it's missing
                                    ddlorderstatus.Items.Add(new ListItem(status, status));
                                    ddlorderstatus.SelectedValue = status;
                                }

                                pupdateorderstatus.Visible = true; // Show the update panel
                            }
                        }
                    }
                }

                // Update the style of the Edit button (since it is an <asp:Button> and not a LinkButton)
                Button btn = e.Item.FindControl("btnEdit") as Button;
                if (btn != null)
                {
                    btn.CssClass = "btn btn-sm btn-warning"; // Apply warning style to indicate it's in edit mode
                }
            }
        }



        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            int orderdetailsid = Convert.ToInt32(hdnId.Value);

            // Connection to the database
            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                // Define the update query
                string updateQuery = "UPDATE orders SET status=@status WHERE orderdetailsid=@orderdetailsid";

                // Create the command and add parameters
                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                {
                    cmd.Parameters.AddWithValue("@orderdetailsid", orderdetailsid);
                    cmd.Parameters.AddWithValue("@status", ddlorderstatus.SelectedValue);

                    try
                    {
                        // Open the connection
                        con.Open();

                        // Execute the update command
                        cmd.ExecuteNonQuery();

                        // Show success message
                        lblMsg.Visible = true;
                        lblMsg.Text = "Order Status Updated Successfully";
                        lblMsg.CssClass = "alert alert-success";

                        // Refresh the order status list
                        getorderstatus();
                    }
                    catch (Exception ex)
                    {
                        // Handle errors and show error message
                        lblMsg.Visible = true;
                        lblMsg.Text = "Error - " + ex.Message;
                        lblMsg.CssClass = "alert alert-danger";
                    }
                    finally
                    {
                        // Close the connection
                        con.Close();
                    }
                }
            }
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pupdateorderstatus.Visible = false;
        }
    }
}