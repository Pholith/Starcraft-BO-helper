using System;
using System.Collections.Generic;
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

        private List<Key> keyPressed = new List<Key>();

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (!keyPressed.Contains(e.Key))
            {
                keyPressed.Add(e.Key);
            }
            UpdateTextBox();
        }

        private void KeyUnpressed(object sender, KeyEventArgs e)
        {
            keyPressed.Remove(e.Key);
            UpdateTextBox();
        }

        private void UpdateTextBox()
        {
            string str = "";
            for (int i = 0; i < keyPressed.Count(); i++)
            {
                str += keyPressed[i].ToString();
                if (! (i == keyPressed.Count() - 1))
                {
                    str += "+";
                }
            }
            textBox.Text = str;
        }

        private void UpdateTextBox(object sender, TextChangedEventArgs e)
        {
            UpdateTextBox();

        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            keyPressed.Clear();
        }
    }
}
