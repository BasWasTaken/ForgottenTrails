extends Button

var index: int

func _ready():
	print("choice button ready")
	pressed.connect(_on_pressed)

func _on_pressed():
	print("pressed choice ", index)
	if printer_state.get_state == printer_state.PRINTING:
		print("request skip")
		SignalBus.control_requests_skip.emit()
	elif printer_state.get_state != printer_state.WAITING: #other states are illegal (like locked or prosessing)
		print("but it was illegal")
		return
	print("and it was legal")
	SignalBus.control_requests_choice.emit(index)
