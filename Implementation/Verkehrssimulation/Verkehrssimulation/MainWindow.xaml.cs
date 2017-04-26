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
            oh.addStrasse(new Strasse(150, 150, 1));

            //  public Testobject(double x,double y, int cardirection, int newid)  
            oh.addObject(new Auto(250, 250,1,25));
            oh.addObject(new Auto(270, 180,2,26));
            oh.addObject(new Auto(150, 300,3,27));
            oh.addObject(new Auto(100, 340, 4, 28));

            EnvironmentBuilder builder = new EnvironmentBuilder();

            
            dispatchTimer.Start();

        }

        private void dispatchTimer_Tick(object sender, EventArgs e)
        {
            oh.UpdateAll(); //update aller elemente (um 5 verschieben je nach direction)
            oh.UpdateID(27); // update des Testobjectes mit der id 28 für vergleich
            oh.UpdateID(27); // update des Testobjectes mit der id 28 für vergleich
            oh.UpdateID(27); // update des Testobjectes mit der id 28 für vergleich
        }
    }
}
