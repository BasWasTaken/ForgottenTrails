=== DialogueYourself ===
//Testing Grounds Dialogue (included for testing purposes)
{CurrentLocation == LOC_TestingGrounds:->DialogueYourselfTestingGrounds}
{CurrentLocation == LOC_CrumblingMonasteryChurchCrypt:->DialogueYourselfCrumblingMonasteryChurchCrypt}

= DialogueYourselfTestingGrounds
You tell yourself a joke. You think you hear the void chuckle.
->Actions

= DialogueYourselfCrumblingMonasteryChurchCrypt
{LeftCrypt == 0 && LightSource == 0:{You mutter to yourself. Didn't you keep a lantern somewhere?|Right! There was a lantern in your satchel. You still feel its strap pulling on your shoulder. Surely it's still in there?|You keep mumbling to yourself in the dark.|You keep mumbling to yourself in the dark. You know, at some point people are going to find you weird.|Yeah, okay, we're in weird territory now. Get a move on will you?|If you keep talking to the void, at some point the void's going to talk back.|As you continue to speak to an empty, dark and unfamiliar room, you start to become strangely comfortable with it.|You and the room have a pleasant conversation.} ->Actions}

{LeftCrypt == 0 && LightSource == 1:Your voice reverberates against the damp walls. ->Actions}

{LeftCrypt == 1 && LightSource == 0:You complain about the lack of light to no one in particular. ->Actions}

{LeftCrypt == 1 && LightSource == 1:You reminisce about waking up here. Would you like to take another nap on the floor? ->Actions}
