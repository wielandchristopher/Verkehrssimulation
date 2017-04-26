using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Verkehrssimulation.GUI;
using Verkehrssimulation.Verkehrsnetz;

namespace Verkehrssimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer dispatchTimer;
        private ObjectHandler oh;
        public MainWindow()
        {
            InitializeComponent();

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += dispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 150);


            oh = new ObjectHandler(myCanvas);
            //  public Testobject(double x,double y, int cardirection, int newid)  
            oh.addObject(new Testobject(250, 250,1,25));
            oh.addObject(new Testobject(270, 180,2,26));
            oh.addObject(new Testobject(300, 150,3,27));
            oh.addObject(new Testobject(100, 340,4,28));

            EnvironmentBuilder builder = new EnvironmentBuilder();

            
            dispatchTimer.Start();

        }

        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            oh.UpdateAll(); //update aller elemente (um 5 verschieben je nach direction)
            oh.UpdateID(28); // update des Testobjectes mit der id 28 für vergleich
            oh.UpdateID(28); // update des Testobjectes mit der id 28 für vergleich
            oh.UpdateID(28); // update des Testobjectes mit der id 28 für vergleich
        }
    }
}
