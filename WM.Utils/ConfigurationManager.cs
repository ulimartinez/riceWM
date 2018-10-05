using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace WM.Utils
{
	public class ConfigurationManager : IConfigurationManager
	{
		private const string Config = "ricerc";

		public Dictionary<string, string> Variables { get; set; }
		public List<Binding> Bindings { get; set; }
		public List<StartupTask> StartupTasks { get; set; }
		public Brush BackgroundColor { get; set; }
		public Brush BackgroundColorLighter { get; set; }
		public Brush AccentColor { get; set; }
		public Brush ForegroundColor { get; set; }
		public string[] JNumbers { get; } = {"一", "二", "三", "四", "五", "六", "七", "八", "九"};


		public ConfigurationManager()
		{
			Variables = new Dictionary<string, string>();
			Bindings = new List<Binding>();
			StartupTasks = new List<StartupTask>();
		}

		public void Initalize()
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
			SetColors();
		}

		public void AddBinding(string[] bindStatement)
		{
			var validNumberOfArguments = bindStatement.Length >= 3;
			var isModifierAndKeyCombination = bindStatement[1].Split('+').Length >= 2;

			if (!validNumberOfArguments || !isModifierAndKeyCombination) return;

			var keyCombination = bindStatement[1];
			var command = bindStatement[2];
			var parameters = bindStatement.Length > 3 ? bindStatement[3] : null;

			Bindings.Add(new Binding(keyCombination, command, parameters));
		}

		public void AddStartupTask(string[] startupTaskStatement)
		{
			var validNumberOfArguments = startupTaskStatement.Length >= 3;

			if (!validNumberOfArguments) return;

			var command = startupTaskStatement[1];
			var parameters = startupTaskStatement[2];

			StartupTasks.Add(new StartupTask(command, parameters));
		}

		public void SetVariable(string[] setVariableStatement)
		{
			var validNumberOfArguments = setVariableStatement.Length == 3;

			if (!validNumberOfArguments) return;

			var variableName = setVariableStatement[1];
			var variableValue = setVariableStatement[2];

			Variables.Add(variableName, variableValue);
		}

		public void SubstituteVariables()
		{
			foreach (var variable in Variables)
			{
				foreach (var binding in Bindings.Where(x => x.KeyCombination.Contains(variable.Key + "+")))
				{
					binding.KeyCombination = binding.KeyCombination.Replace(variable.Key, variable.Value);
				}
			}
		}

		public void SetColors()
		{
			BackgroundColor = (Brush) new BrushConverter().ConvertFrom(Variables["$backgroundColor"]);
			BackgroundColorLighter = (Brush) new BrushConverter().ConvertFrom(Variables["$backgroundColorLighter"]);
			AccentColor = (Brush) new BrushConverter().ConvertFrom(Variables["$accentColor"]);
			ForegroundColor = (Brush) new BrushConverter().ConvertFrom(Variables["$foregroundColor"]);
		}
	}
}