using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace smartshop.user
{
    public partial class reg : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    getuserdetails();
                }
                else if (Session["id"] != null)
                {
                    Response.Redirect("index.aspx");
                }
            }
        }

        protected void btnregister_Click(object sender, EventArgs e)
        {
            string actionname = string.Empty, imagepath = string.Empty, fileextension = string.Empty;
            bool isvalidtoexecute = false;
            int userid = Convert.ToInt32(Request.QueryString["id"]); // Assuming id is passed via query string
            con = new SqlConnection(connection.getconnectionString());

            // Build SQL Query based on whether it's an INSERT or UPDATE
            string sqlQuery = userid == 0
                ? "INSERT INTO users (name, username, mobile, email, address, postcode, password, imageurl,createddate) VALUES (@name, @username, @mobile, @email, @address, @postcode, @password, @imageurl,GETDATE())"
                : "UPDATE users SET name=@name, username=@username, mobile=@mobile, email=@email, address=@address, postcode=@postcode, password=@password, imageurl=@imageurl WHERE userid=@userid";

            cmd = new SqlCommand(sqlQuery, con);
            cmd.Parameters.AddWithValue("@userid", userid); // This is used for UPDATE queries
            cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
            cmd.Parameters.AddWithValue("@username", txtusername.Text.Trim());
            cmd.Parameters.AddWithValue("@mobile", txtmobile.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtemail.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtaddress.Text.Trim());
            cmd.Parameters.AddWithValue("@postcode", txtpostcode.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtpassword.Text.Trim());

            // Inline file extension validation
            if (fuUserImage.HasFile)
            {
                if (IsValidImageExtension(fuUserImage.FileName))
                {
                    Guid obj = Guid.NewGuid();
                    fileextension = Path.GetExtension(fuUserImage.FileName);
                    imagepath = "~/images/user/" + obj.ToString() + fileextension;
                    fuUserImage.PostedFile.SaveAs(Server.MapPath(imagepath));
                    cmd.Parameters.AddWithValue("@imageurl", imagepath);
                    isvalidtoexecute = true;
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Please select a valid image (.jpg, .jpeg, or .png)";
                    lblMsg.CssClass = "alert alert-danger";
                    isvalidtoexecute = false;
                }
            }
            else
            {
                // Handle no image upload (set image to null or retain old image in case of update)
                cmd.Parameters.AddWithValue("@imageurl", DBNull.Value);
                isvalidtoexecute = true;
            }

            if (isvalidtoexecute)
            {
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    actionname = userid == 0
                        ? "Registration is successful! <b><a href='login.aspx'>Click here</a></b> to log in"
                        : "Details updated successfully! <b><a href='profile.aspx'>Check here</a></b>";

                    lblMsg.Visible = true;
                    lblMsg.Text = "<b>" + txtusername.Text.Trim() + "</b> " + actionname;
                    lblMsg.CssClass = "alert alert-success";

                    if (userid != 0)
                    {
                        Response.AddHeader("REFRESH", "1;URL=profile.aspx");
                    }
                    clear();
                }
                catch (SqlException ex)
                {
                    if (ex.Message.Contains("violation of UNIQUE KEY constraint"))
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "<b>" + txtusername.Text.Trim() + "</b> username already exists, try a new one.";
                        lblMsg.CssClass = "alert alert-danger";
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Error - " + ex.Message;
                    lblMsg.CssClass = "alert alert-danger";
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private bool IsValidImageExtension(string fileName)
        {
            string[] validExtensions = { ".jpg", ".jpeg", ".png" };
            string fileExtension = Path.GetExtension(fileName).ToLower();
            return Array.Exists(validExtensions, ext => ext == fileExtension);
        }

        void getuserdetails()
        {
            // Initialize connection
            con = new SqlConnection(connection.getconnectionString());

            // SQL Query to fetch user details
            string sqlQuery = "SELECT name, username, mobile, email, address, postcode, imageurl, password " +
                              "FROM users WHERE userid = @userid";

            cmd = new SqlCommand(sqlQuery, con);
            cmd.Parameters.AddWithValue("@userid", Request.QueryString["id"]);
            sda = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sda.Fill(dt);

            if (dt.Rows.Count == 1)
            {
                // Populate user details
                txtname.Text = dt.Rows[0]["name"].ToString();
                txtusername.Text = dt.Rows[0]["username"].ToString();
                txtmobile.Text = dt.Rows[0]["mobile"].ToString();
                txtemail.Text = dt.Rows[0]["email"].ToString();
                txtaddress.Text = dt.Rows[0]["address"].ToString();
                txtpostcode.Text = dt.Rows[0]["postcode"].ToString();

                // Get image URL from the database
                string imageUrl = dt.Rows[0]["imageurl"].ToString();

                // Log the image URL for debugging
                System.Diagnostics.Debug.WriteLine("Image URL from DB: " + imageUrl);

                // If image URL is empty or null, use the default image
                if (string.IsNullOrEmpty(imageUrl))
                {
                    imgUser.ImageUrl = ResolveUrl("~/images/no_image.png");
                }
                else
                {
                    // Ensure the image file exists on the server
                    string imagePath = Server.MapPath(imageUrl);
                    if (File.Exists(imagePath))
                    {
                        imgUser.ImageUrl = ResolveUrl(imageUrl);
                    }
                    else
                    {
                        // Fallback to default image if file is missing
                        imgUser.ImageUrl = ResolveUrl("~/images/no_image.png");
                    }
                }

                // Set image size
                imgUser.Height = 200;
                imgUser.Width = 200;

                // Set password field to read-only
                txtpassword.TextMode = TextBoxMode.SingleLine;
                txtpassword.ReadOnly = true;
                txtpassword.Text = dt.Rows[0]["password"].ToString();
            }
            else
            {
                // In case no user details are found, handle it
                lblMsg.Visible = true;
                lblMsg.Text = "User details not found.";
                lblMsg.CssClass = "alert alert-danger";
            }

            // Update UI elements
            lblHeaderMsg.Text = "<h2>Edit Profile</h2>";
            btnregister.Text = "Update";
            lblAlredyUser.Text = "";
        }


        private void clear()
        {
            txtname.Text = string.Empty;
            txtusername.Text = string.Empty;
            txtmobile.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtpostcode.Text = string.Empty;
            txtpassword.Text = string.Empty;
        }
    }
}
