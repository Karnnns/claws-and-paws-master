using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace claws_and_paws
{
    public partial class TransactionsForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();
        SqlDataReader dr;

        string title = "Pet Shop Management System";

        private int numberOfItemsPerPage = 0;
        private int numberOfItemsPrintedSoFar = 0;

        public TransactionsForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            loadCash();
        }

        #region Method

   




        // string n = DateTime.Now.ToString("yyyyMM");

        public void loadCash()
        {
            try
            {
                int i = 0;
                double total = 0;

                dgvCash.Rows.Clear();
                cm = new SqlCommand("SELECT transno,p.pname,p.pcategory,qty,price,total,c.name FROM tbCash as cash LEFT JOIN tbCustomer c ON cash.cid = c.id LEFT JOIN tbProduct p ON cash.pcode = p.pcode  WHERE CONCAT( transno,p.pname,p.pcategory,c.name) LIKE '%" + txtSearch.Text + "%'", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
                    total += double.Parse(dr[5].ToString());
                }
                dr.Close();
                cn.Close();
                lblTotal.Text = total.ToString("#,##0.00");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            loadCash();
        }

        private void btnInfo_MouseHover(object sender, EventArgs e)
        {
            lblToast.Visible = true;
        }

        private void btnInfo_MouseLeave(object sender, EventArgs e)
        {
            lblToast.Visible = false;
        }

   

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string curdhead = "Transactions";
            e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua",15 , FontStyle.Bold), Brushes.Black, 350, 50);

            string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 100);

            string g1 = "#";
            e.Graphics.DrawString(g1, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 50, 125);

            string g2 = "Transaction No.";
            e.Graphics.DrawString(g2, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 80, 125);

            string g3 = "Products";
            e.Graphics.DrawString(g3, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 200, 125);

            string g4 = "Category";
            e.Graphics.DrawString(g4, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 300, 125);

            string g5 = "Qty";
            e.Graphics.DrawString(g5, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 400, 125);

            string g6 = "Price";
            e.Graphics.DrawString(g6, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 500, 125);

            string g7 = "Total";
            e.Graphics.DrawString(g7, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 600, 125);

            string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 150);

            int height = 157;
            //int qty = 0;
            for (int l = numberOfItemsPrintedSoFar; l < dgvCash.Rows.Count; l++)
            {
                numberOfItemsPerPage = numberOfItemsPerPage + 1;
                if (numberOfItemsPerPage <= 40)
                {
                    numberOfItemsPrintedSoFar++;

                    if (numberOfItemsPrintedSoFar <= dgvCash.Rows.Count)
                    {

                        height += dgvCash.Rows[0].Height;
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[0].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(50, height, dgvCash.Columns[0].Width, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[1].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(80, height, dgvCash.Columns[0].Width * 3, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[2].FormattedValue.ToString().Length > 15 ? dgvCash.Rows[l].Cells[2].FormattedValue.ToString().Substring(0,12)+"...": dgvCash.Rows[l].Cells[2].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(200, height, dgvCash.Columns[0].Width * 5, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[3].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(300, height, dgvCash.Columns[0].Width * 2, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[4].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(400, height, dgvCash.Columns[0].Width, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[5].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(500, height, dgvCash.Columns[0].Width * 2, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[6].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(600, height, dgvCash.Columns[0].Width * 2, dgvCash.Rows[0].Height));

                    }
                    else
                    {
                     
                        e.HasMorePages = false;                     
                    }
                   
                }
                else
                {
                    numberOfItemsPerPage = 0;
                    e.HasMorePages = true;

               // qty = qty + int.Parse(dgvCash.Rows[l].Cells[4].FormattedValue.ToString());
                    return;

                }
             


            }
            //string l3 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, height + 20);
            e.Graphics.DrawString(lblTotal.Text, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 600, height + 40);
            e.Graphics.DrawString("Total", new System.Drawing.Font("Book Antiqua", 11, FontStyle.Bold), Brushes.Black, 500, height + 40);

            //e.Graphics.DrawString(qty.ToString(), new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 400, height + 40);


            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                System.Windows.Forms.PrintDialog PrintDialog1 = new PrintDialog();
                PrintDialog1.AllowSomePages = true;
                PrintDialog1.ShowHelp = true;
                PrintDialog1.Document = printDocument1;
                DialogResult result = PrintDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    printDocument1.Print();
                }
            }
        }
    }
}
