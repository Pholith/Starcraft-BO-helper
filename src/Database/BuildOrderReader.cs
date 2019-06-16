using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Starcraft_BO_helper
{
    class BuildOrderReader
    {
        private readonly BuildOrder bo;
        private readonly Label timerLabel;

        private DateTime TimerStart { get; set; }
               

        public BuildOrderReader(BuildOrder bo, Label timerLabel)
        {
            this.bo = bo;
            TimerStart = DateTime.Now;
            this.timerLabel = timerLabel;
        }

        private void OnDispatcherTimer_Tick(object sender, EventArgs e)
        {
            var currentValue = DateTime.Now - this.TimerStart;
            TimerStart = DateTime.Now;
            timerLabel.Content = currentValue.Seconds.ToString();
        }

    }
}
