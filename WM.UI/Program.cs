using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleInjector;
using log4net;
using WM.UI.ViewModels;
using WM.UI.Views;
using WM.Utils;

namespace WM.UI
{
	static class Program
	{
		private static string BarPosition { get; set; }
		private static int BarSize { get; set; }
		private static readonly List<Bar> WmBars = new List<Bar>();

		[STAThread]
		static void Main()
		{
			var container = Bootstrap();

			// Any additional other configuration, e.g. of your desired MVVM toolkit.

			RunApplication(container);
		}

		public static Container Bootstrap()
		{
			var container = new Container();
			try
			{
				// Create the container as usual.
				container.Register<IConfigurationManager, ConfigurationManager>(Lifestyle.Singleton);
				container.Register<Bar>();
				container.Register<BarViewModel>();

				container.RegisterConditional(typeof(ILog),
					c => typeof(Log4NetAdapter<>).MakeGenericType(c.Consumer.ImplementationType),
					Lifestyle.Singleton,
					c => true);

				// Register your types, for instance:
				container.Verify();

				return container;
			}
			catch (Exception ex)
			{
				//Log the exception and exit
				container.GetInstance<ILog>().Error(ex);
				throw;
			}
		}

		private static void RunApplication(Container container)
		{
			try
			{
				var configurationManager = container.GetInstance<IConfigurationManager>();
				container.GetInstance<IConfigurationManager>().Initalize();

				BarPosition = configurationManager.Variables["$barPosition"];
				BarSize = Convert.ToInt32(configurationManager.Variables["$barSize"]);


				var app = new App();
				foreach (var screen in Screen.AllScreens)
				{
					var x = screen.Bounds.Left;
					var y = BarPosition == "top" ? screen.Bounds.Top : screen.Bounds.Bottom - BarSize;

					var bar = container.GetInstance<Bar>();
					bar.Initialize();

					bar.Left = x;
					bar.Top = y;
					bar.Height = BarSize;
					bar.Width = screen.Bounds.Width;

					bar.Show();
					WmBars.Add(bar);
				}

				app.Run();
			}
			catch (Exception ex)
			{
				//Log the exception and exit
				container.GetInstance<ILog>().Error(ex);
				throw;
			}
		}
	}
}