using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class name : Form
    {
        public name()
        {

            
            InitializeComponent();
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

        
      

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            AutoCompleteStringCollection names = new AutoCompleteStringCollection();
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            string server = "MYSQL5009.HostBuddy.com";
            string database = "db_9cf135_hawk";
            string uid = "9cf135_hawk";
            string password = "ticcttmosfet";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cms = new MySqlCommand("select * from user", con);
            MySqlDataReader dr = cms.ExecuteReader();
            while (dr.Read())
            {
                names.Add(dr[1].ToString());
            }
         
            dr.Close();
            con.Close();
            textBox1.AutoCompleteCustomSource = names;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string server = "MYSQL5009.HostBuddy.com";
            string database = "db_9cf135_hawk";
            string uid = "9cf135_hawk";
            string password = "ticcttmosfet";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user where `name`='" + textBox1.Text + "'", con);
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

 
        
    }
}
