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
                //obj.Update();
                int thisRoadInfo;
                int nextRoadInfo;
              
                thisRoadInfo= getEnvRules(obj.X, obj.Y);
                
                switch(thisRoadInfo)
                {
                    case (int)EnvElement.StreetType.Street:
                        throw new NotImplementedException();
                        break;
                    //TODO get more detailed information where I can go.
                    //TODO get information if traffic light or not.
                    case (int)EnvElement.StreetType.ThreeKreuzung:
                        throw new NotImplementedException();
                        break;
                    //TODO get information if traffic light or not.
                    case (int)EnvElement.StreetType.FourKreuzung:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();

                }
              
                switch (obj.Direction)
                {
                    //TODO: check if leaving road grid
                    case (int)TrafficObject.Dir.Up:
                        nextRoadInfo = getEnvRules(obj.X - 1, obj.Y);
                        break;
                    case (int)TrafficObject.Dir.Down:
                        nextRoadInfo = getEnvRules(obj.X + 1, obj.Y);
                        break;
                    case (int)TrafficObject.Dir.Left:
                        nextRoadInfo = getEnvRules(obj.X, obj.Y - 1);
                        break;
                    case (int)TrafficObject.Dir.Right:
                        nextRoadInfo = getEnvRules(obj.X, obj.Y + 1);
                        break;
                    default:
                        throw new NotImplementedException();
                }


            }
        }

        public int getEnvRules(int x, int y)
        {
            //Console.WriteLine(eb.getNeededEnvironmentRules(x, y));
            return eb.getNeededEnvironmentRules(x, y);
        }

        public void calcNeighbours()
        {

        }


    }
}
