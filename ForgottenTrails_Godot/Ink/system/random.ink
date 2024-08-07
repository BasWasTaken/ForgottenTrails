=== function D(X) === // roll with a die of X sides
VAR result = 0
~result = RANDOM(1,X)
~Print(result)
~ return result

=== function D6() ===
~ return D(6)

=== function D20() ===
~ return D(20)

=== function D100() ===
~ return D(100)

=== function CheckFlat(odds) === //input the chance to succeed from 0-100% as odds.
VAR threshold = 101
~threshold-=odds
VAR roll = 0
~roll = D100()
{
- roll - odds*2 > 0:
    ~return 2
- roll - odds > 0:
    ~return 1
- else:
    ~return 0
}

=== function CheckSimple(odds, boon, boonX) === // additionally input a variable to use as a boost
~odds += boon * boonX
~return CheckFlat(odds)// same as doing a flat roll with higher odds (thus a lower threshold)

//could also write skillroll as D100() + skill - threshold 

=== function Check(odds, boon, boonX, bane, baneX) === // additionally input a variable to use as a hinderance
~ odds -= bane * baneX
~return CheckSimple(odds, boon, boonX) // same as doing a skill roll with lower odds (i.e. a higher threshold)