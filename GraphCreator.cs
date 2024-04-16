using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    internal class GraphCreator
    {
        private ScottPlot.Plot chart;
        public GraphCreator(ScottPlot.Plot _plot) {
            chart = _plot;
            
        }
        public void DrawOpenPoints(List<double> xs, List<double> ys)
        {
            if (xs.Count == 0) return;
            var points = chart.AddScatter(xs.ToArray(), ys.ToArray());
            points.LineStyle = ScottPlot.LineStyle.None;
            points.MarkerSize = 10;
            points.MarkerShape = MarkerShape.openCircle;
            points.Color = Color.Teal;
        }
        public void DrawFullPoints(List<double> xs, List<double> ys)
        {
            if (xs.Count == 0) return;
            var points = chart.AddScatter(xs.ToArray(), ys.ToArray());
            points.LineStyle = ScottPlot.LineStyle.None;
            points.MarkerSize = 10;
            points.MarkerShape = MarkerShape.filledCircle;
            points.Color = Color.Teal;
        }
        public void DrawDashLines(List<double> _lines)
        {
            if (_lines.Count == 0) return;
            foreach (var line in _lines)
            {
                var l = chart.AddVerticalLine(line);
                l.Color = Color.Silver;
                l.LineWidth = 0.5;
                l.LineStyle = LineStyle.DashDotDot;
            }
        }
        public void DrawArrows(List<(double x1, double y1, double x2, double y2)> arrowsList)
        {
            if (arrowsList.Count == 0) return;
            foreach (var arrow in arrowsList)
            {
                var arr = chart.AddArrow(arrow.x2, arrow.y2, arrow.x1, arrow.y1);
                arr.Color = Color.Teal; 
                arr.LineWidth = 3;
            }
        }
        public void DrawArrowsWithoutHead(List<(double x1, double y1, double x2, double y2)> arrowsList)
        {
            if (arrowsList.Count == 0) return;
            foreach (var arrow in arrowsList)
            {
                var arr = chart.AddArrow(arrow.x2, arrow.y2, arrow.x1, arrow.y1);
                arr.Color = Color.Teal;
                arr.LineWidth = 3;
                arr.ArrowheadLength = 0; arr.ArrowheadLength = 0;
            }
        }
        public void DrawRedFullPoints(List<double> xs, List<double> ys)
        {
            if (xs.Count == 0) return;
            var points = chart.AddScatter(xs.ToArray(), ys.ToArray());
            points.LineStyle = ScottPlot.LineStyle.None;
            points.MarkerSize = 10;
            points.MarkerShape = MarkerShape.filledCircle;
            points.Color = Color.Red;
        }
        public void DrawRedOpenPoints(List<double> xs, List<double> ys)
        {
            if(xs.Count == 0) return;
            var points = chart.AddScatter(xs.ToArray(), ys.ToArray());
            points.LineStyle = ScottPlot.LineStyle.None;
            points.MarkerSize = 10;
            points.MarkerShape = MarkerShape.openCircle;
            points.Color = Color.Red;
        }
        public void SetLimits(double y)
        {
            chart.SetAxisLimitsY(-0.5, y+0.5);
            chart.SetAxisLimitsX(0, 10);
        }
    }
}
