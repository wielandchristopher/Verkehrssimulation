﻿using System;
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

        public TrafficObject(int id, int x, int y, int speed, int direction, int nextDirection)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.speed = speed;
            this.direction = direction;
            this.nextDirection = nextDirection;
            this.mayDrive = false;
        }

        public enum Dir{Up  =0, Left = 1, Down = 2, Right = 3 };// numbers are selceted that way so that are 90° ratation can be made by adding 1; !! do not change those, it will break the module!!
        public enum Fahrzeugtyp { Car = 0, Truck = 1};
        //public abstract void Update();

    }
}
