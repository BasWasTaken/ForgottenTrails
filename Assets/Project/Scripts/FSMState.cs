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
		public virtual void Enter() { Debug.Log("If you see this it means interface methods also get called."); }
		public virtual void Update() { }
		public virtual void Exit() { RequestPop = false; }
		public virtual bool RequestPop
		{
			get
			{
				throw new NotSupportedException();
			}
			protected set
			{
				throw new NotSupportedException();
			}
		}
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
		public virtual void Exit() { RequestPop = false; }
		public virtual bool RequestPop
		{
			get
			{
				throw new NotSupportedException();
			}
			protected set
			{
				throw new NotSupportedException();
			}
		}
	}

}