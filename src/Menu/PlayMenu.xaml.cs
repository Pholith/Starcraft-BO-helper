using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour PlayMenu.xaml
    /// </summary>
    public partial class PlayMenu : Page
    {


        public PlayMenu(BuildOrder bo)
        {
            InitializeComponent(/*BuildOrder bo*/);

            // Starting stopwatch

            BuildOrderReader reader = new BuildOrderReader(bo, timerLabel);

        }

        private void BackSelectMenu(object sender, RoutedEventArgs e)
        {
            Switcher.switchPage(new Select());
        }
    }
}
