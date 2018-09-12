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
            Top = 0;
            Left = 0;
            Background = Configuration.BackgroundColor;

            InitializeComponent();

            WorkSpaces = new List<Workspace>() { new Workspace(0, InitialWorkspaceLabel, null, true)};
            StatusBarItems = new List<StatusBarItem>();
            for (var i = 0; i < 8; i++)
            {
                AddWorkspace();
            }

            AddStatusBarItem(DateTime.Today.ToString(), "");
            SetActive(3);
        }

        public void AddWorkspace()
        {
            var newWorkspaceId = WorkSpaces.Last().Id + 1;
            var newWorkspace = new Workspace(newWorkspaceId, Configuration.JNumbers[newWorkspaceId], null, false);
            WorkSpaces.Add(newWorkspace);
            WorkSpacesStackPanel.Children.Add(newWorkspace.WorkspaceLabel);
            WorkSpacesStackPanel.UpdateLayout();
        }

        public void AddStatusBarItem(string content, string processId)
        {
            var statusBarItem = new StatusBarItem(content, processId);

            StatusBarItems.Add(statusBarItem);
            StatusStackPanel.Children.Add(statusBarItem.StatusBarLabel);
            StatusStackPanel.UpdateLayout();
        }

        public void SetActive(int index) {
            foreach (var ws in WorkSpaces) {
                ws.SetInactive();
            }
            WorkSpaces[index].SetActive(null, null);
            StatusStackPanel.UpdateLayout();
        }
    }
}
