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

namespace Delete_Meter
{



    public partial class Form1 : Form
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        string RecordToDelete;
        string Acct;
        string DBName = "PRODV4";
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = "Enter Account Number";
            label2.Text = "";
            label3.Text = "DataBase you are connected is "+DBName;
            button1.Text = "Load Data";
            this.listBox1.MultiColumn = true;
            button3.Visible = false;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                string AcctTemp = textBox1.Text;
                label2.Text = "";
                string AcctTemp1 = "0000000000" + AcctTemp;
                Acct = AcctTemp1.Right(10);
                
                con = new SqlConnection("server=DB0; Initial Catalog="+DBName+";Integrated Security=SSPI");
                
                cmd = new SqlCommand();
                con.Open();
                
                cmd.Connection = con;
                cmd.CommandText = "SELECT convert(varchar,I_BIF005pk) +' - '+ C_ACCOUNT +' - '+ C_Meter +' - '+ C_RemoteID  +' - '+ C_READTYPE  as Data,* FROM advanced.bif005 where c_account=" + Acct;

                SqlDataAdapter sqlDa = new SqlDataAdapter("SELECT I_BIF005PK,* FROM advanced.bif005 where c_account=" + Acct, con);
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgv1.DataSource = dtbl;

                con.Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8) 
            {
                e.Handled = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            DataGridViewRow selectedRow = dgv1.Rows[index];
            RecordToDelete = selectedRow.Cells[0].Value.ToString();
            label2.Text = "The Record you have selected to delete is: " + RecordToDelete;
            button3.Text="Delete Record " + RecordToDelete;
            button3.Visible = true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Delete Record","Delete record "+RecordToDelete, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                /*
                con = new SqlConnection("server=DB0; Initial Catalog="+DBName+";Integrated Security=SSPI");
                SqlCommand cmd = new SqlCommand("Delete advanced.bif005 where c_account=" + Acct + " and I_BIF005pk=" + RecordToDelete);
               //  cmd.ExecuteNonQuery();
               // MessageBox.Show(cmd.ToString());
                con.Open();
                cmd.ExecuteNonQuery();

                con.Close();
                */
                //--------------------------------------------------------------------------------
                con = new SqlConnection("server=DB0; Initial Catalog=" + DBName + ";Integrated Security=SSPI");
                cmd = new SqlCommand("Delete advanced.bif005 where c_account = " + Acct + " and I_BIF005pk = " + RecordToDelete);
                con.Open();
                
                cmd.Connection = con;
                cmd.ExecuteNonQuery();


                con.Close();




                //--------------------------------------------------------------------------------
                button1.PerformClick();
                button3.Visible = false;
            }
            if (dialogResult == DialogResult.No)
            {   button1.PerformClick();
                button3.Visible = false;
            }
        }
    }
    static class Extensions
    {
        /// <summary>
        /// Get substring of specified number of characters on the right.
        /// </summary>
        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }
    }
}
