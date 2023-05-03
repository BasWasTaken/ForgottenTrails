-> Start // keep this above the external function
LIST VisitedState = yes, no

LIST TimeOfDay = Night, Dawn, Morning, Midday, Afternoon, Dusk, Evening

EXTERNAL Print(string)

=== function Print(a)
    <i>Print to console:</i> {a}

=== Start ===
~ VisitedState = no
~ TimeOfDay = Dawn
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
    With a hint of anger, the voice replies:
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
You bring yourself nearer to the gatehouse. Two wooden doors are set beneath the archway, barring the forward. As you stand before them a small latch is opened to reveal two {TimeOfDay == Dawn: tired blue eyes} crowned by a pair of bushy eyebrows{CastleEntranceFirst.joke: that are frowning in disaproval}.

{CastleEntranceFirst.joke: The man sighs, "Look lad, I appreciate your attempt at a joke -Gods know we could use some more humor out here- but on a bad day behaviour like that could get you killed. I've heard stories of wraiths speaking, wouldn't want to accidentally think you're one." | "Welcome to the castle lad, safest place in the North!" The man's eyes smile, most likely along with his mouth that's still hidden behind the door, "Sorry for all the precaution, can't be too careful with them wraiths out there."}
*"Could you let me in?"
    {CastleEntranceFirst.joke: "Alright, just behave would ye?" | "Of course lad, just a moment."}
    The man steps back and swings the latch shut. You hear the rustling of keys and the clunky rattling of locks, followed by a single door being opened inwards. The man stands behind it holding the door open for you with one hand while leaning on a spear with the other. He's an old sort, nearing his fifties, but broadchested and with seemingly a strong arm. 
    
    As you step inside the man shuts the door again behind you, taking great care to put the locks back into place. 
        **Engage the man in some further conversation
        -> EdgarGatehouse
        **(thanked)Thank the man and head into town
        ->EdinburghCastleGatehouseWalkway
        **(ignored)Say nothing and keep walking
        ->EdinburghCastleGatehouseWalkway
*"Wraiths?"
*"Do you often have to kill creatures gently walking up to the door?"

=== EdinburghCastleGatehouseWalkway ===
->END
=== EdgarGatehouse ===

-> END