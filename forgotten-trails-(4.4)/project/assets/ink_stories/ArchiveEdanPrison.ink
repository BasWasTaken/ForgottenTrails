=== CastlePrison ===
//Vugs: Pretty old piece of text and unfinished. Probably needs a proper rewrite. 
~ CurrentLocation = "EdanCastlePrison"
VAR MaryUpset = false
{MaryPrisonGreeting: You find yourself in the damp prison room. {MetMary: Mary | A figure }is huddled against the far wall, a set of iron bars keeping you apart. | You descend the worn uneven steps cautiously. It's cooler here, and slightly damp. The thick stone walls appear to keep out most of the sun's warmth. You arrive in a broad, round room, a torch flickering on your left. The room is divided down the middle by a set of iron bars. Behind them, at the back of the room, you see a figure huddled against the wall. -> MaryPrisonGreeting}
{MetMary: -> MaryStateChecker | -> MaryPrisonGreeting}

= MaryStateChecker
{MaryUpset == true: -> MaryPrisonUpset}

= MaryPrisonGreeting
+[Greet them]
    **(neutral)"Hi there"
    -> MaryPrisonConvo
    **(kind)"Hi, are you alright?"
    -> MaryPrisonConvo
    **(joke)"The accomodations leave something to be desired, don't they?"
    -> MaryPrisonConvo
    **(rude)"What sort of wretch are you that they left you down here?"
    -> MaryPrisonConvo
    ++[On second thought, never mind] You decide not to speak to them. 
    ->CastlePrison
    
= MaryPrisonConvo
The figure perks up to look at you. Their hood falls back to reveal a young woman's face. Her long auburn hair falls down unkempt, perhaps due to the lack of a comb, but her bright hazel eyes look at you with intrigue.

{CastlePrison.MaryPrisonGreeting.neutral:"A visitor? And a new face too, what a pleasant surprise." She smiles at you.}{CastlePrison.MaryPrisonGreeting.kind:"I have had better days, but things could be worse I suppose." She smiles faintly. "...Thank you."}{CastlePrison.MaryPrisonGreeting.joke:A smile forms on her lips, "Yes, I should have the maid tidy up and bring a new set of pillows." She laughs sadly, "Oh, if only I could return to those halcyon days." She shakes her head as if to drive away her thoughts. }{CastlePrison.MaryPrisonGreeting.rude:"I see another one of the brutes has made it down here? Pray, tell me what it is you want so I can return to my misery." | "Pray tell, what brings you down to my part of the castle?"}
*"I was just exploring, to be honest."
{CastlePrison.MaryPrisonGreeting.rude: "Well go explore somewhere else then." The woman pulls her hood back over her head and turns away from you. | "Oh, is that so? You might want to be careful then, before they suspect you are trying to help me in one way or another."}
*"I heard about you in town... Are the stories true?"
Her expression grows solemn. "That depends" she says. "What did you hear?"
    **"That you're a monster, capable of killing with your bare hands"
    **"That you're a former queen that once ruled these lands"
    **"The stories conflict a bit, but all agree that you're dangerous"
*{CastlePrison.MaryPrisonGreeting.rude or CastlePrison.MaryPrisonGreeting.neutral}"To mock, what else would one do with a prisoner?"
{CastlePrison.MaryPrisonGreeting.neutral: Her face turns to anger. "I should have known, another one of the brutes. Do you lot have nothing better to do? Just leave me." | "Go find someone who cares, you lout." The woman pulls her hood back over her head and turns away from you.}
*"Do I need a reason?"
{CastlePrison.MaryPrisonGreeting.rude: "Well if you do not have any business here, leave me be. I have nothing to say to you." The woman pulls her hood back over her head and turns away from you. -> MaryPrisonUpset | "Mhm, I suppose not. Would you like to chat for a bit? It has been a while since anyone came down here that was willing to engage me in conversation. I am called Mary, by the way."}
    - (MaryNameLoop) 
    **(MetMary)"A pleasure to meet you Mary, I'm {PlayerName}"
        She smiles warmly at you, "The pleasure is all mine, {PlayerName}."
        ->MaryPrisonConvo2
    **"Simply Mary?"
        She smiles sadly, "These days yes, simply Mary. I do not feel quite worthy to use the full name I once bore." -> MaryNameLoop
= MaryPrisonConvo2
    **"Did you have a topic in mind?"
    **[Pick a topic yourself]
    ++"Actually, I have to get going"
    She looks crestfallen. "I see..." She gives you a sad smile. "Well, I will be here if you change your mind."
    ->CastlePrison
= MaryPrisonUpset
Test
-> END