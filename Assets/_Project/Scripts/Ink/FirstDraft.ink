// --------- Shared  ---------
//Start-up

//Set core Ink files
//Story Ink files that need to be included can be further defined in Stories.ink
INCLUDE System.ink
INCLUDE Stories
INCLUDE EdanPrison.ink


//Set inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)

//Set party
~ Party = (Player)
->Start

//Set debug variable
VAR DEBUG = false

=== Start ===
Do you want to toggle developer mode?
+[Yes]
-> DeveloperModeToggle
+[No]
-> Opening

// --------- Vugs  ---------
// Placeholder. Need to move this and make it context specific in several cases.     
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
