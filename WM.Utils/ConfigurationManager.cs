using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace WM.Utils
{
	public class ConfigurationManager
	{
		private const string Config = "ricerc";

		public static Dictionary<string, string> Variables = new Dictionary<string, string>();
		public static List<Binding> Bindings = new List<Binding>();
		public static List<StartupTask> StartupTasks = new List<StartupTask>();
		public static Brush BackgroundColor { get; } = (Brush) new BrushConverter().ConvertFrom("#23232D");
		public static Brush BackgroundColorLighter { get; } = (Brush) new BrushConverter().ConvertFrom("#2c2c36");
		public static Brush AccentColor { get; } = (Brush) new BrushConverter().ConvertFrom("#7289da");
		public static Brush ForegroundColor { get; } = (Brush) new BrushConverter().ConvertFrom("#eae5e5");
		public static string[] JNumbers = {"一", "二", "三", "四", "五", "六", "七", "八", "九"};

		public ConfigurationManager()
		{
			var reader = File.OpenText(Config);
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				const string splitBySpacesRegex = "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
				var parts = Regex.Split(line, splitBySpacesRegex);
				var typeOfConfig = parts[0];
				switch (typeOfConfig)
				{
					case "bind":
						AddBinding(parts);
						continue;
					case "startup":
						AddStartupTask(parts);
						continue;
					case "set":
						SetVariable(parts);
						continue;
				}
			}
			SubstituteVariables();
		}

		private static void AddBinding(string[] bindStatement)
		{
			var validNumberOfArguments = bindStatement.Length >= 3;
			var isModifierAndKeyCombination = bindStatement[1].Split('+').Length >= 2;
			
			if (!validNumberOfArguments || !isModifierAndKeyCombination) return;

			var keyCombination = bindStatement[1];
			var command = bindStatement[2];
			var parameters = bindStatement.Length > 3 ? bindStatement[3] : null;
			
			Bindings.Add(new Binding(keyCombination, command, parameters));
		}

		private static void AddStartupTask(string[] startupTaskStatement)
		{
			var validNumberOfArguments = startupTaskStatement.Length >= 3;
			
			if (!validNumberOfArguments) return;

			var command = startupTaskStatement[1];
			var parameters = startupTaskStatement[2];
			
			StartupTasks.Add(new StartupTask(command, parameters));
		}

		private static void SetVariable(string[] setVariableStatement)
		{
			var validNumberOfArguments = setVariableStatement.Length == 3;
			
			if (!validNumberOfArguments) return;

			var variableName = setVariableStatement[1];
			var variableValue = setVariableStatement[2];

			Variables.Add(variableName, variableValue);
		}

		private static void SubstituteVariables()
		{
			foreach (var variable in Variables)
			{
				foreach (var binding in Bindings.Where(x => x.KeyCombination.Contains(variable.Key + "+")))
				{
					binding.KeyCombination = binding.KeyCombination.Replace(variable.Key, variable.Value);
				}
			}
		}
	}
}