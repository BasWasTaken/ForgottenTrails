-> Start // keep this above the external function
LIST VisitedState = Yes, No

LIST TimeOfDay = Night, Dawn, Morning, Midday, Afternoon, Dusk, Evening

VAR Name = "Alex"


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

EXTERNAL Print(string)

=== function Print(a) ===
    <i>Print to console:</i> {a}

=== Start ===
#backdrop:whiterun
~ TimeOfDay = Dawn
    -> Opening
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END

=== Opening ===
#backdrop:Vault1
The smell of dusty books fills your nostrils. Around you stark white pillars stretch upward to support an almost impossible ceiling, draped in downward facing flowers made of stone. Against the wall countless bookshelves are lined up. You see various cloaked figures milling about; carrying books to and fro, replacing volumes, having heated (but hushed) discussions and, of course, reading. The near endless shelves seem to only surrender their stranglehold on the place to the stained glass windows, although you get the feeling that those too would be covered by endless books if their caretakers could work in the dark. 
<br>
It's a place that in a forgotten past held a different name but you know it by two: The Vault of Forgotten Books and Home. 
<br>
Home? Didn't you leave home months ago?
<br>
You ponder this briefly. A dream perhaps? Maybe. Surely! Maybe... 
The thought has nearly solidified in your mind when an unexpected blow knocks you to the ground. As you look up you see one of the caretakers on the floor, just like you, with books scattered about everywhere. The young man begins to apologize profusely, all the while gathering the dropped books. Before you can even get a word in he has already reformed the towering stack, undoubtedly the thing that obstructed his vision in the first place, and has continued on his way. <br>
On the floor, a mirror sparkles with light's reflection. Did he drop this?
*(MirrorY)[Pick it up]
As you reach to pick it up, a reflection stares back at you.
-> Opening.CharCreation0
*(MirrorN)[Leave it be]
If he left it behind then that is his problem. But as you stand up you cannot help but notice your reflection in the glass.
-> Opening.CharCreation0
= CharCreation0
You see <>
+...a young, male figure[], <>
~ players_gender = "male"
-> Opening.CharCreation1
+...a young, female figure[], <>
~ players_gender = "female"
-> Opening.CharCreation1
+...a young, androgynous figure[], <>
~ players_gender = "nonbinary"
-> Opening.CharCreation1

= CharCreation1
{
	- players_gender == "male":
	    ~ androgynous = "masculine"
		~ they = "he"
        ~ them = "him"
        ~ their = "his"
        ~ theirs = "his"
        ~ Mx = "Mr"
        ~ master = "mister"
        ~ person = "man"
        ~ kid = "boy"
        ~ lad = "lad"
        ~ guy = "guy"        
        
    - players_gender == "female":
        ~ androgynous = "feminine"
        ~ they = "she"
        ~ them = "her"
        ~ their = "her"
        ~ theirs = "hers"
        ~ Mx = "Ms"
        ~ master = "missus"
        ~ person = "woman"
        ~ kid = "girl"
        ~ lad = "lass"
        ~ guy = "gal"
        
	- else:
	    ~ androgynous = "androgynous"
        ~ they = "they"
        ~ them = "them"
        ~ their = "their"
        ~ theirs = "theirs"
        ~ Mx = "Mx"
        ~ master = "master"
        ~ person = "person"
        ~ kid = "kid"
        ~ lad = "lad"
        ~ guy = "guy"
}

whose eyes shine a bright <>
+...blue[]. 
~ players_eyecolor = "Blue"
-> Opening.CharCreation2
+...green[].
~ players_eyecolor = "Green"
-> Opening.CharCreation2
+...brown[].
~ players_eyecolor = "Brown"
-> Opening.CharCreation2
+...grey[].
~ players_eyecolor = "Grey"
-> Opening.CharCreation2
+...hazel[].
~ players_eyecolor = "Hazel"
-> Opening.CharCreation2
+...amber[].
~ players_eyecolor = "Amber"
-> Opening.CharCreation2
+...red[].
~ players_eyecolor = "Red"
-> Opening.CharCreation2

= CharCreation2
{their} hair <>
+...flows down far beyond {their} shoulders<>
~ players_hair = "long"
-> Opening.CharCreation3
+...falls to about shoulder length<>
~ players_hair = "medium"
-> Opening.CharCreation3
+...gently covers the top part of {their} ears and neck<>
~ players_hair = "short"
-> Opening.CharCreation3
+...has been kept short<>
~ players_hair = "very_short"
-> Opening.CharCreation3
+...is shaven away completely<>
~ players_hair = "bald"
-> Opening.Mirror

= CharCreation3
{players_hair == "long": in splendid}{players_hair == "medium": in pretty}{players_hair == "short": with}{players_hair == "very_short": with}
+...black <>
~ players_hair_color = "black"
-> Opening.CharCreation4
+...brown <>
~ players_hair_color = "brown"
-> Opening.CharCreation4
+...auburn <>
~ players_hair_color = "auburn"
-> Opening.CharCreation4
+...red <>
~ players_hair_color = "red"
-> Opening.CharCreation4
+...blonde <>
~ players_hair_color = "blonde"
-> Opening.CharCreation4
+...white <>
~ players_hair_color = "white"
-> Opening.CharCreation4

= CharCreation4
{players_hair == "long":
+...straights[].
~ players_hair_style = "straight"
-> Opening.Mirror
+...wavy locks[].
~ players_hair_style = "wavy"
-> Opening.Mirror
+...curls[].
~ players_hair_style = "curly"
-> Opening.Mirror
}

{players_hair == "medium":
+...straights[].
~ players_hair_style = "straight"
-> Opening.Mirror
+...wavy locks[].
~ players_hair_style = "wavy"
-> Opening.Mirror
+...curls[].
~ players_hair_style = "curly"
-> Opening.Mirror
}

{players_hair == "short":
+...straight locks[].
-> Opening.Mirror
+...wavy locks[].
-> Opening.Mirror
+...curls[].
-> Opening.Mirror
}

{players_hair == "very_short":
+...slicked back locks[].
-> Opening.Mirror
+...curls[].
-> Opening.Mirror
+...tussled hair[].
-> Opening.Mirror
}
= Mirror
Yes, a young {person} with{players_eyecolor == "Blue": blue}{players_eyecolor == "Green": green}{players_eyecolor == "Brown": brown}{players_eyecolor == "Grey": grey}{players_eyecolor == "Hazel": hazel}{players_eyecolor == "Amber": amber}{players_eyecolor == "Red": red} eyes gently smiles at you, {their} {players_hair == "long":face framed by}{players_hair == "medium":face framed by}{players_hair == "short":head crowned by}{players_hair == "very_short":head adorned with short}{players_hair == "bald":head cleanly shaven}{players_hair_style == "straight": straight}{players_hair_style == "wavy": wavy}{players_hair_style == "curly": curly}{players_hair_color == "black": black}{players_hair_color == "brown": brown}{players_hair_color == "auburn": auburn}{players_hair_color == "red": red}{players_hair_color == "blonde": blonde}{players_hair_color == "white": white}{players_hair == "bald":. | hair.}
+And you recognise the face as yours[].
-> Opening.Recognition
+But you do not recognize yourself[].
You blink and a different face stares back at you.
->Opening.CharCreation0

= Recognition
You consider the name that belongs to this face. A young {person} called {Name}.
+ I've always thought it seemed to fit.
-> Opening.Acceptance
* It never seemed right somehow. <>
Too bad you can't change your name.
-> Opening.Acceptance


= Acceptance
+Disregarding your reflection, you move on.
-> Opening.Master

= Master
{Opening.MirrorY: As you lower the mirror in your hand}{Opening.MirrorN: As you leave the mirror on the floor} you hear a familiar voice call out to you. You turn to face it and find that the scenery has changed around you. The comforting smell of books remains but a small open window provides some fresh air. The office is just as you remember it: a small space that forms a stark contrast with the garden outside its window. Its floorboards are barely visible beneath the array of books, knick-knacks and tea cups that should have been returned to the kitchen days ago. You're sitting in a woodback chair. Across from you, behind a hardwooden desk, sits an elderly man. His kind eyes look into yours and you feel a bout of homesickness brewing in your stomach, although cannot fathom why. <br>
He hands you a large, leatherbound tome: your journal. Your most important possession. If you can fill its pages with knowledge not yet held within the Vault, or document where and how you found a tome not yet present on its shelves, you can become like the man in front of you: a keeper. A guaranteed lifetime within these halls, curating knowledge and educating the next generation. If you can't...<br>
You decide not to think about that. You give your master a <>
*...firm handshake <>
and walk out the door.
->Opening.Crossroads
*...warm smile <>
and walk out the door.
->Opening.Crossroads
*...determined nod <>
and walk out the door.
->Opening.Crossroads

=Crossroads
As you step through the door you feel your feet land firmly in the dirt. Before you a path winds gently down green hills.
*[Follow the path]
->Opening.Hills
*[Turn around]
You turn around but the door is gone. Instead, you see the road going on for several hundred yards before dissappearing into a thick woodland.
->Opening.Crossroads

=Crossroads2
*[Follow the path into the hills]
->Opening.Hills
*[Follow the path into the woodland]
->Opening.Woodland

=Hills
{Opening.Crossroads2: You leave the wood behind you and venture into the hills.|Taking a brave step forward you venture into the hills.} The verdant mounds roll by, the grass occassionally marked by a tree or brush. At some point, you notice a figure in the distance. Another lonely traveler on the road? Whoever it is, they'll soon be within shouting distance. 
*[Raise your arm and wave]
You wave happily at the approachinig figure, although they don't return the gesture. 
->Opening.Hills2
*[Walk towards them]
You decide to take the initiative and walk out to meet them on the road. 
->Opening.Hills2
*(Boulder)[Await them patiently]
As luck would have it there's a nice boulder by the roadside. You decide to take a break while the stranger approaches. 
->Opening.Hills2

=Hills2
As the stranger comes closer into view, you feel as if something is a bit off. Their clothing seems to consist out of rags, and their left leg is dragging behind them. Are they hurt? You
*...run towards them to help[]. <>
    You quickly close the gap, the stranger only being a several feet away from you now, allowing you to clearly see their face. <>
    ->Hills3
*...cautiously await their approach[].  <>
{Hills.Boulder:The boulder you're sitting on is actually rather comfortable, so why get up? Nevertheless, you |You }ready yourself, just in case they're hostile. It hasn't happened often on the road, but it's always good to be careful, right? 
    The stranger draws nearer, now being only several feet away from you, allowing a clear view of their face. <>
    ->Hills3
*(run)...decide you'd rather pass on this chance encounter and run in the other direction[]. <>
Breaking into a sprint, you head away from whoever is coming up the road. But as you run, you hear frantic footsteps behind you... And they're catching up. A powerful blow hits you in the back, sending you sprawling to the ground. You turn around to face your would be attacker, now in clear view.
->Hills3

=Hills3
You recoil. Its face is a horrible contradiction. Its right half takes the shape of a beautiful young man, with blemishless skin and perfect features. The left half however, betrays what lies underneath: twisted, rusting metal. It looks at you and smiles, the teeth on its left side stretching disconcerningly further than the remains of its lips on the right would allow. A word forms into your mind:
    
    Wraith. 
    
    Panic sets in. You
    **(fight)...fight[]. 
    With all your might you throw yourself at the horrid creature. As you collide with it, it feels as if you smack into a wall. You bounce of it and fall down to the ground{Hills2.run: once more}. <> 
    ->Hills4
    **(scream)...scream<>
    , but no sound comes from your lips. <>
    ->Hills4
    **(run)...run[].
    {Hills2.run:Perhaps against better judgement, you try to make another break for it. You have barely gotten to your feet before the creature knocks you down once more, a sharp pain shooting through your arm as you fall. Did you break something? |You turn and bolt away at full speed. The creature is much faster. A powerful blow hits you in the back, sending you sprawling to the ground. With what strength you have left, you turn to face it.} <> 
    ->Hills4
=Hills4
The wraith simply tilts its head at you, still wearing its grotesque smile, almost as if to mock your {Hills3.fight:bravery}{Hills3.run:speed}{Hills3.scream:terror}. Slowly, it starts to advance toward you. It raises its left arm, presumably to strike you with it. You shield yourself with your arms, although you know it will probably be for naught. You brace yourself, agonizing seconds ticking by as you wait for the end, and then-
        A roar. 
        A bestial growl reverberates through your body. You look up, just in time to see a bear collide forcefully with the creature. The beast tears into the wraith, somehow capable of tearing through metal as if it were cloth. Between the harsh sounds of ripping metal you hear the monster scream. Inhuman, horrible, but scream nonetheless. Then, silence. 
        The bear stands there, panting. Still catching his breath, he turns towards you. He slowly walks over, his mouth dripping with salliva. He sniffs you. His mouth, reeking with a nearly bloodlike metal smell, mere inches from your face. Without a sound, he opens his strong jaws and bites down on you-
            ->Awakening

=Woodland
The wood looks more appealing and you set off it its direction at a trot. 

-> END

=== Awakening ===
~TimeOfDay = Dawn
You awaken with a start. A dream after all. Of course it was, now that you look back on it. 
You turn on your back, the small canvas tent that shields you from the elements coming into view. You can smell the morning forest and the smouldering remains of your campfire.
->Awakening.Tent

=Tent
You decide to
*...pack up[].
*...make some breakfast first[]. 
*...sleep in[]. One way to shake a nightmare is with a new dream but sadly, sleep doesn't come.
->Tent
//Step by step, you climb the hill. A worn path guides your feet, a pleasant change of pace from the overgrown wood you found yourself in only two days ago.
//*[Put another foot forward]You take another step. And another. Your legs have carried you all the way from Barralon to here: the far North of the Forgotten Isles. 
  //  **[Readjust your backpack]You shift the weight of the pack on your shoulders. It's heavy, but that's a small burden to bear for the essentials you carry. No explorer worth their salt would set out without a tent, sleeping bag, flint and steel or a cooking pot. 
        
    //    Of course, your most important possession is not carried on your back. Chained to your belt is a large, leatherbound book: your journal. You give it a reaffirming tap. A record of all the things you've seen so far and, equally important, plenty of empty pages left to fill with discoveries of the North. 
        
      //  No keeper has set foot here in two hundred years, and even those long dead explorers never ventured deeply into the wilds. Without a doubt, you will be able to complete your task here; to find knowledge not yet stored in the Vault of Barralon. From local folklore to a survey of the wildlife, anything will do. But you did not venture all this way to write down the mundane. You came to find something old. Something ancient, from before the sundering. 
        
        //As you contemplate your quest, you realize your feet have carried you to the top of the hill. 
    //    ***[Survey the landscape]Beneath a pink morning sky fields of flowers roll out before you. The road winds down the hill, slowly making its descent before starting to climb again far in the distance. Its destination: a castle on a sturdy hill. From your vantage point, you can see the land flattening out beyond it, eventually meeting the inlet sea. 
        
      //      As your gaze returns to the path before you, you realize you missed something on your first viewing: a person. Still far in the distance, but

=== CastleEntrance ===
    -> CastleEntranceFirst

= CastleEntranceFirst
#backdrop: CastleGate
#music:
#ambiance:
As you crest the hilltop a gatehouse comes into view. Its stones are worn, ancient. The top parts seem to have crumbled at some point, having now been replaced by wooden battlements. As you step forward in approach you hear a voice call out loudly from inside the gatehouse:

"Halt! Who goes there, man or beast?"

*   "Man!" [] you shout back at him. 
    After a brief moment of quiet a reply comes from inside:
    "Alright {lad}, approach!"
    -> CastleEntranceFirstApproach
    
*   (joke)"Beast!" [] you shout back, mockingly.
    With a hint of anger, the voice replies:
    "Alright then you joker, come closer!"
    -> CastleEntranceFirstApproach    
    
*   [Stay quiet] A few seconds pass in silence. Then the voice rings out again, this time slightly quivering:
        "I won't ask again! Identify yourself!"
    **   [Identify yourself] "Just a traveler!" you shout.
        There's a slight pause and while you can't hear it through the thick walls, you can only imagine the guardsman let out a sigh of relief before replying
        "Don't scare me like that {lad}! Please, approach!"
        -> CastleEntranceFirstApproach
    **   [Remain quiet]
        You hear a bell being rung on the otherside of the wall. The next few moments pass by in a blurr as you hear several people shouting and running behind the wall. There's a low thud followed by a piercing pain. You look down at your chest and as you see the shaft of an arrow sticking out, you realize the gravity of your mistake. You try to raise your arms to somehow remedy the situation, but two more arrows find their mark. You fall to the ground, quickly shifting out of concsiousness and this mortal plane. 
-> Death

= CastleEntranceFirstApproach
You bring yourself nearer to the gatehouse. Two wooden doors are set beneath the archway, barring the way forward. As you stand before them a small latch is opened to reveal two {TimeOfDay == Dawn: tired blue eyes} crowned by a pair of bushy eyebrows{CastleEntranceFirst.joke: that are frowning in disaproval}.

{CastleEntranceFirst.joke: The man sighs, "Look {lad}, I appreciate your attempt at a joke -Gods know we could use some more humor out here- but on a bad day behaviour like that could get you killed. I've heard stories of wraiths speaking, wouldn't want to accidentally think you're one." | "Welcome to the castle {lad}, safest place in the North!" The man's eyes smile, most likely along with his mouth that's still hidden behind the door, "Sorry for all the precaution, can't be too careful with them wraiths out there."}
*"Could you let me in?"
    {CastleEntranceFirst.joke: "Alright, just behave would ye?" | "Of course {lad}, just a moment."}
    The man steps back and swings the latch shut. You hear the rustling of keys and the clunky rattling of locks, followed by a single door being opened inward. The man stands behind it holding the door open for you with one hand while leaning on a spear with the other. He's an old sort, nearing his fifties, but broadchested and with seemingly a strong arm. 
    
    As you step inside the man shuts the door behind you, taking great care to put the locks back into place. 
        **Engage the man in some further conversation
        -> EdgarGatehouse
        **(thanked)Thank the man and head into town
        ->EdCastleGatehouseWalkway
        **(ignored)Say nothing and keep walking
        ->EdCastleGatehouseWalkway
*"Wraiths?"
*"Do you often have to kill creatures gently walking up to the door?"

=== EdCastleGatehouseWalkway ===
//{EdCastleGatehouseWalkway: There's a small courtyard behind the gatehouse, behind which the path climbs steeply up the hill. {TimeOfDay == Night: The light of {MetEdgar: Edgar's | the guard's} lamp quickly fades as you make your way up the steps, and in the dark you nearly take a tumble.}{TimeOfDay == Dawn: }Once at the top, you are greeted by what appears to be a town square. {TimeOfDay == Night: It is empty now, but the various stalls suggest the place will be lively in a few hours.}{TimeOfDay == Dawn: Most townspeople are probably still asleep, but you already spot two men setting up what a market stall}{TimeOfDay == Morning: It's a small affair compared to other places you've been, but for this corner of the world it might as well be Grand Market of Barralon. On the opposite end of the square there are several houses{TimeOfDay == Night:, of which all the lights are dimmed}{TimeOfDay == Dawn: On the right}}}
->END
=== EdgarGatehouse ===

-> END

=== EdCastlePrison ===
VAR MaryUpset = false
{MaryPrisonGreeting: You find yourself in the damp prison room. {MetMary: Mary | A figure }is huddled against the far wall, a set of iron bars keeping you apart. | You descend the worn uneven steps cautiously. It's cooler here, and slightly damp. The thick stone walls appear to keep out most of the sun's warmth. You arrive in a broad, round room, a torch flickering on your left. The room is divided down the middle by a set of iron bars. Behind them, at the back of the room, you see a figure huddled against the wall. -> MaryPrisonGreeting}
{MetMary: -> MaryStateChecker | -> MaryPrisonGreeting}

= MaryStateChecker
{MaryUpset == true: -> MaryPrisonUpset}

= MaryPrisonGreeting
+[Greet them]
    **(neutral)"Hi there"
    -> MaryPrisonConvo
    **(kind)"Hi, are you alright?"
    -> MaryPrisonConvo
    **(joke)"The accomodations leave something to be desired, don't they?"
    -> MaryPrisonConvo
    **(rude)"What sort of wretch are you that they left you down here?"
    -> MaryPrisonConvo
    ++[On second thought, never mind] You decide not to speak to them. 
    ->EdCastlePrison
    
= MaryPrisonConvo
The figure perks up to look at you. Their hood falls back to reveal a young woman's face. Her long auburn hair falls down unkempt, perhaps due to the lack of a comb, but her bright hazel eyes look at you with intrigue.

{EdCastlePrison.MaryPrisonGreeting.neutral:"A visitor? And a new face too, what a pleasant surprise." She smiles at you.}{EdCastlePrison.MaryPrisonGreeting.kind:"I have had better days, but things could be worse I suppose." She smiles faintly. "...Thank you."}{EdCastlePrison.MaryPrisonGreeting.joke:A smile forms on her lips, "Yes, I should have the maid tidy up and bring a new set of pillows." She laughs sadly, "Oh, if only I could return to those halcyon days." She shakes her head as if to drive away her thoughts. }{EdCastlePrison.MaryPrisonGreeting.rude:"I see another one of the brutes has made it down here? Pray, tell me what it is you want so I can return to my misery." | "Pray tell, what brings you down to my part of the castle?"}
*"I was just exploring, to be honest."
{EdCastlePrison.MaryPrisonGreeting.rude: "Well go explore somewhere else then." The woman pulls her hood back over her head and turns away from you. | "Oh, is that so? You might want to be careful then, before they suspect you are trying to help me in one way or another."}
*"I heard about you in town... Are the stories true?"
Her expression grows solemn. "That depends" she says. "What did you hear?"
    **"That you're a monster, capable of killing with your bare hands"
    **"That you're a former queen that once ruled over the north"
    **"The stories conflict a bit, but all agree that you're dangerous"
*{EdCastlePrison.MaryPrisonGreeting.rude or EdCastlePrison.MaryPrisonGreeting.neutral}"To mock, what else would one do with a prisoner?"
{EdCastlePrison.MaryPrisonGreeting.neutral: Her face turns to anger. "I should have known, another one of the brutes. Do you lot have nothing better to do? Just leave me." | "Go find someone who cares, you lout." The woman pulls her hood back over her head and turns away from you.}
*"Do I need a reason?"
{EdCastlePrison.MaryPrisonGreeting.rude: "Well if you do not have any business here, leave me be. I have nothing to say to you." The woman pulls her hood back over her head and turns away from you. -> MaryPrisonUpset | "Mhm, I suppose not. Would you like to chat for a bit? It has been a while since anyone came down here that was willing to engage me in conversation. I am called Mary, by the way."}
    - (MaryNameLoop) 
    **(MetMary)"A pleasure to meet you Mary, I'm {Name}"
        She smiles warmly at you, "The pleasure is all mine,{Name}."
        ->MaryPrisonConvo2
    **"Simply Mary?"
        She smiles sadly, "These days yes, simply Mary. I do not feel quite worthy to use the full name I once bore." -> MaryNameLoop
= MaryPrisonConvo2
    **"Sure, any topic in particular?"
    **[Pick a topic yourself]
    ++"Actually, I have to get going"
    She looks crestfallen. "I see..." She gives you a sad smile. "Well, I will be here if you change your mind."
    ->EdCastlePrison
= MaryPrisonUpset
Test
-> END