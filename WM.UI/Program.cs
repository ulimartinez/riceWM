using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleInjector;
using WM.UI.Views;
using WM.Utils;

namespace WM.UI
{
	static class Program
	{
		public static readonly ConfigurationManager ConfigurationManager = new ConfigurationManager();
		private static readonly string _barPosition = ConfigurationManager.Variables["$barPosition"];
		private static readonly int _barSize = int.Parse(ConfigurationManager.Variables["$barSize"]);
		private static readonly List<Bar> _wmBars = new List<Bar>();

		[STAThread]
		static void Main()
		{
			var container = Bootstrap();

			// Any additional other configuration, e.g. of your desired MVVM toolkit.

			RunApplication(container);
		}

		private static Container Bootstrap()
		{
			// Create the container as usual.
			var container = new Container();
			container.Register<Bar>();

			// Register your types, for instance:
			container.Verify();

			return container;
		}

		private static void RunApplication(Container container)
		{
			try
			{
				var app = new App();
				foreach (var screen in Screen.AllScreens)
				{
					var x = screen.Bounds.Left;
					var y = _barPosition == "top" ? screen.Bounds.Top : screen.Bounds.Bottom - _barSize;

				var bar= container.GetInstance<Bar>();
					bar.Left = x;
					bar.Top = y;
					bar.Height = _barSize;
					bar.Width = screen.Bounds.Width;
					
					bar.Show();
					_wmBars.Add(bar);
				}

				app.Run();
			}
			catch (Exception ex)
			{
				//Log the exception and exit
			}
		}


		private static void CreateWmBars()
		{
		}

		private static void OnProcessExit(object sender, EventArgs e)
		{
			WindowsTaskBarManager.Show();
		}
	}
}

