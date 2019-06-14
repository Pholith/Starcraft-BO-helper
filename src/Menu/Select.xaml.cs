using System;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Logique d'interaction pour Select.xaml
    /// </summary>
    public partial class Select : Page
    {
        public Select()
        {
            InitializeComponent();
            var boList = BuildOrder.readAllBO();

            // Global list
            foreach (var bo in boList)
            {
                this.allList.Items.Add(bo);
            }

            // RaceList
            foreach (var bo in boList)
            {
                if (bo.isTerran())
                {
                    terranList.Items.Add(bo);
                }
                if (bo.isZerg())
                {
                    zergList.Items.Add(bo);
                }
                if (bo.isProtoss())
                {
                    protossList.Items.Add(bo);
                }
            }
        }

        private void playSelected(object sender, RoutedEventArgs e)
        {
            Switcher.switchPage(new PlayMenu());
        }

        private void deleteSelected(object sender, RoutedEventArgs e)
        {
            BuildOrder selectedItem = (BuildOrder) getSelectedList().SelectedItem;
            if (selectedItem != null)
            {
                BuildOrder.deleteBO(selectedItem);

                // Remove element from lists
                allList.Items.Remove(selectedItem);
                terranList.Items.Remove(selectedItem);
                protossList.Items.Remove(selectedItem);
                zergList.Items.Remove(selectedItem);
            }
        }

        private ListBox getSelectedList()
        {
            return (ListBox) tabControl.SelectedContent;
        }

        private void backMenu(object sender, RoutedEventArgs e)
        {

        }
    }
}
