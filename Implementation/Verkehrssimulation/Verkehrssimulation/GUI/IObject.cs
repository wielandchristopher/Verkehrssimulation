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
        bool addCarObject(int x, int y, int id);
    }
}
