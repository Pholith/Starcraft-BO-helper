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
        private void selectMenu(object sender, RoutedEventArgs e)
        {
            Select selectPage = new Select();
            this.navigate(selectPage);
        }

        private void addBo(object sender, RoutedEventArgs e)
        {
            BuildOrder buildedBO = BuildOrder.createBO(BoInput.Text);
            if (buildedBO != null)
            {
                BuildOrder.saveBO(buildedBO);
                labelAddBO.Content = "BO saved to \"" + buildedBO.toPath() +"\"";
            } else
            {
                labelAddBO.Content = "BO has wrong format";
            }
        }

    }
}
