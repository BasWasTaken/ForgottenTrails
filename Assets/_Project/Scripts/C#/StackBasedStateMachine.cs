using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NaughtyAttributes;
using ForgottenTrails.InkFacilitation;

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
        [SerializeField, TextArea(1, 20)]
        private string _StatePeeker  = "Not Started";
        public string StatePeeker
        {
            get { return _StatePeeker; }
            private set
            {
                _StatePeeker = value;
                foreach (var state in StateStack)
                {
                    _StatePeeker += "\n" + state.GetType().Name;
                }
            }
        }

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
        public StackBasedStateMachine(T controller, params BaseState<T>[] states)
        {
            StatePeeker = "Constructing";
            Controller = controller;

            foreach (BaseState<T> state in states)
            {
                state.Controller = controller;
                state.Machine = this;
                KnownStates.Add(state.GetType(), state);
            }
            BaseState = states[0];
            StartState = states[1];
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
            StatePeeker = string.Format(", transitioning to: {0}", goalState.GetType().Name);

            if (CurrentState != null)
            {
                PerformTransitionMethods(goalState);
                StateStack.Push(goalState); // Set the new state
            }
            else
            {
                Debug.LogException(new NullReferenceException());
            }
            StatePeeker = "transitioned";
        }
        /// <summary>
        /// Pop current state and revert back to the previous. Explicitly, in order: 
        /// 1. Fire appropriate OnExit()s.
        /// 2. Pop the state from the stack.
        /// 3. If the stack is now empty, push the default one.
        /// 4. Fire appropriate OnEnter()s for the now topmost state.
        /// </summary>
        public void DropState()
        {
            DropState(CurrentState);
        }

        //TODO: MOVE THIS
        public bool TryGetChildStateFromStack(BaseState<T> parent, out BaseState<T> firstChild)
        {
            foreach (var state in StateStack)
            {
                if (DoesXDescentFromY(state, parent))
                {
                    firstChild = state;
                    return true;
                }
            }
            firstChild = null;
            return false;
        }

        // TODO move this later
        private void PerformDrop()
        {
            if (StateStack.Count < 2)
            {
                throw new Exception("Can't drop from the basestate with nothing to drop to.");
            }
            i++;
            BaseState<T> toState = StateStack.ToArray()[^2];



            PerformTransitionMethods(toState);
            var dropped = StateStack.Pop(); // remove current state from top

            dropped.DropCondition = false;
            CurrentState.OnEnter(); // Perform the enter behaviour
        }

        int i = 0;
        /// <summary>
        /// Drop until given state is dropped.
        /// 1. Check if state is in history.
        /// 2. If it's the current: just drop.
        /// 3. If it's later, or an assignable: drop until the first time you drop the given state.
        /// </summary>
        /// <param name="caller"> The expected state to pop from</param>
        public void DropState(BaseState<T> dropFrom)
        {
            if (CurrentState == dropFrom) // if this is the state we're in
            {
                PerformDrop(); // simply drop it
                DropState(dropFrom); // check if it needs to be done again in case of double states?
            }
            else if (StateStack.Contains(dropFrom)) // else, if we do have that state somwhere
            {
                PerformDrop();  // perform a drop
                DropState(dropFrom); // and keep going deeper until we have done this drop
            }
            else
            {
                if (DoesXDescentFromY(CurrentState, dropFrom)) // if the current state is a descendant of the dropstate
                {
                    PerformDrop();                    // drop it 
                    DropState(dropFrom); // and check again
                }
                else if (TryGetChildStateFromStack(dropFrom, out BaseState<T> child))                // else if we do have a descendant in stack
                {
                    do
                    {
                        PerformDrop(); // drop one state
                    } while (StateStack.Contains(child)); // until you reach past the child
                    DropState(dropFrom); // and keep going deeper to check for next
                }
                else                // else: 
                {
                    //nothing left to do !check how many runthoughs this took.
                    //Debug.Log(i);
                    i = 0;
                }

            }

        }
        private void PerformTransitionMethods(BaseState<T> goalState)
        {

            BaseState<T> intermediateState = CurrentState;

            BaseState<T> commonState = GetCommonAncestorIncluding(intermediateState, goalState);
            // transitions upwards:
            while (intermediateState != commonState & intermediateState!=BaseState) // loops. always fires first onexit, except in cases where the to state is a decendent from the currentstate. (because the current is also the common ancestor)
            {
                intermediateState.OnExit();
                intermediateState = GetParent(intermediateState);
            }
            // transition downwards:
            while (intermediateState != goalState &goalState != BaseState)
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
                DropState();
            }
            else
            {
                StatePeeker = "update";
                CurrentState.OnUpdate();
            }
        }

        #endregion
        // Private Methods
        #region Private Methods 
        public static bool IsXAncestorToY(BaseState<T> X, BaseState<T> Y)
        {
            // simply get the doesdescent function but flipping x and y.
            return DoesXDescentFromY(Y, X);
        }
        public static bool DoesXDescentFromY(BaseState<T> X, BaseState<T> Y)
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
            /*if(child == BaseState)
            {
                return (BaseState<T>)Activator.CreateInstance(typeof(T), Controller);
            }*/
            if (!KnownStates.TryGetValue(U, out BaseState<T> parent))
            {
                throw new KeyNotFoundException(string.Format("Instance for type {0}, (parent of {1}), not found", U, child));
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
            BaseState<T> intermediate = toState; 
            while (fromState != GetParent(intermediate))
            {
                intermediate = GetParent(intermediate);
            }
            return intermediate;
        }
        #endregion
    }
}
