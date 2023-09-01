INCLUDE CustomFeatures
-> Start

=== Start ===
asdfasdf
~Spd(1)
asdfa dadsf
~ Print("Hello world!")// This prints the text to the unity console
~FadeToImage("Vault2",0)
You should now see <b>a vault</b>.{stop}
~FadeToImage("Vault1",1)
You should now see <b>another vault</b>.{stop}
~FadeToImage("Vault3",1)
You should now see <b>another vault</b>.{stop}
~FadeToColor("Black",10)
You should now see <b>black</b>.{stop}
~FadeToColor("White",0.2)
You should now see <b>white</b>.{stop}
~FadeToColor("Red",1)
You should now see <b>red</b>.{stop}
~FadeToImage("whiterun",0)
You should now see <b>whiterun</b>.{stop}
You should now see {glue}
{AddPortrait(Sprites.Alice)}
~ Sprites += Sprites.Alice
one {glue}
~ Sprites += Sprites.Robert
two, {glue}

three characters appear.{glue}{stop}
They should {glue}
~ Portraits = ()
now be gone.{stop}
~ PlayMusic(TheStreetsOfWhiterun, 1.0)
You should hear music.{stop}
You should hear ambiant chatter {glue}
{PlayAmbiance(chatter, 1)}<b>now</b>.{stop}
The chatter should stop {glue}
{PlayAmbiance((),1)}now.{stop}
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