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
            if(listOfActions.Count < 1)
            {
                feedbackTextBlock.Text = "You need at least one action";
                return;
            }


            try {
                BuildOrder.CreateAndSaveBO(nameBox.Text, typeBox.Text, boCommentBox.Text, matchupBox.Text, listOfActions);
                feedbackTextBlock.Text = "BO successfully added";
            }
            catch(Exception ex)
            {

                feedbackTextBlock.Text = ex.Message;
            }
 
            
        }

        private void addAction(object sender, RoutedEventArgs e)
        {
            try
            { 
            Action action = Action.CreateAction(timeBox.Text, actionBox.Text, actionCommentBox.Text);
            listOfActions.Add(action);
            actionsBOPreview.Items.Add(action);
            feedbackTextBlock.Text = "";
            }
            catch(Exception ex)
            {
                feedbackTextBlock.Text = ex.Message;
            }
            
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

        private void ModifySelectedAction(object sender, RoutedEventArgs e)
        {

        }
    }
}
