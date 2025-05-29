LIST Weather = ClearSkies, LightClouds, ThickClouds, (LightRain), HeavyRain, Thunderstorm

=== function ChangeWeather() ===
~print(Weather)
~ temp current = LIST_VALUE(Weather)
//~print(current)
~ temp 3StepCutOff = 20 // chance to move 3 steps
~ temp 2StepCutOff = 50 // chance to move at least 2 steps
~ temp 1StepCutOff = 70 // chance to move at least 1 step

~ temp p = d100() // generate number

~ temp change = 0
{ 
- p>=101-3StepCutOff: //is roll high enough for 3rd cutoff?
    ~ change = 3
- p>=101-2StepCutOff: //is roll high enough for 2nd cutoff?
    ~ change = 2
- p>=101-1StepCutOff: //is roll high enough for 1st cutoff?
    ~ change = 1
}

{
-RANDOM(0,1) == 0: // flip a coin, if heads...
    ~change = change *-1 // change direction of change
}


~print(change + " steps!")

~ temp potentialNew = current + change 
// check if within range, else clamp
{
- potentialNew < 0:
    ~print("too low- capping at 0")
    ~Weather = LIST_MIN(LIST_ALL(Weather))
- potentialNew > LIST_COUNT(LIST_ALL(Weather)):
    ~print("too high- capping at max")
    ~Weather = LIST_MAX(LIST_ALL(Weather))
- else:
    ~Weather += change
}


~print(Weather)
