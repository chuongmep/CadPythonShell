using MgdDbg.Test;

namespace CADPythonShell;

public class SnoopEntitiesNestedCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds cmd = new TestCmds();
        cmd.SnoopNestedEntity();
    }
}