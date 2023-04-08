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
    public partial class Dashboard : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();
        //SqlDataReader dr;

        string title = "Pet Shop Management System";
        public Dashboard()
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            lblDogs.Text = extractData("Dog").ToString();
            lblCats.Text = extractData("Cat").ToString();
            lblFood.Text = extractData("Food").ToString();
            lblProducts.Text = extractData("Product").ToString();
        }

        #region Method
        public int extractData(string str)
        {
            int data = 0;
            try
            {
                cn.Open();
                cm = new SqlCommand("SELECT ISNULL(SUM(pqty), 0) AS qty FROM tbProduct WHERE pcategory = '"+ str +"'",cn);
                data = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
            return data;
        }
        #endregion

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
