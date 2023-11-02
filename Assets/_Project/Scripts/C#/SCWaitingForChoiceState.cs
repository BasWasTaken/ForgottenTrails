using System;
using System.Collections;
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
using items;

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        public partial class InterfaceBroking
        {
            public class SCWaitingForChoiceState : SCWaitingForInputState
            {
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
                    if (!DropCondition)
                    {
                        PresentButtons(); // create new choices
                    }
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();
                }
                public override void OnExit()
                {
                    //this not done here anymore, but elsewhere- when a choise is made. RemoveOptions(); // Destroy old choices
                }
                #endregion
                // Private Methods
                #region Private Methods
                const string opener = "{Unity.UseItem(";
                const string closer = ")}";
                internal void PresentButtons()
                {
                    if (Controller.Story.canContinue)
                    {
                        throw new Exception("no choices detected at this point");
                    }
                    else if (Controller.Story.currentChoices.Count > 0) /// Display all the choices, if there are any!
                    {
                        Controller.InterfaceBroker.RemoveOptions();
                        //Debug.Log("Choices detected!");
                        for (int i = 0; i < Controller.Story.currentChoices.Count; i++)
                        {

                            Choice choice = Controller.Story.currentChoices[i];
                            string input = choice.text;

                            if (input.Contains("{")) // automatically gets itemchoices, mapchoices, etc
                            {
                                int startIndex = input.IndexOf(opener);
                                int endIndex = input.IndexOf(closer, startIndex);

                                if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
                                {
                                    int substringLength = endIndex - startIndex - opener.Length;// (closer.Length-1); 
                                    string key = input.Substring(startIndex + opener.Length, substringLength);

                                    Debug.Log("Encountered hidden choice: " + key);
                                    Controller.InterfaceBroker.hiddenChoices.Add(key, choice);
                                }
                                else
                                {
                                }

                            }
                            else
                            {
                                Button button = PresentButton(choice.text.Trim());
                                /// Tell the button what to do when we press it
                                button.onClick.AddListener(delegate {
                                    Controller.InterfaceBroker.OnClickChoiceButton(choice);
                                });
                            }
                        }
                        //scrollbar.value = 0;
                        return;
                    }
                    /// If we've read all the content and there's no choices, the story is finished!
                    else
                    {
                        throw new NotImplementedException("No choices possible");
                    }
                }

                // Creates a button showing the choice text
                private Button PresentButton(string text)
                {
                    //Debug.Log("make button for " + text);
                    /// Creates the button from a prefab
                    Button choice = Instantiate(Controller.InterfaceBroker.ButtonPrefab) as Button;
                    choice.transform.SetParent(Controller.InterfaceBroker.ButtonAnchor, false);

                    /// Gets the text from the button prefab
                    TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
                    choiceText.text = text;


                    /// Make the button expand to fit the text
                    /* we don't want that, i want the reverse
        HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;
                    */

                    return choice;
                }
                #endregion
            }
        }
    }
}