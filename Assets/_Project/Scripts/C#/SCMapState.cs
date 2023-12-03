using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Bas.Utility;
using DataService;
using Ink.Runtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;
using Travel;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        /// <summary>
        /// 
        /// </summary>
 
        public class SCMapState : SCBookMenuState
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

                Controller.InterfaceBroker.book.pages.mapPage.SetAsLastSibling();
                Controller.InterfaceBroker.book.markers.mapMark.color = Color.clear;
                foreach (var choice in Controller.Story.currentChoices)
                {
                    if (choice.text=="{UNITY:OpenMap}") 
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
                Controller.InterfaceBroker.book.markers.mapMark.color = Color.white;
            }
            #endregion
            // Private Methods
            #region Private Methods
            void ShowOrHideTravelOptions() // show all locations that are known to the player.
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

                foreach (MapLocationContainer item in Controller.InterfaceBroker.mapButtonsContainer.GetComponentsInChildren<MapLocationContainer>())
                {

                    // is canocinal location found in this list?
                    bool isKnown = knownLocations.ContainsItemNamed(item.canonicalLocation);

                    // set (in)active depending on above
                    item.gameObject.SetActive(isKnown);

                    // is this location present in the options from ink?
                    bool canBeVisited = locationOptions.Contains(item.canonicalLocation);

                    // making them interactable or not
                    item.GetComponent<Button>().interactable = canBeVisited;

                }


                // TODO: add check the other way round: any prompts from ink that could not be matched, should produce an error.

            }
            #endregion
        }
    }
}