INCLUDE Functions_Dev.ink



~ print("Testing console log")
Once upon a time..

There was [b]bold[/b] text. There was [i]italics[/i] text. There was [u]underlined[/u] text. There was even [wave]wavy[/wave] text. There was even [shake]shaky[/shake] text. There was even [fade]fading[/fade] text. There was even [tornado]tornado-ing[/tornado] text? There was even [rainbow]rainbow[/rainbow] text.

There were thee choices. (Well, one decision with three options.)

 * There was the first choice.
 
 * There was the second choice.

 * There was the third choice.

- test


Testing some sprites on the spriteboard.
Placing Brian somewhere.
~Spriteboard_Present(Brian, Happy, random)
Placing Gabriel somewhere.
~Spriteboard_Present(Gabriel, Happy, random)
Moving Brian.
~Spriteboard_Move(Brian, top_left)
Moving Brian again.
~Spriteboard_Move(Brian, bottom_right)
Making Gabriel Sad.
~Spriteboard_Alter(Gabriel, Sad)
Making Gabriel Angry.
~Spriteboard_Alter(Gabriel, Angry)
Removing Brian.
~Spriteboard_Remove(Brian)
Moving Gabriel to 40, 60 and making him happy.
~Spriteboard_Present(Gabriel, Happy, "40,60")





Testing out some backdrops:
~FadeToBlack(1)
Fade to Black
~FadeToWhite(1)
Fade to White
~BackdropImage(swamp_house,0)
Swamp House
~FadeIn(1)
Fade in
~BackdropImage(flower_gates,0)
Flower gates
~Flash("white", 1)
Flash!
~Flash("red", 3)
Flash!


~ spd(4)

There was fast text. 

~ spd(0.25)

There was slow text.

~ spd(1)

There was normal text. 

They needed some more text afterwards.

And some more.

- They lived happily ever after.


    -> END

