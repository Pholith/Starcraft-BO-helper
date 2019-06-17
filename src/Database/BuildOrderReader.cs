using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Starcraft_BO_helper
{
    class BuildOrderReader
    {
        private readonly BuildOrder bo;
        private readonly Label timerLabel;

        private DateTime TimerStart { get; set; }


        // TO REFACTOR

        private Action previousAction = Action.Zero();
        public Action PreviousAction
        {
            get
            {
                return previousAction;
            }
            private set
            {
                if (previousAction == value)
                    return;
                OnPropertyChanged(0, previousAction);
                previousAction = value;
            }
        }
        private Action currentAction = Action.Zero();
        public Action CurrentAction
        {
            get
            {
                return currentAction;
            }
            private set
            {
                if (currentAction == value)
                    return;
                OnPropertyChanged(1, currentAction);
                currentAction = value;
            }
        }
        private Action nextAction = Action.Zero();
        public Action NextAction
        {
            get
            {
                return nextAction;
            }
            private set
            {
                if (nextAction == value)
                    return;
                OnPropertyChanged(2, nextAction);
                nextAction = value;
            }
        }

        // Update label on property changed
        private void OnPropertyChanged(int i, Action newAction)
        {
            actionsOnLabels[i] = newAction;
            labels[i].Content = newAction.ActionName();

        }

        private List<Label> labels;
        private List<Action> actionsOnLabels;

        public BuildOrderReader(BuildOrder bo, Label timerLabel, Label previousLabel, Label currentLabel, Label nextLabel)
        {
            this.bo = bo;
            this.timerLabel = timerLabel;

            labels = new List<Label>();
            labels.Add(previousLabel);
            labels.Add(currentLabel);
            labels.Add(nextLabel);

            actionsOnLabels = new List<Action>();
            actionsOnLabels.Add(PreviousAction);
            actionsOnLabels.Add(CurrentAction);
            actionsOnLabels.Add(NextAction);

            NextAction = bo.ListOfAction[0];
            //// Time setup
            this.TimerStart = DateTime.Now;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(OnDispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1); // Delay before each event
            dispatcherTimer.Start();
            ////

        }

        // EventHandler tick
        private void OnDispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Update the timer label
            var currentValue = DateTime.Now - this.TimerStart;
            timerLabel.Content = currentValue.ToString(@"mm\:ss\:ff");

            // Calculate the actions
            List<Action> rawList = bo.ListOfAction;
            rawList.RemoveAll(delegate (Action ac)
            {
                return ac.IsPassed(currentValue);
            });
            IEnumerable<Action> actions = rawList.Skip(1);
            if (actions.Count() < 0)
            {
                if (NextAction.ActionName() == "")
                {
                    return;
                }
                PreviousAction = CurrentAction;
                CurrentAction = NextAction;
                NextAction = Action.Zero();
                return;
            }
            var a = actions.First();
            if (a == null || a == NextAction)
            {
                return;
            }
            PreviousAction = CurrentAction;
            CurrentAction = NextAction;
            NextAction = a;

        }

    }
}
