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

namespace ForgottenTrails.InkFacilitation
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        public partial class TextProduction
        {
            // Public Properties
            #region Public Properties

            public bool EncounteredStop { get; private set; } = false;
            public bool Peeking { get; private set; } = false;
            public bool Skipping { get; private set; } = false;
            public enum TextProducerStatus
            {
                Idle,
                Working_Base,
                Working_Typing,
                Paused,
                Done
            }
            [HideInInspector]
            public TextProducerStatus TPStatus = TextProducerStatus.Idle;

            internal float additionalPause = 0f;
            #endregion
            public class SCProductionState : SCSuperState
            {
                // Private Properties
                #region Private Properties
                private string NewText
                {
                    get; set;
                }
                Coroutine coroutine;
                #endregion
                // Public Methods
                #region Public Methods
                public override void OnEnter()
                {
                    if (!DropCondition)
                    {
                        //start advance?
                    }
                }
                public override void OnUpdate()
                {
                    base.OnUpdate();

                    if (Controller.TextProducer.TPStatus == TextProducerStatus.Working_Typing)
                    {
                        if (!Controller.TextProducer.Skipping)
                        {
                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                if (TimeSinceAdvance > Controller.InterfaceBroker.SkipDelay)
                                {
                                    Controller.TextProducer.Skipping = true;
                                }
                            }
                        }
                    }
                    else if (Controller.TextProducer.TPStatus == TextProducerStatus.Done)
                    {
                        Controller.TextProducer.TPStatus = TextProducerStatus.Idle;
                        StashStoryState(); // stash current scene state
                        DetermineNextTransition();
                    }
                    else if (Controller.TextProducer.TPStatus == TextProducerStatus.Idle)
                    {
                        AdvanceStory();
                    }
                }
                public override void OnExit()
                {
                    //suspend coroutine?
                }
                #endregion
                // Private Methods
                #region Private Methods
                private void AdvanceStory()
                {
                    TimeSinceAdvance = 0; // reset timer for skip button
                    Controller.TextProducer.TPStatus = TextProducerStatus.Working_Base; // NOTE OR transitionto writing
                    Controller.StartCoroutine(ProduceTextOuter()); // TODO perhaps move this method to here too
                }
                /// <summary>
                /// Runs the per line steps required for parsing and showing ink story
                /// </summary>
                public IEnumerator ProduceTextOuter()
                {
                    #region TryFit
                    Stopwatch stopwatch = new();
                    if (Controller.TextProducer.ClearWhenFull)
                    {
                        // TODO: construct future checker for all upcoming lines here
                        var storedState = Controller.Story.state.ToJson(); // set return point
                        Controller.TextProducer.Peeking = true; // begin traversal
                        string toBeAdded = "";
                        while (Controller.Story.canContinue) // continue maximally or to stop
                        {
                            toBeAdded += Controller.Story.Continue();
                            if (toBeAdded.Contains("{stop}")) break;
                        }

                        // check size and clear page if needed

                        string bufferText = Controller.TextProducer.CurrentText; // make backup
                        Controller.TextProducer.VisibleCharacters = Controller.TextProducer.TextBox.text.Length;

                        Controller.TextProducer.CurrentText += toBeAdded + '\n'; // test if the text will fit
                        yield return 0;// wait 1 frame
                        bool overflow = Controller.TextProducer.OverFlowTextBox.text.Length > 0; // store result

                        Controller.TextProducer.CurrentText = bufferText; // restore backup
                        Controller.Story.state.LoadJson(storedState); // return to original state, reverting from peaking
                        Controller.TextProducer.Peeking = false;

                        if (overflow) { Controller.TextProducer.ClearPage(); } // clear page if needed
                    }
                    #endregion TryFit
                    #region continueloop
                    do
                    {
                        if (Controller.StateMachine.CurrentState != this) yield return new WaitUntil(() => Controller.StateMachine.CurrentState == this);

                        // NOTE: do i want to add ifspace remaining in textbox check here per line?
                        #region InnerLoop
                        Controller.Story.ContinueAsync(0); // advance a bit 
                        string newLine = Controller.Story.currentText; // get the next line up until there
                                                                       // string newLine = story.Continue();

                        string parsedText = ParseText(newLine);

                        NewText = parsedText;
                        //Debug.Log("Write a line: " + newLine);
                        if (Controller.TextProducer.PendingFunctions.Count > 0)
                        {
                            yield return new WaitUntil(() => Controller.TextProducer.PendingFunctions.Count == 0); // wait till functions dfone
                        }
                        if (Controller.StateMachine.CurrentState != this)
                        {
                            yield return new WaitUntil(() => Controller.StateMachine.CurrentState == this); // only continue in right state
                        }


                        Controller.TextProducer.TPStatus = TextProducerStatus.Working_Typing;  // Indicate we are now typing

                        if (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.CurrentText.Length)
                        {
                            Controller.TextProducer.VisibleCharacters = Controller.TextProducer.CurrentText.Length;
                            Debug.LogWarning("Not all characters were done.");
                        }

                        string backup = Controller.TextProducer.CurrentText; // make backup
                        Controller.TextProducer.CurrentText += NewText; //add the new text, invisibly
                        if (Controller.TextProducer.OverFlowTextBox.text.Length > 0) // check for overflow
                        {
                            Controller.TextProducer.CurrentText = backup; // restore backup
                            Controller.TextProducer.ClearPage(); // save page and clear it
                            Controller.TextProducer.CurrentText = NewText; // add new text anyway
                        }

                        #region RevealLetters
                        string message = string.Format("On speed {0} (base {1}) wrote:\nChar\t\tDelay\t\tIntended\t\tExtra", Controller.TextProducer.TextSpeedActual, Controller.TextProducer.TextSpeedPreset); // prepare console message
                        int tagLevel = 0; ///int to remember if we go down any nested tags
                        char letter;///prepare marker             
                        while (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.TextBox.text.Length) /// while not all characters are visible
                        {
                            if (!Controller.isActiveAndEnabled) yield return new WaitUntil(() => Controller.isActiveAndEnabled); // only continue if enabled
                            if (Controller.StateMachine.CurrentState != this) yield return new WaitUntil(() => Controller.StateMachine.CurrentState == this); // only continue in right state
                            letter = Controller.TextProducer.TextBox.text[Controller.TextProducer.VisibleCharacters]; /// store letter we're typing letter   
                            Controller.TextProducer.VisibleCharacters++; /// show the character
                            if (letter == '<')/// if we come across this we've entered a(nother) tag
                            {
                                tagLevel++;
                            }
                            else if (letter == '>')/// if we come acros this, we've exited one level of tag
                            {
                                tagLevel--;
                                if (tagLevel < 0)
                                {
                                    throw new("That's more closing than opening tag brackets!");
                                }
                            }
                            else if (tagLevel == 0) /// if we're not in a tag, apply potential delays for the typewriting effect
                            {
                                //Debug.Log("show me " + letter);
                                float delay = 0; /// initialize delay
                                if (!Controller.TextProducer.Skipping) /// in normal behaviour
                                {
                                    /// get the delay from the delay info object
                                    delay = Controller.TextProducer.PauseInfoNormal.GetPause(letter) / Controller.TextProducer.TextSpeedActual;
                                }
                                else if (Controller.TextProducer.AlwaysPause) /// if this setting is enabled, 
                                {
                                    ///get pause info while skipping text too
                                    delay = Controller.TextProducer.PauseInfoSkipping.GetPause(letter);
                                }
                                if (delay > 0) yield return new WaitForSecondsRealtime(delay); /// apply the delay if any
                                float delayMs = delay * 1000 + Controller.TextProducer.additionalPause;
                                float delayActual = stopwatch.ElapsedMilliseconds;
                                message += string.Format("\n\'{0}\'\t\t{1} ms\t\t{2} ms\t\t {3} ms", letter, delayActual, delayMs, delayActual - delayMs);
                                stopwatch.Restart();
                            }
                        }

                        Debug.Log(message);
                        #endregion RevealLetters

                        Controller.TextProducer.TPStatus = TextProducerStatus.Working_Base;
                        #endregion InnerLoop
                        Debug.Log("test produce");

                        if (Controller.TextProducer.EncounteredStop)  // if we encounter a stop
                        {
                            Controller.TextProducer.EncounteredStop = false;
                            // exit the loop or continue with a small delay
                            if (Controller.TextProducer.AutoAdvance)
                            {
                                yield return new WaitForFixedUpdate();
                            }
                            else
                            {
                                break;
                            }
                        }
                    } while (Controller.Story.canContinue); // while the story can continue without input on choices
                    #endregion continueloo
                    Controller.TextProducer.Skipping = false; // turn of skipping if it was on

                    Controller.TextProducer.TPStatus = TextProducerStatus.Done;
                }
                internal void ClearPage()
                {
                    if (Controller.TextProducer.Peeking) return;
                    Controller.TextProducer.HistoryTextBox.text = Controller.TextProducer.CurrentText; // move all text to the history log
                    Controller.TextProducer.CurrentText = ""; // clear current and prospective texts
                    Controller.TextProducer.VisibleCharacters = 0;
                }

                internal string ParseText(string input)
                {
                    Regex tags = new(@"\{([^>]+)\}");

                    foreach (Match match in tags.Matches(input))
                    {
                        //Debug.Log(string.Format("Encountered command {0} in {1}",match.Value,output));

                        if (match.Value == "{stop}")
                        {
                            //Debug.Log("recognised stop command");
                            Controller.TextProducer.EncounteredStop = true;
                            //catchAfterStop = input.Substring(match.Index+match.Value.Length);

                            //input = input.Substring(0, match.Index); 
                        }
                        else if (match.Value == "{glue}")
                        {
                            input = input.TrimEnd('\n'); /// remove linebreak from this
                        }
                        else if (match.Value == "{aglue}")
                        {
                            Controller.TextProducer.TextBox.text = Controller.TextProducer.TextBox.text.TrimEnd('\n'); /// remove linebreak from previous
                        }

                        input = input.Replace(match.Value, ""); /// remove the command
                    }

                    string output = "";

                    if (input == "<br>\n" | input == "<br>") /// when hitting explicit linebreak //which is it?
                    {
                        output += "\n"; //add a newline
                    }
                    else
                    {
                        output = input; /// leave the input unaltered
                    }

                    //Debug.Log("Write line as: " + output);
                    return output;
                }

                private void DetermineNextTransition()
                {
                    if (Controller.Story.canContinue)
                    {
                        Machine.TransitionToState(Controller.waitingForContinueState);
                    }
                    else
                    {
                        if (Controller.InterfaceBroker.CanPresentChoices())
                        {
                            Machine.TransitionToState(Controller.waitingForChoiceState);
                        }
                        else
                        {
                            OnInteractionEnd();
                        }
                    }
                }
                private void OnInteractionEnd()
                {
                    // Disengage with current story / dialogue, as we have seen its end or chose a goodbye option.
                    Controller.Story.RemoveVariableObserver();
                    StashStoryState();
                    Controller.InkStoryAsset = null;
                    Controller.Story = null;
                    Debug.Log(new NotImplementedException());
                    // TODO: LATER: evt volgende story feeden
                    DropCondition = true;
                }

                #endregion
            }
        }

    }
}