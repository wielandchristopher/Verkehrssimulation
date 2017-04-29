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

        public enum Dir{Up=1, Down=2, Right =3, Left=4};

        public abstract void Update();

    }
}
