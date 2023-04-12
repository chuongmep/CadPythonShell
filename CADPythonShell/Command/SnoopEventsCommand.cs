using MgdDbg.Test;

namespace CADPythonShell;

public class SnoopEventsCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.Events();
    }
}