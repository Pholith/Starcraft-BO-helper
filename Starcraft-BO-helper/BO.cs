using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Starcraft_BO_helper
{
    class BuildOrder
    {

        private String name;
        private String race;
        private List<Action> listOfAction;

        public BuildOrder(String name, String race, List<Action> listOfAction)
        {

            this.race = race;
            this.name = name;
            this.listOfAction = listOfAction;





            /* pour plus tard
             * https://docs.microsoft.com/fr-fr/dotnet/api/system.diagnostics.stopwatch?view=netframework-4.8q
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();*/
        }

        private static string bingPathToAppDir(string localPath)
        {
            string currentDir = Environment.CurrentDirectory;
            DirectoryInfo directory = new DirectoryInfo(
                Path.GetFullPath(Path.Combine(currentDir, @"..\..\" + localPath)));
            return directory.ToString();
        }

        public static void readBO()
        {
            string path = bingPathToAppDir(@"saves_bo\bo1.txt");
            if (!File.Exists(path))
            {
                Console.WriteLine("No file found");
                return;
            }

            // Read file using StreamReader. Reads file line by line  
            using (StreamReader file = new StreamReader(path))
            {
                int counter = 0;
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    if (counter == 0)
                    {
                        String race = ln;
                    }
                    else if (counter == 1)
                    {
                        String name = ln;
                    } else
                    {

                        int splitCount = -1;
                        Boolean endOfDescription = false;
                        string atTime = null;
                        string comment = null;
                        TimeSpan time = new TimeSpan();
                        string action = null;

                        foreach (var str in Regex.Split(ln, @"((?:\d{1,2}:{0,1}){2,3}) (?:[^@])*( )(@\d{1,3}% .*){0,1}"))
                        {
                            /*
                            splitCount ++;

                            // TimeSpan at start
                            if (splitCount == 0)
                            {
                                time = TimeSpan.Parse(str);
                                continue;
                            }


                            if (str.StartsWith("@100%"))
                            {
                                endOfDescription = true;

                            }

                            if (!endOfDescription)
                            {
                                action = str;
                            }

                            Action a = new Action(time, action, comment, atTime);
                            Console.WriteLine(a.ToString());*/
                            Console.Write(" , " + str);

                        }
                        Console.WriteLine("");
                    }



                    counter++;
                }
                file.Close();
                Console.WriteLine($"File has {counter} lines.");
            }

        }

    }


    class Action
    {
        private TimeSpan time;
        private String action;
        private String comment;
        private String actionAtEnd;

        public Action(TimeSpan time, string action, string comment, string actionAtEnd)
        {
            this.time = time;
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.comment = comment;
            this.actionAtEnd = actionAtEnd;
        }

        public override string ToString()
        {
            return "Action: " + time.ToString() + " " + action + " comment: " + comment + " atEnd:" + actionAtEnd;
        }
    }
}
