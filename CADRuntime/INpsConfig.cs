using System.Collections.Generic;

namespace CADRuntime
{
    public interface ICpsConfig
    {
        /// <summary>
        /// Returns a list of string variables that the Runtime will add to
        /// the scripts scope under "__vars__".
        ///
        /// In CADPythonShell, these are read from the CADPythonShell.xml file.
        /// </summary>
        IDictionary<string, string> GetVariables();

        /// <summary>
        /// Returns a list of paths to add to the python engine search paths.
        ///
        /// In CADPythonShell, these are read from the CADPythonShell.xml file.
        /// </summary>
        IEnumerable<string> GetSearchPaths();
    }
}