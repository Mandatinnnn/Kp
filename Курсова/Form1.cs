using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Imaging;

namespace Курсова
{
    public partial class Form1 : Form
    {
        public bool checkButt = true;

        Chart chart1 = new Chart();
        ChartArea chartAreaForChart1 = new ChartArea("ChartArea1");

        Chart chart2 = new Chart();
        ChartArea chartAreaForChart2 = new ChartArea("ChartArea2");

        PictureBox pict1 = new PictureBox();

        PictureBox pict2 = new PictureBox();

        Label labelForImage = new Label();

        public Form1()
        {
            InitializeComponent();
        }

        private double resFunction1(double q, double a, double x)
        {
            return Math.Log(a + x) / (q + x);
        }

        private double resFunction2(double q, double x)
        {
            return Math.Pow(q + x, 1.0 / 3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Заповніть необхідні поля", "Помилка");
            }
            else
            {
                Random ran = new Random();
                textBox4.Text = 0.ToString();
                textBox5.Text = 0.ToString();
                double x_min, x_max, dx, y_min_for_f1 = 0, y_min_for_f2 = 0, y_max_for_f1 = 0, y_max_for_f2 = 0;
                bool error1, error2, error3;
                error1 = Double.TryParse(textBox1.Text, out x_min);
                error2 = Double.TryParse(textBox2.Text, out x_max);
                error3 = Double.TryParse(textBox3.Text, out dx);
                if (!error1 || !error2 || !error3)
                {
                    MessageBox.Show("Поля введено некоректно", "Помилка");
                }
                else
                {
                    foreach (Series series in chart1.Series)
                    {
                        series.Points.Clear();
                    }

                    foreach (Series series in chart2.Series)
                    {
                        series.Points.Clear();
                    }

                    string s1 = "", s2 = "";
                    double a = Math.Round(ran.NextDouble(), 2);

                    listBox1.Items.Clear();
                    listBox2.Items.Clear();

                    if (!checkButt)
                    {
                        pict1.Image.Dispose();
                        pict1.Image = null;
                        pict2.Image.Dispose();
                        pict2.Image = null; 
                    }

                    for (double i = x_min; i <= x_max; i += dx)
                    {
                        double q = Math.Round(ran.NextDouble(), 2);
                        if (q <= 0.55)
                        {
                            if (double.IsNaN(resFunction1(q, a, i)) || double.IsInfinity(resFunction1(q, a, i)))
                            {
                                listBox1.Items.Add("Для x = " + Math.Round(i, 2) + " результат y = There is no solution. q = " + q + " and a = " + a);
                                s1 += " " + Math.Round(i, 2);
                            }
                            else
                            {
                                listBox1.Items.Add("Для x = " + Math.Round(i, 2) + " результат y = " + Math.Round(resFunction1(q, a, i), 2) + " q = " + q + " and a = " + a);
                                chart1.Series["MySeries1"].Points.AddXY(i, Math.Round(resFunction1(q, a, i), 2));
                                y_max_for_f1 = Math.Max(y_max_for_f1, Math.Round(resFunction1(q, a, i), 2));
                                y_min_for_f1 = Math.Min(y_min_for_f1, Math.Round(resFunction1(q, a, i), 2));
                            }
                            textBox4.Text = (Int32.Parse(textBox4.Text.ToString()) + 1).ToString();
                        }
                        else
                        {
                            if (double.IsNaN(resFunction2(q, i)) || double.IsInfinity(resFunction2(q, i)))
                            {
                                listBox2.Items.Add("Для x = " + Math.Round(i, 2) + " результат y = There is no solution. q = " + q);
                                s2 += " " + Math.Round(i, 2);
                            }
                            else
                            {
                                listBox2.Items.Add("Для x = " + Math.Round(i, 2) + " результат y = " + Math.Round(resFunction2(q, i), 2) + " q = " + q);
                                chart2.Series["MySeries2"].Points.AddXY(i, Math.Round(resFunction2(q, i), 2));
                                y_max_for_f2 = Math.Max(y_max_for_f2, Math.Round(resFunction2(q, i), 2));
                                y_min_for_f2 = Math.Min(y_min_for_f2, Math.Round(resFunction2(q, i), 2));
                            }
                            textBox5.Text = (Int32.Parse(textBox5.Text.ToString()) + 1).ToString();
                        }
                    }

                    //Вивід повідомлень про неіснування розв'язків функцій
                    if (s1 != "")
                        MessageBox.Show("Розв'язку немає для x:" + s1 + ".", "Немає розв'язку для рівняння 1");
                    if (s2 != "")
                        MessageBox.Show("Розв'язку немає для x:" + s2 + ".", "Немає розв'язку для рівняння 2");

                    chart1.ChartAreas[0].AxisX.Minimum = x_min;
                    chart1.ChartAreas[0].AxisX.Maximum = x_max;
                    chart1.ChartAreas[0].AxisY.Minimum = y_min_for_f1;
                    chart1.ChartAreas[0].AxisY.Maximum = y_max_for_f1;

                    chart2.ChartAreas[0].AxisX.Minimum = x_min ;
                    chart2.ChartAreas[0].AxisX.Maximum = x_max;
                    chart2.ChartAreas[0].AxisY.Minimum = y_min_for_f2;
                    chart2.ChartAreas[0].AxisY.Maximum = y_max_for_f2;
                   
                    chart1.Invalidate();
                    chart2.Invalidate();

                    //Конвертування графіків в зображення
                    chart1.SaveImage("D:\\С#\\Курсова\\Курсова\\bin\\Debug\\image1.png", ImageFormat.Png);
                    chart2.SaveImage("D:\\С#\\Курсова\\Курсова\\bin\\Debug\\image2.png", ImageFormat.Png);

                    //Вивід надпису до зображень
                    labelForImage.Text = "Графікі по точкам, які знайшли в ході дублювання функцій.";
                    Form1.ActiveForm.Controls.Add(labelForImage);

                    //Вивід зображень
                    pict1.Image = Image.FromFile("D:\\С#\\Курсова\\Курсова\\bin\\Debug\\image1.png");
                    pict2.Image = Image.FromFile("D:\\С#\\Курсова\\Курсова\\bin\\Debug\\image2.png");
                    Form1.ActiveForm.Controls.Add(pict1);
                    Form1.ActiveForm.Controls.Add(pict2);

                    //Збільшення форми
                    if (checkButt)
                    {
                        this.Height += pict1.Height + 55;
                        checkButt = false;
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            chart1.ChartAreas.Add(chartAreaForChart1);
            chart1.Series.Add("MySeries1");
            chart1.Series["MySeries1"].ChartType = SeriesChartType.Line;

            chart2.ChartAreas.Add(chartAreaForChart2);
            chart2.Series.Add("MySeries2");
            chart2.Series["MySeries2"].ChartType = SeriesChartType.Line;

            //Стилізація графіків
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorTickMark.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorTickMark.Enabled = false;
            chart1.ChartAreas[0].AxisX.LineWidth = 2;
            chart1.ChartAreas[0].AxisY.LineWidth = 2;
            chart1.ChartAreas[0].AxisX.Title = "X-Axis";
            chart1.ChartAreas[0].AxisY.Title = "Y-Axis";

            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisX.MinorTickMark.Enabled = false;
            chart2.ChartAreas[0].AxisY.MinorTickMark.Enabled = false;
            chart2.ChartAreas[0].AxisX.LineWidth = 2;
            chart2.ChartAreas[0].AxisY.LineWidth = 2;
            chart2.ChartAreas[0].AxisX.Title = "X-Axis";
            chart2.ChartAreas[0].AxisY.Title = "Y-Axis";

            pict1.Location = new Point(87, 440);
            pict1.Size = new Size(300, 300);

            pict2.Location = new Point(563, 440);
            pict2.Size = new Size(300, 300);

            labelForImage.Location = new Point(226, 395);
            labelForImage.Size = new Size(600, 25);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("По-перше, для повного розуміння, що робить програма, потрібно ознайомитися з формулами, які використовуються для обрахунку.\nПо-друге, потрібно ввести всі необхідні дані.\nПісля цього, ви зможете побачити кількість обчислень по кожній формулі, графік по точкам, які були знайдені в ході обчислень, та результати самих обчислень.", "Інформація");
        }
    }
}