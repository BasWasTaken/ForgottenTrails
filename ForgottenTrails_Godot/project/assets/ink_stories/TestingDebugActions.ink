=== TestingDebugActions ===
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

+[Go back]
->Use

=== TestingDebugMovement ===
+[Go to Crumbling Monastery Church Main Hall]
-> CrumblingMonasteryChurchMainHall
+[Go to Crumbling Monastery Church Crypt]
-> CrumblingMonasteryChurchCrypt
+[Go back]
->Move