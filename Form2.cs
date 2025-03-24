using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MNKSolve
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length < 1)
            {
                MessageBox.Show("Введите количество строк с данными");
                return;
            }
            int rowCount = 0;
            try
            {
                rowCount = int.Parse(textBox2.Text);
            }
            catch { }
            if (rowCount < 1)
            {
                MessageBox.Show("Введите количество строк с данными");
                return;
            }

            if (textBox1.Text.Length < 1)
            {
                MessageBox.Show("Введите число переменных");
                return;
            }
            int varCount = 0;
            try
            {
                varCount = int.Parse(textBox1.Text);
            }
            catch { }

            if (varCount < 1)
            {
                MessageBox.Show("Введите число переменных");
                return;
            }

            if (varCount > rowCount)
            {
                MessageBox.Show("Количество строк с данными должно быть больше числа переменных");
                return;
            }

            MatrixMxN a = new MatrixMxN(rowCount, varCount);
            MatrixMxN b = new MatrixMxN(rowCount, 1);
            double t1;

            for (int j = 0; j < varCount; j++)
                for (int i = 0; i < rowCount; i++)
                {
                    try
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value.GetType() == typeof(double))
                        {
                            t1 = (double)dataGridView1.Rows[i].Cells[j].Value;
                        }
                        else
                        {
                            t1 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[j].Value);
                        }
                        a.Set(i, j, t1);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }

            for (int i = 0; i < rowCount; i++)
            {
                try
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.GetType() == typeof(double))
                    {
                        t1 = (double)dataGridView2.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        t1 = GeoLogUtils.mTryParse((string)dataGridView2.Rows[i].Cells[0].Value);
                    }
                    b.Set(i, 0, t1);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            dataGridView3.Rows.Clear();
            try
            {
                MatrixMxN At1 = MatrixMxN.Transponse(a);

                MatrixMxN mul1 = MatrixMxN.Mul(At1, a);

                MatrixMxN obrA = MatrixMxN.ObratNaya(mul1);

                if (obrA == null)
                {
                    MessageBox.Show("Решений нет!");
                    return;
                }

                MatrixMxN temp1 = MatrixMxN.Mul(obrA, At1);
                if (temp1 == null)
                {
                    MessageBox.Show("Решений нет!");
                    return;
                }

                MatrixMxN res = MatrixMxN.Mul(temp1, b);
                if (res == null)
                {
                    MessageBox.Show("Решений нет!");
                    return;
                }

                try
                {
                    for (int i = 0; i < res.m; i++)
                    {
                        dataGridView3.Rows.Add();
                        dataGridView3.Rows[i].Cells[0].Value = res.Get(i, 0);
                    }
                    MessageBox.Show("Решено!");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int gg1 = dataGridView1.Rows.Count - 1;
            int gg2 = dataGridView2.Rows.Count - 1;
            if (gg1 < gg2)
            {
                MessageBox.Show("Введите более " + gg2.ToString() + " строк значений");
                return;
            }
            if (gg1 != gg2)
            {
                MessageBox.Show("Количество строк значений в\nтаблицах А и В должно быть одинаково");
                return;
            }

            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;


            MatrixMxN a = new MatrixMxN(gg1, 6);
            //MatrixMxN b = new MatrixMxN(gg1, 1);
            double t1;

            for (int j = 0; j < 5; j++)
                for (int i = 0; i < gg1; i++)
                {
                    try
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value.GetType() == typeof(double))
                        {
                            t1 = (double)dataGridView1.Rows[i].Cells[j].Value;
                        }
                        else
                        {
                            t1 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[j].Value);
                        }
                        a.Set(i, j, t1);
                    }
                    catch 
                    {
                        a.Set(i, j, 0.0);
                    }
                }
            for (int i = 0; i < gg1; i++)
            {
                try
                {
                    if (dataGridView2.Rows[i].Cells[0].Value.GetType() == typeof(double))
                    {
                        t1 = (double)dataGridView2.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        t1 = GeoLogUtils.mTryParse((string)dataGridView2.Rows[i].Cells[0].Value);
                    }
                    a.Set(i, 5, t1);
                }
                catch
                {
                    a.Set(i, 5, 0.0);
                }
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(MatrixMxN));

            // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                xmlSerializer.Serialize(fs, a);
                Console.WriteLine("Object has been serialized");
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(MatrixMxN));
                // десериализуем объект
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.OpenOrCreate))
                {
                    MatrixMxN person = xmlSerializer.Deserialize(fs) as MatrixMxN;
                    dataGridView1.Rows.Clear();
                    for (int j = 0; j < person.n-1; j++)
                        for (int i = 0; i < person.m; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[j].Value = person.Get(i, j);
                    }
                    for (int i = 0; i < person.m; i++)
                    {
                        dataGridView2.Rows.Add();
                        dataGridView2.Rows[i].Cells[0].Value = person.Get(i, person.n-1);
                    }
                    textBox1.Text = (person.n - 1).ToString();
                    textBox2.Text = (person.m).ToString();
                    //  Console.WriteLine($"Name: {person?.Name}  Age: {person?.Age}");
                }

            }

        }
    }
}

