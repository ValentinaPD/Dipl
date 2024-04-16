using ScottPlot;
using ScottPlot.Palettes;
using ScottPlot.Styles;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using MaterialDesignColors.ColorManipulation;
using MathNet.Numerics.Distributions;

namespace Курсовая
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
            mainChart.Plot.Clear();
            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            Flow flow = new Flow(parametrs);
            flow.Generate(T);
            mainGrid.ItemsSource = flow.GetFlowEvents();

            mainGrid.Columns[0].Header = "Время";
            mainGrid.Columns[1].Header = "Состояние";
            mainGrid.Columns[2].Header = "Доп";
            mainGrid.Columns[3].Header = "Наблюдаемость";
            DataParser dataParser = new DataParser(flow.GetEventList(), flow.DeadTimeInformation, flow.StatesInformation);
            GraphCreator drawer = new GraphCreator(mainChart.Plot);
            drawer.DrawArrows(dataParser.GetArrows());
            drawer.DrawOpenPoints(dataParser.GetOpenPoints().xs, dataParser.GetOpenPoints().ys);
            drawer.DrawFullPoints(dataParser.GetFullPoints().xs, dataParser.GetFullPoints().ys);
            drawer.DrawDashLines(dataParser.GetDashVerticalLines());
            drawer.DrawArrowsWithoutHead(dataParser.GetArrowsWithoutHead());
            drawer.DrawRedOpenPoints(dataParser.GetAdditionsalPoints().xs, dataParser.GetAdditionsalPoints().ys);
            drawer.DrawRedFullPoints(dataParser.GetFullAdditionsalPoints().xs, dataParser.GetFullAdditionsalPoints().ys);
            drawer.SetLimits(dataParser.GetS1Y());
            mainChart.Plot.AddVerticalLine(0);
            mainChart.Plot.AddVerticalLine(T);
            mainChart.Plot.YAxis.Line(false);
            mainChart.Plot.YAxis.Ticks(false);
            mainChart.Plot.XAxis2.Line(false);
            mainChart.Plot.YAxis2.Line(false);
            mainChart.Plot.XAxis.Color(System.Drawing.Color.Black);
            mainChart.Plot.XAxis.TickMarkColor(System.Drawing.Color.Teal);
            mainChart.Plot.XLabel("t");

            mainChart.Refresh();

            visibleCountTextbox.Text = "Количество наблюдаемых событий " + flow.GetVisibleEventsCount();
            CountTextbox.Text = "Количество событий " + flow.GetEventsCount();
            inVisibleCountTextbox.Text = "Количество не наблюдаемых событий " + flow.GetInVisibleEventsCount();
            additionalCountTextbox.Text = "Количество дополнительных событий " + flow.GetAdditionalEventsCount();
        }

        private void StartTestButton_Click(object sender, RoutedEventArgs e)
        {
            #region             Плохой код :(
            double p = Double.Parse(pTextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double q = Double.Parse(pTextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l1 = Double.Parse(Lambda1TextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l2 = Double.Parse(Lambda2TextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a1 = Double.Parse(Alpha1TextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a2 = Double.Parse(Alpha2TextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double tay = Double.Parse(tayTextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double T = Double.Parse(timeTextBox1.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double step = Double.Parse(H1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double pointsCoint = Double.Parse(PointsCount1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double startPoint = Double.Parse(StartPoint1TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            
            exp1.Plot.XAxis.Label("Количество опытов");
            exp1plot2.Plot.XAxis.Label("Количество опытов");
            exp1plot3.Plot.XAxis.Label("Количество опытов");


            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            ParamsChanges changes = new ParamsChanges(0, 0, 0, 0, 0, 0, step);
            var res = ExpExperimentCount(parametrs, startPoint, pointsCoint, T, changes);

            List<String> averageFlowList = new List<String>();
            List<String> averageFinalFlowList = new List<String>();
            List<String> averrageDeadTimeList = new List<String>();
            for (int i = 0; i < res.averageFlow.Count; i++)
            {
                averageFlowList.Add(res.ExpCount[i] + "               " + res.averageFlow[i]);
                averageFinalFlowList.Add(res.ExpCount[i] + "              " + res.averageFinalFlow[i]);
                averrageDeadTimeList.Add(res.ExpCount[i] + "              " + res.averrageDeadTime[i]);
            }
            // list1.ItemsSource
            list1.ItemsSource = averageFlowList.ToArray();
            list2.ItemsSource = averageFinalFlowList;
            list3.ItemsSource = averrageDeadTimeList;
            Draw(exp1, res.ExpCount, res.averageFlow, (res.averageFlow.Sum() / res.averageFlow.Count) - 0.1, (res.averageFlow.Sum() / res.averageFlow.Count) + 0.1);
            Draw(exp1plot2, res.ExpCount, res.averageFinalFlow, (res.averageFinalFlow.Sum() / res.averageFinalFlow.Count) - 0.1, (res.averageFinalFlow.Sum() / res.averageFinalFlow.Count) + 0.1);
            Draw(exp1plot3, res.ExpCount, res.averrageDeadTime, (res.averrageDeadTime.Sum() / res.averrageDeadTime.Count) - 0.1, (res.averrageDeadTime.Sum() / res.averrageDeadTime.Count) + 0.1);


            list1Text.Text = "Выборочное среднее длительности интервалов между событиями в исходном потоке ";
            list2Text.Text = "Выборочное среднее длительности интервалов между событиями в наблюдаемом потоке ";
            list3Text.Text = "Выборочное среднее длительности общего периода ненаблюдаемости ";
            #endregion





        }

        private void StartTest1Button_Click(object sender, RoutedEventArgs e)
        {
            double p = Double.Parse(pTextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double q = Double.Parse(pTextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l1 = Double.Parse(Lambda1TextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l2 = Double.Parse(Lambda2TextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a1 = Double.Parse(Alpha1TextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a2 = Double.Parse(Alpha2TextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double tay = Double.Parse(tayTextBox12.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            double step = Double.Parse(H2TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double pointsCoint = Double.Parse(Points2CountTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double startPoint = Double.Parse(StartPoint2TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double expCount = Double.Parse(CountExpTextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);


            exp2.Plot.XAxis.Label("Время моделирования");
            exp2plot2.Plot.XAxis.Label("Время моделирования");
            exp2plot3.Plot.XAxis.Label("Время моделирования");

            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            Flow flow = new Flow(parametrs);
            ParamsChanges changes = new ParamsChanges(0, 0, 0, 0, 0, step, 0);
            var res = ExpExperimentCount(parametrs, startPoint, pointsCoint, startPoint, changes);
            
            List<String> averageFlowList = new List<String>();
            List<String> averageFinalFlowList = new List<String>();
            List<String> averrageDeadTimeList = new List<String>();
            for (int i = 0; i < res.averageFlow.Count; i++)
            {
                averageFlowList.Add(res.ModelTime[i] + "               " + res.averageFlow[i]);
                averageFinalFlowList.Add(res.ModelTime[i] + "              " + res.averageFinalFlow[i]);
                averrageDeadTimeList.Add(res.ModelTime[i] + "              " + res.averrageDeadTime[i]);
            }
            list12.ItemsSource = averageFlowList.ToArray();
            list22.ItemsSource = averageFinalFlowList;
            list32.ItemsSource = averrageDeadTimeList;

            Draw(exp2, res.ModelTime, res.averageFlow, res.averageFlow.Sum() / res.averageFlow.Count - 0.1, res.averageFlow.Sum() / res.averageFlow.Count + 0.1);
            Draw(exp2plot2, res.ModelTime, res.averageFinalFlow, res.averageFinalFlow.Sum() / res.averageFinalFlow.Count - 0.1, res.averageFinalFlow.Sum() / res.averageFinalFlow.Count + 0.1);
            Draw(exp2plot3, res.ModelTime, res.averrageDeadTime, res.averrageDeadTime.Sum() / res.averrageDeadTime.Count - 0.1, res.averrageDeadTime.Sum() / res.averrageDeadTime.Count + 0.1);

            list12Text.Text = "Выборочное среднее длительности интервалов между событиями в исходном потоке ";
            list22Text.Text = "Выборочное среднее длительности интервалов между событиями в наблюдаемом потоке ";
            list32Text.Text = "Выборочное среднее длительности общего периода ненаблюдаемости ";



        }

        private void mainGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StartTestButton3_Click(object sender, RoutedEventArgs e)
        {
            double p = Double.Parse(pTextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double q = Double.Parse(pTextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l1 = Double.Parse(Lambda1TextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double l2 = Double.Parse(Lambda2TextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a1 = Double.Parse(Alpha1TextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double a2 = Double.Parse(Alpha2TextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double tay = Double.Parse(tayTextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double T = Double.Parse(timeTextBox3.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double step = Double.Parse(H3TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double pointsCoint = Double.Parse(PointsCount3TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double startPoint = Double.Parse(StartPoint3TextBox.Text.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            exp3.Plot.XAxis.Label("Значение длительности мертвого времени");
            exp3.Plot.YAxis.LabelStyle(fontSize: 5);

            exp3plot2.Plot.XAxis.Label("Значение длительности мертвого времени");
            exp1plot4.Plot.XAxis.Label("Значение длительности мертвого времени");

            Parametrs parametrs = new Parametrs(p, q, l1, l2, a1, a2, tay);
            Flow flow = new Flow(parametrs);
            ParamsChanges changes = new ParamsChanges(0, 0, 0, 0, 0.1, step, 0);
            var res = ExpExperimentCount(parametrs, startPoint, pointsCoint, startPoint, changes);

           
            List<String> averageFlowList = new List<String>();
            List<String> averageFinalFlowList = new List<String>();
            List<String> averrageDeadTimeList = new List<String>();
            for (int i = 0; i < res.averageFlow.Count; i++)
            {
                averageFlowList.Add(res.DTime[i] + "               " + res.averageFlow[i]);
                averageFinalFlowList.Add(res.DTime[i] + "              " + res.averageFinalFlow[i]);
                averrageDeadTimeList.Add(res.DTime[i] + "              " + res.averrageDeadTime[i]);
            }

            list4.ItemsSource = averageFlowList.ToArray();
            list5.ItemsSource = averageFinalFlowList;
            list6.ItemsSource = averrageDeadTimeList;

            Draw(exp3, res.DTime, res.averageFlow, (res.averageFlow.Sum() / res.averageFlow.Count) - 0.1, (res.averageFlow.Sum() / res.averageFlow.Count) + 0.1);
            Draw(exp3plot2, res.DTime, res.averageFinalFlow);
            Draw(exp1plot4, res.DTime, res.averrageDeadTime);



            list4Text.Text = "Выборочное среднее длительности интервалов между событиями в исходном потоке ";
            list5Text.Text = "Выборочное среднее длительности интервалов между событиями в наблюдаемом потоке ";
            list7Text.Text = "Выборочное среднее длительности общего периода ненаблюдаемости ";


        }
        private void Draw(WpfPlot plt, List<double> x_coords, List<double> y_coords)
        {
            plt.Plot.Clear();
            var line = plt.Plot.AddScatter(x_coords.ToArray(), y_coords.ToArray());
            line.LineStyle = ScottPlot.LineStyle.None;
            line.MarkerSize = 10;
            line.MarkerColor = System.Drawing.Color.Orange;
            plt.Refresh();
        }
        private void Draw(WpfPlot plt, List<double> x_coords, List<double> y_coords, double yMin, double yMax)
        {
            plt.Plot.Clear();
            Draw(plt, x_coords, y_coords);
            plt.Plot.SetAxisLimitsY(yMin, yMax);
            plt.Plot.Layout(padding: 10);
            plt.Refresh();
        }
        private (List<double> averageFlow,
                 List<double> averageFinalFlow,
                 List<double> averrageDeadTime,
                 List<double> ExpCount,
                 List<double> ModelTime,
                 List<double> DTime
            ) 
         ExpExperimentCount(Parametrs param, double expBegin,double pointsCoint,double modelTime, ParamsChanges paramChanges)
        {
            List<double> averageFlow = new List<double>();
            List<double> averageFinalFlow = new List<double>();
            List<double> averrageDeadTime = new List<double>();
            List<double> Y = new List<double>();
            List<double> Y1 = new List<double>();
            List<double> Y2 = new List<double>();
            double sumAverageInFlow = 0;
            double sumAverageInFinalFlow = 0;
            double sumAverrageDeadTime = 0;
            double expCount = expBegin;
            Flow flow = new Flow(param);
            for (int i = 0; i < pointsCoint; i++)
            {
                sumAverageInFlow = 0;
                sumAverageInFinalFlow = 0;
                sumAverrageDeadTime = 0;
                
                for (int j = 0; j < expCount; j++)
                {
                    flow.Generate(modelTime);
                    sumAverageInFlow += flow.GetAverageTimeInFlow();
                    sumAverageInFinalFlow += flow.GetAverageTimeInFinalFlow();
                    sumAverrageDeadTime += flow.GetAverageDeadTime();

                }
                averageFlow.Add(Math.Round((sumAverageInFlow / expCount), 4));
                averageFinalFlow.Add(Math.Round((sumAverageInFinalFlow / expCount), 4));
                averrageDeadTime.Add(Math.Round((sumAverrageDeadTime / expCount), 4));
                Y.Add(expCount);
                Y1.Add(modelTime);
                Y2.Add(param.DTime);
                var changes = paramChanges.GetChangeParametrs(param, modelTime, expCount);
                param = changes.param;
                modelTime = changes.modelTime;
                expCount = changes.expCount;
            }

            return ((averageFlow, averageFinalFlow, averrageDeadTime, Y,Y1,Y2));

        }

        internal struct ParamsChanges
        {
            public ParamsChanges(double dLamda1, double dLamda2, double dAlpha1, double dAlpha2, double dTay,double dModelTime, double dExpCount)
            {
                DAlpha1 = dAlpha1;
                DAlpha2 = dAlpha2;
                DLamda1 = dLamda1;
                DLamda2 = dLamda2;
                DTay = dTay;
                DModelTime = dModelTime;
                DExpCount = dExpCount;
            }
            public double DLamda1 { get; set; }
            public double DLamda2 { get; set; }
            public double DAlpha1 { get; set; }
            public double DAlpha2 { get; set; }
            public double DTay { get; set; }
            public double DModelTime { get; set; }
            public double DExpCount { get; set; }

            public (Parametrs param,double modelTime, double expCount) GetChangeParametrs(Parametrs param,double modelTime, double expCount)
            {
                param.Lamda[0] = param.Lamda[0] + DLamda1;
                param.Lamda[1] = param.Lamda[1] + DLamda2;

                param.Alpha[0] = param.Alpha[0] + DAlpha1;
                param.Alpha[1] = param.Alpha[1] + DAlpha1;

                param.DTime = param.DTime + DTay;
                modelTime = modelTime + DModelTime;
                expCount = expCount + DExpCount;
                return (param, modelTime, expCount);
            }

        }
        private void EqualWindowOpenButton_Click(object sender, RoutedEventArgs e)
        {
            Equal eq = new Equal();
            eq.Show();
        }

        private void exp1Button_Click(object sender, RoutedEventArgs e)
        {
            Exp1 exp1 = new Exp1();
            exp1.Show();
        }

    }
}
