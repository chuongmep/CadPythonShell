using MgdDbg.App;

namespace CADPythonShell.Command;

public class SnoopEventsCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.Events();
    }
}