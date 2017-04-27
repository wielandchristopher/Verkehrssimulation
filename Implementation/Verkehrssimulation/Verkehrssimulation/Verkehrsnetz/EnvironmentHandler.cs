using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentHandler : IF_Teilnehmer
    {

        int output;

        public EnvironmentHandler()
        {
            output = 2;
        }
        public int getNeededEnvironmentRules(int x, int y)
        {
            return output;
        }

        public void changeOutput(int val)
        {
            output = val;
        }
    }
}
