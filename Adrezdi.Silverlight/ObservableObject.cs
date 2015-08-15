using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Adrezdi.Silverlight
{
	/// <summary>
	/// Provides an implementation of the INotifyPropertyChanged interface that
	/// uses either property expressions or string names to identify properties
	/// in change notifications.
	/// </summary>
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Fires the PropertyChanged event using a lambda expression that
		/// references the property (e.g. "() => Property").
		/// </summary>
		/// <param name="expression">A lambda expression of the form "() => Property".</param>
		protected void FirePropertyChanged<T>(Expression<Func<T>> expression)
		{
			if(expression == null)
				throw new ArgumentNullException("expression");
			var memberExpression = GetMemberExpression(expression.Body);
			FirePropertyChanged(memberExpression.Member.Name);
		}

		/// <summary>
		/// Fires the PropertyChanged event using the property name.
		/// </summary>
		/// <param name="propertyName">The property name.</param>
		protected void FirePropertyChanged(string propertyName)
		{
			if(propertyName == null)
				throw new ArgumentNullException("propertyName");
			VerifyPropertyName(propertyName);
			PropertyChangedEventHandler handler = PropertyChanged;
			if(handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}

		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		protected void VerifyPropertyName(string propertyName)
		{
#if DEBUG
			// Verify that the property name matches a real, public, instance
			// property on this object.
			Debug.Assert(GetType().GetProperty(propertyName) != null, "Invalid property name:  " + propertyName);
#endif
		}

		private static MemberExpression GetMemberExpression(Expression expression)
		{
			while(!(expression is MemberExpression))
			{
				if(expression is LambdaExpression)
					expression = ((LambdaExpression)expression).Body;
				else if(expression is UnaryExpression)
					expression = ((UnaryExpression)expression).Operand;
				else
					throw new ArgumentException("Expression body is not a MemberExpression.", "expression");
			}
			return (MemberExpression)expression;
		}

		/// <summary>
		/// Provides automatic property change notification.
		/// </summary>
		/// <typeparam name="T">The type of the property.</typeparam>
		public class Property<T>
		{
			private Action<string> FirePropertyChanged;
			private string propertyName;
			private T value;

			public Property(Expression<Func<T>> expression)
			{
				var memberExpression = GetMemberExpression(expression);
				var constantExpression = memberExpression.Expression as ConstantExpression;
				if(constantExpression == null)
					throw new ArgumentException("Member expression does not refer to an object.", "expression");
				var observableObject = constantExpression.Value as ObservableObject;
				if(observableObject == null)
					throw new ArgumentException("Member expression does not refer to an ObservableObject.", "expression");
				FirePropertyChanged = observableObject.FirePropertyChanged;
				propertyName = memberExpression.Member.Name;
			}

			public Property(Expression<Func<T>> expression, T value)
				: this(expression)
			{
				this.value = value;
			}

			public Property(Expression<Func<T>> expression, Action<string> firePropertyChanged)
			{
				var memberExpression = GetMemberExpression(expression);
				if(firePropertyChanged == null)
					throw new ArgumentNullException("firePropertyChanged");
				propertyName = memberExpression.Member.Name;
				FirePropertyChanged = firePropertyChanged;
			}

			public Property(Expression<Func<T>> expression, Action<string> firePropertyChanged, T value)
				: this(expression, firePropertyChanged)
			{
				this.value = value;
			}

			/// <summary>
			/// Gets or sets the value of the referenced property and fires a
			/// change notification if setting a different value.
			/// </summary>
			public T Value
			{
				get { return value; }
				set
				{
					if(!EqualityComparer<T>.Default.Equals(this.value, value))
					{
						this.value = value;
						FirePropertyChanged(propertyName);
					}
				}
			}
		}
	}
}
