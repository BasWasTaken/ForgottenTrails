extends Button

var index: int

func _ready():
	print("choice button ready")
	pressed.connect(_on_pressed)

func _on_pressed():
	print("pressed choice ", index)
	SignalBus.control_requests_choice.emit(index)
