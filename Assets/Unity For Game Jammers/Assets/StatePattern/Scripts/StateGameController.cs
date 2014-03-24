using UnityEngine;
using System.Collections;

public class StateGameController : MonoBehaviour {

    void Start()
    {
        StateManager.Instance.EnterTheFirstState(new PlayingState());
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            if (StateManager.Instance.ActiveState.StateName.Equals(new PlayingState().StateName))
            {
                StateManager.Instance.SwitchState(new LossingState());
            }
            else
            {
                StateManager.Instance.SwitchState(new PlayingState());
            }
        }
    }
}
