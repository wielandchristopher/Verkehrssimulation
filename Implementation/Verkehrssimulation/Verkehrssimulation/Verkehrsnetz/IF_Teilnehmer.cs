using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    interface IF_Teilnehmer
    {
        int getNeededEnvironmentRules(int x, int y);

    }
}
