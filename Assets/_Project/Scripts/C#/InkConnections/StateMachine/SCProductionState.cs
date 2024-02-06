using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using VVGames.Common;
using Debug = UnityEngine.Debug;

namespace VVGames.ForgottenTrails.InkConnections
{
    public partial class StoryController : MonoSingleton<StoryController>
    {
        #region Classes

        public partial class TextProduction
        {
            // Public Properties

            #region Fields

            [HideInInspector]
            public TextProducerStatus TPStatus = TextProducerStatus.Idle;

            internal float scriptedPause = 0f;

            #endregion Fields

            #region Enums

            public enum TextProducerStatus
            {
                Idle,
                Working_Base,
                Working_Typing,
                Paused,
                Done
            }

            #endregion Enums

            #region Properties

            public bool EncounteredStop { get; private set; } = false;
            public bool Peeking { get; private set; } = false;
            public bool Skipping { get; private set; } = false;

            #endregion Properties

            #region Classes

            public class SCProductionState : SCSuperState
            {
                // Private Properties

                #region Fields

                internal float timeSinceLastCharacter;

                private int tagLevel = 0;

                private char letter;

                private float typeWriterPause = 0;

                private Stopwatch Stopwatch = new();

                private float delayExpec = 0;

                private string letters;

                #endregion Fields

                #region Properties

                // make into class?? nah..
                internal string report { get; set; } = "";

                private string NewText
                {
                    get; set;
                }

                #endregion Properties

                // Public Methods

                #region Public Methods

                public override void OnEnter()
                {
                    if (!DropCondition)
                    {
                        //start advance?
                        timeSinceLastCharacter = 0f;
                    }
                }

                public override void OnUpdate()
                {
                    base.OnUpdate();
                    if (Controller.TextProducer.TPStatus == TextProducerStatus.Working_Typing)
                    {
                        timeSinceLastCharacter += Time.deltaTime;
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            if (!Controller.TextProducer.Skipping)
                            {
                                if (TimeSinceAdvance > Controller.InterfaceBroker.SkipDelay)
                                {
                                    Controller.TextProducer.Skipping = true;
                                }
                            }
                            else
                            {
                                Debug.LogWarning("Already Skipping!");
                            }
                        }
                        UpdateTypeWriter();
                    }
                    else if (Controller.TextProducer.TPStatus == TextProducerStatus.Done)
                    {
                        Controller.TextProducer.TPStatus = TextProducerStatus.Idle;
                        UpdateDataAsset(); // stash current scene state
                        DetermineNextTransition();
                    }
                    else if (Controller.TextProducer.TPStatus == TextProducerStatus.Idle)
                    {
                        if (Controller.Story.canContinue)
                        {
                            AdvanceStory();
                        }
                        else
                        {
                            DetermineNextTransition();
                        }
                    }
                }

                public override void OnExit()
                {
                    //suspend coroutine?
                }

                #endregion Public Methods

                // Private Methods

                #region Internal Methods

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
                            Controller.TextProducer.CurrentText = Controller.TextProducer.CurrentText.TrimEnd('\n'); /// remove linebreak from previous
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

                #endregion Internal Methods

                #region Private Methods

                private void AdvanceStory()
                {
                    TimeSinceAdvance = 0; // reset timer for skip button
                    Controller.TextProducer.TPStatus = TextProducerStatus.Working_Base;
                    InitiateTextProduction();
                }

                /// <summary>
                /// Runs the per line steps required for parsing and showing ink story
                /// </summary>
                private void InitiateTextProduction()
                {
                    // check for a previous mistake. is this check needed tho?
                    if (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.CurrentText.Length)
                    {
                        Controller.TextProducer.VisibleCharacters = Controller.TextProducer.CurrentText.Length;
                        Debug.LogWarning("Not all characters were done.");
                    }
                    Controller.TextProducer.typingSound.Play();

                    // Test the fit of the new text, clearing page if needed.
                    ClearPageIfNeeded();

                    TextLoop(); // start textloop
                }

                private void TextLoop()
                {
                    // get next line using cotninue async, and parse it using our custom code
                    NewText = ParseText(Controller.Story.Continue());

                    ForceFitText(NewText);

                    // further state and function checks should not be necessary: i've got my statemachine for that and i should trust it. if it doesn't work, i want to discover that so i can fix it.

                    // set state to typing (by lack of better option)
                    report = string.Format("On speed {0} (base {1}) wrote:\nChar\t\tIntended\t\tActual\t\tExtra\n", Controller.TextProducer.TextSpeedActual, Controller.TextProducer.TextSpeedPreset); // prepare console report
                    letter = new();
                    Controller.TextProducer.TPStatus = TextProducerStatus.Working_Typing; // indicate we are typing, and let the method in update() do the rest
                }

                private void ClearPageIfNeeded()
                {
                    var storedState = Controller.Story.state.ToJson(); // set return point
                    Controller.TextProducer.Peeking = true; // begin traversal
                    string paragraphToBeAdded = "";
                    while (Controller.Story.canContinue) // continue maximally or to stop
                    {
                        string lineToBeAdded = Controller.Story.Continue();
                        if (lineToBeAdded.Contains("{stop}")) break;
                        paragraphToBeAdded += lineToBeAdded;
                    }
                    // check size and clear page if needed

                    bool overflow = !DoesTextFit(paragraphToBeAdded);

                    Controller.Story.state.LoadJson(storedState); // return to original state, reverting from peaking
                    Controller.TextProducer.Peeking = false;

                    if (overflow) { Controller.TextProducer.ClearPage(); } // clear page if needed
                }

                private void ForceFitText(string text)
                {
                    if (!DoesTextFit(text))
                    {
                        Debug.LogWarning("Clearing page forcibly- I've not yet configured the code to clear the page preemptively without requiring a coroutine and waited frames.");
                        //Debug.Log("trying to fit " + text);
                        Controller.TextProducer.ClearPage(); // save page and clear it
                    }
                    Controller.TextProducer.CurrentText += text; //add the new text, invisibly
                }

                /// <summary>
                ///
                /// </summary>
                /// <param name="text"></param>
                /// <returns></returns>
                private bool DoesTextFit(string text)
                {
                    string backup = Controller.TextProducer.CurrentText; // make backup
                    //Debug.Log(Controller.TextProducer.TextBox.textInfo.lineCount);
                    Controller.TextProducer.CurrentText += text; // this is not spotted until after a frame...
                                                                 // can i do this manually somehow? by counting the chars and/or linebreaks? that seems very fallible...

                    // Debug.Log(Controller.TextProducer.TextBox.textInfo.lineCount);
                    //if (Controller.Story.currentText.EndsWith((Controller.TextProducer.CurrentText[^2]))) // rudementary check if at choice
                    //    {
                    /*
                                            foreach (var choice in Controller.Story.currentChoices)
                                            {
                                                //                        Debug.Log("testing choices fit...");
                                                Controller.TextProducer.CurrentText += choice.text + '\n' + '\n'+'\n';
                                            }
                                        //  }

                                        */

                    bool overflow = Controller.TextProducer.TextBox.textInfo.lineCount > Controller.TextProducer.maxVis; // store result
                    Controller.TextProducer.CurrentText = backup; // restore backup
                    return !overflow;
                }

                private void UpdateTypeWriter()
                {
                    if (Stopwatch.IsRunning)
                    {
                        float delayActual = MathF.Round(Stopwatch.ElapsedMilliseconds);
                        Stopwatch.Stop();

                        report += string.Format(" ms\t\t{0} ms\t\t {1} ms\n", delayActual, delayActual - delayExpec);
                        letters = "";
                        delayExpec = 0;
                    }
                    float timeLeft = typeWriterPause - timeSinceLastCharacter;
                    if (timeLeft > 0)
                    {
                        // wait
                        if (timeLeft > .1f)
                        {
                            //                            Debug.Log("pause");
                            Controller.TextProducer.typingSound.Pause();
                        }
                    }
                    else
                    {
                        if (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.CurrentText.Length)
                        {
                            // if more character are waiting, type them
                            TypeCharactersUsingDelays();
                        }
                        else
                        {
                            // else indicate done
                            IndicateLineDone();
                        }
                    }
                }

                private void IndicateLineDone()
                {
                    Controller.TextProducer.typingSound.Pause();
                    //Debug.Log(report);
                    // if done, indicate so (ideally i guess iw ould use an event, but for now it's fine to call an encounterstop method)

                    //   in which we continue with a small delay (calling initate text production again if story can still continue) or we exit if either is not the case
                    // and also in that function textproducestatus is set to done again. and skipping is set to false.

                    Controller.TextProducer.TPStatus = TextProducerStatus.Working_Base;

                    if (Controller.TextProducer.TPStatus == TextProducerStatus.Working_Base)
                    {
                        //Debug.Log("checking next");
                        // ifelse logic could be better here
                        if (!Controller.Story.canContinue)
                        {
                            //Debug.Log("this cannot continue starts playing");
                            IndicateTextDone();
                        }
                        else
                        {
                            if (Controller.TextProducer.EncounteredStop)// if we encounter a stop
                            {
                                Controller.TextProducer.EncounteredStop = false;
                                // exit the loop or continue with a small delay
                                if (Controller.TextProducer.AutoAdvance)
                                {
                                    // Might be a good idea to add a small delay here, to fire after each continue, if the dotpauses are not enough.
                                    TextLoop();
                                    return;
                                }
                                else
                                {
                                    IndicateTextDone();
                                    return;
                                }
                            }
                            else
                            {
                                TextLoop();
                                return;
                            }
                        }
                    }
                }

                private void IndicateTextDone()
                {
                    Controller.TextProducer.Skipping = false; // turn of skipping if it was on

                    Controller.TextProducer.TPStatus = TextProducerStatus.Done;
                }

                private void TypeCharactersUsingDelays(bool checkPageAgain = true)
                {
                    Stopwatch.Stop();
                    typeWriterPause = 0;
                    float speed = Controller.TextProducer.TextSpeedActual;
                    if (Controller.TextProducer.Skipping)
                    {
                        if (Controller.TextProducer.StillPauseWhileSkipping)
                        {
                            speed *= (float)TextSpeed.bonkers;
                        }
                        else
                        {
                            speed = Mathf.Infinity;
                        }
                    }
                    Controller.TextProducer.typingSound.UnPause();
                    float cps = speed / Controller.TextProducer.Pauses.normalPause;
                    float fps = Application.targetFrameRate;
                    if (cps > fps) // if tasked to draw characters more frequently than once per frame
                    {
                        // we need to draw multiple per frame
                        float maxDelay = Time.smoothDeltaTime; // get an estimate of how much time we have
                        do
                        {
                            // if any characters left to reveal
                            if (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.CurrentText.Length)
                            {
                                TypeSingleCharacter(); // reveal 1 character and add up the delay
                            }
                            else
                            {
                                break; // we're done
                            }
                        } while (maxDelay > typeWriterPause);
                    }
                    else
                    {
                        TypeSingleCharacter(); // just type 1 character
                    }

                    delayExpec = MathF.Round(typeWriterPause * 1000 + Controller.TextProducer.scriptedPause);
                    report += string.Format("\'{0}\'\t\t{1}", letters, delayExpec);
                    Stopwatch.Restart(); //start measurement from this moment to the capture
                }

                private void TypeSingleCharacter()
                {
                    // reveal next
                    if (Controller.TextProducer.VisibleCharacters < Controller.TextProducer.CurrentText.Length)
                    {
                        // if not all characters have been drawn
                        letters += letter = Controller.TextProducer.CurrentText[Controller.TextProducer.VisibleCharacters]; // get new letter
                        Controller.TextProducer.VisibleCharacters++;  //show it
                        timeSinceLastCharacter = 0;
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
                        else // for any other type of cahracter:
                        {
                            if (tagLevel == 0) // if we're not in a tag,
                            {
                                //apply potential delays for the typewriting effect
                                if (Controller.TextProducer.Skipping & !Controller.TextProducer.StillPauseWhileSkipping) // if skipping and set to not apply any delay then
                                {
                                    // leave at zero
                                }
                                else
                                {
                                    float speed = Controller.TextProducer.TextSpeedActual;
                                    if (Controller.TextProducer.Skipping) speed *= (float)TextSpeed.bonkers; // accelerate if skipping
                                    float additionalPause = Controller.TextProducer.Pauses.GetPause(letter) / speed; // get pause info, offset it by higher speeds.
                                    typeWriterPause += additionalPause;

                                    //float soundSpeed = ((((speed / 10) + 100) / additionalPause) / 1000) / 2 - 1;
                                    float modifier = Mathf.InverseLerp((int)TextSpeed.slow, (int)TextSpeed.fast, speed) * 2; // doubling this sets the average on 1.
                                    float baseSpeed = 1.75f;
                                    Controller.TextProducer.typingSound.pitch = Mathf.Clamp(modifier * baseSpeed, .5f, 2f);
                                    //if (letter == '.') Controller.TextProducer.typingSound.pitch = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
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
                    UpdateDataAsset();
                    Controller.InkStoryAsset = null;
                    Controller.Story = null;
                    Debug.Log(new NotImplementedException());
                    // This is where we'd feed a next story if relevant, but I don't think it will be.
                    // To elaborate: in other games, ink can be used to feed content through a particular object or character, close that, then move on to another.
                    // But in this game, it's all within one story structure- we never leave an ink story, so we never need to load a new one.
                    DropCondition = true;
                }

                #endregion Private Methods
            }

            #endregion Classes
        }

        #endregion Classes
    }
}