-> Start

// Unity functions:
EXTERNAL Print(string)
EXTERNAL PrintWarning(string)
EXTERNAL Spd(float)
EXTERNAL Halt(float)
//EXTERNAL Stop()
EXTERNAL Clear()
EXTERNAL Bg(string)
EXTERNAL Sprites(string)
EXTERNAL Voice(string, float)
EXTERNAL Sfx(string, float)
EXTERNAL Ambiance(string, float)
EXTERNAL Music(string, float)

=== function Print(value)
<<i>Log: {value}</i>>

=== function PrintWarning(value)
<<i>LogWarning: {value}</i>>

=== function Spd(value) //number expressed as multiplier (eg .8 for 80%)
~return

=== function Halt(duration)
~return

=== function Stop()
<stop><br>

=== function Clear()
~return

=== function Bg(image)
<<i>Backdrop: {image}</i>>

=== function Sprites(image)
~return

=== function Voice(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>> // note to self: could also link this to tags or always have mumbling when someoneis speaking

=== function Sfx(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>>

=== function Ambiance(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>>

=== function Music(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>>

// End of List


=== Start ===
~ Print("Hello world!")// This prints the text to the unity console
~Bg("whiterun")
You should now see whiterun.<stop>
You should now see {Sprites("sample1")}one<stop>
{Sprites("sample1, sample2")}two,<stop>
{Sprites("sample 1, sample 2, sample3")}three characters appear.<stop>
They should {Sprites("")}now be gone.<stop>
~Music("the streets of whiterun",1)
You should hear music.<stop>
You should hear ambiant chatter {Ambiance("chatter", 1)}<b>now</b>.<stop>
The chatter should stop {Ambiance("",1)}now.<stop>
-> sfx
=== sfx ===
Ideally, a soft sound should play when {Sfx("gong", 0.5)}<b>this</b> word appears and a loud sound should play when {Sfx("gong", 2)}<b>this</b> word appears.
    Did that sound right? You can check it again if you want.
    + [Hit it again.] -> sfx
    + [Continue] -> spd
=== spd ===
{Spd(2)}<b>This</b> {Spd(1)}is a fast word, {Spd(0.5)}<b>this</b> {Spd(1)}is a slow word. 
{Spd(2)}<b>This {Spd(1)}is a fast sentence</b>, {Spd(0.5)}<b>this {Spd(1)}is a slow sentence</b>.
    Did you get that? Or do you want to see it again?
    + [Run it again.] -> spd
    + [Give me the sounds again.] -> sfx
    + [Continue] -> next
    
=== next ===
Text should stop flowing <b>now</b>.<stop> (And any text you type after the stop tag, such as this sentence, is lost.)
Experiment: how does the stop tag work<stop><>
with glue?
This sentence should <b>halt</b>{Halt(3)} for a few seconds before continueing.<stop>
After you next push Space, all text should be cleared.<stop>
~Clear()
-> flow
=== flow ===
VAR incrementedSpeed = 0
~incrementedSpeed = incrementedSpeed + 1
This text should appear after you've {pushed space|clicked the choice}, and run along for a while, even as it goes on long enough to {Spd(incrementedSpeed)}fill up multiple lines of the textbox, going on and on and on and on all the way as you wonder of it will ever stop.
~incrementedSpeed = incrementedSpeed + 1
On and on they go as the fleeting moments of your mortal life pass on and you wonder if any of this is worth it in the end, like, are we all just passing time on the inexorable road to the grave? 
~incrementedSpeed = incrementedSpeed + 1
Is there truly nothing more meaningful you could be doing with your life than just sit here and watch text go by farther and farther? 
{| |I Hate you.}
{| | |Why are you doing this?|Stop.|}
{ incrementedSpeed > 15:
Anyway here's wonderwall.
    +[Wait, what?]->movingOn
- else :
Anyway all of this trite should continue until you hit either the next stop tag, such as before, or the next set of choises, such as these:
    + [Tell me again, but faster this time.] -> flow
    
    + [Okay, that's quite enough of that.] -> movingOn
}

=== movingOn ===
~Spd(1)
Moving on.
    -> demo
    
=== demo ===
~Spd(1)
    <br>
    * [Show me inkle's demo.] Inkle's demo:
    <br>
- I looked at Monsieur Fogg. 
*   ... and I could contain myself no longer.
    'What is the purpose of our journey, Monsieur?'
    'A wager,' he replied.
    * *     'A wager!'[] I returned.
            He nodded. 
            * * *   'But surely that is foolishness!'
            * * *  'A most serious matter then!'
            - - -   He nodded again.
            * * *   'But can we win?'
                    'That is what we will endeavour to find out,' he answered.
            * * *   'A modest wager, I trust?'
                    'Twenty thousand pounds,' he replied, quite flatly.
            * * *   I asked nothing further of him then[.], and after a final, polite cough, he offered nothing more to me. <>
    * *     'Ah[.'],' I replied, uncertain what I thought.
    - -     After that, <>
*   ... but I said nothing[] and <>
- we passed the day in silence.
- -> END