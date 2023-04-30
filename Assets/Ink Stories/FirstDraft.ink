/*  
    This sample story contains an example of each custom function you have available to you (apart from all of Ink's innate features.
    
    Check out the documentation for tag use: 
    - https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#marking-up-your-ink-content-with-tags
    
    ther helpful links:
    - github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#5-functions
    - https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#part-5-advanced-state-tracking
    - https://www.patreon.com/posts/tips-and-tricks-18637020
*/

    //#backdrop:whiterun // this should set the background
    //#sprites:b34auw3h_0, b34auw3h_1, b34auw3h_2 // this should set images in front of the background (removing all previous ones)
    //#ambiance:chatter // this should set the ambiance
    //#music:the streets of whiterun // this should set the music
   // ~ Print("Hello world!")// This prints the text to the unity console
    // Later functions such as picking up items would ideally be handled in a similar matter, e.g. #pickup:sword or ~ Pickup("sword") depending on the implementation. I'll look at those in detail when we get to them and add them as we go along.
    //#sfx:gong // this plays a sound. 
    //#backdrop: this removes the backdrop
   // #sprites: this removes all sprites
   
-> Start // keep this above the external function
LIST VisitedState = yes, no
LIST TimeOfDay = Night, Dawn, Morning, Midday, Afternoon, Dusk, Evening
EXTERNAL Print(string)

=== function Print(a)
    <i>Print to console:</i> {a}

=== Start ===
~ VisitedState = no 
    -> CastleEntrance
    
=== Death ===
And so ends this tale. Another apprentice that would not return, their findings lost to the wind.
->END

=== CastleEntrance ===
{ VisitedState == no:
    -> CastleEntranceFirst
    - else: -> END
}
= CastleEntranceFirst
#backdrop: CastleGate
#music:
#ambiance:
As you crest the hilltop a gatehouse comes into view. Its stones are worn, ancient. The top parts seem to have crumbled at some point, having now been replaced by wooden battlements. As you step forward in approach you hear a voice call out loudly from inside the gatehouse:

"Halt! Who goes there, man or beast?"

*   "Man!" [] you shout back at him. 
    After a brief moment of quiet a reply comes from inside:
    "Alright lad, approach!"
    -> CastleEntranceFirstApproach
    
*   (joke)"Beast!" [] you shout back, mockingly.
    The voice replies:
    "Alright then you joker, come closer!"
    -> CastleEntranceFirstApproach    
    
*   [Stay quiet] A few seconds pass in silence. Then the voice rings out again, this time slightly quivering:
        "I won't ask again! Identify yourself!"
    **   [Identify yourself] "Just a traveler!" you shout.
        There's a slight pause and while you can't hear it through the thick walls, you can only imagine the guardsman let out a sigh of relief before replying
        "Don't scare me like that lad! Please, approach!"
        -> CastleEntranceFirstApproach
    **   [Remain quiet]
        You hear a bell being rung on the otherside of the wall. The next few moments pass by in a blurr as you hear several people shouting and running behind the wall. There's a low thud followed by a piercing pain. You look down at your chest and as you see the shaft of an arrow sticking out, you realize the gravity of your mistake. You try to raise your arms to somehow remedy the situation, but two more arrows find their mark. You fall to the ground, quickly shifting out of concsiousness and this mortal plane. 
-> Death

= CastleEntranceFirstApproach
//You bring yourself nearer to the gatehouse. Two wooden doors are set beneath the archway, barring the way in. As you stand before them a small latch is opened to reveal two {TimeOfDay = Dawntired blue eyes. 
Test
{CastleEntrance.CastleEntranceFirst.joke} A joker are we?
->END
