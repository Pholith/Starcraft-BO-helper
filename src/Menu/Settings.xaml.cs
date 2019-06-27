using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {

        public Settings()
        {
            InitializeComponent();
        }
        private void BackSelectMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new MainMenu());
        }

        private void OnAction()
        {
            Db.Save();
        }

        private new void Loaded(object sender, RoutedEventArgs e)
        {
            // Load the textBox
            keyPressed = Db.Instance.skipKey;
            UpdateTextBox();

        }


        // Textbox
        private List<Key> keyPressed = new List<Key>();

        private void KeyPressed(object sender, KeyEventArgs e)
        {

            var keyPressedCopy = keyPressed;
            // Remove all key that are Alphanumeric
            keyPressedCopy.RemoveAll(k =>
                (k >= Key.A && k <= Key.Z) || (k >= Key.D0 && k <= Key.D9) || (k >= Key.NumPad0 && k <= Key.NumPad9));

            
            if (keyPressedCopy.Count() > 1)
            {
                // Don't add the Key
            }
            else if (!keyPressed.Contains(e.Key))
            {
                keyPressed.Add(e.Key);
            }
            UpdateTextBox();
        }

        private void KeyUnpressed(object sender, KeyEventArgs e)
        {
            Db.Instance.skipKey = keyPressed;
            UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            // ToString
            string str = "";
            for (int i = 0; i < keyPressed.Count(); i++)
            {
                str += keyPressed[i].ToString();
                if (!(i == keyPressed.Count() - 1))
                {
                    str += "+";
                }
            }
            textBox.Text = str;
            OnAction();
        }

        private void UpdateTextBox(object sender, TextChangedEventArgs e)
        {
            UpdateTextBox();
        }
        private void TextBox_GotFocus(object sender, MouseEventArgs e)
        {
            keyPressed.Clear();
            UpdateTextBox();
        }

        // SkipMode Checkbox control
        private void SkipBoxLoad(object sender, RoutedEventArgs e)
        {
            onlySkipModeCheckbox.IsChecked = Db.Instance.onlySkipMode;
        }

        private void OnlySkipModeChecked(object sender, RoutedEventArgs e)
        {
            if (showWorkersCheckbox.IsLoaded)
            {
                Db.Instance.onlySkipMode = true;
                OnAction();
            }
        }
        private void OnlySkipModeUnchecked(object sender, RoutedEventArgs e)
        {
            if (showWorkersCheckbox.IsLoaded)
            {
                Db.Instance.onlySkipMode = false;
                OnAction();
            }
        }

        // Worker checkbox control
        private void WorkerLewd(object sender, RoutedEventArgs e)
        {
            showWorkersCheckbox.IsChecked = Db.Instance.showWorkers;
        }

        private void ShowWorkersChecked(object sender, RoutedEventArgs e)
        {
            if (showWorkersCheckbox.IsLoaded)
            {
                Db.Instance.showWorkers = true;
                OnAction();
            }
        }
        private void ShowWorkersUnchecked(object sender, RoutedEventArgs e)
        {
            if (showWorkersCheckbox.IsLoaded)
            {
                Db.Instance.showWorkers = false;
                OnAction();
            }
        }

        private void Color_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (colorSelectBox.IsLoaded)
            {
                Db.Instance.Skin = colorSelectBox.SelectedIndex;
                OnAction();
            }
        }
        
        private void ColorSelectLoaded(object sender, RoutedEventArgs e)
        {
            colorSelectBox.SelectedIndex = Db.Instance.Skin;
        }

    }
}
