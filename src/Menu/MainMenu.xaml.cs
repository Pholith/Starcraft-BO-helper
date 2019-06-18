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

        private void AddBo(object sender, RoutedEventArgs e)
        {
            BuildOrder buildedBO = BuildOrder.CreateBO(BoInput.Text);
            if (buildedBO != null)
            {
                BuildOrder.SaveBO(buildedBO);
                labelAddBO.Content = "BO saved to \"" + buildedBO.ToPath() +"\"";
            } else
            {
                labelAddBO.Content = "BO has wrong format";
            }
        }

        private void SettingsButton(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new Settings());
        }
    }
}
