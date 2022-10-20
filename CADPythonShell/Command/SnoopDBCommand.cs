using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CADPythonShell
{
    public class SnoopDBCommand : ICadCommand
    {
        public override void Execute()
        {
            string fullCmdLine = $"_{nameof(MgdDbgAction.SnoopDB)}\n";
            Application.DocumentManager.MdiActiveDocument.SendStringToExecute(fullCmdLine, false, false, true);
        }
    }
}

