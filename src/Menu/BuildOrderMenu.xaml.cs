using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour BuildOrderMenu.xaml
    /// </summary>
    public partial class BuildOrderMenu : Page
    {
        private List<Action> listOfActions = new List<Action>();

        public BuildOrderMenu()
        {
            InitializeComponent();
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new MainMenu());
        }

        private void UpdatePreview()
        {
            if (actionsBOPreview.Items.Count < 1)
            {
                saveButton.IsEnabled = false;
                return;
            }
            saveButton.IsEnabled = true;
        }

        /*
         * Saving the BO
         * Does not save it if informations are missing or incorrect
         */
        private void Saving(object sender, RoutedEventArgs e)
        {
            // regex mathchup: (?(?=XvX)Nope|[TZP]v[TZPX]) -- Prend tous les matchUp sauf "XvX" ou qui commence par X
            // regex Type: (Cheese|All-In|Timing Attack|Economic|Co-op)  -- A check, pas optimal / mettre le case sensitive
            if(listOfActions.Count < 1)
            {
                feedbackLabel.Content = "You need at least one action";
                return;
            }


            if(BuildOrder.CreateAndSaveBO(nameBox.Text, typeBox.Text, boCommentBox.Text, matchupBox.Text, listOfActions))
            {
                feedbackLabel.Content = "BO successfully saved";
            }
            else
            {
                feedbackLabel.Content = "Save failed";
            }
            
        }

        // Add a 0 if the time format is m:ss
        private static string PreFormatTime(string time)
        {
            string[] splitedTime = Regex.Split(time, @"(\d{1,2}):(\d\d)");

            if (splitedTime[1].Count() == 1)
            {
                splitedTime[1] = String.Concat("0", splitedTime[1]);
            }
            return string.Concat(splitedTime[1], ":", splitedTime[2]);
        }

        private void addAction(object sender, RoutedEventArgs e)
        {
            if(actionBox.Text == "")
            {
                feedbackLabel.Content = "Add failed, invalid action";
                return;
            }

            if (timeBox.Text.Contains("Time") || actionBox.Text.Contains("Action") || actionCommentBox.Text.Contains("Optionnal description"))
            {
                feedbackLabel.Content = "Add failed, invalid step";
                return;
            }

            TimeSpan time = TimeSpan.ParseExact(PreFormatTime(timeBox.Text), "mm\\:ss", CultureInfo.InvariantCulture);
            Action action = new Action(time, actionBox.Text, actionCommentBox.Text);
            listOfActions.Add(action);
            actionsBOPreview.Items.Add(action);
            feedbackLabel.Content = "";
        }

        

        private void removeSelectedAction(object sender, RoutedEventArgs e)
        {
            Action selectedAction = (Action) actionsBOPreview.SelectedItem;

            if(selectedAction != null)
            {
                listOfActions.Remove(selectedAction);
                actionsBOPreview.Items.Remove(selectedAction);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //?
        }
    }
}
