using MgdDbg.App;

namespace CADPythonShell.Command;

public class TestFrameworkCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.TestDb();
    }
}