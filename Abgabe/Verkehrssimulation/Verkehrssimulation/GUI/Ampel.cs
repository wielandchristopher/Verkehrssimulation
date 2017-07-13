using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Verkehrssimulation.GUI
{
    class Ampel : IVerkehrsnetz
    {
        int id;
        Shape shp;
        Shape redCircle;
        Shape yellowCircle;
        Shape greenCircle;
        public bool green, yellow, red;
        int posx, posy;
        int dir; // 1 = vertical, 2 = horizontal, 3 = horizontal swapped, 3 = vertical swapped

        public int objid { get { return id; } set { id = value; } }
        public int xpos { get { return posx; } set { posx = value; } }
        public int ypos { get { return posy; } set { posy = value; } }
        public int objdir { get { return dir; } set { dir = value; } }
        public Shape objshp { get { return shp; } set { shp = value; } }
        public Shape greenLight { get { return greenCircle; } set { greenCircle = value; } }
        public Shape yellowLight { get { return yellowCircle; } set { yellowCircle = value; } }
        public Shape redLight { get { return redCircle; } set { redCircle = value; } }

        public Ampel(double x, double y, int dir, int testId)
        {
            //
            id = testId;

            // grün
            greenCircle = new Ellipse();
            greenCircle.Width = 8;
            greenCircle.Height = 8;
            greenCircle.Fill = new SolidColorBrush(Colors.White);
            green = true;

            // grün
            yellowCircle = new Ellipse();
            yellowCircle.Width = 8;
            yellowCircle.Height = 8;
            yellowCircle.Fill = new SolidColorBrush(Colors.Yellow);
            yellow = true;

            // rot
            redCircle = new Ellipse();
            redCircle.Width = 8;
            redCircle.Height = 8;
            redCircle.Fill = new SolidColorBrush(Colors.White);
            red = false;

            // shape
            shp = new Rectangle();

            shp.Fill = new SolidColorBrush(Colors.Black);
            Canvas.SetTop(shp, x);
            Canvas.SetLeft(shp, y);

            if (dir == 1)
            {
                Canvas.SetTop(greenCircle, x + 22);
                Canvas.SetLeft(greenCircle, y + 1);

                Canvas.SetTop(yellowCircle, x + 11);
                Canvas.SetLeft(yellowCircle, y + 1);

                Canvas.SetTop(redCircle, x + 1);
                Canvas.SetLeft(redCircle, y + 1);

                shp.Width = 10;
                shp.Height = 33;
            }

            if (dir == 2)
            {
                Canvas.SetTop(greenCircle, x + 1);
                Canvas.SetLeft(greenCircle, y + 22);

                Canvas.SetTop(yellowCircle, x + 1);
                Canvas.SetLeft(yellowCircle, y + 11);

                Canvas.SetTop(redCircle, x + 1);
                Canvas.SetLeft(redCircle, y + 1);

                shp.Width = 33;
                shp.Height = 10;
            }


            if (dir == 3)
            {
                Canvas.SetTop(greenCircle, x + 1);
                Canvas.SetLeft(greenCircle, y + 1);

                Canvas.SetTop(yellowCircle, x + 11);
                Canvas.SetLeft(yellowCircle, y + 1);

                Canvas.SetTop(redCircle, x + 22);
                Canvas.SetLeft(redCircle, y + 1);

                shp.Width = 10;
                shp.Height = 33;
            }

            if (dir == 4)
            {
                Canvas.SetTop(greenCircle, x + 1);
                Canvas.SetLeft(greenCircle, y + 1);

                Canvas.SetTop(yellowCircle, x + 1);
                Canvas.SetLeft(yellowCircle, y + 11);

                Canvas.SetTop(redCircle, x + 1);
                Canvas.SetLeft(redCircle, y + 22);

                shp.Width = 33;
                shp.Height = 10;
            }
        }

        public Shape getShape()
        {
            return this.shp;
        }

        public Shape getCircleGreen()
        {
            return this.greenCircle;
        }

        public Shape getCircleYellow()
        {
            return this.yellowCircle;
        }

        public Shape getCircleRed()
        {
            return this.redCircle;
        }
    }
}
