using System;

namespace Adrezdi.Windows
{
	public class DelegatedCommand : Command
	{
		private Func<object, bool> InternalCanExecute;
		private Action<object> InternalExecute;

		public DelegatedCommand(Action<object> execute)
			: this(execute, _ => true)
		{
		}

		public DelegatedCommand(Action<object> execute, Func<object, bool> canExecute)
		{
			if(canExecute == null)
				throw new ArgumentNullException("canExecute");
			if(execute == null)
				throw new ArgumentNullException("execute");
			InternalCanExecute = canExecute;
			InternalExecute = execute;
		}

		public override bool CanExecute(object parameter)
		{
			return InternalCanExecute(parameter);
		}

		public override void Execute(object parameter)
		{
			InternalExecute(parameter);
		}
	}
}
