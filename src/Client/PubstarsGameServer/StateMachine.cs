﻿using PubstarsGameServer.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer
{
    public class StateMachine
    {
        private Queue<IState> m_States = new Queue<IState>();
        private List<IState> m_GlobalStates = new List<IState>();
        private IState m_CurrentState;

        public void AddState(IState state)
        {
            m_States.Enqueue(state);
        }        

        public void AddGlobalState(IState state)
        {
            m_GlobalStates.Add(state);
        }

        public Task Init(IState state)
        {            
            m_States = new Queue<IState>();
            AddState(state);          
            m_GlobalStates.ForEach(x => x.OnEnter());
            return Task.FromResult<object>(null);
        }

        public async Task<bool> Update()
        {
            m_GlobalStates.ForEach(x => x.Execute());
            if(m_CurrentState == null || await Execute())
            {
                return await AdvanceState();
            }
            return false;
        }

        private async Task<bool> AdvanceState()
        {
            if (m_CurrentState != null) await m_CurrentState.OnExit();

            if (m_States.Count > 0)
            {
                m_CurrentState = m_States.Dequeue();
                await m_CurrentState.OnEnter();
                return false;
            }
            else
            {                
                Console.WriteLine("StateMachine: Done at " + m_CurrentState);
                m_CurrentState = null;
                return true;
            }                           
        }

        private async Task<bool> Execute()
        {
            return await m_CurrentState.Execute();
        }
    }
}
