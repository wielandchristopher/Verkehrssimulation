using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    class AmpelHandler
    {
        List<Ampel> ampellist;
        Canvas canvas;

        public AmpelHandler(Canvas mycanvas)
        {
            ampellist = new List<Ampel>();
            canvas = mycanvas;

            //initialize();
        }

        private void initialize()
        {
            int j = 0;
            // Ampel Kreuzung unten
            for (int i = 60; i < 700; i += 100)
            {
                for (int y = 60; y < 700; y += 100)
                {
                    addAmpel(new Ampel(i, y, 1, j+1));
                }
            }

            // Ampel Kreuzung rechts
            for (int i = 30; i < 700; i += 100)
            {
                for (int y = 60; y < 700; y += 100)
                {
                    addAmpel(new Ampel(i, y, 2, j+1));
                }
            }

            // Ampel Kreuzung oben
            for (int i = 7; i < 700; i += 100)
            {
                for (int y = 30; y < 700; y += 100)
                {
                    addAmpel(new Ampel(i, y, 3, j+1));
                }
            }

            // Ampel Kreuzung links
            for (int i = 60; i < 700; i += 100)
            {
                for (int y = 7; y < 700; y += 100)
                {
                    Ampel a = new Ampel(i, y, 4, j+1);
                    addAmpel(a);
                }
            }
        }

        public bool addAmpel(Ampel ampel)
        {
            ampellist.Add(ampel);
            canvas.Children.Add(ampel.getShape());
            canvas.Children.Add(ampel.getCircleGreen());
            canvas.Children.Add(ampel.getCircleYellow());
            canvas.Children.Add(ampel.getCircleRed());
            return true;
        }

        public void blinky()
        {
            foreach (Ampel a in ampellist)
            {
                if (a.yellow == false)
                {
                    a.setYellow();
                    a.yellow = true;
                }
                else
                {
                    a.unsetYellow();
                    a.yellow = false;
                }

            }
        }
    }
}
