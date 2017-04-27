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
        private List<TrafficObject> trafficobjs; // liste mit Verkehrsobjekten
        private EnvironmentHandler eb; // ref auf Environmenthandler zum abfragen der rules

        public TrafficHandler(ref EnvironmentHandler _eb)
        {
            trafficobjs = new List<TrafficObject>();
            eb = _eb;
        }

        public void updateAll()
        {
            foreach(TrafficObject obj in trafficobjs)
            {
                obj.Update();
            }
        }

        public void getEnvRules(int x, int y)
        {
            Console.WriteLine(eb.getNeededEnvironmentRules(x, y));
        }

        public void calcNeighbours()
        {

        }


    }
}
