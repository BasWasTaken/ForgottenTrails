extends Node
# making this script receive input and send signals to the bus is a choice. certainly you could also do it by ust having the signals here and other things reaching out to this. both approached are valid i believe

func _unhandled_input(event):
	# input that is not handled by a button or other control element, comes in here
	# process it, and call the appropriate signal
	#print("input received: " + str(event))
	if event.is_action_pressed("quicksave"):
		print("input manager received quicksave")
		SignalBus.control_requests_quicksave.emit()
	elif event.is_action_pressed("quickload"):
		print("input manager received quickload")
		SignalBus.control_requests_quickload.emit()
	elif event.is_action_pressed("skip"):# | StoryNavigator.text_presenter.typing: TEMPORARYLI DISABLED SKIPPING due to this not working without reference, en ik ga zometeen een state machine bouwen dus dan vind ik nu dit neerzetten niet worth.
		print("input manager received skip")
		SignalBus.control_requests_skip.emit()
	else:
		# if event.is_action_pressed("ui_accept"):
		# 	print("input manager received accept")
		# 	SignalBus.control_requests_accept.emit()
		# elif event.is_action_pressed("ui_cancel"):
		# 	print("input manager received cancel")
		# 	SignalBus.control_requests_cancel.emit()
		if event.is_action_pressed("options"): #TODO later expand this with a script that handles which guis to open and close when, with dedicated events with window parameter or the like
			print("input manager received options")
			SignalBus.control_requests_options.emit()
		# else:
		# 	for index in range(1, 10):
		# 		if event.is_action_pressed("choice_"+str(index)):
		# 			SignalBus.control_requests_choice.emit(index-1)
