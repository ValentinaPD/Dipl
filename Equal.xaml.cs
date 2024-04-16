using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для Equal.xaml
    /// </summary>
    public partial class Equal : Window
    {
        public Equal()
        {
            InitializeComponent();
        }

        private void StartModelButton_Click(object sender, RoutedEventArgs e)
        {
            double p = Double.Parse(pTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double q = Double.Parse(qTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l1 = Double.Parse(Lambda1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l2 = Double.Parse(Lambda2TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a1 = Double.Parse(Alpha1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a2 = Double.Parse(Alpha2TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double tay = Double.Parse(tayTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double T = Double.Parse(timeTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            mainChart.Plot.Clear();
            expChart.Plot.Clear();
            expChart1.Plot.Clear();



            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            Flow flow = new Flow(parametrs);

            //Здесь решение уравнений!
            var solution = GetSolution(p, q, l1, l2, a1, a2, tay, T);
            flow.Generate(T);
            var coords = GetFunc(p, q, l1, l2, a1, a2, tay, tay);
            var x_cord = coords.Item1;
            var y_cord = coords.Item2;
            mainChart.Plot.AddScatter(x_cord.ToArray(), y_cord.ToArray());
            mainChart.Plot.AddHorizontalLine((solution.Item3 / 1000));
            mainChart.Refresh();
            MessageBox.Show("Решение уравнения "+(solution.Item1).ToString()+"\n"+"Выборочная вариация "+Math.Round(solution.Item2,3).ToString());
            List<String> expList = new List<String>();
            List<double> X = new List<double>();
            List<double> Y = new List<double>();
            List<double> Y2 = new List<double>();
            for (double k = tay; k <= tay+1; k += 0.1f)
            {
                solution = GetSolution(p, q, l1, l2, a1, a2, k, T);
                expList.Add("T = "+Math.Round(k,3)+"\nРешение " + solution.Item1.ToString() +"\nВариация " + solution.Item2.ToString());
                X.Add(k);
                Y.Add(solution.Item1);
                Y2.Add(solution.Item2);
            }
            listExp.ItemsSource = expList;
/*
            var line = expChart.Plot.AddScatter(X.ToArray(), Y2.ToArray());
            line.LineStyle = ScottPlot.LineStyle.None;
            line.MarkerSize = 10;
            line.MarkerColor = System.Drawing.Color.Orange;
            expChart.Plot.XLabel("T");
            expChart.Plot.YLabel("V");
            

            var line2 = expChart1.Plot.AddScatter(X.ToArray(), Y.ToArray());
            line2.LineStyle = ScottPlot.LineStyle.None;
            line2.MarkerSize = 10;
            line2.MarkerColor = System.Drawing.Color.Orange;
            expChart1.Plot.XLabel("T");
            expChart1.Plot.YLabel("M");

            expChart.Refresh();
            expChart1.Refresh();*/
        }

        private (List<double>, List<double> ) GetFunc(double p, double q, double l1, double l2, double a1, double a2, double tay, double tay_min)
        {
            List<double> xCoord = new List<double>();
            List<double> yCoord = new List<double>();
            double z1 = 0.5f * (l1 + a1 + l2 + a2 - Math.Sqrt(Math.Pow(l1 + a1 - l2 - a2, 2) + 4 * (1 - q) * a2 * (1 - p) * a1));
            double z2 = 0.5f * (l1 + a1 + l2 + a2 + Math.Sqrt(Math.Pow(l1 + a1 - l2 - a2, 2) + 4 * (1 - q) * a2 * (1 - p) * a1));
            for (double t = 0; t <= tay_min+tay_min/10; t = t + 0.1)
            {
                double gamma1 = (z2 - l1 - l2) / (z2 - z1);
                double gamma2 = -(z1 - l1 - l2) / (z2 - z1);
                double fi_0 = gamma1 * Math.Exp(-z1 * t) + gamma2 * Math.Exp(-z2 * t);
                double M_ksi = (1 / fi_0) * (1 / (z1 * z2)) * (gamma1 * z2 + gamma2 * z1 - gamma1 * z2 * Math.Exp(-z1 * t) - gamma2 * z1 * Math.Exp(-z2 * t));
                double A = (1 / (a1 + a2)) * (1 / Math.Pow(z1 * z2, 2)) * (l1 - l2 + q * a1 - p * a2) * (l1 + p * a1 - l2 - a2 * q) * a1 * a2;
                double chisl = gamma1 * Math.Exp(-t * (a1 + a2 + z1)) + gamma2 * Math.Exp(-t * (a1 + a2 + z2));
                double zn = 1 - ((1 - Math.Exp(-t * (a1 + a2 + z1)) * z1 * gamma1) / (a1 + a2 + z1)) - ((1 - Math.Exp(-t * (a1 + a2 + z2)) * z2 * gamma2) / (a1 + a2 + z2));
                double func = (1 / (z1 * z2)) * (z1 + z2 - (z1 * z2) / (a1 + a2)) + M_ksi - A * chisl / zn;
                xCoord.Add(t);
                yCoord.Add(func);
            }
            return((xCoord, yCoord));
        }
        private (double,double,double) GetSolution(double p, double q, double l1, double l2, double a1, double a2, double tay,double T)
        {
            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            Flow flow = new Flow(parametrs);

            double sum_rez = 0;
            double stat_f = 0;
            List<double> res_est = new List<double>();
            List<double> tay_min = new List<double>();
            double n = 1000;

            for (int k = 0; k < n; k++)
            {
                flow.Generate(T);
                double stat1 = flow.GetAverageTimeInFinalFlow();
                if (double.IsNaN(stat1))
                {
                    continue;
                }
                stat_f = stat_f + stat1;
                Equation equation = new Equation();
                double m = 0;
                if (flow.GetDurationOfIntervals().Count > 0)
                {
                    m = flow.GetDurationOfIntervals().Min();
                }
                double res = equation.GetSolution(a1, a2, l1, l2, p, q, stat1,m);
                sum_rez = sum_rez + res;
                res_est.Add(res);
                //MessageBox.Show(flow.GetDurationOfIntervals().Min().ToString());
                
            }
            double disp = 0;
            double res_final = sum_rez / n;
            for (int k = 0; k < res_est.Count; k++)
            {
                disp = disp + (res_est[k] - res_final) * (res_est[k] - res_final);
            }
            double disp_final = disp / res_est.Count;
            return ((res_final, disp_final,stat_f));
        }
    }
}
