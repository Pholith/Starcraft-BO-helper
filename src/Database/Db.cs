using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Input;

namespace Starcraft_BO_helper
{
    [Serializable]
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
        protected int skin = 0;
        public int Skin
        {
            get
            {
                return skin;
            }
            set
            {
                skin = value;
                skinManager.ChangeColor(skin);
                skinManager.UpdateWindowSkin();
            }
        }

        // Managers auto instanciate when needed
        [NonSerialized]
        private SkinManager skinManager;
        public SkinManager SkinManager
        {
            get
            {
                if (skinManager == null)
                {
                    skinManager = new SkinManager(skin);
                }
                return skinManager;
            }
        }

        // Constructors
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
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader file = new StreamReader(filePath))
                    {
                        IFormatter formatter = new BinaryFormatter();

                        _instance = (Db)formatter.Deserialize(file.BaseStream);

                        Console.WriteLine("Database loaded from file.");
                    }
                }
                else
                {
                    File.Delete(filePath);
                    throw new IOException();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Database file not found or old version. Creating a new Database.");
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

                formatter.Serialize(file.BaseStream, Instance);
            }
            Console.WriteLine("Successfully saved database.");
        }

    }
}
