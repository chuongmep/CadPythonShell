using Autodesk.AutoCAD.Runtime;

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
            dialog.ShowDialog();

        }
    }
}
