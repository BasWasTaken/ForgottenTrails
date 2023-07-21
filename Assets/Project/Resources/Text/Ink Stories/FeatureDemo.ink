-> Start

// Unity functions:
EXTERNAL Print(string)
EXTERNAL PrintWarning(string)
VAR spd = 1.0 //number expressed as multiplier (eg .8 for 80%)
//EXTERNAL Spd(float)
EXTERNAL Halt(float)
//EXTERNAL Stop()
EXTERNAL Clear()
EXTERNAL Bg(string)
EXTERNAL Sprites(string)
EXTERNAL Vox(string, float)
EXTERNAL Sfx(string, float)
EXTERNAL Ambiance(string, float)
EXTERNAL Music(string, float)

=== function Print(value)
<<i>Log: {value}</i>>

=== function PrintWarning(value)
<<i>LogWarning: {value}</i>>

=== function Spd(value) 
~spd=value

=== function Halt(duration)
~return

=== function Stop()
#stop
<stop><br>

VAR stop = "\{stop\}"

VAR glue = "\{glue\}"

=== function Clear()
~return

=== function Bg(image)
<<i>Backdrop: {image}</i>>

=== function Sprites(image)
~return

=== function Vox(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>> // note to self: could also link this to tags or always have mumbling when someoneis speaking

=== function Sfx(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>>

=== function Ambiance(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>>

=== function Music(clip, volume) // volume between 0.0 and 1.0
<<i>Audio: {clip}</i>>

=== function Reset()
~Spd(1)
~Sfx("",1)
~Vox("",1)

// End of List
VAR Name = "PlayerName"


VAR players_gender = "Undefined"
VAR players_eyecolor = "Undefined"
VAR players_hair = "Undefined"
VAR players_hair_color = "Undefined"
VAR players_hair_style = "Undefined"

VAR androgynous = "androgynous"
VAR they = "they"
VAR them = "them"
VAR their = "their"
VAR theirs = "theirs"
VAR Mx = "Mx"
VAR master = "master"
VAR person = "person"
VAR kid = "kid"
VAR lad = "lass"
VAR guy = "guy"




=== Start ===
~Reset()
~ Print("Hello world!")// This prints the text to the unity console
~Bg("whiterun")
~Spd(1)
You should now see whiterun.{stop}
You should now see {Sprites("b34auw3h_0")}one {glue}
{Sprites("b34auw3h_0, b34auw3h_1")}two, {glue}
{Sprites("b34auw3h_0, b34auw3h_1, b34auw3h_2")}three characters appear.{glue}{stop}
They should {Sprites("")}now be gone.{stop}

~Music("the streets of whiterun",1)
You should hear music.{stop}
You should hear ambiant chatter {glue}
{Ambiance("chatter", 1)}<b>now</b>.{stop}
The chatter should stop {glue}
{Ambiance("",1)}now.{stop}
-> sfx
=== sfx ===
~ Reset()
Get ready for some sounds.
Ideally, a soft sound should play when {glue}
{Sfx("gong", 0.25)}<b>this</b> word appears and a loud sound should play when {glue}
{Sfx("",0)}{Sfx("gong", 1)}<b>this</b> word appears.
    Did that sound right? You can check it again if you want.
    + [Hit it again.] -> sfx
    + [Continue] -> spdtst
=== spdtst ===
{Spd(2)}<b>This</b> {glue}
{Spd(1)}is a fast word, {glue} 
{Spd(0.5)}<b>this</b> {glue}
{Spd(1)}is a slow word.{stop}
{Spd(100)}<b>This is a fast sentence</b>, {glue}
{Spd(0.01)}<b>this is a slow sentence</b>.{stop}
{Spd(1)}Did you get that? Or do you want to see it again?
    + [Run it again.] -> spdtst
    + [Give me the sounds again.] -> sfx
    + [Continue] -> next
    
=== next ===
This is just another line.{stop}
And so is this.
And this as well.
This working for you?{stop}
Text should stop flowing <b>now</b>.{stop} (Any text you type after the stop tag, such as this sentence, is sent anyway, which causes confusing behaviour.)
Experiment: how does the stop tag work{stop}<>
with glue?
This sentence should <b>halt</b>{Halt(3)} for a few seconds before continueing.{stop}
After you next push Space, all text should be cleared.{stop}
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