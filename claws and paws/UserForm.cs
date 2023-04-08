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
    public partial class UserForm : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();
        SqlDataReader dr;

        string title = "Pet Shop Management System";

        private int numberOfItemsPerPage = 0;
        private int numberOfItemsPrintedSoFar = 0;
        public UserForm()
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            LoadUser();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                UserModule module = new UserModule(this);
                module.lbluid.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                module.txtName.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                module.txtAddress.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                module.txtPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();
                module.cbRole.Text = dgvUser.Rows[e.RowIndex].Cells[5].Value.ToString();
                module.DtDob.Text = dgvUser.Rows[e.RowIndex].Cells[6].Value.ToString();
                module.txtPass.Text = dgvUser.Rows[e.RowIndex].Cells[7].Value.ToString();

                module.btnSave.Enabled = false;
                module.btnUpdate.Enabled = true;
                module.ShowDialog();

            }else if(colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this record","Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                {
                    dbCon.executeQuery("DELETE FROM tbUser WHERE id LIKE '" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("User data has been successfully removed", title, MessageBoxButtons.OK, MessageBoxIcon.Question);
                }
            }
            LoadUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModule module = new UserModule(this);
            module.ShowDialog();
        }

        #region method

        public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser WHERE CONCAT(name,address,phone,dob,role) LIKE '%" + txtSearch.Text + "%'",cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }


        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            string curdhead = "User Details";
            e.Graphics.DrawString(curdhead, new System.Drawing.Font("Book Antiqua", 15, FontStyle.Bold), Brushes.Black, 350, 50);

            string l1 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l1, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 100);

            string g1 = "#";
            e.Graphics.DrawString(g1, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 50, 125);

            string g2 = "NAME";
            e.Graphics.DrawString(g2, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 80, 125);

            string g3 = "ADDRESS";
            e.Graphics.DrawString(g3, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 200, 125);

            string g4 = "CONTACT";
            e.Graphics.DrawString(g4, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 450, 125);

            string g5 = "ROLE";
            e.Graphics.DrawString(g5, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 550, 125);


            string l2 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, 150);

            int height = 157;
           // int qty = 0;
            for (int l = numberOfItemsPrintedSoFar; l < dgvUser.Rows.Count; l++)
            {
                numberOfItemsPerPage = numberOfItemsPerPage + 1;
                if (numberOfItemsPerPage <= 50)
                {
                    numberOfItemsPrintedSoFar++;

                    if (numberOfItemsPrintedSoFar <= dgvUser.Rows.Count)
                    {

                        height += dgvUser.Rows[0].Height;
                        e.Graphics.DrawString(dgvUser.Rows[l].Cells[0].FormattedValue.ToString(), dgvUser.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(50, height, dgvUser.Columns[0].Width, dgvUser.Rows[0].Height));
                        e.Graphics.DrawString(dgvUser.Rows[l].Cells[2].FormattedValue.ToString().Length > 15 ? dgvUser.Rows[l].Cells[2].FormattedValue.ToString().Substring(0, 12) + "..." : dgvUser.Rows[l].Cells[2].FormattedValue.ToString(), dgvUser.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(80, height, dgvUser.Columns[0].Width * 2, dgvUser.Rows[0].Height));
                        e.Graphics.DrawString(dgvUser.Rows[l].Cells[3].FormattedValue.ToString().Length > 30 ? dgvUser.Rows[l].Cells[3].FormattedValue.ToString().Substring(0, 27) + "..." : dgvUser.Rows[l].Cells[3].FormattedValue.ToString(), dgvUser.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(200, height, dgvUser.Columns[0].Width * 6, dgvUser.Rows[0].Height));
                        e.Graphics.DrawString(dgvUser.Rows[l].Cells[4].FormattedValue.ToString(), dgvUser.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(450, height, dgvUser.Columns[0].Width * 2, dgvUser.Rows[0].Height));
                        e.Graphics.DrawString(dgvUser.Rows[l].Cells[5].FormattedValue.ToString(), dgvUser.Font = new Font("Book Antiqua", 10), Brushes.Black, new RectangleF(550, height, dgvUser.Columns[0].Width * 2, dgvUser.Rows[0].Height));

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

                    // qty = qty + int.Parse(dgvUser.Rows[l].Cells[4].FormattedValue.ToString());
                    return;

                }



            }
            //string l3 = "---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
            e.Graphics.DrawString(l2, new System.Drawing.Font("Book Antiqua", 9, FontStyle.Bold), Brushes.Black, 0, height + 20);
            // e.Graphics.DrawString(lblTotal.Text, new System.Drawing.Font("Book Antiqua", 10, FontStyle.Bold), Brushes.Black, 600, height + 40);

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
