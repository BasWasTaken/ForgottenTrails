LIST TimeOfDay = (Dawn), Morning, Midday, Afternoon, Dusk, Evening, Night
//(Here we consider the day to start at dawn and end at night. Admittedly a large part of day 1's night is technically part of day 2, the alternatives are either saying that the day ends in evening, making night part of the next day entirely, which complicates the condition "TimeOfDay>=Dusk", or splitting the night up further in before or after midnight. Of these three I find the current option to be least unsatisfactory.)

LIST DayOfTheWeek = (Monday), Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday

VAR DaysPassed = 0

=== function Time_Advance() // used to progress time 1 or loop back around and progress dayspasse 1.
{ TimeOfDay == LIST_MAX(LIST_ALL(TimeOfDay)):
    ~ DaysPassed ++
	~ TimeOfDay = LIST_MIN(LIST_ALL(TimeOfDay))
- else:
	~ TimeOfDay ++ 
}   
~Print("It is now {TimeOfDay}, Day {DaysPassed+1}")

=== function Time_AdvanceUntil(time)
~ Time_AdvanceUntilBetween(time, time)

=== function Time_AdvanceUntilBetween(min, max)
~ Time_Advance() // first call at least 1 advancement
~ Time_WaitUntilBetween(min, max) // then check and loop if needed

=== function Time_WaitUntil(time)
~ Time_WaitUntilBetween(time, time)

=== function Time_WaitUntilBetween(min, max) // used to progress time to point, iterating dayspassed by 1 if appropriate. Does NOT progress if time is already fit.
{
- min <= TimeOfDay && TimeOfDay <= max:
// do nothing
- else:
~ Time_Advance() // call function
~ Time_WaitUntilBetween(min, max) // and go again
}