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
                ResizeDataGridView();
            }
            catch
            {
                throw new Exception("A forrás fájl hibás");
            }

        }

        public void ResizeDataGridView()
        {
            dataGridView1.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            dataGridView1.RowHeadersWidth = 50;
            int height = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                height += row.Height;
            }
            height += dataGridView1.ColumnHeadersHeight;

            int width = 0;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                width += col.Width;
            }
            width += dataGridView1.RowHeadersWidth;

            dataGridView1.ClientSize = new Size(width + 2, height + 2);
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

        public string inputConvert()
        {
            if (originalInput.Text.Length != 0)
            {
                if (originalInput.Text[originalInput.Text.Length - 1] == '#')
                {
                    convertedInput.Text = Regex.Replace(originalInput.Text, "[0-9]+", "i");
                }
                else
                {
                    convertedInput.Text = Regex.Replace(originalInput.Text, "[0-9]+", "i") + "#";
                }
                return convertedInput.Text;
            }
            else
            {
                MessageBox.Show("Az input mező üres, kérlek add meg az input adatot!");
                return "empty";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            inputConvert();
        }
        public bool checkDataGrid()
        {
            if(dataGridView1.RowCount == 0)
            {
                MessageBox.Show("A táblázat üres");
                return false;
            }
            else
            {
                return true;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //input konvertálása
            string input = inputConvert();
            //Táblázat ellenörzése
            bool IsEmpty = checkDataGrid();

            // Megvizsgáljuk, hogy üres-e a Táblázat és az Input
            if (input != "empty" && IsEmpty)
            {
                stepByStepTextBox.Text = "";
                List<String> RuleNumber = new List<string>();
                Stack<string> StepsStack = new Stack<string>();

                //Kezdeti adatok felvétele a verembe
                StepsStack.Push("#");
                StepsStack.Push("E");

                int InputIndex = 0;
                bool Stop = false;
                // StatusTuple.Item1 = Input szalag fennmaradó része
                // StatusTuple.Item2 = A verem aktuális tartalma
                // StatusTuple.Item3 = Eddig alkalmazott szabályok sorozata
                (string, string, string) StatusTuple = (input, "E#", "");
                stepByStepTextBox.AppendText(StatusTuple.Item1 + "," + StatusTuple.Item2 + "," + StatusTuple.Item3 + "\r\n");
                StatusTuple.Item3 = "";


                while (Stop == false)
                {
                    string Step = StepsStack.Peek();
                    int ColumnIndex = GetColumnIndex(input[InputIndex]);
                    int RowIndex = GetRowIndex(Step);
                    if (Step == "ε")
                    {
                        StepsStack.Pop();
                        StatusTuple.Item2 = String.Join("", StepsStack);
                        stepByStepTextBox.AppendText(StatusTuple.Item1 + "," + StatusTuple.Item2 + "," + StatusTuple.Item3 + "\r\n");
                    }
                    else
                    {
                        //Hiba ellenörzés
                        if (ColumnIndex == -1)
                        {
                            MessageBox.Show("Nem megfelelő input, hibás karakter a következő indexen: " + (InputIndex + 1), "Hiba");
                            break;
                        }
                        if (RowIndex == -1)
                        {
                            MessageBox.Show("Hiba a sornevekben: A stackben található karakter nincs a sornevek között", "Hiba");  
                            break;
                        }
                        

                        string Rule = dataGridView1.Rows[RowIndex].Cells[ColumnIndex].Value.ToString();
                        //metszet = dataGridView1.Rows[RowIndex].Cells[ColumnIndex].Value.ToString();

                        if (Rule == "")
                        {
                            MessageBox.Show("Hiba! Az input helytelen");
                            Stop = true;
                        }
                        else if (Rule == "accept")
                        {
                            Stop = true;
                            MessageBox.Show("Az input helyes");
                        }
                        else if (Rule == "pop")
                        {
                            StepsStack.Pop();
                            StatusTuple.Item2 = String.Join("", StepsStack);
                            InputIndex++;
                            StatusTuple.Item1 = input.Substring(InputIndex);
                            stepByStepTextBox.AppendText(StatusTuple.Item1 + "," + StatusTuple.Item2 + "," + StatusTuple.Item3 + "\r\n");
                        }
                        else
                        {
                            StepsStack.Pop();
                            Rule = Rule.Substring(1, Rule.Length - 2);
                            string[] NextSteps = Rule.Split(',');
                            for (int i = NextSteps[0].Length - 1; i > -1; i--)
                            {
                                if (i - 1 > -1 && NextSteps[0][i].ToString() == "'")
                                {
                                    StepsStack.Push(NextSteps[0][i - 1].ToString() + NextSteps[0][i].ToString());
                                    i--;
                                }
                                else
                                {
                                    StepsStack.Push(NextSteps[0][i].ToString());
                                }
                            }
                            RuleNumber.Add(NextSteps[1]);
                            StatusTuple.Item3 = StatusTuple.Item3 + NextSteps[1];
                            if (StepsStack.Peek() != "ε")
                            {
                                StatusTuple.Item2 = String.Join("", StepsStack);
                                stepByStepTextBox.AppendText(StatusTuple.Item1 + "," + StatusTuple.Item2 + "," + StatusTuple.Item3 + "\r\n");
                            }
                        }
                    }

                }
            }

        }

        public int GetRowIndex(string step)
        {
            int RowIndex = -1;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                string checkingString = dataGridView1.Rows[i].HeaderCell.Value.ToString();
                if (step == checkingString)
                {
                    RowIndex = i;
                }
            }
            return RowIndex;
        }

        public int GetColumnIndex(char step)
        {
            int ColumnIndex = -1;
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                if (step.ToString() == dataGridView1.Columns[i].HeaderCell.Value.ToString())
                {
                    ColumnIndex = i;
                }
            }
            return ColumnIndex;
        }
    }
}
