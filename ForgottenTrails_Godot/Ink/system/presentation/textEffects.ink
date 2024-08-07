
VAR Speed = 1.0 

=== function Spd(value) // change speed of textflow based on positive number, expressed as multiplier (eg .8 for 80%)
~ Speed = value
~ _Spd(value)
=== function _Spd(float) 
// nothing
EXTERNAL _Spd(float) 

=== function Halt(duration)
~ _Halt(duration)

=== function _Halt(float) // [EXPERIMENTAL] pause text for x seconds 
<<i>Unity halts for: {float}</i>> 
EXTERNAL _Halt(float) 

=== function Clear()  // Clears the textbox and moves that text to log
~ _Clear()

=== function _Clear()
<<i>Clear Page</i>>
EXTERNAL _Clear()

VAR stop = "\{stop\}" // Used in Unity for stopping the continue loop

VAR glue = "\{glue\}" // used to glue next line to this.

VAR aglue = "\{aglue\}" // used to glue this to previous line


  === Section_VariableLanguage ===
  /* ---------------------------------
   ### System: Variable language for dealing with plural vs singular, multiple possible player genders, etc.

  ----------------------------------*/
  -> DONE
  // 


=== function IsAre(list)
	{LIST_COUNT(list) == 1:is|are}

=== function ListWithCommas(list, if_empty)
    {LIST_COUNT(list):
    - 2:
            {LIST_MIN(list)} and {ListWithCommas(list - LIST_MIN(list), if_empty)}
    - 1:
            {list}
    - 0:
            {if_empty}
    - else:
              {LIST_MIN(list)}, {ListWithCommas(list - LIST_MIN(list), if_empty)}
    }



VAR PlayerName = "PlayerName"

EXTERNAL PromptName()
=== function PromptName()
~PlayerName = "Player"

// This is the list of vars for the gender stuff, but that's also not really well implemented yet of course.

LIST players_gender = (undefined), nonbinary, male, female

// TODO: reform the below to lists
VAR players_eyecolor = "Undefined"
VAR players_hair = "Undefined"
VAR players_hair_color = "Undefined"
VAR players_hair_style = "Undefined"

VAR androgynous = "androgynous"
VAR they = "they"
VAR them = "them"
VAR their = "their"
VAR theirs = "theirs"
VAR Mx = "Mx"
VAR master = "master"
VAR person = "person"
VAR kid = "kid"
VAR lad = "lass"
VAR guy = "guy"

=== function SetPronouns(gender)
~ players_gender = gender
{
	- players_gender == male:
	    ~ androgynous = "masculine"
		~ they = "he"
        ~ them = "him"
        ~ their = "his"
        ~ theirs = "his"
        ~ Mx = "Mr"
        ~ master = "mister"
        ~ person = "man"
        ~ kid = "boy"
        ~ lad = "lad"
        ~ guy = "guy"        
        
    - players_gender == female:
        ~ androgynous = "feminine"
        ~ they = "she"
        ~ them = "her"
        ~ their = "her"
        ~ theirs = "hers"
        ~ Mx = "Ms"
        ~ master = "missus"
        ~ person = "woman"
        ~ kid = "girl"
        ~ lad = "lass"
        ~ guy = "gal"
        
	- else:
	    ~ androgynous = "androgynous"
        ~ they = "they"
        ~ them = "them"
        ~ their = "their"
        ~ theirs = "theirs"
        ~ Mx = "Mx"
        ~ master = "master"
        ~ person = "person"
        ~ kid = "kid"
        ~ lad = "lad"
        ~ guy = "guy"
}
