using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;

namespace Adrezdi.Silverlight.Controls
{
	public class UploadComponent : DependencyObject
	{
		public event EventHandler<ProgressUpdatedEventArgs> ProgressUpdated;
		public event EventHandler<WriteCompletedEventArgs> WriteCompleted;
		private WebClient client = new WebClient();
		private bool canceled;

		public WebHeaderCollection Headers
		{
			get { return client.Headers; }
			set { client.Headers = value; }
		}

		/// <summary>
		/// Cancels a pending asynchronous upload operation.
		/// </summary>
		public void CancelAsync()
		{
			canceled = true;
			client.CancelAsync();
		}

		/// <summary>
		/// Uploads the file given by <paramref name="fileInfo" /> to the resource
		/// given by <paramref name="targetUri" />.
		/// </summary>
		/// <remarks>
		/// If not null, the <paramref name="contentType" /> overwrites the
		/// "Content-Type" header.  The file name given in <paramref name="fileInfo" />
		/// sets the "Content-Name" header.
		/// </remarks>
		public void UploadAsync(FileInfo fileInfo, Uri targetUri, object userState)
		{
			if(fileInfo == null)
				throw new ArgumentNullException("fileInfo");
			if(targetUri == null)
				throw new ArgumentNullException("targetUri");
			if(client.IsBusy)
				throw new InvalidOperationException(string.Format("This {0} is already in use.", GetType().Name));
			canceled = false;
			client.Headers["Content-Name"] = Path.GetFileNameWithoutExtension(fileInfo.Name);
			client.OpenWriteCompleted += client_OpenWriteCompleted;
			client.OpenWriteAsync(targetUri, null, new object[] { fileInfo, userState });
		}

		/// <remarks>
		/// The worker thread below invokes this method.
		/// </remarks>
		private void FireProgressUpdated(double progress, object userState)
		{
			if(ProgressUpdated != null)
				Dispatcher.BeginInvoke(() => ProgressUpdated(this, new ProgressUpdatedEventArgs(progress, userState)));
		}

		private void FireWriteCompleted(OpenWriteCompletedEventArgs e)
		{
			if(WriteCompleted != null)
			{
				var state = (object[])e.UserState;
				WriteCompleted(this, new WriteCompletedEventArgs(e.Error, state[1], e.Cancelled));
			}
		}

		/// <remarks>
		/// The worker thread below invokes this method.
		/// </remarks>
		private void FireWriteCompleted(Exception ex, object userState)
		{
			if(WriteCompleted != null)
				Dispatcher.BeginInvoke(() => WriteCompleted(this, new WriteCompletedEventArgs(ex, userState, canceled)));
		}

		private void client_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
		{
			if(e.Error != null || e.Cancelled)
				FireWriteCompleted(e);
			else
			{
				ThreadPool.QueueUserWorkItem(nil =>
				{
					var state = (object[])e.UserState;
					var fileInfo = (FileInfo)state[0];
					Stream fileStream = fileInfo.OpenRead();
					try
					{
						var buffer = new byte[4096];
						double writtenSize = 0;
						long fileSize = fileInfo.Length;
						while(!canceled && writtenSize < fileSize)
						{
							int i = fileStream.Read(buffer, 0, buffer.Length);
							e.Result.Write(buffer, 0, i);
							writtenSize += i;
							FireProgressUpdated(writtenSize / fileSize, state[1]);
						}
						FireWriteCompleted(null, state[1]);
					}
					catch(Exception ex)
					{
						FireWriteCompleted(ex, state[1]);
					}
					finally
					{
						e.Result.Close();
						fileStream.Close();
					}
				});
			}
		}
	}

	public class ProgressUpdatedEventArgs : EventArgs
	{
		public double Progress { get; private set; }
		public object UserState { get; private set; }

		public ProgressUpdatedEventArgs(double progress, object userState)
		{
			Progress = progress;
			UserState = userState;
		}
	}

	public class WriteCompletedEventArgs : EventArgs
	{
		public Exception Error { get; private set; }
		public object UserState { get; private set; }
		public bool Canceled { get; private set; }

		public WriteCompletedEventArgs(Exception error, object userState, bool canceled)
		{
			Error = error;
			UserState = userState;
			Canceled = canceled;
		}
	}
}
