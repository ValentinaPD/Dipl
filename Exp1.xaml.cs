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
    /// Логика взаимодействия для Exp1.xaml
    /// </summary>
    public class ExpResult
    {
        private string v1;
        private double v2;
        private double v3;
        private double v4;
        private double v5;
        private double v6;
        private double v7;
        public ExpResult()
        {
        }
        public ExpResult(string v1, double v2, double v3, double v4, double v5, double v6, double v7)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
            this.v4 = v4;
            this.v5 = v5;
            this.v6 = v6;
            this.v7 = v7;
        }

        public string V1 {
            get { return this.v1; }
            set { this.v1 = value; }
        }
        public double V2 {
            get { return this.v2; }
            set { this.v2 = value; }
        }
        public double V3 {
            get { return this.v3; }
            set { this.v3 = value; }
        }
        public double V4 {
            get { return this.v4; }
            set { this.v4 = value; }
        }
        public double V5 {
            get { return this.v5; }
            set { this.v5 = value; }
        }
        public double V6 {
            get { return this.v6; }
            set { this.v6 = value; }
        }
        public double V7
        {
            get { return this.v7; }
            set { this.v7 = value; }
        }

    }
    public partial class Exp1 : Window
    {
        public Exp1()
        {
            InitializeComponent();
        }

        private void StartModelButton_Click(object sender, RoutedEventArgs e)
        {
            double p = Double.Parse(pTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double q = Double.Parse(pTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l1 = Double.Parse(Lambda1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l2 = Double.Parse(Lambda2TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a1 = Double.Parse(Alpha1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a2 = Double.Parse(Alpha2TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double tay = Double.Parse(tayTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double T = Double.Parse(timeTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);

            Exp(parametrs);

        }
        private (double,double) GetParams(double l1,double l2)
        {
            /*           double x = l1 * l2;
                        l1 = l1 + 0.1;
                        l2 = x / l1;
                        return ((l1, l2));*/
            double x = l1*l2;

            while(true)
            {
                l1 = l1 + 0.2;
                l2 = x / l1;
                if(l1*l2 == x) break;
            } 
            return ((l1, l2));
        }
        private void Draw(WpfPlot plt,List<double> x_coords, List<double> y_coords)
        {
            plt.Plot.Clear();
            var line = plt.Plot.AddScatter(x_coords.ToArray(), y_coords.ToArray());
            line.LineStyle = ScottPlot.LineStyle.None;
            line.MarkerSize = 10;
            line.MarkerColor = System.Drawing.Color.Orange;
            plt.Refresh();
        }
        private void Draw(WpfPlot plt, List<double> x_coords, List<double> y_coords,string x_axis_title, string y_axis_title, string plot_title)
        {
            Draw(plt, x_coords, y_coords);
            plt.Plot.XLabel(x_axis_title);
            plt.Plot.YLabel(y_axis_title);
            plt.Plot.Title(plot_title);
            plt.Refresh();
        }
        private void Exp(Parametrs parametrs)
        {
            List<WpfPlot> plots = new List<WpfPlot>();
            List<DataGrid> grids = new List<DataGrid>();
            plots.Add(plot1);plots.Add(plot2);plots.Add(plot3);plots.Add(plot4);plots.Add(plot5);plots.Add(plot6);
            grids.Add(Exp1Grid); grids.Add(Exp2Grid); grids.Add(Exp3Grid); grids.Add(Exp4Grid); grids.Add(Exp5Grid); grids.Add(Exp6Grid);
            double tay = 0;
            double tay_0 = parametrs.DTime;
            //var begin_param = parametrs;
            List<double> xs = new List<double>();
            List<double> ys_v = new List<double>();
            List<double> ys_m = new List<double>();
            List<List<ExpResult>> ResultList = new List<List<ExpResult>>();
            //ResultList.Add(new ExpResult("М(т)", 1, 1, 1, 1, 1));
            for (int i = 0; i < 6; i++)
            {
                xs.Clear();
                ys_v.Clear();
                ys_m.Clear();
               // ResultList.Clear();
               // ResultList.Clear();
                //parametrs = new Parametrs(begin_param.P, begin_param.Q, begin_param.Lamda[0], begin_param.Lamda[1], begin_param.Alpha[0], begin_param.Alpha[1], tay);
                tay = parametrs.DTime;
                for (int j = 0; j < 10; j++)
                {
                    tay = tay + 0.1;
                    var solution = GetSolution(parametrs.P, parametrs.Q, parametrs.Lamda[0], parametrs.Lamda[1], parametrs.Alpha[0], parametrs.Alpha[1], tay, 2000);
                    xs.Add(tay);
                    //MessageBox.Show(parametrs.Lamda[0].ToString());
                    ys_v.Add(solution.Item2);
                    ys_m.Add(solution.Item1);


                }
                ResultList.Add(new List<ExpResult>());
                ResultList[i].Add(new ExpResult("М(т)", Math.Round(ys_m[0],6), Math.Round(ys_m[1],6), Math.Round(ys_m[2], 6), Math.Round(ys_m[3], 6), Math.Round(ys_m[4], 6), Math.Round(ys_m[5], 6)));
                ResultList[i].Add(new ExpResult("V(т)", Math.Round(ys_v[0], 6), Math.Round(ys_v[1], 6), Math.Round(ys_v[2], 6), Math.Round(ys_v[3], 6), Math.Round(ys_v[4], 6), Math.Round(ys_v[5], 6)));
                
                Draw(plots[i], xs, ys_v, "T","?", "λ1 = " + parametrs.Lamda[0].ToString());
                grids[i].ItemsSource = ResultList[i];
                grids[i].Columns[0].Header = "λ1 =  " + parametrs.Lamda[0].ToString();
                grids[i].Columns[1].Header = "T = " + tay_0.ToString();
                grids[i].Columns[2].Header = "T = " + (tay_0+0.1*1).ToString();
                grids[i].Columns[3].Header = "T = " + (tay_0 + 0.1 * 2).ToString();
                grids[i].Columns[4].Header = "T = " + (tay_0 + 0.1 * 3).ToString();
                grids[i].Columns[5].Header = "T = " + (tay_0 + 0.1 * 4).ToString();
                grids[i].Columns[6].Header = "T = " + (tay_0 + 0.1 * 5).ToString();
                var l = GetParams(parametrs.Lamda[0], parametrs.Lamda[1]);
                parametrs.Lamda[0] = l.Item1;
                parametrs.Lamda[1] = l.Item2;

                //grids[i].ItemsSource = ResultList[i];
            }
            //Exp1Grid.ItemsSource = ResultList;
            

        }
        private (List<double>, List<double>) GetFunc(double p, double q, double l1, double l2, double a1, double a2, double tay, double tay_min)
        {
            List<double> xCoord = new List<double>();
            List<double> yCoord = new List<double>();
            double z1 = 0.5f * (l1 + a1 + l2 + a2 - Math.Sqrt(Math.Pow(l1 + a1 - l2 - a2, 2) + 4 * (1 - q) * a2 * (1 - p) * a1));
            double z2 = 0.5f * (l1 + a1 + l2 + a2 + Math.Sqrt(Math.Pow(l1 + a1 - l2 - a2, 2) + 4 * (1 - q) * a2 * (1 - p) * a1));
            for (double t = 0; t <= tay_min + tay_min / 10; t = t + 0.1)
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
            return ((xCoord, yCoord));
        }
        private (double, double, double) GetSolution(double p, double q, double l1, double l2, double a1, double a2, double tay, double T)
        {
            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            Flow flow = new Flow(parametrs);

            double sum_rez = 0;
            double stat_f = 0;
            List<double> res_est = new List<double>();
            List<double> tay_min = new List<double>();
            double n = 100;

            for (int k = 0; k < n; k++)
            {
                flow.Generate(T);
                double stat1 = flow.GetAverageTimeInFinalFlow();
                double m = 0;
                if (flow.GetDurationOfIntervals().Count > 0)
                {
                    m = flow.GetDurationOfIntervals().Min();
                }
                stat_f = stat_f + stat1;
                Equation equation = new Equation();
                double res = equation.GetSolution(a1, a2, l1, l2, p, q, stat1,m);
                sum_rez = sum_rez + res;
                res_est.Add(res);
            }
            double disp = 0;
            double res_final = sum_rez / n;
            for (int k = 0; k < res_est.Count; k++)
            {
                disp = disp + (res_est[k] - res_final) * (res_est[k] - res_final);
            }
            double disp_final = disp / res_est.Count;
            return ((res_final, disp_final, stat_f));
        }
        
    }
}
