using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Starcraft_BO_helper
{
    // Interface of navigable page
    interface INavigable
    {
        void navigate(Page nextPage);
    }

    // Add a navigation method to Page class (impossible to made a abstract class)
    public static class PageNavigationExtension
    {
        // Return cleared string of whiteSpace not including single space
        public static void navigate(this Page page, Page page2)
        {
            page.Content = page2.Content;
        }
    }

}
