using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Bas.Utility
{
	/// <summary>
	/// <para>Base class for states used in StateMachine Pattern.</para>
	/// </summary>
	public abstract class BaseState<T>
	{
		// Public Properties
		#region Public Properties
		public StackBasedStateMachine<T> Machine { get; set; }
		public T Controller { get; set; }
		public Type DirectBase => GetType().BaseType;

		#endregion		
		// Public Methods
		#region Public Methods
		public virtual void OnEnter(/*T controller*/) { }
		public virtual void OnUpdate(/*T controller*/) { }
		public virtual void OnExit(/*T controller*/) { }
		#endregion
	}

}