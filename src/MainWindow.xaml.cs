using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

        public MainWindow()
        {
            // initialize the MainWindow
            InitializeComponent();

            // initialize the others windows
            mainMenu = new MainMenu();

            //page switcher and initial page   
            Switcher.window = this;
            Switcher.SwitchPage(mainMenu);  
        }


        // method called by the static Switcher to navigate between pages in this window
        internal void Navigate(Page newPage)
        {
            Content = newPage.Content;
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
        }
    }
}
