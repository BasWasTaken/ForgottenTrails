VAR DEBUG = false

=== developer_mode_toggle ===
// Developer mode adds a few shortcuts - remember to set to false in release!
~DEBUG = true
{
-DEBUG:
	IN DEVELOPER MODE!
    + [Proceed with Vugs' sequence]
        -> RuinedCoast
    + [Proceed to Character Creation]
    -> Opening

-else:
    -> Start
}