using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace MNKSolve
{

    public partial class Form1 : Form
    {
        // DataTable dt;
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Length < 1)
            {
                MessageBox.Show("Введите количество строк значений");
                return;
            }
            int gg1 = int.Parse(textBox4.Text); //dataGridView1.Rows.Count-1;
            if (gg1 < 3)
            {
                MessageBox.Show("Введите более 3 строк значений");
                return;
            }
            MatrixMxN a = new MatrixMxN(gg1, 3);
            MatrixMxN b = new MatrixMxN(gg1, 1);

            double t1;
            double r10;
            double r1;
            for (int i = 0; i < gg1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.GetType() == typeof(double))
                {
                    t1 = (double)dataGridView1.Rows[i].Cells[0].Value;
                }
                else
                {
                    t1 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[0].Value);
                }
                if (dataGridView1.Rows[i].Cells[1].Value.GetType() == typeof(double))
                {
                    r10 = (double)dataGridView1.Rows[i].Cells[1].Value;
                }
                else
                    r10 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[1].Value);
                if (dataGridView1.Rows[i].Cells[2].Value.GetType() == typeof(double))
                {
                    r1 = (double)dataGridView1.Rows[i].Cells[2].Value;
                }
                else
                    r1 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[2].Value);

                a.Set(i, 0, t1);
                a.Set(i, 1, t1 * r10);
                a.Set(i, 2, 1.0);
                b.Set(i, 0, r1 - r10);
            }           

            MatrixMxN At1 = MatrixMxN.Transponse(a);

            MatrixMxN mul1 = MatrixMxN.Mul(At1, a);

            MatrixMxN obrA = MatrixMxN.ObratNaya(mul1);

            MatrixMxN temp1 = MatrixMxN.Mul(obrA, At1);

            MatrixMxN res = MatrixMxN.Mul(temp1, b);

            textBox1.Text = res.Get(0, 0).ToString();
            textBox2.Text = res.Get(1, 0).ToString();
            textBox3.Text = res.Get(2, 0).ToString();


            int hh = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {

          //  SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;

            int gg1 = dataGridView1.Rows.Count - 1;
            if (gg1 < 3) MessageBox.Show("Введите более 3 строк значений");
            MatrixMxN a = new MatrixMxN(gg1, 3);
            MatrixMxN b = new MatrixMxN(gg1, 1);
            double t1;
            double r10;
            double r1;

            for (int i = 0; i < gg1; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.GetType() == typeof(double))
                {
                    t1 = (double)dataGridView1.Rows[i].Cells[0].Value;
                }
                else
                {
                    t1 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[0].Value);
                }
                if (dataGridView1.Rows[i].Cells[1].Value.GetType() == typeof(double))
                {
                    r10 = (double)dataGridView1.Rows[i].Cells[1].Value;
                }
                else
                    r10 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[1].Value);
                if (dataGridView1.Rows[i].Cells[2].Value.GetType() == typeof(double))
                {
                    r1 = (double)dataGridView1.Rows[i].Cells[2].Value;
                }
                else
                    r1 = GeoLogUtils.mTryParse((string)dataGridView1.Rows[i].Cells[2].Value);
                a.Set(i, 0, t1);
                a.Set(i, 1, r10);
                a.Set(i, 2, r1);                
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
                    for (int i = 0; i < person.m; i++)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[i].Cells[0].Value = person.Get(i, 0);
                        dataGridView1.Rows[i].Cells[1].Value = person.Get(i, 1);
                        dataGridView1.Rows[i].Cells[2].Value = person.Get(i, 2);
                    }
                    //  Console.WriteLine($"Name: {person?.Name}  Age: {person?.Age}");
                }

            }

        }
    }
}
