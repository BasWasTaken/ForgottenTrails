INCLUDE functions.ink
INCLUDE stories.ink

Once upon a time...

 * There were two choices.
 * There were four lines of content.

- They lived happily ever after.
+[Now show me our story.]
    -> preStartTemp

=== preStartTemp ===

//Set inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)

//Set party
~ Party = (Player)

->Start


=== Start ===
~ChangeWeather()
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
