extends Node

# Store for the default values
var _default_settings : Dictionary = {
	"opacity": 0.5,  # Example default for transparency setting
	"volume": 100,        # Example default for volume
	"text_speed":speeds.mid
	# Add more defaults here as needed
}

# Store for the active settings
var _active_settings : Dictionary = {}

# The Config file in memory, storing changed settings
@onready var _stored_settings: ConfigFile = _get_or_create_config_file()

func _get_or_create_config_file():
	if false:
		pass
	else:
		_stored_settings = ConfigFile.new()

# Register a setting
func _register_setting(name : String, newValue=null):
	if !_default_settings.has(name):
		print("Define setting in default list first!")
		return

	if get_stored_value(name)!=null:
		newValue = _stored_settings[name]
	
	set_active_value(name, newValue)

# Set the active value for a setting to its default
func _reset_active_value(name:String):
	if _active_settings.has(name):
		_active_settings[name] = null

func _reset_active_values_all():
	for setting in _active_settings.keys:
		_reset_active_value(setting)

# Get the default value for a setting
func get_default_value(name : String) -> Variant:
	return _default_settings[name]

# Set the value of a setting
func set_active_value(name : String, value : Variant):
	_active_settings[name] = value

# Get the value of a setting
func get_active_value(name : String) -> Variant:
	if !_active_settings.has(name): #if not yet active...
		_register_setting(name) # activate from memory or default
	return _active_settings[name] if _active_settings[name]!=null else _default_settings[name] #return the stored if changed and default if no custom has been stored

# Store the active value for a setting
func _store_active_value(name : String):
	if _active_settings.has(name):
		_stored_settings.set_value(DataManager.player_name,name,_active_settings[name])

func _store_active_values_all():
	for setting in _active_settings.keys:
		_store_active_value(setting)

# Revert the active value back to equal what was stored (in settings)
func _revert_active_value(name : String):
	if _active_settings.has(name):
		_active_settings[name] = get_stored_value(name)

func _revert_active_values_all():
	for setting in _active_settings.keys:
		_revert_active_value(setting)

# Get the stored value for a setting
func get_stored_value(name : String, player_name : String = DataManager.player_name) -> Variant:
	return _stored_settings.get_value(player_name,name)

func write_to_disk():
	#TODO
	pass

func read_from_disk():
	#TODO
	pass


#---
enum speeds{
	slow=1,
	mid=50,
	fast=100
}
