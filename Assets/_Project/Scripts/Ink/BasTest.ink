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