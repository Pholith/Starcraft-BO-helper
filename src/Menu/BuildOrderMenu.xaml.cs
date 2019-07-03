using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Windows.Input;

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour BuildOrderMenu.xaml
    /// </summary>
    public partial class BuildOrderMenu : Page
    {
        public BuildOrderMenu()
        {
            InitializeComponent();
        }

        public BuildOrderMenu(BuildOrder bo)
        {
            InitializeComponent();
            FillBoxes(bo);
        }

        private void BackMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new MainMenu());
        }

        /*
         * Saving the BO
         * Does not save it if informations are missing or incorrect
         */
        private void Saving(object sender, RoutedEventArgs e)
        {
            if (actionsBOPreview.Items.Count < 1)
            {
                feedbackTextBlock.Text = "You need at least one action";
                return;
            }

            try
            {
                List<Action> castedList = actionsBOPreview.Items.OfType<Action>().ToList();

                Dictionary<string, string> metaDatas = new Dictionary<string, string>();
                metaDatas.Add("type", typeBox.Text);
                metaDatas.Add("matchup", matchupBox.Text);
                metaDatas.Add("author", authorBox.Text);
                metaDatas.Add("description", boCommentBox.Text);

                BuildOrder.CreateAndSaveBO(nameBox.Text, castedList, metaDatas);
                feedbackTextBlock.Text = "BO successfully added";
            }
            catch (Exception ex)
            {
                feedbackTextBlock.Text = ex.Message;
            }


        }

        private Action GetSelectedAction()
        {
            return actionsBOPreview.SelectedItem as Action;
        }

        private void ClearActionsTextBox()
        {
            timeBox.Text = "";
            actionBox.Text = "";
            actionCommentBox.Text = "";
        }
    
        private void addAction(object sender, RoutedEventArgs e)
        {
            try
            {
                Action action = Action.CreateAction(timeBox.Text, actionBox.Text, actionCommentBox.Text);
                actionsBOPreview.Items.Add(action);
                // Sort the items using the CompareTime property of Action
                actionsBOPreview.Items.SortDescriptions.Add(new SortDescription("CompareTime", ListSortDirection.Ascending));
                ClearActionsTextBox();
            }
            catch (Exception ex)
            {
                feedbackTextBlock.Text = ex.Message;
            }
        }

        private void removeSelectedAction(object sender, RoutedEventArgs e)
        {
            Action selectedAction = GetSelectedAction();

            if (selectedAction != null)
            {
                actionsBOPreview.Items.Remove(selectedAction);
                ClearActionsTextBox();
            }
        }

        private void ModifySelectedAction(object sender, RoutedEventArgs e)
        {
            Action selectedAction = GetSelectedAction();

            if(selectedAction != null)
            {
                
                addAction(sender, e);
                removeSelectedAction(sender, e);
            }

        }

        private void keyAddAction(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                addAction(sender, e);
            }
        }

        private void UpdateSelection(object sender, SelectionChangedEventArgs e)
        {
            Action selectedAction = GetSelectedAction();

            if (selectedAction != null)
            {
                timeBox.Text = selectedAction.Time.ToString(@"mm\:ss");
                actionBox.Text = selectedAction._Action;
                actionCommentBox.Text = selectedAction.Comment;
            }
        }

        private void importClipboardClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // TODO Salt encoding here
                BuildOrder bo = BuildOrder.CreateBOFromString(Clipboard.GetText());
                FillBoxes(bo);
                feedbackTextBlock.Text = "Sucessfully import bo";
            }
            catch (Exception ex)
            {
                feedbackTextBlock.Text = "There is no build order in Clipboard";
            }
        }

        // Fill boxes with the bo informations
        private void FillBoxes(BuildOrder bo)
        {
            nameBox.Text = bo.Name;
            matchupBox.Text = bo.MetaDatas["matchup"];
            authorBox.Text = bo.MetaDatas["author"];
            boCommentBox.Text = bo.MetaDatas["description"];
            typeBox.Text = bo.MetaDatas["type"];
            actionsBOPreview.Items.Clear();

            foreach (var action in bo.ListOfAction)
            {
                actionsBOPreview.Items.Add(action);
            }
        }

        private void ImportFileClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Build order file (*.bo)|*.bo|All files (*.*)|*.*";
                dialog.ShowDialog();

                // TODO replay ???????
                BuildOrder bo = BuildOrder.ReadBO(dialog.FileName);
                FillBoxes(bo);

                feedbackTextBlock.Text = "Sucessfully import bo";
            }
            catch (Exception)
            {
                feedbackTextBlock.Text = "File is not build order";
            }
        }
    }
}
