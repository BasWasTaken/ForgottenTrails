using Bas.ForgottenTrails.InkConnections;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bas.ForgottenTrails.UI
{
    /// <summary>
    /// <para>Summary not provided.</para>
    /// </summary>
    public class Book : MonoBehaviour
    {
        #region Properties

        [field: SerializeField]
        public Pages pages { get; set; }

        [field: SerializeField]
        public Bookmarks markers { get; set; }

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

            public RectTransform settingPage;
            public RectTransform dataPage;
            public RectTransform inventoryPage;
            public RectTransform partyPage;
            public RectTransform mapPage;

            #endregion Fields
        }

        [Serializable]
        public class Bookmarks
        {
            #region Fields

            public Image settingMark;
            public Image dataMark;
            public Image inventoryMark;
            public Image partyMark;
            public Image mapMark;

            #endregion Fields
        }

        #endregion Classes
    }
}