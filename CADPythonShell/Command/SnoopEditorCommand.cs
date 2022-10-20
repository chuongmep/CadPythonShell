namespace CADPythonShell
{
    public class SnoopEditorCommand : ICadCommand
    {
        public override void Execute()
        {
            string fullCmdLine = $"_{nameof(MgdDbgAction.SnoopEd)}\n";
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute(fullCmdLine, false, false, true);
        }
    }
}

