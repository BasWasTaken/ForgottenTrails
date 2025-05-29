INCLUDE Functions.ink
INCLUDE Stories.ink
INCLUDE System/DevMode.ink

//Files that need to be included to not break something else, but need to be replaced
INCLUDE Archive/EdanRegion/RandomEvents.ink
INCLUDE Archive/IntroductionAwakening.ink
INCLUDE Archive/EdanRegion/RuinedCoast.ink
INCLUDE Archive/EdanRegion/SeaBreezePath.ink
INCLUDE Archive/EdanRegion/SouthernRoads.ink
INCLUDE Archive/Edan/EdanGate.ink
INCLUDE Archive/Edan/EdanInn.ink
INCLUDE Archive/Edan/EdanMarket.ink
INCLUDE Archive/Edan/EdanPrison.ink



//Set inventory
~ Inventory = (Knife, Rope, Lantern, ForagedMushrooms)

//Set party
~ Party = (Player)

->Start

=== Start ===
//@Bas was there a reason for including change weather here?
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
