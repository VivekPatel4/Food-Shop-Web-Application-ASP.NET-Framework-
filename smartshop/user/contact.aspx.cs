using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace smartshop.user
{
    public partial class contact : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
                {
                    string query = "INSERT INTO contact (name, email, subject, message, createddate) " +
                                   "VALUES (@name, @email, @subject, @message, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        
                        cmd.Parameters.AddWithValue("@name", txtname.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtemail.Text.Trim());
                        cmd.Parameters.AddWithValue("@subject", txtsubject.Text.Trim());
                        cmd.Parameters.AddWithValue("@message", txtmessage.Text.Trim());
                       
                       
                        con.Open();

                        
                        cmd.ExecuteNonQuery();

                        
                        lblMsg.Visible = true;
                        lblMsg.Text = "Thanks for reaching out. We will look into your query.";
                        lblMsg.CssClass = "alert alert-success";

                        
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.CssClass = "alert alert-danger";
            }
        }


        private void Clear()
        {
            txtname.Text = string.Empty;
            txtemail.Text= string.Empty;
            txtsubject.Text = string.Empty;
            txtmessage.Text = string.Empty;

        }
    }
}