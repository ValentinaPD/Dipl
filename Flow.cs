using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;
using System.Windows;

namespace Курсовая
{
    internal class Flow
    {
        private Parametrs parametrs;
        private List<Event> events;
        private List<(double begin, double end)> deadTimeInformation;
        private List<(int state, double duration)> statesInformation;
        private int eventsCount;
        private int additionalEventsCount;
        private int visibleEventsCount;
        private int inVisibleEventsCount;
        private double averageTimeInFlow;
        private double averageTimeInFinalFlow;
        private double averageTimeInDeadTime;
   

        public Parametrs Parametrs
        {
            get { return parametrs; }
            set { parametrs = value; }
        }
        public List<Event> Events
        {
            get { return events; }
        }
        public List<(double begin, double end)> DeadTimeInformation
        {
            get { return deadTimeInformation; }
        }
        public List<(int state, double duration)> StatesInformation
        {
            get { return statesInformation; }
        }

        public Flow(Parametrs parametrs)
        {
            this.parametrs = parametrs;
            events = new List<Event>();
            deadTimeInformation = new List<(double begin, double end)>();
            statesInformation = new List<(int state, double duration)>();
        }

        public void Generate(double Time)
        {
           Events.Clear();
           DeadTimeInformation.Clear();
           StatesInformation.Clear();
            System.Random rng = SystemRandomSource.Default;

            int state = 0;
            double timeInState = 0; // Длительность пребывания в состоянии (общее)
            double currentTime = 0; // Текущее время (общее)
            double endDeadTime = 0; // Конец мертвого времени(в состоянии)
            double t = 0; // Текущее время в состоянии
            double X = 0;
            double probability = 0;

            X = rng.NextDouble();
            state = X > 0.5 ? 1 : 2;


            while (currentTime < Time)
            {
                X = rng.NextDouble();
                timeInState = (-1 / parametrs.Alpha[state - 1]) * Math.Log(1 - X, Math.E);

                if (currentTime + timeInState > Time)
                    timeInState = Time - currentTime;
                statesInformation.Add((state, timeInState));
                t = 0;

                if (currentTime != 0)
                {
                    X = rng.NextDouble();
                    probability = state == 1 ? this.parametrs.Q : this.parametrs.P;

                    if (X < probability)
                    {
                        if (endDeadTime == 0)
                            AddEvent(t + currentTime, state, true, true);
                        else
                            AddEvent(t + currentTime, state, true, false);

                        endDeadTime = t + this.parametrs.DTime;
                        deadTimeInformation.Add((t + currentTime, endDeadTime + currentTime));


                    }

                }

                while (t <= timeInState)
                {
                    X = rng.NextDouble();

                    t = t + (-1 / this.parametrs.Lamda[state - 1]) * Math.Log(1 - X, Math.E);

                    if (t > timeInState) break;

                    if (t >= endDeadTime)
                        AddEvent(t + currentTime, state, false, true);
                    else
                        AddEvent(t + currentTime, state, false, false);

                    endDeadTime = t + this.parametrs.DTime;
                    deadTimeInformation.Add((t + currentTime, endDeadTime + currentTime));

                }
                endDeadTime = endDeadTime > timeInState ? endDeadTime - timeInState : 0;
                currentTime = currentTime + timeInState;

                state = state == 1 ? 2 : 1;
            }
            EventCount();
        }

        private void AddEvent(double time, int state, bool isAdditionalal, bool visible)
        {
            Event e = new Event(Math.Round(time, 3), state, isAdditionalal, visible);
            events.Add(e);
        }
        public void Print()
        {
            foreach (Event e in this.events)
            {
                e.Print();
            }
        }
        public void PrintInformation()
        {
            Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine("States");
            Console.WriteLine("===========================================================");
            Console.WriteLine();
            foreach ((int, double) e in statesInformation)
            {
                Console.WriteLine("Состояние - {0}, длительность - {1}", e.Item1, Math.Round(e.Item2, 3));
                Console.WriteLine("--------------------------------------");
            }
            Console.WriteLine();
            Console.WriteLine("===========================================================");
            Console.WriteLine("Dead Time");
            Console.WriteLine("===========================================================");
            Console.WriteLine();
            foreach ((double, double) e in deadTimeInformation)
            {
                Console.WriteLine("Начало - {0}, конец {1}", Math.Round(e.Item1, 3), Math.Round(e.Item2, 3));
                Console.WriteLine("--------------------------------------");
            }
            foreach (Event e in this.events)
            {
                e.Print();
            }
        }
        public List<Event> GetFlowEvents()
        {
            return events;
        }
        public List<(int state, double time, bool visible, bool isAddition)> GetEventList()
        {
            List<(int state, double time, bool visible, bool isAddition)> eventList = new List<(int state, double time, bool visible, bool isAddition)>();
            foreach (Event e in events)
            {
                eventList.Add(e.GetCharectetistics());
            }
            return eventList;
        }

        public int GetEventsCount()
        {
            return eventsCount;
        }
        public int GetVisibleEventsCount()
        {
            return visibleEventsCount;
        }
        public int GetInVisibleEventsCount()
        {
            return inVisibleEventsCount;
        }
        public int GetAdditionalEventsCount()
        {
            return additionalEventsCount;
        }
        public List<double> GetDurationOfIntervals()
        {
            List<double> duration = new List<double>();
            List<double> visibleEvents = new List<double>();
            foreach(Event e in events)
            {
                if (e.Visible)
                    visibleEvents.Add(e.Time);
            }
            for(int i = 0; i < visibleEvents.Count-1; i++)
            {
                duration.Add(visibleEvents[i + 1] - visibleEvents[i]);
            }
            return duration;
        }
        public double GetAverageTimeInFlow()
        {
            int count = 0;
            double sum = 0;
            for(int i = 1; i < events.Count; i++)
            {
                count++;
                sum += (events[i].Time - events[i - 1].Time);
            }
           // MessageBox.Show((sum / count).ToString());
            return sum/count;
        }
        public double GetAverageTimeInFinalFlow()
        {
            
            List<double> visibleEvents = new List<double>();
            foreach(Event e in events)
            {
                if (e.Visible)
                    visibleEvents.Add(e.Time);
            }
            int count = 0;
            double sum = 0;
            //MessageBox.Show(visibleEvents.Count.ToString());
          //  for (int i = 0; i < visibleEvents.Count; i++)
          //      MessageBox.Show(visibleEvents[i].ToString());
            for (int i = 1; i < visibleEvents.Count; i++)
            {
                
                    count++;
                    sum += (visibleEvents[i]- visibleEvents[i - 1]);
                    //MessageBox.Show((visibleEvents[i] - visibleEvents[i - 1]).ToString()+"  "+ i.ToString()+"  "+(i-1).ToString());
                
                
            }
            //MessageBox.Show((sum / count).ToString());
            return sum / count;
        }
        public double GetMinDuration()
        {

            List<double> visibleEvents = new List<double>();
            List<double> duration = new List<double>();
            foreach (Event e in events)
            {
                if (e.Visible)
                    visibleEvents.Add(e.Time);
            }
            
            for (int i = 1; i < visibleEvents.Count; i++)
            {
                duration.Add(visibleEvents[i] - visibleEvents[i - 1]);
               
            }
            return duration.Min();
        }

        public double GetAverageDeadTime()
        {
            double SUM = 0;
            int COUNT = 0;

            int count = 0;
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
                    SUM += deadTimeInformation[i - 1].end - deadTimeInformation[i - count - 1].begin;
                    COUNT++;
                    //arrows.Add((deadTimeInformation[i - count - 1].begin, coordinatsY.deadTimeY, deadTimeInformation[i - 1].end, coordinatsY.deadTimeY));
                    count = 0;
                }
                if (i == deadTimeInformation.Count - 1)
                {
                    if (deadTimeInformation[i].end < GetT())
                    {
                        SUM += deadTimeInformation[i].end - deadTimeInformation[i - count].begin;
                        COUNT++;
                    }
                        //arrows.Add((deadTimeInformation[i - count].begin, coordinatsY.deadTimeY, deadTimeInformation[i].end, coordinatsY.deadTimeY));
                    else
                    {
                        SUM += GetT() - deadTimeInformation[i - count].begin;
                        COUNT++;
                    }
                        //arrows.Add((deadTimeInformation[i - count].begin, coordinatsY.deadTimeY, T, coordinatsY.deadTimeY));
                }


            }
           
            return SUM/COUNT;
        }
        private void EventCount()
        {
            int visible = 0;
            int inVisible = 0;
            int additional = 0;
            foreach (Event e in events)
            {
                if (e.Visible) 
                    visible++;
                if (!e.Visible)
                    inVisible++;
                if (e.IsAdditional)
                    additional++;

            }
            eventsCount = events.Count;
            visibleEventsCount = visible;
            inVisibleEventsCount = inVisible;
            additionalEventsCount = additional;
            
        }
        private double GetT()
        {
            double t = 0;
            foreach(var s in StatesInformation)
            {
                t += s.duration;
            }
            return t;
        }

    }
}
