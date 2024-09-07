using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Password
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtParola.Text))
            {
                int uzunluk;
                if (int.TryParse(txtUzunluk.Text, out uzunluk))
                {
                    string parolaAdi = txtAdi.Text.ToUpper();
                    if (string.IsNullOrEmpty(parolaAdi))
                    {
                        MessageBox.Show("Parola adı giriniz.");
                        return;
                    }

                    // Parolayı dosyaya kaydet
                    SavePasswordToFile(parolaAdi, txtParola.Text);

                }
                else
                {
                    MessageBox.Show("Geçerli bir uzunluk giriniz.");
                }
            }
            else
            {
                int uzunluk;
                if (int.TryParse(txtUzunluk.Text, out uzunluk))
                {
                    string parolaAdi = txtAdi.Text.ToUpper();
                    if (string.IsNullOrEmpty(parolaAdi))
                    {
                        MessageBox.Show("Parola adı giriniz.");
                        return;
                    }

                    string parola = GeneratePassword(uzunluk);
                    txtParola.Text = parola;

                    // Parolayı dosyaya kaydet
                    SavePasswordToFile(parolaAdi, parola);

                }
                else
                {
                    MessageBox.Show("Geçerli bir uzunluk giriniz.");
                }
            }
            
        }

        private string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            StringBuilder result = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(validChars[random.Next(validChars.Length)]);
            }

            return result.ToString();
        }

        private void SavePasswordToFile(string passwordName, string password)
        {
            string filePath = "parolalar.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{passwordName} : {password}");
            }

            MessageBox.Show("Parola dosyaya kaydedildi.");
        }
       

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeDataGridView();
            LoadPasswordsFromFile();
        }
        private DataTable dataTable;
        private void InitializeDataGridView()
        {
            dataTable = new DataTable();
            dataTable.Columns.Add("ParolaAdı", typeof(string));
            dataTable.Columns.Add("Parola", typeof(string));
            dataGridViewParolalar.DataSource = dataTable;
        }

        private void AddPasswordToDataGridView(string passwordName, string password)
        {
          
            dataTable.Rows.Add(passwordName, password);
        }

        private void LoadPasswordsFromFile()
        {
            string filePath = "parolalar.txt";

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        AddPasswordToDataGridView(parts[0].Trim(), parts[1].Trim());
                    }
                }
            }
        }

        

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterText = txtFilterValue.Text;
            DataView dv = dataTable.DefaultView;
            dv.RowFilter = string.Format("ParolaAdı LIKE '%{0}%'", filterText);
            dataGridViewParolalar.DataSource = dv.ToTable();
        }
    }
}
