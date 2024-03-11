using System;
using UnityEngine;
using UnityEngine.UI;
using VVGames.ForgottenTrails.InkConnections;

namespace VVGames.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class InGameMenuSwapper : MonoBehaviour
    {
        #region Properties

        public GUISlideIn Slide => GetComponent<GUISlideIn>();

        [field: SerializeField]
        public Pages pages { get; set; }

        [field: SerializeField]
        public Bookmarks markers { get; set; }

        [field: SerializeField]
        public RightPage RightPage { get; set; }

        #endregion Properties

        #region Public Methods

        public void DropMenuState()
        {/*
        Type menuType = typeof(SCBookMenuState);
        var current = StoryController.Instance.StateMachine.CurrentState;
        Type currentType = current.GetType();
        if (menuType.IsAssignableFrom(currentType))
        {
            StoryController.Instance.StateMachine.DropState(current);
        }*/
            StoryController.Instance.bookMenuState.ExitMenu();
        }

        public void EnterSettingsState()
        {
            StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.settingsState);
        }

        public void EnterInventoryState()
        {
            StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.inventoryState);
        }

        public void EnterDataState()
        {
            StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.dataState);
        }

        public void EnterMapState()
        {
            StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.mapState);
        }

        public void EnterPartyState()
        {
            StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.partyState);
        }

        #endregion Public Methods

        #region Classes

        [Serializable]
        public class Pages
        {
            #region Fields

            public RectTransform settingsPage;
            public RectTransform dataPage;
            public RectTransform inventoryPage;
            public RectTransform partyPage;
            public RectTransform mapPage;
            public RectTransform logPage;

            #endregion Fields
        }

        [Serializable]
        public class Bookmarks
        {
            #region Fields

            public Image settingsPageLabel;
            public Image dataPageLabel;
            public Image inventoryPageLabel;
            public Image partyPageLabel;
            public Image mapPageLabel;
            public Image logMark;

            #endregion Fields
        }

        #endregion Classes
    }
}