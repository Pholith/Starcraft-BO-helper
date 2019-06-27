﻿using System;
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
            selectedBOPreview.Items.Clear();
           
            BuildOrder preview = (BuildOrder) GetSelectedList().SelectedItem;
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

        private void SelectedBOPreview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
