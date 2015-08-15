using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Interop;

namespace Adrezdi.Windows
{
	public class SelectFolderDialog
	{
		private const int MAX_PATH = 260;

		/// <summary>
		/// Location of the root folder from which to start browsing.  Only the
		/// specified folder and any folders beneath it in the namespace
		/// hierarchy appear in the dialog box.
		/// </summary>
		public FolderID StartLocation { get; set; }

		public string Title { get; set; }

		public Win32.Shell.BrowseInfoFlags Flags { get; set; }

		/// <summary>
		/// Gets the full path to the folder selected by the user.
		/// </summary>
		public string FolderPath { get; private set; }

		/// <summary>
		/// Enumeration of CSIDLs identifying standard shell folders.
		/// </summary>
		public enum FolderID
		{
			Desktop = 0x0000,
			Printers = 0x0004,
			MyDocuments = 0x0005,
			Favorites = 0x0006,
			Recent = 0x0008,
			SendTo = 0x0009,
			StartMenu = 0x000b,
			MyComputer = 0x0011,
			NetworkNeighborhood = 0x0012,
			Templates = 0x0015,
			MyPictures = 0x0027,
			NetAndDialUpConnections = 0x0031,
		}

		public SelectFolderDialog()
		{
			Title = "Please select a folder. Or, type or paste the full path into the text box.";
			Flags = Win32.Shell.BrowseInfoFlags.RestrictToFilesystem | Win32.Shell.BrowseInfoFlags.UseNewUI;
		}

		/// <summary>
		/// Shows the folder browser dialog box with the active window as the owner.
		/// </summary>
		public bool ShowDialog()
		{
			return ShowDialog(null);
		}

		/// <summary>
		/// Shows the folder browser dialog box with the specified owner window.
		/// </summary>
		public bool ShowDialog(IWin32Window owner)
		{
			// Get an owner window handle for this dialog.
			IntPtr hWndOwner = owner != null ? owner.Handle : Win32.GetActiveWindow();

			// Turn off the new style flag if in the multi-threaded apartment.
			Win32.Shell.BrowseInfoFlags flags = Flags;
			if((flags & Win32.Shell.BrowseInfoFlags.NewDialogStyle) != 0)
			{
				if(Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
					flags &= ~Win32.Shell.BrowseInfoFlags.NewDialogStyle;
			}

			// Get the IDL for the specific start location.
			IntPtr pidlRoot;
			Win32.Shell.SHGetSpecialFolderLocation(hWndOwner, (int)StartLocation, out pidlRoot);
			if(pidlRoot == IntPtr.Zero)
				return false;

			try
			{
				// Construct a BROWSEINFO structure.
				var bi = new Win32.Shell.BROWSEINFO();
				IntPtr buffer = Marshal.AllocHGlobal(MAX_PATH);

				bi.pidlRoot = pidlRoot;
				bi.hwndOwner = hWndOwner;
				bi.pszDisplayName = buffer;
				bi.lpszTitle = Title;
				bi.ulFlags = flags;
				// The constructor sets the rest of the fields to zero.
				// bi.lpfn = null; bi.lParam = IntPtr.Zero; bi.iImage = 0;

				IntPtr pidlRet = IntPtr.Zero;

				try
				{
					// Show the dialog.
					pidlRet = Win32.Shell.SHBrowseForFolder(ref bi);

					// Free the buffer allocated on the global heap.
					Marshal.FreeHGlobal(buffer);

					if(pidlRet == IntPtr.Zero)
					{
						// The user clicked the Cancel button.
						return false;
					}

					// Retrieve the path from the IDList.
					var sb = new StringBuilder(MAX_PATH);
					if(!Win32.Shell.SHGetPathFromIDList(pidlRet, sb))
						return false;

					// Convert it to a string.
					FolderPath = sb.ToString();
				}
				finally
				{
					if(pidlRet != IntPtr.Zero)
						Marshal.FreeCoTaskMem(pidlRet);
				}

				return true;
			}
			finally
			{
				Marshal.FreeCoTaskMem(pidlRoot);
			}
		}
	}
}
