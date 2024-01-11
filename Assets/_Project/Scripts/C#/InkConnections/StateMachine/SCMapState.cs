using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VVGames.Common;
using VVGames.ForgottenTrails.InkConnections.Travel;
using Debug = UnityEngine.Debug;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        /// <summary>
        ///
        /// </summary>

        #region Classes

        public class SCMapState : SCBookMenuState
        {
            // Inspector Properties

            // Public Properties

            // Private Properties

            // Public Methods

            #region Public Methods

            public override void OnEnter()
            {
                Controller.InterfaceBroker.book.pages.mapPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.mapMark.color = Color.clear;
                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text == "{UNITY:OpenMap}")
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);// hopelijk wordt ook dit niet dubbelop als je al van de visible optie komt.
                        Controller.Story.Continue();
                        Controller.InterfaceBroker.FindHiddenChoices();
                        break;
                    }
                }
                ShowOrHideTravelOptions();
            }

            public override void OnUpdate()
            {
                base.OnUpdate();
            }

            public override void OnExit()
            {
                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text.Contains("{UNITY:CloseMap}"))
                    {
                        Controller.Story.ChooseChoiceIndex(choice.index);
                        Controller.Story.ContinueMaximally();
                        break;
                    }
                }
                Controller.InterfaceBroker.book.markers.mapMark.color = Color.white;
            }

            #endregion Public Methods

            // Private Methods

            #region Private Methods

            private void ShowOrHideTravelOptions() // show all locations that are known to the player.
            {
                // first, hide all buttons.

                // then, for each button, check the known locations in ink, and if it's there, reveal that button.

                // then, for each location travelable according to inky, try to find and make interactable a matching button (that should by now have been revealed)
                // this means that outside of mapscenes, each button will remain noninteractable even when revealed.

                // first let's collect all the options ink has given us

                List<string> locationOptions = new();
                foreach (var found in Controller.InterfaceBroker.hiddenChoices) // taken hidden instead of current because these will be conveniently filtered to have only the location as text hopefully
                {
                    if (found.Value.Type == InterfaceBroking.SCWaitingForChoiceState.ChoiceType.Map)
                    {
                        locationOptions.Add(found.Key);
                    }
                }

                // then we'll go over each of the buttons in our mapscreen

                // get list of known locations
                InkList knownLocations = Controller.Story.state.variablesState["KnownLocations"] as InkList;

                foreach (var item in knownLocations)
                {
                    Debug.Log(item.Key.itemName);
                }

                foreach (MapLocationContainer item in Controller.InterfaceBroker.mapButtonsContainer.GetComponentsInChildren<MapLocationContainer>())
                {
                    // is canocinal location found in this list?
                    bool isKnown = knownLocations.ContainsItemNamed(item.canonicalLocation);

                    Debug.LogFormat("{0} is {1} known!", item.canonicalLocation, isKnown ? "" : "not");

                    // set (in)active depending on above
                    item.gameObject.SetActive(isKnown);

                    // is this location present in the options from ink?
                    bool canBeVisited = locationOptions.Contains(item.canonicalLocation);

                    // making them interactable or not
                    item.GetComponent<Button>().interactable = canBeVisited;
                    Debug.LogFormat("{0} can {1} be visited!", item.canonicalLocation, canBeVisited ? "" : "not");
                }

                // TODO 202401111659: add check the other way round: any prompts from ink that could not be matched, should produce an error.
            }

            #endregion Private Methods
        }

        #endregion Classes
    }
}