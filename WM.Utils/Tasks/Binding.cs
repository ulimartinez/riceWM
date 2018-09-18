namespace WM.Utils
{
	public class Binding : WMTask
	{

		public string KeyCombination { get; set; }

		public Binding(string keyCombination, string command, string parameters) : base(command, parameters)
		{
			KeyCombination = keyCombination;
		}
	}
}