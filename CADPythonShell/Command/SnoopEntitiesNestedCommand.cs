using MgdDbg.App;

namespace CADPythonShell.Command;

public class SnoopEntitiesNestedCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopNestedEntity();
    }
}