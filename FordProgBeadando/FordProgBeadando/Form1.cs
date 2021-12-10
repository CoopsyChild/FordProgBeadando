using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FordProgBeadando
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            string FilePath = openFileDialog1.FileName;
            OpenTable(FilePath);
        }

        public void OpenTable(string FilePath)
        {
            try
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                string[] csv = System.IO.File.ReadAllLines(FilePath);
                if (csv.Length > 0)
                {
                    //Első sor beolvasása az oszlopnevek létrehozásához
                    string FirstLine = csv[0].Substring(1, csv[0].Length - 1);
                    string[] ColumnNames = FirstLine.Split(';');
                    foreach (string Column in ColumnNames)
                    {
                        dataGridView1.Columns.Add(Column, Column);
                    }
                    //Táblák feltöltése
                    for (int i = 1; i < csv.Length; i++)
                    {

                        string[] Data = csv[i].Split(';');
                        string[] row = new string[Data.Length];

                        for (int j = 1; j < Data.Length; j++)
                        {
                            row[j - 1] = Data[j];
                        }
                        dataGridView1.Rows.Add(row);
                        dataGridView1.Rows[i - 1].HeaderCell.Value = Data[0];
                    }
                }
                dataGridView1.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            }
            catch
            {
                throw new Exception("A forrás fájl hibás");
            }
            
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string newColumnName = Interaction.InputBox("Kérlek írd be az új adatot:", "Adatváltoztatás", "");
            dataGridView1.Columns[e.ColumnIndex].HeaderText = newColumnName;
        }

        private void dataGridView1_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string newRowName = Interaction.InputBox("Kérlek írd be az új adatot:", "Adatváltoztatás", "");
            dataGridView1.Rows[e.RowIndex].HeaderCell.Value = newRowName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(originalInput.Text.Length!=0)
            {
                if(originalInput.Text[originalInput.Text.Length-1]=='#')
                {
                    convertedInput.Text = Regex.Replace(originalInput.Text, "[0-9]+", "i");
                }
                else
                {
                    convertedInput.Text = Regex.Replace(originalInput.Text, "[0-9]+", "i") + "#";
                }
                
            }
            else
            {
                MessageBox.Show("Az input mező üres, kérlek add meg az input adatot!");
            }
        }
    }
}
