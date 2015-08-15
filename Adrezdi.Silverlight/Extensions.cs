using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Adrezdi.Silverlight.Extensions
{
	public static class Extensions
	{
		public static T[] EnumGetValues<T>()
		{
			return InternalEnumGetValues(typeof(T)).Cast<T>().ToArray();
		}

		public static object[] EnumGetValues(Type enumType)
		{
			if(enumType == null)
				throw new ArgumentNullException("enumType");
			return InternalEnumGetValues(enumType).ToArray();
		}

		private static IEnumerable<object> InternalEnumGetValues(Type enumType)
		{
			if(!enumType.IsEnum)
				throw new ArgumentException("Type " + enumType.Name + " is not an enum.", "enumType");
			return from field in enumType.GetFields()
				   where field.IsLiteral
				   select field.GetValue(enumType);
		}

		/// <summary>
		/// Converts the elements in this list to another type and returns a
		/// list containing the converted elements.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of the source array.</typeparam>
		/// <typeparam name="TResult">The type of the elements of the target array.</typeparam>
		/// <param name="converter">A Converter delegate that converts each element from one type to another type.</param>
		/// <returns>A list of the target type containing the converted elements from this list.</returns>
		public static List<TResult> ConvertAll<TSource, TResult>(this List<TSource> source, Converter<TSource, TResult> converter)
		{
			return source.Select(s => converter(s)).ToList();
		}

		/// <summary>
		/// Retrieves the bounds of this element relative to the root visual
		/// (regardless of nesting, margins, etc.).
		/// </summary>
		public static Rect GetAbsoluteBounds(this FrameworkElement element)
		{
			GeneralTransform gt = element.TransformToVisual(Application.Current.RootVisual);
			Point topLeft = gt.Transform(new Point(0, 0));
			Point bottomRight = gt.Transform(new Point(element.ActualWidth, element.ActualHeight));
			return new Rect(topLeft, bottomRight);
		}

		public static Point GetCenter(this FrameworkElement element)
		{
			double x = element.ActualWidth / 2;
			double y = element.ActualHeight / 2;
			return new Point(x, y);
		}

		public static Point GetCenter(this Rect rect)
		{
			double x = rect.X + rect.Width / 2;
			double y = rect.Y + rect.Height / 2;
			return new Point(x, y);
		}

		public static bool IntersectsWith(this Rect me, Rect rect)
		{
			return !me.IsEmpty && !rect.IsEmpty
				&& rect.Left < me.Right && me.Left < rect.Right
				&& rect.Top < me.Bottom && me.Top < rect.Bottom;
		}

		public static Rect GetCenterScaledRect(this Rect me, double width, double height)
		{
			double aspectRatio = width / height;
			double scaledWidth = Math.Min(me.Height * aspectRatio, me.Width);
			double scaledHeight = Math.Min(me.Width / aspectRatio, me.Height);
			return new Rect(me.X + (me.Width - scaledWidth) / 2, me.Y + (me.Height - scaledHeight) / 2, scaledWidth, scaledHeight);
		}

		public static UIElement GetContent(this UserControl control)
		{
			return UC.GetContent(control);
		}

		public static UIElement GetRootContent(this Application application)
		{
			var control = application.RootVisual as UserControl;
			if(control == null)
				throw new InvalidOperationException("root visual is not a user control");
			return control.GetContent();
		}

		/// <remarks>
		/// This exists solely to allow the GetContent extension method to get
		/// access to the user control's Content property.
		/// </remarks>
		abstract class UC : UserControl
		{
			internal static UIElement GetContent(UserControl control)
			{
				return (UIElement)control.GetValue(UserControl.ContentProperty);
			}
		}
	}
}
