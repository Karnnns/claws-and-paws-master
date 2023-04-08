using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace claws_and_paws
{
    public partial class CashForm : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();
        SqlDataReader dr;

        string title = "Pet Shop Management System";

        private int numberOfItemsPerPage = 0;
        private int numberOfItemsPrintedSoFar = 0;

        double cGst = 0;
        double sGst = 0;
        double gTotal;

        Main main;

        public CashForm(Main form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            main = form;
            getTransNo();
            loadCash();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CashProduct product = new CashProduct(this);
            product.uname = main.lblUsername.Text;
            product.ShowDialog();
        }

        #region method

        public void getTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                int count;
                string transno;

                cn.Open();
                cm = new SqlCommand("SELECT TOP 1 transno FROM tbCash WHERE transno LIKE '" + sdate + "%' ORDER BY cashid DESC ",cn);
                dr = cm.ExecuteReader();
                dr.Read();

                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001";
                    lblTransno.Text = transno;
                }
                dr.Close();
                cn.Close();

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        public double loadCash()
        {
            try
            {
                int i = 0;
                double total = 0;

                dgvCash.Rows.Clear();
                cm = new SqlCommand("SELECT cashid,pcode,pname,qty,price,total,c.name,cashier FROM tbCash as cash LEFT JOIN tbCustomer c ON cash.cid = c.id WHERE transno LIKE "+ lblTransno.Text +"", cn);
                cn.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dgvCash.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
                    total += double.Parse(dr[5].ToString());
                }
                if(!dr.HasRows)
                {
                    btnCash.Enabled = false;
                }
                else
                {
                    btnCash.Enabled = true;
                }
                dr.Close();
                cn.Close();

                lblTotal.Text = total.ToString("#,##0.00");

                cGst = total * 0.025;
                sGst = cGst;

              return  gTotal = cGst + sGst + total;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
                throw;
            }
        }

        public int checkPqty(string pcode)
        {
            int i = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT pqty FROM tbProduct WHERE pcode LIKE '" + pcode + "'", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            return i;
        }

        #endregion

        private void dgvCash_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCash.Columns[e.ColumnIndex].Name;
            removeItem:

            if(colName == "DeleteC")
            {
                if (MessageBox.Show("Are you sure you want to delete this record?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dbCon.executeQuery("DELETE FROM tbCash WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("item record has been successfully removed", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            else if (colName == "Increase")
            {
                int i = checkPqty(dgvCash.Rows[e.RowIndex].Cells[2].Value.ToString());
                if(int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) < i)
                {
                    dbCon.executeQuery("UPDATE tbCash SET qty = qty + " + 1 + " WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                }
                else
                {
                    MessageBox.Show("Remaining quantity on hand is "+ i + " ! ","Out of Stock",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }

            }
            else if (colName == "Decrease")
            {
                if (int.Parse(dgvCash.Rows[e.RowIndex].Cells[4].Value.ToString()) == 1)
                {
                    colName = "DeleteC";
                    goto removeItem;
                }
                dbCon.executeQuery("UPDATE tbCash SET qty = qty - " + 1 + " WHERE cashid LIKE '" + dgvCash.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
            }
            loadCash();
        }

        private void btnCash_Click(object sender, EventArgs e)
        {
            CashCustomer customer = new CashCustomer(this);
            customer.ShowDialog();
            if(MessageBox.Show("Are you sure you want to cash this product?","Cashing",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                getTransNo();
                main.loadSalesReports();
                for(int i = 0; i < dgvCash.Rows.Count; i++)
                {
                    dbCon.executeQuery("UPDATE tbProduct SET pqty= pqty - " + int.Parse(dgvCash.Rows[i].Cells[4].Value.ToString()) + "WHERE pcode LIKE "+ dgvCash.Rows[i].Cells[2].Value.ToString() +"");
                }
                if(MessageBox.Show("Are you want to print invoice","Print!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){
                    if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                    {
                        printDocument1.Print();
                    }
                }
               

                dgvCash.Rows.Clear();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string curdhead = "Claws & Paws";
            string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            
            e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold), Brushes.Black, 350, 10);
            e.Graphics.DrawString("Pet Shop", new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold), Brushes.Black, 370, 30);
            e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 60);

            e.Graphics.DrawString("#:" + lblTransno.Text.Substring(2) + "", new Font("Lucida Console", 10, FontStyle.Regular), Brushes.Black, new Point(200, 80));
            e.Graphics.DrawString("Date:" + DateTime.Now.ToString("d") + "", new Font("Lucida Console", 10, FontStyle.Regular), Brushes.Black, new Point(200, 100));
            e.Graphics.DrawString("Time:" + DateTime.Now.ToString("t") + "", new Font("Lucida Console", 10, FontStyle.Regular), Brushes.Black, new Point(500, 100));


            //e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 120);

            e.Graphics.DrawString("----- Tax Invoice -----", new Font("Lucida Console", 10, FontStyle.Bold), Brushes.Black, new Point(320, 125));

            string g2 = "Items";
            e.Graphics.DrawString(g2, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 300, 150);

            string g5 = "Qty";
            e.Graphics.DrawString(g5, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 400, 150);

            string g6 = "Price";
            e.Graphics.DrawString(g6, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black,450, 150);

            string g7 = "Total";
            e.Graphics.DrawString(g7, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 550, 150);

            string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 170);

            int height = 172;
            int qty = 0;
            string cName = "";
            for (int l = numberOfItemsPrintedSoFar; l < dgvCash.Rows.Count; l++)
            {
                numberOfItemsPerPage = numberOfItemsPerPage + 1;
                if (numberOfItemsPerPage <= 50)
                {
                    numberOfItemsPrintedSoFar++;

                    if (numberOfItemsPrintedSoFar <= dgvCash.Rows.Count)
                    {

                        height += dgvCash.Rows[0].Height;
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[3].FormattedValue.ToString().Length > 15 ? dgvCash.Rows[l].Cells[3].FormattedValue.ToString().Substring(0, 12) + "..." : dgvCash.Rows[l].Cells[3].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(300, height, dgvCash.Columns[0].Width * 5, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[4].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(400, height, dgvCash.Columns[0].Width, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[5].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(450, height, dgvCash.Columns[0].Width*2, dgvCash.Rows[0].Height));
                        e.Graphics.DrawString(dgvCash.Rows[l].Cells[6].FormattedValue.ToString(), dgvCash.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(550, height, dgvCash.Columns[0].Width * 3, dgvCash.Rows[0].Height));

                        cName = dgvCash.Rows[l].Cells[7].Value.ToString();
                        qty += int.Parse(dgvCash.Rows[l].Cells[4].Value.ToString());
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
            e.Graphics.DrawString("Customer Name :" + cName + "", new Font("Lucida Console", 10, FontStyle.Regular), Brushes.Black, new Point(500, 80));

            string cgst = cGst.ToString("#,##0.00");
            string sgst = sGst.ToString("#,##0.00");

            //string l3 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, height + 20);

            e.Graphics.DrawString("@ CGST\n" + "@ SGST\n" + "Total", new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 450, height + 40);
            e.Graphics.DrawString( cgst +"\n"+ sgst + "\n" + gTotal + "", new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 550, height + 40);

            e.Graphics.DrawString(qty.ToString(), new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 400, height + 40);


            numberOfItemsPerPage = 0;
            numberOfItemsPrintedSoFar = 0;


            //lblTotal.Text = gTotal.ToString("#,##0.00");
            //string cgst = cGst.ToString("#,##0.00");
            //String sgst = cgst;

            //e.Graphics.DrawString("Price \n\n" + str3 + "--------------\n" +"@ CGST\n"+ cgst +"\n" +"@ SGST\n" + sgst+ "\n" +lblTotal.Text + "", new Font("Lucida Console", 18, FontStyle.Regular), Brushes.Black, new Point(470, 300)); ;
            //e.Graphics.DrawString("" + lblTotal.Text + "", new Font("Century Gothic", 18, FontStyle.Regular), Brushes.Black, new Point(400, 250));

           

        }
    }
}
