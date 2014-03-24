public abstract class State
{
    private string stateName;
    public abstract string StateName
    {
        get;
    }
    public abstract void EnterState();
    public abstract void StateUpdate();
    public abstract void ExitState();
}