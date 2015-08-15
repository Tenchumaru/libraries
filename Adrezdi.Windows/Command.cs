using System;
using System.Windows.Input;

namespace Adrezdi.Windows
{
	public abstract class Command : ICommand
	{
		protected void FireCanExecuteChanged()
		{
			CommandManager.InvalidateRequerySuggested();
		}

		public abstract bool CanExecute(object parameter);

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public abstract void Execute(object parameter);
	}
}
