=== BasTravelTest ===
~SetLocation(LOC_EdanCastle)
    For sake of example, you are currently at Edan Castle<>
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

    +  [{AllowPartyChanges()}] -> PartyScreen(-> BasTravelTest) ->
    +  [{AllowMap()}] -> MapScreen(-> BasTravelTest) 
   
    
=== SampleSampleCaveScene
You're in a cave now!
-> DONE

=== SampleSeaBreesePathScene
You're on a sea breeze path now! Whoo!
-> DONE
