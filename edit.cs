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
    public partial class edit : Form
    {
        
        public string MyProperty { get; set; }
        public edit()
        {
            InitializeComponent();
            
        }

        private void edit_Load(object sender, EventArgs e)
        {
            string name = this.MyProperty.ToString();
            textBox1.Text=name;
            string server = "MYSQL5009.HostBuddy.com";
            string database = "db_9cf135_hawk";
            string uid = "9cf135_hawk";
            string password = "ticcttmosfet";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("select * from user where `id`='"+name+"'", con);
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
              
            }
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string server = "sql6.freemysqlhosting.net";
            string database = "sql682012";
            string uid = "sql682012";
            string password = "iB1%eB5!";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            
            MySqlConnection con = new MySqlConnection(connectionString);
            con.Open();
            MySqlCommand cmd2 = new MySqlCommand("update user set `name`='" + textBox2.Text + "',`age`='" + textBox3.Text + "',`branch`='" + textBox4.Text + "',`email`='" + textBox5.Text + "',`facebook`='" + textBox6.Text + "',`phone`='" + textBox7.Text + "',`status`='" + textBox12.Text + "',`dob`='" + textBox13.Text + "' where `id`='" + textBox1.Text + "'", con);
            cmd2.ExecuteNonQuery();
            con.Close();
            this.Close();
            
        }

        
            
        }
    }

