//--------VUGS--------

//To do:
//-Add function to return to where you cast your line
//-Add location and wheather based flavour text
//-Make the mechanic more interesting (Different behaviour by fish type/size, more interaction)
=== Fishing ===
LIST FishType = AtlanticCod, AtlanticPollock, AtlanticMackerel, BallanWrasse, EuropeanFlounder, SeaTrout, Whiting
//Above list based on an online site tracking the most caught fish in the Firth of Forth xD

~Weather = ClearSkies //Needs to be set by the current location, but currently here for testing purposes
You take out your fishing rod and cast your line into the water.
{CurrentLocation == LOC_RuinedCoast:{Weather == ClearSkies:Your lure landed with a gentle splash and is currently bobbing along with the waves.}}

//Remaining weather to receive flavour text: LightClouds, ThickClouds, LightRain, HeavyRain, Thunderstorm

//Variables used
LIST FishingWaters = FISH_FirthofForth, FISH_EdanWell
VAR FishSize = -1
~ FishSize = RANDOM(0, 100)
VAR FishNibble = -1
~ FishNibble = RANDOM(0, 100)
VAR NibbleBoost = 0
VAR RodPull = 0
VAR ReelIn = 0

{
-FishSize > 0 && FishSize <= 30 && FishingWaters == FISH_FirthofForth:
    ~FishType = Whiting
-FishSize > 29 && FishSize <= 80 && FishingWaters == FISH_FirthofForth:
    ~FishType = SeaTrout
-FishSize > 79 && FishSize <= 100 && FishingWaters == FISH_FirthofForth:
    ~FishType = AtlanticCod    
}

->FishingCoreLoop

=FishingCoreLoop
{FishNibble >= 0 && FishNibble <= 70:You feel the gentle pull of something on the line. ->Nibble}
{FishNibble > 70:There's something on your hook!}
+[Reel in your line]
    ->Bite
    
=Nibble
+[Wait]
    ~ NibbleBoost = RANDOM(0, 20)
    ~ FishNibble = FishNibble + NibbleBoost
    You wait patiently. 
    ->FishingCoreLoop

+[Pull on the rod]
    You tug on your rod, slightly moving the bobber. 
    ~ RodPull = RANDOM(0, 100)
    {RodPull >= 10: 
        ~NibbleBoost = RANDOM(5, 20)
    }
    ~ FishNibble = FishNibble + NibbleBoost
    {RodPull < 10: It appears you scared the fish away. ->DONE}
    ->FishingCoreLoop
    
    
+[Reel in your line]
    You tug on your rod, slightly moving the bobber. 
    ~ ReelIn = RANDOM(0, 100)
    {ReelIn > 80: 
        ~ NibbleBoost = RANDOM(5, 20)
    }    
    ~ FishNibble = FishNibble + NibbleBoost
    {ReelIn < 80: It appears you scared the fish away. ->DONE}
    ->FishingCoreLoop

===Bite===

You've caught a {FishType}!
->DONE
//I forgot how to redirect to the previous/current/next destination like with the travel system. Could you show me how? 
