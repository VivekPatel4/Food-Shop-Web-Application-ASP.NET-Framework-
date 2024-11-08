using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static smartshop.connection;

namespace smartshop.user
{
    public partial class payment : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr, dr1;
        DataTable dt;
        SqlTransaction transaction = null;
        string _name = string.Empty; string _cardno=string.Empty; string _expirydate=string.Empty; string _cvv=string.Empty; 
        string _address=string.Empty; string _paymentmode=string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userid"] == null)
                {
                    Response.Redirect("login.aspx");
                }
            }
        }
               
        protected void lbcardsubmit_Click(object sender, EventArgs e)
        {
            _name = txtname.Text.Trim();
            _cardno = txtcardno.Text.Trim();
            _cardno = string.Format("************{0}", txtcardno.Text.Trim().Substring(12, 4));
            _expirydate = txtexpmonth.Text.Trim() + "/" +txtexpyear.Text.Trim();
            _cvv = txtcvv.Text.Trim();
            _address = txtaddress.Text.Trim();
            _paymentmode = "card";
            if (Session["userid"] != null)
            {
                orderpayment(_name,_cardno,_expirydate,_cvv,_address,_paymentmode);
            }
            else
            {
                Response.Redirect("login.aspx");                 
            }
        }
        protected void lbcodsubmit_Click(object sender, EventArgs e)
        {
            _address = txtcodaddress.Text.Trim();
            _paymentmode = "cod";
            if (Session["userid"] != null)
            {
                orderpayment(_name, _cardno, _expirydate, _cvv, _address, _paymentmode);
            }
            else
            {
                Response.Redirect("login.aspx");
            }
        }
        void orderpayment(string name, string cardno, string expirydate, string cvv, string address, string paymentmode)
        {
            int paymentid;
            int productid;
            int quantity;

            // DataTable for storing order details
            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] {
        new DataColumn("orderno", typeof(string)),
        new DataColumn("productid", typeof(int)),
        new DataColumn("quantity", typeof(int)),
        new DataColumn("userid", typeof(int)),
        new DataColumn("status", typeof(string)),
        new DataColumn("paymentid", typeof(int)),
        new DataColumn("orderdate", typeof(DateTime)),
    });

            con = new SqlConnection(connection.getconnectionString());
            con.Open();

            #region Sql Transaction
            transaction = con.BeginTransaction();

            // Step 1: Insert the payment information into the Payments table
            string paymentQuery = "INSERT INTO payment (name, cardno, expirydate, cvvno, address, paymentmode) " +
                                  "OUTPUT INSERTED.paymentid VALUES (@name, @cardno, @expirydate, @cvv, @address, @paymentmode)";
            SqlCommand paymentCmd = new SqlCommand(paymentQuery, con, transaction);  // Create a new SqlCommand object for payment
            paymentCmd.Parameters.AddWithValue("@name", name);
            paymentCmd.Parameters.AddWithValue("@cardno", cardno);
            paymentCmd.Parameters.AddWithValue("@expirydate", expirydate);
            paymentCmd.Parameters.AddWithValue("@cvv", cvv);
            paymentCmd.Parameters.AddWithValue("@address", address);
            paymentCmd.Parameters.AddWithValue("@paymentmode", paymentmode);

            try
            {
                // Execute the payment insertion and get the generated PaymentID
                paymentid = (int)paymentCmd.ExecuteScalar();

                #region Getting Cart Items
                // Step 2: Retrieve items from the Cart table
                string cartQuery = "SELECT productid, quantity FROM carts WHERE userid = @userid";
                SqlCommand cartCmd = new SqlCommand(cartQuery, con, transaction);  // Create a new SqlCommand object for cart
                cartCmd.Parameters.AddWithValue("@userid", Session["userid"]);

                SqlDataReader dr = cartCmd.ExecuteReader();
                while (dr.Read())
                {
                    productid = (int)dr["productid"];
                    quantity = (int)dr["quantity"];

                    // Update the product quantity
                    updatequantity(productid, quantity, transaction, con);

                    // Delete the item from the cart
                    deletecartitem(productid, transaction, con);

                    // Add the order details to the DataTable
                    dt.Rows.Add(utils.getuniqueid(), productid, quantity, (int)Session["userid"], "pending", paymentid, Convert.ToDateTime(DateTime.Now));
                }
                dr.Close(); // Close the SqlDataReader after use
                #endregion Getting Cart Items

                #region Save Order Details
                // Step 3: Insert the order details into the Orders table if there are items
                if (dt.Rows.Count > 0)
                {
                    // Use a new SqlCommand object for inserting orders
                    foreach (DataRow row in dt.Rows)
                    {
                        string orderQuery = "INSERT INTO orders (orderno, productid, quantity, userid, status, paymentid, orderdate) " +
                                            "VALUES (@orderno, @productid, @quantity, @userid, @status, @paymentid, @orderdate)";
                        SqlCommand orderCmd = new SqlCommand(orderQuery, con, transaction);
                        orderCmd.Parameters.AddWithValue("@orderno", row["orderno"]);
                        orderCmd.Parameters.AddWithValue("@productid", row["productid"]);
                        orderCmd.Parameters.AddWithValue("@quantity", row["quantity"]);
                        orderCmd.Parameters.AddWithValue("@userid", row["userid"]);
                        orderCmd.Parameters.AddWithValue("@status", row["status"]);
                        orderCmd.Parameters.AddWithValue("@paymentid", row["paymentid"]);
                        orderCmd.Parameters.AddWithValue("@orderdate", row["orderdate"]);

                        orderCmd.ExecuteNonQuery();
                    }
                }
                #endregion Save Order Details

                // Commit the transaction
                transaction.Commit();
                lblmsg.Visible = true;
                lblmsg.Text = "Your item ordered successfully!!!";
                lblmsg.CssClass = "alert alert-success";

                // Redirect to the invoice page with payment ID
                Response.AddHeader("REFRESH", "1;URL=invoice.aspx?id=" + paymentid);
            }
            catch (Exception ex)
            {
                try
                {
                    // Rollback transaction if something goes wrong
                    transaction.Rollback();
                }
                catch (Exception rollbackEx)
                {
                    Response.Write("<script>alert('" + rollbackEx.Message + "') </script>");
                }

                // Display the original exception
                Response.Write("<script>alert('" + ex.Message + "') </script>");
            }
            finally
            {
                con.Close();
            }
            #endregion Sql Transaction
        }



        void updatequantity(int _productid, int _quantity, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            int dbquantity;

            // Step 1: Get the current quantity of the product from the database
            string getQuantityQuery = "SELECT quantity FROM products WHERE productid = @productid";
            SqlCommand cmd = new SqlCommand(getQuantityQuery, sqlConnection, sqlTransaction);
            cmd.Parameters.AddWithValue("@productid", _productid);

            try
            {
                // Execute the query and read the quantity
                SqlDataReader dr1 = cmd.ExecuteReader();
                while (dr1.Read())
                {
                    dbquantity = (int)dr1["quantity"];

                    // Step 2: Check if we can update the quantity (quantity in DB must be greater than the requested quantity)
                    if (dbquantity > _quantity && dbquantity > 2)
                    {
                        // Calculate the new quantity
                        dbquantity = dbquantity - _quantity;

                        // Step 3: Update the quantity in the Products table
                        string updateQuantityQuery = "UPDATE products SET quantity = @quantity WHERE productid = @productid";
                        SqlCommand updateCmd = new SqlCommand(updateQuantityQuery, sqlConnection, sqlTransaction);
                        updateCmd.Parameters.AddWithValue("@quantity", dbquantity);
                        updateCmd.Parameters.AddWithValue("@productid", _productid);

                        // Execute the update query
                        updateCmd.ExecuteNonQuery();
                    }
                }
                dr1.Close(); // Close the reader after reading the data
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }


        void deletecartitem(int _productid, SqlTransaction sqlTransaction, SqlConnection sqlConnection)
        {
            // Use raw SQL query instead of stored procedure
            string query = "DELETE FROM carts WHERE productid = @productid AND userid = @userid";
            SqlCommand cmd = new SqlCommand(query, sqlConnection, sqlTransaction);

            // Add parameters
            cmd.Parameters.AddWithValue("@productid", _productid);
            cmd.Parameters.AddWithValue("@userid", Session["userid"]);

            try
            {
                // Execute the DELETE query
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Response.Write("<script>alert('" + ex.Message + "')</script>");
            }
        }

    }
}