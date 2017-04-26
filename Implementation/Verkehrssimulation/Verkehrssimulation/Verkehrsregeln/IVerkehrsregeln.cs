using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsregeln
{
    interface IVerkehrsregeln
    {
        List<Schilder> createSchilder(int _anzahl, int _type);
        List<int> getAllgemeineVerkehrsregeln();
    }
}
