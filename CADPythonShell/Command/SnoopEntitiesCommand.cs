namespace CADPythonShell;

public class SnoopEntitiesCommand : ICadCommand
{
    public override void Execute()
    {
        string fullCmdLine = $"_{nameof(MgdDbgAction.SnoopEnts)}\n";
        Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(fullCmdLine, false, false, true);
    }
}