using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        SqlConnection con = null;
        int id_kategorii = 0;

        public Form2()
        {
            this.setConnection();
            InitializeComponent();
        }

        private void setConnection()
        {
            // Połączenie z bazą lokalną
            string conn_str = Properties.Settings.Default.dbConnectionString;

            // Połączenie z bazą online
            //string conn_str = Properties.Settings.Default.HelpDeskDBConnectionString;

            con = new SqlConnection(conn_str);
            try
            {
                con.Open();
            }
            catch (Exception err)
            {

            }
        }

        private string RandomString(int range)
        {
            var chars = "abcdefghijklmnopqrstuwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, range)
                            .Select(s => s[random.Next(s.Length)])
                            .ToArray());

            return result;
        }

        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT  A.Imie, A.Nazwisko, A.PhoneNumber, A.UserName, A.PasswordHash, K.Nazwa FROM AspNetUsers AS A cross join Kategories AS K WHERE A.KategorieId=K.Id", con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        private void GetIdKat()
        {
            SqlDataReader rdr = null;
            SqlCommand command2 = new SqlCommand("SELECT Id FROM Kategories WHERE Nazwa=@Nazwa", con);
            command2.Parameters.Clear();
            command2.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = comboBox1.Text;
            try
            {
                rdr = command2.ExecuteReader();
                while (rdr.Read())
                {
                    id_kategorii = Int32.Parse(rdr[0].ToString());
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                if (rdr != null) rdr.Close();
            }

        }

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, con);

            GetIdKat();

            switch (state)
            {
                case 0:
                    msg = "Pomyślnie dodano użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id", System.Data.SqlDbType.NVarChar).Value = RandomString(40);
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.NVarChar).Value = textBox1.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@PhoneNumber", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@PasswordHash", System.Data.SqlDbType.NVarChar).Value = textBox6.Text;
                    command.Parameters.Add("@KategorieId", System.Data.SqlDbType.Int).Value = id_kategorii;
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.NVarChar).Value = textBox1.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@PhoneNumber", System.Data.SqlDbType.NVarChar).Value = textBox3.Text;
                    command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    command.Parameters.Add("@PasswordHash", System.Data.SqlDbType.NVarChar).Value = textBox6.Text;
                    command.Parameters.Add("@KategorieId", System.Data.SqlDbType.Int).Value = id_kategorii;
                    break;
                case 2:
                    msg = "Pomyślnie usunięto użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar).Value = textBox4.Text;
                    break;
            }
            try
            {
                int n = command.ExecuteNonQuery();
                if (n > 0)
                {
                    MessageBox.Show(msg);
                    this.updateDataGrid();
                }
            }
            catch (Exception err)
            {

            }
        }

        private void resetAll()
        {
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox6.Text = null;
            comboBox1.Text = null;

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            textBox4.ReadOnly = false;

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.updateDataGrid();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String sql = "INSERT INTO AspNetUsers (Id, Imie, Nazwisko, PhoneNumber, UserName, PasswordHash, KategorieId) " +
                "VALUES (@Id, @Imie, @Nazwisko, @PhoneNumber, @UserName, @PasswordHash, @KategorieId)";
            this.updateDatabase(sql, 0);
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String sql = "UPDATE AspNetUsers SET Imie = @Imie, Nazwisko = @Nazwisko, PhoneNumber = @PhoneNumber, " +
                "PasswordHash = @PasswordHash, KategorieId = @KategorieId " +
                "WHERE UserName = @UserName";
            this.updateDatabase(sql, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Usunąć użytkownika " + textBox4.Text + "?", "Komunikat", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                String sql = "DELETE FROM AspNetUsers " +
                "WHERE UserName = @UserName";
                this.updateDatabase(sql, 2);
                this.resetAll();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.resetAll();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                //id_uzytkownika = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                //textBox7.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                //textBox5.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox6.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                textBox4.ReadOnly = true;
            }
        }
    }
}
