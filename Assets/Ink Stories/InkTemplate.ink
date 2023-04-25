/*  
    This sample story contains an example of each custom function you have available to you (apart from all of Ink's innate features.
    
    Check out the documentation for tag use: 
    - https://github.com/inkle/ink/blob/master/Documentation/RunningYourInk.md#marking-up-your-ink-content-with-tags
    
    ther helpful links:
    - github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#5-functions
    - https://github.com/inkle/ink/blob/master/Documentation/WritingWithInk.md#part-5-advanced-state-tracking
    - https://www.patreon.com/posts/tips-and-tricks-18637020
*/
-> Start // keep this above the external function

EXTERNAL Print(string)

=== function Print(a)
    <i>Print to console:</i> {a}

=== Start ===
    ___________________________________________________________________________________________
    This is the beginning of the ink story. Feel free to edit anything after this point.
    #backdrop:whiterun // this should set the background
    #ambiance:chatter // this should set the ambiance
    #music:the streets of whiterun // this should set the music
    ~ Print("Hello world!")// This prints the text to the unity console
    // Later functions such as picking up items would ideally be handled in a similar matter, e.g. #pickup:sword or ~ Pickup("sword") depending on the implementation. I'll look at those in detail when we get to them and add them as we go along.
    #sfx:gong // this plays a sound. 
    -> LoremIpsum
=== LoremIpsum ===
    <br>
    Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc aliquam pellentesque ipsum scelerisque molestie. Nam condimentum neque non interdum feugiat. Curabitur eget dolor eget sapien condimentum aliquet. Donec mattis laoreet bibendum. Duis tincidunt egestas sem, sit amet tempor nunc varius luctus. Nam iaculis, lorem id ultrices tincidunt, enim tortor iaculis dui, eu consequat elit odio non massa. Suspendisse et sapien eu lorem gravida rhoncus. Nulla lorem nisl, pharetra sit amet bibendum vel, aliquet eu massa. Proin velit nunc, porttitor et ligula et, tincidunt rhoncus nisi. Maecenas nisi risus, laoreet eu commodo a, eleifend non odio. Sed eros sem, mollis sit amet elit vel, venenatis dapibus ipsum. Vivamus rhoncus malesuada dictum. 
    <br>
    Praesent bibendum sagittis velit, ut semper mi tristique nec. In venenatis nunc eu scelerisque eleifend. Donec elementum metus non ipsum accumsan tristique. Duis fermentum et ante et feugiat. Etiam ut lobortis lectus. Praesent nunc urna, faucibus quis pretium at, feugiat varius turpis. Etiam eget enim dictum, molestie turpis nec, posuere lacus. In et ante nec velit porttitor mollis et id ex. Nullam tincidunt iaculis dolor a sodales. Proin non tempor tellus. 
    <br>
    -> END