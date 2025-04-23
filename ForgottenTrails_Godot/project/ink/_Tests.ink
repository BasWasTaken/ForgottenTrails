INCLUDE Functions_Dev.ink


~FadeToWhite(0)
~ print("Testing console log")
Once upon a time..
~FadeIn(1)



Testing some sprites on the spriteboard.
~Spriteboard_Present(Brian, Happy, random)
Placing Brian somewhere.
~Spriteboard_Present(Gabriel, Happy, random)
Placing Gabriel somewhere.
~Spriteboard_Move(Brian, top_left)
Moving Brian.
~Spriteboard_Move(Brian, bottom_right)
Moving Brian again.
~Spriteboard_Alter(Gabriel, Sad)
Making Gabriel Sad.
~Spriteboard_Alter(Gabriel, Angry)
Making Gabriel Angry.
~Spriteboard_Remove(Brian)
Removing Brian.
~Spriteboard_Present(Gabriel, Happy, "40,60")
Moving Gabriel to 40, 60 and making him happy.





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


There were choices.

 * There was the first choice.
 
 * There was the second choice.

 * There was the third choice.

- There was a response.


There was [b]bold[/b] text. There was [i]italics[/i] text. There was [u]underlined[/u] text. There was even [wave]wavy[/wave] text. There was even [shake]shaky[/shake] text. There was even [fade]fading[/fade] text. There was even [tornado]tornado-ing[/tornado] text? There was even [rainbow]rainbow[/rainbow] text.


They needed some more text afterwards.

And some more.

- They lived happily ever after.


    -> END

