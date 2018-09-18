namespace WM.Utils
{
	public class WMTask
	{
		public string Command { get; set; }
		public string Parameters { get; set; }
		
		public WMTask(string command, string parameters)
		{
			Command = command;
			Parameters = parameters;
		}
	}
}