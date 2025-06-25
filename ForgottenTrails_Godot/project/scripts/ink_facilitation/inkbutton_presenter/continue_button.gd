extends Button


func _ready():
	button_down.connect(SignalBus.ui_button_clicked.emit)
	#pressed.connect(_on_pressed)
	button_up.connect(SignalBus.ui_button_released.emit)
	grab_focus()

func _on_pressed():
	print("pressed continue")
	if printer_state.get_state == printer_state.VN_State.PRINTING:
		print("request skip")
		SignalBus.control_requests_skip.emit()
	elif printer_state.get_state != printer_state.VN_State.WAITING: 
		print("but it was illegal")
		return #other states are illegal (like locked or prosessing)
	#print("and it was legal")
	SignalBus.control_requests_continue.emit()

