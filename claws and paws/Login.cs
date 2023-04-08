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
    public partial class Login : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();
        SqlDataReader dr;

        string title = "Pet Shop Management System";
        public Login()
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string _name = "", _role = "";
                cn.Open();
                cm = new SqlCommand("SELECT name,role FROM tbUser WHERE name=@name and password=@password", cn);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    _name = dr["name"].ToString();
                    _role = dr["role"].ToString();
                    MessageBox.Show("Welcome " + _name + " | ", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Main main = new Main();
                    main.lblUsername.Text = _name;
                    main.lblRole.Text = _role;

                    if (_role == "Administrator")
                        main.btnUser.Enabled = true;

                    this.Hide();
                    main.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid Credentials!", "ACCESS DENIED", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                    cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnForgetPass_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your Boss!", "FORGET PASSWORD", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Exit Application?","Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void txtPass_IconRightClick_1(object sender, EventArgs e)
        {
            if(txtPass.PasswordChar == char.Parse("●")){
                txtPass.UseSystemPasswordChar = false;
                txtPass.PasswordChar = char.Parse("\0");
            }
            else
            {
                txtPass.UseSystemPasswordChar = true;
                txtPass.PasswordChar = char.Parse("●");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
