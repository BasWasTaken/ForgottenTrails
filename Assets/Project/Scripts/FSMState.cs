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
		// Public Properties
		#region Public Properties
		public abstract Type GetParentState();
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
		#endregion
		// Public Methods
		#region Public Methods
		public virtual void Enter() { Debug.Log("If you see this it means interface methods also get called."); }
		public virtual void Update() { }
		public virtual void Exit() { RequestPop = false; }
		#endregion


	}
	public abstract class FSMState:IFSMState
	{
		// Public Properties
		#region Public Properties
		public Type GetParentState() => GetType().BaseType; // NOTE als dit nit werkt gewoon lekker basetyp laten calle nvanaf statemachine
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
		#endregion
		// Private Properties
		#region Private Properties
		protected MonoBehaviour owner;
		#endregion
		// Constructor
		#region Constructor
		public FSMState(MonoBehaviour owner)
		{
			this.owner = owner;
		}
		#endregion
		// Public Methods
		#region Public Methods
		public void Enter() { }
		public virtual void Update() { }
		public void Exit() { RequestPop = false; }
		#endregion
		// Private Methods
	}

}