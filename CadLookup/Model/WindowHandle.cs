using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace CADSnoop.Model
{
    public static class WindowHandle
    {
        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

		/// <summary>
		/// Sets the given window's owner to Cad window.
		/// </summary>
		/// <param name="dialog">Target window.</param>
		public static void SetCadAsWindowOwner(this Window dialog)
		{
			if (null == dialog) { return; }

			WindowInteropHelper helper = new WindowInteropHelper(dialog);
			helper.Owner = FindCadWindowHandle();
		}

		/// <summary>
		/// Finds the Cad window handle.
		/// </summary>
		/// <returns>Revit window handle.</returns>
		public static IntPtr FindCadWindowHandle()
		{
			try
			{
				IntPtr foundRevitHandle = IntPtr.Zero;
				uint currentThreadID = GetCurrentThreadId();

				// Search for the Revit process with current thread ID.
				Process[] revitProcesses = Process.GetProcessesByName("acad");
				Process foundRevitProcess = null;
				foreach (Process aRevitProcess in revitProcesses)
				{
					foreach (ProcessThread aThread in aRevitProcess.Threads)
					{
						if (aThread.Id == currentThreadID)
						{
							foundRevitProcess = aRevitProcess;
							break;
						}
					}  // For each thread in the process.

					// When we have found our Revit process, then stop searching.
					if (null != foundRevitProcess) { break; }
				}  // For each revit process found

				if (null != foundRevitProcess)
				{
					foundRevitHandle = foundRevitProcess.MainWindowHandle;
				}

				return foundRevitHandle;
			}
			catch (Exception)
			{
				return IntPtr.Zero;
			}
		}
	}
}
