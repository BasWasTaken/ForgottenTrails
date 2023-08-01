using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;
using Ink.Runtime;

namespace ForgottenTrails.InkFacilitation.ControllerState
{
    /// <summary>
    /// <para>Base class for all machinestates for <see cref="StoryController"/> object.</para>
    /// </summary>
    public abstract class StoryControllerBaseState : FSMState
    {
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties
        protected StoryController Owner => (StoryController)owner;
        #endregion
        // Constructor
        #region Constructor
        public StoryControllerBaseState(MonoBehaviour owner) : base(owner)
        {

        }

        #endregion
        // Public Methods
        #region Public Methods

        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }

    public class Entry : StoryControllerBaseState
    {
        // Public Properties
        #region Public Properties
        
        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Constructor
        #region Constructor
        public Entry(MonoBehaviour owner) : base(owner)
        {

        }

        #endregion        
        #region Events
        public static event Action<Story> OnCreateStory; // TODO: Check if this is used anywhere.

        #endregion
        // Public Methods
        #region Public Methods
        public new void Enter()
        {
            PrepStory();
            Owner.StartStory();
        }
        public override void Update()
        {
            base.Update();
        }
        public new void Exit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods
        /// <summary>
        /// Preps story for play. Should be called after <see cref="InkData"/> object has been initialised or loaded.
        /// </summary>
        private void PrepStory()
        {
            Owner.InputBroker.RemoveOptions();
            Owner.Story = new Story(Owner.InkStoryAsset.text);
            OnCreateStory?.Invoke(Owner.Story); // NOTE: Is it okay that I moved this to be before instead of after the next few lines?
            Owner.Story.state.variablesState["Name"] = DataManager.Instance.MetaData.playerName; // get name from metadata
            Owner.BindAndObserve(Owner.Story);
        }
        #endregion
    }
    public abstract class ActiveState : StoryControllerBaseState
    {
        public ActiveState(IFSMState parentState, MonoBehaviour owner) : base(parentState, owner)
        {
        }
        public override void Update()
        {
            base.Update();
            StoryController.Instance.TimeSinceAdvance += Time.unscaledDeltaTime;
        }
        public class Loading : ActiveState
        {
            public Loading(IFSMState parentState) : base(parentState)
            {
            }
        }

        public class Production : ActiveState
        {
            public Production(IFSMState parentState) : base(parentState)
            {
            }
            public override void Update()
            {
                base.Update();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (StoryController.Instance.TimeSinceAdvance > InterfaceBroker.Instance.SkipDelay)
                    {
                        TextProducer.Instance.FastForward();
                    }
                }
            }
            public class Writing : Production
            {
                public Writing(IFSMState parentState) : base(parentState)
                {
                }
                public class Peeking : Writing
                {
                    public Peeking(IFSMState parentState) : base(parentState)
                    {
                    }

                }
                public class FastForwarding : Writing
                {
                    public FastForwarding(IFSMState parentState) : base(parentState)
                    {
                    }
                }

            }
            public class Functions : Production
            {
                public Functions(IFSMState parentState) : base(parentState)
                {
                }
            }
        }
        public class AwaitingInput : ActiveState
        {
            public AwaitingInput(IFSMState parentState) : base(parentState)
            {
            }
            public override void Enter()
            {
                // Debug.Log("Turning input on.");
                if (InterfaceBroker.Instance.CanPresentChoices()) // try to make buttons if any
                {
                    StoryController.Instance.StateMachine.TransitionToState(awaitingChoiceState);
                }
                else
                {
                    StoryController.Instance.StateMachine.TransitionToState(awaitingContinueState);
                }
                RequestPop = true; // duty fulfilled
            }
            public override void Exit()
            {
                // Debug.Log("Turning input off.");
                //NOTE: Could also just call generic "remove options and marker" function here...
            }
            public class AwaitingChoice : AwaitingInput
            {
                public AwaitingChoice(IFSMState parentState) : base(parentState)
                {
                }
                public override void Enter()
                {
                    InterfaceBroker.Instance.PresentButtons();
                }
                public override void Exit()
                {
                    InterfaceBroker.Instance.RemoveOptions(); // Destroy old choices
                }

            }
            public class AwaitingContinue : AwaitingInput
            {
                public AwaitingContinue(IFSMState parentState) : base(parentState)
                {

                }
                public override void Enter()
                {
                    InterfaceBroker.Instance.FloatingMarker.gameObject.SetActive(true); // else set bouncing triangle at most recent line
                }
                public override void Update()
                {
                    base.Update();
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (StoryController.Instance.Story.canContinue)
                        {
                            // TODO: Insert go to next state, ending on the code below
                            StoryController.Instance.AdvanceStory();
                        }
                    }
                }
                public override void Exit()
                {
                    InterfaceBroker.Instance.FloatingMarker.gameObject.SetActive(false); // remove marker 
                }
            }
        }

        public class Menu : ActiveState
        {
            public Menu(IFSMState parentState) : base(parentState)
            {
            }
            public class Inventory : Menu
            {
                public Inventory(IFSMState parentState) : base(parentState)
                {
                }
            }

            public class Settings : Menu
            {
                public Settings(IFSMState parentState) : base(parentState)
                {
                }

            }

        }
        public class Saving : ActiveState
        {
            public Saving(IFSMState parentState) : base(parentState)
            {

            }
            public override void Enter()
            {
                base.Enter();
                DataManager.Instance.WriteStashedDataToDisk();
                DataManager.Instance.OnDataSaved += () =>
                {
                    RequestPop = true;
                };
            }
            public override void Exit()
            {

            }
        }
    }
    public class Exiting : StoryControllerBaseState
    {
        public Exiting(IFSMState parentState) : base(parentState)
        {

        }
    }
}
