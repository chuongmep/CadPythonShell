using MgdDbg.Test;

namespace CADPythonShell;

public class SnoopDBCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopDatabase();
    }
}