﻿using System;
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
