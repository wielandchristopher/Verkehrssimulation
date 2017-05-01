using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsteilnehmer
{
    abstract class TrafficObject
    {
        protected int x, y;
        public int X { get; set; }
        public int Y { get; set; }
        protected int speed;
        protected int direction;
        public int Direction{ get; set; }
        protected int nextDirection;
        public int NextDirection { get; set; }
        protected Boolean mayDrive; //default false, should be set to false if car moved
        public Boolean MayDrive { get; set; }

        public enum Dir{Up=0, Left=1, Down=2, Right =3 };// numbers are selceted that way so that are 90° ratation can be made by adding 1; !! do not change those, it will break the module!!

        public abstract void Update();

    }
}
