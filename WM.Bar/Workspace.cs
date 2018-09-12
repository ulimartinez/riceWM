using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WM.Bar
{
    public class Workspace
    {
        public int Id { get; set; }
        public Label WorkspaceLabel { get; set; }
        public List<string> Processes { get; set; }
        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                UpdateLabel();
            }
        }

        public Workspace(int id, string workspaceLabel, List<string> processes, bool isActive)
        {
            Id = id;
            WorkspaceLabel = new Label
            {
                Content = workspaceLabel,
                Width = 30,
                Height = 30,
                Foreground = Configuration.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = Configuration.AccentColor,
            };
            Processes = processes;
            _isActive = isActive;
        }

        public Workspace(int id, Label workspaceLabel, List<string> processes, bool isActive)
        {
            Id = id;
            WorkspaceLabel = workspaceLabel;
            Processes = processes;
            _isActive = isActive;
        }

        public void UpdateLabel() {
            WorkspaceLabel.Background = IsActive ? Configuration.BackgroundColorLighter : Configuration.BackgroundColor;
            WorkspaceLabel.BorderThickness = IsActive ? new Thickness(0, 0, 0, 3) : new Thickness(0, 0, 0, 0);
            WorkspaceLabel.Background = IsActive ? Configuration.BackgroundColorLighter : Configuration.BackgroundColor;
        }

    }
}
