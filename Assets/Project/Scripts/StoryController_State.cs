using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;

namespace ForgottenTrails.InkFacilitation
{
    /// <summary>
    /// <para>Base class for all machinestates for <see cref="StoryController"/> object.</para>
    /// </summary>
    public abstract class StoryController_State : FSMState
    {
        public StoryController_State(IFSMState parentState):base(parentState)
        {
        }
        public class Entry : StoryController_State
        {
            public Entry(IFSMState parentState) : base(parentState)
            {
            }
        }
        public abstract class Active : StoryController_State
        {
            public Active(IFSMState parentState) : base(parentState)
            {
            }
            public override void Update()
            {
                base.Update();
                StoryController.Instance.TimeSinceAdvance += Time.unscaledDeltaTime;
            }
            public class Loading : Active
            {
                public Loading(IFSMState parentState) : base(parentState)
                {
                }
            }

            public class Production : Active
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
            public class AwaitingInput : Active
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

            public class Menu : Active
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
            public class Saving : Active
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
                        StoryController.Instance.StateMachine.FinishTask(this);
                    };
                }
                public override void Exit()
                {
                    base.Exit();
                }
            }
        }
        public class Exiting : StoryController_State
        {
            public Exiting(IFSMState parentState) : base(parentState)
            {

            }
        }

        public static Entry entryState;
        public static Active.Loading loadingState;
        public static Active.Production productionState;
        public static Active.Production.Writing writingState;
        public static Active.Production.Writing.Peeking peekingState;
        public static Active.Production.Writing.FastForwarding fastForwardState;
        public static Active.Production.Functions functionState;
        public static Active.AwaitingInput awaitingInputState;
        public static Active.AwaitingInput.AwaitingChoice awaitingChoiceState;
        public static Active.AwaitingInput.AwaitingContinue awaitingContinueState;
        public static Active.Menu menuState;
        public static Active.Menu.Inventory inventoryState;
        public static Active.Menu.Settings settingsState;
        public static Active.Saving savingState;
        public static Exiting exitingState;
    }
}
