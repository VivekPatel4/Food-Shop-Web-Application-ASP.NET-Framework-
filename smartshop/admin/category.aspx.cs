using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace smartshop.admin
{
    public partial class category : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = "category";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }
                else
                {
                    getcategories();
                }
            }
            lblMsg.Visible = false;
        }

        protected void btnAddorUpdate_Click(object sender, EventArgs e)
        {
            // Initialize variables
            string actionname = string.Empty, imagepath = string.Empty, fileextension = string.Empty;
            bool isvalidtoexecute = false;
            int categoryid = Convert.ToInt32(hdnId.Value);

            // Establish the connection
            using (con = new SqlConnection(connection.getconnectionString()))
            {
                // Determine the SQL command based on the action
                string sqlQuery;
                if (categoryid == 0) // Insert new category
                {
                    sqlQuery = "INSERT INTO categories (name, imageurl, isactive, createddate) " +
                               "VALUES (@name, @imageurl, @isactive, GETDATE())";
                }
                else // Update existing category
                {
                    sqlQuery = "UPDATE categories SET name = @name, isactive = @isactive";

                    // Include image URL in the update only if a new image is uploaded
                    if (fuCategoryImage.HasFile)
                    {
                        sqlQuery += ", imageurl = @imageurl";
                    }

                    sqlQuery += " WHERE categoryid = @categoryid";
                }

                // Initialize the command
                cmd = new SqlCommand(sqlQuery, con);

                // Add common parameters
                cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                cmd.Parameters.AddWithValue("@isactive", cbIsactive.Checked);

                // Handle file upload if there's an image
                if (fuCategoryImage.HasFile)
                {
                    fileextension = System.IO.Path.GetExtension(fuCategoryImage.FileName).ToLower();
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                    if (allowedExtensions.Contains(fileextension))
                    {
                        string relativePath = "~/images/category/";  // Correct the relative path
                        string physicalPath = Server.MapPath(relativePath);

                        // Ensure the directory exists
                        if (!System.IO.Directory.Exists(physicalPath))
                        {
                            System.IO.Directory.CreateDirectory(physicalPath);
                        }

                        imagepath = relativePath + fuCategoryImage.FileName;
                        fuCategoryImage.SaveAs(physicalPath + "\\" + fuCategoryImage.FileName);
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
                    if (categoryid != 0)
                    {
                        cmd.Parameters.AddWithValue("@imageurl", hdnImagePath.Value);
                    }
                    isvalidtoexecute = true;
                }

                // Add the category ID parameter if updating
                if (categoryid != 0)
                {
                    cmd.Parameters.AddWithValue("@categoryid", categoryid);
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
                            lblMsg.Text = categoryid == 0 ? "Category added successfully." : "Category updated successfully.";
                            lblMsg.ForeColor = System.Drawing.Color.Green;
                            lblMsg.CssClass = "alert alert-success";
                            lblMsg.Visible = true;

                            // Clear fields after successful operation
                            getcategories();
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

        private void getcategories()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT * FROM categories ORDER BY createddate DESC";

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

        protected void rcategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            string connectionString = connection.getconnectionString();

            if (e.CommandName == "Edit")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT * FROM categories WHERE categoryid = @categoryid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@categoryid", e.CommandArgument);
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                // Populate the form fields with data
                                txtname.Text = dt.Rows[0]["name"].ToString();
                                cbIsactive.Checked = Convert.ToBoolean(dt.Rows[0]["isactive"]);
                                string imageUrl = dt.Rows[0]["imageurl"].ToString();
                                imgCategory.ImageUrl = string.IsNullOrEmpty(imageUrl)
                                    ? ResolveUrl("~/images/category/no_image.png")
                                    : ResolveUrl(imageUrl);
                                imgCategory.Height = 200;
                                imgCategory.Width = 200;
                                hdnId.Value = dt.Rows[0]["categoryid"].ToString();
                                hdnImagePath.Value = imageUrl;
                                btnAddorUpdate.Text = "Update";
                            }
                        }
                    }
                }

                // Change the style of the edit button
                LinkButton btn = e.Item.FindControl("lnkEdit") as LinkButton;
                if (btn != null)
                {
                    btn.CssClass = "badge badge-warning";
                }
            }
            else if (e.CommandName == "Delete")
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "DELETE FROM categories WHERE categoryid = @categoryid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@categoryid", e.CommandArgument);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            lblMsg.Visible = true;
                            lblMsg.Text = "Category deleted successfully!";
                            lblMsg.CssClass = "alert alert-success";
                            getcategories();  // Refresh the category list
                        }
                        catch (Exception ex)
                        {
                            lblMsg.Visible = true;
                            lblMsg.Text = "Error - " + ex.Message;
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }
            }
        }


        // Method to clear form fields
        private void ClearForm()
        {
            txtname.Text = string.Empty;
            cbIsactive.Checked = false;
            hdnId.Value = "0";
            hdnImagePath.Value = string.Empty;
            imgCategory.ImageUrl = "~/images/no_image.png"; // Reset image to default
            btnAddorUpdate.Text = "Add";
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected string GetImageUrl(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                return ResolveUrl(imageUrl);
            }
            return ResolveUrl("~/images/category/no_image.png"); // Fallback image if no URL provided
        }
    }
}
