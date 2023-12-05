INCLUDE CustomFeatures
INCLUDE Stories

// TODO: split the scenes in this file up into smaller files, each inclued in stories.


//AffectionValues
VAR AffEdgar = 50
VAR AffHenry = 50

//Knowledge Chains
LIST EdanCastleKnow = (none), Exists, IsCastleOnHill
LIST Edgar = (none), Exists, Name
LIST Henry = (none), Exists, Name

// starting inventory
~ Inventory = (knife, rope, lantern, foragedMushrooms)

-> Start 

=== Start ===
    -> Preamble
    
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END

=== RandomEventsEdanArea ===
//To do: add event content
{~!->MerchantBrothers|!->Deer|!->Downpour}

=MerchantBrothers
TestMerchant
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END
=Deer
TestDeer
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END
=Downpour
//Add companion dependent dialogue
As you're traveling, you start to notice dark clouds gathering overhead.
*Press on
    It's probably nothing. And even so, a little rain can't stop you, right?
    [Bas Note: Something happens with the rain I guess? But anyway I'm diverting to the next bit.]
    -> CastleEntrance
*Seek shelter
    You decide not to risk getting drenched and find some cover. Unfortunately, you don't 
{CurrentLocation == roadToEdanCastleLoc and PreviousLocation == EdinburghCrossroadsLoc: -> CastleEntrance}
If you're seeing this something went wrong with the random event bit in Inky!
->END

=== Awakening ===
~TimeOfDay = Dawn
~SetLocation(ScotlandEntranceRoadLoc)
You awaken with a start. A dream after all. Of course it was, now that you look back on it. 
You turn on your back, the small canvas tent that shields you from the elements coming into view. You can smell the morning forest and the smouldering remains of your campfire.
->Awakening.Tent

=Tent

You decide to
+  [{AllowMap()}] -> MapScreen(->Awakening.Tent) // dit moet toch meer consise kunnen...?

*(PackUpEarly)[...pack up]{aglue} pack up.
Or you would, but the grumbling of your stomach tells you that it's not going to be a fun hike without something to eat.

You can check your current hunger level on the right. As time passes, your need for food will increase. You wouldn't be the first adventurer to die of starvation, so keep an eye on it! [Vugs note: not yet implemented] 
->Tent
*[...make some breakfast first]{aglue} make some breakfast.
Your stomach rumbles, and what poor sort would head off without a proper meal first anyway? 
The campfire has yet to go out completely and should be easy to light. With the help of some kindling you gathered last night, it doesn't take you long to get a nice flame going.
The next step would be to hang your pot over the fire, but where did you leave the damn thing?

~Item_Add(pot)
You can find your belongings by clicking on the backpack icon on the right. You can then right click an item and select 'use' to put it into action.
    **[{ItemChoice(cooking)}]
    You set up the small iron stakes and hang the pot on it, placing it nice and snug over the fire. Now, to put some food in. 
        ***[{ItemChoice(food)}]
        {Item_Consume()}
        You drop the {UsedItem} into the pot, resulting in a satisfying sizzle. Good thing master Pedrál went through that herbology phase last semester, or you would have left them by the wayside in fear of poison.
        A few minutes of stirring and a sprinkle of salt later, your woodland meal is ready to eat. It's not something you'd serve to a king or worse, a mother-in-law, but your stomach is grateful for it nevertheless. 
        {Tent.PackUpEarly: |You can check your current hunger level on the right. As time passes, your need for food will increase. You wouldn't be the first adventurer to die of starvation, so keep an eye on it! [Vugs note: not yet implemented]}
        Fully fed, it's about time to head off if you still want to make some decent progress today. 
        While outside, you can get a general sense of what time it is by looking at the time indicator in the top right. Most actions and events will cause time to progress. The world will differ depending on the day-night cycle, but do not worry on missing out. A Keeper's quest is as much -if not more so- about the journey than it is about the destination. And if Fate wants certain people to meet, she always finds a way. [Vugs note: not yet implemented, time is currently tracked in Inky but needs a coding addition] 
            ~TimeOfDay = Morning
            ****[Pack up]
            You gather your belongings and make sure the fire is thorougly smothered by a heap of sand. With a few steps you move from the small clearing where you made your camp back to the road. Looking north, it slopes gently upward.
            ->Awakening.PackUp
            
*...sleep in[]. One way to shake a nightmare is with a new dream but sadly, sleep doesn't come.
->Tent

=PackUp
*So you take the road North.
->ScotlandEntranceRoad
+But you decide to head south.
{Southbound is the way you came. The goal of your journey is the other way.|You came from there only yesterday. It would be such a waste to turn back now.|Your master would be sorely dissappointed if you came back without accomplishing what you set out to do.|Because somehow, you feel compelled to do so. You feel like if you take one step in that direction, you cannot help but walk all the way back home.|And end your northern adventures right here, only to begin the long journey home.->END} ->Awakening.PackUp


===ScotlandEntranceRoad===
~SetLocation(ScotlandEntranceRoadLoc)
{PreviousLocation ? EdinburghCrossroadsLoc: Ah, a return visit! Too bad this isn't implemented yet.->ScotlandEntranceRoad.ReturnVisit|->ScotlandEntranceRoad.FirstVisit}
=ReturnVisit
*[Go back the way you came]

~PreviousLocation = ScotlandEntranceRoadLoc
->EdinburghCrossroads
=FirstVisit
Step by step, you climb the hill. A worn path guides your feet, a pleasant change of pace from the overgrown woodland you found yourself in only two days ago.
*[Put another foot forward]You take another step. And another. Your legs have carried you all the way from Barralon to here: the far North of the Forgotten Isles. 
    **[Readjust your backpack]You shift the weight of the pack on your shoulders. It's heavy, but that's a small burden to bear for the essentials you carry.
        
        Of course, your most important possession is not carried on your back. Chained to your belt is a large, leatherbound book: your journal. You give it a reaffirming tap. A record of all the things you've seen so far and, equally important, plenty of empty pages left to fill with discoveries of the North. 
        
        You can check the contents of your journal by clicking its corresponding icon, on the right. [Vugs note: not yet implemented]
        
        No keeper has set foot here in two hundred years, and even those long dead explorers never ventured deeply into the wilds. Without a doubt, you will be able to complete your task here; to find knowledge not yet stored in the Vault of Barralon. From local folklore to a survey of the wildlife, anything will do. But you did not venture all this way to write down the mundane. You came to find something old. Something ancient, from before the sundering. 
        
        As you contemplate your quest, you realize your feet have carried you to the top of the hill. 
        ***[Survey the landscape]Beneath a bright morning sky fields of flowers roll out before you. The road winds down the hill, slowly making its descent before starting to climb again far in the distance. Its destination: a castle on a rocky outcrop. From your vantage point, you can see the land flattening out beyond it, eventually meeting the inlet sea. 
            ****[Continue following the road towards the castle]
            You continue your journey, the earth crunching beneath your feet. Almost at the halfway point between your hilltop outlook and the castle you find yourself at a crossroads. 
            ~ PreviousLocation = ScotlandEntranceRoadLoc
            ->EdinburghCrossroads
        
            //As your gaze returns to the path before you, you realize you missed something on your first viewing: a person. Still far in the distance, but unmistakenbly a fellow traveler. They seem to be approaching at a fair pace, aided by a walking stick.
           // ****[Continue following the road]
            //****[Wait for the traveler]
            //****[Turn back]
            //****[Set up an ambush]
            
=== EdinburghCrossroads ===
~ SetLocation(EdinburghCrossroadsLoc)
The road splits here into four directions. The northbound road {!HasVisited(EdanCastle) and Knows(edanca.Exists): presumably |}leads to {Knows(EdanCastleKnow.IsCastleOnHill):Edan Castle|the castle on the hill}{PreviousLocation ? roadToEdanCastleLoc:, from which you came|.} The road South would carry you away from the Northern Lands, perhaps even all the way back home{PreviousLocation ? ScotlandEntranceRoadLoc:, but you just came from there.|.} You're unsure where the roads leading East and West would take you.
At the center of the crossing you spot a decorated boulder: a Waystone.
->EdinburghCrossroads.Crossing
=Crossing
+[Take the North Road]
You decide to take the North road{PreviousLocation == EdinburghCastleEntrance: and go and go back the way you came.|  leading to {Knows(EdanCastleKnow.Exists):Edan Castle |the Hilltop Castle.}}
->RoadToEdanCastle
+[Take the East Road]
Sorry buddy, no content East yet!
->EdinburghCrossroads.Crossing
+[Take the South Road]
You decide to {PreviousLocation ? ScotlandEntranceRoad:go back the way you came.|take the Southern Road.}
->ScotlandEntranceRoad
+[Take the West Road]
Sorry buddy, no content West yet!
->EdinburghCrossroads.Crossing
+[Inspect the Waystone]

~ Learn(EdanCastleKnow.Exists)
{You decide to take a closer look at the Waystone in the middle of the crossing. It's decorated in a blocky script, which thankfully matches the sources you were able to study back in Barralon. In the Northern Tongue it reads:|You decide to take another look at the Waystone. It reads:}
"May the blessings of Crìsdaen be upon the honorable traveler

From this stone to Edan Castle, 7 miles Northbound
From this stone to the Sea, 8 miles Eastbound
From this stone to Thahnford, 107 miles Southbound"

A fourth line is also there, but the markings are scratched out. Carved beneath it in a freeform script -that barely passes as legible- is written a single word: "daemons".
->EdinburghCrossroads.Crossing
* [Consult your map]
    You can open your map by clicking on the relevant tab above!
    \[This is where the player learns about the map, and that they can also always open it with in the menu. But, currently, using this option caused a softlock! So sorry buddy, your game is softlocked now. You won't be able to go advance the story after the map closes. I'm working on it, al right?
    {stop}
    (Hope the quickload functionality works at the moment...)
    {OpenMap()}
    ->MapScreen (-> EdinburghCrossroads.Crossing)

+  [{AllowMap()}] -> MapScreen(-> EdinburghCrossroads.Crossing) // dit moet toch meer consise kunnen...?

=== RoadToEdanCastle
//Add first time content, repeated content and randomizer element
~SetLocation(roadToEdanCastleLoc)
{~->RandomEventsEdanArea|->CastleEntranceFromRoad}

=== CastleEntranceFromRoad
// removed manual edit of previouslocation, als het goed is wordt dit nu bijgehouden door setlocation. ~PreviousLocation = RoadToEdanCastle
->CastleEntrance

=== CastleEntrance ===
{HasVisited(EdanCastleEntrance): -> CastleEntranceReturnVisit|-> CastleEntranceFirst}

= CastleEntranceFirst
~SetLocation(EdanCastleEntrance)
~FadeToImage(CastleGate,0)
//~Music_Play([nameOfClip])
//~Ambiance_Add([name
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
    ~SetLocation(EdanCastleGatehouse)
        **Engage the man in some further conversation
        -> EdgarGatehouse
        **(thanked)Thank the man and head into town
        ->CastleGatehouseWalkway
        **(ignored)Say nothing and keep walking
        ->CastleGatehouseWalkway
*"Wraiths?"
*"Do you often have to kill creatures gently walking up to the door?"

= CastleEntranceReturnVisit
~SetLocation(EdanCastleEntrance)
As you {once again crest the|crest the increasingly familiar|crest the well known} hill, the Edani Gatehouse comes into view. {TimeOfDay == Night:It's hard to make out in de dark, {!Inventory has lantern:but knowing it's there helps guide your feet.}{Inventory has lantern:but your lantern illuminates your surroundings enough to find your way.} ->CastleEntranceReturnVisitNight}{TimeOfDay == Dawn:The morning sun casts a gentle yellow hue on the building.}{TimeOfDay == Dusk: A pair of torches has already been lit, despite the setting sun still providing ample lighting.}{TimeOfDay == Evening:Two torches placed on either side of the gate illuminate it with a flickering orange light.} The gate's ironbound doors are open, welcoming visitors. In front of them, you spot {Knows(Henry.Name):Henry|a guard} leaning on his halberd. 
//{AffHenry < 25: } (to do: make scenario where Henry stops you)
He looks {LIST_COUNT(Party)==1: you|your party} over and smiles. With his left hand, he gestures that you may pass into the settlement.
+[Continue on]
+[Talk to {Knows(Henry.Name):Henry|the guard}.]
+[Go back]
~PreviousLocation = "EdanCastleEntrance"
->RoadToEdanCastle

= CastleEntranceReturnVisitNight
{Inventory has lantern:As you approach, you hear someone shouting from behind the door. -> CastleEntranceReturnVisitNightNoLantern}
{Inventory has lantern:As you approach, you hear a man's voice ring out from behind the battlements:}
"Hail traveler{LIST_COUNT(Party):s}, what's your business in Edani at this hour?"
*{Knows(Edgar.Name)}"It's {LIST_COUNT(Party):us|me} Edgar, {PlayerName}{Party has Alice and Party !? Robert: and Alice}{Party !? Alice and Party has Robert: and Robert}, {Party has Alice and Party has Robert:Alice and Robert}."
    {AffEdgar < 25:"{PlayerName} ey? Don't think I've heard that name before, but sounds like the name of a twat! Try coming back in the morning, maybe Henry will let you in."->CastleEntranceReturnVisitNightLocked}
    {AffEdgar < 50 and AffEdgar > 24:Oh, {PlayerName}. Behaving yourself at this hour I hope? Well no matter, come on in, it's no time to be outside. ->CastleGatehouseWalkway} 
    {AffEdgar > 49:Ah, {PlayerName}! What are you{LIST_COUNT(Party)>1: all} doing outside at this hour? Ah no matter, let me open up the gate for you!" ->CastleGatehouseWalkway}
*"{LIST_COUNT(Party)>1:We're|I'm} simply looking for some shelter in the night." 
*[Jokingly say:]"Why, to rob you blind of course! 
*[Sternly say: ]"Open the gate, 

=CastleEntranceReturnVisitNightNoLantern
This bit is still being developed! Head on over to behind the gate. 
->CastleGatehouseWalkway
=CastleEntranceReturnVisitNightLocked
You find yourself locked out of Edani.
*[Leave]
You decide to leave and take the path back down the hill.
~PreviousLocation = CastleEntrance
->RoadToEdanCastle
*[Knock on the gate]
*[Wait]

=== CastleGatehouseWalkway ===
//{CastleGatehouseWalkway: There's a small courtyard behind the gatehouse, behind which the path climbs steeply up the hill. {TimeOfDay == Night: The light of {MetEdgar:Edgar's| the guard's} lamp quickly fades as you make your way up the steps, {ItemLantern == 0:and in the dark you nearly take a tumble.}{ItemLantern == 1: but you have your own light to guide you.}{TimeOfDay == Dawn: } Once at the top, you are greeted by what appears to be a town square. {TimeOfDay == Night: It is empty now, but the various stalls suggest the place will be lively in a few hours.}{TimeOfDay == Dawn: Most townspeople are probably still asleep, but you already spot two men setting up what appears to be a market stall}{TimeOfDay == Morning: A few stalls are set up, with merchants plying their wares. It's a small affair compared to other places you've been, but for this corner of the world it might as well be Grand Market of Barralon. On the opposite end of the square there are several stonework houses{TimeOfDay == Night:, of which all the lights are out}{TimeOfDay == Dawn: On the right}}}
->END
=== EdgarGatehouse ===

-> END

=== CastlePrison ===
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
    **"That you're a former queen that once ruled over the north"
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
    **"Sure, any topic in particular?"
    **[Pick a topic yourself]
    ++"Actually, I have to get going"
    She looks crestfallen. "I see..." She gives you a sad smile. "Well, I will be here if you change your mind."
    ->CastlePrison
= MaryPrisonUpset
Test
-> END