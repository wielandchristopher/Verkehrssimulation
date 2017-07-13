using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    interface I_ENV_GUI
    {
        /// <summary>
        /// Fügt über die GUI ein obstacle hinzu
        /// </summary>
        /// <param name="startx"></param>
        /// <param name="starty"></param>
        /// <param name="endx"></param>
        /// <param name="endy"></param>
        void addObstacle(int startx, int starty, int endx, int endy);

        /// <summary>
        /// Schaltet über die GUI die ampelanlagen ein/aus
        /// </summary>
        /// <param name="val"></param>
        void setAmpelanlage(bool val);
    }
}
