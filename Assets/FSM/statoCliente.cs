using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns;
using static IAClienteFSM;

#region states
public class statoCliente : State
{
    protected IAClienteFSM iaCliente;
    private ClienteStates clienteState;

    public ClienteStates ClienteState { get { return clienteState; } }
    public statoCliente(FSM fsm, ClienteStates type, IAClienteFSM ia) : base(fsm)
    {
        iaCliente = ia;
        clienteState = type;
    }


    //il delegato
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