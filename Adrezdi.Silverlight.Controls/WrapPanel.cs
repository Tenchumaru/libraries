namespace System.Windows.Controls
{
	public class WrapPanel : Panel
	{
		public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight",
			typeof(double), typeof(WrapPanel),
			new PropertyMetadata(double.NaN, (d, e) => ((WrapPanel)d).CheckWidthHeight(e.NewValue)));
		public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth",
			typeof(double), typeof(WrapPanel),
			new PropertyMetadata(double.NaN, (d, e) => ((WrapPanel)d).CheckWidthHeight(e.NewValue)));
		public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
			typeof(Orientation), typeof(WrapPanel),
			new PropertyMetadata(Orientation.Horizontal, (d, e) => ((WrapPanel)d).UpdateLayout()));

		public double ItemHeight
		{
			get { return (double)base.GetValue(ItemHeightProperty); }
			set { base.SetValue(ItemHeightProperty, value); }
		}

		public double ItemWidth
		{
			get { return (double)base.GetValue(ItemWidthProperty); }
			set { base.SetValue(ItemWidthProperty, value); }
		}

		public Orientation Orientation
		{
			get { return (Orientation)GetValue(OrientationProperty); }
			set { SetValue(OrientationProperty, value); }
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			bool isHorizontal = Orientation == Orientation.Horizontal;
			double itemWidth = ItemWidth;
			double itemHeight = ItemHeight;
			double setWidth = isHorizontal ? itemWidth : itemHeight;
			double finalWidth = isHorizontal ? finalSize.Width : finalSize.Height;
			bool hasItemWidth = !double.IsNaN(itemWidth);
			bool hasItemHeight = !double.IsNaN(itemHeight);
			bool useSetWidth = isHorizontal ? hasItemWidth : hasItemHeight;
			UIElementCollection internalChildren = Children;
			var rowSize = new Size();
			int start = 0;
			double top = 0;
			for(int end = 0, count = internalChildren.Count; end < count; ++end)
			{
				UIElement element = internalChildren[end];
				if(element != null)
				{
					Size desiredSize = element.DesiredSize;
					double desiredWidth = hasItemWidth ? itemWidth : desiredSize.Width;
					double desiredHeight = hasItemHeight ? itemHeight : desiredSize.Height;
					var itemSize = new Size(isHorizontal ? desiredWidth : desiredHeight,
						isHorizontal ? desiredHeight : desiredWidth);
					if(rowSize.Width + itemSize.Width > finalWidth)
					{
						ArrangeLine(top, rowSize.Height, start, end, useSetWidth, setWidth);
						top += rowSize.Height;
						start = end;
						if(itemSize.Width > finalWidth)
						{
							++end;
							ArrangeLine(top, itemSize.Height, start, end, useSetWidth, setWidth);
							top += itemSize.Height;
							rowSize = new Size();
						}
						else
							rowSize = itemSize;
					}
					else
					{
						rowSize.Width += itemSize.Width;
						rowSize.Height = Math.Max(itemSize.Height, rowSize.Height);
					}
				}
			}
			if(start < internalChildren.Count)
				ArrangeLine(top, rowSize.Height, start, internalChildren.Count, useSetWidth, setWidth);
			return finalSize;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			bool isHorizontal = Orientation == Orientation.Horizontal;
			if(!isHorizontal)
				availableSize = new Size(availableSize.Height, availableSize.Width);
			var rowSize = new Size();
			var totalSize = new Size();
			double availableWidth = availableSize.Width;
			double itemWidth = ItemWidth;
			double itemHeight = ItemHeight;
			bool hasItemWidth = !double.IsNaN(itemWidth);
			bool hasItemHeight = !double.IsNaN(itemHeight);
			var childAvailableSize = new Size(hasItemWidth ? itemWidth : availableSize.Width, hasItemHeight ? itemHeight : availableSize.Height);
			foreach(var element in Children)
			{
				if(element != null)
				{
					element.Measure(childAvailableSize);
					double childItemWidth = hasItemWidth ? itemWidth : element.DesiredSize.Width;
					double childItemHeight = hasItemHeight ? itemHeight : element.DesiredSize.Height;
					var itemSize = new Size(isHorizontal ? childItemWidth : childItemHeight,
						isHorizontal ? childItemHeight : childItemWidth);
					if(rowSize.Width + itemSize.Width > availableWidth)
					{
						totalSize.Width = Math.Max(rowSize.Width, totalSize.Width);
						totalSize.Height += rowSize.Height;
						if(itemSize.Width > availableWidth)
						{
							totalSize.Width = Math.Max(itemSize.Width, totalSize.Width);
							totalSize.Height += itemSize.Height;
							rowSize = new Size();
						}
						else
							rowSize = itemSize;
					}
					else
					{
						rowSize.Width += itemSize.Width;
						rowSize.Height = Math.Max(itemSize.Height, rowSize.Height);
					}
				}
			}
			totalSize.Width = Math.Max(rowSize.Width, totalSize.Width);
			totalSize.Height += rowSize.Height;
			return isHorizontal ? totalSize : new Size(totalSize.Height, totalSize.Width);
		}

		private void ArrangeLine(double top, double lineHeight, int start, int end, bool useItemWidth, double itemWidth)
		{
			double left = 0;
			bool isHorizontal = Orientation == Orientation.Horizontal;
			UIElementCollection internalChildren = Children;
			for(int i = start; i < end; ++i)
			{
				UIElement element = internalChildren[i];
				if(element != null)
				{
					double desiredWidth = isHorizontal ? element.DesiredSize.Width : element.DesiredSize.Height;
					double actualWidth = useItemWidth ? itemWidth : desiredWidth;
					element.Arrange(isHorizontal
						? new Rect(left, top, actualWidth, lineHeight)
						: new Rect(top, left, lineHeight, actualWidth));
					left += actualWidth;
				}
			}
		}

		private void CheckWidthHeight(object newValue)
		{
			var n = (double)newValue;
			if(double.IsNaN(n) || (n >= 0 && !double.IsPositiveInfinity(n)))
				UpdateLayout();
			else
				throw new ArgumentOutOfRangeException();
		}
	}
}
