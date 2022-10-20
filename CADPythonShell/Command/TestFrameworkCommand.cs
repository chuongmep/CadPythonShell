namespace CADPythonShell
{
    public class TestFrameworkCommand : ICadCommand
    {
        public override void Execute()
        {
            string fullCmdLine = $"_{nameof(MgdDbgAction.SnoopTests)}\n";
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(fullCmdLine, false, false, true);
        }
    }
}

