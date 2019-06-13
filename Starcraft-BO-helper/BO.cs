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
            if (! new string[6] { "protoss", "p", "terran", "t", "zerg", "z" }.Contains(race.ToLower()))
            {
                throw new ArgumentException("Bad race");
            }
            this.race = race;
            this.name = name.clearWhiteSpace();
            this.listOfAction = listOfAction;

        }

        // Transform a local path into a True local Path
        private static string bingPathToAppDir(string localPath)
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + localPath)));
            return directory.ToString();
        }

        // TODO 
        public static void readAllBO()
        {
            readBO(@"saves_bo\bo1.txt");
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
        public static void readBO(string pathSrc)
        {
            string path = bingPathToAppDir(pathSrc);
            if (!File.Exists(path))
            {
                Console.WriteLine("No file found.");
                return;
            }

            // Read file using StreamReader. Reads file line by line  
            using (StreamReader file = new StreamReader(path))
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
                        race = ln;
                    }
                    else if (counter == 1)
                    {
                        name = ln;
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
                new BuildOrder(race, name, actions);

                file.Close();
                Console.WriteLine($"File has {counter} lines.");
            }

        }

        // Save a BO file
        public static void saveBO(BuildOrder bo)
        {
            string path = bingPathToAppDir(bo.toPath());
            using (StreamWriter file = new StreamWriter(path))
            {
                file.Write(bo.ToString());
            }
        }

        // Return the BO with string format
        public override string ToString()
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

        // Return the relative path of the BO save
        public string toPath()
        {
            return string.Concat("saves_bo/", name, ".bo");
        }
    }


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
