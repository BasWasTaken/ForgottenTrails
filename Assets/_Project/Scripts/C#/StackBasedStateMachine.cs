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
            StateStack.Push(BaseState);
            StatePeeker = "Constructed";

            TransitionToState(StartState);
        }

        #endregion
        // Public Methods
        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="goalState"></param>
        public void TransitionToState(BaseState<T> goalState)
        {
            StatePeeker += string.Format(", transitioning to: {0}", goalState.GetType().ToString());

            if (CurrentState != null)
            {
                PerformTransitionMethods(goalState);
                StateStack.Push(goalState); // Set the new state
            }
            else
            {
                Debug.LogException(new NullReferenceException());
            }
            StatePeeker = CurrentState.GetType().ToString();
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
            if (CurrentState != caller)
            {
                Debug.LogError(string.Format("State mismatch: {0} vs {1}.", caller, CurrentState));
                // reset system.
                Reset();
                return;
            }
            if(StateStack.Count < 2)
            {
                Debug.LogWarning("Can't drop from the basestate with nothing to drop to.");
                return;
            }

            BaseState<T> goalState = StateStack.ToArray()[^2];
            
            PerformTransitionMethods(goalState);
            StateStack.Pop(); // remove current state from top
            CurrentState.OnEnter(); // Perform the enter behaviour

            caller.DropCondition = false;

            /* this should not be necessary
            if (StateStack.Count == 0)
            {
                //StateStack.Push(BaseState);
                TransitionToState(BaseState);
            }
            */
        }
        private void PerformTransitionMethods(BaseState<T> goalState)
        {

            BaseState<T> intermediateState = CurrentState;

            BaseState<T> commonState = GetCommonAncestorIncluding(intermediateState, goalState);
            // transitions upwards:
            while (intermediateState != commonState) // loops. always fires first onexit, except in cases where the to state is a decendent from the currentstate. (because the current is also the common ancestor)
            {
                intermediateState.OnExit();
                intermediateState = GetParent(intermediateState);
            }
            // transition downwards:
            while (intermediateState != goalState)
            {
                intermediateState = GetChildTowards(intermediateState, goalState);
                intermediateState.OnEnter();
            }
        }
        private BaseState<T> GetCommonAncestorIncluding(BaseState<T> fromState, BaseState<T> toState)
        {
            int levelDifference = 0;
            BaseState<T> current = fromState;
            
            while (current != null && !IsXAncestorToY(current, toState)) //if this is not an ancestor
            {
                // go up a level
                current = GetParent(current);
                levelDifference++;
            }
            return current;
            // de fromstate kan hier uitkomen, als het goed is. ik denk de tostate ook, maar daar ben ik minder zeker van. check dit morgen met frisse ogen nog even.
        }
        public void Reset(bool hard = false)
        {
            // should use this, but it's not working:
            if (hard)
            {
                StateStack.Clear();
                StateStack.Push(BaseState);
            }
            TransitionToState(StartState);
            StateStack.Clear();
            StateStack.Push(BaseState);
            StateStack.Push(StartState);
        }

        /// <summary>
        /// Perform the <see cref="CurrentState"/>'s Update method"/>
        /// </summary>
        public void Update()
        {
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

        #endregion
        // Private Methods
        #region Private Methods 
        private bool IsXAncestorToY(BaseState<T> X, BaseState<T> Y)
        {
            // simply get the doesdescent function but flipping x and y.
            return DoesXDescentFromY(Y, X);
        }
        private bool DoesXDescentFromY(BaseState<T> X, BaseState<T> Y)
        {
            Type descendantType = X.GetType();
            Type ancestorType = Y.GetType();

            // Check if the X type is assignable from the Y type,
            // which means 'Y' is an ancestor of 'X'.
            // which means 'X' descents from 'Y'
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
        private BaseState<T> GetChildTowards(BaseState<T> fromState, BaseState<T> toState)
        {
            if(!IsXAncestorToY(fromState, toState))
            {
                Debug.LogError(string.Format("{0} does not descent from {1}.", toState, fromState));
                return null;
            }

            // go up the ancestry tree until we get the direct descendant of the fromstate:
            BaseState<T> intermediate = GetParent(toState); 
            while (fromState != GetParent(intermediate))
            {
                intermediate = GetParent(intermediate);
            }
            return intermediate;
        }
        #endregion
    }
}
