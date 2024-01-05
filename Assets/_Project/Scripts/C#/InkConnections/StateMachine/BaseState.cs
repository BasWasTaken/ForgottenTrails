using System;

namespace Bas.ForgottenTrails.InkConnections
{
    /// <summary>
    /// <para>Base class for states used in StateMachine Pattern.</para>
    /// </summary>
    public abstract class BaseState<T>
    {
        // Public Properties

        #region Fields

        public bool DropCondition = false;

        #endregion Fields

        #region Properties

        public StackBasedStateMachine<T> Machine { get; set; }
        public T Controller { get; set; }
        public Type DirectBase => GetType().BaseType;

        #endregion Properties

        // Public Methods

        #region Public Methods

        public virtual void OnEnter(/*T controller*/)
        { }

        public virtual void OnUpdate(/*T controller*/)
        { }

        public virtual void OnExit(/*T controller*/)
        { }

        #endregion Public Methods
    }
}