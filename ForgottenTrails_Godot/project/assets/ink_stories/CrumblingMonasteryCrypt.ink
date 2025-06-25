=== CrumblingMonasteryChurchCrypt
//General area settings
~SetLocation(LOC_CrumblingMonasteryChurchCrypt)
{->CrumblingMonasteryChurchCryptIntroduction|}
//Return visit text
->Actions

//Area Action System Prompts
=== CrumblingMonasteryChurchCryptUse
+{LightSource == 1 && LeftCrypt == 0}[Use the door]
You pull on the doorhandle which, when coaxed with a bit of force, gives way and accompanied by a characteristic creeking sound lets you open the door. Beyond it, you see a short hallway that quickly transitions into a steep stairway leading upward.
->CrumblingMonasteryChurchCryptLeave
+{LightSource == 0 && LeftCrypt == 0}[Use what you presume to be a doorhandle]
    You pull on the handle and with a creak the door gives way. Unfortunately, it is just as dark on the other side.
        ++[Step forward]
        You take a step into the darkness, the same type of flooring greeting your feet.
            +++[Keep going]
            You keep walking forward. While dark, this actually seems pretty doable? You set one foot in front of the other and make goo-
            //Add sound here
            With a yelp you trip and crash into hard, lined stone. It seems to feel like a staircase? What you also feel are painful arms and shins which are undoubtedly scraped and bruised from the fall. 
                ++++[Climb up the stairs]
                ~LeftCrypt = 1
                ->CrumblingMonasteryChurchMainHall
            +++[Turn back after all]
            An empty space in the dark? No thanks. You decide to turn back for now.
            ->Actions
        ++[Turn back]
        You decide not to venture forth just yet and, perhaps to create some sense of safety, pull the door closed.
        ->Actions
+[Nevermind]
->Actions

=== CrumblingMonasteryChurchExamine
+{LightSource == 1}[Examine the door]
The door seems a sturdy affair despite its cleary showing age. Its composed out of strong wooden planks (oak perhaps?) and bound by a solid metal.{LeftCrypt == 0: An iron door handle invites you to use it.} 
->Actions
+{LightSource == 1}[Examine the room]
The room gives a slightly oppressive atmosphere. Cobwebs connect the corners of the ceiling to one another, giving off the sense that it hasn't been actively used in quite some time. 
->Actions
+{LightSource == 1}[Examine the alcoves]
Most of the alcoves seem to have been cleared out long ago, as their empty shelves are covered in a thick layer of dust. A few still hold urns and small offerings, mainly in the shape of wooden effigies and half burned candles. 
~CryptEffigies = 1
->Actions
+{LightSource == 1 && CryptEffigies == 1}[Examine the effigies]
On several of the alcoves you find effigies made of wood. They seem to depict two variations of a draped figure, one with its arms held out welcomingly and another cradling a small figure, a babe perhaps. Some still hold the faintest hint of paint, but whatever detailing the crafter had once put into it has since been lost to time. 
->Actions
+{LightSource == 1 && CryptEffigies == 1}[Examine the coffins]
{You spot what appear to be two coffins in one of the alcoves. Both are rather plain, with little in the way of decoration. The one on the top shelf has had its lid smashed and appears to be empty inside. The one on the shelf bellow it, however, seems to be in tact.|The two coffins rest silently in the alcove}
    ++[Open the coffin]
    {You pull the lid off the coffin and place it against the wall next to the alcove. Peering inside you find, perhaps somewhat unsuprisingly, a skeleton. It strikes you as rather tall, perhaps belonging to a burly man once. Its clothes have mostly withered, barely covering the frame and with holes in several places.|You once again open the coffin, finding the skeleton inside} 
        +++[Examine the skeleton in detail]
        {CryptRing == 0:You inspect the body a bit further. Aside from the tattered cloth, you notice a gold band around its left hand ring finger.|You glance at your handiwork, {CryptFinger = 0:and reassuringly the man looks undisturbed.|the man's mangled hand resting without a ring and accompanying finger.}}
            ****[Take the ring]
            ~ Inventory += GoldRing
            ~CryptRing = 1
            It's not like he'll be using it anymore, right? Or that there's anyone here judging you for what you're about to do. You lean into the coffin and start to pry the ring off the dead mans hand. The finger proves brittle, and breaks off in the process. Detached from the hand, the ring comes off easily and finds its way into your pockets. The finger itself...
                *****You place back as neatly as possible in its original position.
                You may be a graverobber, but there's no reason to be disrespectful about it right? With some effort you manage to make the man's hand presentable again... Somewhat.
                ->Actions
                *****You drop on the floor.
                A crypt's a crypt right? As long as he's interred in the ground somewhere. A nice trinket richer, you return to your exploring.
                ->Actions
            ++++[Close the coffin]
            You place the lid back on the coffin and return the man to his rest.
        ->Actions
        +++[Close the coffin]
        You place the lid back on the coffin and return the man to his rest.
        ->Actions
    ++[Leave the coffin be]
    You decide not to disturb this resting place and return to the room.
    ->Actions
+{LightSource == 1}[Nevermind]
->Actions

+{LightSource == 0}[Try to examine the room]
{The room is obscured in deep darkness. As you carefully feel around, you feel the damp stone walls of the room pass by your fingers.|You stumble around in the darkness, trying to find something to grip. Suddenly, the wall gives way to a hole causing you to almost tumble in. {LitCrypt == 0:Steadying yourself, you feel around. It appears to be some sort of shelf in the stone, a wooden box atop it.|You must have fallen into one of the alcoves. Judging by the wood beneath your fingertips, this one seems to hold a coffin.}->CrumblingMonasteryChurchCryptBoxToggle|Continueing to rummage around in the dark, you trek your way along the walls. Eventually, stone makes way briefly for metal and then wood. {LitCrypt == 0:A door perhaps? ->CrumblingMonasteryChurchCryptDoorToggle|You seem to have found the door.}|You fumble around in the dark some more, but don't seem to find anything of interest.}
->Actions

+{LightSource == 0 && CryptBox == 1}[Examine the {LitCrypt == 0:box(?) on the wallshelf|coffins in the alcove}]
{CryptEffigies == 0:{You run your hands along the box. You feel a sharp prick as the polished wood gives way for rough splinters. Did someone break this? Thankfully, you seem to have avoided getting any splinters stuck in your hands.|Its probably better to avoid the broken box until you have a lightsource.}|You gingerly run your hands along the lid of the top coffin, mindful of the shattered wood, but you find nothing of interest there, nor on the lid of the bottom coffin. Presumably investigation will go better with a light.}
->Actions
+{LightSource == 0 && CryptDoor == 1}[Examine {LitCrypt == 0:what you presume to be a door|the door}]
Your hands feel up and down the wood, occasionally feeling the cold brush of metal. At just above hip height, your hand grasps around a handle, just begging to be used.
->Actions

=== CrumblingMonasteryChurchCryptMove
->Actions

//Area Specific Content
=== CrumblingMonasteryChurchCryptIntroduction
VAR LeftCrypt = 0
VAR LitCrypt = 0
VAR CryptBox = 0
VAR CryptDoor = 0
VAR CryptEffigies = 0
VAR CryptRing = 0
VAR CryptFinger = 0

You slowly wake. A dream after all. You find yourself face down on a damp, stone floor. Your head is pounding something fierce, as if to drive home the harsh divide between the realm of slumber and the one you currently find yourself in... Where are you anyway? The darkness stretches every way you look. 
->Actions

=== CrumblingMonasteryChurchCryptFirstLighting
~LitCrypt = 1
You fumble around in your pack for a moment, your hand quickly brushing against the familiar metal and glass of your trusty lantern. Habit makes the spark stone easy to find and with a few flicks of your thumb, the wick catches flame. 

The warm light engulfs the room, and you realize you find yourself in some form of crypt. A vaulted ceiling hangs low over the brooding room, its walls lined with alcoves meant for coffins and offerings, although most are empty. The room appears to be a dead-end, with only a single door set in one of the walls.
->Actions

=== CrumblingMonasteryChurchCryptLeave
+[Step into the hallway]
You leave the room behind you, stepping into the hallway and up the stairs.
~LeftCrypt = 1
->CrumblingMonasteryChurchMainHall
+[Look around the room a bit more first]
The hallway isn't going anywhere. You decide to close the door and explore here a bit more first. 
->Actions

=== CrumblingMonasteryChurchCryptBoxToggle
~CryptBox = 1
->Actions

=== CrumblingMonasteryChurchCryptDoorToggle
~CryptDoor = 1
->Actions

