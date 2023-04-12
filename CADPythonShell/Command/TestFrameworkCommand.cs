using MgdDbg.Test;

namespace CADPythonShell;

public class TestFrameworkCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.TestDb();
    }
}