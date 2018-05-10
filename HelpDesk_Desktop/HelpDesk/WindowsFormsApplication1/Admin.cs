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
        int id_uzytkownika = 0;
        public Form2()
        {
            this.setConnection();
            InitializeComponent();
        }

        private void setConnection()
        {
            string conn_str = @"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Dropbox\Dokumenty Karol\PWSIP\Semestr 6\Projekt zespołowy II\Repozytorium\HelpDesk_Desktop\HelpDesk\WindowsFormsApplication1\Database1.mdf;Integrated Security=True;User Instance=True";
            con = new SqlConnection(conn_str);
            try
            {
                con.Open();
            }
            catch (Exception err)
            {

            }
        }
        
        private void updateDataGrid()
        {
            SqlCommand command = new SqlCommand("SELECT * FROM Uzytkownicy", con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
        }

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, con);

            switch (state)
            {
                case 0:
                    msg = "Pomyślnie dodano użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.VarChar).Value = textBox1.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.VarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.VarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Login", System.Data.SqlDbType.VarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Haslo", System.Data.SqlDbType.VarChar).Value = textBox6.Text;
                    command.Parameters.Add("@Uprawnienia", System.Data.SqlDbType.VarChar).Value = comboBox1.Text;
                    break;
                case 1:
                    msg = "Pomyślnie zaktualizowano użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@IdUzytkownika", System.Data.SqlDbType.Int).Value = id_uzytkownika;
                    command.Parameters.Add("@Imie", System.Data.SqlDbType.VarChar).Value = textBox1.Text;
                    command.Parameters.Add("@Nazwisko", System.Data.SqlDbType.VarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Telefon", System.Data.SqlDbType.VarChar).Value = textBox3.Text;
                    command.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = textBox4.Text;
                    command.Parameters.Add("@Login", System.Data.SqlDbType.VarChar).Value = textBox5.Text;
                    command.Parameters.Add("@Haslo", System.Data.SqlDbType.VarChar).Value = textBox6.Text;
                    command.Parameters.Add("@Uprawnienia", System.Data.SqlDbType.VarChar).Value = comboBox1.Text;
                    break;
                case 2:
                    msg = "Pomyślnie usunięto użytkownika!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@IdUzytkownika", System.Data.SqlDbType.Int).Value = id_uzytkownika;
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
            id_uzytkownika = 0;
            textBox1.Text = null;
            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
            textBox6.Text = null;
            textBox7.Text = null;
            comboBox1.Text = null;

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
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
            String sql = "INSERT INTO Uzytkownicy(Imie, Nazwisko, Numer_tel, Email, Login, Haslo, Uprawnienia) " +
                "VALUES(@Imie, @Nazwisko, @Telefon, @Email, @Login, @Haslo, @Uprawnienia)";
            this.updateDatabase(sql, 0);
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String sql = "UPDATE Uzytkownicy SET Imie = @Imie, Nazwisko = @Nazwisko, Numer_tel = @Telefon, " +
                "Email = @Email, Login = @Login, Haslo = @Haslo, Uprawnienia = @Uprawnienia " +
                "WHERE IdUzytkownika = @IdUzytkownika";
            this.updateDatabase(sql, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String sql = "DELETE FROM Uzytkownicy " +
                "WHERE IdUzytkownika = @IdUzytkownika";
            this.updateDatabase(sql, 2);
            this.resetAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.resetAll();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                id_uzytkownika = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                textBox7.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                textBox6.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }

        }
    }
}
