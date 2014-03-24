using UnityEngine;
using System.Collections;

public class LossingState : State {
    
    public override string StateName
    {
        get 
        {
            return this.GetType().ToString();
        }
    }
    
    public override void EnterState()
    {
        Debug.Log("Enter LossingState");
    }

    public override void StateUpdate()
    {

    }
    public override void ExitState()
    {

    }
}
