=== function d(X) === // roll with a die of X sides
VAR result = 0
~result = RANDOM(1,X)
~print(result)
~ return result

=== function d6() ===
~ return d(6)

=== function d20() ===
~ return d(20)

=== function d100() ===
~ return d(100)

=== function check_flat(odds) === //input the chance to succeed from 0-100% as odds.
VAR threshold = 101
~threshold-=odds
VAR roll = 0
~roll = d100()
{
- roll - odds*2 > 0:
    ~return 2
- roll - odds > 0:
    ~return 1
- else:
    ~return 0
}

=== function check_simple(odds, boon, boonX) === // additionally input a variable to use as a boost
~odds += boon * boonX
~return check_flat(odds)// same as doing a flat roll with higher odds (thus a lower threshold)

//could also write skillroll as d100() + skill - threshold 

=== function check(odds, boon, boonX, bane, baneX) === // additionally input a variable to use as a hinderance
~ odds -= bane * baneX
~return check_simple(odds, boon, boonX) // same as doing a skill roll with lower odds (i.e. a higher threshold)