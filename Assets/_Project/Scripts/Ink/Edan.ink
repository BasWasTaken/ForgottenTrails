// --------- Vugs  ---------
=== CastleEntrance ===
{HasVisited(LOC_EdanCastleEntrance): -> CastleEntranceReturnVisit|-> CastleEntranceFirst}

= CastleEntranceFirst
~SetLocation(LOC_EdanCastleEntrance)
~FadeToImage(BG_CastleGate,0)
//~Music_Play([nameOfClip])
//~Ambiance_Add([name
As you crest the hilltop a gatehouse comes into view. Its stones are worn, ancient. The top parts seem to have crumbled at some point, having now been replaced by wooden battlements. As you approach you hear a voice call out loudly from inside the gatehouse:

"Halt! Who goes there, man or beast?"

*   "Man!" [] you shout back at him. 
    After a brief moment a reply comes from inside:
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
        You hear a bell being rung on the other side of the wall. The next few moments pass by in a blurr as you hear several people shouting and running. There's a low thud followed by a piercing pain. You look down at your chest and as you see the shaft of an arrow sticking out, you realize the gravity of your mistake. You try to raise your arms to somehow remedy the situation, but two more arrows find their mark. You fall to the ground, quickly drifting out of concsiousness and this mortal plane. 
-> Death

= CastleEntranceFirstApproach
You bring yourself nearer to the gatehouse. Two wooden doors are set beneath the archway, barring the way forward. As you stand before them a small latch is opened to reveal two {TimeOfDay == Dawn: tired blue eyes} crowned by a pair of bushy eyebrows{CastleEntranceFirst.joke: that are frowning in disaproval}.

{CastleEntranceFirst.joke: The man sighs, "Look {lad}, I appreciate your attempt at a joke -Gods know we could use some more humor out here- but on a bad day behaviour like that could get you killed. I've heard stories of wraiths speaking, wouldn't want to accidentally think you're one." | "Welcome to the castle {lad}, safest place in the North!" The man's eyes smile, most likely along with his mouth that's still hidden behind the door, "Sorry for all the precaution, can't be too careful with them wraiths out there."}
*"Could you let me in?"
    {CastleEntranceFirst.joke: "Alright, just behave would ye?" | "Of course {lad}, just a moment."}
    The man steps back and swings the latch shut. You hear the rustling of keys and the clunky rattling of locks, followed by a single door being opened inward. The man holds it open for you with one hand while leaning on a spear with the other. He's an old sort, nearing his fifties, but broadchested and with seemingly a strong arm. 
    
    As you step inside the man shuts the door behind you, taking great care to put the locks back into place. 
    ~SetLocation(LOC_EdanCastleGatehouse)
        **Engage the man in some further conversation
        -> Guardhouse
        **(thanked)Thank the man and head into town
        ->CastleGatehouseCourtyard
        **(ignored)Say nothing and keep walking
        ->CastleGatehouseCourtyard
*"Wraiths?"
*"Do you often have to kill creatures gently walking up to the door?"

= CastleEntranceReturnVisit
~SetLocation(LOC_EdanCastleEntrance)
As you {once again crest the|crest the increasingly familiar|crest the well known} hill, the Edani Gatehouse comes into view. {TimeOfDay == Night:It's hard to make out in de dark, {!Inventory has Lantern:but knowing it's there helps guide your feet.}{Inventory has Lantern:but your lantern illuminates your surroundings enough to find your way.} ->CastleEntranceReturnVisitNight}{TimeOfDay == Dawn:The morning sun casts a gentle yellow hue on the building.}{TimeOfDay == Dusk: A pair of torches has already been lit, despite the setting sun still providing ample lighting.}{TimeOfDay == Evening:Two torches placed on either side of the gate illuminate it with a flickering orange light.} The gate's ironbound doors are open, welcoming visitors. In front of them, you spot {Knows(Henry.Name):Henry|a guard} leaning on his halberd. 
//{AffHenry < 25: } (to do: make scenario where Henry stops you)
He looks {LIST_COUNT(Party)==1: you|your party} over and smiles. With his left hand, he gestures that you may pass into the settlement.
+[Continue on]
+[Talk to {Knows(Henry.Name):Henry|the guard}.]
+[Go back]
~PreviousLocation = "EdanCastleEntrance"
->RoadToEdanCastle

= CastleEntranceReturnVisitNight
{Inventory has Lantern:As you approach, you hear someone shouting from behind the door. -> CastleEntranceReturnVisitNightNoLantern}
{Inventory has Lantern:As you approach, you hear a man's voice ring out from behind the battlements:}
"Hail traveler{LIST_COUNT(Party):s}, what's your business in Edan at this hour?"
*{Knows(Edgar.Name)}"It's {LIST_COUNT(Party):us|me} Edgar, {PlayerName}{Party has Alice and Party !? Robert: and Alice}{Party !? Alice and Party has Robert: and Robert}, {Party has Alice and Party has Robert:along with Alice and Robert}."
    {AffEdgar < 25:"{PlayerName} ey? Don't think I've heard that name before, but sounds like the name of a twat! Try coming back in the morning, maybe Henry will let you in."->CastleEntranceReturnVisitNightLocked}
    {AffEdgar < 50 and AffEdgar > 24:Oh, {PlayerName}. Behaving yourself at this hour I hope? Well no matter, come on in, it's no time to be outside. ->CastleGatehouseCourtyard} 
    {AffEdgar > 49:Ah, {PlayerName}! What are you{LIST_COUNT(Party)>1: all} doing outside at this hour? Ah no matter, let me open up the gate for you!" ->CastleGatehouseCourtyard}
*"{LIST_COUNT(Party)>1:We're|I'm} simply looking for some shelter in the night." 
*[Jokingly say:]"Why, to rob you blind of course! 
*[Sternly say: ]"Open the gate, 

=CastleEntranceReturnVisitNightNoLantern
This bit is still being developed! Head on over to behind the gate. 
->CastleGatehouseCourtyard
=CastleEntranceReturnVisitNightLocked
You find yourself locked out of Edani.
*[Leave]
You decide to leave and take the path back down the hill.
~PreviousLocation = CastleEntrance
->RoadToEdanCastle
*[Knock on the gate]
*[Wait]

=== CastleGatehouseCourtyard ===
~ Time_AdvanceUntil(Dawn)
//Vugs: I'm 100% certain I'm using the wrong syntax for the line below, but unsure how to fix. @Bas
//Bas @Vugs: Not sure if you're talking about the TimeOfDay syntax, or the ink if-statements. 
// I don't think you can say "variable == value1 or value2 or value3", you'd have to say "variable == value1 or variable == value2 or variable == value3".But, lists assume an additive structure, meaning you can instead use "greater than", "smaller than", etc. (Here we consider the day to start at dawn and end at night.)
// Ik heb die hieronder toegepast. Treft dit wat je bedoelde, of had je het ergens anders over?
//  I've also changed the above line to use a new function for passing time of day until a specific point, instead of setting it manually. The reason for this is that this function also ticks 1 day in the variable that tracks how many in-game days have passed since the start of the story. 
// I don't see anything else wrong with the broader if-statement at first glance but could be missing something.
{On the other side of the gate you find yourself standing in a small courtyard. Directly across from you the road continues at a steady incline and further into the settlement. The castle town's battlements stand firm to your left and right{TimeOfDay >= Dusk:, illuminated by flickering torchlight.|. You see one or two other{TimeOfDay == Dawn: early} travelers passing through.}| You find yourself in the courtyard next to the entrance gate. {TimeOfDay >= Dusk: The torchlight from the battlements casts a pleasant glow.}}
+[Follow the road into town]
//{TimeOfDay == Night: The light of {MetEdgar:Edgar's| the guard's} lamp quickly fades as you make your way up the steps, {Inventory!?Lantern:and in the dark you nearly take a tumble.}{Inventory?Lantern: but you have your own light to guide you.}}
//Vugs: this functionality is gone I think in the new item system. Any way to passively use items? @Bas
//Bas@Vugs: You can check wether the player has an item in inventory by checking "Inventory?ItemLantern" or "Inventory has ItemLantern". This could be used for passive/automatic item useage. Applied above.
// This does not remove the item from inventory or use any Unity inventory UI. Which, if I understand you correctly, I believe is what you want in this case.
// If you do want to remove the item from inventory manually, you can call "Item_Remove(item)". 
// There is currently not a way to "use" an item via ink without breaking it, and I don't think we miss it at the moment? But if you do need that as some point (e.g. to fascilitate a durability system), that is something I could implement. 
->EdanTownSquare
+[Approach the guardhouse]
-> Guardhouse
+{TimeOfDay == Dawn}Talk to {Knows(Edgar.Name):Edgar|the guard}.
->EdgarGateConvo
+{TimeOfDay == Morning or Midday or Afternoon or Dusk}Talk to {Knows(Henry.Name):Henry|the guard}.
->HenryGateConvo
+[Leave through the gate]


->END

= EdgarGateConvo
->DONE
= HenryGateConvo
->DONE
=== Guardhouse ===
->DONE

=== EdanTownSquare ===
 You are quickly greeted by what appears to be the town square. {TimeOfDay == Night: It is empty now, but the various stalls suggest the place will be lively in a few hours.}{TimeOfDay == Dawn: Most townspeople are probably still asleep, but you already spot two men setting up what appears to be a market stall}{TimeOfDay == Morning or Midday or Afternoon: A few stalls are set up, with merchants plying their wares. It's a small affair compared to other places you've been, but for this corner of the world it might as well be Grand Market of Barralon.}


-> END

=== CastlePrison ===
//Vugs: Pretty old piece of text and unfinished. Probably needs a proper rewrite. 
~ CurrentLocation = "EdinburghCastlePrison"
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
    ->CastlePrison
    
= MaryPrisonConvo
The figure perks up to look at you. Their hood falls back to reveal a young woman's face. Her long auburn hair falls down unkempt, perhaps due to the lack of a comb, but her bright hazel eyes look at you with intrigue.

{CastlePrison.MaryPrisonGreeting.neutral:"A visitor? And a new face too, what a pleasant surprise." She smiles at you.}{CastlePrison.MaryPrisonGreeting.kind:"I have had better days, but things could be worse I suppose." She smiles faintly. "...Thank you."}{CastlePrison.MaryPrisonGreeting.joke:A smile forms on her lips, "Yes, I should have the maid tidy up and bring a new set of pillows." She laughs sadly, "Oh, if only I could return to those halcyon days." She shakes her head as if to drive away her thoughts. }{CastlePrison.MaryPrisonGreeting.rude:"I see another one of the brutes has made it down here? Pray, tell me what it is you want so I can return to my misery." | "Pray tell, what brings you down to my part of the castle?"}
*"I was just exploring, to be honest."
{CastlePrison.MaryPrisonGreeting.rude: "Well go explore somewhere else then." The woman pulls her hood back over her head and turns away from you. | "Oh, is that so? You might want to be careful then, before they suspect you are trying to help me in one way or another."}
*"I heard about you in town... Are the stories true?"
Her expression grows solemn. "That depends" she says. "What did you hear?"
    **"That you're a monster, capable of killing with your bare hands"
    **"That you're a former queen that once ruled these lands"
    **"The stories conflict a bit, but all agree that you're dangerous"
*{CastlePrison.MaryPrisonGreeting.rude or CastlePrison.MaryPrisonGreeting.neutral}"To mock, what else would one do with a prisoner?"
{CastlePrison.MaryPrisonGreeting.neutral: Her face turns to anger. "I should have known, another one of the brutes. Do you lot have nothing better to do? Just leave me." | "Go find someone who cares, you lout." The woman pulls her hood back over her head and turns away from you.}
*"Do I need a reason?"
{CastlePrison.MaryPrisonGreeting.rude: "Well if you do not have any business here, leave me be. I have nothing to say to you." The woman pulls her hood back over her head and turns away from you. -> MaryPrisonUpset | "Mhm, I suppose not. Would you like to chat for a bit? It has been a while since anyone came down here that was willing to engage me in conversation. I am called Mary, by the way."}
    - (MaryNameLoop) 
    **(MetMary)"A pleasure to meet you Mary, I'm {PlayerName}"
        She smiles warmly at you, "The pleasure is all mine, {PlayerName}."
        ->MaryPrisonConvo2
    **"Simply Mary?"
        She smiles sadly, "These days yes, simply Mary. I do not feel quite worthy to use the full name I once bore." -> MaryNameLoop
= MaryPrisonConvo2
    **"Did you have a topic in mind?"
    **[Pick a topic yourself]
    ++"Actually, I have to get going"
    She looks crestfallen. "I see..." She gives you a sad smile. "Well, I will be here if you change your mind."
    ->CastlePrison
= MaryPrisonUpset
Test
-> END