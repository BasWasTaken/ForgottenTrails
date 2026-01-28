INCLUDE Functions_Dev.ink

// opening
~FadeToWhite(0)
~FadeToImage(flower_gates,0)
~ print("Testing console log")
Once upon a time..
There was some text.
And more text.
In fact, there was such a huge amount of text it doesn't matter what's written here and it just serves to test what the textlogger looks like when it's full so just type and actually fuck this get some lorem ipsum up in here. 
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin sollicitudin mi felis, quis dictum justo tincidunt scelerisque. Donec quis tortor eu justo gravida bibendum quis quis urna. Ut sed orci orci. Nullam nec elementum tellus, at commodo neque. Nam interdum, turpis vitae blandit varius, erat libero varius purus, a vestibulum ligula nisi in urna. Ut volutpat, nisi et ullamcorper finibus, lectus arcu mollis nulla, quis rutrum ante arcu non sem. Morbi non nisl at ex vestibulum convallis et nec libero. Vivamus fermentum nulla et viverra mollis. Duis eleifend pulvinar odio, vitae varius magna congue in. Vestibulum ut erat nec nulla ultricies tempus. Donec euismod faucibus convallis. Fusce eu cursus augue.

Nulla at pharetra massa, quis convallis dui. Etiam ullamcorper interdum facilisis. Curabitur volutpat odio et justo aliquet lobortis. Sed condimentum convallis ligula, vel imperdiet augue aliquet id. Fusce egestas mauris arcu, in dapibus felis rutrum ornare. Etiam pellentesque ipsum lectus, at sodales magna tincidunt a. Nulla facilisi. Nullam libero tortor, faucibus ut sollicitudin ac, mattis id nunc. Mauris at massa vitae neque egestas consequat. Aenean vitae nunc blandit erat euismod aliquet vel id risus. Morbi elementum varius metus, id commodo ante porttitor at. Pellentesque nec porttitor leo. Mauris lacinia posuere elit, sed scelerisque eros posuere ut. Nunc at erat sodales nisi lobortis dapibus nec a ligula. Pellentesque malesuada porttitor bibendum. Aliquam eget consequat diam.

In vitae porta nisi. In nec viverra sem. Phasellus tincidunt consectetur augue. Suspendisse potenti. Cras tincidunt ac urna in tempor. Maecenas ac ante pulvinar, suscipit est id, lobortis eros. Aliquam venenatis ante magna, at auctor nibh porttitor eu. Aenean condimentum commodo sollicitudin. Phasellus eget diam fermentum dui egestas malesuada.

Nulla cursus nec lacus quis accumsan. Aliquam cursus risus elit. Suspendisse eget facilisis arcu. Phasellus pharetra justo metus, vitae ultricies lacus bibendum eu. Mauris id eleifend quam. Curabitur lacinia lacus tortor, ut mattis leo elementum non. Sed pellentesque tempus eleifend. Ut euismod lobortis quam eu dignissim. Mauris convallis et mauris ac interdum.

Etiam libero magna, tincidunt a rutrum sit amet, dignissim et orci. Suspendisse feugiat odio at lacus ultricies, a dapibus massa porta. Fusce vel egestas purus. Duis augue felis, dictum ut quam a, pulvinar eleifend mi. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque suscipit aliquam est, in gravida orci elementum ultricies. Nulla sit amet interdum augue. Suspendisse tortor risus, imperdiet ut leo eu, hendrerit imperdiet nibh. Etiam tempus augue mi, eu tincidunt mi vulputate vel.
~Ambiance_Play(river, 0.8)
Sound of a river.
~FadeIn(1)
Fadein.

// spriteboard 
~Spriteboard_Present(Brian, Happy, random)
Placing Brian somewhere.
~Spriteboard_Present(Gabriel, Happy, random)
Placing Gabriel somewhere.
~Ambiance_Play(chatter, 0.4)
Making more sound
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
~Spriteboard_Remove_All()
Removing all sprites.

// backdrops and visual effects
~FadeToBlack(0.25)
Fade to Black
~FadeToImage(swamp_house,0)
~FadeIn(0.25)
Swamp House, Fade in
~FadeToWhite(0.5)
Fade to White
~FadeToImage(flower_gates,0)
~FadeIn(0.5)
Flower gates, Fade in
~Flash("white", 1)
Flash!
~Flash("red", 3)

// sfx
~Sfx_Play(punch,0.3)
~Sfx_Play(punch,0.5)
~Sfx_Play(punch,0.8)
Punch sounds! (3, but you only here 2...)

// textbased functions
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

