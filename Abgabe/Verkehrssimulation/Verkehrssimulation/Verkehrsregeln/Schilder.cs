using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsregeln
{
    /*********************************
     * Type:
     * 1 = Stopp
     * 2 = Vorrang geben
     * 3 = Einbahn
     * 4 = Einfahrt verboten
     * *******************************/
    public class Schilder : IVerkehrsregeln
    {
        private int type;

        private Schilder(int _type)
        {
            this.type = _type;
        }

        public Schilder()
        {

        }

        public List<Schilder> createSchilder(int _anzahl, int _type)
        {
            List<Schilder> schilderList = new List<Schilder>();

            for(int i = 0; i < _anzahl; i++)
            {
                Schilder schild = new Schilder(_type);

                schilderList.Add(schild);
            }

            return schilderList;
        }

        public List<int> getAllgemeineVerkehrsregeln()
        {
            throw new NotImplementedException();
        }
    }
}
