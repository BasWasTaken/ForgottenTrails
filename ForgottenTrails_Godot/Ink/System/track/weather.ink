LIST Weather = ClearSkies, LightClouds, ThickClouds, (LightRain), HeavyRain, Thunderstorm

=== function ChangeWeather() ===
~Print(Weather)
~ temp current = LIST_VALUE(Weather)
//~Print(current)
~ temp 3StepCutOff = 20 // chance to move 3 steps
~ temp 2StepCutOff = 50 // chance to move at least 2 steps
~ temp 1StepCutOff = 70 // chance to move at least 1 step

~ temp check = D100() // generate number

~ temp change = 0
{ 
- check>=101-3StepCutOff: //is roll high enough for 3rd cutoff?
    ~ change = 3
- check>=101-2StepCutOff: //is roll high enough for 2nd cutoff?
    ~ change = 2
- check>=101-1StepCutOff: //is roll high enough for 1st cutoff?
    ~ change = 1
}

{
-RANDOM(0,1) == 0: // flip a coin, if heads...
    ~change = change *-1 // change direction of change
}


~Print(change + " steps!")

~ temp potentialNew = current + change 
// check if within range, else clamp
{
- potentialNew < 0:
    ~Print("too low- capping at 0")
    ~Weather = LIST_MIN(LIST_ALL(Weather))
- potentialNew > LIST_COUNT(LIST_ALL(Weather)):
    ~Print("too high- capping at max")
    ~Weather = LIST_MAX(LIST_ALL(Weather))
- else:
    ~Weather += change
}


~Print(Weather)
