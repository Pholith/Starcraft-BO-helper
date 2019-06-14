using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Starcraft_BO_helper
{
    public class BuildOrder
    {

        public String name { get; }
        private String race;
        private List<Action> listOfAction;

        /* to use later
  * https://docs.microsoft.com/fr-fr/dotnet/api/system.diagnostics.stopwatch?view=netframework-4.8q
 Stopwatch stopWatch = new Stopwatch();
 stopWatch.Start();*/


        // A BuildOrder represent a sequence of action at a exact time
        BuildOrder(String name, String race, List<Action> listOfAction)
        {
            race = race.clearWhiteSpace();
            // Throw ArgumentException if race string is not valide
            if (!Race.Races.Contains(race.ToLower()))
            {
                throw new ArgumentException("Can't Parse the BO. Don't reconnizing the race.");
            }
            this.race = race;
            this.name = name.clearWhiteSpace();
            this.listOfAction = listOfAction;

        }

        // Transform a local path into a True local Path
        private static string fixPathToRelative(string localPath)
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + localPath)));
            return directory.ToString();
        }

        // Read All bo file in the ressource dir "save_bo/"
        public static HashSet<BuildOrder> readAllBO()
        {

            HashSet<BuildOrder> set = new HashSet<BuildOrder>();
            foreach (string file in Directory.EnumerateFiles( fixPathToRelative("src/saves_bo/"), "*.bo"))
            {
                set.Add(readBO(file));
            }
            return set;
        }

        // Remove a BO from the file system
        internal static void deleteBO(BuildOrder selectedItem)
        {
            File.Delete(fixPathToRelative(selectedItem.toPath()));
        }

        // create a object BO from it string format
        // Return null if bad format
        public static BuildOrder createBO(String bo)
        {
            BuildOrder trueBO;
            try
            {
                var lines = bo.Split('\n');
                var actionLines = lines.Skip(2);
                var actions = new List<Action>();

                foreach (var actionLine in actionLines)
                {
                    // Pass if the line is empty
                    if (string.IsNullOrWhiteSpace(actionLine))
                    {
                        continue;
                    }
                    actions.Add(Action.readActionLine(actionLine));
                }
                trueBO = new BuildOrder(lines[0], lines[1], actions);
            }
            catch (Exception)
            {
                return null;
            }
            return trueBO;
        }

        // Read a BO file
        public static BuildOrder readBO(string pathSrc)
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

                String race = null;
                String name = null;
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
                    } else
                    {
                        // Pass if the line is empty
                        if (string.IsNullOrWhiteSpace(ln))
                        {
                            continue;
                        }
                        actions.Add(Action.readActionLine(ln));
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
        public static void saveBO(BuildOrder bo)
        {
            string path = fixPathToRelative(bo.toPath());
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(bo.toFormat());
            }
        }

        // Return the BO with string format
        public string toFormat()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(name).AppendLine();
            builder.Append(race).AppendLine();
            foreach (var action in listOfAction)
            {
                builder.Append(action).AppendLine();
            }

            return builder.ToString();
        }

        public Boolean isTerran()
        {
            return race.ToLower() == Race.Terran;
        }

        public Boolean isZerg()
        {
            return race.ToLower() == Race.Zerg;
        }

        public Boolean isProtoss()
        {
            return race.ToLower() == Race.Protoss;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(race).Append(": ");
            builder.Append(name);

            return builder.ToString();
        }
        // Return the relative path of the BO save
        public string toPath()
        {
            return string.Concat("saves_bo/", name, ".bo");
        }
    }

    // Enum for race
    static class Race
    {
        public const string Protoss = "protoss";
        public const string Terran = "terran";
        public const string Zerg = "zerg";
        public static readonly string[] Races = { Protoss, Terran, Zerg };
    }

    // Action of a bo
    class Action
    {
        private TimeSpan time;
        private String action;
        private int atTime;
        private String atTimeAction;

        public Action(TimeSpan time, string action, int atTime, String atTimeAction)
        {
            this.time = time;
            this.action = action.clearWhiteSpace() ?? throw new ArgumentNullException(nameof(action));
            this.atTime = atTime;
            if (atTimeAction != null)
            {
                this.atTimeAction = atTimeAction.clearWhiteSpace();
            } else
            {
                this.atTimeAction = null;
            }
        }


        // Parse a line with a action
        public static Action readActionLine(String line)
        {
            /// Split les lignes du style  0:49 Assimilator @100% gaz
            string[] splited = Regex.Split(line, @"^(\d{1,2}:\d{1,2})[^\S\r\n]*([^@\n\r]*)[^\S\r\n]*(?:(@\d{1,3}% {0,1}.*)|(?:.*))");

            TimeSpan time = TimeSpan.Parse(splited[1]);

            Action a;
            if (!string.IsNullOrEmpty(splited[3]))
            {
                /// Split les lignes du style  @100% gaz
                /// // TOFIX Don't correctly parse action
                var atTimeSplited = Regex.Split(splited[3], @"(?:@)(\d{1,3})(?:%)[^\S\r\n](.*)");
                a = new Action(time, splited[2], int.Parse(atTimeSplited[1]), atTimeSplited[2]);
            }
            a = new Action(time, splited[2], 0, null);
            return a;
        }

        public override string ToString()
        {
            string toFormat = "{0} {1}";
            if (atTime != 0 && !string.IsNullOrWhiteSpace(atTimeAction))
            {
                toFormat = string.Concat(toFormat, string.Format(" @{0}% {1}", atTime, atTimeAction));
            }
            
            return string.Format(toFormat, time, action);
        }
    }
}
