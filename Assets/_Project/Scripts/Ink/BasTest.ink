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
{stop}
There will now be some choices.
<-AllowPartyScreen(-> top)
<-AllowMap(-> top)
-> DONE
    
=== lineBreakTest
testNormalA
testNormalB
testStopA{stop}
testStopB
testStopSameLineA{stop}testStopSameLineB
testLineBreakA

testLineBreakB
testNativeGlueA<>
testNativeGlueB
testMyGlueA
testMyGlueB{glue}

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