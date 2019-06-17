using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Starcraft_BO_helper
{

    public static class Switcher
    {
        public static MainWindow pageSwitcher;

        // Change the content of the MainWindow to show a other page
        public static void SwitchPage(Page newPage)
        {
            pageSwitcher.Navigate(newPage);
        }
    }

}