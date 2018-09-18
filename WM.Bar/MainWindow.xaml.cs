using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WM.Utils;

namespace WM.Bar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public List<Workspace> WorkSpaces { get; set; }
        public List<StatusBarItem> StatusBarItems { get; set; }
        public static readonly ConfigurationManager ConfigurationManager = new ConfigurationManager();

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
            Background = ConfigurationManager.BackgroundColor;

            InitializeComponent();

            InitialWorkspaceLabel.MouseLeftButtonUp += Workspace_MouseClickUp;
            WorkSpaces = new List<Workspace>() { new Workspace(0, InitialWorkspaceLabel, null, true) };
            StatusBarItems = new List<StatusBarItem>();
            for (var i = 0; i < 8; i++)
            {
                AddWorkspace();
            }

            AddStatusBarItem(DateTime.Today.ToString(CultureInfo.InvariantCulture), "");
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Handle messages...
            if (msg == 0x165) {
                SelectWorkspace(WorkSpaces[(int) lParam - 1].WorkspaceLabel);
            }
            return IntPtr.Zero;
        }

        public void AddWorkspace()
        {
            var newWorkspaceId = WorkSpaces.Last().Id + 1;
            var newWorkspace = new Workspace(newWorkspaceId, ConfigurationManager.JNumbers[newWorkspaceId], null, false);
            newWorkspace.WorkspaceLabel.MouseLeftButtonUp += Workspace_MouseClickUp;

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

        public void Workspace_MouseClickUp(object sender, MouseEventArgs e) {
            var label = (Label) sender;
            SelectWorkspace(label);
        }

        private void SelectWorkspace(Label label) {
            foreach (var workspace in WorkSpaces) {
                workspace.IsActive = false;
            }
            var selectedWorkspace = WorkSpaces.Find(w => w.WorkspaceLabel.Content == label.Content);
            selectedWorkspace.IsActive = true;
        }
    }
}
