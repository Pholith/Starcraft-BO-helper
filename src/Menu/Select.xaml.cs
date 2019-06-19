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
            var boList = BuildOrder.ReadAllBO();

            // Global list
            foreach (var bo in boList)
            {
                if (bo != null)
                {
                    this.allList.Items.Add(bo);
                }
            }

            // RaceList
            foreach (var bo in boList)
            {
                if (bo != null)
                {

                    if (bo.IsTerran())
                    {
                        terranList.Items.Add(bo);
                    }
                    if (bo.IsZerg())
                    {
                        zergList.Items.Add(bo);
                    }
                    if (bo.IsProtoss())
                    {
                        protossList.Items.Add(bo);
                    }
                }
            }
        }

        private void PlaySelected(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new PlayMenu((BuildOrder) GetSelectedList().SelectedItem));
        }

        private void DeleteSelected(object sender, RoutedEventArgs e)
        {
            BuildOrder selectedItem = (BuildOrder) GetSelectedList().SelectedItem;
            if (selectedItem != null)
            {
                BuildOrder.DeleteBO(selectedItem);

                // Remove element from lists
                allList.Items.Remove(selectedItem);
                terranList.Items.Remove(selectedItem);
                protossList.Items.Remove(selectedItem);
                zergList.Items.Remove(selectedItem);
            }
        }

        private ListBox GetSelectedList()
        {
            return (ListBox) tabControl.SelectedContent;
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new MainMenu());
        }

        private void UpdateSelection(object sender, SelectionChangedEventArgs e)
        {
            if (GetSelectedList().SelectedItem == null)
            {
                playButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                return;
            }
            playButton.IsEnabled = true;
            deleteButton.IsEnabled = true;

            // BO preview
            selectedBO.Items.Clear();

           
            BuildOrder preview = (BuildOrder) GetSelectedList().SelectedItem;
            boName.Content = preview.Name;
            List <Action> actions = preview.ListOfAction;
            foreach (Action action in actions)
            {
                selectedBO.Items.Add(action.ToString());
            }
        }
    }
}
