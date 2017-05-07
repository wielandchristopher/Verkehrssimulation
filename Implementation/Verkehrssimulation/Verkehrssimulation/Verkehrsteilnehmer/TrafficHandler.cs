using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkehrssimulation.Verkehrsnetz;
using Verkehrssimulation.GUI;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    class TrafficHandler
    {
        private List<TrafficObject> trafficobjs; // liste mit Verkehrsobjekten
        private EnvironmentBuilder eb; // ref auf Environmenthandler zum abfragen der rules
        private ObjectHandler oh; //ref zu GUI
        Random rng = new Random(); //random number generator
        int id_number;

        public enum StreetRegion { NormalStreet = 0, IntersectionAhead = 1, Intersection = 2};

        public TrafficHandler(ref EnvironmentBuilder _eb, ref ObjectHandler _oh)
        {
            trafficobjs = new List<TrafficObject>();
            oh = _oh;
            eb = _eb;
            id_number = 1;
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
                        //may drive if road ahead is empty
                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1); // only this car is around
                        break;
                        //TODO get more detailed information where I can go.
                        //TODO get information if traffic light or not.

                    case (int)EnvElement.StreetType.ThreeKreuzung:
                    case (int)EnvElement.StreetType.FourKreuzung:
                        int streetRegion = getStreetRegion(obj.X, obj.Y);
                        switch (streetRegion)
                        {
                            case (int) StreetRegion.NormalStreet:
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                break;
                            case (int)StreetRegion.IntersectionAhead:
                                obj.MayDrive = checkIfCanDrive4WayWithoutTrafficLight(obj) && (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                break;
                            //TODO find Solution for "zugestaute Kreuzungen"
                            case (int)StreetRegion.Intersection:
                                switch ((obj.NextDirection - obj.Direction +4) % 4)
                                {
                                    case 0: //contues to drive in same direction
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                        break;
                                    default: //biegt wo ab.
                                        switch (obj.NextDirection)
                                        {
                                            case (int) TrafficObject.Dir.Up:
                                                if ((obj.X % 100 <= 55 && nextRoadX%100 >= 55) || (obj.X % 100 >= 55 && nextRoadX % 100 <= 55))
                                                {
                                                    decimal d = nextRoadX / 100;
                                                    nextRoadX = ((int)Math.Floor(d) * 100) + 55;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int) TrafficObject.Dir.Down:
                                                if ((obj.X % 100 <= 45 && nextRoadX % 100 >= 45) || (obj.X % 100 >= 45 && nextRoadX % 100 <= 45))
                                                {
                                                    decimal d = nextRoadX / 100;
                                                    nextRoadX = ((int)Math.Floor(d) * 100) + 45;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Right:
                                                if ((obj.Y % 100 <= 55 && nextRoadY % 100 >= 55) || (obj.Y % 100 >= 55 && nextRoadY % 100 <= 55))
                                                {
                                                    decimal d = nextRoadY / 100;
                                                    nextRoadY = ((int)Math.Floor(d) * 100) + 55;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Left:
                                                if ((obj.Y % 100 <= 45 && nextRoadY % 100 >= 45) || (obj.Y % 100 >= 45 && nextRoadY % 100 <= 45))
                                                {
                                                    decimal d = nextRoadY / 100;
                                                    nextRoadY = ((int)Math.Floor(d) * 100) + 45;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                break;
                        }
                        break;
                    case 0: //traffic Light
                        //TODO reduce duplicate code with no traffic light
                        int streetRegion2 = getStreetRegion(obj.X, obj.Y);
                        switch (streetRegion2)
                        {
                            case (int)StreetRegion.NormalStreet:
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                break;
                            case (int)StreetRegion.IntersectionAhead:
                                obj.MayDrive = checkIfCanDriveWithTrafficLight(obj) && (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                break;
                            //TODO find Solution for "zugestaute Kreuzungen"
                            case (int)StreetRegion.Intersection:
                                switch ((obj.NextDirection - obj.Direction) % 4)
                                {
                                    case 0: //contues to drive in same direction
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                        break;
                                    default: //biegt wo ab.
                                        switch (obj.NextDirection)
                                        {
                                            case (int)TrafficObject.Dir.Up:
                                                if ((obj.X % 100 <= 55 && nextRoadX % 100 >= 55) || (obj.X % 100 >= 55 && nextRoadX % 100 <= 55))
                                                {
                                                    decimal d = nextRoadX / 100;
                                                    nextRoadX = ((int)Math.Floor(d) * 100) + 55;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Down:
                                                if ((obj.X % 100 <= 45 && nextRoadX % 100 >= 45) || (obj.X % 100 >= 45 && nextRoadX % 100 <= 45))
                                                {
                                                    decimal d = nextRoadX / 100;
                                                    nextRoadX = ((int)Math.Floor(d) * 100) + 45;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Right:
                                                if ((obj.Y % 100 <= 55 && nextRoadY % 100 >= 55) || (obj.Y % 100 >= 55 && nextRoadY % 100 <= 55))
                                                {
                                                    decimal d = nextRoadY / 100;
                                                    nextRoadY = ((int)Math.Floor(d) * 100) + 55;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Left:
                                                if ((obj.Y % 100 <= 45 && nextRoadY % 100 >= 45) || (obj.Y % 100 >= 45 && nextRoadY % 100 <= 45))
                                                {
                                                    decimal d = nextRoadY / 100;
                                                    nextRoadY = ((int)Math.Floor(d) * 100) + 45;
                                                    obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1);
                                break;
                        }
                        break;

                    case (int)EnvElement.StreetType.Grass:
                        //may drive if road ahead is empty
                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, nextRoadX, nextRoadY) <= 1); // only this car is around
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
                    int nextRoadX = nextRoadXY.Item1;
                    int nextRoadY = nextRoadXY.Item2;
                    //decide where to go next when leaving roadTile
                    if ((Math.Abs(obj.X % 100 - nextRoadX % 100) > (obj.Speed + 1)) || (Math.Abs(obj.Y % 100 - nextRoadY % 100) > (obj.Speed + 1)))
                    {
                        int nextRoadInfo = getEnvRules(nextRoadX, nextRoadY); 
                        switch (nextRoadInfo)
                        {
                            case (int)EnvElement.StreetType.Street:
                                //no direction change posible
                                break;
                            case (int)EnvElement.StreetType.ThreeKreuzung:
                                //TODO get layout from Straßennetz
                                int layout = 1;
                                int rotation_modifier = 0;
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
                                        if (rn == 0)
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
                                obj.NextDirection = (obj.NextDirection + rngMinus1toPlus1 + 4) % 4;
                                break;
                        }
                    }

                    obj.MayDrive = false;

                    obj.X = nextRoadXY.Item1;
                    obj.Y = nextRoadXY.Item2;//move the car to its new position
                    //send update to UI
                    oh.updateCarWithID(obj.Y, obj.X, obj.Id); //xy and y are in ui the other way around;
                }
            }
        }

        public int getEnvRules(int x, int y)
        {
            return eb.getNeededEnvironmentRules(x, y);
        }

        public void createNewVerkehrsteilnehmer(int x, int y, int speed, int direction, int nextDirection)
        {
      
            trafficobjs.Add(new TrafficObject(id_number,x, y, speed, direction, nextDirection));
            oh.addCarObject(x, y, id_number);
            id_number++;
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

            //TODO call UI to remove car
        }

        private int checkIfTilesAreEmpty(int startX, int startY, int destX, int destY)
        {
            int count = 0;
            foreach (TrafficObject obj in trafficobjs)
            {
                int xIncrement = 1;
                int yIncrement = 1;
                if (startX > destX)
                {
                    xIncrement = -1;
                }
                if (startY > destY)
                {
                    yIncrement = -1;
                }
                for(int x = startX; x!=(destX+xIncrement); x=x+xIncrement)
                    for (int y = startY; y != (destY + yIncrement); y = y + yIncrement)
                        if (obj.X == x && obj.Y == y)
                        {
                            count++;
                        }
            }
            return count;
        }

        private Boolean checkIfCanDrive4WayWithoutTrafficLight (TrafficObject obj)
        {
            switch ((obj.NextDirection - obj.Direction + 4) % 4) //calculates the Abbiegerichtung: 1= nach rechts, 2= nach geradeaus, 3 =nach links
            {
                case 3: //rechts abbiegen
                    return true;
                case 0: //geradeaus
                    return checkIfIntersectionEntryIsEmpty(obj.X, obj.Y, (obj.Direction + 1) % 4); //schau ob rechts nichts kommt
                case 1://links abbiegen
                    return checkIfIntersectionEntryIsEmpty(obj.X, obj.Y, (obj.Direction + 1) % 4) && checkIfIntersectionEntryIsEmpty(obj.X, obj.Y, (obj.Direction + 2) % 4);//schau ob rechts und vorne nichts kommt
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
                if ((obj.NextDirection - obj.Direction + 4) % 4 == 1) //links abiegen{
                {
                    return checkIfIntersectionEntryIsEmpty(obj.X, obj.Y, (obj.Direction + 2) % 4);//schau vorne nichts kommt

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
                    return new Tuple<int, int>(obj.X, obj.Y - obj.Speed);
                case (int)TrafficObject.Dir.Down:
                    return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
                case (int)TrafficObject.Dir.Left:
                    return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                case (int)TrafficObject.Dir.Right:
                    return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                default:
                    throw new NotImplementedException();
            }
        }

        private Boolean checkIfIntersectionEntryIsEmpty(int x, int y, int direction) //dirction describes in what direction a car would, therefore a car coming from right would have direction left
        {
            decimal d = x / 100;
            x = (int) Math.Floor(d)*100;
            d = y / 100;
            y = (int)Math.Floor(d) * 100;

            switch (direction)
            {
                case (int)TrafficObject.Dir.Down:
                    return (checkIfTilesAreEmpty(x + 40, y + 0, x + 50, y + 40) <= 0);
                case (int)TrafficObject.Dir.Right:
                    return (checkIfTilesAreEmpty(x + 0, y + 50, x + 40, y + 60) <= 0);
                case (int)TrafficObject.Dir.Up:
                    return (checkIfTilesAreEmpty(x + 50, y + 60, x + 60, y + 100) <= 0);
                case (int)TrafficObject.Dir.Left:
                    return (checkIfTilesAreEmpty(x + 60, y + 40, x + 100, y + 50) <= 0);
                default:
                    return false;
            }
        }

        private int getStreetRegion(int x, int y)
        {
            x = x % 100;
            y = y % 100;

            if (x >= 40 && x <= 60 && y >= 40 && y <= 60)//intersection
            {
                return (int) StreetRegion.Intersection;
            }
            else if (x >= 40 && x <= 50 && y >= 30 && y <= 40) //down-intersection-entry
            {
                return (int)StreetRegion.IntersectionAhead;
            }
            else if (x >= 30 && x <= 40 && y >= 50 && y <= 60)//right-intersection-entry
            {
                return (int)StreetRegion.IntersectionAhead;
            }
            else if (x >= 50 && x <= 60 && y >= 60 && y <= 70)//up-intersection-entry
            {
                return (int)StreetRegion.IntersectionAhead;
            }
            else if (x >= 60 && x <= 70 && y >= 40 && y <= 50)//left-intersection-entry
            {
                return (int)StreetRegion.IntersectionAhead;
            }
            else
            {
                return (int)StreetRegion.NormalStreet;
            }
        }

    }
}
