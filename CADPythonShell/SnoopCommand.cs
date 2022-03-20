using Autodesk.AutoCAD.Runtime;

namespace CADPythonShell
{
    public class SnoopCommand
    {
        [CommandMethod("Snoop")]
        public void Execute()
        {
           new CADSnoop.SnoopCommand().Execute();
        }
    }
}
