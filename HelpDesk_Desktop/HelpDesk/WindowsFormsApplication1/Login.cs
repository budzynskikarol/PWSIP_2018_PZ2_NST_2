using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace WindowsFormsApplication1
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Połączenie z bazą lokalną
        //string conn_str = HelpDesk_Desktop.Properties.Settings.Default.dbConnectionString;

        // Połączenie z bazą online
        string conn_str = HelpDesk_Desktop.Properties.Settings.Default.HelpDeskDBConnectionString;

        SqlDataReader rdr;

        private void button1_Click(object sender, EventArgs e)
        {
            string username2 = null;
            string pass = null;
            string imie = null;
            string nazwisko = null;
            string telefon = null;
            int upraw = 0;

            using (SqlConnection connection = new SqlConnection(conn_str))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException err)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);  
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("SELECT  UserName, PasswordHash, KategorieId, Imie, Nazwisko, PhoneNumber FROM AspNetUsers WHERE Username=@Username", connection);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Username", System.Data.SqlDbType.NVarChar).Value = TLogin.Text;
                    rdr = command.ExecuteReader();

                    while (rdr.Read())
                    {
                        upraw = Int32.Parse(rdr[2].ToString());
                        pass = rdr[1].ToString();
                        username2 = rdr[0].ToString();
                        imie = rdr[3].ToString();
                        nazwisko = rdr[4].ToString();
                        telefon = rdr[5].ToString();
                    }

                    if (rdr != null) rdr.Close();
                    connection.Close();
                }
            }
            
            if (pass != "")
            {
                if (TPass.Text == pass)
                {
                    if (upraw == 9)
                    {
                        Form2 Admin = new Form2();
                        Admin.label1.Text = TLogin.Text;
                        Admin.label28.Text = imie + " " + nazwisko;
                        Admin.label29.Text = telefon;
                        Admin.label30.Text = username2;
                        Admin.label26.Text = imie + Admin.label26.Text;
                        Admin.label37.Text = pass;
                        Admin.ShowDialog();

                        Application.ExitThread();
                    }
                    else if (upraw == 8)
                    {
                        User User = new User();
                        User.label2.Text = TLogin.Text;
                        User.label28.Text = imie + " " + nazwisko;
                        User.label29.Text = telefon;
                        User.label30.Text = username2;
                        User.label26.Text = imie + User.label26.Text;
                        User.label37.Text = pass;
                        User.ShowDialog();

                        Application.ExitThread();
                    }
                    else
                    {
                        Specjalista Specjalista = new Specjalista();
                        Specjalista.label2.Text = TLogin.Text;
                        Specjalista.label4.Text = upraw.ToString();
                        Specjalista.label28.Text = imie + " " + nazwisko;
                        Specjalista.label29.Text = telefon;
                        Specjalista.label30.Text = username2;
                        Specjalista.label26.Text = imie + Specjalista.label26.Text;
                        Specjalista.label37.Text = pass;
                        Specjalista.ShowDialog();
                        
                        Application.ExitThread();
                    }
                }
                else
                    MessageBox.Show("Niepoprawny login lub hasło!", "Komunikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
