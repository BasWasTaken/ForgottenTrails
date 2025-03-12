extends Node

func _unhandled_input(event):
	# input that is not handled by a button or other control element, comes in here
	# process it, and call the appropriate signal
	print("input received: " + str(event))
	if event.is_action_pressed("quicksave"):
		SignalBus.control_requests_quicksave.emit()
	elif event.is_action_pressed("quickload"):
		SignalBus.control_requests_quickload.emit()
	elif event.is_action_pressed("skip"):# | StoryNavigator.text_presenter.typing: TEMPORARYLI DISABLED SKIPPING due to this not working without reference, en ik ga zometeen een state machine bouwen dus dan vind ik nu dit neerzetten niet worth.
		SignalBus.control_requests_skip.emit()
	else:
		if event.is_action_pressed("ui_accept"):
			SignalBus.control_requests_continue.emit()
		elif event.is_action_pressed("ui_cancel"):
			SignalBus.control_requests_cancel.emit()
		elif event.is_action_pressed("ui_down"):
			SignalBus.control_requests_next.emit() #hopyfully will be uunnessecary because it should be handled by ui navigation in godot
		elif event.is_action_pressed("ui_up"):
			SignalBus.control_requests_previous.emit() #hopyfully will be uunnessecary because it should be handled by ui navigation in godot
		elif event.is_action_pressed("options"): #TODO later expand this with a script that handles which guis to open and close when, with dedicated events with window parameter or the like
			SignalBus.control_requests_options.emit()
		# else:
		# 	for index in range(1, 10):
		# 		if event.is_action_pressed("choice_"+str(index)):
		# 			SignalBus.control_requests_choice.emit(index-1)
