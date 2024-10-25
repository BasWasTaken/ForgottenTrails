extends Node

# Script - Globally accessible
@onready var _opacity: setting_float=setting_float.new(.5,0,1)
@onready var _speed: setting_float=setting_float.new(50,0,100)

enum Keys{
	NA,
	test,
	opacity,
	speed
}

@onready var setting_items = {
	Keys.opacity:_opacity,
	Keys.speed:_speed
}
