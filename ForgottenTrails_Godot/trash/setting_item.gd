extends Node
class_name Setting
# a convenient front for other classes to have a reference to one object as if that stores the data. in fact is all managed by confighandler

var key_name : String # The name of the setting, needed to fetch from config manager.

# Getter for the current value of the setting
var value:
	get:
		return ConfigHandler.get_value(self.key_name)
	set(value):
		ConfigHandler.set_value(self.key_name, value)

# Getter for the default value of the setting
var default:
	get:
		return ConfigHandler.get_default(self.key_name)

# Constructor
func _init(name: String):
	self.key_name = name
