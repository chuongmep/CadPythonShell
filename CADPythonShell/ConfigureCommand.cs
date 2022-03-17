using Autodesk.AutoCAD.Runtime;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CADPythonShell
{
    /// <summary>
    /// Open the configuration dialog.
    /// </summary>

    public class ConfigureCommand
    {
        [CommandMethod("PythonShellSetting")]
        public void Execute()
        {
            //load the application
        	if (!CADPythonShellApplication.applicationLoaded)
        	{
        		CADPythonShellApplication.OnLoaded();
        	}
        	
        	var dialog = new ConfigureCommandsForm();
            dialog.StartPosition = FormStartPosition.CenterScreen;
            NativeWindow nativeWindow = new NativeWindow();
            nativeWindow.AssignHandle(Application.MainWindow.Handle);
            dialog.ShowDialog(nativeWindow);

        }
    }
}
