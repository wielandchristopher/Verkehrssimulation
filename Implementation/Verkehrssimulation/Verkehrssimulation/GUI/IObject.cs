using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    interface IObject
    {
        Shape objshp
        {
            get;
            set;
        }

        int xpos
        {
            get;
            set;
        }

        int ypos
        {
            get;
            set;
        }


    }
}
