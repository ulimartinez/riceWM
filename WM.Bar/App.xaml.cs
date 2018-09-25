using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using WM.Utils;

namespace WM.Bar
{
	public partial class App
	{
		public static readonly ConfigurationManager ConfigurationManager = new ConfigurationManager();
		private readonly string _barPosition = ConfigurationManager.Variables["$barPosition"];
		private readonly int _barSize = int.Parse(ConfigurationManager.Variables["$barSize"]);
		private readonly List<MainWindow> _wmBars = new List<MainWindow>();

		private void App_OnStartup(object sender, StartupEventArgs e)
		{
			AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
			WindowsTaskBarManager.Hide();
			
			CreateWmBars();

		}

		private void CreateWmBars()
		{
			foreach (var screen in Screen.AllScreens)
			{
				var x = screen.Bounds.Left;
				var y = _barPosition == "top" ? screen.Bounds.Top : screen.Bounds.Bottom - _barSize;

				var bar = new MainWindow {Left = x, Top = y, Height = _barSize, Width = screen.Bounds.Width};
				bar.Show();

				_wmBars.Add(bar);
			}
			
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			WindowsTaskBarManager.Show();
		}
	}
}