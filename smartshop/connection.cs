using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace smartshop
{
    public class connection
    {

        public static string getconnectionString()
        {
            return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        }
        public class utils
        {
            //    SqlConnection con;
            //    SqlCommand cmd;
            //    public bool updatecartquantity(int quantity, int productid, int userid)
            //    {
            //        bool isupdated = false;
            //        con = new SqlConnection(connection.getconnectionString());
            //        cmd = new SqlCommand("cart_crud", con);
            //        cmd.Parameters.AddWithValue("@action", "update");
            //        cmd.Parameters.AddWithValue("@productid", productid);
            //        cmd.Parameters.AddWithValue("@quantity", quantity);
            //        cmd.Parameters.AddWithValue("@userid", userid);
            //        cmd.CommandText = CommandType.StoredProcedure;
            //        try
            //        {
            //            con.Open();
            //            cmd.ExecuteNonQuery();
            //            isupdated = true;
            //        }
            //        catch (Exception ex)
            //        {
            //            isupdated = false;
            //            System.Web.HttpContext.Current.Response.Write("<scrip>alert('Error - " + ex.Message + "')<scrip>");
            //        }
            //        finally
            //        {
            //            con.Close();
            //        }
            //        return isupdated;
            //    }

            SqlConnection con;
            SqlCommand cmd;
            SqlDataAdapter sda;

            public int cartcount(int userid)
            {
                string query = "SELECT COUNT(*) FROM carts WHERE userid = @userid";

                using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userid", userid);

                        try
                        {
                            con.Open();
                            int count = (int)cmd.ExecuteScalar(); // Get the count directly from the query result
                            return count;
                        }
                        catch (Exception ex)
                        {
                            // Log the error or handle it accordingly
                            throw new Exception("An error occurred while fetching cart count: " + ex.Message);
                        }
                    }
                }
            }

            public static string getuniqueid()
            {
                Guid guid = Guid.NewGuid();
                String uniqueid = guid.ToString();
                return uniqueid;
            }

        }

        public class dashboardcount
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlDataReader sda;

            // Method to execute different queries directly
            public int count(string action)
            {
                int count = 0;
                con = new SqlConnection(connection.getconnectionString());

                // Set the SQL query based on the action
                string query = "";
                switch (action)
                {
                    case "categories":
                        query = "SELECT COUNT(*) FROM categories";
                        break;
                    case "products":
                        query = "SELECT COUNT(*) FROM products";
                        break;
                    case "orders":
                        query = "SELECT COUNT(*) FROM orders";
                        break;
                    case "deliveredOrders":
                        query = "SELECT COUNT(*) FROM orders WHERE status = 'delivered'";
                        break;
                    case "pendingDispatchedOrders":
                        query = "SELECT COUNT(*) FROM orders WHERE status IN ('pending', 'dispatched')";
                        break;
                    case "users":
                        query = "SELECT COUNT(*) FROM users";
                        break;
                    case "contacts":
                        query = "SELECT COUNT(*) FROM contact";
                        break;
                    default:
                        throw new Exception("Invalid action provided");
                }

                // Execute the query
                cmd = new SqlCommand(query, con);
                con.Open();
                count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return count;
            }

            // Method to calculate the total sold value (SUM of quantity * price)
            public decimal getTotalSoldValue()
            {
                decimal totalSoldValue = 0;
                con = new SqlConnection(connection.getconnectionString());

                string sql = @"SELECT SUM(o.quantity * p.price)
                       FROM orders o 
                       INNER JOIN products p ON p.productid = o.productid";

                cmd = new SqlCommand(sql, con);

                con.Open();
                var result = cmd.ExecuteScalar();
                totalSoldValue = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                con.Close();

                return totalSoldValue;
            }
        }


    }
}