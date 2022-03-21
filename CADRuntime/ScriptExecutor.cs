using IronPython.Runtime.Exceptions;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CADRuntime
{
    /// <summary>
    /// Executes a script scripts
    /// </summary>
    public class ScriptExecutor
    {
        private string _message;
        private readonly ICpsConfig _config;

        public ScriptExecutor(ICpsConfig config)
        {
            _config = config;

            _message = "";
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        /// <summary>
        /// Run the script and print the output to a new output window.
        /// </summary>
        public int ExecuteScript(string source, string sourcePath)
        {
            try
            {
                var engine = CreateEngine();
                var scope = SetupEnvironment(engine);

                var scriptOutput = new ScriptOutput();
                scriptOutput.Show();
                var outputStream = new ScriptOutputStream(scriptOutput, engine);

                scope.SetVariable("__window__", scriptOutput);
                scope.SetVariable("__file__", sourcePath);

                // Add script directory address to sys search paths
                var path = engine.GetSearchPaths();
                path.Add(System.IO.Path.GetDirectoryName(sourcePath));
                engine.SetSearchPaths(path);

                engine.Runtime.IO.SetOutput(outputStream, Encoding.UTF8);
                engine.Runtime.IO.SetErrorOutput(outputStream, Encoding.UTF8);
                engine.Runtime.IO.SetInput(outputStream, Encoding.UTF8);

                var script = engine.CreateScriptSourceFromString(source, SourceCodeKind.Statements);
                var errors = new ErrorReporter();
                var command = script.Compile(errors);
                if (command == null)
                {
                    // compilation failed
                    _message = string.Join("\n", errors.Errors);
                    return -1;
                }

                try
                {
                    script.Execute(scope);

                    _message = (scope.GetVariable("__message__") ?? "").ToString();
                    return (int)(scope.GetVariable("__result__") ?? 0);
                }
                catch (SystemExitException)
                {
                    // ok, so the system exited. That was bound to happen...
                    return 0;
                }
                catch (Exception exception)
                {
                    // show (power) user everything!
                    _message = exception.ToString();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                _message = ex.ToString();
                return -1;
            }
        }

        private ScriptEngine CreateEngine()
        {
            var engine = IronPython.Hosting.Python.CreateEngine(new Dictionary<string, object>() { { "Frames", true }, { "FullFrames", true } });
            return engine;
        }

        private void AddEmbeddedLib(ScriptEngine engine)
        {
            // use embedded python lib
            var asm = this.GetType().Assembly;
            var resQuery = from name in asm.GetManifestResourceNames()
                           where name.ToLowerInvariant().EndsWith("python_27_lib.zip")
                           select name;
            var resName = resQuery.Single();
            var importer = new IronPython.Modules.ResourceMetaPathImporter(asm, resName);
            dynamic sys = IronPython.Hosting.Python.GetSysModule(engine);
            sys.meta_path.append(importer);
        }

        /// <summary>
        /// Set up an IronPython environment - for interactive shell or for canned scripts
        /// </summary>
        public ScriptScope SetupEnvironment(ScriptEngine engine)
        {
            var scope = IronPython.Hosting.Python.CreateModule(engine, "__main__");

            SetupEnvironment(engine, scope);

            return scope;
        }

        public void SetupEnvironment(ScriptEngine engine, ScriptScope scope)
        {
            // these variables refer to the signature of the IExternalCommand.Snoop method
            scope.SetVariable("__message__", _message);
            scope.SetVariable("__result__", 0);

            // add two special variables: __revit__ and __vars__ to be globally visible everywhere:
            var builtin = IronPython.Hosting.Python.GetBuiltinModule(engine);
            builtin.SetVariable("__vars__", _config.GetVariables());

            // add the search paths
            AddSearchPaths(engine);
            AddEmbeddedLib(engine);

            // reference Autocad or Civil3D Api Document and Application
            engine.Runtime.LoadAssembly(typeof(Autodesk.AutoCAD.ApplicationServices.Document).Assembly);
            engine.Runtime.LoadAssembly(typeof(CADSnoop.SnoopCommand).Assembly);
            // also, allow access to the RPS internals
            engine.Runtime.LoadAssembly(typeof(ScriptExecutor).Assembly);
        }

        /// <summary>
        /// Be nasty and reach into the ScriptScope to get at its private '_scope' member,
        /// since the accessor 'ScriptScope.Scope' was defined 'internal'.
        /// </summary>
        private Microsoft.Scripting.Runtime.Scope GetScope(ScriptScope scriptScope)
        {
            var field = scriptScope.GetType().GetField(
                "_scope",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (Microsoft.Scripting.Runtime.Scope)field.GetValue(scriptScope);
        }

        /// <summary>
        /// Add the search paths defined in the ini file to the engine.
        /// The data folder CADPythonShell20XX is also added
        /// </summary>
        private void AddSearchPaths(ScriptEngine engine)
        {
            var searchPaths = engine.GetSearchPaths();
            foreach (var path in _config.GetSearchPaths())
            {
                searchPaths.Add(path);
            }
            engine.SetSearchPaths(searchPaths);
        }
    }

    public class ErrorReporter : ErrorListener
    {
        public List<String> Errors = new List<string>();

        public override void ErrorReported(ScriptSource source, string message, SourceSpan span, int errorCode, Severity severity)
        {
            Errors.Add(string.Format("{0} (line {1})", message, span.Start.Line));
        }

        public int Count
        {
            get { return Errors.Count; }
        }
    }
}