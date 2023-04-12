using MgdDbg.App;

namespace CADPythonShell.Command;

public class SnoopEditorCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopEd();
    }
}