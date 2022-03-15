using System;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Threading;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using CADRuntime;
using Microsoft.Scripting;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = System.Exception;
using Forms = System.Windows.Forms;

namespace CADPythonShell
{
    public class IronPythonConsoleCommand
    {
        /// <summary>
        /// Open a window to let the user enter python code.
        /// </summary>
        /// <returns></returns>
        [CommandMethod("PythonConsole")]
        public void Execute()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            //load the application
            if (!CADPythonShellApplication.applicationLoaded)
        	{
        		CADPythonShellApplication.OnLoaded();
        	}
        	
            var gui = new IronPythonConsole();
            gui.consoleControl.WithConsoleHost((host) =>
            {
                // now that the console is created and initialized, the script scope should
                // be accessible...
                new ScriptExecutor(CADPythonShellApplication.GetConfig() )
                    .SetupEnvironment(host.Engine, host.Console.ScriptScope);

                host.Console.ScriptScope.SetVariable("__window__", gui);

                // run the initscript
                var initScript = CADPythonShellApplication.GetInitScript();
                if (initScript != null)
                {
                    try
                    {
                    	var scriptSource = host.Engine.CreateScriptSourceFromString(initScript, SourceCodeKind.Statements);
                    	scriptSource.Execute(host.Console.ScriptScope);
                    }
                    catch (Exception ex)
                    {
                    	Forms.MessageBox.Show(ex.ToString(), "Something went horribly wrong!");
                    }
                }                
            });

            var dispatcher = Dispatcher.FromThread(Thread.CurrentThread);
            gui.consoleControl.WithConsoleHost((host) =>
            {                
                host.Console.SetCommandDispatcher((command) =>
                {
                    if (command != null)
                    {
                        // Slightly involved form to enable keyboard interrupt to work.
                        var executing = true;
                        var operation = dispatcher.BeginInvoke(DispatcherPriority.Normal, command);
                        while (executing)
                        {
                            if (operation.Status != DispatcherOperationStatus.Completed)
                                operation.Wait(TimeSpan.FromSeconds(1));
                            if (operation.Status == DispatcherOperationStatus.Completed)
                                executing = false;
                        }
                    }                 
                });
                host.Editor.SetCompletionDispatcher((command) =>
                {
                    var executing = true;
                    var operation = dispatcher.BeginInvoke(DispatcherPriority.Normal, command);
                    while (executing)
                    {
                        if (operation.Status != DispatcherOperationStatus.Completed)
                            operation.Wait(TimeSpan.FromSeconds(1));
                        if (operation.Status == DispatcherOperationStatus.Completed)
                            executing = false;
                    }
                });
            });
            WindowInteropHelper helper = new WindowInteropHelper(gui);
            IntPtr hander = Application.MainWindow.Handle;
            helper.Owner = hander;
            gui.Show();
        }
    }    
}
