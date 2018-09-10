using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WM.Bar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Workspace> WorkSpaces { get; set; }
        public List<StatusBarItem> StatusBarItems { get; set; }

        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            WindowStyle = WindowStyle.None;
            Width = SystemParameters.PrimaryScreenWidth;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;

            Height = 30;
            Top = 40;
            Left = 0;
            Background = Configuration.BackgroundColor;

            InitializeComponent();

            WorkSpaces = new List<Workspace>() { new Workspace(InitialWorkspace, null) };
            StatusBarItems = new List<StatusBarItem>();

            AddWorkspace(null);
            AddWorkspace(null);
            AddWorkspace(null);
            AddWorkspace(null);
            AddWorkspace(null);

            AddStatusBarItem(DateTime.Today.ToString(), "");
        }

        public void AddWorkspace(List<string> process)
        {
            var newWorkspaceId = int.Parse(WorkSpaces.Last().WorkspaceLabel.Content.ToString()) + 1;
            var workspace = new Workspace(newWorkspaceId.ToString(), process);
            WorkSpaces.Add(workspace);
            WorkSpacesStackPanel.Children.Add(workspace.WorkspaceLabel);
            WorkSpacesStackPanel.UpdateLayout();
        }

        public void AddStatusBarItem(string content, string processId)
        {
            var statusBarItem = new StatusBarItem(content, processId);

            StatusBarItems.Add(statusBarItem);
            StatusStackPanel.Children.Add(statusBarItem.StatusBarLabel);
            StatusStackPanel.UpdateLayout();
        }
    }
}
