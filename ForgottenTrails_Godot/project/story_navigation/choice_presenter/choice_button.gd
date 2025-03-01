extends Button

var index: int

func _ready():
	pressed.connect(_on_pressed)

func _on_pressed():
	SignalBus.choice_button_pressed.emit(index)
