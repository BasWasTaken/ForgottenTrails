=== SeaBreezePath ===
~SetLocation(LOC_SeaBreezePath)
{PreviousLocation == LOC_EdinburghCrossroads:
You travel along the road. You pick up a hint of salt on the breeze. In the distance, you can see the ocean. 
+[Continue on]
->RuinedCoast
+[Head back]
You decide to turn around. 
->EdinburghCrossroads
}

{PreviousLocation == LOC_RuinedCoast:
You travel along the road. The ocean wind blowing at your back. In the distance, you can see {KnowsLoc(LOC_EdanCastle):Edan Castle town resting on its hill.|the castle on the hill.} 
+[Continue on]
->EdinburghCrossroads
+[Head back]
You decide to turn around. 
->RuinedCoast
}

