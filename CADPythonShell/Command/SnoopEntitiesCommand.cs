using MgdDbg.Test;

namespace CADPythonShell;

public class SnoopEntitiesCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopEntity();
    }
}