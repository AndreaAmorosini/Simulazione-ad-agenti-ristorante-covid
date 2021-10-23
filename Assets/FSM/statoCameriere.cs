using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using static IACameriereFSM;

#region states
public class statoCameriere : State
{

    protected IACameriereFSM iaCameriere;
    private CameriereStates cameriereState;

    public CameriereStates CameriereState { get { return cameriereState;  } }

    public statoCameriere(FSM fsm , CameriereStates type, IACameriereFSM ia) : base(fsm)
    {
        iaCameriere = ia;
        cameriereState = type;
    }

    public delegate void StateDelegate();

    public StateDelegate OnEnterDelegate { get; set; } = null;
    public StateDelegate OnExitDelegate { get; set; } = null;
    public StateDelegate OnUpdateDelegate { get; set; } = null;
    public StateDelegate OnFixedUpdateDelegate { get; set; } = null;

    public override void Enter()
    {
        OnEnterDelegate?.Invoke();
    }

    public override void Exit()
    {
        OnExitDelegate?.Invoke();
    }

    public override void Update()
    {
        OnUpdateDelegate?.Invoke();
    }

    public override void FixedUpdate()
    {
        OnFixedUpdateDelegate?.Invoke();
    }
}
#endregion