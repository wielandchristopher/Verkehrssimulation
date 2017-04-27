using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkehrssimulation.Verkehrsnetz;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    class TrafficHandler
    {
        private List<TrafficObject> trafficobjs;
        private EnvironmentHandler eb;

        public TrafficHandler(ref EnvironmentHandler _eb)
        {
            trafficobjs = new List<TrafficObject>();
            eb = _eb;
            writeneededEnvrules();
        }

        public void updateAll()
        {
            foreach(TrafficObject obj in trafficobjs)
            {
                obj.Update();
            }
        }

        public void writeneededEnvrules()
        {
            Console.WriteLine(eb.getNeededEnvironmentRules(4, 5));
        }

        public void calcNeighbours()
        {

        }


    }
}
