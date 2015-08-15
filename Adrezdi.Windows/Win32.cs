using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Adrezdi.Windows
{
	public static class Win32
	{
		[DllImport("user32")]
		public static extern IntPtr GetActiveWindow();

		public static class Shell
		{
			// Flags used in the BROWSEINFO.ulFlags field.
			[Flags]
			public enum BrowseInfoFlags
			{
				RestrictToFilesystem = 0x0001, // BIF_RETURNONLYFSDIRS
				RestrictToDomain = 0x0002, // BIF_DONTGOBELOWDOMAIN
				ShowStatusText = 0x0004, // BIF_STATUSTEXT
				RestrictToSubfolders = 0x0008, // BIF_RETURNFSANCESTORS
				ShowTextBox = 0x0010, // BIF_EDITBOX
				ValidateSelection = 0x0020, // BIF_VALIDATE
				NewDialogStyle = 0x0040, // BIF_NEWDIALOGSTYLE
				AllowUrls = 0x0080, // BIF_BROWSEINCLUDEURLS
				ShowUsageHint = 0x0100, // BIF_UAHINT
				HideNewFolderButton = 0x0200, // BIF_NONEWFOLDERBUTTON
				ReturnShortcuts = 0x0400, // BIF_NOTRANSLATETARGETS
				BrowseForComputer = 0x1000, // BIF_BROWSEFORCOMPUTER
				BrowseForPrinter = 0x2000, // BIF_BROWSEFORPRINTER
				BrowseForEverything = 0x4000, // BIF_BROWSEINCLUDEFILES
				BrowseForShares = 0x8000, // BIF_SHAREABLE
				BrowseForJunctions = 0x10000, // BIF_BROWSEFILEJUNCTIONS
				UseNewUI = ShowTextBox | NewDialogStyle, // BIF_USENEWUI
			}

			// Delegate type used in BROWSEINFO.lpfn field.
			public delegate int BFFCALLBACK(IntPtr hwnd, uint uMsg, IntPtr lParam, IntPtr lpData);

			[StructLayout(LayoutKind.Sequential, Pack = 8)]
			public struct BROWSEINFO
			{
				public IntPtr hwndOwner;
				public IntPtr pidlRoot;
				public IntPtr pszDisplayName;
				[MarshalAs(UnmanagedType.LPTStr)]
				public string lpszTitle;
				[MarshalAs(UnmanagedType.U4)]
				public Win32.Shell.BrowseInfoFlags ulFlags;
				[MarshalAs(UnmanagedType.FunctionPtr)]
				public BFFCALLBACK lpfn;
				public IntPtr lParam;
				public int iImage;
			}

			[DllImport("shell32", CharSet = CharSet.Auto)]
			public static extern IntPtr SHBrowseForFolder(ref BROWSEINFO bi);

			[DllImport("shell32")]
			public static extern bool SHGetPathFromIDList(IntPtr pidl, StringBuilder Path);

			[DllImport("shell32")]
			public static extern int SHGetSpecialFolderLocation(IntPtr hwndOwner, int nFolder, out IntPtr ppidl);
		}
	}
}
