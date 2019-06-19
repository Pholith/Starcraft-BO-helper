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

        private BuildOrderReader reader;

        public PlayMenu(BuildOrder bo)
        {
            InitializeComponent();

            reader = new BuildOrderReader(bo, this, timerLabel, titleLabel, previousLabel, actualLabel, nextLabel);

        }

        // Back Button
        private void BackSelectMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new Select());
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            reader.SkipAction(true);
        }

        // Bind the Key handlers at loading the page
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("UserControl_Loaded");
            Switcher.window.KeyDown += KeyPressed;
        }
        // UnBind the Key handlers at unloading the page (switching page)
        private void Unload(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("UserControl_Unloaded");
            Switcher.window.KeyDown -= KeyPressed;
        }
    }
}
