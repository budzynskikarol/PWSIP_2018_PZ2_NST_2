using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net.Mail;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        SqlConnection con = null;
        int id_kategorii = 0;
        int id_zgloszenia = 0;
        int id_status = 0;
        bool wszystko_ok = true;
        bool wszystko_ok2 = true;
        bool wszystko_ok3 = true;
  
        public Form2()
        {
            this.setConnection();
            InitializeComponent();
        }
        
        private void setConnection()
        {
            // Połączenie z bazą lokalną
            //string conn_str = HelpDesk_Desktop.Properties.Settings.Default.dbConnectionString;

            // Połączenie z bazą online
            string conn_str = HelpDesk_Desktop.Properties.Settings.Default.HelpDeskDBConnectionString;

            con = new SqlConnection(conn_str);
            con.Open();
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
            dataGridView1.Columns[0].HeaderCell.Value = "Imię";
            dataGridView1.Columns[1].HeaderCell.Value = "Nazwisko";
            dataGridView1.Columns[2].HeaderCell.Value = "Telefon";
            dataGridView1.Columns[3].HeaderCell.Value = "E-mail";
            dataGridView1.Columns[4].HeaderCell.Value = "Hasło";
            dataGridView1.Columns[5].HeaderCell.Value = "Uprawnienia";
        }

        private void updateDataGrid1()
        {
            SqlCommand command = new SqlCommand("SELECT z.Id, z.Uzytkownik, z.Nazwa, z.Opis, z.Komentarz, s.Nazwa, k.Nazwa, z.DataDodania FROM Zgloszenias AS z INNER JOIN Statusies AS s ON z.StatusyId=s.Id INNER JOIN Kategories AS k ON z.KategorieId=k.Id", con);
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView2.DataSource = dt.DefaultView;
            dr.Close();
            dataGridView2.Columns[0].HeaderCell.Value = "ID";
            dataGridView2.Columns[1].HeaderCell.Value = "Użytkownik";
            dataGridView2.Columns[2].HeaderCell.Value = "Temat";
            dataGridView2.Columns[3].HeaderCell.Value = "Treść";
            dataGridView2.Columns[4].HeaderCell.Value = "Komentarz";
            dataGridView2.Columns[5].HeaderCell.Value = "Status";
            dataGridView2.Columns[6].HeaderCell.Value = "Kategoria";
            dataGridView2.Columns[7].HeaderCell.Value = "Data dodania";
        }

        private void GetIdKat(string nazwa)
        {
            SqlDataReader rdr = null;
            SqlCommand command2 = new SqlCommand("SELECT Id FROM Kategories WHERE Nazwa=@Nazwa", con);
            command2.Parameters.Clear();
            command2.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = nazwa;
            rdr = command2.ExecuteReader();

            while (rdr.Read())
            {
                id_kategorii = Int32.Parse(rdr[0].ToString());
            }

            if (rdr != null) rdr.Close();
        }

        private void GetIdStat(string nazwa)
        {
            SqlDataReader rdr = null;
            SqlCommand command2 = new SqlCommand("SELECT Id FROM Statusies WHERE Nazwa=@Nazwa", con);
            command2.Parameters.Clear();
            command2.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = nazwa;
            rdr = command2.ExecuteReader();

            while (rdr.Read())
            {
                id_status = Int32.Parse(rdr[0].ToString());
            }

            if (rdr != null) rdr.Close();
        }

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, con);      

            switch (state)
            {
                case 0:
                    GetIdKat(comboBox1.Text);
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
                    GetIdKat(comboBox1.Text);
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
                case 3:
                    GetIdStat(comboBox2.Text);
                    GetIdKat(comboBox3.Text);
                    msg = "Pomyślnie dodano zgłoszenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Nazwa", System.Data.SqlDbType.NVarChar).Value = textBox9.Text;
                    command.Parameters.Add("@Opis", System.Data.SqlDbType.NVarChar).Value = textBox8.Text;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@StatusyId", System.Data.SqlDbType.Int).Value = id_status;
                    command.Parameters.Add("@KategorieId", System.Data.SqlDbType.Int).Value = id_kategorii;
                    command.Parameters.Add("@Uzytkownik", System.Data.SqlDbType.NVarChar).Value = textBox10.Text;
                    command.Parameters.Add("@DataDodania", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                    break;
                case 4:
                    GetIdStat(comboBox2.Text);
                    GetIdKat(comboBox3.Text);
                    msg = "Pomyślnie zaktualizowano zgłoszenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id_zgloszenia;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@StatusyId", System.Data.SqlDbType.Int).Value = id_status;
                    command.Parameters.Add("@KategorieId", System.Data.SqlDbType.Int).Value = id_kategorii;
                    break;
                case 5:
                    msg = "Pomyślnie usunięto zgłoszenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id_zgloszenia;
                    break;
                case 6:
                    msg = "Pomyślnie zmieniono hasło!\nNastąpi zamknięcie systemu.\nProszę się ponownie zalogować.";
                    command.Parameters.Clear();
                    command.Parameters.Add("@UserName", System.Data.SqlDbType.NVarChar).Value = label1.Text;
                    command.Parameters.Add("@PasswordHash", System.Data.SqlDbType.NVarChar).Value = textBox12.Text;
                    break;
            }

            int n = command.ExecuteNonQuery();
            if (n > 0)
            {
                MessageBox.Show(msg);
                this.updateDataGrid();
                this.updateDataGrid1();
                if(state == 6)
                {
                    this.Close();
                }
            }
        }

        private void resetErrorLabels()
        {
            label10.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label17.Visible = false;
            label18.Visible = false;
            label19.Visible = false;
        }

        private void resetErrorLabels2()
        {
            label20.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
            label23.Visible = false;
        }

        private void resetErrorLabels3()
        {
            label34.Visible = false;
            label35.Visible = false;
            label36.Visible = false;
            label38.Visible = false;
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

            resetErrorLabels();
        }

        private void resetAll2()
        {
            textBox10.Text = null;
            textBox9.Text = null;
            textBox8.Text = null;
            textBox7.Text = null;
            this.textBox5.Text = DateTime.Now.ToString();
            comboBox2.Text = null;
            comboBox3.Text = null;

            textBox8.ReadOnly = false;
            textBox9.ReadOnly = false;

            this.textBox10.Text = this.label1.Text;

            button8.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = false;
            button9.Enabled = false;

            resetErrorLabels2();
        }

        private void check_boxy()
        {
            wszystko_ok = true;
            resetErrorLabels();

            if (textBox1.Text == "")
            {
                wszystko_ok = false;
                label10.Visible = true;
            }

            if (textBox2.Text == "")
            {
                wszystko_ok = false;
                label15.Visible = true;
            }

            if (textBox3.Text == "")
            {
                wszystko_ok = false;
                label16.Visible = true;
            }

            try
            {
                new System.Net.Mail.MailAddress(this.textBox4.Text);
            }
            catch (ArgumentException)
            {
                wszystko_ok = false;
                label17.Visible = true;
            }
            catch (FormatException)
            {
                wszystko_ok = false;
                label17.Visible = true;
            }

            if (textBox6.Text.Length < 8)
            {
                wszystko_ok = false;
                label18.Visible = true;
            }

            if (comboBox1.Text == "")
            {
                wszystko_ok = false;
                label19.Visible = true;
            }
        }

        private void check_boxy2()
        {
            wszystko_ok2 = true;
            resetErrorLabels2();

            if (textBox9.Text == "")
            {
                wszystko_ok2 = false;
                label20.Visible = true;
            }

            if (textBox8.Text == "")
            {
                wszystko_ok2 = false;
                label21.Visible = true;
            }

            if (comboBox2.Text == "")
            {
                wszystko_ok2 = false;
                label22.Visible = true;
            }

            if (comboBox3.Text == "")
            {
                wszystko_ok2 = false;
                label23.Visible = true;
            }
        }

        private void check_boxy3()
        {
            wszystko_ok3 = true;
            resetErrorLabels3();

            if (textBox11.Text != label37.Text)
            {
                wszystko_ok3 = false;
                label34.Visible = true;
            }

            if (textBox12.TextLength < 8)
            {
                wszystko_ok3 = false;
                label35.Visible = true;
            }

            if (textBox11.Text == textBox12.Text)
            {
                wszystko_ok3 = false;
                label38.Visible = true;
            }

            if (textBox13.Text != textBox12.Text)
            {
                wszystko_ok3 = false;
                label36.Visible = true;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.updateDataGrid();
            this.updateDataGrid1();
            this.textBox5.Text = DateTime.Now.ToString();
            this.textBox10.Text = this.label1.Text;
            Timer timer = new Timer();
            timer.Interval = (1 * 1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            label39.Text = DateTime.Now.ToLongTimeString();
            label40.Text = DateTime.Now.ToLongDateString();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            label39.Text = DateTime.Now.ToLongTimeString();
        }


        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            check_boxy();
            if(wszystko_ok)
            {
                String sql = "INSERT INTO AspNetUsers (Id, Imie, Nazwisko, PhoneNumber, UserName, PasswordHash, KategorieId) " +
                "VALUES (@Id, @Imie, @Nazwisko, @PhoneNumber, @UserName, @PasswordHash, @KategorieId)";
                this.updateDatabase(sql, 0);
                resetAll();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            check_boxy();
            if (wszystko_ok)
            {
                String sql = "UPDATE AspNetUsers SET Imie = @Imie, Nazwisko = @Nazwisko, PhoneNumber = @PhoneNumber, " +
                "PasswordHash = @PasswordHash, KategorieId = @KategorieId " +
                "WHERE UserName = @UserName";
                this.updateDatabase(sql, 1);
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            this.resetAll2();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Usunąć wybrane zgłoszenie?", "Komunikat", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                String sql = "DELETE FROM Zgloszenias " +
                "WHERE Id=@Id";
                this.updateDatabase(sql, 5);
                this.resetAll2();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            check_boxy2();
            if (wszystko_ok2)
            {
                String sql = "UPDATE Zgloszenias SET Komentarz = @Komentarz, StatusyId = @StatusyId, KategorieId = @KategorieId " +
                "WHERE Id = @Id";
                this.updateDatabase(sql, 4);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            check_boxy2();
            if (wszystko_ok2)
            {
                String sql = "INSERT INTO Zgloszenias (Nazwa, Opis, Komentarz, StatusyId, KategorieId, Uzytkownik, DataDodania) " +
                "VALUES (@Nazwa, @Opis, @Komentarz, @StatusyId, @KategorieId, @Uzytkownik, @DataDodania)";
                this.updateDatabase(sql, 3);
                resetAll2();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Wiadomosci Wiadomosci = new Wiadomosci();
            Wiadomosci.label1.Text = id_zgloszenia.ToString();
            Wiadomosci.label2.Text = this.label1.Text;
            Wiadomosci.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(!groupBox3.Visible)
            {
                groupBox3.Visible = true;
            }
            else
            groupBox3.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            check_boxy3();
            if (wszystko_ok3)
            {
                String sql = "UPDATE AspNetUsers SET PasswordHash = @PasswordHash " +
                "WHERE UserName = @UserName";
                this.updateDatabase(sql, 6);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                textBox3.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                textBox4.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                textBox6.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                comboBox1.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();

                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
                textBox4.ReadOnly = true;
            }
        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Index != -1)
            {
                label24.Text = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                id_zgloszenia = Int32.Parse(label24.Text);
                textBox10.Text = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                textBox9.Text = dataGridView2.CurrentRow.Cells[2].Value.ToString();
                textBox8.Text = dataGridView2.CurrentRow.Cells[3].Value.ToString();
                textBox7.Text = dataGridView2.CurrentRow.Cells[4].Value.ToString();
                comboBox2.Text = dataGridView2.CurrentRow.Cells[5].Value.ToString();
                comboBox3.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();
                textBox5.Text = dataGridView2.CurrentRow.Cells[7].Value.ToString();

                button8.Enabled = false;
                button7.Enabled = true;
                button6.Enabled = true;
                button9.Enabled = true;
                textBox9.ReadOnly = true;
                textBox8.ReadOnly = true;
            }
        }
    }
}
