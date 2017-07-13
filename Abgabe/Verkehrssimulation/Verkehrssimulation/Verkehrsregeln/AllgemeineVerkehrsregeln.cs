using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsregeln
{
    /*********************************
     * Type:
     * 1 = Rechts vor Links
     * 2 = Gegenverkehrsregel
     ********************************/
    public class AllgemeineVerkehrsregeln : IVerkehrsregeln
    {
        private List<int> regeln;
        private int anzahlRegeln = 2;

        private List<int> fillList()
        {
            List<int> regeln = new List<int>();

            for(int i = 1; i <= anzahlRegeln; i++)
            {
                regeln.Add(i);
            }

            return regeln;
        }

        public List<Schilder> createSchilder(int _anzahl, int _type)
        {
            throw new NotImplementedException();
        }

        public List<int> getAllgemeineVerkehrsregeln()
        {
            List<int> regeln = fillList();
            return regeln;
        }
    }
}
