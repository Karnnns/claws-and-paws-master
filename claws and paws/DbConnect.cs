using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace claws_and_paws
{

    internal class DbConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        private string con;

        public string connection()
        {
            con = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Karan\Downloads\claws-and-paws-master\claws-and-paws-master\claws and paws\dbPetShop.mdf;Integrated Security=True;Connect Timeout=30";
            return con;
        }

        public void executeQuery(string sql)
        {
            try
            {
                cn.ConnectionString = connection();
                cn.Open();
                cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}