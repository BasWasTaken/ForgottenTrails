extends Button
# make part of larger myInputReceiver?

signal continueSignal

func _on_pressed():
	# This connects the button to the storyGetter
	continueSignal.emit()

func _process(delta):
	# This connects the spacebar to the button:
	if Input.is_action_just_pressed("continue"): 
		_on_pressed()
