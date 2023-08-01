using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;
using Ink.Runtime;

namespace ForgottenTrails.InkFacilitation
{
    public class SCEntryState : BaseState<StoryController>
    {
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        #region Events
        public static event Action<Story> OnCreateStory;
         
        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {
            PrepStory();
            StartStory();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
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
            Controller.InputBroker.RemoveOptions();
            Controller.Story = new Story(Controller.InkStoryAsset.text);
            Controller.Story.state.variablesState["Name"] = DataManager.Instance.MetaData.playerName; // get name from metadata
            Controller.BindAndObserve(Controller.Story);
            OnCreateStory?.Invoke(Controller.Story); 
        }
        /// <summary>
        /// Creates a new Story object with the compiled story which we can then play!
        /// </summary>
        private void StartStory()
        {
            if (DataManager.Instance.DataAvailable(InkDataAsset.Key))
            {
                Debug.Log("found data! trying to load...");
                // enter loading state
                TryLoadData();
                // then exit again
            }
            else
            {
                InkDataAsset = CreateBlankData(); // NOTE: you're making data a second time, there's already blank data made. why am i doing this again?
            }

            if (InkDataAsset.StoryStateJson != "")
            {
                Debug.Log("continueing from savepoint!");
                Story.state.LoadJson(InkDataAsset.StoryStateJson);
                StartCoroutine(TextProducer.FeedText(Story.currentText));
            }
            else
            {
                Debug.Log("no save point detected, starting from start");
                Story.state.GoToStart();
            }
            AdvanceStory(); // show the first bit of story
        }
        #endregion
    }
    public class SCGameState : BaseState<StoryController>
    {        
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            StoryController.Instance.TimeSinceAdvance += Time.unscaledDeltaTime;
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCLoadingState : SCGameState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }

    public class SCProductionState : SCGameState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (StoryController.Instance.TimeSinceAdvance > InterfaceBroker.Instance.SkipDelay)
                {
                    TextProducer.Instance.FastForward();
                }
            }
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion        
    }
    public class SCWritingState : SCProductionState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCPeekingState : SCWritingState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCFastForwardState : SCWritingState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCFunctionState : SCProductionState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCWaitingForInputState : SCGameState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {
            // Debug.Log("Turning input on.");
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {
            // Debug.Log("Turning input off.");
        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion        
    }
    public class SCWaitingForChoiceState : SCWaitingForInputState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {
            Controller.InputBroker.PresentButtons(); // create new choices
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {
            Controller.InputBroker.RemoveOptions(); // Destroy old choices
        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCWaitingForContinueState : SCWaitingForInputState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {
            Controller.InputBroker.FloatingMarker.gameObject.SetActive(true); // else set bouncing triangle at most recent line
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Controller.Story.canContinue)
                {
                    Controller.AdvanceStory(); // TODO: move this function to the private methods region here
                }
            }
        }
        public override void OnExit()
        {
            Controller.InputBroker.FloatingMarker.gameObject.SetActive(false); // remove marker 
        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }

    public class SCGameMenuState : SCGameState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCInventoryState : SCGameMenuState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }

    public class SCSettingsState : SCGameMenuState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCSavingState : SCGameState
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {
            DataManager.Instance.WriteStashedDataToDisk();
            DataManager.Instance.OnDataSaved += () =>
            {
                Machine.DropState(this);
            };
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
    public class SCExitState : BaseState<StoryController>
    {
        // Inspector Properties
        #region Inspector Properties

        #endregion
        // Public Properties
        #region Public Properties

        #endregion
        // Private Properties
        #region Private Properties

        #endregion
        // Public Methods
        #region Public Methods
        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }
        public override void OnExit()
        {

        }
        #endregion
        // Private Methods
        #region Private Methods

        #endregion
    }
}
