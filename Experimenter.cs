using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Курсовая
{
    internal interface IExperiment
    {
        void Algorithm(Parametrs par, double expCount,double modelTime, double changeNum, int pointsCount);
    }

    internal class ConcreteStrategy1 : IExperiment
    {
        public void Algorithm(Parametrs par,double expCount, double modelTime, double changeNum, int pointsCount)
        {
            List<double> averageFlow = new List<double>();
            List<double> averageFinalFlow = new List<double>();
            List<double> averrageDeadTime = new List<double>();
            List<double> Y = new List<double>();

            for(double i = 0; i<=pointsCount;i++)
            {
                for(double j = 0; j < expCount + i*changeNum; i++)
                {
                    Parametrs parametrs = par;
                    Flow flow = new Flow(parametrs);
                    flow.Generate(modelTime);
                }
                
            }


            
            

        }
    }

 /*   public class ConcreteStrategy2 : IExperiment
    {
        public void Algorithm(double p, double q, double lamda1, double lamda2, double alpha1, double alpha2, double tay,
                       double expCount, double modelTime, double changeNum, int pointsCount)
        { 

        }
    }
*/
    internal class Context
    {
        public IExperiment ContextStrategy { get; set; }

        public Context(IExperiment _strategy)
        {
            ContextStrategy = _strategy;
        }

        public void ExecuteAlgorithm(Parametrs param,double expCount, double modelTime, double changeNum, int pointsCount)
        {
            ContextStrategy.Algorithm(param,expCount,modelTime,changeNum,pointsCount);
        }
    }
}
