using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;    

namespace Bas.Utility
{
    /// <summary>
    /// <para>State machine for a particular type of <see cref="FSMState"/>, and using stack-based logic such as pop and push.</para>
    /// </summary>
    public class StackFiniteStateMachine<T> where T:FSMState
    {
        // Inspector Properties
        #region Inspector Properties
        [field:SerializeField, ReadOnly]
        public Stack<T> Stack { get; protected set; }

        #endregion
        // Public Properties
        #region Public Properties
        public T CurrentState => Stack.TryPeek(out T result)?result:null;

        #endregion
        // Private Properties
        #region Private Properties
        protected T DefaultState { get; private set; }
        #endregion
        // Constructor
        #region Constructor
        public StackFiniteStateMachine(T defaultState)
        {
            DefaultState = defaultState;
            Stack.Clear();
            Stack.Push(DefaultState);
            CurrentState.Enter();
        }
        #endregion
        // Public Methods
        #region Public Methods
        public void TransitionToState(T newState, bool retainCurrent=true)
        {
            if (CurrentState != null)
            {
                int levelDifference = GetLevelDifference(CurrentState, newState);
                if (levelDifference > 0)
                {
                    // Higher to lower level transition
                    IFSMState intermediateState = CurrentState;
                    while (levelDifference > 0)
                    {
                        intermediateState.Exit(); // perform all the required exit behaviour
                        intermediateState = intermediateState.GetParentState(); 
                        levelDifference--;
                    }

                    if (!retainCurrent) Stack.Pop();
                    Stack.Push(newState); // Go to the new state
                    CurrentState.Enter(); // Perform the enter behaviour
                }
                else if (levelDifference < 0)
                {
                    // Lower to higher level transition
                    CurrentState.Exit();
                    if (!retainCurrent) Stack.Pop();
                    Stack.Push(newState);
                    IFSMState intermediateState = CurrentState;
                    while (levelDifference < 0)
                    {
                        intermediateState = intermediateState.GetParentState(); 
                        intermediateState.Enter(); // perform all the required enter behaviour
                        levelDifference++;
                    }
                }
                else
                {
                    // Transition between states at the same level
                    CurrentState.Exit();
                    if (!retainCurrent) Stack.Pop();
                    Stack.Push(newState);
                    CurrentState.Enter();
                }
            }
            else
            {
                // Initial state transition
                Stack.Push(newState);
                CurrentState.Enter();
            }
        }
        public void FinishTask(T identifier)
        {
            if(CurrentState == identifier)
            {
                TransitionToState(Stack.ToArray()[^2], false);
            }
        }
        /// <summary>
        /// Perform the <see cref="CurrentState"/>'s Update method"/>
        /// </summary>
        public void Update()
        {
            CurrentState.Update();
        }
        /* Depricated
        /// <summary>
        /// Pop current state and revert to the previous. Explicitly, in order: 
        /// 1. Fire OnExit().
        /// 2. Pop the state from the stack.
        /// 3. If the stack is now empty, push the default one.
        /// 4. Fire OnEnter() for the now topmost state.
        /// </summary>
        /// <param name="caller"> The expected state to pop from</param>
        public void PopState(T caller)
        {
            if(CurrentState == caller)
            {
                CurrentState.OnExit();
                Stack.Pop();
                if (Stack.Count==0) // if empty, 
                {
                    Stack.Push(DefaultState); // place the default
                }
                CurrentState.OnEnter();
            }
            else
            {
                Debug.LogError(string.Format("State mismatch: {0} vs {1}.", caller, CurrentState));
            }
        }
        */

        #endregion
        // Private Methods
        #region Private Methods
        private int GetLevelDifference(T fromState, T toState)
        {
            int levelDifference = 0;
            IFSMState current = fromState;

            // Traverse upward in the hierarchy until the common ancestor is found
            while (current != null && !IsAncestor(current, toState))
            {
                current = current.GetParentState();
                levelDifference++;
            }

            // If 'current' is null, it means we reached the top of the hierarchy without finding 'toState'
            // This may happen if 'fromState' and 'toState' are not in the same hierarchy.
            // You can handle this scenario according to your requirements.

            return levelDifference;
        }

        private bool IsAncestor(IFSMState ancestor, IFSMState descendant)
        {
            Type ancestorType = ancestor.GetType();
            Type descendantType = descendant.GetType();

            // Check if the ancestor type is assignable from the descendant type,
            // which means 'ancestor' is an ancestor of 'descendant'.
            return ancestorType.IsAssignableFrom(descendantType);
        }
        #endregion
    }
}
