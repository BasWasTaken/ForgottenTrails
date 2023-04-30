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
