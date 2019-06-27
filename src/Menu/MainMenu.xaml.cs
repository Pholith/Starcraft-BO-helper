using System.Windows;
using System.Windows.Controls;

namespace Starcraft_BO_helper
{
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        private void SelectMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new Select());
        }


        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new Settings());
        }

        private void CreateBOClick(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new BuildOrderMenu());
        }
    }
}
