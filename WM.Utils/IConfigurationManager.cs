using System.Collections.Generic;
using System.Windows.Media;

namespace WM.Utils
{
	public interface IConfigurationManager
	{
		Dictionary<string, string> Variables { get; set; }
		List<Binding> Bindings { get; set; }
		List<StartupTask> StartupTasks { get; set; }
		Brush BackgroundColor { get; set; }
		Brush BackgroundColorLighter { get; set; }
		Brush AccentColor { get; set; }
		Brush ForegroundColor { get; set; }
		string[] JNumbers { get; }

		void AddBinding(string[] bindStatement);
		void AddStartupTask(string[] startupTaskStatement);
		void SetVariable(string[] setVariableStatement);
		void SubstituteVariables();
		void SetColors();

		void Initalize();
	}
}