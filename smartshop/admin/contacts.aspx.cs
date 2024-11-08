using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mysqlx.Session;

namespace smartshop.admin
{
    public partial class contacts : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadcrum"] = "Contact Users";
                if (Session["zipping"] == null)
                {
                    Response.Redirect("../user/login.aspx");
                }
                else
                {

                    getcontacts();

                }
            }
        }

        private void getcontacts()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string sqlQuery = "SELECT ROW_NUMBER() OVER (ORDER BY(SELECT 1)) AS [srno],* FROM contact";

                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            con.Open();
                            sda.Fill(dt);
                            rcontacts.DataSource = dt;
                            rcontacts.DataBind();
                        }
                        catch (SqlException sqlEx)
                        {
                            lblMsg.Text = "SQL error occurred: " + sqlEx.Message;
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

        protected void rcontacts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                {
                    string sqlQuery = "DELETE FROM contact WHERE contactid = @contactid";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@contactid", e.CommandArgument);
                        try
                        {
                            con.Open();
                            cmd.ExecuteNonQuery();
                            lblMsg.Visible = true;
                            lblMsg.Text = "record deleted successfully!";
                            lblMsg.CssClass = "alert alert-success";
                            getcontacts();  // Refresh the category list
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