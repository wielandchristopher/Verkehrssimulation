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
        protected int speed;
        protected int direction;

        public abstract void Update();

    }
}
