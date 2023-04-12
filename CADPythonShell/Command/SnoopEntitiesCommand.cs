using MgdDbg.App;

namespace CADPythonShell.Command;

public class SnoopEntitiesCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopEntity();
    }
}