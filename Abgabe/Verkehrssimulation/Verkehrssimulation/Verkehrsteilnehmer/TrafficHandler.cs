﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verkehrssimulation.Verkehrsnetz;
using Verkehrssimulation.GUI;
using static Verkehrssimulation.RabbitMQ.RabbitMQHandler;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    class TrafficHandler : ITrafficHandler,IGUI
    {
        private static TrafficHandler instance = null;
        private int targetNumberOfCars = 20;
        private int truckratio = 0; 
        private List<TrafficObject> trafficobjs; // liste mit Verkehrsobjekten
        private I_ENV_VKTeilnehmer eb; // ref auf Environmenthandler zum abfragen der rules
        private IObject oh; //ref zu GUI
        private RabbitMQ.RabbitMQHandler mqhandler; //rabbitMq
        private RabbitMQ.RabbitMQHandler.RemoteTransaction remoteTransaction;
        Random rng = new Random(); //random number generator
        int id_number;

        public enum StreetRegion { NormalStreet = 0, IntersectionAhead = 1, Intersection = 2};

        private TrafficHandler(ref I_ENV_VKTeilnehmer _eb, ref IObject _oh, ref RabbitMQ.RabbitMQHandler _mqhandler)
        {
            trafficobjs = new List<TrafficObject>();
            oh = _oh;
            eb = _eb;
            mqhandler = _mqhandler;
            id_number = 1;
        }

        public static TrafficHandler getInstance(ref I_ENV_VKTeilnehmer _eb, ref IObject _oh, ref RabbitMQ.RabbitMQHandler _mqhandler)
        {
            if (instance == null)
            {
                instance = new TrafficHandler(ref _eb, ref _oh, ref _mqhandler);
            }
            return instance;
        }

        public static TrafficHandler getInstance()
        {
             return instance;
        }

        public void updateAll()
        {
            foreach (TrafficObject obj in trafficobjs)  //check how can move forward and flag them if they can do so
            {
                StreetInfo thisRoadInfo;
                //int nextRoadX;
                //int nextRoadY;

                thisRoadInfo = getEnvRules(obj.Y, obj.X);
                Tuple<int, int> nextRoadTileXY = getNextRoadTileXY(obj);
                obj.NextX = nextRoadTileXY.Item1;
                obj.NextY = nextRoadTileXY.Item2;
                if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.TurnAround)
                {
                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                    if (obj.MayDrive)
                    {
                        if (obj.Direction == (int)TrafficObject.Dir.Up)
                        {
                            if ((obj.X - obj.Speed) % 100 < 45)
                            {
                                decimal d = obj.NextX / 100;
                                obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                            }
                            if (obj.X % 100 == 45)
                            {
                                obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                obj.Direction = (int)TrafficObject.Dir.Down;
                                obj.NextDirection = (int)TrafficObject.Dir.Down;
                                nextRoadTileXY = getNextRoadTileXY(obj);
                                obj.NextX = nextRoadTileXY.Item1;
                                obj.NextY = nextRoadTileXY.Item2;
                            }
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                        }

                        else if (obj.Direction == (int)TrafficObject.Dir.Down)
                        {
                            if ((obj.X + obj.Speed) % 100 > 55)
                            {
                                decimal d = obj.NextX / 100;
                                obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                            }
                            if (obj.X % 100 == 55)
                            {
                                obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                obj.Direction = (int)TrafficObject.Dir.Up;
                                obj.NextDirection = (int)TrafficObject.Dir.Up;
                                nextRoadTileXY = getNextRoadTileXY(obj);
                                obj.NextX = nextRoadTileXY.Item1;
                                obj.NextY = nextRoadTileXY.Item2;
                            }
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                        }

                        else if (obj.Direction == (int)TrafficObject.Dir.Left)
                        {
                            if ((obj.Y + obj.Speed) % 100 > 55)
                            {
                                decimal d = obj.NextY / 100;
                                obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                            }
                            if (obj.Y % 100 == 55)
                            {
                                obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                obj.Direction = (int)TrafficObject.Dir.Right;
                                obj.NextDirection = (int)TrafficObject.Dir.Right;
                                nextRoadTileXY = getNextRoadTileXY(obj);
                                obj.NextX = nextRoadTileXY.Item1;
                                obj.NextY = nextRoadTileXY.Item2;
                            }
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                        }

                        else if (obj.Direction == (int)TrafficObject.Dir.Right)
                        {
                            if ((obj.Y - obj.Speed) % 100 < 45)
                            {
                                decimal d = obj.NextY / 100;
                                obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                            }
                            if (obj.Y % 100 == 45)
                            {
                                obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                obj.Direction = (int)TrafficObject.Dir.Left;
                                obj.NextDirection = (int)TrafficObject.Dir.Left;
                                nextRoadTileXY = getNextRoadTileXY(obj);
                                obj.NextX = nextRoadTileXY.Item1;
                                obj.NextY = nextRoadTileXY.Item2;
                            }
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                        }
                    }
                }
                else
                {

                    switch (thisRoadInfo.type)
                    {
                        case (int)EnvElement.StreetType.Street:
                            if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide)
                            {
                                //may drive if road ahead is empty
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
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
                                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
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
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                if (obj.MayDrive)
                                {
                                    if (obj.Direction == (int)TrafficObject.Dir.Up)
                                    {
                                        if ((obj.X - obj.Speed) % 100 > 45)
                                        {
                                            decimal d = obj.NextX / 100;
                                            obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                        }
                                        if (obj.X % 100 == 45)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                    if (obj.Direction == (int)TrafficObject.Dir.Down)
                                    {
                                        if ((obj.X + obj.Speed) % 100 < 55)
                                        {
                                            decimal d = obj.NextX / 100;
                                            obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                        }
                                        if (obj.X % 100 == 55)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                    if (obj.Direction == (int)TrafficObject.Dir.Left)
                                    {
                                        if ((obj.Y + obj.Speed) % 100 < 55)
                                        {
                                            decimal d = obj.NextY / 100;
                                            obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                        }
                                        if (obj.Y % 100 == 55)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                    if (obj.Direction == (int)TrafficObject.Dir.Right)
                                    {
                                        if ((obj.Y - obj.Speed) % 100 > 45)
                                        {
                                            decimal d = obj.NextY / 100;
                                            obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                                        }
                                        if (obj.Y % 100 == 45)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.WrongSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                }
                            }

                            else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                            {
                                if (checkIfObstacleAhead(obj.X, obj.Y, ((int)obj.Direction + 3) % 4, 10))
                                {
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0) && !checkIfObstacleAhead(obj.X, obj.Y, obj.Direction, obj.Speed);
                                }
                                else
                                {
                                    obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.GoingRightSide;
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around

                                }
                            }

                            else
                            {
                                obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                if (obj.MayDrive)
                                {
                                    if (obj.Direction == (int)TrafficObject.Dir.Down)
                                    {
                                        if ((obj.X - obj.Speed) % 100 > 45)
                                        {
                                            decimal d = obj.NextX / 100;
                                            obj.NextX = ((int)Math.Floor(d) * 100) + 45;
                                        }
                                        if (obj.X % 100 == 45)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                    if (obj.Direction == (int)TrafficObject.Dir.Up)
                                    {
                                        if ((obj.X + obj.Speed) % 100 < 55)
                                        {
                                            decimal d = obj.NextX / 100;
                                            obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                        }
                                        if (obj.X % 100 == 55)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                    if (obj.Direction == (int)TrafficObject.Dir.Right)
                                    {
                                        if ((obj.Y + obj.Speed) % 100 < 55)
                                        {
                                            decimal d = obj.NextY / 100;
                                            obj.NextY = ((int)Math.Floor(d) * 100) + 55;
                                        }
                                        if (obj.Y % 100 == 55)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }

                                    if (obj.Direction == (int)TrafficObject.Dir.Left)
                                    {
                                        if ((obj.Y - obj.Speed) % 100 < 45)
                                        {
                                            decimal d = obj.NextY / 100;
                                            obj.NextY = ((int)Math.Floor(d) * 100) + 45;
                                        }
                                        if (obj.Y % 100 == 45)
                                        {
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.RightSide;
                                            nextRoadTileXY = getNextRoadTileXY(obj);
                                            obj.NextX = nextRoadTileXY.Item1;
                                            obj.NextY = nextRoadTileXY.Item2;
                                        }
                                        obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                                    }
                                }
                            }

                            break;
                        case (int)EnvElement.StreetType.ThreeKreuzung:
                        case (int)EnvElement.StreetType.FourKreuzung:
                            int streetRegion = getStreetRegion(obj.X, obj.Y);
                            switch (streetRegion)
                            {
                                case (int)StreetRegion.NormalStreet:
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0);
                                    break;
                                case (int)StreetRegion.IntersectionAhead:
                                    obj.MayDrive = checkIfCanDrive4Way(obj, thisRoadInfo) && (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0);
                                    break;
                                case (int)StreetRegion.Intersection:
                                    switch ((obj.NextDirection - obj.Direction + 4) % 4)
                                    {
                                        case 0: //contues to drive in same direction
                                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0);
                                            break;
                                        default: //biegt wo ab.
                                            switch (obj.NextDirection)
                                            {
                                                case (int)TrafficObject.Dir.Up:
                                                    if ((obj.X % 100 <= 55 && obj.NextX % 100 >= 55) || (obj.X % 100 >= 55 && obj.NextX % 100 <= 55))
                                                    {
                                                        decimal d = obj.NextX / 100;
                                                        obj.NextX = ((int)Math.Floor(d) * 100) + 55;
                                                        //obj.Direction = obj.NextDirection;
                                                    }
                                                    break;
                                                case (int)TrafficObject.Dir.Down:
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
                                    obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0);
                                    break;
                            }
                            break;
                        case (int)EnvElement.StreetType.Grass:
                            //may drive if road ahead is empty
                            obj.MayDrive = (checkIfTilesAreEmpty(obj.X, obj.Y, obj.NextX, obj.NextY, obj.Id, false) == 0); // only this car is around
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }

            List<int> removeIds = new List<int>();
            foreach (TrafficObject obj in trafficobjs) //update cars if they may drive
            {
                if (obj.WaitingCounter >= 10 && obj.MayDrive)
                {
                    decimal dx = obj.NextX / 100;
                    decimal dy = obj.NextY / 100;
                    obj.MayDrive = checkIfTilesAreEmpty(((int)Math.Floor(dx) * 100) + 40, ((int)Math.Floor(dy) * 100) + 40, ((int)Math.Floor(dx) * 100) + 60, ((int)Math.Floor(dy) * 100) + 60, obj.Id, false) == 0;
                }
                if (obj.MayDrive)
                {
                    obj.WaitingCounter = 0;
                    Tuple<int, int> nextRoadXY = getNextRoadTileXY(obj);
                    //decide where to go next (only) when leaving (100x100) roadTile
                    if ((Math.Abs(obj.X % 100 - obj.NextX % 100) > (obj.Speed + 1)) || (Math.Abs(obj.Y % 100 - obj.NextY % 100) > (obj.Speed + 1)))
                    {
                        StreetInfo nextRoadInfo = getEnvRules(obj.NextY, obj.NextX); 
                        switch (nextRoadInfo.type)
                        {
                            case (int)EnvElement.StreetType.Street:
                                //no direction change posible
                                if (nextRoadInfo.layout == 1 && (obj.Direction == (int)TrafficObject.Dir.Up || obj.Direction == (int)TrafficObject.Dir.Down))
                                    obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;
                                if (nextRoadInfo.layout == 2 && (obj.Direction == (int)TrafficObject.Dir.Right || obj.Direction == (int)TrafficObject.Dir.Left))
                                    obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;
                               break;
                            case (int)EnvElement.StreetType.ThreeKreuzung:
                                //get layout from Straßennetz
                                int layout = nextRoadInfo.layout;
                                int rotation_modifier = 0;
                                switch (layout)
                                {
                                    case 1:
                                        //    ||
                                        // ===
                                        //    ||
                                        rotation_modifier = 0;
                                        if (obj.Direction == (int) TrafficObject.Dir.Left)
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;
                                        
                                        break;
                                    case 2:
                                        // ===  ===
                                        //    ||
                                        rotation_modifier = 1;
                                        if (obj.Direction == (int)TrafficObject.Dir.Down)
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;
                                      
                                        break;
                                    case 3:
                                        // ||
                                        //   ===
                                        // ||
                                        rotation_modifier = 2;
                                        if (obj.Direction == (int)TrafficObject.Dir.Right)
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;
                                        break;
                                    case 4:
                                        //    ||
                                        // ===  ===
                                        rotation_modifier = 3;
                                        if (obj.Direction == (int)TrafficObject.Dir.Up)
                                            obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;                                       
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
                                    case (int)TrafficObject.Dir.Left:
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
                            case (int)EnvElement.StreetType.Grass:
                                obj.PassingObstacleStatus = (int)TrafficObject.PassingObstStatus.TurnAround;//wenden
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
                    oh.updateCarWithID(obj.Y, obj.X, obj.Id, obj.Typ, obj.Direction); //x and y are in ui the other way around

                    //destroy car if the left simulation, send eventually to other group
                    if (obj.X <0 || obj.X > 700 || obj.Y < 0 || obj.Y > 700)
                    {
                        removeIds.Add(obj.Id);

                        //send Cars to other group via RabbitMq
                        int speed = obj.Speed * 20;
                        String group = "";
                        switch (obj.Direction)
                        {
                            case (int)TrafficObject.Dir.Down:
                                group = "group1";
                                break;
                            case (int)TrafficObject.Dir.Right:
                                group = "group2";
                                break;
                            case (int)TrafficObject.Dir.Up:
                                group = "group4";
                                break;
                        }
                        if ((!group.Equals("")) && rng.NextDouble() < 0.1){ //cars only have a 10% change to be sent
                            string typ = obj.Typ == (int) TrafficObject.Fahrzeugtyp.Car ? "PKW" : "LKW";
                            remoteTransaction = new RemoteTransaction(speed > 100 ? 100 : speed, typ);
                            mqhandler.Send(remoteTransaction, group);
                        }
                            
                    }
                }
                else
                {
                    obj.WaitingCounter++;
                    if (obj.WaitingCounter >= 100)
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
                if (proberbility > rng.NextDouble())
                {
                    List<EntryPoint> entrypoints = eb.getEnvironmentEntries();
                    int entrypointIndex = rng.Next(0, entrypoints.Count);
                    EntryPoint entrypoint = entrypoints.ElementAt(entrypointIndex);
                    //TODO if entry point is at the corner of the grid find out if road is vertical or horizontal
                    //TODO adjust NextRoad Direction when Martin gives me layout of 3-way roads
                    int typ = getCurrentTruckRatio() < truckratio ? (int)TrafficObject.Fahrzeugtyp.Truck : (int)TrafficObject.Fahrzeugtyp.Car;
                    addCarToEntryPoint(entrypoint, typ);
                }
            }

            if (rng.NextDouble() < 0.05) //for performance there is only a 5% change to request cars from rabbirmq
                mqhandler.Receive(); //add Cars from other group
        }

        private StreetInfo getEnvRules(int x, int y)
        {
            return eb.getNeededStreetRules(x, y);
        }

        public void createNewVerkehrsteilnehmer(int x, int y, int speed, int typ, int direction, int nextDirection)
        {
      
            trafficobjs.Add(new TrafficObject(id_number,x, y, speed, typ, direction, nextDirection));
            //add cars to GUI
            if (typ == (int) TrafficObject.Fahrzeugtyp.Car)
                oh.addCarObject(y, x, id_number);
            else
                oh.addLKWObject(y, x, direction, id_number);
            id_number++;
        }

        public void addCarToEntryPoint(EntryPoint entrypoint, int typ, int speed = 5, int moveForward = 0)
        {
            if (entrypoint == null) {
                entrypoint = eb.getEnvironmentEntries().ElementAt(0);
            }

            if (entrypoint.TileX == 0)
            {
                if (entrypoint.TileY == 0 || entrypoint.TileY == 600)
                {
                    if (eb.getNeededStreetRules(entrypoint.TileX, entrypoint.TileY).layout == 1)
                        createNewVerkehrsteilnehmer(moveForward, entrypoint.TileY + 55, speed, typ, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
                    else
                    {
                        if (entrypoint.TileY == 0)
                            createNewVerkehrsteilnehmer(entrypoint.TileX + 45, moveForward, speed, typ, (int)TrafficObject.Dir.Down, (int)TrafficObject.Dir.Down);
                        else
                            createNewVerkehrsteilnehmer(entrypoint.TileX + 55, 700 - moveForward, speed, typ, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
                    }
                }
                else
                    createNewVerkehrsteilnehmer(moveForward, entrypoint.TileY + 55, speed, typ, (int)TrafficObject.Dir.Right, (int)TrafficObject.Dir.Right);
            }
            else if (entrypoint.TileX == 600)
            {
                if (entrypoint.TileY == 0 || entrypoint.TileY == 600)
                {
                    if (eb.getNeededStreetRules(entrypoint.TileX, entrypoint.TileY).layout == 1)
                        createNewVerkehrsteilnehmer(700- moveForward, entrypoint.TileY + 45, speed, typ, (int)TrafficObject.Dir.Left, (int)TrafficObject.Dir.Left);
                    else
                    {
                        if (entrypoint.TileY == 0)
                            createNewVerkehrsteilnehmer(entrypoint.TileX + 45, moveForward, speed, typ, (int)TrafficObject.Dir.Down, (int)TrafficObject.Dir.Down);
                        else
                            createNewVerkehrsteilnehmer(entrypoint.TileX + 55, 700 - moveForward, speed, typ, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
                    }
                }
                else
                    createNewVerkehrsteilnehmer(700 - moveForward, entrypoint.TileY + 45, speed, typ, (int)TrafficObject.Dir.Left, (int)TrafficObject.Dir.Left);
            }
            else if (entrypoint.TileY == 0)
            {
                createNewVerkehrsteilnehmer(entrypoint.TileX + 45, moveForward, speed, typ, (int)TrafficObject.Dir.Down, (int)TrafficObject.Dir.Down);
            }
            else if (entrypoint.TileY == 600)
            {
                createNewVerkehrsteilnehmer(entrypoint.TileX + 55, 700 - moveForward, speed, typ, (int)TrafficObject.Dir.Up, (int)TrafficObject.Dir.Up);
            }
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

            oh.removeObject(id);
        }

        private int checkIfTilesAreEmpty(int startX, int startY, int destX, int destY, int requesterId, Boolean ignoreStandingCars)
        {
            int count = 0;
            foreach (TrafficObject obj in trafficobjs)
            {
                if (obj.Id != requesterId)
                {
                    if (ignoreStandingCars && (obj.WaitingCounter >= 30))
                        return 0;
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
                    for (int x = startX; x != (destX + xIncrement); x = x + xIncrement)
                        for (int y = startY; y != (destY + yIncrement); y = y + yIncrement)
                            if (obj.X == x && obj.Y == y)
                            {
                                count++;
                            }
                }
            }
            return count;
        }

        private Boolean checkIfCanDrive4Way(TrafficObject obj, StreetInfo info)
        {
            int ampelstatus = 0;
            switch (obj.Direction)
            {
                case (int) TrafficObject.Dir.Down:
                    ampelstatus = info.ampelstatusUp;
                    break;
                case (int) TrafficObject.Dir.Up:
                    ampelstatus = info.ampelstatusDown;
                    break;
                case (int)TrafficObject.Dir.Right:
                    ampelstatus = info.ampelstatusLeft;
                    break;
                case (int)TrafficObject.Dir.Left:
                    ampelstatus = info.ampelstatusRight;
                    break;
            }
            if (ampelstatus != 0 && ampelstatus != 1 && ampelstatus != 2)
            {
                return checkIfCanDrive4WayWithoutTrafficLight(obj);
            }
            else
            {
                return checkIfCanDriveWithTrafficLight(obj, ampelstatus);
            }
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

        private Boolean checkIfCanDriveWithTrafficLight(TrafficObject obj, int trafficLightColour)
        {

            Boolean isGreen = (trafficLightColour == 2);
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
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.TurnAround)
                        return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                    else 
                        return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                case (int)TrafficObject.Dir.Down:
                    if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                        return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.TurnAround)
                        return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                    else 
                        return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                case (int)TrafficObject.Dir.Left:
                    if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                        return new Tuple<int, int>(obj.X - obj.Speed, obj.Y);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.TurnAround)
                        return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
                    else 
                        return new Tuple<int, int>(obj.X, obj.Y - obj.Speed);
                case (int)TrafficObject.Dir.Right:
                    if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.RightSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.WrongSide)
                        return new Tuple<int, int>(obj.X + obj.Speed, obj.Y);
                    else if (obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.GoingWrongSide || obj.PassingObstacleStatus == (int)TrafficObject.PassingObstStatus.TurnAround)
                        return new Tuple<int, int>(obj.X, obj.Y - obj.Speed);
                    else 
                        return new Tuple<int, int>(obj.X, obj.Y + obj.Speed);
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
                    return (checkIfTilesAreEmpty(x + 40, y + 0, x + 50, y + 40, -1, true) <= 0); //stehende Autos werden ignoriert weil Vorfahrsverzicht.
                case (int)TrafficObject.Dir.Right:
                    return (checkIfTilesAreEmpty(x + 0, y + 50, x + 40, y + 60, - 1, true) <= 0);
                case (int)TrafficObject.Dir.Up:
                    return (checkIfTilesAreEmpty(x + 50, y + 60, x + 60, y + 100, -1, true) <= 0);
                case (int)TrafficObject.Dir.Left:
                    return (checkIfTilesAreEmpty(x + 60, y + 40, x + 100, y + 50, -1, true) <= 0);
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

        private Boolean checkIfObstacleAhead(int y, int x, int direction, int speed) //x and y other way round in other packages
        {
            List<Verkehrsnetz.Obstacle> obstacles = eb.getObstacles();
            switch (direction)
            {
                case (int)TrafficObject.Dir.Down:
                    for (int y2 = y; y2 <= y + speed; y2++)
                    {
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
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
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
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
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
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
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
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

        private Boolean checkIfCanPassObstacle(int y, int x, int direction)//x and y other way round in other packages
        {
            List<Verkehrsnetz.Obstacle> obstacles = eb.getObstacles();
            int speed = 50;
            switch (direction)
            {
                case (int)TrafficObject.Dir.Down:
                    for (int y2 = y; y2 <= y + speed; y2++)
                    {
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y2 && y2 <= obst.EndX && obst.StartY <= x+10 && x+10 <= obst.StartY)
                            {
                                return false;
                            }
                        }
                    }
                    return checkIfTilesAreEmpty(x + 10, y, x + 10, y + 50, -1, false) == 0;
                case (int)TrafficObject.Dir.Up:
                    for (int y2 = y; y2 <= y - speed; y2++)
                    {
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y2 && y2 <= obst.EndX && obst.StartY <= x-10 && x-10 <= obst.StartY)
                            {
                                return false;
                            }
                        }
                    }
                    return checkIfTilesAreEmpty(x - 10, y, x - 10, y + 50, -1, false) == 0;
                case (int)TrafficObject.Dir.Left:
                    for (int x2 = x; x2 <= x - speed; x2++)
                    {
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y+10 && y+10 <= obst.EndX && obst.StartY <= x2 && x2 <= obst.StartY)
                            {
                                return false;
                            }
                        }

                    }
                    return checkIfTilesAreEmpty(x, y + 10, x + 50, y + 10, -1, false) == 0;
                case (int)TrafficObject.Dir.Right:
                    for (int x2 = x; x2 <= x + speed; x2++)
                    {
                        foreach (Verkehrsnetz.Obstacle obst in obstacles)
                        {
                            if (obst.StartX <= y-10 && y-10 <= obst.EndX && obst.StartY <= x2 && x2 <= obst.StartY)
                            {
                                return false;
                            }
                        }
                    }
                    return checkIfTilesAreEmpty(x, y - 10, x + 50, y - 10, -1, false) == 0;
            }
            return false;
        }

        public void updateCarAmount(int cars)
        {
            targetNumberOfCars = cars;
        }

        public void updateTruckRatio(int percent)
        {
            truckratio = percent;
        }

        private int getCurrentTruckRatio() {
            int trucks = 0;
            foreach (TrafficObject obj in trafficobjs)
            {
                if (obj.Typ == (int) TrafficObject.Fahrzeugtyp.Truck)
                {
                    trucks++;
                }
            }
            if (trafficobjs.Count != 0)
            {
                return 100 * trucks / trafficobjs.Count;
            }
            else
            {
                return 0;
            }
        }
    }
}
