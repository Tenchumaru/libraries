using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Adrezdi.Windows
{
	public static class Executor
	{
		private static Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
		private static Action<Action> executor = GetExecutor();

		private static Action<Action> GetExecutor()
		{
			return action =>
			{
				if(dispatcher.CheckAccess())
					action();
				else
					dispatcher.BeginInvoke(action);
			};
		}

		public static void OnUIThread(this Action action)
		{
			executor(action);
		}
	}
}
