using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Org.BouncyCastle.Asn1.Ocsp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Net;
using System.Drawing;
using Font = iTextSharp.text.Font;

namespace smartshop.user
{
    public partial class invoice : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sda;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["userid"] != null)
                {
                    if (Request.QueryString["id"] != null)
                    {
                        rorderitem.DataSource = getorderdetails();
                        rorderitem.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("login.aspx");
                }
            }
        }
        DataTable getorderdetails()
        {
            double grandtotal = 0;

            // Initialize the connection and the SQL command with the direct query
            using (SqlConnection con = new SqlConnection(connection.getconnectionString()))
            {
                string query = @"
            SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [srno],
                   o.orderno,
                   p.name,
                   p.price,
                   o.quantity,
                   (p.price * o.quantity) AS totalprice,
                   o.orderdate,
                   o.status
            FROM orders o
            INNER JOIN products p ON p.productid = o.productid
            WHERE o.paymentid = @paymentid AND o.userid = @userid";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    // Add the parameters for the query
                    cmd.Parameters.AddWithValue("@paymentid", Convert.ToInt32(Request.QueryString["id"]));
                    cmd.Parameters.AddWithValue("@userid", Session["userid"]);

                    // Create a SqlDataAdapter to fill the DataTable
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    // Open the connection, fill the DataTable and close the connection
                    con.Open();
                    sda.Fill(dt);
                    con.Close();

                    // If rows exist, calculate the grand total
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow drow in dt.Rows)
                        {
                            grandtotal += Convert.ToDouble(drow["totalprice"]);
                        }
                    }

                    // Add a new row for grand total at the end of the table
                    DataRow dr = dt.NewRow();
                    dr["totalprice"] = grandtotal;
                    dt.Rows.Add(dr);

                    return dt;
                }
            }
        }


        protected void lbdownloadinvoice_Click(object sender, EventArgs e)
        {
            try
            {
                string downloadpath = @"E:\order_invoice.pdf";
                DataTable dtbl=getorderdetails();
                exporttopdf(dtbl, downloadpath, "Order Invoice");
                WebClient client = new WebClient();
                Byte[] buffer=client.DownloadData(downloadpath);
                if (buffer != null)
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-length",buffer.Length.ToString());
                    Response.BinaryWrite(buffer);
                }
            }
            catch (Exception ex)
            {
                lblmsg.Visible = true;
                lblmsg.Text = "Error Message:- " + ex.Message.ToString();
            }
        }

        void exporttopdf(DataTable dtbltable, string strpdfpath, string strheader)
        {
            // Create a file stream
            using (FileStream fs = new FileStream(strpdfpath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                // Initialize document and set page size
                Document document = new Document();
                document.SetPageSize(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // Set font for the header
                BaseFont bfnthead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fnthead = new Font(bfnthead, 16, Font.BOLD, BaseColor.GRAY);
                Paragraph prgheading = new Paragraph();
                prgheading.Alignment = Element.ALIGN_CENTER;
                prgheading.Add(new Chunk(strheader.ToUpper(), fnthead));
                document.Add(prgheading);

                // Add author and order details
                BaseFont btnauthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntauthor = new Font(btnauthor, 8, Font.ITALIC, BaseColor.GRAY);
                Paragraph prgauthor = new Paragraph();
                prgauthor.Alignment = Element.ALIGN_CENTER;
                prgauthor.Add(new Chunk("Order From : Foodie Fast Food", fntauthor));
                prgauthor.Add(new Chunk("\nOrder Date : " + dtbltable.Rows[0]["orderdate"].ToString(), fntauthor));
                document.Add(prgauthor);

                // Add a line separator
                Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_CENTER, -1)));
                document.Add(p);

                // Add a new line
                document.Add(new Chunk("\n", fnthead));

                // Create a table with the number of columns minus two
                PdfPTable table = new PdfPTable(dtbltable.Columns.Count - 2);

                // Add column headers
                BaseFont btncolumnheader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fntcolumnheader = new Font(btncolumnheader, 9, Font.BOLD, BaseColor.WHITE);
                for (int i = 0; i < dtbltable.Columns.Count - 2; i++)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.GRAY;
                    cell.AddElement(new Chunk(dtbltable.Columns[i].ColumnName.ToUpper(), fntcolumnheader));
                    table.AddCell(cell);
                }

                // Add rows
                Font fntcolumndata = new Font(btncolumnheader, 8, Font.NORMAL, BaseColor.BLACK);
                for (int i = 0; i < dtbltable.Rows.Count; i++)
                {
                    for (int j = 0; j < dtbltable.Columns.Count - 2; j++)
                    {
                        PdfPCell cell = new PdfPCell();
                        cell.AddElement(new Chunk(dtbltable.Rows[i][j].ToString(), fntcolumndata));
                        table.AddCell(cell);
                    }
                }

                // Add the table to the document
                document.Add(table);

                // Close the document and writer
                document.Close();
                writer.Close();
            }
        }

    }
}