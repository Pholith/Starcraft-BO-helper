using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Starcraft_BO_helper
{
    // Action of a bo
    internal class Action
    {
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

        // Parse a line with a action
        public static Action ReadActionLine(String line)
        {
            /// Split les lignes du style  0:49 Assimilator @100% gaz
            string[] splited = Regex.Split(line, @"^(\d{1,2}:\d{1,2})[^\S\r\n]*([^@\n\r]*)[^\S\r\n]*(?:(@\d{1,3}% {0,1}.*)|(?:.*))");

            TimeSpan time = TimeSpan.ParseExact(splited[1], "mm\\:ss" ,CultureInfo.InvariantCulture);

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

        // Return true if the time of the action is lower than the time in parameters
        public Boolean IsPassed(TimeSpan currentTime)
        {
            return currentTime.TotalMilliseconds > time.TotalMilliseconds;
        }

        public override string ToString()
        {
            string toFormat = "{0} {1}";
            if (atTime != 0 && !string.IsNullOrWhiteSpace(atTimeAction))
            {
                toFormat = string.Concat(toFormat, string.Format(" @{0}% {1}", atTime, atTimeAction));
            }

            return string.Format(toFormat, time.ToString(@"mm\:ss"), action);
        }
    }
}
