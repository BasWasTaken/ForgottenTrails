using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;    

namespace Bas.Utility
{
    [Serializable]
    /// <summary>
    /// <para>State machine for a particular type of <see cref="FSMState"/>, and using stack-based logic such as pop and push.</para>
    /// A nested stack-based finite state machine. To be honest i strongly suspect this bastard hybrid form of a nested and a stack-based fsm has the copmlications of both and possibly the benefits of neither but it's what i threw together and build the current system on, and it suits my purposes for now.
    /// </summary>
    public class StackBasedStateMachine<T>
    {
        // Inspector Properties
        #region Inspector Properties
        [ReadOnly]
        public string StatePeeker = "Not Started";
        [field: SerializeField, ReadOnly]
        public Stack<BaseState<T>> StateStack { get; private set; } = new();

        public T Controller { get; private set; }

        #endregion
        // Public Properties
        #region Public Properties
        public BaseState<T> CurrentState => StateStack.TryPeek(out BaseState<T> result)?result:null;
        public Dictionary<Type, BaseState<T>> KnownStates { get; private set; } = new();

        #endregion
        // Private Properties
        #region Private Properties
        private BaseState<T> BaseState { get; set; }
        private BaseState<T> StartState { get; set; }

        #endregion
        // Constructor
        #region Constructor
        public StackBasedStateMachine(T controller, BaseState<T> dummyState, params BaseState<T>[] states)
        {
            StatePeeker = "Constructing";
            Controller = controller;
            BaseState = dummyState; // why do i have a dummy state? oh probably to avoid an error later in the first transition? that doesn't seem very clean...

            foreach (BaseState<T> state in states)
            {
                state.Controller = controller;
                state.Machine = this;
                KnownStates.Add(state.GetType(), state);
            }
            StartState = states[0];

            StatePeeker = "Constructed";

            TransitionToState(StartState);
        }

        #endregion
        // Public Methods
        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newState">The state to transition to</param>
        public void TransitionToState(BaseState<T> newState)
        {
            StatePeeker += string.Format(", transitioning to: {0}", newState.GetType().ToString());
            
            if (CurrentState != null)
            {
                int levelDifference = GetLevelDifference(CurrentState, newState);
                if (levelDifference > 0)
                {
                    // Higher to lower level transition
                    BaseState<T> intermediateState = CurrentState;
                    while (levelDifference > 0)
                    {
                        Debug.Log("Exiting " + intermediateState);
                        intermediateState.OnExit(); // perform all the required exit behaviour
                        intermediateState = GetParent(intermediateState);
                        levelDifference--;
                    }
                    StateStack.Push(newState); // Go to the new state
                    Debug.Log("Entering " + CurrentState);
                    CurrentState.OnEnter(); // Perform the enter behaviour
                }
                else if (levelDifference < 0)
                {
                    // Lower to higher level transition
                    Debug.Log("Exiting " + CurrentState);
                    CurrentState.OnExit();
                    StateStack.Push(newState);
                    BaseState<T> intermediateState = CurrentState;
                    while (levelDifference < 0)
                    {
                        intermediateState = GetParent(intermediateState);
                        Debug.Log("Entering " + intermediateState);
                        intermediateState.OnEnter(); // perform all the required enter behaviour
                        levelDifference++;
                    }
                }
                else 
                {
                    // Transition between states at the same level
                    Debug.Log("Exiting " + CurrentState);
                    CurrentState.OnExit();
                    StateStack.Push(newState);
                    Debug.Log("Entering " + CurrentState);
                    CurrentState.OnEnter();
                }
            }
            else
            {
                // Initial state transition
                StateStack.Push(newState);
                Debug.Log("Entering " + CurrentState);
                CurrentState.OnEnter();
            }
            StatePeeker = CurrentState.GetType().ToString();
        }

        public void Reset()
        {

            // should use this, but it's not working:
          StateStack.Clear();
            TransitionToState(StartState);

            // shoul i iterative exit out of all states??
            // isn't that just going to the super state?

        }

        /// <summary>
        /// Perform the <see cref="CurrentState"/>'s Update method"/>
        /// </summary>
        public void Update()
        {
           // StatePeeker = CurrentState.ToString();
            if (CurrentState.DropCondition)
            {
                DropState(CurrentState);
            }
            else
            {
                StatePeeker = CurrentState.GetType().ToString();
                CurrentState.OnUpdate();
            }
        }

        /// <summary>
        /// Pop current state and revert back to the previous. Explicitly, in order: 
        /// 1. Fire appropriate OnExit()s.
        /// 2. Pop the state from the stack.
        /// 3. If the stack is now empty, push the default one.
        /// 4. Fire appropriate OnEnter()s for the now topmost state.
        /// </summary>
        /// <param name="caller"> The expected state to pop from</param>
        private void DropState(BaseState<T> caller)
        {
            if (CurrentState == caller)
            {
                BaseState<T> newState;
                if (StateStack.Count >= 2)
                {
                    newState = StateStack.ToArray()[^2];
                }
                else
                {
                    newState = BaseState;
                }

                int levelDifference = GetLevelDifference(CurrentState, newState);
                if (levelDifference > 0)
                {
                    // Higher to lower level transition
                    BaseState<T> intermediateState = CurrentState;
                    while (levelDifference > 0)
                    {
                        Debug.Log("Exiting " + intermediateState);
                        intermediateState.OnExit(); // perform all the required exit behaviour
                        intermediateState = GetParent(intermediateState);
                        levelDifference--;
                    }

                    StateStack.Pop(); // remove current state from top
                    if (StateStack.Count == 0)
                    {
                        StateStack.Push(BaseState);
                    }
                    Debug.Log("Entering " + CurrentState);
                    CurrentState.OnEnter(); // Perform the enter behaviour
                }
                else if (levelDifference < 0)
                {
                    // Lower to higher level transition

                    Debug.Log("Exiting " + CurrentState);
                    CurrentState.OnExit();
                    StateStack.Pop(); // remove current state from the top
                    if (StateStack.Count == 0)
                    {
                        StateStack.Push(BaseState);
                    }
                    BaseState<T> intermediateState = CurrentState;
                    while (levelDifference < 0)
                    {
                        intermediateState = GetParent(intermediateState);
                        intermediateState.OnEnter(); // perform all the required enter behaviour
                        levelDifference++;
                    }
                }
                else
                {
                    // Transition between states at the same level

                    Debug.Log("Exiting " + CurrentState);
                    CurrentState.OnExit();
                    StateStack.Pop(); // remove current state from the top
                    if (StateStack.Count == 0)
                    {
                        StateStack.Push(BaseState);
                    }
                    Debug.Log("Entering " + CurrentState);
                    CurrentState.OnEnter();
                }
                caller.DropCondition = false;
            }
            else
            {
                Debug.LogError(string.Format("State mismatch: {0} vs {1}.", caller, CurrentState));
                // reset system.
                Reset();
            }
        }

        #endregion
        // Private Methods
        #region Private Methods
        private int GetLevelDifference(BaseState<T> fromState, BaseState<T> toState)
        {
            int levelDifference = 0;
            BaseState<T> current = fromState;

            // Traverse upward in the hierarchy until the common ancestor is found
            while (current != null && !IsAncestor(current, toState))
            {
                current = GetParent(current);
                levelDifference++;
            }

            // If 'current' is null, it means we reached the top of the hierarchy without finding 'toState'
            // This may happen if 'fromState' and 'toState' are not in the same hierarchy.
            // You can handle this scenario according to your requirements.

            return levelDifference;
        }

        private bool IsAncestor(BaseState<T> ancestor, BaseState<T> descendant)
        {
            Type ancestorType = ancestor.GetType();
            Type descendantType = descendant.GetType();

            // Check if the ancestor type is assignable from the descendant type,
            // which means 'ancestor' is an ancestor of 'descendant'.
            return ancestorType.IsAssignableFrom(descendantType);
        }
        private BaseState<T> GetParent(BaseState<T> child)
        {
            Type U = child.DirectBase;
            if (!KnownStates.TryGetValue(U, out BaseState<T> parent))
            {
                throw new KeyNotFoundException();
                /*
                parent = (BaseState<T>)Activator.CreateInstance(typeof(T), Controller);
                KnownStates.Add(U, parent);
                */
            }
            return parent;
        }
        #endregion
    }
}
