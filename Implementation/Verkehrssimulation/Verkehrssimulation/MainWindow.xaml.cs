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
        private DispatcherTimer dispatchTimer;
        private ObjectHandler oh;
        private AmpelHandler ap;
        public MainWindow()
        {
            InitializeComponent();

            dispatchTimer = new DispatcherTimer();
            dispatchTimer.Tick += dispatchTimer_Tick;
            dispatchTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            EnvironmentBuilder builder = new EnvironmentBuilder(myCanvas);
            EnvironmentHandler envhandler = new EnvironmentHandler();
            TrafficHandler traffichandler = new TrafficHandler(ref envhandler);
            
            oh = new ObjectHandler(myCanvas);
            ap = new AmpelHandler(myCanvas);

            // Ampel Kreuzung unten
            for (int i = 60; i < 700; i += 100)
            {
                for (int y = 60; y < 700; y += 100)
                {
                    ap.addAmpel(new Ampel(i, y, 1));
                }
            }

            // Ampel Kreuzung rechts
            for (int i = 30; i < 700; i += 100)
            {
                for (int y = 60; y < 700; y += 100)
                {
                    ap.addAmpel(new Ampel(i, y, 2));
                }
            }

            // Ampel Kreuzung oben
            for (int i = 18; i < 700; i += 100)
            {
                for (int y = 30; y < 700; y += 100)
                {
                    ap.addAmpel(new Ampel(i, y, 3));
                }
            }
            
            // Ampel Kreuzung links
            for (int i = 60; i < 700; i += 100)
            {
                for (int y = 18; y < 700; y += 100)
                {
                    ap.addAmpel(new Ampel(i, y, 4));
                }
            }

            dispatchTimer.Start();

        }

        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            oh.UpdateAll(); //update aller elemente (um 5 verschieben je nach direction)
        }


    }
}
