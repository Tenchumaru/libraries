using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Adrezdi.Silverlight.Controls
{
	/// <summary>
	/// Provides feedback for uploading files.
	/// </summary>
	/// <remarks>
	/// In order for this implementation to use the GradientBrush, it must have
	/// at least three GradientStops.
	/// </remarks>
	[TemplatePart(Name = "RootElement", Type = typeof(Panel))]
	[TemplatePart(Name = "BodyElement", Type = typeof(Shape))]
	[TemplatePart(Name = "GradientBrush", Type = typeof(GradientBrush))]
	public class UploadControl : Control
	{
		public event EventHandler<ProgressUpdatedEventArgs> ProgressUpdated;
		public event EventHandler<WriteCompletedEventArgs> WriteCompleted;
		public double Progress { get; private set; }
		private GradientBrush gradientBrush;
		private UploadComponent component;

		public bool IsBusy
		{
			get { return component != null; }
		}

		public UploadControl()
		{
			DefaultStyleKey = typeof(UploadControl);
		}

		public void BeginUpload(FileInfo fileInfo, Uri uri, WebHeaderCollection headers)
		{
			if(fileInfo == null)
				throw new ArgumentNullException("fileInfo");
			if(uri == null)
				throw new ArgumentNullException("uri");
			if(IsBusy)
				throw new InvalidOperationException("The control is uploading a file.");
			component = new UploadComponent();
			component.ProgressUpdated += component_ProgressUpdated;
			component.WriteCompleted += component_WriteCompleted;
			component.Headers = headers;
			component.UploadAsync(fileInfo, uri, null);
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			gradientBrush = GetTemplateChild("GradientBrush") as GradientBrush;
			if(gradientBrush != null)
			{
				if(gradientBrush.GradientStops.Count < 3)
					gradientBrush = null;
				else
				{
					var background = Background as SolidColorBrush;
					var foreground = Foreground as SolidColorBrush;
					if(background != null && foreground != null)
					{
						gradientBrush.GradientStops[0].Color = gradientBrush.GradientStops[1].Color = foreground.Color;
						gradientBrush.GradientStops[2].Color = background.Color;
						if(gradientBrush.GradientStops.Count > 3)
							gradientBrush.GradientStops[3].Color = background.Color;
					}
				}
			}
		}

		private void component_WriteCompleted(object sender, WriteCompletedEventArgs e)
		{
			component = null;
			if(WriteCompleted != null)
				WriteCompleted(this, e);
		}

		private void component_ProgressUpdated(object sender, ProgressUpdatedEventArgs e)
		{
			Progress = e.Progress;
			if(gradientBrush != null)
				gradientBrush.GradientStops[1].Offset = gradientBrush.GradientStops[2].Offset = e.Progress;
			if(ProgressUpdated != null)
				ProgressUpdated(this, e);
		}
	}
}
