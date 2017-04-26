using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Ampelsteuerung
{
    interface IF_Ampel
    {
        List<Ampel> getAmpelStatus();
    }
}
