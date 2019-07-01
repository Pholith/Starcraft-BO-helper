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
        public Dictionary<string, string> MetaDatas { get; }

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
                    string.Concat("Race: ", Race.TotalName(MetaDatas["race"])),
                    string.Concat("Matchup: ", MetaDatas["matchup"]),
                    string.Concat("Type: ", MetaDatas["type"]),
                    string.Concat("Description: ", MetaDatas["description"]),
                    ""
                };
                return metas;
            }
        }

        private static List<string> allowedDatas = new List<string> { "matchup", "description", "type", "author" };
        private static List<string> requiredDatas = new List<string> { "matchup" };


        // A BuildOrder represent a sequence of action at a exact time
        private BuildOrder(string name, IEnumerable<Action> listOfAction, Dictionary<string, string> metaDatas)
        {
            if (metaDatas == null || string.IsNullOrWhiteSpace(name) || listOfAction == null)
            {
                throw new Exception("Fields with an * must be filled");
            }
            // Check if required data are set
            foreach (var item in requiredDatas)
            {
                if (!metaDatas.ContainsKey(item))
                {
                    throw new ArgumentException(item + " data is required to create a build order");
                }
            }
            // Fill empty datas if not given in the constructor
            foreach (var item in allowedDatas)
            {
                if (!metaDatas.ContainsKey(item))
                {
                    metaDatas.Add(item, "");
                } else
                {
                    metaDatas[item] = metaDatas[item].ClearWhiteSpace();
                }
            }

            // Throw ArgumentException if race string is not valide
            Regex matchupParser = new Regex(@"[^\S\r\n]*([ZTP])V[ZTPX][^\S\r\n]*", RegexOptions.IgnoreCase);
            Match match = matchupParser.Match(metaDatas["matchup"]);

            if (!match.Groups[1].Success)
            {
                throw new ArgumentException("Matchup field must be in format \"PvX\",\"ZvT\"...");
            }
            metaDatas.Add("race", match.Groups[1].ToString());

            this.MetaDatas = metaDatas;

            Name = name.ClearWhiteSpace();

            this.listOfAction = (List<Action>) listOfAction;
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
                try
                {
                    set.Add(ReadBO(file));
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in bo reading: "+ file);
                }
            }
            return set;
        }

        // Remove a BO from the file system
        internal static void DeleteBO(BuildOrder selectedItem)
        {
            string directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + folderName;
            string path = string.Concat(directory, selectedItem.ToPath());

            if (!File.Exists(path))
            {
                Console.WriteLine("Error deleting BuildOrder. File not found. Maybe the name of the file is not the name of the BO.");
            }
            File.Delete(path);
        }

        // create a object BO from it string format
        // Return null if bad format
        public static BuildOrder CreateBOFromString(string boStr, string name ="")
        {
            string[] lines = Regex.Split(boStr, "(?:\r\n)");

            bool actionLines = false;
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            foreach (var item in allowedDatas)
            {
                metaData.Add(item, "");
            }

            List<Action> actionList = new List<Action>();

            foreach (var line in lines)
            {
                // Match these tags without a specific order
                var splitedLine = Regex.Split(line, @"^(?=.*(Name|Type|Description|Matchup|Author).*\:(.*))(?:.*|\r)", RegexOptions.IgnoreCase);

                // Check if the line is a action line
                if (Action.regexActionParser.IsMatch(line))
                {
                    actionLines = true;
                }
                    
                if (!actionLines)
                {
                    if (splitedLine.Count() > 1)
                    {
                        var meta = splitedLine[1].ToLower();
                        if (meta == "name")
                        {
                            name = splitedLine[2];
                        }
                        else if (metaData.Keys.Contains(meta))
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
            BuildOrder bo = new BuildOrder(name, actionList, metaData);
            return bo;
        }

        //Create a custom BO from the BuildOrderMenu
        public static void CreateAndSaveBO(string name, IEnumerable<Action> actionList, Dictionary<string, string> metaDatas)
        {
            BuildOrder bo = new BuildOrder(name, actionList, metaDatas);
            SaveBO(bo);
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

            return CreateBOFromString(str, name);
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
            builder.Append("Name : " + Name).AppendLine();
            foreach (var item in MetaDatas)
            {
                builder.Append(item.Key + ": " + item.Value).AppendLine();
            }
            foreach (Action action in listOfAction)
            {
                builder.Append(action).AppendLine();
            }

            return builder.ToString();
        }

        // Return true if the BuildOrder is of the specified race
        public bool IsTerran()
        {
            return MetaDatas["race"].ToLower() == Race.Terran;
        }

        public bool IsZerg()
        {
            return MetaDatas["race"].ToLower() == Race.Zerg;
        }

        public bool IsProtoss()
        {
            return MetaDatas["race"].ToLower() == Race.Protoss;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(MetaDatas["race"]).Append(": ");
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
