=== Actions ===
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
{CurrentLocation == LOC_TestingGrounds:->ExamineTestingGrounds}

=== Use ===
//DebugOptions
//Debug Adding Party Members
+{DEBUG == true}[Show debug options]
->TestingDebugActions
+[Nevermind]
->Actions

=== Move ===

//Testing Grounds Choices (This should include every accessible location)
+{DEBUG == true}[Show debug options]
->TestingDebugMovement