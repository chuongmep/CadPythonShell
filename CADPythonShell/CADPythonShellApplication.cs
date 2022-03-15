using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using CADRuntime;
using Forms = System.Windows.Forms;

namespace CADPythonShell
{
    static class CADPythonShellApplication
    {
        private static string settingsFolder;
        public static bool applicationLoaded;

        /// <summary>
        /// Hook into Revit to allow starting a command.
        /// </summary>
        public static void OnLoaded()
        {
            try
            {
                applicationLoaded = true;
                var dllfolder = GetSettingsFolder();
                settingsFolder = dllfolder;

                var settings = GetSettings();

                //var assemblyName = "CommandLoaderAssembly";
                //var dllfullpath = Path.Combine(dllfolder, assemblyName + ".dll");
                //CreateCommandLoaderAssembly(settings, dllfolder, assemblyName);
                //seems like I need to pre-load my dependencies
                AppDomain.CurrentDomain.Load(typeof(NpsConfig).Assembly.GetName());

                ExecuteStartupScript();
                return;
            }
            catch (Exception ex)
            {
                Forms.MessageBox.Show(ex.ToString(), "Error setting up CADPythonShell");
                return;
            }
        }

        private static void ExecuteStartupScript()
        {
            // execute StartupScript
            var startupScript = GetStartupScript();
            if (startupScript != null)
            {
                var executor = new ScriptExecutor(GetConfig());
                var result = executor.ExecuteScript(startupScript, GetStartupScriptPath());
                if (result == -1)
                {
                    Forms.MessageBox.Show(executor.Message, "CADPythonShell - StartupScript");
                }
            }
        }

        private static ImageSource GetEmbeddedBmp(System.Reflection.Assembly app, string imageName)
        {
            var file = app.GetManifestResourceStream(imageName);
            var source = BmpBitmapDecoder.Create(file, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            return source.Frames[0];
        }

        private static ImageSource GetEmbeddedPng(System.Reflection.Assembly app, string imageName)
        {
            var file = app.GetManifestResourceStream(imageName);
            var source = PngBitmapDecoder.Create(file, BitmapCreateOptions.None, BitmapCacheOption.None);
            return source.Frames[0];
        }


        /// <summary>
        /// Creates a dynamic assembly that contains types for starting the canned commands.
        /// </summary>
        private static void CreateCommandLoaderAssembly(XDocument repository, string dllfolder, string dllname)
        {
            var assemblyName = new AssemblyName { Name = dllname + ".dll", Version = new Version(1, 0, 0, 0) };
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave, dllfolder);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("CommandLoaderModule", dllname + ".dll");

            foreach (var command in GetCommands(repository))
            {
                var typebuilder = moduleBuilder.DefineType("Command" + command.Index,
                                                        TypeAttributes.Class | TypeAttributes.Public,
                                                        typeof(CommandLoaderBase));

                // call base constructor with script path
                var ci = typeof(CommandLoaderBase).GetConstructor(new[] { typeof(string) });

                var constructorBuilder = typebuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[0]);
                var gen = constructorBuilder.GetILGenerator();
                gen.Emit(OpCodes.Ldarg_0);                // Load "this" onto eval stack
                gen.Emit(OpCodes.Ldstr, command.Source);  // Load the path to the command as a string onto stack
                gen.Emit(OpCodes.Call, ci);               // call base constructor (consumes "this" and the string)
                gen.Emit(OpCodes.Nop);                    // Fill some space - this is how it is generated for equivalent C# code
                gen.Emit(OpCodes.Nop);
                gen.Emit(OpCodes.Nop);
                gen.Emit(OpCodes.Ret);                    // return from constructor
                typebuilder.CreateType();
            }
            assemblyBuilder.Save(dllname + ".dll");
        }

        public static void OnUnloading()
        {
            // FIXME: deallocate the python shell...
            return;
        }

        public static IRpsConfig GetConfig()
        {
            return new NpsConfig(GetSettingsFile());
        }

        /// <summary>
        /// Returns a handle to the settings file.
        /// </summary>
        /// <returns></returns>
        public static XDocument GetSettings()
        {
            string settingsFile = GetSettingsFile();
            return XDocument.Load(settingsFile);
        }

        private static string GetSettingsFile()
        {
            string folder = GetSettingsFolder();
            return Path.Combine(folder, "CADPythonShell.xml");
        }

        /// <summary>
        /// Returns the name of the folder with the settings file. This folder
        /// is also the default folder for relative paths in StartupScript and InitScript tags.
        /// </summary>
        private static string GetSettingsFolder()
        {
            if (!string.IsNullOrEmpty(settingsFolder))
            {
                return settingsFolder;
            }
            //return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CADPythonShell" + versionNumber);
            return Path.GetDirectoryName(typeof(CADPythonShellApplication).Assembly.Location);
        }

        /// <summary>
        /// Returns a list of commands as defined in the repository file.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Command> GetCommands(XDocument repository)
        {
            int i = 0;
            foreach (var commandNode in repository.Root.Descendants("Command") ?? new List<XElement>())
            {
                var addinAssembly = typeof(CADPythonShellApplication).Assembly;
                var commandName = commandNode.Attribute("name").Value;
                var commandSrc = commandNode.Attribute("src").Value;
                var group = commandNode.Attribute("group") == null ? "" : commandNode.Attribute("group").Value;

                ImageSource largeImage = null;
                if (IsValidPath(commandNode.Attribute("largeImage")))
                {
                    var largeImagePath = GetAbsolutePath(commandNode.Attribute("largeImage").Value);
                    largeImage = BitmapDecoder.Create(File.OpenRead(largeImagePath), BitmapCreateOptions.None, BitmapCacheOption.None).Frames[0];
                }
                else
                {
                    largeImage = GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.PythonScript32x32.png");
                }

                ImageSource smallImage = null;
                if (IsValidPath(commandNode.Attribute("smallImage")))
                {
                    var smallImagePath = GetAbsolutePath(commandNode.Attribute("smallImage").Value);
                    smallImage = BitmapDecoder.Create(File.OpenRead(smallImagePath), BitmapCreateOptions.None, BitmapCacheOption.None).Frames[0];
                }
                else
                {
                    smallImage = GetEmbeddedPng(addinAssembly, "CADPythonShell.Resources.PythonScript16x16.png");
                }

                yield return new Command
                {
                    Name = commandName,
                    Source = commandSrc,
                    Group = group,
                    LargeImage = largeImage,
                    SmallImage = smallImage,
                    Index = i++
                };
            }
        }

        /// <summary>
        /// True, if the contents of the attribute is a valid absolute path (or relative path to the assembly) is
        /// an existing path.
        /// </summary>
        private static bool IsValidPath(XAttribute pathAttribute)
        {
            if (pathAttribute != null && !string.IsNullOrEmpty(pathAttribute.Value))
            {
                return File.Exists(GetAbsolutePath(pathAttribute.Value));
            }
            return false;
        }

        /// <summary>
        /// Return an absolute path for input path, with relative paths seen as
        /// relative to the assembly location. No guarantees are made as to
        /// wether the path exists or not.
        /// </summary>
        private static string GetAbsolutePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            else
            {
                var assembly = typeof(CADPythonShellApplication).Assembly;
                return Path.Combine(Path.GetDirectoryName(assembly.Location), path);
            }
        }

        /// <summary>
        /// Returns a string to be executed, whenever the interactive shell is started.
        /// If this is not specified in the XML file (under /CADPythonShell/InitScript),
        /// then null is returned.
        /// </summary>
        public static string GetInitScript()
        {
            var path = GetInitScriptPath();
            if (File.Exists(path))
            {
                using (var reader = File.OpenText(path))
                {
                    var source = reader.ReadToEnd();
                    return source;
                }
            }

            // backwards compatibility: InitScript used to have a CDATA section directly
            // embedded in the settings xml file
            var initScriptTags = GetSettings().Root.Descendants("InitScript") ?? new List<XElement>();
            if (initScriptTags.Count() == 0)
            {
                return null;
            }
            var firstScript = initScriptTags.First();
            // backwards compatibility: InitScript used to be included as CDATA in the config file
            return firstScript.Value.Trim();
        }

        /// <summary>
        /// Returns the path to the InitScript as configured in the settings file or "" if not
        /// configured. This is used in the ConfigureCommandsForm.
        /// </summary>
        public static string GetInitScriptPath()
        {
            return GetScriptPath("InitScript");
        }


        /// <summary>
        /// Returns the path to the StartupScript as configured in the settings file or "" if not
        /// configured. This is used in the ConfigureCommandsForm.
        /// </summary>
        public static string GetStartupScriptPath()
        {
            return GetScriptPath("StartupScript");
        }

        /// <summary>
        /// Returns the value of the "src" attribute for the tag "tagName" in the settings file
        /// or "" if not configured.
        /// </summary>        
        private static string GetScriptPath(string tagName)
        {
            var tags = GetSettings().Root.Descendants(tagName) ?? new List<XElement>();
            if (tags.Count() == 0)
            {
                return "";
            }
            var firstScript = tags.First();
            if (firstScript.Attribute("src") != null)
            {
                var path = firstScript.Attribute("src").Value;
                if (Path.IsPathRooted(path))
                {
                    return path;
                }
                else
                {
                    return Path.Combine(GetSettingsFolder(), path);
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Returns a string to be executed, whenever the revit is started.
        /// If this is not specified as a path to an existing file in the XML file (under /CADPythonShell/StartupScript/@src),
        /// then null is returned.
        /// </summary>
        public static string GetStartupScript()
        {
            var path = GetStartupScriptPath();
            if (File.Exists(path))
            {
                using (var reader = File.OpenText(path))
                {
                    var source = reader.ReadToEnd();
                    return source;
                }
            }
            // no startup script found
            return null;
        }

        /// <summary>
        /// Writes settings to the settings file, replacing the old commands.
        /// </summary>
        public static void WriteSettings(
            IEnumerable<Command> commands,
            IEnumerable<string> searchPaths,
            IEnumerable<KeyValuePair<string, string>> variables,
            string initScript,
            string startupScript)
        {
            var doc = GetSettings();

            // clean out current stuff
            foreach (var xmlExistingCommands in (doc.Root.Descendants("Commands") ?? new List<XElement>()).ToList())
            {
                xmlExistingCommands.Remove();
            }
            foreach (var xmlExistingSearchPaths in doc.Root.Descendants("SearchPaths").ToList())
            {
                xmlExistingSearchPaths.Remove();
            }
            foreach (var xmlExistingVariables in doc.Root.Descendants("Variables").ToList())
            {
                xmlExistingVariables.Remove();
            }
            foreach (var xmlExistingInitScript in doc.Root.Descendants("InitScript").ToList())
            {
                xmlExistingInitScript.Remove();
            }
            foreach (var xmlExistingStartupScript in doc.Root.Descendants("StartupScript").ToList())
            {
                xmlExistingStartupScript.Remove();
            }

            // add commnads
            var xmlCommands = new XElement("Commands");
            foreach (var command in commands)
            {
                xmlCommands.Add(new XElement(
                    "Command",
                        new XAttribute("name", command.Name),
                        new XAttribute("src", command.Source),
                        new XAttribute("group", command.Group)));

            }
            doc.Root.Add(xmlCommands);

            // add search paths
            var xmlSearchPaths = new XElement("SearchPaths");
            foreach (var path in searchPaths)
            {
                xmlSearchPaths.Add(new XElement(
                    "SearchPath",
                        new XAttribute("name", path)));

            }
            doc.Root.Add(xmlSearchPaths);

            // add variables
            var xmlVariables = new XElement("Variables");
            foreach (var variable in variables)
            {
                xmlVariables.Add(new XElement(
                    "StringVariable",
                        new XAttribute("name", variable.Key),
                        new XAttribute("value", variable.Value)));

            }
            doc.Root.Add(xmlVariables);

            // add init script
            var xmlInitScript = new XElement("InitScript");
            xmlInitScript.Add(new XAttribute("src", initScript));
            doc.Root.Add(xmlInitScript);

            // add startup script
            var xmlStartupScript = new XElement("StartupScript");
            xmlStartupScript.Add(new XAttribute("src", startupScript));
            doc.Root.Add(xmlStartupScript);

            doc.Save(GetSettingsFile());
        }
    }

    /// <summary>
    /// A simple structure to hold information about canned commands.
    /// </summary>
    internal class Command
    {
        public string Name;
        public string Group;
        public string Source;
        public int Index;
        public ImageSource LargeImage;
        public ImageSource SmallImage;

        public override string ToString()
        {
            return Name;
        }
    }
}
