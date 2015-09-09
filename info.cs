using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class info : Form
    {
        
        public string MyProperty { get; set; }
       
        public info()
        {
            InitializeComponent();
           
        
        }
       
           
        
       

        private void info_Load(object sender, EventArgs e)
        {
            string nam = this.MyProperty.ToString();
            textBox1.Text = nam;
            string server = "MYSQL5009.HostBuddy.com";
            string database = "db_9cf135_hawk";
            string uid = "9cf135_hawk";
            string password = "ticcttmosfet";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user where `id`='" + textBox1.Text + "'", con);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                textBox2.Text = dr[1].ToString();
                textBox3.Text = dr[2].ToString();
                textBox4.Text = dr[3].ToString();
                textBox5.Text = dr[4].ToString();
                textBox6.Text = dr[5].ToString();
                textBox7.Text = dr[6].ToString();
                textBox12.Text = dr[7].ToString();
                textBox13.Text = dr[8].ToString();
                pictureBox1.ImageLocation = dr[9].ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            edit frmm = new edit();
            frmm.MyProperty = textBox1.Text;
            frmm.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        
    }
}
