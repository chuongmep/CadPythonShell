using CADRuntime;
using System.IO;

namespace CADPythonShell
{
    /// <summary>
    /// Starts up a ScriptOutput window for a given canned command.
    ///
    /// It is expected that this will be inherited by dynamic types that have the field
    /// _scriptSource set to point to a python file that will be executed in the constructor.
    /// </summary>

    public abstract class CommandLoaderBase
    {
        protected string _scriptSource;

        private CommandLoaderBase(string scriptSource)
        {
            _scriptSource = scriptSource;
        }

        /// <summary>
        /// Overload this method to implement an external command within CAD.
        /// </summary>
        /// <returns>
        /// The result indicates if the execution fails, succeeds, or was canceled by user. If it does not
        /// succeed, Revit will undo any changes made by the external command.
        /// </returns>
        private int Execute(ref string message, params string[] parameters)
        {
            // FIXME: somehow fetch back message after script execution...
            var executor = new ScriptExecutor(CADPythonShellApplication.GetConfig());

            string source;
            using (var reader = File.OpenText(_scriptSource))
            {
                source = reader.ReadToEnd();
            }

            var result = executor.ExecuteScript(source, _scriptSource);
            message = executor.Message;

            return result;
        }
    }
}