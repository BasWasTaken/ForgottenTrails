extends Button

@export var scene: PackedScene

func _ready():
	self.pressed.connect(launch)

func launch():
	scene.instantiate()
