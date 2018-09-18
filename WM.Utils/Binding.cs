namespace WM.Utils
{
	public class Binding
	{

		public string KeyCombination { get; set; }
		public string Command { get; set; }
		public string Parameters { get; set; }
		
		public Binding(string keyCombination, string command, string parameters)
		{
			KeyCombination = keyCombination;
			Command = command;
			Parameters = parameters;
		}
	}
}