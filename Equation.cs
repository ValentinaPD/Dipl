using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая
{
    internal class Equation
    {
        public double GetSolution(double a1, double a2, double l1, double l2, double p, double q, double C)
        {
            int n = 0;
            List<double> x = new List<double>();
            x.Add(0.5);
            x.Add(GetFunc(a1, a2, l1, l2, p, q, x[0], C));
            x.Add(GetFunc(a1, a2, l1, l2, p, q, x[1], C));
            n = 1;
            double res = 0;
            while (Math.Abs(x[n + 1] - x[n]) > 0.001)
            {
                n = n + 1;
                //  res = x[n] - (((x[n] - x[n - 1]) * GetFunc(a1, a2, l1, l2, p, q, x[n], C)) / (GetFunc(a1, a2, l1, l2, p, q, x[n], C) - GetFunc(a1, a2, l1, l2, p, q, x[n - 1], C)));
                //x.Add(Math.Abs((x[n-1]*GetFunc(a1, a2, l1, l2, p, q, x[n], C) - x[n]* GetFunc(a1, a2, l1, l2, p, q, x[n-1], C))/(GetFunc(a1, a2, l1, l2, p, q, x[n], C) - x[n]- GetFunc(a1, a2, l1, l2, p, q, x[n-1], C) + x[n-1])));
                // if(res > 0)
                //   x.Add(res);
                // else
                //    x.Add(0);
                //x.Add(Math.Abs(GetFunc(a1, a2, l1, l2, p, q, x[n],C)));
                res = x[n] - (((x[n] - x[0]) * GetFunc(a1, a2, l1, l2, p, q, x[n], C)) / (GetFunc(a1, a2, l1, l2, p, q, x[n], C) - GetFunc(a1, a2, l1, l2, p, q, x[0], C)));
                x.Add(res);
                               

            }
            return x[n+1];
        }
        private double GetFunc(double a1,double a2,double l1,double l2,double p,double q,double t,double C)
        {
            double z1 = 0.5 * (l1 + a1 + l2 + a2 - Math.Sqrt(Math.Pow(l1 + a1 - l2 - a2, 2) + 4 * (1 - q) * a2 * (1 - p) * a1));
            double z2 = 0.5 * (l1 + a1 + l2 + a2 + Math.Sqrt(Math.Pow(l1 + a1 - l2 - a2, 2) + 4 * (1 - q) * a2 * (1 - p) * a1));
            double gamma1 = (z2 - l1 - l2) / (z2 - z1);
            double gamma2 = -(z1 - l1 - l2) / (z2 - z1);
            double fi_0 = gamma1 * Math.Exp(-z1 * t) + gamma2 * Math.Exp(-z2 * t);
            double M_ksi = (1 / fi_0) * (1 / (z1 * z2)) * (gamma1 * z2 + gamma2 * z1 - gamma1 * z2 * Math.Exp(-z1 * t) - gamma2 * z1 * Math.Exp(-z2 * t));
            double A = (1 / (a1 + a2)) * (1 / Math.Pow(z1 * z2, 2)) * (l1 - l2 + q * a1 - p * a2) * (l1 + p * a1 - l2 - a2 * q) * a1 * a2;
            double chisl = gamma1 * Math.Exp(-t * (a1 + a2 + z1)) + gamma2 * Math.Exp(-t * (a1 + a2 + z2));
            double zn = 1 - ((1 - Math.Exp(-t * (a1 + a2 + z1)) * z1 * gamma1) / (a1 + a2 + z1)) - ((1 - Math.Exp(-t * (a1 + a2 + z2)) * z2 * gamma2) / (a1 + a2 + z2));
            double func = ((1 / (z1 * z2)) * (z1 + z2 - (z1 * z2) / (a1 + a2)) + M_ksi - A * chisl / zn)-C;

            return func;
            
        }
        public double GetSolution(double a1, double a2, double l1, double l2, double p, double q, double C, double tay_min)
        {
            int n = 0;
            List<double> x = new List<double>();
            x.Add(0);
            x.Add(tay_min);
            n = 0;
            double res = 0;
            while (Math.Abs(x[n + 1] - x[n]) > 0.001)
            {
                n = n + 1;
                res = x[n] - (((x[n] - x[n - 1]) * GetFunc(a1, a2, l1, l2, p, q, x[n], C)) / (GetFunc(a1, a2, l1, l2, p, q, x[n], C) - GetFunc(a1, a2, l1, l2, p, q, x[n - 1], C)));
                //x.Add(Math.Abs((x[n-1]*GetFunc(a1, a2, l1, l2, p, q, x[n], C) - x[n]* GetFunc(a1, a2, l1, l2, p, q, x[n-1], C))/(GetFunc(a1, a2, l1, l2, p, q, x[n], C) - x[n]- GetFunc(a1, a2, l1, l2, p, q, x[n-1], C) + x[n-1])));
                // if(res > 0)
                //   x.Add(res);
                // else
                //    x.Add(0);
                //x.Add(Math.Abs(GetFunc(a1, a2, l1, l2, p, q, x[n],C)));
                //res = x[n] - (((x[n] - x[0]) * GetFunc(a1, a2, l1, l2, p, q, x[n], C)) / (GetFunc(a1, a2, l1, l2, p, q, x[n], C) - GetFunc(a1, a2, l1, l2, p, q, x[0], C)));


                //else
                //{
                //     x.Add(0);
                //  }
                x.Add(res);

            }
            double ret_res = 0;
            if (double.IsNaN(res))
            {
                ret_res = 0;
            }
            if (res >= tay_min)
            {
                ret_res = tay_min;
            }
            else if (res < tay_min)
            {
                ret_res = res;
            }
            return ret_res;
        }
    }
}
