namespace System.Windows.Controls.Primitives
{
	using System.Linq;

	public class UniformGrid : Panel
	{
		public int Rows
		{
			get { return (int)GetValue(RowsProperty); }
			set { SetValue(RowsProperty, value); }
		}

		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.Register("Rows", typeof(int), typeof(UniformGrid),
			new PropertyMetadata(0, new PropertyChangedCallback(IntPropertyChanged)));

		public int Columns
		{
			get { return (int)GetValue(ColumnsProperty); }
			set { SetValue(ColumnsProperty, value); }
		}

		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.Register("Columns", typeof(int), typeof(UniformGrid),
			new PropertyMetadata(0, new PropertyChangedCallback(IntPropertyChanged)));

		public int FirstColumn
		{
			get { return (int)GetValue(FirstColumnProperty); }
			set { SetValue(FirstColumnProperty, value); }
		}

		public static readonly DependencyProperty FirstColumnProperty =
			DependencyProperty.Register("FirstColumn", typeof(int), typeof(UniformGrid),
			new PropertyMetadata(0, new PropertyChangedCallback(IntPropertyChanged)));

		private static void IntPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Use the previous value if the new value is negative.
			if((int)e.NewValue < 0)
				d.SetValue(e.Property, e.OldValue);
			else
				((UniformGrid)d).Refresh();
		}

		private int rows, columns;

		private void Refresh()
		{
			InvalidateMeasure();
			InvalidateArrange();
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			double itemWidth = arrangeSize.Width / columns;
			var finalRect = new Rect(0, 0, itemWidth, arrangeSize.Height / rows);
			double finalWidth = arrangeSize.Width - 1;
			finalRect.X += itemWidth * FirstColumn;
			foreach(UIElement element in Children)
			{
				element.Arrange(finalRect);
				if(element.Visibility != Visibility.Collapsed)
				{
					finalRect.X += itemWidth;
					if(finalRect.X >= finalWidth)
					{
						finalRect.Y += finalRect.Height;
						finalRect.X = 0;
					}
				}
			}
			return arrangeSize;
		}

		protected override Size MeasureOverride(Size constraint)
		{
			UpdateComputedValues();
			var availableSize = new Size(constraint.Width / columns, constraint.Height / rows);
			double width = 0;
			double height = 0;
			foreach(UIElement element in Children)
			{
				element.Measure(availableSize);
				Size desiredSize = element.DesiredSize;
				if(width < desiredSize.Width)
					width = desiredSize.Width;
				if(height < desiredSize.Height)
					height = desiredSize.Height;
			}
			return new Size(width * columns, height * rows);
		}

		private void UpdateComputedValues()
		{
			columns = Columns;
			rows = Rows;
			if(FirstColumn >= columns)
				FirstColumn = 0;
			if(rows == 0 || columns == 0)
			{
				int count = Children.Count(e => e.Visibility != Visibility.Collapsed);
				if(count == 0)
					count = 1;
				if(rows > 0)
					columns = 1 + (count - 1) / rows;
				else if(columns > 0)
					rows = 1 + (count + FirstColumn - 1) / columns;
				else
				{
					rows = (int)Math.Sqrt(count);
					if(rows * rows < count)
						++rows;
					columns = rows;
				}
			}
		}
	}
}
