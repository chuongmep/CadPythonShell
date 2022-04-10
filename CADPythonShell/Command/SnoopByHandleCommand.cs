using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace CADPythonShell;

public class SnoopByHanderCommand : ICadCommand
{
    public override void Execute()
    {
        string fullCmdLine = $"_{nameof(MgdDbgAction.SnoopByHandle)}\n";
        Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(fullCmdLine, false, false, true);
    }
}