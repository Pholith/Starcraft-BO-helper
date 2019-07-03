using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // initialize the MainWindow
            InitializeComponent();

            Db.Load();
            var udp = new UDP_Controller();

            //page switcher and initial page   
            Switcher.window = this;
            Switcher.SwitchPage(new MainMenu());

            this.Topmost = true; // Always Bring the window on front

        }


        // method called by the static Switcher to navigate between pages in this window
        internal void Navigate(Page newPage)
        {
            Content = newPage.Content;
            Db.Instance.SkinManager.UpdateWindowSkin();
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
        }
    }
}
