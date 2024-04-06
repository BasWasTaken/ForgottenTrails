// --------- Bas  ---------
=== BasTravelTest ===
~Party_AddMember(Alice)
~Party_AddMember(Robert)
//added party{stop}
~SetLocation(LOC_EdanCastle)
//settet location{stop}
- (top)
Hello player.
{For sake of example, y|Y}ou are {currently|still} at {CurrentLocation}<>
{
    - Party?Alice && Party?Robert:
    , together with Alice and Robert.
    - Party?Alice:
    , together with Alice.
    - Party?Robert:
    , together with Robert.
    - else:
    . Alone.
}
There will now be some choices.
<-AllowPartyScreen(-> top)
<-AllowMap(-> top)
* Please show me a travel event. -> Downpour -> TravelingTo(LOC_EdanCastle, ->ScotlandEntranceRoad)->top
-> DONE
    
=== lineBreakTest
testNormalA
testNormalB
testStopA{stop}
testStopB
testStopSameLineA{stop}testStopSameLineB
testLineBreakA

testLineBreakB
testLineBreaksA


testLineBreaksB
testBRA
<br>
testBRB
testNativeGlueA<>
testNativeGlueB
testMyGlueA{glue}
testMyGlueB

testMyAfterGlueA
{aglue}testMyAfterGlueB
+ Ok, Thanks
-> BasTravelTest
    
=== SampleSampleCaveScene
You're in a cave now!
-> DONE

=== SampleSeaBreesePathScene
You're on a sea breeze path now! Whoo!
-> DONE


=== blahblah
test
-> DONE


=== ItemUses
test, use an item
 *[{ItemChoice(EdanInnRoomKey1)}]
 you open the door with your key
    -> blahblah
 *[{ItemChoice("tool&thin")}]
 you lockpick the door
    -> blahblah
    

-> DONE



=== JustStartWriting
-> Antecedent


LIST LiveStates = (alive), dead
VAR GuardA = alive
VAR GuardB = alive
VAR GuardC = alive

LIST Doors = (closed), broken, unlocked, open, blasted

LIST PoliceAwareness = (none), rumors, spotted, identified

=== Antecedent
VAR Time = 0
VAR Attempts=0
// only show on first visit?
The gem feels heavy in your hands. That shouldn't have surprised you. It's a giant gem. Of course it's heavy. But its weight made it real in a way it wasn't before.
You've worked so much to get here. You've been planning this for so long. But now it's finally in front of you, and all you have to do know is escape.
ESCAPE!
The single word is enough to startle you out of your stupor. If this goes well, you'll have plenty of time to admire the jewel later. IF this goes well. And that will be decided by one thing and one thing only: whether you get out of this mess unscaved. 
You don't hear any alarms yet. Still, that doesn't mean it's safe to assume you'll have a quiet night. Honestly, it'd be a miracle if no-one saw you on the rooftops. Best to assume "help" is on the way.
First things first. As you put the gem away in your pack, your head swivels around and you consider your potential exists: <>
- (l00)
    {|you see }the main entrance, the back door, and the vents above you.
    <- MainEntrance
    <- BackDoor
    <- Vent
    -> DONE
== MainEntrance
    + The main entrance[...]
    ~ Time += 1
    <> consists of a set of heavy-looking double doors, sealed shut.
        + + [Blast the door open]
        ~ Doors = blasted
        Note door is broken and describe exit.# WIP 
        -> EscapeFromFront
        + + {Doors == closed}[Pick the lock]
        VAR foundLock = false
        Steeling yourself, you take of your pack and take out your lockpicking set. Glancing at the lock, you estimate the lock's size to be about a 3. You take out the appropriate rod along with the pryer, and turn to the door. // the size of the lock could be randomized and maybe the esitmate could be wrong if hurried
            -> LockPicking
        + + {Doors == unlocked} Peek through the doors.
        -> FrontPeek
        + + {Doors >= unlocked} Exit through the doors.
        -> EscapeFromFront
        + + [Return]
            -> Antecedent.l00
= LockPicking
        You decide to 
        * * * {Doors == closed && !foundLock} <> carefully feel the lock.
            ~ Time += 5
            You take a breath, and carefully set to work. You soon find the right pin for the lock.
            ~ foundLock = true
            -> LockPicking
        + + + <> try to force the lock.
            ~ Attempts ++
            ~ Time += 2
            {foundLock: having found the purchase |hurriedly, }you try to force the lock.
            {
            - foundLock || RANDOM(1,10-Attempts)==1:
                The pick clicks into place. The door opens.
                ~ Doors = unlocked
                -> MainEntrance
            - RANDOM(1,100-Attempts*10)==1:
                The lock breaks from the force.
                ~ Doors = broken
            - else:
                But the door does not give way.
                ->LockPicking
            } 
        -> DONE
        + + [Return]
            -> Antecedent.l00
== BackDoor
    + [The back door...]
    Yada
    -> DONE
== Vent
    + [The vents...]
    Yada
    -> DONE


// make a conditional choice for if you know the secret entrance

// todo: replace these options below by point by point choices, giving multiple options. each one impacts the state of things for next time.
// tip: for help with the nested structure, open ink's crime scene example.
== WIP
+ [Blast open the door, kill the guards, steal a car, and drive off before they know what hit them.]
~ GuardA = dead
~ GuardB = dead
~ Doors = blasted
~ PoliceAwareness = spotted
-> Consequent
+ [Sneak out the back, avoiding the cameras. Kill the lone guard gaurding the rear and keep a low profile until you're out of the city.]
~ GuardC = dead
-> Consequent

== EscapeFromFront
guards
-> DONE


=== Consequent

-> DONE




