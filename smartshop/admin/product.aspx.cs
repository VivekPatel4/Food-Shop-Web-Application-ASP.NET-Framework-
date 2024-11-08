using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace smartshop.admin
{
    public partial class product : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = "product";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }
                else
                {
                    getproducts();
                }
            }
            lblMsg.Visible = false;
        }

        protected void btnAddorUpdate_Click(object sender, EventArgs e)
        {
            // Initialize variables
            string actionname = string.Empty, imagepath = string.Empty, fileextension = string.Empty;
            bool isvalidtoexecute = false;
            int productid = Convert.ToInt32(hdnId.Value);

            // Establish the connection
            using (con = new SqlConnection(connection.getconnectionString()))
            {
                // Determine the SQL command based on the action
                string sqlQuery;
                if (productid == 0) // Insert new product
                {
                    sqlQuery = "INSERT INTO products (name, description, price, quantity, imageurl, isactive, categoryid, createddate) " +
                               "VALUES (@name, @description, @price, @quantity, @imageurl, @isactive, @categoryid, GETDATE())";
                }
                else // Update existing product
                {
                    sqlQuery = "UPDATE products SET name = @name, description = @description, price = @price, quantity = @quantity, " +
                               "isactive = @isactive, categoryid = @categoryid";

                    // Include image URL in the update only if a new image is uploaded
                    if (fuproductImage.HasFile)
                    {
                        sqlQuery += ", imageurl = @imageurl";
                    }

                    sqlQuery += " WHERE productid = @productid";
                }

                // Initialize the command
                cmd = new SqlCommand(sqlQuery, con);

                // Add common parameters
                cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                cmd.Parameters.AddWithValue("@description", txtdescription.Text.Trim());
                cmd.Parameters.AddWithValue("@price", Convert.ToDecimal(txtprice.Text.Trim()));
                cmd.Parameters.AddWithValue("@quantity", Convert.ToInt32(txtqunatity.Text.Trim()));
                cmd.Parameters.AddWithValue("@isactive", cbIsactive.Checked);
                cmd.Parameters.AddWithValue("@categoryid", ddlcategories.SelectedValue);

                // Handle file upload if there's an image
                if (fuproductImage.HasFile)
                {
                    fileextension = System.IO.Path.GetExtension(fuproductImage.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    if (allowedExtensions.Contains(fileextension))
                    {
                        string relativePath = "~/images/product/";  // Correct the relative path
                        string physicalPath = Server.MapPath(relativePath);

                        // Ensure the directory exists
                        if (!System.IO.Directory.Exists(physicalPath))
                        {
                            System.IO.Directory.CreateDirectory(physicalPath);
                        }

                        imagepath = relativePath + fuproductImage.FileName;
                        fuproductImage.SaveAs(System.IO.Path.Combine(physicalPath, fuproductImage.FileName));
                        cmd.Parameters.AddWithValue("@imageurl", imagepath);
                        isvalidtoexecute = true;
                    }
                    else
                    {
                        lblMsg.Text = "Invalid file type. Please upload an image file.";
                        lblMsg.ForeColor = System.Drawing.Color.Red;
                        lblMsg.CssClass = "alert alert-danger";
                        return;
                    }
                }
                else
                {
                    // Retain the existing image URL if not uploading a new one
                    if (productid != 0)
                    {
                        cmd.Parameters.AddWithValue("@imageurl", hdnImagePath.Value);
                    }
                    isvalidtoexecute = true;
                }

                // Add the product ID parameter if updating
                if (productid != 0)
                {
                    cmd.Parameters.AddWithValue("@productid", productid);
                }

                // Execute the command if valid
                if (isvalidtoexecute)
                {
                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMsg.Text = productid == 0 ? "Product added successfully." : "Product updated successfully.";
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                            lblMsg.CssClass = "alert alert-success";
                            lblMsg.Visible = true;

                            // Clear fields after successful operation
                            getproducts();
                            ClearForm();
                        }
                        else
                        {
                            lblMsg.Text = "No rows were affected. Please check your input.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.CssClass = "alert alert-danger";
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
                else
                {
                    lblMsg.Text = "Operation could not be performed. Please check your input.";
                    lblMsg.ForeColor = System.Drawing.Color.Red;
                    lblMsg.CssClass = "alert alert-danger";
                }
            }
        }

        private void getproducts()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT p.*, c.name AS categoryname FROM products p " +
                                  "INNER JOIN categories c ON c.categoryid = p.categoryid " +
                                  "ORDER BY p.createddate DESC";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);
                            rproduct.DataSource = dt;
                            rproduct.DataBind();
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

        private void ClearForm()
        {
            txtname.Text = string.Empty;
            txtdescription.Text = string.Empty;
            txtqunatity.Text = string.Empty;
            txtprice.Text = string.Empty;
            ddlcategories.ClearSelection();
            cbIsactive.Checked = false;
            hdnId.Value = "0";
            hdnImagePath.Value = string.Empty;
            imgProduct.ImageUrl = "~/images/no_image.png"; // Reset image to default
            btnAddorUpdate.Text = "Add";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void rproduct_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Add logic for Edit/Delete commands here
            if (e.CommandName == "Edit")
            {
                int productid = Convert.ToInt32(e.CommandArgument);
                // Load the product data for editing
                LoadProductForEdit(productid);
            }
            else if (e.CommandName == "Delete")
            {
                int productid = Convert.ToInt32(e.CommandArgument);
                // Delete the product
                DeleteProduct(productid);
            }
        }

        private void LoadProductForEdit(int productid)
        {
            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT * FROM products WHERE productid = @productid";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@productid", productid);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtname.Text = reader["name"].ToString();
                            txtdescription.Text = reader["description"].ToString();
                            txtprice.Text = reader["price"].ToString();
                            txtqunatity.Text = reader["quantity"].ToString();
                            ddlcategories.SelectedValue = reader["categoryid"].ToString();
                            cbIsactive.Checked = Convert.ToBoolean(reader["isactive"]);
                            hdnId.Value = reader["productid"].ToString();
                            hdnImagePath.Value = reader["imageurl"].ToString();
                            imgProduct.ImageUrl = reader["imageurl"].ToString();
                            btnAddorUpdate.Text = "Update";
                        }
                    }
                }
            }
        }

        private void DeleteProduct(int productid)
        {
            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "DELETE FROM products WHERE productid = @productid";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@productid", productid);
                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            lblMsg.Text = "Product deleted successfully.";
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                            lblMsg.CssClass = "alert alert-success";
                            lblMsg.Visible = true;

                            // Refresh the product list after deletion
                            getproducts();
                        }
                        else
                        {
                            lblMsg.Text = "No rows were affected. Please check your input.";
                            lblMsg.ForeColor = System.Drawing.Color.Red;
                            lblMsg.CssClass = "alert alert-danger";
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

        protected string GetImageUrl(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return ResolveUrl(imageUrl);
            }
            return ResolveUrl("~/images/product/no_image.png"); // Fallback image if no URL provided
        }
    }
}
