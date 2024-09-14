// --------- Vugs  ---------
=== CastleEntrance ===
~SetLocation(LOC_EdanCastleEntrance)
{once:-> CastleEntranceFirst}
->CastleEntranceOutsideApproach

=CastleEntranceOutsideApproach
As you {once again crest the|crest the increasingly familiar|crest the well known} hill, the Edani Gatehouse comes into view. {TimeOfDay == Night:It's hard to make out in the dark, {Inventory has Lantern:but your lantern illuminates your surroundings enough to find your way.|but knowing it's there helps guide your feet.} ->CastleEntranceReturnVisitNight}{TimeOfDay == Dawn:The morning sun casts a gentle yellow hue on the building.}{TimeOfDay == Dusk: A pair of torches has already been lit, despite the setting sun still providing ample lighting.}{TimeOfDay == Evening:Two torches placed on either side of the gate illuminate it with a flickering orange light.} The gate's ironbound doors are open, welcoming visitors. In front of them, you spot {knows(HenryKnowState.Name):Henry|a guard} leaning on his halberd. 
//{AffHenry < 25: } (to do: make scenario where Henry stops you)
He looks {LIST_COUNT(Party)==1:you|your party} over and gives a gentle smile. With his left hand, he gestures that you may pass into the settlement.
+[Continue on]
->CastleGatehouseCourtyard
+[Talk to {knows(HenryKnowState.Name):Henry|the guard}.]
->HenryGateConversation
+[Head back]
~PreviousLocation = "EdanCastleEntrance"
->RoadToEdanCastle

=HenryGateConversation
+[Head into town]
->CastleGatehouseCourtyard
+[Leave town]
~PreviousLocation = "EdanCastleEntrance"
->RoadToEdanCastle

= CastleEntranceReturnVisitNight
{Inventory has Lantern:As you approach, you hear someone shouting from behind the door. -> CastleEntranceReturnVisitNightNoLantern}
{Inventory has Lantern:As you approach, you hear a man's voice ring out from behind the battlements:}
"Hail traveler{LIST_COUNT(Party):s}, what's your business in Edan at this hour?"
+{knows(EdgarKnowState.Name)}"It's {LIST_COUNT(Party):us|me} Edgar, {PlayerName}{Party has Alice and Party !? Robert: and Alice}{Party !? Alice and Party has Robert: and Robert}, {Party has Alice and Party has Robert:along with Alice and Robert}."
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
+[Leave]
You decide to leave and take the path back down the hill.
~PreviousLocation = CastleEntrance
->RoadToEdanCastle
+[Knock on the gate]
+[Wait until dawn]

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
    ~AffEdgar -= 5
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

{CastleEntranceFirst.joke: The man sighs, "Look {lad}, I appreciate your attempt at a joke -Gods know we could use some more humor out here- but on a bad day behaviour like that could get you killed. We've had a run in with a wraith only yesterday, hence the closed door policy." | "Welcome to the castle {lad}, safest place in the North!" The man's eyes smile, most likely along with his mouth that's still hidden behind the door, "Sorry for all the precaution, we've had a run in with a wraith only yesterday. Have to make sure there aren't any stragglers!"}
->CastleEntranceFirstDialogue

= CastleEntranceFirstDialogue
*"Could you let me in?"
    {CastleEntranceFirst.joke: "Alright, just behave would ye?" | "Of course {lad}, just a moment."}
    The man steps back and swings the latch shut. You hear the rustling of keys and the clunky rattling of locks, followed by a single door being opened inward. The man holds it open for you with one hand while leaning on a halberd with the other. He's an old sort, nearing his fifties, but broadchested and with seemingly a strong arm. 
    
    As you step inside the man shuts the door behind you, taking great care to put the locks back into place. 
    ~SetLocation(LOC_EdanCastleGatehouse)
        **"I have a few more questions, if you don't mind."
        {CastleEntranceFirst.joke:The man grunts. Alright then, but let's head up to the guardhouse so I can keep an eye on the road while you badger me with your questions.|"Sure thing {lad}, but let's head up to the guard house. That way I can keep an eye on the road."}
            The man starts walking up a set of stairs, set to the left of the gate, and beckons you to follow. They lead up to the small guardhouse that sits atop the gate. 
            -> Guardhouse
        **(thanked)Thank the man and head into town
        ~AffEdgar += 5
        ->CastleGatehouseCourtyard
        **(ignored)Say nothing and keep walking
        ~AffEdgar -= 10
        ->CastleGatehouseCourtyard
*(AskedAboutWraith)"A wraith?"
"Aye, a wraith." he says. When a short pause lets him know you don't know what that means his eyes widen. "You don't know about wraiths? {lad}, have you been living under a rock? Those monsters wearing human skin! Must have been the topic of half my wetnurse's lullabies. 
    **"Ah, of course. Sorry, we call them something else where I'm from." [Lie]
    The man simply nods. 
    "Anyway..."
    ->CastleEntranceFirstDialogue
    **[Just nod and change the topic.] 
    "Anyway..."
    ->CastleEntranceFirstDialogue 
*(AskedAboutIncident)"You had an incident?"
The man nods. "Aye, {TimeOfDay == Evening or Night or Dawn:just yesterday.|just earlier today.} Damn bugger came meandering down the road. Gate was open then, as it usualy is during the daytime. Poor Freddy was standing guard. Always was too kindly a fellow, let that bloody monster come way too close before he realised what was up. Tore sodding half his arm off it did! I think you can still see some of the bloodstains in the dirt."
    - (top)
    **["Is Freddy alright?]
    The man shakes his head. "I don't know {lad}. They took him up to Bartholomew, but I haven't heard anything since. Old man's seen enough by now to know what to do with him, but Gods that was a grizzly wound.
    -> top
    **[Look for the stains]
    You glance around, looking for the bloodstains he mentioned. {TimeOfDay == Evening or Night:It's hard to make out in the dark{LightSource == 1: but with the help of your {LanternState == 1:lantern}{TorchState == 1:torch} you spot what the man was refering to. Several feet to the left of the closed gate, the ground is stained a deep, dark red. Judging by the size of it, the man must have lost a lot of blood.|, and without a light you seem unable to find what he's refering to.}|It doesn't take you long to see that several feet to the left of the gate, the ground is stained a deep, dark red. Judging by the size of it, the man must have lost a lot of blood.}
    ->top
    **[Ask about something else]
    ->CastleEntranceFirstDialogue

=== CastleGatehouseCourtyard ===
{On the other side of the gate you find yourself standing in a small courtyard. Directly across from you the road continues at a steady incline and further into the settlement. The castle town's battlements stand firm to your left and right{TimeOfDay >= Dusk and TimeOfDay <= Night:, illuminated by flickering torchlight.|. You see one or two other{TimeOfDay == Dawn: early} travelers passing through.}| You find yourself in the courtyard next to the entrance gate. {TimeOfDay >= Dusk and TimeOfDay <= Night: The torchlight from the battlements casts a pleasant glow.}}
+[Follow the road into town]
//{TimeOfDay == Night: The light of {MetEdgar:Edgar's| the guard's} lamp quickly fades as you make your way up the steps, {Inventory!?Lantern:and in the dark you nearly take a tumble.}{Inventory?Lantern: but you have your own light to guide you.}}
//Vugs: fix dit met een variable voor light source aan/uit
->EdanTownSquare
+[Approach the guardhouse]
-> Guardhouse
+{TimeOfDay == Dawn}Talk to {knows(EdgarKnowState.Name):Edgar|the guard}.
->EdgarGateConvo
+{TimeOfDay == Morning or Midday or Afternoon or Dusk}Talk to {knows(HenryKnowState.Name):Henry|the guard}.
->HenryGateConvo
+[Leave through the gate]


->END

= EdgarGateConvo
->DONE
= HenryGateConvo
->DONE

=== Guardhouse ===
->DONE



