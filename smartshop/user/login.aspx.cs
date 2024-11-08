using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using smartshop.admin;

namespace smartshop.user
{
    public partial class login : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userid"] != null)
            {
                Response.Redirect("index.aspx");
            }
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            if (txtusername.Text.Trim() == "zipping" && txtpassword.Text.Trim() == "1234")
            {
                // If the username and password are hardcoded (e.g., admin login)
                Session["zipping"] = txtusername.Text.Trim();
                Response.Redirect("../admin/dashboard.aspx");
            }
            else
            {

                using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                {

                    string sqlQuery = "SELECT * FROM users WHERE username=@username AND password=@password";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {

                        cmd.Parameters.AddWithValue("@username", txtusername.Text.Trim());
                        cmd.Parameters.AddWithValue("@password", txtpassword.Text.Trim());


                        SqlDataAdapter sda = new SqlDataAdapter(cmd);


                        DataTable dt = new DataTable();


                        sda.Fill(dt);


                        if (dt.Rows.Count == 1)
                        {

                            Session["username"] = txtusername.Text.Trim();
                            Session["userid"] = dt.Rows[0]["userid"];


                            Response.Redirect("index.aspx");
                        }
                        else
                        {

                            lblMsg.Visible = true;
                            lblMsg.Text = "Invalid username or password.";
                            lblMsg.CssClass = "alert alert-danger";
                        }
                    }
                }

            }
        }
    }
}