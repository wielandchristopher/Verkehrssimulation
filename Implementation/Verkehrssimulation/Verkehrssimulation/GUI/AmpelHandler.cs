using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Verkehrssimulation.GUI
{
    class AmpelHandler
    {
        List<Ampel> ampellist;
        Canvas canvas;
        Random random;

        public AmpelHandler(Canvas mycanvas)
        {
            ampellist = new List<Ampel>();
            canvas = mycanvas;
        }

        public bool addAmpel(Ampel ampel)
        {
            ampellist.Add(ampel);
            canvas.Children.Add(ampel.getShape());
            canvas.Children.Add(ampel.getCircleGreen());
            canvas.Children.Add(ampel.getCircleRed());
            return true;
        }
    }
}
