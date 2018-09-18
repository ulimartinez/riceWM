namespace WM.Utils
{
	public class StartupTask
	{

		public string Command { get; set; }
		public string Parameters { get; set; }
		
		public StartupTask (string command, string parameters)
		{
			Command = command;
			Parameters = parameters;
		}
	}
}