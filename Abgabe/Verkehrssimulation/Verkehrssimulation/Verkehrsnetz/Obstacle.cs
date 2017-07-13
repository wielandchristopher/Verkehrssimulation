using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    /// <summary>
    /// Obstacle Elemente für den Obstaclehandler
    /// </summary>
    public class Obstacle
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }

        public Obstacle(int startx, int starty, int endx, int endy)
        {
            this.StartX = startx;
            this.StartY = starty;
            this.EndX = endx;
            this.EndY = endy;
        }
    }

    public class ObstacleHandler
    {
        private List<Obstacle> obstacles;

        public ObstacleHandler()
        {
            obstacles = new List<Obstacle>();
        }

        public void addObstacle(Obstacle obs)
        {
            obstacles.Add(obs);
        }

        public bool inArea(int x, int y, Obstacle obs)
        {
            if (x > obs.StartX && x < obs.EndX && y > obs.StartY && y < obs.EndY)
            {
                return true;
            }
            return false;
        }

        public bool checkObstacles(int x, int y)
        {
            foreach (Obstacle obs in obstacles)
            {
                if (inArea(x, y, obs))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Obstacle> getObstacles()
        {
            return this.obstacles;
        }
    }
}
