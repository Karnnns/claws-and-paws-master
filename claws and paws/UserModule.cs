using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace claws_and_paws
{
    public partial class UserModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();

        string title = "Pet Shop Management System";

        bool check = false;

        UserForm userForm;
        public UserModule(UserForm user)
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            userForm = user;
            cbRole.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this user?", "User Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbUser(name,address,phone,role,dob,password)VALUES(@name,@address,@phone,@role,@dob,@password)", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", DtDob.Value);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("User has been successfully registered!", title);

                        clear();
                        userForm.LoadUser();
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }
       

        private void UserModule_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRole.Text=="Employee")
            {
                this.Height = 450 - 30;
                lblPass.Visible= false;
                txtPass.Visible= false;
            }
            else
            {
                lblPass.Visible = true;
                txtPass.Visible = true;
                this.Height = 450;
            }
        }

        // this region - endregion is used to keep code organized
        #region method

        public void clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            cbRole.SelectedItem = 0;
            DtDob.Value = DateTime.Now;
            txtPass.Clear();

            btnUpdate.Enabled = false;
        }

        public void checkField()
        {
            if(txtName.Text == "" | txtAddress.Text == "")
            {
                MessageBox.Show("Required data field", "Warning!");
                return;
            }

            if(checkAge(DtDob.Value) < 18)
            {
                MessageBox.Show("User is under 18", "Warning!");
                return;
            }
            check = true;
        }

        //to calculate age for under 18
        private static int checkAge(DateTime dateofBirth)
        {
            int age = DateTime.Now.Year - dateofBirth.Year;
            if (DateTime.Now.DayOfYear < dateofBirth.DayOfYear)
                age = age - 1;
            return age;
        }

        #endregion

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to update this record?", "Edit Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbUser SET name = @name ,address = @address ,phone = @phone ,role = @role,dob = @dob,password = @password WHERE id = @id", cn);

                        cm.Parameters.AddWithValue("@id", lbluid.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        cm.Parameters.AddWithValue("@role", cbRole.Text);
                        cm.Parameters.AddWithValue("@dob", DtDob.Value);
                        cm.Parameters.AddWithValue("@password", txtPass.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("User has been successfully updated!", title);

                        clear();
                        userForm.LoadUser();
                        this.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, title);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
