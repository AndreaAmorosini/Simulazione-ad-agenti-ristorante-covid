using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Patterns
{
    public class State
    {
        protected FSM m_fsm;

        public State(FSM fsm)
        {
            m_fsm = fsm;
        }

        //metodo per quando si entra nello stato
        public virtual void Enter() { }
        //metodo per quando si esce dallo stato
        public virtual void Exit() { }
        //metodo che verrà chiamato in ogni update di unity
        public virtual void Update() { }
        //metodo che verrà chiamato in ogni fixedUpdate in unity
        public virtual void FixedUpdate() { }
    }


    public class FSM
    {
        protected Dictionary<int, State> m_states = new Dictionary<int, State>();
        protected State m_currentState;
        public FSM()
        {

        }

        public void Add(int key, State state)
        {
            m_states.Add(key, state);
        }

        public State GetState(int key)
        {
            return m_states[key];
        }

        public void SetCurrentState(State state)
        {
            if(m_currentState != null)
            {
                m_currentState.Exit();
            }

            m_currentState = state;

            if(m_currentState != null)
            {
                m_currentState.Enter();
            }
        }

        public void Update()
        {
            if(m_currentState != null)
            {
                m_currentState.Update();
            }
        }

        public void FixedUpdate()
        {
            if(m_currentState != null)
            {
                m_currentState.FixedUpdate();
            }
        }
    }
}