using UnityEngine;

public class StateManager : MonoBehaviour
{
    #region Singleton Pattern
    private static StateManager _instance = null;
    public static StateManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Instantiate(Resources.Load("StatePattern/StateManager", typeof(StateManager))) as StateManager;
                _instance.Init();
                return _instance;
            }
            else if (_instance != null)
            {
                return _instance;
            }
            return null;
        }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    #endregion
    
    private State _activeState;
    public State ActiveState
    {
        get
        {
            return _activeState;
        }
    }

    void Update()
    {
        if (_activeState != null)
        {
            _activeState.StateUpdate();
        }
    }

    public void SwitchState(State newState)
    {
        _activeState.ExitState();
        _activeState = newState;
        _activeState.EnterState();
    }

    public void EnterTheFirstState(State firstState)
    {
        _activeState = firstState;
        _activeState.EnterState();
    }

    void Init()
    {

    }
}