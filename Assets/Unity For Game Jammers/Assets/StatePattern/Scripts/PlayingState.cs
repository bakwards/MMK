using UnityEngine;
using System.Collections;

public class PlayingState : State
{
    public override string StateName
    {
        get
        {
            return this.GetType().ToString();
        }
    }

    public override void EnterState()
    {
        Debug.Log("Enter PlayingState");
    }

    public override void StateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioController.Instance.PlaySound();
        }
    }
    public override void ExitState()
    {

    }
}
