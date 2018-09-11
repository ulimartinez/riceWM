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
        public int Id { get; set; }
        public Label WorkspaceLabel { get; set; }
        public List<string> Processes { get; set; }
        public bool IsActive { get; set; }

        public Workspace(int id, string workspaceLabel, List<string> processes, bool isActive)
        {
            Id = id;
            IsActive = isActive;
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


        public Workspace(int id, Label workspaceLabel, List<string> processes, bool isActive)
        {
            Id = id;
            IsActive = isActive;
            WorkspaceLabel = workspaceLabel;
            Processes = processes;

        }
    }
}
