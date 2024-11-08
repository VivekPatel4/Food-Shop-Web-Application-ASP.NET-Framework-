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
    public partial class index1 : System.Web.UI.Page
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
    }
}