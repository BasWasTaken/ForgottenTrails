extends Button


func _ready():
	pressed.connect(_on_pressed)

func _on_pressed():
	print("pressed continue")
	SignalBus.control_requests_continue.emit()
