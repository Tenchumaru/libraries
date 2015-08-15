using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Adrezdi.Silverlight
{
	public abstract class Command : ICommand
	{
		/// <summary>
		/// Occurs when the command manager detects conditions that might
		/// change the ability of a command to execute.
		/// </summary>
		/// <remarks>
		/// This is the name used in WPF for this purpose.
		/// </remarks>
		public static event EventHandler RequerySuggested;

		public static void InvalidateRequerySuggested()
		{
			if(RequerySuggested != null)
				RequerySuggested(null, EventArgs.Empty);
		}

		public static ICommand GetInstance(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(InstanceProperty);
		}

		public static void SetInstance(DependencyObject obj, ICommand value)
		{
			obj.SetValue(InstanceProperty, value);
		}

		public static object GetParameter(DependencyObject obj)
		{
			return obj.GetValue(ParameterProperty);
		}

		public static void SetParameter(DependencyObject obj, object value)
		{
			obj.SetValue(ParameterProperty, value);
		}

		public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance",
			typeof(ICommand), typeof(Command), new PropertyMetadata(OnInstancePropertyChanged));
		public static readonly DependencyProperty ParameterProperty = DependencyProperty.RegisterAttached("Parameter",
			typeof(object), typeof(Command), null);
		private static readonly DependencyProperty CommandBehaviorProperty = DependencyProperty.RegisterAttached(Guid.NewGuid().ToString(),
			typeof(ButtonCommandBehavior), typeof(Command), null);

		public abstract bool CanExecute(object parameter);

		public event EventHandler CanExecuteChanged
		{
			add { RequerySuggested += value; }
			remove { RequerySuggested -= value; }
		}

		public abstract void Execute(object parameter);

		private static void OnInstancePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var button = d as ButtonBase;
			if(button != null)
			{
				if(e.OldValue != null)
				{
					var behavior = (ButtonCommandBehavior)button.GetValue(CommandBehaviorProperty);
					button.ClearValue(CommandBehaviorProperty);
					behavior.Detach();
				}
				if(e.NewValue != null)
				{
					var behavior = new ButtonCommandBehavior(button, (ICommand)e.NewValue);
					button.SetValue(CommandBehaviorProperty, behavior);
					behavior.Attach();
				}
			}
		}

		private class ButtonCommandBehavior
		{
			// If the target button loses all other references, I don't want
			// mine to keep it alive.
			// TODO: consider using weak events (see IWeakEventListener and WeakEventManager).
			private readonly WeakReference reference;
			private readonly ICommand command;

			internal ButtonCommandBehavior(ButtonBase element, ICommand command)
			{
				this.reference = new WeakReference(element);
				this.command = command;
			}

			internal void Attach()
			{
				ButtonBase element = GetElement();
				element.Click += element_Click;
				command.CanExecuteChanged += command_CanExecuteChanged;
				CheckCanExecute(command, element);
			}

			internal void Detach()
			{
				ButtonBase element = GetElement();
				element.Click -= element_Click;
				command.CanExecuteChanged -= command_CanExecuteChanged;
			}

			private static void CheckCanExecute(ICommand command, ButtonBase element)
			{
				object parameter = element.GetValue(ParameterProperty);
				element.IsEnabled = command.CanExecute(parameter);
			}

			private ButtonBase GetElement()
			{
				return reference.Target as ButtonBase;
			}

			private void command_CanExecuteChanged(object sender, EventArgs e)
			{
				ButtonBase element = GetElement();
				if(element != null)
					CheckCanExecute(command, element);
				else
					command.CanExecuteChanged -= command_CanExecuteChanged;
			}

			private static void element_Click(object sender, EventArgs e)
			{
				var element = (DependencyObject)sender;
				var command = (ICommand)element.GetValue(InstanceProperty);
				object parameter = element.GetValue(ParameterProperty);
				command.Execute(parameter);
			}
		}
	}
}
