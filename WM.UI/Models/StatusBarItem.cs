using System.Windows;
using System.Windows.Controls;
using SimpleInjector;
using WM.Utils;

namespace WM.UI.Models
{

    public class StatusBarItem
    {
        private readonly IConfigurationManager _configurationManager;
        public Label StatusBarLabel { get; set; }
        public string LinkedProcessId { get; set; }
        

        public StatusBarItem(string statusBarItemLabel, string linkedProcessId)
        {
            
            _configurationManager = Program.Bootstrap().GetInstance<IConfigurationManager>();

            StatusBarLabel = new Label
            {
                Content = statusBarItemLabel,
                Height = 30,
                Background = _configurationManager.BackgroundColor,
                Foreground = _configurationManager.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Right,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Padding = new Thickness(10, 0, 10, 0),
            };
            LinkedProcessId = linkedProcessId;
        }
    }
}
