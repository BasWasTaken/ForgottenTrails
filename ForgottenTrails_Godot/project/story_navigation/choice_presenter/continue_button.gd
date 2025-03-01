extends Button


func _ready():
	pressed.connect(_on_pressed)

func _on_pressed():
	print("pressed continue")
	SignalBus.continue_button_pressed.emit()
