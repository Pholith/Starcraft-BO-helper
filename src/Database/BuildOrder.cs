using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Starcraft_BO_helper
{
    public class BuildOrder
    {

        public string Name { get; private set; }
        private readonly string race;

        private List<Action> listOfAction;
        internal List<Action> ListOfAction
        {
            get
            {
                // Remove workers from list if don't not workers option
                if (!Db.Instance.showWorkers)
                {
                    var listCopy = listOfAction;
                    listCopy.RemoveAll(a => a.IsWorker());
                    return listCopy;
                }
                return listOfAction;
            }
        }

        // A BuildOrder represent a sequence of action at a exact time
        private BuildOrder(string name, string race, List<Action> listOfAction)
        {
            race = race.ClearWhiteSpace();
            // Throw ArgumentException if race string is not valide
            if (!Race.Races.Contains(race.ToLower()))
            {
                throw new ArgumentException("Can't Parse the BO. Don't reconnizing the race.");
            }
            this.race = race;
            this.Name = name.ClearWhiteSpace();
            this.listOfAction = listOfAction;

        }
        // Transform a local path into a True local Path
        private static string FixPathToRelative(string localPath)
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + localPath)));
            return directory.ToString();
        }

        // Read All bo file in the ressource dir "save_bo/"
        public static HashSet<BuildOrder> ReadAllBO()
        {

            HashSet<BuildOrder> set = new HashSet<BuildOrder>();
            foreach (string file in Directory.EnumerateFiles(FixPathToRelative("src/saves_bo/"), "*.bo"))
            {
                set.Add(ReadBO(file));
            }
            return set;
        }

        // Remove a BO from the file system
        internal static void DeleteBO(BuildOrder selectedItem)
        {
            File.Delete(FixPathToRelative(selectedItem.ToPath()));
        }

        // create a object BO from it string format
        // Return null if bad format
        public static BuildOrder CreateBO(string bo)
        {
            BuildOrder trueBO;
            try
            {
                string[] lines = bo.Split('\n');
                IEnumerable<string> actionLines = lines.Skip(2);
                List<Action> actions = new List<Action>();

                foreach (string actionLine in actionLines)
                {
                    // Pass if the line is empty
                    if (string.IsNullOrWhiteSpace(actionLine))
                    {
                        continue;
                    }
                    actions.Add(Action.ReadActionLine(actionLine));
                }
                trueBO = new BuildOrder(lines[0], lines[1], actions);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during creation of BO. " + e.ToString());
                return null;
            }
            return trueBO;
        }

        // Read a BO file
        public static BuildOrder ReadBO(string pathSrc)
        {
            if (!File.Exists(pathSrc))
            {
                Console.WriteLine("No file found.");
                return null;
            }

            BuildOrder bo;
            // Read file using StreamReader. Reads file line by line  
            using (StreamReader file = new StreamReader(pathSrc))
            {
                int counter = 0;
                string ln;

                string race = null;
                string name = null;
                List<Action> actions = new List<Action>();


                // Reading line by line
                while ((ln = file.ReadLine()) != null)
                {
                    if (counter == 0)
                    {
                        name = ln;
                    }
                    else if (counter == 1)
                    {
                        race = ln;
                    }
                    else
                    {
                        // Pass if the line is empty
                        if (string.IsNullOrWhiteSpace(ln))
                        {
                            continue;
                        }
                        actions.Add(Action.ReadActionLine(ln));
                    }
                    counter++;
                }
                name = Path.GetFileNameWithoutExtension(pathSrc);
                bo = new BuildOrder(name, race, actions);

                file.Close();
                Console.WriteLine($"File has {counter} lines.");
            }
            return bo;

        }

        // Save a BO file
        public static void SaveBO(BuildOrder bo)
        {
            string path = FixPathToRelative(bo.ToPath());
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(bo.ToFormat());
            }
        }

        // Return the BO with string format
        public string ToFormat()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name).AppendLine();
            builder.Append(race).AppendLine();
            foreach (Action action in listOfAction)
            {
                builder.Append(action).AppendLine();
            }

            return builder.ToString();
        }

        public bool IsTerran()
        {
            return race.ToLower() == Race.Terran;
        }

        public bool IsZerg()
        {
            return race.ToLower() == Race.Zerg;
        }

        public bool IsProtoss()
        {
            return race.ToLower() == Race.Protoss;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(race).Append(": ");
            builder.Append(Name);

            return builder.ToString();
        }
        // Return the relative path of the BO save
        public string ToPath()
        {
            return string.Concat("src/saves_bo/", Name, ".bo");
        }
    }

    // Enum for race
    internal static class Race
    {
        public const string Protoss = "protoss";
        public const string Terran = "terran";
        public const string Zerg = "zerg";
        public static readonly string[] Races = { Protoss, Terran, Zerg };
    }


}
