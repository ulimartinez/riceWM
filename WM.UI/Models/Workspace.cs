using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WM.Utils;

namespace WM.UI.Models
{
    public class Workspace
    {
        private readonly IConfigurationManager _configurationManager;
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
            _configurationManager = Program.Bootstrap().GetInstance<IConfigurationManager>();
            Id = id;
            WorkspaceLabel = new Label
            {
                Content = workspaceLabel,
                Width = 30,
                Height = 30,
                Foreground = _configurationManager.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = _configurationManager.AccentColor,
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
            WorkspaceLabel.Background = IsActive ? _configurationManager.BackgroundColorLighter : _configurationManager.BackgroundColor;
            WorkspaceLabel.BorderThickness = IsActive ? new Thickness(0, 0, 0, 3) : new Thickness(0, 0, 0, 0);
            WorkspaceLabel.Background = IsActive ? _configurationManager.BackgroundColorLighter : _configurationManager.BackgroundColor;
        }

    }
}
