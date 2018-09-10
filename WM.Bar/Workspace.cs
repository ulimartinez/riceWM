using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WM.Bar
{
    public class Workspace
    {
        public Label WorkspaceLabel { get; set; }
        public List<string> Processes { get; set; }

        public Workspace(string workspaceLabel, List<string> processes)
        {

            WorkspaceLabel = new Label
            {
                Content = workspaceLabel,
                Width = 30,
                Height = 30,
                Background = Configuration.BackgroundColorLighter,
                Foreground = Configuration.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0, 1, 1, 1),
            };
            Processes = processes;

        }


        public Workspace(Label workspaceLabel, List<string> processes)
        {

            WorkspaceLabel = workspaceLabel;
            Processes = processes;

        }
    }
}
