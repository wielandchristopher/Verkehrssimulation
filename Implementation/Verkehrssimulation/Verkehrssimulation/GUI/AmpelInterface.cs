using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    interface AmpelInterface
    {
        Shape objshp
        {
            get;
            set;
        }

        Shape greenLight
        {
            get;
            set;
        }

        Shape redLight
        {
            get;
            set;
        }

        int objid
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
