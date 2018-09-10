using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WM.Bar
{

    public class StatusBarItem
    {
        public Label StatusBarLabel { get; set; }
        public string LinkedProcessId { get; set; }

        public StatusBarItem(string statusBarItemLabel, string linkedProcessId)
        {

            StatusBarLabel = new Label
            {
                Content = statusBarItemLabel,
                Height = 30,
                Background = Configuration.BackgroundColorLighter,
                Foreground = Configuration.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Right,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(10, 0, 10, 0),
            };
            LinkedProcessId = linkedProcessId;
        }
    }
}
