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
    public partial class Specjalista : Form
    {
        SqlConnection con = null;
        int id_kategorii = 0;
        int id_zgloszenia = 0;
        int id_status = 0;
        bool wszystko_ok2 = true;
        bool wszystko_ok3 = true;

        public Specjalista()
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

        private void updateDataGrid1()
        {
            SqlCommand command = new SqlCommand("SELECT z.Id, z.Uzytkownik, z.Nazwa, z.Opis, z.Komentarz, s.Nazwa, k.Nazwa, z.DataDodania FROM Zgloszenias AS z INNER JOIN Statusies AS s ON z.StatusyId=s.Id INNER JOIN Kategories AS k ON z.KategorieId=k.Id WHERE z.KategorieId=@KategorieId", con);
            command.Parameters.Clear();
            command.Parameters.Add("@KategorieId", System.Data.SqlDbType.Int).Value = Int32.Parse(label4.Text);
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
                    msg = "Pomyślnie zaktualizowano zgłoszenie!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id_zgloszenia;
                    command.Parameters.Add("@Komentarz", System.Data.SqlDbType.NVarChar).Value = textBox7.Text;
                    command.Parameters.Add("@StatusyId", System.Data.SqlDbType.Int).Value = id_status;
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
                this.updateDataGrid1();
                if (state == 6)
                {
                    this.Close();
                }
            }
        }

        private void resetErrorLabels2()
        {
            label5.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            label22.Visible = false;
        }

        private void resetErrorLabels3()
        {
            label34.Visible = false;
            label35.Visible = false;
            label36.Visible = false;
            label38.Visible = false;
        }

        private void resetAll2()
        {
            textBox10.Text = null;
            textBox9.Text = null;
            textBox8.Text = null;
            textBox7.Text = null;
            comboBox2.Text = null;
            comboBox3.Text = null;

            textBox8.ReadOnly = false;
            textBox9.ReadOnly = false;

            this.textBox10.Text = this.label2.Text;

            button8.Enabled = true;
            button7.Enabled = false;
            button9.Enabled = false;

            textBox1.Visible = false;
            comboBox3.Visible = true;
            
            resetErrorLabels2();
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
                label5.Visible = true;
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

        private void Specjalista_Load(object sender, EventArgs e)
        {
            this.updateDataGrid1();
            this.textBox10.Text = this.label2.Text;
            Timer timer = new Timer();
            timer.Interval = (1 * 1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
            label40.Text = DateTime.Now.ToLongTimeString();
            label41.Text = DateTime.Now.ToLongDateString();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            label40.Text = DateTime.Now.ToLongTimeString();
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.resetAll2();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            check_boxy2();
            if (wszystko_ok2)
            {
                String sql = "UPDATE Zgloszenias SET Komentarz = @Komentarz, StatusyId = @StatusyId " +
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
            Wiadomosci.label2.Text = this.label2.Text;
            Wiadomosci.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!groupBox3.Visible)
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
                textBox1.Text = dataGridView2.CurrentRow.Cells[6].Value.ToString();

                button8.Enabled = false;
                button7.Enabled = true;
                button9.Enabled = true;
                textBox9.ReadOnly = true;
                textBox8.ReadOnly = true;
                comboBox3.Visible = false;
                textBox1.Visible = true;
            }
        }
    }
}
