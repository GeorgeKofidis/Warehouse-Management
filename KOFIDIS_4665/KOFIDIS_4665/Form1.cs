using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Data.Common;

namespace KOFIDIS_4665
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlDataAdapter DataAdapterPelates, DataAdapterApothiki, DataAdapterParaggelies, DataAdapterProiontaParaggelion;
        DataSet DataSet1, DataSet2, DataSet3, DataSet4;
        BindingSource BindingSource1, BindingSource2, BindingSource3, BindingSource4;
        SqlCommandBuilder cmdbl;

        // ISTORIKO PARAGGELION PELATH
        SqlDataAdapter DataAdapterIstorikoPelath, DataAdapter2;
        DataSet DataSetIstorikoPelath;
        BindingSource bindingSource2;

        // ISTORIKO_KINHSHS_PROIONTON_APOTHIKHS 
        SqlDataAdapter DataAdapterIstorikoApothiki, DataAdapterIstor_Apothiki;
        DataSet DataSetApothiki;
        BindingSource sourceApothiki;

        // FOTOGRAFIA PELATES
        SqlDataAdapter sqlAdapterFotoPelates;
        DataSet DataSetFotoPelates;
        SqlCommand command;

        // FOTOGRAFIA APOTHIKI
        SqlDataAdapter sqlDataAdapterFotoApothiki;
        DataSet DataSetFotoApothiki;
        SqlCommand commandApothiki;


        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection("Data Source=LAPTOP-KNGSTHV4\\SQLEXPRESS;Initial Catalog=APOTHIKI_4665;Integrated Security=True");
            connection.Open();

            DataAdapterIstorikoPelath = new SqlDataAdapter("select * from PELATES", connection);
            DataTable dt1 = new DataTable();
            DataAdapterIstorikoPelath.Fill(dt1);
            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "EPITHETO";

            // ISTORIKO_KINHSHS_PROIONTON_APOTHIKHS 
            DataAdapterIstorikoApothiki = new SqlDataAdapter("select * from APOTHIKI", connection);
            DataTable dt2 = new DataTable();
            DataAdapterIstorikoApothiki.Fill(dt2);
            comboBox2.DataSource = dt2;
            comboBox2.DisplayMember = "EIDOS";

            // FOTOGRAFIA PELATES 
            bindingNavigator5.BindingSource = bindingSource5;

            // FOTOGRAFIA APOTHIKI
            bindingNavigator6.BindingSource = bindingSource6;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataAdapterPelates = new SqlDataAdapter("select * from PELATES", connection);
            DataSet1 = new DataSet();
            DataAdapterPelates.Fill(DataSet1, "Pelates_Table");
            BindingSource1 = new BindingSource();
            BindingSource1.DataSource = DataSet1.Tables[0].DefaultView;
            bindingNavigator1.BindingSource = BindingSource1;
            dataGridView1.DataSource = BindingSource1;

            // DataGridView APOTHIKI
            DataAdapterApothiki = new SqlDataAdapter("select * from APOTHIKI", connection);
            DataSet2 = new DataSet();
            DataAdapterApothiki.Fill(DataSet2, "Apothiki_Table");
            BindingSource2 = new BindingSource();
            BindingSource2.DataSource = DataSet2.Tables[0].DefaultView;
            bindingNavigator2.BindingSource = BindingSource2;
            dataGridView2.DataSource = BindingSource2;

            // DataGridView PARAGGELIES
            DataAdapterParaggelies = new SqlDataAdapter("select * from PARAGGELIA", connection);
            DataSet3 = new DataSet();
            DataAdapterParaggelies.Fill(DataSet3, "Paraggelia_Table");
            BindingSource3 = new BindingSource();
            BindingSource3.DataSource = DataSet3.Tables[0].DefaultView;
            bindingNavigator3.BindingSource = BindingSource3;
            dataGridView3.DataSource = BindingSource3;

            // DataGridView PROIONTA_PARAGGELION
            DataAdapterProiontaParaggelion = new SqlDataAdapter("select * from PROIONTA_PARAGGELION", connection);
            DataSet4 = new DataSet();
            DataAdapterProiontaParaggelion.Fill(DataSet4, "Proionta_Paraggelion");
            BindingSource4 = new BindingSource();
            BindingSource4.DataSource = DataSet4.Tables[0].DefaultView;
            bindingNavigator4.BindingSource = BindingSource4;
            dataGridView4.DataSource = BindingSource4;


            // FOTOGRAFIA PELATES
            if (connection.State == ConnectionState.Open)
            {
                MessageBox.Show("Connection Established!");
            }
            else
            {
                MessageBox.Show("Connection Error!");
                Application.Exit();
            }
            sqlAdapterFotoPelates = new SqlDataAdapter("select * from PELATES", connection);
            DataSetFotoPelates = new DataSet();
            sqlAdapterFotoPelates.Fill(DataSetFotoPelates);
            bindingSource5.DataSource = DataSetFotoPelates.Tables[0];
            bindingNavigator5.Refresh();
            textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource5, "KOD_PELATH", true));
            textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource5, "EPONYMIA", true));
            textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource5, "EPITHETO", true));
            textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource5, "ONOMA", true));
            textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource5, "FOTO", true));
            refreshImagePelath();


            // FOTOGRAFIA APOTHIKI
            sqlDataAdapterFotoApothiki = new SqlDataAdapter("select * from APOTHIKI", connection);
            DataSetFotoApothiki = new DataSet();
            sqlDataAdapterFotoApothiki.Fill(DataSetFotoApothiki);
            bindingSource6.DataSource = DataSetFotoApothiki.Tables[0];
            bindingNavigator6.Refresh();
            textBox6.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource6, "KE", true));
            textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource6, "EIDOS", true));
            textBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource6, "KATHGORIA", true));
            textBox9.DataBindings.Add(new System.Windows.Forms.Binding("Text", bindingSource6, "FOTO", true));
            refreshImageApothiki();



        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            cmdbl = new SqlCommandBuilder(DataAdapterPelates);
            DataAdapterPelates.Update(DataSet1, "Pelates_Table");
            MessageBox.Show("Information Updated");
        }

        private void saveToolStripButton1_Click(object sender, EventArgs e)
        {
            cmdbl = new SqlCommandBuilder(DataAdapterApothiki);
            DataAdapterApothiki.Update(DataSet2, "Apothiki_Table");
            MessageBox.Show("Information Updated");
        }

        private void saveToolStripButton2_Click(object sender, EventArgs e)
        {
            cmdbl = new SqlCommandBuilder(DataAdapterParaggelies);
            DataAdapterParaggelies.Update(DataSet3, "Paraggelia_Table");
            MessageBox.Show("Information Updated");
        }

        private void saveToolStripButton3_Click(object sender, EventArgs e)
        {
            cmdbl = new SqlCommandBuilder(DataAdapterProiontaParaggelion);
            DataAdapterProiontaParaggelion.Update(DataSet4, "Proionta_Paraggelion");
            MessageBox.Show("Information Updated");
        }
        
        // ISTORIKO PARAGGELION PELATH
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDataSet();
        }

        public void fillDataSet()
        {
            DataAdapter2 = new SqlDataAdapter("select EPONYMIA,AFM,EIDOS,KATHGORIA,TIMH_POLHSHS,FPA,POSOTHTA from PELATES, PARAGGELIA,APOTHIKI,PROIONTA_PARAGGELION WHERE (PELATES.KOD_PELATH = PARAGGELIA.K_PEL) AND (PARAGGELIA.KOD_PAR = PROIONTA_PARAGGELION.K_PAR) AND (APOTHIKI.KE = PROIONTA_PARAGGELION.K_E) AND PELATES.EPITHETO = '" + comboBox1.Text.ToString() + "'", connection);
            DataSetIstorikoPelath = new DataSet();
            DataAdapter2.Fill(DataSetIstorikoPelath);
            bindingSource2 = new BindingSource();
            DataTable dt = new DataTable();
            bindingSource2.DataSource = DataSetIstorikoPelath.Tables[0].DefaultView;
            dataGridView5.DataSource = bindingSource2;
            int sum = 0;
            for(int i = 0; i<dataGridView5.Rows.Count; i++)
            {
                sum += Convert.ToInt32(dataGridView5.Rows[i].Cells[4].Value)*Convert.ToInt32(dataGridView5.Rows[i].Cells[6].Value)*Convert.ToInt32(dataGridView5.Rows[i].Cells[5].Value)/100;
            }
            label4.Text = sum.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDataSetApothiki();
        }

        public void fillDataSetApothiki()
        {
            DataAdapterIstor_Apothiki = new SqlDataAdapter("select EIDOS,KATHGORIA,APOTHEMA,TIMH_POLHSHS from APOTHIKI where APOTHIKI.EIDOS='" + comboBox2.Text.ToString() + "'", connection);
            DataSetApothiki = new DataSet();
            DataAdapterIstor_Apothiki.Fill(DataSetApothiki);
            sourceApothiki = new BindingSource();
            DataTable dt3 = new DataTable();
            sourceApothiki.DataSource = DataSetApothiki.Tables[0].DefaultView;
            dataGridView6.DataSource = sourceApothiki;
            int sum1 = 0;
            for (int j = 0; j < dataGridView6.Rows.Count; j++)
            {
                sum1 += Convert.ToInt32(dataGridView6.Rows[j].Cells[3].Value);
            }
            label8.Text = sum1.ToString();
        }

        public void refreshImagePelath()
        {
            String photoPathPelath = textBox5.Text.Trim();
            if (photoPathPelath != null && File.Exists(photoPathPelath))
            {
                pictureBox1.Image = Image.FromFile(photoPathPelath);
            }
            else
            {
                pictureBox1.Image = Image.FromFile(@"C:\Projects\SQLSERVER\Project\Photo_Pelates\error.png");
            }
        }

        // FOTOGRAFIA APOTHIKI
        public void refreshImageApothiki()
        {
            String photoPathApothiki = textBox9.Text.Trim();
            if (photoPathApothiki != null && File.Exists(photoPathApothiki))
            {
                pictureBox2.Image = Image.FromFile(photoPathApothiki);
            }
            else
            {
                pictureBox2.Image = Image.FromFile(@"C:\Projects\SQLSERVER\Project\Photo_Apothiki\error.png");
            }
        }

        private void bindingNavigator5_RefreshItems(object sender, EventArgs e)
        {
            refreshImagePelath();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String openPathPelath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openPathPelath = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                textBox5.Text = openPathPelath;
                pictureBox1.Image = Image.FromFile(openPathPelath);
                command = new SqlCommand("UPDATE PELATES SET FOTO='" + openPathPelath + "' WHERE KOD_PELATH=" + textBox1.Text + ";", connection);
                command.ExecuteNonQuery();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }

        private void bindingNavigator6_RefreshItems(object sender, EventArgs e)
        {
            refreshImageApothiki();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String openPathApothiki;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                openPathApothiki = openFileDialog2.InitialDirectory + openFileDialog2.FileName;
                textBox9.Text = openPathApothiki;
                pictureBox2.Image = Image.FromFile(openPathApothiki);
                commandApothiki = new SqlCommand("UPDATE APOTHIKI SET FOTO='" + openPathApothiki + "'WHERE KE=" + textBox6.Text + ";", connection);
                commandApothiki.ExecuteNonQuery();
            }
        }

    }
}
