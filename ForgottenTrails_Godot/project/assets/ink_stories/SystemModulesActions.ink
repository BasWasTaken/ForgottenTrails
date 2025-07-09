=== Actions ===
+{DEBUG == true}[Show debug options]
->TestingDebugActions
+[Talk]
->Talk
+[Examine]
->Examine
+[Use]
->Use
+[Move]
->Move
+[{ItemChoice("lantern")}]
->ItemStateLantern->
-> Actions
+[{ItemChoice("torch")}]
->ItemStateTorch->
-> Actions


=== Talk ===
+{Party has Wesley}[Talk to Wes]
->DialogueWesley
+{Party == Player}[Talk to yourself]
->DialogueYourself
+[Nevermind]
->Actions

=== Examine ===
{CurrentLocation == LOC_TestingGrounds:->TestingGroundsExamine}
{CurrentLocation == LOC_CrumblingMonasteryChurchCrypt:->CrumblingMonasteryChurchExamine}

=== Use ===
{CurrentLocation == LOC_TestingGrounds:->TestingGroundsUse}
{CurrentLocation == LOC_CrumblingMonasteryChurchCrypt:->CrumblingMonasteryChurchCryptUse}
+[Nevermind]
->Actions

=== Move ===
+{DEBUG == true}[Show debug options]
->TestingDebugMovement
+{CurrentLocation == LOC_CrumblingMonasteryChurchCrypt && LeftCrypt == 1}[Leave the crypt]
->CrumblingMonasteryChurchMainHall
+[Nevermind]
->Actions