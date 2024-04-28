// --------- Bas  ---------
// Utility
    VAR TimeSpent = 0
    VAR TimePoliceCalled = 0 
=== function SpendTime(minutes)
    ~ TimeSpent += minutes
{
- Chase:
    ~ ChaseSpace -= minutes // while you are busy, the chasers are closing in
}
    
VAR ExtraChance = true
    
=== CheckTime(->ret)
    ~ Print("It is now roughly {ReadTime(TimeSpent)}. You have spent {TimeSpent} minutes during your chasers.")
{
- Chase:
    {
    - ChaseSpace<=0:
        {
        -ExtraChance && ChaseSpace>-2:
            ~ ExtraChance=false
            You know the {chasers()} have caught up. You know it's futile. But you keep running. 
        - else: 
            But before you can make your move, you feel your neck hairs stand on end moments before you feel a weight on your back as you hear "That's as far as you go! Don't move another step."
        -> GameOver.Caught 
        }
    - ChaseSpace==1: 
        You can feel the {chasers()} are right behind you now.
        # The player now has 1 space left. If the player next chooses a move even 1 above 3, they lose.
    - else: 
        The {chasers()} are {|still} on your heels. //Getting closer or less close, but I don't know that.
    }
}
{
- TimeSpent > 360:
    -> GameOver.Time
- TimeSpent > 60:
    You have spent an hour in your escape.
- TURNS_SINCE(->Sirens)>5 && !Police_Raid && (TimeSpent - TimePoliceCalled) > RANDOM(10,15):
    -> Police_Raid ->
- !Sirens && TimeSpent > RANDOM(7,15):
    ~ TimePoliceCalled = TimeSpent
    -> Sirens ->
}
=== function ReadTime(minutes)
    ~ return "{TimeSpent/60} o\' clock"

=== tunnel_as_thread(-> tunnel, -> ret )
    -   (top) 
    ~ temp preTurnCount = TURNS_SINCE(-> Start)
    -> tunnel -> 
    {preTurnCount == TURNS_SINCE(-> Start): -> DONE }  -> ret


=== function back(->scene)
    {seen_very_recently(scene):back |}


    VAR Alfons = "mobile"
    VAR Bernard = "mobile"
    VAR Charles = "asleep"
    
    LIST evidence = (ventOpened),(gemMissing),mainHallFight,GTA

=== function chasers
{
- Police_Raid && (Alfons == "mobile" || Bernard == "mobile" || Charles == "mobile"):
    police & guards
- Police_Raid:
    police
- else:
    guards
}

=== JustStartWriting
    -> Antecedent

=== Antecedent
    -> Central_Room

=== Central_Room
    ~ ChaseSpace += 3 // you make some leeway
    -> CheckTime(->enter)
    - (enter)
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
    + The main entrance[...] consists of a set of heavy-looking double doors, sealed shut.
        VAR Doors = "locked"
        ~ SpendTime(1)
        ++ (blastFrontDoor)[Kick in the door]
            ~ SpendTime(3)
            ~Doors = "blasted"
            ~temp MainGuards = "alerted"
            You kick the door down or explode it or something. 
            
        ++ {Doors == "locked"}[Pick the lock]
            Steeling yourself, you take of your pack and take out your lockpicking set. Glancing at the lock, you estimate the lock's size to be about a 3. You take out the appropriate rod along with the pryer, and turn to the door.
            -> LockPicking ->
        ** {Doors != "locked" && Doors!="blasted"}/*like maybe it was broken open with a breekijzer*/ [Peek through the doorcrack.]
            ~ SpendTime(1)
            Beyond the doors are two guards. They don't seem to have noticed you. Beyond them is the open streets. // doesn't really make sense, ok for now
                
        ++ {Doors=="blasted"||Doors=="open"||Doors=="unlocked"}[Travel {back(->Main_Hall)}to Main Hall] // This option should be shown after having picked the lock or broken it down. (See below)
                ~ SpendTime(2)
        -- -> Main_Hall
        ++ [return]
            -> top
    + The back doors[...] are there too.
        ~ SpendTime(1)
        ++ [Travel {back(->Back_Room)}to Back Room] // This option is chosen after having broken it down or checked the lock.
            ~ SpendTime(2)
        -- -> Back_Room
        ++ [return]
            -> top
    + {SearchRoom}The vents[...] hang above you. 
        ~ SpendTime(1)
        ++ {evidence?ventOpened}[Travel {back(->Central_Vents)}to Vents]
            ~ SpendTime(3)
        ++ {evidence?ventOpened}[Shut the vent cover]
            ~ SpendTime(1)
            ~evidence-= ventOpened
            You closed the vent{| again}.
            ->top
        ++ {!(evidence?ventOpened)}[Open the vent cover]
            ~ SpendTime(1)
            ~evidence+= ventOpened
            You opened the vent again.
            ->top
        -- -> Central_Vents
        ++ [return]
            -> top
    * (SearchRoom)[Look around the room]
        You look around the room again.
        ~ SpendTime(1)
        You take another careful look around you to take in the room you're in.
        {Central_Room<2:->Description->|Good thing, too: }You spot some things you didn't before, such as the vent above. You actually used this to enter but completely forgot about them.
        -> top

== LockPicking
    VAR foundLock = false
    VAR Attempts = -1
            --- (topLock)
            You decide to
            *** {Doors == "locked" && !foundLock} <> carefully feel the lock.
                ~ SpendTime(5)
                You take a breath, and carefully set to work. You soon find the right pin for the lock.
                ~ foundLock = true
                -> topLock
            +++ <> {foundLock: |blindly try to} force open the lock.
                ~ Attempts ++
                ~ SpendTime(2)
                {foundLock: having found the purchase |hurriedly, }you try to force the lock.
{
- foundLock || CheckSimple(25,Attempts,10): //skill throw
            The pick clicks into place. The door opens.
            ~ Doors = "unlocked"
            // do i need to make all the door interactions a whole own scene...? that seems absurd.
            ->->
- !Check(75,0,1,Attempts,20): // saving throw: notice the "!"
            The lock breaks from the force. You can't pick it anymore.
            ~ Doors = "busted"
            ->->
- else:
            But the door does not give way.
            ->topLock->
} 
    
== CentralRoomChase
    The {chasers()} are hot on your heels as you dart into the central room.
    -> Central_Room

== Description
    WIP
#WIP
    - ->-> 

=== Main_Hall
    ~ ChaseSpace += 3 // you make some leeway if in chase
    -> CheckTime(->enter)
    - (enter)
    {Main_Hall==1:The main hall is a grand foyer lush with paintings, vases on half-pillars, and velvet ropes and carpes leading the way from the front door toward the centrall room.} // first time description
{
-(Alfons == "mobile" || Bernard == "mobile"):
    {
    -Doors == "blasted":// if coming straight from the breach entry
        You don't have much time to appreciate the room, though. The two guards in the room immediately turn around to face you. 
        -> breach
    -else:
        There are two guards here, chatting with each other. They do not seem to have noticed you.
        -> top
    }
    The guards {Alfons=="dead":' corpses} {evidence?"mainHallFight":lie still on the floor.|are cropped up in the corner.}
    -> top
- else:
    -> bottom
}
    -> top
    - (top)
    + {(Alfons == "mobile" || Bernard == "mobile")}[Sneak into the room]
        ~ SpendTime(3)
        ++ {(Alfons == "mobile" || Bernard == "mobile")}[Avoid the Guards]
            ~ SpendTime(10)
            //->bottom
        ++ {(Alfons == "mobile" || Bernard == "mobile")}[Sneak up to the guards]
            VAR stealthApproach = true // seen this scene
            ~ SpendTime(2)
            -> MainHallCombat->//bottom
    + {(Alfons == "mobile" || Bernard == "mobile")}[Attack the guards]
        ~ temp MainGuards = "Alerted"
        -> MainHallCombat->//bottom

    - (breach)
    + [Face them head on.]
        -> MainHallCombat->//bottom
    + [Turn around and run.]
        You take advantage of their momentary stupor and shoot of back into the central room.
        -> CentralRoomChase // en daar een beschrijving afhankelijk van waar je vandaan komt

    - (bottom) // catch the avoid option above, or options where combat is avoided altogether because guards died in a previous visit to this scene.
    + [Travel {back(->Front_Street)}outside]
        You quickly slip outside.
        -> Front_Street
    + [Travel {back(->Central_Room)}to Central Room]
        ~ SpendTime(2)
        -> Central_Room

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
    * [Attack Guards]
        You consider before you strike. Should you take care not to kill these two, or forego such kindness?
        ** [Kill them]
            ~ Alfons = "dead"
            ~ Bernard = "dead"
            -> aftermath
        ** [Knock 'em out] // And tie them up for extra time but more security?
            ~ Alfons = "knockedOut"
            ~ Bernard = "knockedOut" 
            ~ SpendTime(5) // avoiding lethal blows takes some more time.
            -> aftermath
    * [Reconsider. Turn back.]
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
    ~ evidence += mainHallFight
    Hide evidence?
    * [Y] 
        You clean up the aftermath of the struggle.
        # The bodies are cleaned up. remember to take this into account with the investigation.
        ~evidence-= "mainHallFight"
        ~SpendTime(15)
       ->->
    + [N]
        No time to play concierge.
        # The fight scene is NOT cleaned up. remember to take this into account with the investigation.
       ->->
    
== MainHallChase 
    The {chasers()} are hot on your heels as you dart into the main hall.
    -> Main_Hall

=== Front_Street
    \[Description of front streets here.\]
    - (top)
    <- Street_Options
    + [Go back inside]
        ~ SpendTime(2)
        -> Main_Hall

=== Back_Room
    ~ ChaseSpace += 3 // you make some leeway if in chase
    -> CheckTime(->enter)
    - (enter)
{
- Back_Room==1:
        There is a {Charles=="asleep":sleeping |sleepy-looking }guard here.
- Charles=="asleep" && !Chase:
        The guard is still sleeping.
- Charles=="dead":
        The guard is where you left him.
}
    - (top)
    * {Charles=="mobile"}[Take him out]
        ** [Kill him]
            ~ SpendTime(3)
            ~ Charles = "dead" 
        ** [Choke him out] // And tie them up for extra time but more security?
            ~ Charles = "knockedOut" 
            ~ SpendTime(6)
    * {Charles!="mobile"}[Quickly take him out]
        ** [Slit his throat]
            ~ SpendTime(2)
            ~ Charles = "dead" 
        ** [Choke him out] // And tie them up for extra time but more security?
            ~ Charles = "knockedOut" 
            ~ SpendTime(3)
    + {Charles=="asleep"}[Leave the Guard be and sneak by]
        ~ SpendTime(4)
    + {Charles=="dead"||Charles=="knockedOut"}[Simply walk {back(->Back_Street)}out.]
        ~ SpendTime(1)
        - -> Back_Street
    + [Travel {back(->Central_Room)}to Central Room]
        ~ SpendTime(2)
        -> Central_Room
        
== BackRoomChase
    The {chasers()} are hot on your heels as you dart into the back room.
    -> Back_Room
    
=== Back_Street
    ~ ChaseSpace += 3 // you make some leeway if in chase
    -> CheckTime(->enter)
    - (enter)
    \[Intro text here.\]
    - (top)
    <- Street_Options
    + [Go back inside]
        ~ SpendTime(2)
        -> Back_Room
    //-> DONE

=== Central_Vents
    ~ ChaseSpace += 3 // you make some leeway if in chase
    -> CheckTime(->enter)
    - (enter)
    # To prevent this sequence form becoming overlong, the vents just immediately take you to a consequence-free escape. Go you!
    -> Escape

=== Street_Options
    * [Quick & Dirty]
        ~ SpendTime(20)
        You steal a car and high-tale it outta there. You ditch the car at the harbor.
        ~ evidence+=GTA
        -> Escape
    * [Careful & Slow]
        You stay on foot and stick to dimly lit alleyways. After a while, you safely make it to shore.
        ~ SpendTime(60)
        -> Escape

=== Sirens
    In the distance, you can hear sirens. Seems someone tipped off the local law enforcement.
    - ->->

=== Police_Raid
    Countless tires screech to a halt outside. The sirens, having grown louder relentlessly, now stop their approach. Numerous doors open & close, indistinct orders are being shouted and more footsteps than you can count approach the building.
    The police have arrived. 
    * [Shit.]
    In a minute, the cops will have surrounded you. If they perceive you as a threat they will likely immediately open fire on you.
    Given your current situation, you decide the only course of action afforded to you is to...
    * [Forfeit the gem & Surrender]
        You know when you're beat. Better to play along and be locked up, biding your chance to escape and try again another day, than to be shot here and have it all end. You take a last look at the gem you risked so much for...
        What a marvel.
        You throw your hands up as the guards enter the room.
        -> GameOver.Caught
    * [Forfeit the gem & Hide]
        ~ evidence-=gemMissing
        You have one last desparate play left. You drop the gem where you stand, and<>
    * [Hide] You<>
        - (hide) enter the nearest vent bide your time.
        #WIP
        Need options to fight or flight.
        -> DONE
    * [Wait by a door and attack the first cop who enters your room.]
        This probably just leads the player to get shot and/or caught and put in jail.
        -> GameOver.Caught
    * [Run & Continue your escape]
        Fuck it. You've come this far, you've dug yourself this deep. The only way out is through.
    You hear the sound of doors breaking down and glass shattering. It's now or never.
    VAR Chase = true // Enter chase mode. 
    {Charles=="asleep":
        ~Charles="mobile" // this wakes charles up
    }
    VAR ChaseSpace = 5
    - ->->

=== Escape
    You escaped! But what consequences have you wrought..?
    -> Police_Investigation

=== Police_Investigation
    The police {Police_Raid): |arrive and }search the place for evidence...
    The report is as follows:
    Friday, at midnight, {Police_Raid:the suspect|an unknown suspect} entered the building {evidence^ventOpened:through a rooftop vent and exited into the central room|through unknown means}. The casing of one "magnus gem" was destroyed, after which the gem was taken. 
{
-Doors == "blasted":
    The doors to the main hall were found demolished.
-Doors != "locked":
    The lock of the doors to the main hall were tampered with.
}
{Alfons=="dead"||Alfons=="knockedOut": 
    {
    -evidence^mainHallFight:
        Two guards are currently missing.
    -else:
        Two guards found {Alfons=="dead":murdered|incapacitated} in the main hall.
    } 
}
{Charles=="dead"||Charles=="knockedOut":
    One guard found {Charles=="dead":murdered|strangled} in the back room. 
}
    Charges:
    /- Breaking & Entering
{
- evidence?gemMissing:
    /- Theft.
- else:
    /- Attempted theft.
}
{
- evidence?mainHallFight:
    {
    - Alfons == "dead":
    /- Two accounts of manslaughter.
    - Alfons == "knockedOut":
    /- Two accounts of assault.
    }
}
{
- Alfons == "dead":
    /- Murder.
- Alfons == "knockedOut":
    /- Assault.
}
{
- Doors=="blasted":
    /- Destruction of private property
- Doors=="busted":
    /- Damaging private property
}
{
- evidence?GTA:
    /- Grand Theft Auto
}
{
- Police_Raid:
    /- Resisting Arrest
}
    # consider the route the player took and how many rooms they entered. how long did they linger, etc
    # how loud was the player?
    # how aggregous were the actions?  how vindictvie would the police be?
    # check if there are prints on where attacked?
    WIP
// treat it as if an investigation and see how much they can find
// more rooms to be in increases chacne of witnesses, more actions increases vivor of search etc.
// remember to involve cameras somehow too
    -> Consequent

=== Consequent
    So, the next time the player would return to the scene, they could expect..:
    WIP. 
    # describe verbally what consequences could occur next time the player crosses here.
    # if you entered through the vents, they might be barred now, etc.
    -> END

=== GameOver
    Game Over.
    -> END
    -- (Time)
        Time's Up.
        -> GameOver
    -- (Caught)
        Police catch you.
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




