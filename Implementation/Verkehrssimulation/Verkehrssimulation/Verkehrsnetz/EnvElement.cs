using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Verkehrssimulation.Verkehrsnetz
{
    abstract class EnvElement
    {
        public enum StreetType { Street = 1, ThreeKreuzung = 2, FourKreuzung = 3, Grass = 4, greenCircle = 5 };
        protected Image img;
        protected int x,y,dir;
        protected StreetType stype;
        public Image getImage() {
            return img;
        }

        public void enter(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 0.9;
        }

        public void leave(object sender, EventArgs e)
        {
            ((Image)sender).Opacity = 1;
        }


    }
}
