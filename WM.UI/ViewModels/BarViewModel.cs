using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ExampleGUI.Commands;
using log4net;

namespace WM.UI.ViewModels
{
	public class BarViewModel : ViewModelBase
	{
		private readonly ILog consoleLog;
		public IAsyncCommand StartButtonCommand { get; set; }
		public ICommand StopButtonCommand { get; set; }
		private CancellationTokenSource ctSource { get; set; }

		public BarViewModel(ILog log)
		{
			consoleLog = log;
			StartButtonCommand = new AwaitableDelegateCommand(StartButtonExecute);
			StopButtonCommand = new DelegateCommand(StopButtonExecute, () => Running);
		}

		private async Task StartButtonExecute()
		{
			WriteLine("Starting");
			Running = true;
			ctSource = new CancellationTokenSource();
			await Task.Run(async () =>
			{
				try
				{
					var random = new Random();
					while (!ctSource.Token.IsCancellationRequested)
					{
						var delayMs = random.Next(1000, 2000);
						await Task.Delay(delayMs, ctSource.Token);
						WriteLine($"Delayed {delayMs}ms");
					}
				}
				catch (OperationCanceledException)
				{
				}

				WriteLine("Stopped");
			});
		}

		private void StopButtonExecute()
		{
			WriteLine("Stopping");
			ctSource?.Cancel();
			Running = false;
		}

		private void WriteLine(string message)
		{
			consoleLog.Info(message);
			ConsoleText += message + Environment.NewLine;
		}

		private bool running;

		public bool Running
		{
			get => running;
			set
			{
				if (value == running) return;
				running = value;
				OnPropertyChanged();
			}
		}

		private string consoleText;

		public string ConsoleText
		{
			get => consoleText;
			set
			{
				if (value == consoleText) return;
				consoleText = value;

				OnPropertyChanged();
			}
		}
	}
}