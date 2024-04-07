// --------- Bas  ---------
// Utility
VAR TimeSpent = 0
=== function SpendTime(minutes)
    ~ TimeSpent += minutes
    
=== CheckTime(->ret)
    ~ Print("It is now roughly {ReadTime(TimeSpent)}. You have spent {TimeSpent} minutes during your heist.")
    {
    - TimeSpent > 360:
        -> Time
    - TimeSpent > 60:
        WIP
    - TURNS_SINCE(->Sirens)>5 && !Police_Raid && TimeSpent > RANDOM(10,25):
        -> Police_Raid ->
    - !Sirens && TimeSpent > RANDOM(7,15):
        -> Sirens ->
    }
=== function ReadTime(minutes)
    ~ return "{TimeSpent/60} o\' clock"

VAR Alfons = "mobile"
VAR Bernard = "mobile"
VAR Charles = "asleep"

=== tunnel_as_thread(-> tunnel, -> ret )
- (top) 
    ~ temp preTurnCount = TURNS_SINCE(-> Start)
   -> tunnel -> 
   {preTurnCount == TURNS_SINCE(-> Start): -> DONE }  -> ret

// Story
// TODO: 
// 1) Encorporate what you have at bottom
// 2) Rewrite to take into account when guards spot and chase you.

=== JustStartWriting
    -> Antecedent

=== Antecedent
    -> Central_Room

=== Central_Room
    {
    - Central_Room==1:
        The gem feels heavy in your hands. That shouldn't have surprised you. It's a giant gem. Of course it's heavy. But its weight made it real in a way it wasn't before.
        You've worked so much to get here. You've been planning this for so long. But now it's finally in front of you, and all you have to do know is escape.
        ESCAPE!
        The single word is enough to startle you out of your stupor. If this goes well, you'll have plenty of time to admire the jewel later. IF this goes well. And that will be decided by one thing and one thing only: whether you get out of this mess unscaved. 
        You don't hear any alarms yet. Still, that doesn't mean it's safe to assume you'll have a quiet night. Honestly, it'd be a miracle if no-one saw you on the rooftops. Best to assume "help" is on the way.
        First things first. As you put the gem away in your pack, your head swivels around and you consider your potential exists:
    - Central_Room==2 && !SearchRoom:
        \[You haven't taken the room in before. Now you do.\]
        -> Description ->
    - else:
        \[You're in the Central Room.\]
    } 
- (top)
    You see the main entrance {SearchRoom:the back door, and the vents above you| and the back door}.
    +   The main entrance[...] consists of a set of heavy-looking double doors, sealed shut.
        VAR Doors = "locked"
        ~ SpendTime(1)
        ++  (blastFrontDoor)[Blast the door open]
            ~ SpendTime(3)
            ~Doors = "blasted"
            VAR MainGuards = "Alerted"
            You kick the door down or explode it or something. 
            -> Main_Hall
        ++ {Doors == "locked"}[Pick the lock]
            HIER VERDER:/*
            
            
        VAR foundLock = false
        Steeling yourself, you take of your pack and take out your lockpicking set. Glancing at the lock, you estimate the lock's size to be about a 3. You take out the appropriate rod along with the pryer, and turn to the door. // the size of the lock could be randomized and maybe the esitmate could be wrong if hurried
            -> LockPicking
        * * {Doors == unlocked} [Peek through the doors.]
        ~ Time += 1
            Beyond the doors are two guards. They don't seem to have noticed you. Beyond them it's a busy street.
            -> MainEntrance
        + + {Doors >= unlocked} Exit through the doors.
        ~ Time += 5
        -> EscapeFromFront
        + + [Return]
            -> Antecedent.l00
= LockPicking
        You decide to 
        * * * {Doors == locked && !foundLock} <> carefully feel the lock.
            ~ Time += 5
            You take a breath, and carefully set to work. You soon find the right pin for the lock.
            ~ foundLock = true
            -> LockPicking
        + + + <> try to force the lock.
            ~ Attempts ++
            ~ Time += 2
            {foundLock: having found the purchase |hurriedly, }you try to force the lock.
            {
            - foundLock || RANDOM(1,100)<=60-Attempts*10:
                The pick clicks into place. The door opens.
                ~ Doors = unlocked
                -> MainEntrance
            - RANDOM(1,100)<=90-Attempts*20:
                The lock breaks from the force.
                ~ Doors = stuck
            - else:
                But the door does not give way.
                ->LockPicking
            } 
        -> DONE
        + + [Return]
            -> Antecedent.l00*/
            
            
            
        ++  {Doors=="blasted"}{Doors=="open"}[Travel to Main Hall] // This option should be shown after having picked the lock or broken it down. (See below)
            ~ SpendTime(2)
            -> Main_Hall
        ++  [return]
            -> top
    +   [Travel to Back Room] // This option is chosen after having broken it down or checked the lock.
        ~ SpendTime(2)
        -> Back_Room
    +   {SearchRoom}[Travel to Vents] // After climbing back up to it.
        ~ SpendTime(3)
        -> Central_Vents
    *   (SearchRoom)[Look around the room]
        You look around the room again.
        ~ SpendTime(1)
        You take another careful look around you to take in the room you're in.
        {Central_Room<2:->Description->|Good thing, too: }You spot some things you didn't before, such as the vent above.
        -> top


    
== CentralRoomChase
    WIP
    -> DONE

== Description
    This room do be central.
-    ->-> 

=== Main_Hall
    {Main_Hall>1:->top} // Skip intro if seen already.
    \[Main Hall Description Here.\]
    {Doors == "blasted":
    The two guards beyond it immediately turn around to face you. 
    -else:
    There are two guards here, chatting with each other. They do not seem to have noticed you.
    }
- (top)
    {Doors == "blasted" && (Alfons == "mobile" || Bernard == "mobile"):
    +   [Face them head on.]
        -> MainHallCombat
    +   [Turn around and run.]
        -> CentralRoomChase // en daar een beschrijving afhankelijk van waar je vandaan komt
    -else:
    +   [Sneak into the room]
        ~ SpendTime(3)
        ++  [Avoid the Guards and exit the building]
            ~ SpendTime(10)
            -> Front_Street
        ++  [Sneak up to the guards]
            VAR stealthApproach = true
            ~ SpendTime(2)
            -> MainHallCombat
    +   [Attack the guards]
        ~ MainGuards = "Alerted"
        -> MainHallCombat
    +   [Travel to Central Room]
        ~ SpendTime(2)
        -> Central_Room
    }

== MainHallCombat
    {
    - Doors=="blasted":
    The guards are panicked. They'll go down easily.
    ~SpendTime(2)
    - stealthApproach:
    The guards obviously have no clue you're coming.
    ~SpendTime(3)
    - else:
    The guards take out their batons and assume defensive positions. This will not be easy.
    ~SpendTime(10)
    }
    *   [Attack Guards]
        You consider before you strike. Should you take care not to kill these two, or forego such kindness?
        **  [Kill them]
            ~ Alfons = "dead"
            ~ Bernard = "dead"
            -> aftermath
        **  [Knock 'em out] // And tie them up for extra time but more security?
            ~ Alfons = "knockedOut"
            ~ Bernard = "knockedOut" 
            ~ SpendTime(5) // avoiding lethal blows takes some more time.
            -> aftermath
    *   [Reconsider. Turn back.]
        {stealthApproach:->Central_Room|->CentralRoomChase}
- (aftermath)
    {
    - Doors=="blasted":
    The guards are panicked. They went down easily. But that definitely blew any chance of going unnoticed.
    ~SpendTime(2)
    - stealthApproach:
    The guards are caught of guard. You swiftly take them out.
    ~SpendTime(3)
    - else:
    The guards put up a good fight. It takes you longer than you would have liked.
    ~SpendTime(10)
    }
    Hide evidence?
    +   [Y] 
        After cleaning up the evidence of the struggle, y<>
        # The bodies are cleaned up. remember to take this into account with the investigation.
        ~SpendTime(15)
        -> exit
    +   [N]
        Y<>
        # The fight scene is NOT cleaned up. remember to take this into account with the investigation.
        -> exit
- (exit)
    ou quickly slip outside.
    -> Front_Street
    
    
== MainHallChase
    WIP
    -> DONE

=== Front_Street
    \[Intro text here.\]
- (top)
    <- Street_Options
    +   [Go back inside]
        ~ SpendTime(2)
        -> Main_Hall

=== Back_Room
    {
    -   Back_Room==1:
        There is a sleeping guard here.
    -   Charles=="asleep":
        The guard is still sleeping.
    -   Charles=="dead":
        The guard is where you left him.
    }
- (top)
    *   {Charles!="dead"}[Quickly Kill the Guard]
        **  [Kill them]
            ~ Alfons = "dead"
            ~ Bernard = "dead"
            ~ SpendTime(2)
            -> Front_Street
        **  [Knock 'em out] // And tie them up for extra time but more security?
            ~ Alfons = "knockedOut"
            ~ Bernard = "knockedOut" 
            ~ SpendTime(5)
            -> Front_Street
    +   {Charles!="dead"}[Leave the Guard be and sneak by]
        ~ SpendTime(4)
        -> Back_Street
    +   {Charles=="dead"}[Make your way out.]
        ~ SpendTime(1)
        -> Back_Street
    + [Travel to Central Room]
        ~ SpendTime(2)
        -> Central_Room

=== Back_Street
    \[Intro text here.\]
- (top)
    <- Street_Options
    +   [Go back inside]
        ~ SpendTime(2)
        -> Back_Room
    -> DONE

=== Central_Vents
    WIP
    -> DONE

=== Street_Options
    *   [Quick & Dirty]
        ~ SpendTime(20)
        You steal a car and high-tale it outta there. You ditch the car at the harbor.
        -> Escape
    *   [Careful & Slow]
        You stay on foot and stick to dimly lit alleyways. After a while, you safely make it to shore.
        ~ SpendTime(60)
        -> Escape

=== Sirens
    In the distance, you can hear sirens. Seems someone tipped off the local law enforcement.
-   ->->

=== Police_Raid
    WIP. A chase goes here, likely fathal but the player might escape.
-   ->->

=== Escape
    You escaped! But what consequences have you wrought..?
    -> Police_Investigation
=== Police_Investigation
// consider the police and see what evidence the police have.
// treat it as if an investigation and see how much they can find
// more rooms to be in increases chacne of witnesses, more actions increases vivor of search etc.
// remember to involve cameras somehow too
    -> Consequent

=== Consequent
    WIP
    -> END

=== GameOver
    Game Over.
    -> END
== Time
    Time's Up.
    -> GameOver
/*

//LIST Rooms = centralRoom, mainHall, backRoom, ventsCentral, ventsMain, ventsBack, ceiling, street, alleyg

VAR CentralRoom = 0
VAR MainHall = (Alfons, Bernard)

LIST AliveStates = alive, responsive, awake, mobile // ascending; if x, is also all under x. incremental / superseding

//LIST PatrolStates = active, alert, aware, engaged // ascending; if x, is also all under x.

// should i make a function to split those states..?
VAR Alfons = mobile//(mobile, active, mainHall)
VAR Bernard = mobile//(mobile, active, mainHall)
VAR Charles = responsive//(responsive, active, backRoom) //sleeping 

LIST LockDown = _Rooms // list of locations not safe to travel to 
~LockDown=()

/*
=== function GuardsInRoom(room)
VAR Collected = 0

{Alfons^room: Collected += Alfons} // oef dit is een stuk minder mooi dan het zou zijn in c#- ik mis echt de looping logica
{Bernard^room: Collected += Bernard}
{Charles^room: Collected += Charles}
~ return Collected
*/

/*/*

LIST Doors = (locked), stuck, unlocked, open, blasted

LIST PoliceAwareness = (none), rumors, spotted, identified


 <>
- (l00)
    
    - GuardA==alerted or GuardB == alerted or GuardC == alerted:
        + [Flee toward the main entrance.]
            ~ Time += 1
            -> MainEntrance
        + [Flee toward the back door.]
            ~ Time += 1
            -> BackDoor
        + [Flee into the vent.]
            ~ Time += 1
            -> Vent
    - else:
        + The main entrance[...] consists of a set of heavy-looking double doors, sealed shut.
            ~ Time += 1
            -> MainEntrance
        + [The back door...]
            ~ Time += 1
            -> BackDoor
        + [The vent...]
            ~ Time += 1
            -> Vent
    }
== MainEntrance
    {caught:->Caught}
        


== EscapeFromFront
    * {GuardA<incapasitated || GuardB <incapasitated} [Attack the guards]
    * * [Kill them stone dead.]
        -> AttackFrontGuards(true)
    * * [Just make them take a little nap.]
        -> AttackFrontGuards(false)
    * {(GuardA<incapasitated || GuardB <incapasitated) && !MainEntrance.blaste} [Avoid the guards]
        {RANDOM(1,100)<=10+Time*10:
        ~ Time +=15
        Sneaky sneaky! You think made it out without being seen.
        - else:
            Sneak fail! You were spotted. 
        }
        -> DONE
    + {GuardA>undisturbed && GuardB > undisturbed}
    -> WIP
= AttackFrontGuards(lethal)
    Boom, pow, boom, pow.
    {lethal:
        ~ Time+=5
        You fuckin killed those guards so hard dude.
        ~GuardA = dead
        ~GuardB = dead
    - else:
        ~ Time+=10
        Sleepy time.
        ~GuardA = incapasitated
        ~GuardB = incapasitated
    }
        -> GetawayFront



// todo: replace these options below by point by point choices, giving multiple options. each one impacts the state of things for next time.
// tip: for help with the nested structure, open ink's crime scene example.



