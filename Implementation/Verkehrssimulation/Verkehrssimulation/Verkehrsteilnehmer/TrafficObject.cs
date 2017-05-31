using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    class TrafficObject
    {
        private int id;
        private int x, y;
        private int speed;
        private int direction;
        private int nextDirection;
        private Boolean mayDrive; //default false, should be set to false if car moved
        private int typ;

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Direction{ get; set; }
        public int NextDirection { get; set; }
        public Boolean MayDrive { get; set; }
        public int PassingObstacleStatus { get; set; }

        public TrafficObject(int id, int x, int y, int speed, int direction, int nextDirection)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            this.Direction = direction;
            this.NextDirection = nextDirection;
            this.MayDrive = false;
            this.PassingObstacleStatus = (int) PassingObstStatus.RightSide;
        }

        public enum Dir{Up  =0, Left = 1, Down = 2, Right = 3 };// numbers are selceted that way so that are 90° ratation can be made by adding 1; !! do not change those, it will break the module!!
        public enum Fahrzeugtyp { Car = 0, Truck = 1};
        public enum PassingObstStatus { RightSide = 0, GoingWrongSide = 1, WrongSide = 2, GoingRightSide = 3};
        //public abstract void Update();

    }
}
