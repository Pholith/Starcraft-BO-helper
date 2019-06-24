using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Resources;

namespace Starcraft_BO_helper
{
    public class Db
    {

        // Template of a Singleton
        private static Db _instance;
        static readonly object instanceLock = new object();

        private static readonly string fileName = "settings.bin";
        public static Db Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                            _instance = new Db();
                    }
                }
                return _instance;
            }
        }
        // End template of a Singleton


        // Settings with initials value
        public List<Key> skipKey = new List<Key>() { Key.LeftCtrl, Key.D };
        public bool onlySkipMode = false;
        public bool showWorkers = true;


        private Db(List<Key> skipKey, bool onlySkipMode, bool showWorkers)
        {
            this.skipKey = skipKey;
            this.onlySkipMode = onlySkipMode;
            this.showWorkers = showWorkers;
        }
        private Db()
        {

        }

        // Load the settings from the file
        public static void Load()
        {
            // Get the executable directory 
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string filePath = Path.Combine(directory, fileName);

            // If File exists, read, else create a empty database
            if (File.Exists(filePath))
            {
                using (StreamReader file = new StreamReader(filePath))
                {
                    IFormatter formatter = new BinaryFormatter();
                    List<Key> skipKey = (List<Key>) formatter.Deserialize(file.BaseStream);
                    bool onlySkipMode = (bool) formatter.Deserialize(file.BaseStream);
                    bool showWorkers = (bool) formatter.Deserialize(file.BaseStream);
                    Console.WriteLine("Database loaded from file.");
                    _instance = new Db(skipKey, onlySkipMode, showWorkers);
                }
            } else
            {
                Console.WriteLine("Database file not found. Creating a new Database.");
                _instance = new Db();
            }
        }
        // Save the settings into the file
        public static void Save()
        {
            // Get the executable directory 
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string filePath = Path.Combine(directory, fileName);

            using (StreamWriter file = new StreamWriter(filePath))
            {
                IFormatter formatter = new BinaryFormatter();

                formatter.Serialize(file.BaseStream, Instance.skipKey);
                formatter.Serialize(file.BaseStream, Instance.onlySkipMode);
                formatter.Serialize(file.BaseStream, Instance.showWorkers);
            }
            Console.WriteLine("Successfully saved database.");
        }
    }
}
