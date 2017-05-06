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
using Verkehrssimulation.Verkehrsregeln;
using Verkehrssimulation.Verkehrsteilnehmer;

namespace Verkehrssimulation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatchTimer, dpTimer2;
        private ObjectHandler oh;
        private AmpelHandler ap;
        public MainWindow()
        {
            InitializeComponent();

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += dispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            // Test mit blinkender Ampel
            dpTimer2 = new DispatcherTimer();
            dpTimer2.Tick += dpTimer2_Tick;
            dpTimer2.Interval = new TimeSpan(0, 0, 0, 0, 1000);

            EnvironmentBuilder builder = new EnvironmentBuilder(myCanvas);
            EnvironmentHandler envhandler = new EnvironmentHandler();
            TrafficHandler traffichandler = new TrafficHandler(ref envhandler);
            
            oh = new ObjectHandler(myCanvas);
            ap = new AmpelHandler(myCanvas);

            dispatchTimer.Start();
            dpTimer2.Start();

        }

        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            //oh.UpdateAll(); //update aller elemente (um 5 verschieben je nach direction)
        }

        private void dpTimer2_Tick(object sender, EventArgs e)
        {
            //ap.blinky();
        }
    }
}
