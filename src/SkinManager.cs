using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Starcraft_BO_helper
{
    public class SkinManager
    {
        private int skin;

        public SkinManager(int skin)
        {
            Console.WriteLine("Created Skin Manager.");
            this.skin = skin;
            UpdateWindowSkin();
        }
        // Change the skin color
        public void ChangeColor(int skin)
        {
            this.skin = skin;
            UpdateWindowSkin();
        }

        // Update the content color
        public void UpdateWindowSkin()
        {
            Panel panel = (Panel)Application.Current.MainWindow.Content;

            List<Color> colorList = Skin.GetColorList(skin);

            Application.Current.MainWindow.Background = new SolidColorBrush(colorList[2]);

            // Change color of each type of element
            var itemList = panel.FindAllChildren();

            foreach (var control in itemList)
            {
                if (control is Control)
                {
                    //Console.WriteLine(control.ToString());
                    ((Control)control).Foreground = new SolidColorBrush(colorList[0]);
                    if (!(control is Label))
                    {
                        ((Control)control).Background = new SolidColorBrush(colorList[1]);
                    }
                }

                if (control is ListBox)
                {
                    ((ListBox)control).Background = new SolidColorBrush(colorList[1]);
                }
                else if (control is Button)
                {
                    ((Button)control).Background = new SolidColorBrush(colorList[1]);

                }
                else if (control is TabControl tabControl)
                {
                    tabControl.Background = new SolidColorBrush(colorList[1]);

                    // TabItem are not in the list so we change them here
                    foreach (TabItem item in tabControl.Items)
                    {
                        if (item.Content is ListBox listBox)
                        {
                            listBox.Foreground = new SolidColorBrush(colorList[0]);
                        }
                    }
                }
                else if (control is TextBlock)
                {
                    ((TextBlock)control).Foreground = new SolidColorBrush(colorList[0]);

                }
                else if (control is ListBoxItem)
                {
                    ((ListBoxItem)control).Foreground = new SolidColorBrush(colorList[0]);

                }
                else if (control is ComboBox comboBox)
                {
                    // ComboBoxItem are not in the list so we change them here
                    foreach (ComboBoxItem item in comboBox.Items)
                    {
                        item.Foreground = new SolidColorBrush(colorList[0]);
                        item.Background = new SolidColorBrush(colorList[1]);
                    }
                    comboBox.Background = new SolidColorBrush(colorList[1]);
                    
                }
            }
        }
    }

    // Contain the skin list and the skin's color
    class Skin
    {
        public static List<Color> GetColorList(int skin)
        {
            List<Color> colorList = new List<Color>();
            ///
            /// Foreground color
            /// others colors

            switch (skin)
            {
                case Terran:
                    colorList.Add((Color) ColorConverter.ConvertFromString("#FFFFFF"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#3546B0"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#2334A3"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#1D2B7E"));
                    break;
                case Zerg:
                    colorList.Add((Color) ColorConverter.ConvertFromString("#FFFFFF"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#8328AA"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#751B9D"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#5F167E"));
                    break;
                case Protoss:
                    colorList.Add((Color) ColorConverter.ConvertFromString("#000000"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#fff570"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#e8de4d"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#d9ca00"));
                    break;
                case Light:
                    colorList.Add((Color) ColorConverter.ConvertFromString("#000000"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#CDCDCD"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#B4B4B4"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#9C9B9B"));
                    break;
                case Dark:
                    colorList.Add((Color) ColorConverter.ConvertFromString("#FFFFFF"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#616161"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#424141"));
                    colorList.Add((Color) ColorConverter.ConvertFromString("#1D1D1D"));
                    break;
                default:
                    throw new ArgumentException();
            }
            return colorList;
        }

        public const int Light = 0;
        public const int Dark = 1;

        public const int Terran = 2;
        public const int Zerg = 3;
        public const int Protoss = 4;
    }
}
