using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Media;
using DependencyObject = System.Windows.DependencyObject;

namespace Adrezdi.Windows
{
	public static class Extensions
	{
		public static MemberExpression GetMemberExpression(Expression expression)
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

		public static bool HasChanged<T>(this PropertyChangedEventArgs e, Expression<Func<T>> expression)
		{
			var memberExpression = GetMemberExpression(expression.Body);
			return e.PropertyName == memberExpression.Member.Name;
		}

		public static T FindAncestor<T>(this DependencyObject reference) where T : DependencyObject
		{
#if DEBUG
			try
			{
#endif
				for(; reference != null; reference = VisualTreeHelper.GetParent(reference))
				{
					Debug.Write(" -> " + reference.ToString().Replace("System.Windows.Controls.", ""));
					if(reference is T)
						return (T)reference;
				}
				return null;
#if DEBUG
			}
			finally
			{
				Debug.WriteLine("");
			}
#endif
		}

		/// <summary>
		/// Performs a breath-first search for descendants.
		/// </summary>
		/// <typeparam name="T">The type of descendant for which to search.</typeparam>
		/// <param name="reference">The object to search for descendants.</param>
		/// <returns>An enumerable of matching descendants.</returns>
		public static IEnumerable<T> FindDescendants<T>(this DependencyObject reference) where T : DependencyObject
		{
			var queue = new Queue<DependencyObject>();
			queue.Enqueue(reference);
			do
			{
				reference = queue.Dequeue();
				if(reference is T)
					yield return (T)reference;
				int count = VisualTreeHelper.GetChildrenCount(reference);
				for(int i = 0; i < count; ++i)
					queue.Enqueue(VisualTreeHelper.GetChild(reference, i));
			}
			while(queue.Any());
		}
	}
}
