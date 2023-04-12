namespace CADPythonShell.Command;

public abstract class ICadCommand
{
    /// <summary>
    /// Execute something
    /// </summary>
    public abstract void Execute();

    public enum MgdDbgAction
    {
        Snoop,
        SnoopDB,
        SnoopEnts,
        SnoopNEnts,
        SnoopByHandle,
        SnoopEd,
        SnoopEvents,
        SnoopTests,
    }
}