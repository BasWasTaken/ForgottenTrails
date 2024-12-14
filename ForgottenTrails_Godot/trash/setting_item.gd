@tool
extends Node
class_name Setting
# a convenient front for other classes to have a reference to one object as if that stores the data. in fact is all managed by confighandler

@export var key_name : String # The name of the setting, needed to fetch from config manager.
	#get:
		#return key_name
	#set(value):
		#label.text=value
		#key_name=value

# Getter for the current value of the setting
var value:
	get:
		return ConfigHandler.get_active_value(self.key_name)
	set(value):
		ConfigHandler.set_active_value(self.key_name, value)

# Getter for the default value of the setting
var default_value:
	get:
		return ConfigHandler.get_default_value(self.key_name)


@onready var label: Label = get_node("Button Handler/Label")
func _ready():
	label.text=key_name

# Constructor
func _init(name: String=key_name):
	self.key_name = name
