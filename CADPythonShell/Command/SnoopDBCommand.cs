using MgdDbg.App;

namespace CADPythonShell.Command;

public class SnoopDBCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopDatabase();
    }
}