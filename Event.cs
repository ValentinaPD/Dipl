using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    internal class Event
    {
        private double time;
        private int state;
        private bool isAdditional;
        private bool visible;

        public double Time  //Время настпуления собития
        {
            get { return this.time; }
            set { this.time = value; }
        }
        public int State //Состояние 1-S1, 2-S2
        {
            get { return this.state; }
            set { this.state = value; }
        }
        public bool IsAdditional //Является ли событие дополнительным false-нет, true-да
        {
            get { return this.isAdditional; }
            set { this.isAdditional = value; }
        }
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }

        public Event(double time, int state, bool isAdditional, bool visible)
        {
            this.time = time;
            this.state = state;
            this.isAdditional = isAdditional;
            this.visible = visible;
        }

        public (int state, double time, bool visible, bool isAddition) GetCharectetistics()
        {
            return (State, Time, Visible, IsAdditional);
        }

        public void Print()
        {
            Console.Write("Время наступления - {0}; состояние - {1}; дополнительное - ", Time, State);
            if (this.IsAdditional)
                Console.Write(" да, ");
            else
                Console.Write(" нет, ");
            Console.Write("наблюдаемость - ");
            if (this.visible)
                Console.WriteLine(" да");
            else
                Console.WriteLine(" нет");

            Console.WriteLine("-------------------------------------------------------------------------------------------");

        }
    }
}
