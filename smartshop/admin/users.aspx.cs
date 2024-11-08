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
    public partial class users : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = "Users";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }
                else
                {

                    getusers();

                }
            }
        }


        private void getusers()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT ROW_NUMBER() OVER (ORDER BY(SELECT 1)) AS [userid],userid,name,username,email,createddate FROM users";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);
                            ruser.DataSource = dt;
                            ruser.DataBind();
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



        protected void ruser_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                {
                    string sqlQuery = "DELETE FROM users WHERE userid = @userid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@userid", e.CommandArgument);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            lblMsg.Visible = true;
                            lblMsg.Text = "user deleted successfully!";
                            lblMsg.CssClass = "alert alert-success";
                            getusers();  // Refresh the category list
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

    }

}