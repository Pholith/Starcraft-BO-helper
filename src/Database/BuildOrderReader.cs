using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Starcraft_BO_helper
{
    class BuildOrderReader
    {
        private readonly BuildOrder bo;
        private readonly TextBlock timerLabel;

        private DateTime TimerStart { get; set; }


        // TO REFACTOR
        // Do not use the fields, you must change the value using the private setter
        private Action previousAction;
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
                previousAction = value;
                OnPropertyChanged(0, previousAction);
            }
        }
        private Action currentAction;
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
                currentAction = value;
                TimerAnimate();
                OnPropertyChanged(1, currentAction);
            }
        }

        private Action nextAction;
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
                nextAction = value;
                OnPropertyChanged(2, nextAction);
            }
        }

        // Update label on property changed
        private void OnPropertyChanged(int i, Action newAction)
        {
            actionsOnLabels[i] = newAction;
            labels[i].Content = newAction.ToString();
        }

        private readonly DoubleAnimation fontSizeAnimation;
        private readonly ColorAnimation colorAnimation;

        private readonly Page page;
        private readonly List<Label> labels;
        private readonly List<Action> actionsOnLabels;

        private readonly List<Action> listOfActions;
        private TimeSpan currentValue;
        private bool playStarted = false;

        private const int fontSize = 36;

        public BuildOrderReader(BuildOrder bo, Page page, TextBlock timerLabel, Label titleLabel, Label previousLabel, Label currentLabel, Label nextLabel)
        {
            this.bo = bo;
            this.timerLabel = timerLabel;
            this.page = page;
            
            // Initialize labels and actions lists
            labels = new List<Label>
            {
                previousLabel,
                currentLabel,
                nextLabel
            };

            actionsOnLabels = new List<Action>
            {
                PreviousAction,
                CurrentAction,
                NextAction
            };
            titleLabel.Content = bo.Name;

            //Initialise list
            listOfActions = bo.ListOfAction;
            NextAction = listOfActions[0];
            listOfActions.Remove(NextAction);

            // Time setup

            // Animation on timer label
            colorAnimation = new ColorAnimation(Colors.White, Colors.Red, TimeSpan.FromMilliseconds(800))
            {
                AutoReverse = true
            };
            fontSizeAnimation = new DoubleAnimation(fontSize, fontSize + 10, TimeSpan.FromMilliseconds(800))
            {
                AutoReverse = true
            };
        }

        public void Start()
        {
            Console.WriteLine("Build Order Reader started.");
            timerLabel.FontSize = fontSize;
            TimerStart = DateTime.Now;
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(OnDispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1); // Delay before each event
            dispatcherTimer.Start();
            playStarted = true;
        }
        public bool Started()
        {
            return playStarted;
        }
        // EventHandler tick
        private void OnDispatcherTimer_Tick(object sender, EventArgs e)
        {
            // Update the timer label
            currentValue = DateTime.Now - this.TimerStart;
            timerLabel.Text = currentValue.ToString(@"mm\:ss\:ff");

           if (NextAction.IsPassed(currentValue))
           {
               return;
           }
           SkipAction(false);
        }
        // Skip a action in the list action
        public void SkipAction(bool forced)
        {
            Action a;

            // No actions
            if (listOfActions.Count() == 0)
            {
                a = Action.Infinite();
            }
            // 1 action or more
            else 
            {
                a = listOfActions[0];
            }

            // update actions
            PreviousAction = CurrentAction;
            CurrentAction = NextAction;
            NextAction = a;

            listOfActions.Remove(a);

        }
        // Animate the timerLabel
        private void TimerAnimate()
        {
            timerLabel.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, colorAnimation);
            timerLabel.BeginAnimation(TextBlock.FontSizeProperty, fontSizeAnimation);
        }
    }
}
