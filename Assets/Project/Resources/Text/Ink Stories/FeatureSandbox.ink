INCLUDE CustomFeatures
->Start


=== Start ===
Write your text here!

LIST OnOffState = on, off
LIST ChargeState = uncharged, charging, charged

VAR PhoneState = (off, uncharged)

*	{PhoneState !? uncharged } [Plug in phone]
	~ PhoneState -= LIST_ALL(ChargeState)
	~ PhoneState += charging
	You plug the phone into charge.
*	{ PhoneState ? (on, charged) } [ Call my mother ]

-> END