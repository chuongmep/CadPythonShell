namespace CADPythonShell
{
    public class SnoopEventsCommand : ICadCommand
    {
        public override void Execute()
        {
            string fullCmdLine = $"_{nameof(MgdDbgAction.SnoopEvents)}\n";
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(fullCmdLine, false, false, true);
        }
    }
}

