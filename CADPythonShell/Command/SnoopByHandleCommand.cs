using MgdDbg.App;

namespace CADPythonShell.Command;

public class SnoopByHandleCommand : ICadCommand
{
    public override void Execute()
    {
        TestCmds testCmds = new TestCmds();
        testCmds.SnoopEntityByHandle();
    }
}