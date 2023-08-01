using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bas.Utility
{
	/// <summary>
	/// <para>Base class for states used in FiniteStateMachine Pattern.</para>
	/// </summary>
	public interface IFSMState
	{
		public virtual void Enter() { }
		public virtual void Update() { }
		public virtual void Exit() { }
		public virtual bool PopCondition => false;
		public abstract IFSMState GetParentState();
	}
	public abstract class FSMState:IFSMState
	{
		protected readonly IFSMState parentState;

		public FSMState(IFSMState parentState)
		{
			this.parentState = parentState;
		}
		public IFSMState GetParentState()
		{
			return parentState;
		}
		public virtual void Enter() { }
		public virtual void Update() { }
		public virtual void Exit() { }
		public virtual bool PopCondition => false;
	}

}