=== BasTravelTest ===
~SetLocation(LOC_EdanCastle)
    {For sake of example, y|Y}ou are {currently|still} at Edan Castle<>
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
<-AllowPartyScreen(-> BasTravelTest)
<-AllowMap(-> BasTravelTest)
-> DONE
    
=== SampleSampleCaveScene
You're in a cave now!
-> DONE

=== SampleSeaBreesePathScene
You're on a sea breeze path now! Whoo!
-> DONE
