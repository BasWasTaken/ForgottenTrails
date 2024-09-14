using Ink.Runtime;
using System;
using TMPro;
using UnityEngine.UI;
using VVGames.Common;
using Debug = UnityEngine.Debug;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public partial class InterfaceBroking
        {
            #region Classes

            public class SCWaitingForChoiceState : SCWaitingForInputState
            {
                // Public Properties

                // Private Properties

                // Public Methods

                #region Enums

                public enum ChoiceType
                {
                    Item,
                    Map,
                    Party
                }

                #endregion Enums

                #region Public Methods

                public override void OnEnter()
                {
                    if (!DropCondition)
                    {
                        PresentButtons(); // create new choices
                    }
                    EnableButtons(true);
                }

                public override void OnUpdate()
                {
                    base.OnUpdate();
                }

                public override void OnExit()
                {
                    EnableButtons(false);
                }

                #endregion Public Methods

                #region Internal Methods

                internal void EnableButtons(bool enable = true)
                {
                    foreach (Button button in Controller.InterfaceBroker.ButtonAnchor.GetComponentsInChildren<Button>())
                    {
                        button.interactable = enable;
                    }
                }

                internal void DisableButtons() => EnableButtons(false);

                internal void PresentButtons()
                {
                    if (Controller.Story.canContinue)
                    {
                        Debug.LogWarning("can continue- should do that before asking choices");
                        // go to writing state?
                        DropCondition = true;
                    }
                    else if (Controller.Story.currentChoices.Count > 0) /// Display all the choices, if there are any!
                    {
                        Controller.InterfaceBroker.RemoveOptions();
                        //Debug.Log("Choices detected!");
                        for (int i = 0; i < Controller.Story.currentChoices.Count; i++)
                        {
                            Choice choice = Controller.Story.currentChoices[i];
                            if (Controller.InterfaceBroker.TryAddHiddenChoice(choice)) { }
                            else if (choice.text == "{UNITY:" +
                                     "OpenMap" +
                                     "}")
                            {
                                //Controller.InterfaceBroker.InGameMenu.labels.mapPageLabel.GetComponent<Button>().interactable = true; // allow the use of the map button
                                // nee, ik denk te moeilijk! dit hoeft niet de knop te enabelen, gewoon wanneer dit er is kan de speler als het goed is o pde knop drukken en gaan reizen, maar hij kan altidj drukken.
                                Debug.Log("Info: Map Travel Available (Bas has not yet put in a notification or whatever)");
                            }
                            else if (choice.text == "{UNITY:" +
                                     "OpenPartyScreen" +
                                     "}")
                            {
                                Debug.Log("Info: Party Discussion Available (Bas has not yet put in a notification or whatever)");
                            }
                            else
                            {
                                Button button = PresentButton(choice.text.Trim());
                                /// Tell the button what to do when we press it
                                button.onClick.AddListener(delegate
                                {
                                    Controller.InterfaceBroker.OnClickChoiceButton(choice);
                                });
                            }
                        }
                        //scrollbar.value = 0;
                        return;
                    }

                    else// if(Controller.InterfaceBroker.hiddenChoices.Count==0)
                    {
                        // If we've read all the content and there's no choices, the story is finished!

                        throw new NotImplementedException("No choices possible");
                    }
                }

                #endregion Internal Methods

                #region Private Methods

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

                #endregion Private Methods

                #region Classes

                // Private Methods
                public class HiddenChoice
                {
                    #region Fields

                    public ChoiceType Type;
                    public Choice Choice;

                    #endregion Fields

                    #region Public Constructors

                    public HiddenChoice(ChoiceType Type, Choice Choice)
                    {
                        this.Type = Type;
                        this.Choice = Choice;
                    }

                    #endregion Public Constructors
                }

                #endregion Classes
            }

            #endregion Classes
        }

        #endregion Classes
    }
}