using smartshop.admin;
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
    public partial class cart : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        decimal grandtotal = 0;
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
                    getcartitem();
                }

            }
        }

        void getcartitem()
        {

            string query = @"SELECT c.productid, p.name, p.imageurl, p.price, c.quantity AS qty, p.quantity AS prdqty
                     FROM carts c
                     INNER JOIN products p ON p.productid = c.productid
                     WHERE c.userid = @userid";


            con = new SqlConnection(connection.getconnectionString());
            cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();

            try
            {
                sda.Fill(dt);
                rcartitem.DataSource = dt;
                if (dt.Rows.Count == 0)
                {
                    
                    rcartitem.FooterTemplate = null;
                    rcartitem.FooterTemplate = new customtemplate(ListItemType.Footer);// Assuming CustomTemplate is defined elsewhere
                }
               

                rcartitem.DataBind();
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

        protected string GetImageUrl(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return ResolveUrl(imageUrl);
            }
            return ResolveUrl("~/images/product/no_image.png");
        }
        protected void rcartitem_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            utils utils = new utils();
            if (e.CommandName == "remove")
            {
                string query = "DELETE FROM carts WHERE productid = @productid AND userid = @userid";

                using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@productid", e.CommandArgument);
                        cmd.Parameters.AddWithValue("@userid", Session["userid"]);

                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            getcartitem();
                            Session["cartcount"] = utils.cartcount(Convert.ToInt32(Session["userid"]));
                        }
                        catch (Exception ex)
                        {
                            Response.Write("<script>alert('Error - " + ex.Message + "')</script>");
                        }
                    }
                }
            }

            if (e.CommandName == "updatecart")
            {
                bool iscartupdated = false;
                for (int item = 0; item < rcartitem.Items.Count; item++)
                {
                    if (rcartitem.Items[item].ItemType == ListItemType.Item || rcartitem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox quantity = rcartitem.Items[item].FindControl("txtquantity") as TextBox;
                        HiddenField _productid = rcartitem.Items[item].FindControl("hdnproductid") as HiddenField;
                        HiddenField _quantity = rcartitem.Items[item].FindControl("hdnquantity") as HiddenField;

                        int quantityfromcart = Convert.ToInt32(quantity.Text);
                        int productid = Convert.ToInt32(_productid.Value);
                        int quantityfrondb = Convert.ToInt32(_quantity.Value);

                        bool istrue = false;
                        int updatedquantity = 1;

                        if (quantityfromcart > quantityfrondb)
                        {
                            updatedquantity = quantityfromcart;
                            istrue = true;
                        }
                        else if (quantityfromcart < quantityfrondb)
                        {
                            updatedquantity = quantityfromcart;
                            istrue = true;
                        }

                        if (istrue)
                        {
                            string updateQuery = "UPDATE carts SET quantity = @quantity WHERE productid = @productid AND userid = @userid";

                            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                            {
                                using (SqlCommand cmd = new SqlCommand(updateQuery, con))
                                {
                                    cmd.Parameters.AddWithValue("@quantity", updatedquantity);
                                    cmd.Parameters.AddWithValue("@productid", productid);
                                    cmd.Parameters.AddWithValue("@userid", Session["userid"]);

                                    try
                                    {
                                        con.Open();
                                        cmd.ExecuteNonQuery();
                                        iscartupdated = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        Response.Write("<script>alert('Error - " + ex.Message + "')</script>");
                                    }
                                }
                            }
                        }
                    }
                }
                if (iscartupdated)
                {
                    getcartitem();
                }
            }

            if (e.CommandName == "checkout")
            {
                bool istrue = false;
                string pname = string.Empty;
                for (int item = 0; item < rcartitem.Items.Count; item++)
                {
                    if (rcartitem.Items[item].ItemType == ListItemType.Item || rcartitem.Items[item].ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField _productid = rcartitem.Items[item].FindControl("hdnproductid") as HiddenField;
                        HiddenField _cartquantity = rcartitem.Items[item].FindControl("hdnquantity") as HiddenField;
                        HiddenField _productquantity = rcartitem.Items[item].FindControl("hdnprdquantity") as HiddenField;
                        Label productname = rcartitem.Items[item].FindControl("lblname") as Label;
                        int productid = Convert.ToInt32(_productid.Value);
                        int cartquantity = Convert.ToInt32(_cartquantity.Value);
                        int productquantity = Convert.ToInt32(_productquantity.Value);

                        if (productquantity > cartquantity && productquantity > 2)
                        {

                            istrue = true;
                        }
                        else
                        {
                            istrue = false;
                            pname = productname.Text.ToString();
                            break;
                        }
                    }
                }
                if (istrue)
                {
                    Response.Redirect("payment.aspx");
                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Item <b>'" + pname + "'</b> is out of stock:(";
                    lblmsg.CssClass = "alert alert-warning";
                }

            }
        }


        protected void rcartitem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label totalprice = e.Item.FindControl("lbltotalprice") as Label;
                Label productprice = e.Item.FindControl("lblprice") as Label;
                TextBox quantity = e.Item.FindControl("txtquantity") as TextBox;
                decimal caltotalprice = Convert.ToDecimal(productprice.Text) * Convert.ToDecimal(quantity.Text);
                totalprice.Text = caltotalprice.ToString();
                grandtotal += caltotalprice;
            }
            Session["grandtotalprice"] = grandtotal;
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
                    var footer = new LiteralControl("<tr><td colspan='5'><b>Your cart is empty.</b> <a href='menu.aspx' class='badge badge-info ml-2'>Continue Shopping</a></td></tr>");
                    container.Controls.Add(footer);
                }
            }
        }

    }
}