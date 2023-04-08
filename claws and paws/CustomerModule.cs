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
    public partial class CustomerModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DbConnect dbCon = new DbConnect();

        string title = "Pet Shop Management System";

        bool check = false;

        CustomerForm customer;
        private CashCustomer cashCustomer;

        public CustomerModule(CustomerForm form)
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            customer = form;
        }

        public CustomerModule(CashCustomer cashCustomer)
        {
            InitializeComponent();
            cn = new SqlConnection(dbCon.connection());
            this.cashCustomer = cashCustomer;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to register this Customer?", "User Registration", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("INSERT INTO tbCustomer(name,address,phone)VALUES(@name,@address,@phone)", cn);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);

                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("Customer has been successfully registered!", title);

                        clear();
                        if(customer!=null)
                        customer.LoadCustomer();
                        else if(cashCustomer!=null)
                        cashCustomer.LoadCustomer();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, title);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                checkField();
                if (check)
                {
                    if (MessageBox.Show("Are you sure you want to Edit this record?", "Record Edited", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cm = new SqlCommand("UPDATE tbCustomer SET name=@name, address=@address, phone=@phone  WHERE id=@id", cn);
                        cm.Parameters.AddWithValue("@id", lblcid.Text);
                        cm.Parameters.AddWithValue("@name", txtName.Text);
                        cm.Parameters.AddWithValue("@address", txtAddress.Text);
                        cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                        
                        cn.Open();
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("Customer has been successfully updated!", title);
                        customer.LoadCustomer();
                        clear();
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #region method

        public void clear()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();

            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        public void checkField()
        {
            if (txtName.Text == "" | txtAddress.Text == "" | txtPhone.Text == "")
            {
                MessageBox.Show("Required data field", "Warning!");
                return;
            }

            check = true;
        }

        #endregion

    }
}
