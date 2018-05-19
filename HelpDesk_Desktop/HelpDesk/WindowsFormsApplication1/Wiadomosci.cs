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
    public partial class Wiadomosci : Form
    {
        SqlConnection con = null;
        int id_zgloszenia;

        public Wiadomosci()
        {
            this.setConnection();
            InitializeComponent();
        }

        private void setConnection()
        {
            // Połączenie z bazą lokalną
            //string conn_str = Properties.Settings.Default.dbConnectionString;

            // Połączenie z bazą online
            string conn_str = HelpDesk_Desktop.Properties.Settings.Default.HelpDeskDBConnectionString;

            con = new SqlConnection(conn_str);
            con.Open();
        }

        private void updateDataGrid()
        {
            id_zgloszenia = Int32.Parse(label1.Text);
            SqlCommand command = new SqlCommand("SELECT Tresc, Nadawca, DataDodania FROM Wiadomoscis WHERE ZgloszeniaId = @ZgloszeniaId", con);
            command.Parameters.Clear();
            command.Parameters.Add("@ZgloszeniaId", System.Data.SqlDbType.Int).Value = id_zgloszenia;
            SqlDataReader dr = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt.DefaultView;
            dr.Close();
            dataGridView1.Columns[0].HeaderCell.Value = "Treść";
            dataGridView1.Columns[1].HeaderCell.Value = "Nadawca";
            dataGridView1.Columns[2].HeaderCell.Value = "Data dodania";
        }

        private void updateDatabase(String sql_stmt, int state)
        {
            String msg = "";
            SqlCommand command = new SqlCommand(sql_stmt, con);
            id_zgloszenia = Int32.Parse(label1.Text);

            switch (state)
            {
                case 0:
                    msg = "Pomyślnie dodano wiadomość!";
                    command.Parameters.Clear();
                    command.Parameters.Add("@Tresc", System.Data.SqlDbType.NVarChar).Value = textBox2.Text;
                    command.Parameters.Add("@Nadawca", System.Data.SqlDbType.NVarChar).Value = textBox1.Text;
                    command.Parameters.Add("@ZgloszeniaId", System.Data.SqlDbType.Int).Value = id_zgloszenia;
                    command.Parameters.Add("@DataDodania", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                    break;
            }

            int n = command.ExecuteNonQuery();
            if (n > 0)
            {
                MessageBox.Show(msg);
                this.updateDataGrid();
            }
        }

        private void resetAll()
        {
            textBox1.Text = label2.Text;
            textBox2.Text = null;
            button1.Enabled = true;
            label4.Visible = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            updateDataGrid();
        }

        private void Wiadomosci_Load(object sender, EventArgs e)
        {
            updateDataGrid();
            textBox1.Text = label2.Text;
            Timer timer = new Timer();
            timer.Interval = (5 * 1000);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                label4.Visible = true;
            }
            else
            {
                String sql = "INSERT INTO Wiadomoscis (Tresc, Nadawca, ZgloszeniaId, Datadodania) " +
                "VALUES (@Tresc, @Nadawca, @ZgloszeniaId, @Datadodania)";
                this.updateDatabase(sql, 0);
                resetAll();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetAll();
        }
    }
}
