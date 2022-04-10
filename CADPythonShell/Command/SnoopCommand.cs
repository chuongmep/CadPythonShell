using Autodesk.AutoCAD.Runtime;

namespace CADPythonShell
{
    public class SnoopCommand : CadCommand
    {
        [CommandMethod("Snoop")]
        public void Execute()
        {
           new CADSnoop.SnoopCommand().Snoop();
        }
    }
}
