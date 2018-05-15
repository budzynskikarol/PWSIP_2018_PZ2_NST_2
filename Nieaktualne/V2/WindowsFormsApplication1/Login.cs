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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        string conn_str = @"Data Source=.\SQLEXPRESS;AttachDbFilename=D:\Dropbox\Dokumenty Karol\PWSIP\Semestr 6\Projekt zespołowy II\Repozytorium\HelpDesk_Desktop\HelpDesk\WindowsFormsApplication1\Database1.mdf;Integrated Security=True;User Instance=True";
        SqlDataReader rdr;

        private void button1_Click(object sender, EventArgs e)
        {
            int id = 0;
            string pass = null;
            string upraw = null;

            using (SqlConnection connection = new SqlConnection(conn_str))
            {
                try
                {
                    connection.Open();
                }
                catch (SqlException err)
                {
                    MessageBox.Show("Błąd połączenia z bazą danych");  
                }
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    SqlCommand command = new SqlCommand("SELECT * FROM Uzytkownicy WHERE Login=@Login", connection);
                    command.Parameters.Clear();
                    command.Parameters.Add("@Login", System.Data.SqlDbType.VarChar).Value = TLogin.Text;
                    try
                    {
                        rdr = command.ExecuteReader();
                        while (rdr.Read())
                        {
                            id = Int32.Parse(rdr[0].ToString());
                            pass = rdr[6].ToString();
                            upraw = rdr[7].ToString();
                        }
                    }
                    catch (Exception err)
                    {

                    }
                    finally
                    {
                        if (rdr != null) rdr.Close();
                        connection.Close();
                    }
                }
            }
            
            //utworzenie nowego wątku, uruchamiającego nową aplikację
            if (pass != "")
            {
                if (TPass.Text == pass)
                {
                    if (upraw == "Administrator")
                    {
                        Form2 Admin = new Form2();
                        Admin.label1.Text = id.ToString();
                        Admin.ShowDialog();

                        //zamknięcie starego wątku
                        Application.ExitThread();
                    }
                    else if (upraw == "Specjalista")
                    {
                        Specjalista Specjalista = new Specjalista();
                        Specjalista.label1.Text = upraw;
                        Specjalista.ShowDialog();

                        //zamknięcie starego wątku
                        Application.ExitThread();
                    }
                    else if (upraw == "Użytkownik")
                    {
                        User User = new User();
                        User.label1.Text = upraw;
                        User.ShowDialog();

                        //zamknięcie starego wątku
                        Application.ExitThread();
                    }
                }
                else
                    MessageBox.Show("Niepoprawny login lub hasło!");
            }
        }
    }
}
