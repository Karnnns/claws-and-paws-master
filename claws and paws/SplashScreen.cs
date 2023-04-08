using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace claws_and_paws
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

       

            int startPoint = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startPoint += 2;
            guna2ProgressBar1.Value = startPoint;
            if(guna2ProgressBar1.Value == 100)
            {
                guna2ProgressBar1.Value = 0;
                timer1.Stop();
                Login login = new Login();
                login.ShowDialog();
                this.Close();
            }
        }

        public static void progressBar()
        {

        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
