using System.Windows;
using System.Windows.Controls;
using WM.Utils;

namespace WM.UI.Models
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
                Background = ConfigurationManager.BackgroundColorLighter,
                Foreground = ConfigurationManager.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Right,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(10, 0, 10, 0),
            };
            LinkedProcessId = linkedProcessId;
        }
    }
}
