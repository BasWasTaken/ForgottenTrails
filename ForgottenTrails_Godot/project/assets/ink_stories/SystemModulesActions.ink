=== Actions ===
+[Talk]
->Talk
+[Examine]
->Examine
+[Use]
->Use
+[Move]
->Move

=== Talk ===
+{Party has Wesley}[Talk to Wes]
->DialogueWesley
+{Party == Player}[Talk to yourself]
->DialogueYourself
+[Nevermind]
->Actions

=== Examine ===
{CurrentLocation == LOC_TestingGrounds:->ExamineTestingGrounds}

=== Use ===
//DebugOptions
//Debug Adding Party Members
+{DEBUG == true && Party hasnt Wesley}[Add Wes to the party]
~Party_AddMember(Wesley)
Added Wes to the party!
->Actions

+{DEBUG == true && Party has Wesley}[Remove Wes from the party]
~Party_RemoveMember(Wesley)
Removed Wes from the party!
->Actions

+{DEBUG == true && Party hasnt Alice}[Add Alice to the party]
~Party_AddMember(Alice)
Added Alice to the party!
->Actions

+{DEBUG == true && Party has Alice}[Remove Alice from the party]
~Party_RemoveMember(Alice)
Removed Alice from the party!
->Actions

+{DEBUG == true && Party hasnt Robert}[Add Robert to the party]
~Party_AddMember(Robert)
Added Robert to the party!
->Actions

+{DEBUG == true && Party has Robert}[Remove Robert from the party]
~Party_RemoveMember(Robert)
Removed Robert from the party!
->Actions

=== Move ===

//Testing Grounds Choices (This should include every accessible location)
+{CurrentLocation == LOC_TestingGrounds}Go to Crumbling Monastery Church Main Hall
-> CrumblingMonasteryChurchMainHall