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

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour BuildOrderMenu.xaml
    /// </summary>
    public partial class BuildOrderMenu : Page
    {
        public BuildOrderMenu()
        {
            InitializeComponent();
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new MainMenu());
        }

    }
}
