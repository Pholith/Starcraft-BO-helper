using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private MainMenu mainMenu;
        private Select selectMenu;

        public MainWindow()
        {
            // initialize the MainWindow
            InitializeComponent();

            // initialize the others windows
            mainMenu = new MainMenu();
            selectMenu = new Select();

            //page switcher and initial page   
            Switcher.pageSwitcher = this;
            Switcher.switchPage(mainMenu);  
        }


        // method called by the static Switcher to navigate between pages in this window
        internal void navigate(Page newPage)
        {
            Content = newPage.Content;
        }
    }
}
