// --------- Vugs ---------
===Opening===
~ FadeToImage(BG_VaultLibrary, 1)
~ SetLocation(LOC_DreamState)
The smell of dusty books fills your nostrils. Around you stark white pillars stretch upward to support an almost impossible ceiling, draped in downward facing flowers made of stone. Against the wall countless bookshelves are lined up. You see various cloaked figures milling about; carrying books to and fro, replacing volumes, having heated (but hushed) discussions and, of course, being engrossed in a book. The near endless shelves seem to only surrender their stranglehold on the place to the stained glass windows, although you get the feeling that those too would be covered by bookshelves if their caretakers could work in the dark.

It's a place that in an ancient past held a different name but you know it by two: "The Vault of Forgotten Books" and "Home". 
Home? Didn't you leave home months ago?

You ponder this briefly. A dream perhaps? Maybe. Surely! Surely? Maybe... 
The thought has nearly solidified in your mind when an unexpected blow knocks you to the ground. As you look up you see one of the caretakers on the floor, just like you, with books scattered about everywhere. The young man begins to apologize profusely, all the while gathering his dropped books. Before you can even get a word in he has already reformed the towering stack, undoubtedly the thing that obstructed his vision in the first place, and has continued on his way. <br>
On the floor, a mirror sparkles with light's reflection. Did he drop this?
*(MirrorY)[Pick it up]
As you reach to pick it up, a reflection stares back at you.
-> Opening.CharCreation0
*(MirrorN)[Leave it be]
If he left it behind then that is his problem. But as you stand up you cannot help but notice your reflection in the glass.
-> Opening.CharCreation0
*[I'm a developer, skip me past the creation.]
~ PlayerName = "Developer"
//-> Awakening

= CharCreation0
You see <>
+{aglue}...a young, male figure[], <>
~ players_gender = male
-> Opening.CharCreation1
+...a young, female figure[], <>
~ players_gender = female
-> Opening.CharCreation1
+...a young, androgynous figure[], <>
~ players_gender = nonbinary
-> Opening.CharCreation1

= CharCreation1
~ set_pronouns(players_gender)

whose eyes shine a bright <>
+...blue[]. 
~ players_eyecolor = "Blue"
-> Opening.CharCreation2
+...green[].
~ players_eyecolor = "Green"
-> Opening.CharCreation2
+...brown[].
~ players_eyecolor = "Brown"
-> Opening.CharCreation2
+...grey[].
~ players_eyecolor = "Grey"
-> Opening.CharCreation2
+...hazel[].
~ players_eyecolor = "Hazel"
-> Opening.CharCreation2
+...amber[].
~ players_eyecolor = "Amber"
-> Opening.CharCreation2
+...red[].
~ players_eyecolor = "Red"
-> Opening.CharCreation2

= CharCreation2
{their} hair <>
+...flows down far beyond {their} shoulders<>
~ players_hair = "long"
-> Opening.CharCreation3
+...falls to about shoulder length<>
~ players_hair = "medium"
-> Opening.CharCreation3
+...gently covers the top part of {their} ears and neck<>
~ players_hair = "short"
-> Opening.CharCreation3
+...has been kept short<>
~ players_hair = "very_short"
-> Opening.CharCreation3
+...is shaven away completely<>
~ players_hair = "bald"
-> Opening.Mirror

= CharCreation3
{players_hair == "long": in splendid}{players_hair == "medium": in pretty}{players_hair == "short": with}{players_hair == "very_short": with}
+...black <>
~ players_hair_color = "black"
-> Opening.CharCreation4
+...brown <>
~ players_hair_color = "brown"
-> Opening.CharCreation4
+...auburn <>
~ players_hair_color = "auburn"
-> Opening.CharCreation4
+...red <>
~ players_hair_color = "red"
-> Opening.CharCreation4
+...blonde <>
~ players_hair_color = "blonde"
-> Opening.CharCreation4
+...white <>
~ players_hair_color = "white"
-> Opening.CharCreation4

= CharCreation4
{players_hair == "long":
+...straights[].
~ players_hair_style = "straight"
-> Opening.Mirror
+...wavy locks[].
~ players_hair_style = "wavy"
-> Opening.Mirror
+...curls[].
~ players_hair_style = "curly"
-> Opening.Mirror
}

{players_hair == "medium":
+...straights[].
~ players_hair_style = "straight"
-> Opening.Mirror
+...wavy locks[].
~ players_hair_style = "wavy"
-> Opening.Mirror
+...curls[].
~ players_hair_style = "curly"
-> Opening.Mirror
}

{players_hair == "short":
+...straight locks[].
-> Opening.Mirror
+...wavy locks[].
-> Opening.Mirror
+...curls[].
-> Opening.Mirror
}

{players_hair == "very_short":
+...slicked back locks[].
-> Opening.Mirror
+...curls[].
-> Opening.Mirror
+...tussled hair[].
-> Opening.Mirror
}
= Mirror
Yes, a young {person} with{players_eyecolor == "Blue": blue}{players_eyecolor == "Green": green}{players_eyecolor == "Brown": brown}{players_eyecolor == "Grey": grey}{players_eyecolor == "Hazel": hazel}{players_eyecolor == "Amber": amber}{players_eyecolor == "Red": red} eyes gently smiles at you, {their} {players_hair == "long":face framed by}{players_hair == "medium":face framed by}{players_hair == "short":head crowned by}{players_hair == "very_short":head adorned with short}{players_hair == "bald":head cleanly shaven}{players_hair_style == "straight": straight}{players_hair_style == "wavy": wavy}{players_hair_style == "curly": curly}{players_hair_color == "black": black}{players_hair_color == "brown": brown}{players_hair_color == "auburn": auburn}{players_hair_color == "red": red}{players_hair_color == "blonde": blonde}{players_hair_color == "white": white}{players_hair == "bald":. | hair.}
+And you recognise the face as yours[].
-> Opening.Recognition
+But you do not recognize yourself[].
You blink and a different face stares back at you.
->Opening.CharCreation0

= Recognition
You consider the name that belongs to this face. 
{prompt_name()}
<This is a buffer line. If removed, {PlayerName} does not show properly in the line immediately following it, although it is still saved. this is a bug Bas needs to fix later.>
Right. A young {person} called {PlayerName}.
+ I've always thought it seemed to fit.
-> Opening.Acceptance
+ It never seemed right somehow. <>
-> Opening.Recognition


= Acceptance
+Disregarding your reflection, you move on.
-> Opening.Master

= Master
~ FadeToImage(BG_VaultOffice, 1)
{Opening.MirrorY: As you lower the mirror in your hand}{Opening.MirrorN: As you leave the mirror on the floor} you hear a familiar voice call out to you. You turn to face it and find that the scenery has changed around you. The comforting smell of books remains but a small open window provides some fresh air. The office is just as you remember it: a small space that forms a stark contrast with the garden outside its window. Its floorboards are barely visible beneath the array of books, knick-knacks and tea cups that should have been returned to the kitchen days ago. You're sitting in a woodback chair. Across from you, behind a worn desk, sits an elderly man. His kind eyes look into yours and you feel a bout of homesickness brewing in your stomach, although cannot fathom why. <br>
He hands you a large, leatherbound tome: your journal. Your most important possession. If you can fill its pages with knowledge not yet held within the Vault, or document where and how you found a tome not yet present on its shelves, you can become like the man in front of you: a keeper. A guaranteed lifetime within these halls, curating knowledge and educating the next generation. If you can't...<br>
You decide not to think about that. You give your master a <>
*...firm handshake <>
and walk out the door.
->Opening.Crossroads
*...warm smile <>
and walk out the door.
->Opening.Crossroads
*...determined nod <>
and walk out the door.
->Opening.Crossroads

=Crossroads
As you step through the door you feel your feet land firmly in the dirt. Before you a path winds gently down green hills.
*[Follow the path]
->Opening.Hills
*[Turn around]
You turn around but the door is gone. Instead, you see the road going on for several hundred yards before dissappearing into a thick woodland.
->Opening.Crossroads2

=Crossroads2
*[Follow the path into the hills]
->Opening.Hills
*[Follow the path into the woodland]
->Opening.Woodland

=Hills
{Opening.Crossroads2: You leave the wood behind you and venture into the hills.|Taking a brave step forward you venture into the hills.} The verdant mounds roll by, the grass occassionally marked by a tree or brush. At some point, you notice a figure in the distance. Another lonely traveler on the road? Whoever it is, they'll soon be within shouting distance. 
*[Raise your arm and wave]
You wave happily at the approachinig figure, although they don't return the gesture. 
->Opening.Hills2
*[Walk towards them]
You decide to take the initiative and walk out to meet them on the road. 
->Opening.Hills2
*(Boulder)[Await them patiently]
As luck would have it there's a nice boulder by the roadside. You decide to take a break while the stranger approaches. 
->Opening.Hills2

=Hills2
As the stranger comes closer into view, you feel as if something is a bit off. Their clothing seems to consist out of rags, and their left leg is dragging behind them. Are they hurt? You
*...run towards them to help[]. <>
    You quickly close the gap, the stranger only being a several feet away from you now, allowing you to clearly see their face. <>
    ->Hills3
*...cautiously await their approach[].  <>
{Hills.Boulder:The boulder you're sitting on is actually rather comfortable, so why get up? Nevertheless, you |You }ready yourself, just in case they're hostile. It hasn't happened often on the road, but it's always good to be careful, right? 
    The stranger draws nearer, now being only several feet away from you, allowing a clear view of their face. <>
    ->Hills3
*(run)...decide you'd rather pass on this chance encounter and run in the other direction[]. <>
Breaking into a sprint, you head away from whoever is coming up the road. But as you run, you hear frantic footsteps behind you... And they're catching up. A powerful blow hits you in the back, sending you sprawling to the ground. You turn around to face your would be attacker, now in clear view.
->Hills3

=Hills3
You recoil. Its face is a horrible contradiction. Its right half takes the shape of a beautiful young man, with blemishless skin and perfect features. The left half however, betrays what lies underneath: twisted, rusting metal. It looks at you and smiles, the teeth on its left side stretching disconcerningly further than the remains of its lips on the right would allow. A word forms into your mind:
    
    Wraith. 
    
    Panic sets in. You
    **(fight){aglue}fight[]. 
    With all your might you throw yourself at the horrid creature. As you collide with it, it feels as if you smack into a wall. You bounce of it and fall down to the ground{Hills2.run: once more}. <> 
    ->Hills4
    **(scream){aglue}scream<>
    , but no sound comes from your lips. <>
    ->Hills4
    **(run){aglue}run[].
    {Hills2.run:Perhaps against better judgement, you try to make another break for it. You have barely gotten to your feet before the creature knocks you down once more, a sharp pain shooting through your arm as you fall. Did you break something? |You turn and bolt away at full speed. The creature is much faster. A powerful blow hits you in the back, sending you sprawling to the ground. With what strength you have left, you turn to face it.} <> 
    ->Hills4
=Hills4
The wraith simply tilts its head at you, still wearing its grotesque smile, almost as if to mock your {Hills3.fight:bravery}{Hills3.run:speed}{Hills3.scream:terror}. Slowly, it starts to advance toward you. It raises its left arm, presumably to strike you with it. You shield yourself with your arms, although you know it will probably be for naught. You brace yourself, agonizing seconds ticking by as you wait for the end, and then-
        A roar. 
        A bestial growl reverberates through your body. You look up, just in time to see a bear collide forcefully with the creature. The beast tears into the wraith, somehow capable of tearing through metal as if it were cloth. Between the harsh sounds of ripping metal you hear the monster scream. Inhuman, horrible, but scream nonetheless. Then, silence. 
        The bear stands there, panting. Still catching his breath, he turns towards you. He slowly walks over, his mouth dripping with salliva. He sniffs you. His mouth, reeking with a nearly bloodlike metal smell, mere inches from your face. Without a sound, he opens his strong jaws and bites down on you-
//            ->Awakening
=Woodland
The wood looks more appealing and you set off in its direction at a trot. Before long, you find yourself surrounded by tall oaks, beeches and other greenery. A gentle breeze carries the song of a variety of birds. 
As you press on, your ears pick up another melody. Faintly at first, but with every step you hear it more clearly: a melancholic tune in a woman's voice. You decide to
*...look for the source[].
    Thankfully, the tune is not hard to track down. Your ears quickly lead you to a small, sunlit meadow. In its center you spot the music's source, although it's not quite what you expected: a small bird, draped in bright blue, orange and yellow feathers sits on a small boulder. While her appearance and the sound she produces are a clear mismatch, she's unmistakenbly the one responsible for the song you hear. 
        **[Listen quietly]
        You stand there quietly, making sure not to disturb the creature as you enjoy her performance.
        **[Softly approach the bird]
        Ever so gently, you step forward towards the bird. Even though you try to move with utmost stealth, the grass of the meadow concealed a surprise. You hear and feel the snap of a twig under your foot. 
        The bird stops her song and turns to look at you, slightly tilting her head. Is she taking measure of you? 
        ...
        Perhaps you passed her test, for she gently resumes her song. 
        **[Throw something at it]
        You look for something to hurl at the beast. Why? Who knows. Maybe you mistrust a bird singing with a woman's voice. Maybe you dislike her singing. Or maybe you just want to see if you could. Regardless, you find a nice, chunky stone besides your foot. You pick it up, and with a fluid motion hurl it at the small thing. 
        With a crack, the stone hits the boulder, you missed. It is however enough to startle the bird, who flies up and out of the meadow. 
*...follow the path[].
*...turn back towards the plains[].

-> END