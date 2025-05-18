INCLUDE Functions.ink
INCLUDE Stories.ink
INCLUDE System/DevMode.ink
INCLUDE Introduction/ChurchBasement.ink



//Set inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)

//Set party
~ Party = (Player)

->Start

=== Start ===
~ChangeWeather()
Do you want to toggle developer mode?
+[Yes]
-> developer_mode_toggle
+[No]
-> Opening

// Placeholder. Need to move this and make it context specific in several cases.     
=== Death ===
And so ends this tale. Another apprentice that would never return, their findings lost to the wind.
->END
    