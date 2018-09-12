using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
                Background = isActive ? Configuration.BackgroundColorLighter: Configuration.BackgroundColor,
                Foreground = Configuration.ForegroundColor,
                HorizontalAlignment = HorizontalAlignment.Left,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                BorderBrush = Configuration.AccentColor,
                BorderThickness = isActive? new Thickness(0, 0, 0, 3): new Thickness(0, 0, 0, 0)
            };
            WorkspaceLabel.MouseLeftButtonUp += SetActive;
            Processes = processes;

        }

        public void SetActive(object sender, MouseEventArgs e) {
            IsActive = true;
            WorkspaceLabel.BorderThickness = IsActive ? new Thickness(0, 0, 0, 3) : new Thickness(0, 0, 0, 0);
            WorkspaceLabel.Background = IsActive ? Configuration.BackgroundColorLighter: Configuration.BackgroundColor;
        }


        public Workspace(int id, Label workspaceLabel, List<string> processes, bool isActive)
        {
            Id = id;
            IsActive = isActive;
            workspaceLabel.MouseLeftButtonUp += SetActive;
            WorkspaceLabel = workspaceLabel;
            Processes = processes;

        }

        public void SetInactive() {
            IsActive = false;
            WorkspaceLabel.BorderThickness = IsActive ? new Thickness(0, 0, 0, 3) : new Thickness(0, 0, 0, 0);
            WorkspaceLabel.Background = IsActive ? Configuration.BackgroundColorLighter: Configuration.BackgroundColor;
        }
    }
}
