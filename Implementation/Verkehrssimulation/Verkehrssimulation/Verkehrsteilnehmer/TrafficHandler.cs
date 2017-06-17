using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkehrssimulation.Verkehrsnetz;
using Verkehrssimulation.GUI;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    class TrafficHandler : ITrafficHandler
    {
        readonly private int targetNumberOfCars = 20;
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
                //int nextRoadX;
                //int nextRoadY;
            
                thisRoadInfo= getEnvRules(obj.Y, obj.X);
                Tuple<int,int> nextRoadTileXY = getNextRoadTileXY(obj);
                obj.NextX = nextRoadTileXY.Item1;
                obj.NextY = nextRoadTileXY.Item2;
                switch (thisRoadInfo)
                {
                    case (int)EnvElement.StreetType.Street:
                        if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide)
                        {
                            //may drive if road ahead is empty
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                            if (obj.MayDrive)
                            {
                                if (checkIfObstacleAhead(obj.X, obj.Y, obj.Direction, obj.Speed))
                                {
                                    if (checkIfCanPassObstacle(obj.X, obj.Y, obj.Direction))
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.GoingWrongSide;
                                        nextRoadTileXY = getNextRoadTileXY(obj);
                                        obj.NextX = nextRoadTileXY.Item1;
                                        obj.NextY = nextRoadTileXY.Item2;
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                    }
                                    else
                                    {
                                        obj.MayDrive = false;
                                    }
                                }
                            }
                        }
                        else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide)
                        {
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                            if (obj.MayDrive)
                            {
                                if (obj.Direction == (int)TrafficObject.Dir.Up)
                                {
                                    if ((obj.X-obj.Speed)%100 > 45)
                                    {
                                        decimal d = obj.NextX / 100;
                                        obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                    }
                                    if (obj.X%100 == 45)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                        nextRoadTileXY = getNextRoadTileXY(obj);
                                        obj.NextX = nextRoadTileXY.Item1;
                                        obj.NextY = nextRoadTileXY.Item2;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }

                                if (obj.Direction == (int)TrafficObject.Dir.Down)
                                {
                                    if ((obj.X + obj.Speed) % 100 < 55)
                                    {
                                        decimal d = obj.NextX / 100;
                                        obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                    }
                                    if (obj.X%100 == 55)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                        nextRoadTileXY = getNextRoadTileXY(obj);
                                        obj.NextX = nextRoadTileXY.Item1;
                                        obj.NextY = nextRoadTileXY.Item2;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }

                                if (obj.Direction == (int)TrafficObject.Dir.Left)
                                {
                                    if ((obj.Y + obj.Speed) % 100 < 55)
                                    {
                                        decimal d = obj.NextY / 100;
                                        obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                    }
                                    if (obj.Y%100 == 55)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                        nextRoadTileXY = getNextRoadTileXY(obj);
                                        obj.NextX = nextRoadTileXY.Item1;
                                        obj.NextY = nextRoadTileXY.Item2;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }

                                if (obj.Direction == (int)TrafficObject.Dir.Right)
                                {
                                    if ((obj.Y - obj.Speed) % 100 < 45)
                                    {
                                        decimal d = obj.NextY / 100;
                                        obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                                    }
                                    if (obj.Y%100 == 45)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                        nextRoadTileXY = getNextRoadTileXY(obj);
                                        obj.NextX = nextRoadTileXY.Item1;
                                        obj.NextY = nextRoadTileXY.Item2;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }
                  
                            }
                        }

                        else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide) {
                            if (checkIfObstacleAhead(obj.X, obj.Y,((int) obj.Direction + 3) % 4, 10))
                            {
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1) && !checkIfObstacleAhead(obj.X, obj.Y, obj.Direction , obj.Speed);
                            }
                            else
                            {
                                obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.GoingRightSide;
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around

                            }
                        }

                        else
                        {
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                            if (obj.MayDrive)
                            {
                                if (obj.Direction == (int)TrafficObject.Dir.Down)
                                {
                                    if ((obj.X - obj.Speed) % 100 > 45)
                                    {
                                        decimal d = obj.NextX / 100;
                                        obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                    }
                                    if (obj.X%100 == 45)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }

                                if (obj.Direction == (int)TrafficObject.Dir.Up)
                                {
                                    if ((obj.X + obj.Speed) % 100 < 55)
                                    {
                                        decimal d = obj.NextX / 100;
                                        obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                    }
                                    if (obj.X%100 == 55)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }

                                if (obj.Direction == (int)TrafficObject.Dir.Right)
                                {
                                    if ((obj.Y + obj.Speed) % 100 < 55)
                                    {
                                        decimal d = obj.NextY / 100;
                                        obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                    }
                                    if (obj.Y%100 == 55)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }

                                if (obj.Direction == (int)TrafficObject.Dir.Left)
                                {
                                    if ((obj.Y - obj.Speed) % 100 < 45)
                                    {
                                        decimal d = obj.NextY / 100;
                                        obj.NextY = ((int)Math.Floor(d) * 100) + 455;
                                    }
                                    if (obj.Y%100 == 45)
                                    {
                                        obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                    }
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                                }
                            }
                        }

                        break;
                        //TODO get more detailed information where I can go.
                        //TODO get information if traffic light or not.

                    case (int)EnvElement.StreetType.ThreeKreuzung:
                    case (int)EnvElement.StreetType.FourKreuzung:
                        int streetRegion = getStreetRegion(obj.X, obj.Y);
                        switch (streetRegion)
                        {
                            case (int) StreetRegion.NormalStreet:
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                break;
                            case (int)StreetRegion.IntersectionAhead:
                                obj.MayDrive = checkIfCanDrive4WayWithoutTrafficLight(obj) && (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                break;
                            //TODO find Solution for "zugestaute Kreuzungen"
                            case (int)StreetRegion.Intersection:
                                switch ((obj.NextDirection - obj.Direction +4) % 4)
                                {
                                    case 0: //contues to drive in same direction
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                        break;
                                    default: //biegt wo ab.
                                        switch (obj.NextDirection)
                                        {
                                            case (int) TrafficObject.Dir.Up:
                                                if ((obj.X % 100 <= 55 && obj.NextX % 100 >= 55) || (obj.X % 100 >= 55 && obj.NextX % 100 <= 55))
                                                {
                                                    decimal d = obj.NextX / 100;
                                                    obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                                    //obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int) TrafficObject.Dir.Down:
                                                if ((obj.X % 100 <= 45 && obj.NextX % 100 >= 45) || (obj.X % 100 >= 45 && obj.NextX % 100 <= 45))
                                                {
                                                    decimal d = obj.NextX / 100;
                                                    obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                                    //obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Right:
                                                if ((obj.Y % 100 <= 55 && obj.NextY % 100 >= 55) || (obj.Y % 100 >= 55 && obj.NextY % 100 <= 55))
                                                {
                                                    decimal d = obj.NextY / 100;
                                                    obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                                    //obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Left:
                                                if ((obj.Y % 100 <= 45 && obj.NextY % 100 >= 45) || (obj.Y % 100 >= 45 && obj.NextY % 100 <= 45))
                                                {
                                                    decimal d = obj.NextY / 100;
                                                    obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                                                    //obj.Direction = obj.NextDirection;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                break;
                        }
                        break;
                    case 0: //traffic Light
                        //TODO reduce duplicate code with no traffic light
                        int streetRegion2 = getStreetRegion(obj.Y, obj.X);
                        switch (streetRegion2)
                        {
                            case (int)StreetRegion.NormalStreet:
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                break;
                            case (int)StreetRegion.IntersectionAhead:
                                obj.MayDrive = checkIfCanDriveWithTrafficLight(obj) && (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                break;
                            //TODO find Solution for "zugestaute Kreuzungen"
                            case (int)StreetRegion.Intersection:
                                switch ((obj.NextDirection - obj.Direction) % 4)
                                {
                                    case 0: //contues to drive in same direction
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1);
                                        break;
                                    default: //biegt wo ab.
                                        switch (obj.NextDirection)
                                        {
                                            case (int)TrafficObject.Dir.Up:
                                                if ((obj.X % 100 <= 55 && obj.NextX % 100 >= 55) || (obj.X % 100 >= 55 && obj.NextX % 100 <= 55))
                                                {
                                                    decimal d = obj.NextX / 100;
                                                    obj.NextX = ((int)Math.Floor(d) * 100) + 55; 
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Down:
                                                if ((obj.X % 100 <= 45 && obj.NextX % 100 >= 45) || (obj.X % 100 >= 45 && obj.NextX % 100 <= 45))
                                                {
                                                    decimal d = obj.NextX / 100;
                                                    obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Right:
                                                if ((obj.Y % 100 <= 55 && obj.NextY % 100 >= 55) || (obj.Y % 100 >= 55 && obj.NextY % 100 <= 55))
                                                {
                                                    decimal d = obj.NextY / 100;
                                                    obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                                }
                                                break;
                                            case (int)TrafficObject.Dir.Left:
                                                if ((obj.Y % 100 <= 45 && obj.NextY % 100 >= 45) || (obj.Y % 100 >= 45 && obj.NextY % 100 <= 45))
                                                {
                                                    decimal d = obj.NextY / 100;
                                                    obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                                                }
                                                break;
                                        }
                                        break;
                                }
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextY, obj.NextY) <= 1);
                                break;
                        }
                        break;

                    case (int)EnvElement.StreetType.Grass:
                        //may drive if road ahead is empty
                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY) <= 1); // only this car is around
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            List<int> removeIds = new List<int>();
            foreach (TrafficObject obj in trafficobjs) //update cars if they may drive
            {
                if (obj.MayDrive)
                { 
                    Tuple<int, int> nextRoadXY = getNextRoadTileXY(obj);
                    //int nextRoadX = nextRoadXY.Item1;
                    //int nextRoadY = nextRoadXY.Item2;
                    //decide where to go next (only) when leaving (100x100) roadTile
                    if ((Math.Abs(obj.X % 100 - obj.NextX % 100) > (obj.Speed + 1)) || (Math.Abs(obj.Y % 100 - obj.NextY % 100) > (obj.Speed + 1)))
                    {
                        int nextRoadInfo = getEnvRules(obj.NextY, obj.NextX); 
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

                    //fix position when taking a turn
                    if ((getStreetRegion(obj.X, obj.Y) == (int) StreetRegion.Intersection) && ((obj.NextDirection - obj.Direction) % 4) != 0){
                        switch (obj.NextDirection)
                        {
                            case (int)TrafficObject.Dir.Up:
                                if ((obj.X % 100 <= 55 && obj.NextX % 100 >= 55) || (obj.X % 100 >= 55 && obj.NextX % 100 <= 55))
                                {
                                    decimal d = obj.NextX / 100;
                                    obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                    obj.Direction = obj.NextDirection;

                                }
                                break;
                            case (int)TrafficObject.Dir.Down:
                                if ((obj.X % 100 <= 45 && obj.NextX % 100 >= 45) || (obj.X % 100 >= 45 && obj.NextX % 100 <= 45))
                                {
                                    decimal d = obj.NextX / 100;
                                    obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                    obj.Direction = obj.NextDirection;
                                }
                                break;
                            case (int)TrafficObject.Dir.Right:
                                if ((obj.Y % 100 <= 55 && obj.NextY % 100 >= 55) || (obj.Y % 100 >= 55 && obj.NextY % 100 <= 55))
                                {
                                    decimal d = obj.NextY / 100;
                                    obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                    obj.Direction = obj.NextDirection;
                                }
                                break;
                            case (int)TrafficObject.Dir.Left:
                                if ((obj.Y % 100 <= 45 && obj.NextY % 100 >= 45) || (obj.Y % 100 >= 45 && obj.NextY % 100 <= 45))
                                {
                                    decimal d = obj.NextY / 100;
                                    obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                                    obj.Direction = obj.NextDirection;
                                }
                                break;
                        }

                    }

                    obj.X = obj.NextX;
                    obj.Y = obj.NextY;//move the car to its new position
                    //send update to UI
                    oh.updateCarWithID(obj.Y, obj.X, obj.Id); //x and y are in ui the other way around

                    //destroy car if the left simulation, TODO send eventually to other group
                    if (obj.X <0 || obj.X > 700 || obj.Y < 0 || obj.Y > 700)
                    {
                        removeIds.Add(obj.Id);
                    }
                }
            }

            //remove cars that left the grid
            if (removeIds.Any())
            {
                foreach (int id in removeIds)
                {
                    removeVerkehrsteilnehmer(id);
                }
            }

            //add cars
            if (trafficobjs.Count < targetNumberOfCars)
            {
                double proberbility = 1 - Math.Pow((double)trafficobjs.Count / (double)targetNumberOfCars, 2);
                if (proberbility < rng.NextDouble())
                {
                    List<EntryPoint> entrypoints = eb.getEnvironmentEntries();
                    int entrypointIndex = rng.Next(0, entrypoints.Count);
                    EntryPoint entrypoint = entrypoints.ElementAt(entrypointIndex);
                    //TODO if entry point is at the corner of the grid find out if road is vertical or horizontal
                    //TODO adjust NextRoad Direction when Martin gives me layout of 3-way roads
                    if(entrypoint.TileX == 0)
                    {
                        createNewVerkehrsteilnehmer(0, entrypoint.TileY + 55, 5, (int) TrafficObject.Dir.Right, (int) TrafficObject.Dir.Right);
                    }
                    else if(entrypoint.TileX == 600)
                    {
                        createNewVerkehrsteilnehmer(700, entrypoint.TileY + 45, 5, (int)TrafficObject.Dir.Left, (int)TrafficObject.Dir.Left);
                    }
                    else if(entrypoint.TileY == 0)
                    {
                        createNewVerkehrsteilnehmer(entrypoint.TileX + 45, 0, 5, (int)TrafficObject.Dir.Down, (int)TrafficObject.Dir.Down);
                    }
                    else if (entrypoint.TileY == 600)
                    {
                        createNewVerkehrsteilnehmer(entrypoint.TileX + 55, 700, 5, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
                    }
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
            oh.addCarObject(y, x, id_number);
            id_number++;
        }

        public void removeVerkehrsteilnehmer(int id)
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
                    if (obj.PassingObstacleStatus == (int) TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                       return new Tuple<int, int>(obj.X, obj.Y - obj.Speed);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide)
                        return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                    else 
                        return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                case (int)TrafficObject.Dir.Down:
                    if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                        return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide)
                        return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                    else 
                        return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                case (int)TrafficObject.Dir.Left:
                    if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                        return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide)
                        return new Tuple<int, int>(obj.X, obj.Y - obj.Speed);
                    else 
                        return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
                case (int)TrafficObject.Dir.Right:
                    if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                        return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide)
                        return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
                    else 
                        return new Tuple<int, int>(obj.X, obj.Y - obj.Speed);
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

        private Boolean checkIfObstacleAhead(int x, int y, int direction, int speed)
        {
            List<Obstacle> obstacles = eb.getObstacles();
            switch (direction)
            {
                case (int)TrafficObject.Dir.Down:
                    for (int y2 = y; y2 <= y + speed; y2++)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y2 && y2 <= obst.EndX && obst.StartY <= x && x <= obst.EndY)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case (int)TrafficObject.Dir.Up:
                    for (int y2 = y; y2 >= y - speed; y2--)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y2 && y2 <= obst.EndX && obst.StartY <= x && x <= obst.EndY)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case (int)TrafficObject.Dir.Left:
                    for (int x2 = x; x2 >= x - speed; x2--)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y && y <= obst.EndX && obst.StartY <= x2 && x2 <= obst.EndY)
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case (int)TrafficObject.Dir.Right:
                    for (int x2 = x; x2 <= x + speed; x2++)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y && y <= obst.EndX && obst.StartY <= x2 && x2 <= obst.EndY)
                            {
                                return true;
                            }
                        }
                    }
                    break;
            }
            return false;
        }

        private Boolean checkIfCanPassObstacle(int x, int y, int direction)
        {
            List<Obstacle> obstacles = eb.getObstacles();
            int speed = 50; //TODO cleanup
            switch (direction)
            {
                case (int)TrafficObject.Dir.Down:
                    for (int y2 = y; y2 <= y + speed; y2++)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y2 && y2 <= obst.EndX && obst.StartY <= x+10 && x+10 <= obst.StartY)
                            {
                                return false;
                            }
                        }
                    }
                    return checkIfTilesAreEmpty(x + 10, y, x + 10, y + 50) == 0;
                case (int)TrafficObject.Dir.Up:
                    for (int y2 = y; y2 <= y - speed; y2++)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y2 && y2 <= obst.EndX && obst.StartY <= x-10 && x-10 <= obst.StartY)
                            {
                                return false;
                            }
                        }
                    }
                    return checkIfTilesAreEmpty(x - 10, y, x - 10, y + 50) == 0; //TODO solve problem only one car can pass obstacle at once, may argue the driver behind cannot see if save passing behind a passing car is possible so they only pass one at a time
                case (int)TrafficObject.Dir.Left:
                    for (int x2 = x; x2 <= x - speed; x2++)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y+10 && y+10 <= obst.EndX && obst.StartY <= x2 && x2 <= obst.StartY)
                            {
                                return false;
                            }
                        }

                    }
                    return checkIfTilesAreEmpty(x, y + 10, x + 50, y + 10) == 0;
                case (int)TrafficObject.Dir.Right:
                    for (int x2 = x; x2 <= x + speed; x2++)
                    {
                        foreach (Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y-10 && y-10 <= obst.EndX && obst.StartY <= x2 && x2 <= obst.StartY)
                            {
                                return false;
                            }
                        }
                    }
                    return checkIfTilesAreEmpty(x, y - 10, x + 50, y - 10) == 0;
            }
            return false;
        }
    }
}
