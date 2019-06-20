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
    internal class Action
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
        public static readonly string regexActionParser = @"^[^\S\r\n]*(\d{1,3}){0,1}[^\S\r\n]*(\d{1,2}:\d{1,2})[^\S\r\n]*([^@\n\r]*)[^\S\r\n]*(?:(@\d{1,3}% {0,1}.*)|(?:.*))";

        private readonly TimeSpan time;
        private readonly String action;
        public String ActionName() { return action; }
        private readonly int atTime;
        private readonly String atTimeAction;

        public Action(TimeSpan time, string action, int atTime, String atTimeAction)
        {
            this.time = time;
            this.action = action.ClearWhiteSpace() ?? throw new ArgumentNullException(nameof(action));
            this.atTime = atTime;
            if (atTimeAction != null)
            {
                this.atTimeAction = atTimeAction.ClearWhiteSpace();
            }
            else
            {
                this.atTimeAction = null;
            }
        }

        // Equivalent for null of a action
        public static Action Zero()
        {
            return new Action(new TimeSpan(), "", 0, "");
        }
        public static Action Infinite()
        {
            return new Action(TimeSpan.MaxValue, "", 0, "");
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
            /// Split lines like "0:49 Assimilator @100% gaz" or  "11 0:26 Drone x2"
            string[] splited = Regex.Split(line, Action.regexActionParser);


            TimeSpan time = TimeSpan.ParseExact(PreFormatTime(splited[2]), "mm\\:ss" ,CultureInfo.InvariantCulture);

            Action a;
            if (!string.IsNullOrEmpty(splited[4]))
            {
                /// Split lines like "@100% gaz"
                /// // TOFIX Don't correctly parse action
                var atTimeSplited = Regex.Split(splited[4], @"(?:@)(\d{1,3})(?:%)[^\S\r\n](.*)");
                a = new Action(time, splited[3], int.Parse(atTimeSplited[2]), atTimeSplited[3]);
            }
            a = new Action(time, splited[3], 0, null);
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
            if (atTime != 0 && !string.IsNullOrWhiteSpace(atTimeAction))
            {
                toFormat = string.Concat(toFormat, string.Format(" @{0}% {1}", atTime, atTimeAction));
            }
            var toStringTime = time.ToString(@"mm\:ss");
            if (time == TimeSpan.MaxValue)
            {
                toStringTime = "";
            }
            return string.Format(toFormat, toStringTime, action);
        }

        public override int GetHashCode()
        {
            var hashCode = -949174417;
            hashCode = hashCode * -1521134295 + EqualityComparer<TimeSpan>.Default.GetHashCode(time);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(action);
            hashCode = hashCode * -1521134295 + atTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(atTimeAction);
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            Action action = obj as Action;
            return action != null &&
                   time.Equals(action.time) &&
                   this.action == action.action &&
                   atTime == action.atTime &&
                   atTimeAction == action.atTimeAction;
        }
    }
}
