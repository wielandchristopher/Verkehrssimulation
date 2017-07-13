using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    interface I_ENV_VKTeilnehmer
    {
        /// <summary>
        /// Gibt die Straßeninformation zurück
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        StreetInfo getNeededStreetRules(int x, int y);

        /// <summary>
        /// Gibt die Obstacle Liste zurück
        /// </summary>
        /// <returns></returns>
        List<Obstacle> getObstacles();

        /// <summary>
        /// Gibt die Entrypoints zurück (für das spawnen der Farhzeuge)
        /// </summary>
        /// <returns></returns>
        List<EntryPoint> getEnvironmentEntries();

        /// <summary>
        /// Gibt Typen der Straße zurück (opt)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int getNeededEnvironmentRules(int x, int y);

        /// <summary>
        /// (optional) - gibt zurück ob sich ein Hindernis in diesem bereich befindet
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool isObstacleInMyWay(int x, int y);

        /// <summary>
        /// (optional) - gibt zurück ob sich ein Auto mit der übergebenen Position aus dem Spielfeld bewegt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        bool isOutside(int x, int y);
    }
}
