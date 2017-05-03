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
        Random rng = new Random(); //random number generator

        public TrafficHandler(ref EnvironmentHandler _eb)
        {
            trafficobjs = new List<TrafficObject>();
            eb = _eb;
        }

        public void updateAll()
        {
            foreach(TrafficObject obj in trafficobjs)  //check how can move forward and flag them if they can do so
            {
                int thisRoadInfo; 
                int nextRoadX;
                int nextRoadY;
            
                thisRoadInfo= getEnvRules(obj.X, obj.Y);
                Tuple<int,int> nextRoadTileXY = getNextRoadTileXY(obj);
                nextRoadX = nextRoadTileXY.Item1;
                nextRoadY = nextRoadTileXY.Item2;

                switch (thisRoadInfo)
                {
                    case (int)EnvElement.StreetType.Street:
                        if(checkIfTileIsEmpty(nextRoadX, nextRoadY, obj.NextDirection))
                        {
                            //may drive
                            obj.MayDrive = true;
                        }
                        break;
                    //TODO get more detailed information where I can go.
                    //TODO get information if traffic light or not.
                    case (int)EnvElement.StreetType.ThreeKreuzung:
                        //TODO get Ausrichtung Kreuzung
                        int layout = 1;
                        if (checkIfCanDrive3WayWithoutTrafficLight(layout, obj) && checkIfTileIsEmpty(nextRoadX, nextRoadY, obj.NextDirection))
                        {

                            //may drive
                            obj.MayDrive = true;
                        }
                        break;
                    case (int)EnvElement.StreetType.FourKreuzung:
                        if (checkIfCanDrive4WayWithoutTrafficLight(obj) && checkIfTileIsEmpty(nextRoadX, nextRoadY, obj.NextDirection))
                        {
                            //may drive
                            obj.MayDrive = true;
                        }
                        
                        break;
                    case 0: //traffic Light
                        if (checkIfCanDriveWithTrafficLight(obj) && checkIfTileIsEmpty(nextRoadX, nextRoadY, obj.NextDirection))
                        {
                            //may drive
                            obj.MayDrive = true;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            foreach (TrafficObject obj in trafficobjs) //update cars
            {
                if (obj.MayDrive)
                { 
                    Tuple<int, int> nextRoadXY = getNextRoadTileXY(obj);
                    obj.X = nextRoadXY.Item1;
                    obj.Y = nextRoadXY.Item2;//move the car to its new position

                    int nextRoadInfo = getEnvRules(obj.X, obj.Y); //decide where to go next
                    switch (nextRoadInfo)
                    {
                        case (int)EnvElement.StreetType.Street:
                            //no direction change posible
                            break;
                        case (int)EnvElement.StreetType.ThreeKreuzung:
                            //TODO get layout from Straßennetz
                            int layout = 1;
                            int rotation_modifier = 0;
                            //TODO find a way to resolve Deadlocks
                            switch (layout)
                            {
                                case 1:
                                    //    ||
                                    // ===
                                    //    ||
                                    rotation_modifier = 0;
                                    break;
                                case 2:
                                    // ===  ===
                                    //    ||
                                    rotation_modifier = 1;
                                    break;
                                case 3:
                                    // ||
                                    //   ===
                                    // ||
                                    rotation_modifier = 2;
                                    break;
                                case 4:
                                    //    ||
                                    // ===  ===
                                    rotation_modifier = 3;
                                    break;
                                default:
                                    rotation_modifier = 0;
                                    break;
                            }
                            int rn = rng.Next(0, 2);
                            switch ((obj.Direction + rotation_modifier) % 4)
                            {
                                
                                case (int)TrafficObject.Dir.Up:
                                    if(rn == 0)
                                    {
                                        obj.NextDirection = ((int)TrafficObject.Dir.Up + rotation_modifier) % 4;
                                    }
                                    else
                                    {
                                        obj.NextDirection = ((int)TrafficObject.Dir.Left + rotation_modifier) % 4;
                                    }
                                    break;
                                case (int)TrafficObject.Dir.Down:
                                    if (rn == 0)
                                    {
                                        obj.NextDirection = ((int)TrafficObject.Dir.Down + rotation_modifier) % 4;
                                    }
                                    else
                                    {
                                        obj.NextDirection = ((int)TrafficObject.Dir.Left + rotation_modifier) % 4;
                                    }
                                    break;
                                case (int)TrafficObject.Dir.Right:
                                    if (rn == 0)
                                    {
                                        obj.NextDirection = ((int)TrafficObject.Dir.Down + rotation_modifier) % 4;
                                    }
                                    else
                                    {
                                        obj.NextDirection = ((int)TrafficObject.Dir.Up + rotation_modifier) % 4;
                                    }
                                    break;
                                default:
                                    break;
                            }

                
                            break;
                        case (int)EnvElement.StreetType.FourKreuzung:
                            int rngMinus1toPlus1 = rng.Next(1, 4) - 2; // -1 <= rng <= 1
                            obj.Direction = (obj.Direction + rngMinus1toPlus1) % 4;
                            break;
                    }

                    obj.MayDrive = false;

                    //TODO send update to UI
                    throw new NotImplementedException();
                }
            }
        }

        public int getEnvRules(int x, int y)
        {
            return eb.getNeededEnvironmentRules(x, y);
        }

        public void createNewVerkehrsteilnehmer(int id, int x, int y, int speed, int direction, int nextDirection)
        {
      
            trafficobjs.Add(new TrafficObject(id,x, y, speed, direction, nextDirection));
        }

        public void removeVerkehrteilnehmer(int id)
        {
            TrafficObject removeObject = null;
            foreach (TrafficObject obj in trafficobjs)
            {
                if (obj.Id.Equals(id))
                {
                    removeObject = obj;
                }
            }
            if (removeObject != null)
            {
                trafficobjs.Remove(removeObject);
            }
        }

        private Boolean checkIfTileIsEmpty(int x, int y,int direction)
        {
            foreach (TrafficObject obj in trafficobjs)
            {
                if (obj.X == x && obj.Y == y && obj.Direction == direction)
                {
                    return false;
                }
            }
            return true;
        }

        private Boolean checkIfCanDrive3WayWithoutTrafficLight(int layout, TrafficObject obj)
        {
            int rotation_modifier=0;
            //TODO find a way to resolve Deadlocks
            switch (layout)
            {
                case 1:
                    //    ||
                    // ===
                    //    ||
                    rotation_modifier = 0;
                    break;
                case 2:
                    // ===  ===
                    //    ||
                    rotation_modifier = 1;
                    break;
                case 3:
                    // ||
                    //   ===
                    // ||
                    rotation_modifier = 2;
                    break;
                case 4:
                    //    ||
                    // ===  ===
                    rotation_modifier = 3;
                    break;
                default:
                    rotation_modifier = 0;
                    break;
            }

            switch ((obj.Direction + rotation_modifier) % 4)
            {
                case (int)TrafficObject.Dir.Up:
                    switch ((obj.NextDirection + rotation_modifier) % 4)
                    {
                        case (int)TrafficObject.Dir.Up:
                            //may always drive
                            return true;
                        case (int)TrafficObject.Dir.Left:
                            //may only drive if there is noone coming from front
                            return checkIfTileIsEmpty(obj.X, obj.Y, (int) TrafficObject.Dir.Down);
                        default:
                            //car wants to go where no road is, should never happen.
                            return false;
                    }
                case (int)TrafficObject.Dir.Right:
                    switch ((obj.NextDirection + rotation_modifier) % 4)
                    {
                        case (int)TrafficObject.Dir.Down:
                            //may always drive
                            return true;
                        case (int)TrafficObject.Dir.Up:
                            return checkIfTileIsEmpty(obj.X, obj.Y, (int) TrafficObject.Dir.Up);
                        default:
                            //car wants to go where no road is, should never happen.
                            return false;
                    }
                case (int)TrafficObject.Dir.Down:
                    switch ((obj.NextDirection + rotation_modifier) % 4)
                    {
                        case (int)TrafficObject.Dir.Left:
                            //may always drive
                            return true;
                        case (int)TrafficObject.Dir.Down:
                            return checkIfTileIsEmpty(obj.X, obj.Y, (int)TrafficObject.Dir.Right);
                        default:
                            return false;
                    }
                default:
                    return false;
            }
                
        }

        private Boolean checkIfCanDrive4WayWithoutTrafficLight (TrafficObject obj)
        {
            switch ((obj.NextDirection - obj.Direction) % 4) //calculates the Abbiegerichtung: 1= nach rechts, 2= nach geradeaus, 3 =nach links
            {
                case 1: //rechts abbiegen
                    return true;
                case 2: //geradeaus
                    return checkIfTileIsEmpty(obj.X, obj.Y, (obj.Direction + 1) % 4); //schau ob rechts nichts kommt
                case 3://links abbiegen
                    return checkIfTileIsEmpty(obj.X, obj.Y, (obj.Direction + 1) % 4) && checkIfTileIsEmpty(obj.X, obj.Y, (obj.Direction + 2) % 4);//schau ob rechts und vorne nichts kommt
                default:
                    return false;
            }
        }

        private Boolean checkIfCanDriveWithTrafficLight(TrafficObject obj)
        {
            //TODO get trafficlightstatus from Verkehrsnetz
            Boolean isGreen = true;
            if (!isGreen)
            {
                return false;
            }
            else
            {
                if ((obj.NextDirection - obj.Direction) % 4 == 3) //links abiegen{
                {
                   return checkIfTileIsEmpty(obj.X, obj.Y, (obj.Direction + 2) % 4);
                }
                else
                {
                    return true;
                }
            }
        }

        private Tuple<int,int> getNextRoadTileXY(TrafficObject obj)
        {
            switch (obj.Direction)
            {
                case (int)TrafficObject.Dir.Up:
                    return new Tuple<int, int>(obj.X - 1, obj.Y);
                case (int)TrafficObject.Dir.Down:
                    return new Tuple<int, int>(obj.X + 1, obj.Y);
                case (int)TrafficObject.Dir.Left:
                    return new Tuple<int, int>(obj.X, obj.Y - 1);
                case (int)TrafficObject.Dir.Right:
                    return new Tuple<int, int>(obj.X, obj.Y + 1);
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
