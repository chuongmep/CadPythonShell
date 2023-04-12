using MgdDbg.Test;

namespace CADPythonShell;

public class SnoopEditorCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopEd();
    }
}