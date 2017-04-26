using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Ampelsteuerung
{

    // here cristopher handles the communication to his ampeln? then martin have nothing to do with WCF ?
    class Ampelhandler : IF_Ampel
    {

        public List<Ampel> getAmpelStatus() // for the Verkehrsnetz to get the informations about the ampeln
        {
            throw new NotImplementedException();
        }
    }

    public class Ampel
    {
    }
}
