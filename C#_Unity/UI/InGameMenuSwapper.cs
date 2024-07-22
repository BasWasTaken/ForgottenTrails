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
        public Labels labels { get; set; }

        #endregion Properties

        #region Public Methods

        public void DropMenuState()
        {/*
        Type menuType = typeof(SCInGameMenuState);
        var current = StoryController.Instance.StateMachine.CurrentState;
        Type currentType = current.GetType();
        if (menuType.IsAssignableFrom(currentType))
        {
            StoryController.Instance.StateMachine.DropState(current);
        }*/
            StoryController.Instance.bookMenuState.ExitMenu();
        }

        public void ToggleSettingsScreen()
        {
            if (StoryController.Instance.StateMachine.CurrentState != StoryController.Instance.settingsState)
            {
                StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.settingsState);
            }
            else
            {
                StoryController.Instance.StateMachine.DropState(StoryController.Instance.bookMenuState); //                StoryController.Instance.bookMenuState.ExitMenu();?
            }
        }

        public void ToggleInventoryScreen()
        {
            if (StoryController.Instance.StateMachine.CurrentState != StoryController.Instance.inventoryState)
            {
                StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.inventoryState);
            }
            else
            {
                StoryController.Instance.StateMachine.DropState(StoryController.Instance.bookMenuState);
            }
        }

        public void ToggleDataScreen()
        {
            if (StoryController.Instance.StateMachine.CurrentState != StoryController.Instance.dataState)
            {
                StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.dataState);
            }
            else
            {
                StoryController.Instance.StateMachine.DropState(StoryController.Instance.bookMenuState);
            }
        }

        public void ToggleMapScreen()
        {
            if (StoryController.Instance.StateMachine.CurrentState != StoryController.Instance.mapState)
            {
                StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.mapState);
            }
            else
            {
                StoryController.Instance.StateMachine.DropState(StoryController.Instance.bookMenuState);
            }
        }

        public void TogglePartyScreen()
        {
            if (StoryController.Instance.StateMachine.CurrentState != StoryController.Instance.partyState)
            {
                StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.partyState);
            }
            else
            {
                StoryController.Instance.StateMachine.DropState(StoryController.Instance.bookMenuState);
            }
        }

        public void ToggleJournalScreen()
        {
            if (StoryController.Instance.StateMachine.CurrentState != StoryController.Instance.journalState)
            {
                StoryController.Instance.StateMachine.TransitionToState(StoryController.Instance.journalState);
            }
            else
            {
                StoryController.Instance.StateMachine.DropState(StoryController.Instance.bookMenuState);
            }
        }

        public void ToggleMenuScreen(StoryController.SCInGameMenuState menu)
        {
            if (menu.GetType() == typeof(StoryController.SCSettingsMenuState))
            {
                ToggleSettingsScreen();
            }
            else if (menu.GetType() == typeof(StoryController.SCInventoryMenuState))
            {
                ToggleInventoryScreen();
            }
            else if (menu.GetType() == typeof(StoryController.SCDataMenuState))
            {
                ToggleDataScreen();
            }
            else if (menu.GetType() == typeof(StoryController.SCMapMenuState))
            {
                ToggleMapScreen();
            }
            else if (menu.GetType() == typeof(StoryController.SCPartyMenuState))
            {
                TogglePartyScreen();
            }
            else if (menu.GetType() == typeof(StoryController.SCJournalMenuState))
            {
                ToggleJournalScreen();
            }
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
            public RectTransform journalPage;

            #endregion Fields
        }

        [Serializable]
        public class Labels
        {
            #region Fields

            public Image settingsPageLabel;
            public Image dataPageLabel;
            public Image inventoryPageLabel;
            public Image partyPageLabel;
            public Image mapPageLabel;
            public Image journalPageLabel;

            #endregion Fields
        }

        #endregion Classes
    }
}