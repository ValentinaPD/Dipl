using ScottPlot.Plottable;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Xml.Schema;
using System.Runtime.CompilerServices;

namespace Курсовая
{
    internal class DataParser
    {
        private List<(int state, double time, bool visible, bool isAddition)> eventList;
        private List<(double begin, double end)> deadTimeInformation;
        private List<(int state, double duration)> statesInformation;
        private (double state1Y, double state2Y,double deadTimeY,double finalY) coordinatsY;
        private double T;
       
      
        public DataParser(List<(int state, double time, bool visible, bool isAddition)> _eventList, List<(double begin, double end)> _deadTimeInformation, List<(int state, double duration)> _statesInformation)
        {
            eventList = _eventList;
            deadTimeInformation = _deadTimeInformation;
            statesInformation = _statesInformation;
            coordinatsY = EqualYCoordinats();
            T = SetT();
        }

        public (List<double> xs, List<double> ys) GetOpenPoints()
        {
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();
            foreach (var e in eventList)
            {
                if (e.visible && !e.isAddition)
                {
                    xs.Add(e.time);
                    ys.Add(coordinatsY.finalY);
                    if (e.state == 1)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state1Y);
                    }
                    if (e.state == 2)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state2Y);
                    }
                }
                
            }

            return (xs, ys);            

        }
        public (List<double> xs, List<double> ys) GetFullPoints()
        {
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();
            foreach (var e in eventList)
            {
                if (!e.visible && !e.isAddition)
                {
                
                    if (e.state == 1)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state1Y);
                    }
                    if (e.state == 2)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state2Y);
                    }
                }

            }

            return (xs, ys);

        }

        public (List<double> xs, List<double> ys) GetAdditionsalPoints()
        {
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();
            foreach (var e in eventList)
            {
                if (e.visible && e.isAddition)
                {

                    if (e.state == 1)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state1Y);
                    }
                    if (e.state == 2)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state2Y);
                    }
                }

            }

            return (xs, ys);

        }
        public (List<double> xs, List<double> ys) GetFullAdditionsalPoints()
        {
            List<double> xs = new List<double>();
            List<double> ys = new List<double>();
            foreach (var e in eventList)
            {
                if (!e.visible && e.isAddition)
                {

                    if (e.state == 1)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state1Y);
                    }
                    if (e.state == 2)
                    {
                        xs.Add(e.time);
                        ys.Add(coordinatsY.state2Y);
                    }
                }

            }

            return (xs, ys);

        }
        public List<double> GetDashVerticalLines()
        {
            List<double > lines = new List<double>();
            foreach(var e in eventList)
            {
                lines.Add(e.time);
            }
            return lines;
        }
        public List<(double x1,double y1,double x2, double y2)> GetArrows()
        {
            List<(double x1, double y1, double x2, double y2)> arrows = new List<(double x1, double y1, double x2, double y2)> ();
            double time = 0;
            double prev_time = 0;
            double s = 0;
            double prev_s = 0;
            foreach(var state in statesInformation)
            {
                time = prev_time + state.duration;
                if (state.state == 1)
                {
                    arrows.Add((prev_time, coordinatsY.state1Y, time, coordinatsY.state1Y));
                    arrows.Add((time, coordinatsY.state1Y, time, coordinatsY.state2Y));
                }
                    
                if(state.state == 2)
                {
                    arrows.Add((prev_time, coordinatsY.state2Y, time, coordinatsY.state2Y));
                    arrows.Add((time, coordinatsY.state2Y, time, coordinatsY.state1Y));
                }
                    
                prev_time = time;

            }
            arrows.Add((0, coordinatsY.finalY, time, coordinatsY.finalY));
            return arrows;
        }

        public List<(double x1, double y1, double x2, double y2)> GetArrowsWithoutHead()
        {
            List<(double x1, double y1, double x2, double y2)> arrows = new List<(double x1, double y1, double x2, double y2)>();

            int count = 0;
            arrows.Add((deadTimeInformation[0].begin, coordinatsY.state2Y - 0.1, deadTimeInformation[0].end, coordinatsY.state2Y - 0.1));
            for (int i = 1; i < deadTimeInformation.Count; i++)
            {
                if (deadTimeInformation[i - 1].end > deadTimeInformation[i].begin)
                {
                    count++;
                }
                else
                {
                    count = 0;
                }
                if (deadTimeInformation[i].end < T)
                    arrows.Add((deadTimeInformation[i].begin, coordinatsY.state2Y - 0.1 - count * 0.03, deadTimeInformation[i].end, coordinatsY.state2Y - 0.1 - count * 0.03));
                else
                    arrows.Add((deadTimeInformation[i].begin, coordinatsY.state2Y - 0.1 - count * 0.03, T, coordinatsY.state2Y - 0.1 - count * 0.03));

            }
             count = 0;
            bool isChanged = false;
            for (int i = 1; i < deadTimeInformation.Count; i++)
            {
                if (deadTimeInformation[i - 1].end > deadTimeInformation[i].begin)
                {
                    count++;
                }
                if ((deadTimeInformation[i - 1].end <= deadTimeInformation[i].begin))
                {
                    isChanged = true;
                    
                           
                }
                if (isChanged)
                {
                    isChanged = false;
                    arrows.Add((deadTimeInformation[i - count-1].begin, coordinatsY.deadTimeY, deadTimeInformation[i-1].end, coordinatsY.deadTimeY));
                    count = 0;
                }
                if(i == deadTimeInformation.Count-1)
                {
                    if (deadTimeInformation[i].end<T)
                        arrows.Add((deadTimeInformation[i - count].begin, coordinatsY.deadTimeY, deadTimeInformation[i].end, coordinatsY.deadTimeY));
                    else
                        arrows.Add((deadTimeInformation[i - count].begin, coordinatsY.deadTimeY, T, coordinatsY.deadTimeY));
                }   
                

            }



            return arrows;
        }
        private (double state1Y, double state2Y, double deadTimeY, double finalY) EqualYCoordinats()
        {
            double state1Y = 2;
            double state2Y = 1;
            double deadTimeY = 0.2;
            double finalY = 0;
            int count = 0;
            int maxCount = 0;
            double prev_end = 0;
            foreach (var dt in deadTimeInformation)
            {
                if (dt.begin < prev_end)
                    count++;
                else
                {
                    
                        
                    count = 0;
                }
                if (count > maxCount)
                {
                    maxCount = count;
                }
                prev_end = dt.end;
                

            }
            state2Y = 0.4 + maxCount * 0.03;
            state1Y = state2Y + 0.3;
            deadTimeY = 0.2;
            finalY = 0;
            return (state1Y,state2Y,deadTimeY,finalY);
        }
        private double SetT()
        {
            double time = 0;
            foreach (var state in statesInformation)
                time = time + state.duration;
            return time;
           
        }
        public double GetS1Y()
        {
            return coordinatsY.state1Y;
        }
    }

}
