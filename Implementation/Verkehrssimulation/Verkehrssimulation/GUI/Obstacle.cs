﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    class Obstacle
    {
        Shape shp;
        int posx, posy;
        int id;

        public Obstacle(double x, double y, int newId)
        {
            shp = new Rectangle();
            shp.Width = 5;
            shp.Height = 5;
            shp.Fill = new SolidColorBrush(Colors.Red);

            id = newId;
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);
        }

        public Shape getShape()
        {
            return this.shp;
        }
    }
}




