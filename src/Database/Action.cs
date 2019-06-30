using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Starcraft_BO_helper
{
    // Action of a bo
    public class Action
    {

        private static readonly List<string> probsName = new List<string>
            {
                "SCV",
                "CSV",
                "VCS",
                "Worker",
                "Probe",
                "Prob",
                "Drone"
            };
        public static readonly Regex regexActionParser = new Regex(@"^(?:[^\S\r\n]*(\d{1,3}){0,1}[^\S\r\n]+){0,1}(\d{1,2}:\d{1,2})\s*([^-\n\r]*)(?:-\s*(.*)){0,1}");

        private readonly TimeSpan time;
        private readonly String action;
        public String ActionName() { return action; }

        private readonly String comment;


        public Action(TimeSpan time, string action, string comment)
        {
            this.time = time;
            this.comment = comment;
            this.action = action.ClearWhiteSpace() ?? throw new ArgumentNullException(nameof(action));
        }

        // Equivalent for null of a action
        public static Action Zero()
        {
            return new Action(new TimeSpan(), "", "");
        }
        public static Action Infinite()
        {
            return new Action(TimeSpan.MaxValue, "", "");
        }

        // Add a 0 if the time format is m:ss
        private static string PreFormatTime(string time)
        {
            string[] splitedTime = Regex.Split(time, @"(\d{1,2}):(\d\d)");

            if (splitedTime[1].Count() == 1)
            {
                splitedTime[1] = String.Concat("0", splitedTime[1]);
            }
            return string.Concat(splitedTime[1], ":", splitedTime[2]);
        }

        // Parse a line with a action
        public static Action ReadActionLine(String line)
        {
            if (line.Count() < 1)
            {
                return null;
            }
            /// Split lines like "0:49 Assimilator - commentary" or "11 0:26 Drone x2"
            string[] splited = new string[4];
            Match match = regexActionParser.Match(line);

            TimeSpan time = TimeSpan.ParseExact(PreFormatTime(match.Groups[2].ToString()), "mm\\:ss", CultureInfo.InvariantCulture);
            Action a = new Action(time, match.Groups[3].ToString(), match.Groups[4].ToString());
            return a;
        }

        // Return true if the time of the action is lower than the time in parameters
        public bool IsPassed(TimeSpan currentTime)
        {
            return currentTime.TotalMilliseconds < time.TotalMilliseconds;
        }

        // Return true if this action is about a worker
        public bool IsWorker()
        {
            foreach (var item in Action.probsName)
            {
                if (action.ToLower().Contains(item.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            string toFormat = "{0} {1}";

            var toStringTime = time.ToString(@"mm\:ss");
            if (time == TimeSpan.MaxValue)
            {
                toStringTime = "";
            }
            toFormat = string.Format(toFormat, toStringTime, action, comment);

            if (!string.IsNullOrWhiteSpace(comment))
            {
                toFormat = string.Concat(toFormat, " - ", comment);
            }
            return toFormat;
        }    	  
    }
}
