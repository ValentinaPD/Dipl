using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    internal class Parametrs
    {
        private double p;
        private double q;
        private double[] lamda;
        private double[] alpha;
        private double dTime;

        public double P
        {
            get { return p; }
            set { p = value; }
        }
        public double Q
        {
            get { return q; }
            set { q = value; }
        }
        public double DTime
        {
            get { return dTime; }
            set { dTime = value; }
        }

        public double[] Alpha
        {
            get { return alpha; }

            set { alpha = value; }
        }
        public double[] Lamda
        {
            get { return lamda; }
            set { lamda = value; }
        }
       
        public Parametrs(double p, double q, double lamda1, double lamda2, double alpha1, double alpha2, double dTime)
        {
            this.lamda = new double[2];
            this.alpha = new double[2];
            this.p = p;
            this.q = q;
            this.lamda[0] = lamda1;
            this.lamda[1] = lamda2;
            this.alpha[0] = alpha1;
            this.alpha[1] = alpha2;
            this.dTime = dTime;

        }
    }
}
