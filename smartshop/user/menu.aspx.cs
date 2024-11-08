using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using smartshop.admin;
using Mysqlx.Session;
using static smartshop.connection;

namespace smartshop.user
{
    public partial class menu : System.Web.UI.Page
    {

        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getcategories();
                getproducts();
            }
        }
        protected string GetImageUrl(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return ResolveUrl(imageUrl);
            }
            return ResolveUrl("~/images/product/no_image.png");
        }
        private void getproducts()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT p.*, c.name AS categoryname FROM products p " +
                                  "INNER JOIN categories c ON c.categoryid = p.categoryid " +
                                  "WHERE p.isactive=1";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);
                            rproducts.DataSource = dt;
                            rproducts.DataBind();
                        }
                       
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        //public String lowercase(object obj)
        //{
        //    return obj.ToString().ToLower();    
        //}

        private void getcategories()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT * FROM categories WHERE isactive=1";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);
                            rcategory.DataSource = dt;
                            rcategory.DataBind();
                        }
                      
                        finally
                        {
                            con.Close();
                        }
                    }
                }
            }
        }

        protected void rproducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (Session["userid"] != null)
            {
                bool isCartItemUpdated = false;
                int productId = Convert.ToInt32(e.CommandArgument);
                int userId = Convert.ToInt32(Session["userid"]);
                int quantityInCart = isitemexistincart(productId); 

                if (quantityInCart == 0)
                {
                   
                    con = new SqlConnection(connection.getconnectionString());
                    string insertQuery = "INSERT INTO carts (productid, userid, quantity) VALUES (@productid, @userid, @quantity)";
                    cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@productid", productId);
                    cmd.Parameters.AddWithValue("@quantity", 1); 
                    cmd.Parameters.AddWithValue("@userid", userId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
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
                else
                {
                   
                    string updateQuery = "UPDATE carts SET quantity = @quantity WHERE productid = @productid AND userid = @userid";
                    con = new SqlConnection(connection.getconnectionString());
                    cmd = new SqlCommand(updateQuery, con);
                    cmd.Parameters.AddWithValue("@quantity", quantityInCart + 1); 
                    cmd.Parameters.AddWithValue("@productid", productId);
                    cmd.Parameters.AddWithValue("@userid", userId);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        isCartItemUpdated = true;
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

               
                lblmsg.Visible = true;
                lblmsg.Text = "Item added successfully to your cart!";
                lblmsg.CssClass = "alert alert-success";
                Response.AddHeader("REFRESH", "1;URL=cart.aspx");
            }
            else
            {
               Response.Redirect("login.aspx");
            }
        }



        int isitemexistincart(int productid)
        {
           
            con = new SqlConnection(connection.getconnectionString());
            string query = "SELECT quantity FROM carts WHERE productid = @productid AND userid = @userid";
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@productid", productid);
            cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            con.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            int quantity = 0;
           
            if (dt.Rows.Count > 0)
            {
                quantity = Convert.ToInt32(dt.Rows[0]["quantity"]);
            }
            con.Close();          
            return quantity;
        }

    }
}