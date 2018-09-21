using System;
using System.Runtime.InteropServices;

namespace WM.Utils
{
	public static class WindowsTaskBarManager
	{
		[DllImport("user32.dll")]
		private static extern int ShowWindow(int hwnd, int command);
		
		[DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
		static extern int FindWindow(string lpClassName, string lpWindowName);

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

			ShowWindow(FindWindowEx(FindWindow("Shell_TrayWnd", null), 0, "Button", null), SwHide);
		}

		public static void DisableTaskBarsOnMultipleDisplays()
		{
//			REG ADD HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced /V MMTaskbarEnabled /T REG_dWORD /D 0 /F
//			HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StuckRects3
//			taskkill /f /im explorer.exe
//				start explorer.exe
			
			
		}
	}
}