using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Globalization;
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
namespace СНАУ
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart_initialize();          
        }
        double clarity = 0.1;
        static byte f = 0;
        double glob_a, glob_b, x = 10.165;
        public  List<double> solutions;
        static ArrayList delegates = new ArrayList();//массив для самих функций(первые 3 элемента) и для методов (следующие 3)
        public delegate double one(double var);
        public static one it = new one(functions.func1);

        private void Form1_Load(object sender, EventArgs e)
        {
            CultureInfo inf = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = inf;
            inf.NumberFormat.NumberDecimalSeparator = ".";

            listBox1.Items.Add("x*x + 8*x - 5");
            listBox1.Items.Add("Math.Exp(-x * x) - (x - 1) * (x - 1)");
            listBox1.Items.Add("x*x- Math.Cos(x)");

            comboBox1.Items.Add("Метод дихотомии");
            comboBox1.Items.Add("Метод касательных");

            delegates.Add(it);
            it = functions.func2;
            delegates.Add(it);
            it = functions.func3;
            delegates.Add(it);
            comboBox1.SelectedIndex = 0;
            button1.Click += Calculate;
            Calculate(this, e);
        }

        void Calculate(object sender, EventArgs e)
        {
            output.Text = "";
            try {
                clarity = Convert.ToDouble(textBox2.Text);
                string[] gr = textBox1.Text.Split(',');
                glob_a = Convert.ToDouble(gr[0]);
                glob_b = Convert.ToDouble(gr[1]);
            }
            catch(FormatException)
            {
                MessageBox.Show("Введите числовые значения");
            }
            switch (listBox1.SelectedIndex)
            {
                case 0: f = 0; break;
                case 1: f = 1; break;
                case 2: f = 2; break;
            }
            it = (one)delegates[f];
            switch (comboBox1.SelectedIndex)
            {
                case 0: dihotom(); break;
                case 1: Newton(); break;
            }
            draw();

            output.Text = "Численный ответ:" + "\n";
            for (int i = 0; i < solutions.Count; i++)
                output.Text += Convert.ToString(solutions[i]) + "\n";
        }

        public void dihotom()
        {
            solutions = new List<double>();
            // check_d(it);            
            double a = glob_a, b = glob_b, c = 0;
            double step = clarity * 1.5;
            do
            {
                for (x = a + step; x < glob_b - 0.5 * clarity; x += step)
                    if (it(x) * it(x - step) < 0)
                        break;
                if (x >= glob_b - 0.5 * clarity)//промежуток не найден
                    break;
                b = x;
                a = b - step;
                do
                {
                    c = (a + b) / 2;
                    if (it(a) * it(c) > 0)
                        a = c;
                    else b = c;
                } while (b - a > clarity);
                if (c > glob_b)
                    break;
                solutions.Add(c);
                a = c;
            } while (glob_b - a >= clarity);
        }

        public void Newton()
        {
            solutions = new List<double>();
            double mun = 1;//множитель 1 или -1
            double a = glob_a, b = glob_b, c = 0;
            do
            {
                for (x = a + 0.1; x < glob_b - clarity; x += 0.1)
                    if (it(x) * it(x - 0.1) < 0)
                        break;
                if (x >= glob_b - clarity)//промежуток не найден
                    break;
                b = x;
                a = b - 0.1;
                double first = Derivative.first((a + b) / 2, it);
                if (Math.Round(first, 6) == 0.0)//если первой производной нет
                    return;
                double second = Derivative.second((a + b) / 2, first, it);
                if (Math.Round(second, 12) == 0.0)
                    return;

                double e1 = b - a, e2 = 0, temp_point = 0;
                if (second < 0)
                    mun = -1;
                if (first < 0 && it(a) * it(b) < 0)
                {
                    temp_point = a;//точка, к которой теперь проводим касательную
                    while (e1 - e2 > Math.Abs(clarity))
                    {
                        e1 = e2;
                        e2 = temp_point - it(temp_point) / Derivative.first(temp_point, it);
                        temp_point = e2;
                    }
                    solutions.Add(e2);
                    a = b;
                }
                else//first<0
                {
                    temp_point = b;
                    while (e1 - e2 > Math.Abs(clarity) && it(a) * it(b) < 0)
                    {
                        e1 = e2;
                        e2 = temp_point - it(temp_point) / Derivative.first(temp_point, it);
                        temp_point = e2;
                    }
                    solutions.Add(e2);
                    a = b;
                }
            } while (b < glob_b);
        }

        void chart_initialize()
        {
            chart1.ChartAreas.Add(new ChartArea("Default"));
            chart1.Series.Add(new Series());
            chart1.Series.Add(new Series());
            chart1.Series[0].ChartType = SeriesChartType.Line;
            chart1.ChartAreas[0].AxisX.LineWidth += 2;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.Crossing = 0;
            chart1.ChartAreas[0].AxisY.Crossing = 0;

            chart1.Series[1].ChartType = SeriesChartType.Line;
        }

        void draw()
        {
            chart1.Series[1].Points.Clear();
            for (int i = 0; i < 1000; i++)
                chart1.Series[1].Points.AddXY(glob_a + (glob_b - glob_a) * i / 1000, it(glob_a + (glob_b - glob_a) * i / 1000));
        }

       
    }
}

