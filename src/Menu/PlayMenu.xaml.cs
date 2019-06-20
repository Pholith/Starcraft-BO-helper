using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;

namespace Starcraft_BO_helper
{
    /// <summary>
    /// Logique d'interaction pour PlayMenu.xaml
    /// </summary>
    public partial class PlayMenu : Page
    {

        private BuildOrderReader reader;

        KeyboardListener KListener = new KeyboardListener();

        public PlayMenu(BuildOrder bo)
        {
            InitializeComponent();

            reader = new BuildOrderReader(bo, this, timerLabel, titleLabel, previousLabel, actualLabel, nextLabel);

            Console.WriteLine("Starting Global Key listener...");
            KListener.KeyDown += new RawKeyEventHandler(GlobalKeyPressed);
            KListener.KeyUp += new RawKeyEventHandler(GlobalKeyUnpressed);


            /*
            Process[] processlist = Process.GetProcesses();
            Process sp = null;
            foreach (Process process in processlist)
            {
                //Console.WriteLine("Process: {0} ID: {1}", process.ProcessName, process.Id);
                if (process.ProcessName == "SC2_x64")
                {
                    sp = process;
                    Console.WriteLine(process.ProcessName);
                }
                if (process.Id == 7888)
                {
                    Console.WriteLine(process.StartTime);
                    Console.WriteLine("Process: {0} ID: {1}", process.ProcessName, process.Id);
                }
            } */
        }
        // Match globals key (using a other app like Starcraft)
        private List<Key> keyPressed = new List<Key>();

        private void GlobalKeyPressed(object sender, RawKeyEventArgs e)
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

            // Check Key List 
            if (Db.Instance.skipKey.All(keyPressed.Contains))
            {
                keyPressed.Clear();

                if (reader.Started())
                {
                    reader.SkipAction(true);
                }
                else
                {
                    reader.Start();
                }
            }
        }
        // Check if keypressed are the same as the settings, and skip the action
        private void GlobalKeyUnpressed(object sender, RawKeyEventArgs args)
        {
            keyPressed.Remove(args.Key);
        }

        // Back Button
        private void BackSelectMenu(object sender, RoutedEventArgs e)
        {
            Switcher.SwitchPage(new Select());
        }

        // UnBind the Key handlers at unloading the page (switching page)
        private void Unload(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("End Global key listener...");
            KListener.Dispose();
        }

        private void MouseEnterClic(object sender, MouseEventArgs e)
        {
            reader.Start();
        }
    }
}
