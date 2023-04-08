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
    public partial class Main : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();
       // SqlDataReader dr;

        string title = "Pet Shop Management System";
        public Main()
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            btnDashboard.PerformClick();
            loadSalesReports();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.IsHandleCreated)
            {
                progress.Invoke((MethodInvoker)delegate
                {
                    progress.Text = DateTime.Now.ToString("hh:mm:ss");
                    progress.Value = Convert.ToInt32(DateTime.Now.Second);
                });
            }
            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            openChildForm(new UserForm());
        }

       

        private void btnProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new ProductForm());
        }


        #region Method

        private Form activeForm = null;
        public void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            lblTitle.Text = childForm.Text;
            panelChild.Controls.Add(childForm);
            panelChild.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        public void loadSalesReports()
        {
            string sDate = DateTime.Now.ToString("yyyyMMdd");
            string mDate = DateTime.Now.ToString("yyyyMM");

            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(total), 0) AS total FROM tbCash WHERE transno LIKE'" + sDate + "%'", cn);
                lblDailySale.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");

                cm = new SqlCommand("SELECT ISNULL(SUM(total), 0) AS total FROM tbCash WHERE transno LIKE'" + mDate + "%'", cn);
                lblMonthlySale.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");

                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        #endregion

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            openChildForm(new CustomerForm());
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            openChildForm(new CashForm(this));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Logout Application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Dispose();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new Dashboard());
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnTrans_Click(object sender, EventArgs e)
        {
            openChildForm(new TransactionsForm());
        }
    }
}
