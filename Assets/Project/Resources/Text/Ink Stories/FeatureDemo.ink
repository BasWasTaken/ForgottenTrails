-> Start

//BEGIN LIST Unity functionality

EXTERNAL Print(string)
// send text to unity console as message
=== function Print(value) 
<<i>Log: {value}</i>>

EXTERNAL PrintWarning(string)
// send text to unity console as warning
=== function PrintWarning(value)
<<i>LogWarning: {value}</i>>

EXTERNAL Halt(float) 
// [EXPERIMENTAL] pause text for x seconds 
=== function Halt(duration) 
<<i>Halt: {duration}</i>> 

EXTERNAL Clear()
 // Clears the textbox and moves that text to log
=== function Clear()
<<i>Clear Page</i>>

VAR stop = "\{stop\}" // Used in Unity for stopping the continue loop

VAR glue = "\{glue\}" // used to glue next line to this.

VAR aglue = "\{aglue\}" // used to glue this to previous line

VAR spd = 1.0 
EXTERNAL Spd(float) 
// [TEMPRAMENTAL] change the text speed 
=== function Spd(value) //positive number, expressed as multiplier (eg .8 for 80%)
~spd=value

EXTERNAL Bg(string)
 // sets background to image (fade WIP)
=== function Bg(image)
<<i>Backdrop: {image}</i>> 

EXTERNAL Sprites(string)
// sets sprites for characters, etc. 
=== function Sprites(images) //If multiple sprites, separated by comma.
<<i>Images: {images}</i>> 

EXTERNAL Vox(string, float)
// plays audio on voice channel, unlooped
=== function Vox(clip, volume) // use volume between 0.0 and 1.0
<<i>Vox: {clip}</i>> 
// note to self: could perhaps also play appropriate sound when someone is speaking via use of inky tags

EXTERNAL Sfx(string, float)
// plays audio on sfx channel, unlooped
=== function Sfx(clip, volume) // use volume between 0.0 and 1.0
<<i>Sfx: {clip}</i>>

EXTERNAL Ambiance(string, float)
// plays audio on ambiance channel, looping
=== function Ambiance(clip, volume) //use "" to stop loop.  use volume between 0.0 and 1.0
<<i>Ambiance: {clip}</i>>

EXTERNAL Music(string, float)
// plays audio on music channel, looping
=== function Music(clip, volume) //use "" to stop audio. use volume between 0.0 and 1.0
<<i>Music: {clip}</i>>
// END OF LIST Unity Functionality
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
~ Print("Hello world!")// This prints the text to the unity console
~Bg("whiterun")
~Spd(1)
You should now see <b>whiterun</b>.{stop}
You should now see {Sprites("b34auw3h_0")}one {glue}
{Sprites("b34auw3h_0, b34auw3h_1")}two, {glue}
{Sprites("b34auw3h_0, b34auw3h_1, b34auw3h_2")}three characters appear.{glue}{stop}
They should {Sprites("")}now be gone.{stop}
~Music("the streets of whiterun",1.0)
You should hear music.{stop}
You should hear ambiant chatter {glue}
{Ambiance("chatter", 1)}<b>now</b>.{stop}
The chatter should stop {glue}
{Ambiance("",1)}now.{stop}
-> sfx
=== sfx ===
Get ready for some sounds.
Ideally, a soft sound should play when {glue}
{Sfx("gong", 0.25)}<b>this</b> word appears, {glue}
{Sfx("",0)}and a loud sound should play when {glue}
{Sfx("gong", 1)}<b>this</b> word appears.
    Did that sound right? You can check it again if you want.
    + [Hit it again.] -> sfx
    + [Continue] -> spdtst
=== spdtst ===
{Spd(10)}<b>This</b> {glue}
{Spd(1)}is a fast word, {glue}{stop}
{Spd(0.1)}<b>this</b> {glue}
{Spd(1)}is a slow word.{stop}
{Spd(10)}<b>This is a fast sentence</b>, {glue}{stop}
{Spd(0.1)}<b>this is a slow sentence</b>.{stop}
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
This sentence should <b>halt</b> {glue}
{Halt(3)}for a few seconds before continueing.{stop}
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