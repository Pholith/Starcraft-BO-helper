using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            Switcher.SwitchPage(new PlayMenu(GetSelectedBO()));
        }

        private void DeleteSelected(object sender, RoutedEventArgs e)
        {
            BuildOrder selectedItem = GetSelectedBO();
            if (selectedItem != null)
            {
                BuildOrder.DeleteBO(selectedItem);

                // Remove element from lists
                allList.Items.Remove(selectedItem);
                terranList.Items.Remove(selectedItem);
                protossList.Items.Remove(selectedItem);
                zergList.Items.Remove(selectedItem);
                selectedBOPreview.Items.Clear();
            }
        }
        // Return the selected Buildorder From the selected list
        private BuildOrder GetSelectedBO()
        {
            return GetSelectedList().SelectedItem as BuildOrder;
        }
        // Return the selected list in the tab control
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
            if (GetSelectedBO() == null)
            {
                playButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
                modifyButton.IsEnabled = false;
                copyButton.IsEnabled = false;
                return;
            }
            playButton.IsEnabled = true;
            deleteButton.IsEnabled = true;
            modifyButton.IsEnabled = true;
            copyButton.IsEnabled = true;

            // BO preview
            selectedBOPreview.Items.Clear();
           
            BuildOrder preview = GetSelectedBO();
            boNamePreview.Content = preview.Name;
            foreach (var meta in preview.MetaDataToString)
            {
                selectedBOPreview.Items.Add(meta);
            }
            List<Action> actions = preview.ListOfAction;
            foreach (Action action in actions)
            {
                selectedBOPreview.Items.Add(action.ToString());
            }
        }

        private void modifyButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void CopyClipboardClic(object sender, RoutedEventArgs e)
        {
            BuildOrder selectedBo = GetSelectedBO();
            if (selectedBo == null)
            {
                // TODO Write a error message on a label
                return;
            }

            Clipboard.SetText(selectedBo.ToFormat());
            // TODO confirmation message in a label
        }
    }
}
