using System.Runtime.InteropServices;

namespace WM.Utils
{
	public static class WindowsTaskBarManager
	{
		[DllImport("user32.dll")]
		private static extern int ShowWindow(int hwnd, int command);

		[DllImport("user32.dll")]
		private static extern int FindWindowEx(int parent, int afterWindow, string className, string windowText);

		private const int SwHide = 0;
		private const int SwShow = 1;

		public static void Show()
		{
			var hWnd = 0;
			while ((hWnd = FindWindowEx(0, hWnd, "Shell_TrayWnd", "")) != 0)
				ShowWindow(hWnd, SwShow);
		}

		public static void Hide()
		{
			var hWnd = 0;
			while ((hWnd = FindWindowEx(0, hWnd, "Shell_TrayWnd", "")) != 0)
				ShowWindow(hWnd, SwHide);
		}
	}
}