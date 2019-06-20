using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Starcraft_BO_helper
{
    public class BuildOrder
    {

        public string Name { get; private set; }
        private readonly string type;
        private readonly string matchup;
        private readonly string description;
        private string race;

        private readonly List<Action> listOfAction;
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
        internal List<string> MetaDataToString
                {
            get
            {
                List<string> metas = new List<string>
                {
                    string.Concat("Race: ", Race.TotalName(race)),
                    string.Concat("Matchup: ", matchup),
                    string.Concat("Type: ", type),
                    ""
                };
                return metas;
            }
        }

        // A BuildOrder represent a sequence of action at a exact time
        private BuildOrder(string name, string type, string matchup, string description, List<Action> listOfAction)
        {
            // Throw ArgumentException if race string is not valide
            string[] temp = Regex.Split(matchup, @"[^\S\r\n]*([ZTPX])V[ZTPX][^\S\r\n]*", RegexOptions.IgnoreCase);
            if (temp.Count() < 1)
            {
                throw new ArgumentException("Can't Parse the BO. Matchup field is required.");
            }
            race = temp[1];
            if (!Race.Races.Contains(race.ToLower()))
            {
                throw new ArgumentException("Can't Parse the BO. Don't reconnizing the race.");
            }


            Name = name.ClearWhiteSpace();
            this.type = type.ClearWhiteSpace();
            this.matchup = matchup.ClearWhiteSpace();
            this.description = description.ClearWhiteSpace();

            this.listOfAction = listOfAction;
        }

        // Transform a local path into a True local Path
        /*private static string FixPathToRelative(string localPath)
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + localPath)));
            return directory.ToString();
        }*/


        private static readonly string folderName = "/saves_bo/";
        // Return the directory path for bo, and create it if needed.
        private static string DirectoryPath()
        {
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            directory = string.Concat(directory, folderName);
            if (!Directory.Exists(directory))
            {
                Console.WriteLine(folderName + " not found. Building it ...");
                Directory.CreateDirectory(directory);
            }
            return directory;
        }
        // Read All bo file in the ressource dir "save_bo/"
        public static HashSet<BuildOrder> ReadAllBO()
        {
            HashSet<BuildOrder> set = new HashSet<BuildOrder>();

            foreach (string file in Directory.EnumerateFiles(DirectoryPath(), "*.bo"))
            {
                set.Add(ReadBO(file));
            }
            return set;
        }

        // Remove a BO from the file system
        internal static void DeleteBO(BuildOrder selectedItem)
        {
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string path = string.Concat(directory, selectedItem.ToPath());

            if (!File.Exists(path))
            {
                Console.WriteLine("Error deleting BuildOrder. File not found. Maybe the name of the file is not the name of the BO.");
            }
            File.Delete(path);
        }

        // create a object BO from it string format
        // Return null if bad format
        public static BuildOrder CreateBO(string boStr, string name)
        {
            BuildOrder bo;
            try
            {
                string[] lines = Regex.Split(boStr, "(?:\r\n)");

                bool actionLines = false;
                Dictionary<string, string> metaData = new Dictionary<string, string>
                {
                    { "name", name },
                    { "type", "" },
                    { "description", "" },
                    { "matchup", "" }
                };
                List<Action> actionList = new List<Action>();

                foreach (var line in lines)
                {
                    // Match these tags without a specific order
                    var splitedLine = Regex.Split(line, @"^(?=.*(Name|Type|Description|Matchup)\:(.*))(?:.*|\r)", RegexOptions.IgnoreCase);

                    // Check if the line is a action line
                    if (Regex.Split(line, Action.regexActionParser).Count() > 2)
                    {
                        actionLines = true;
                    }
                    
                    if (!actionLines)
                    {
                        if (splitedLine.Count() > 1)
                        {
                            var meta = splitedLine[1].ToLower();
                            if (metaData.Keys.Contains(meta))
                            {
                                metaData[meta] = splitedLine[2];
                            }

                        }
                    }
                    else
                    {
                        var action = Action.ReadActionLine(line);
                        if (action != null)
                        {
                            actionList.Add(action);
                        };
                    }
                }
                bo = new BuildOrder(metaData["name"], metaData["type"], metaData["matchup"], metaData["description"], actionList);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error during creation of BO. " + e.ToString());
                return null;
            }
            return bo;
        }

        // Read a BO file
        public static BuildOrder ReadBO(string pathSrc)
        {
            if (!File.Exists(pathSrc))
            {
                Console.WriteLine("No file found.");
                return null;
            }
            string str = File.ReadAllText(pathSrc);
            string name = Path.GetFileNameWithoutExtension(pathSrc);

            return CreateBO(str, name);
        }

        // Save a BO file
        public static void SaveBO(BuildOrder bo)
        {

            using (StreamWriter file = new StreamWriter(string.Concat(DirectoryPath(), bo.ToPath())))
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
            return string.Concat(Name, ".bo");
        }
    }

    // Enum for race
    internal static class Race
    {
        public const string Protoss = "p";
        public const string Terran = "t";
        public const string Zerg = "z";
        public static readonly string[] Races = { Protoss, Terran, Zerg };

        public static string TotalName(string s)
        {
            if (s.ToLower() == Protoss)
            {
                return "Protoss";
            }
            if (s.ToLower() == Terran)
            {
                return "Terran";
            }
            if (s.ToLower() == Zerg)
            {
                return "Zerg";
            }
            throw new InvalidDataException("The race name is not recognizable.");
        }
    }
}
