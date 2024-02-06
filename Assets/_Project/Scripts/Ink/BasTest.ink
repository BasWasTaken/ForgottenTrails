// --------- Bas  ---------
=== BasTravelTest ===
Blah Blah
testt
test2
test3
+ what goes wrong?
-> verder

== verder
~Party_AddMember(Alice)
+ add member?

-> verder1

== verder1
test4
~Party_AddMember(Robert)
test5

+ add another?

-> verder2

== verder2
~SetLocation(LOC_EdanCastle)

+ set location?

-> verder4

== verder4
test6
- (top)

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