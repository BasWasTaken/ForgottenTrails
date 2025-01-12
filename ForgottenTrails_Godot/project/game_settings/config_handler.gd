extends Node

# Dictionary containing default keyvaluepairs, all to be set in the code before buiild
var _default_values : Dictionary = {
	"opacity": 0.5,  # Example default for transparency setting
	"volume": 100,        # Example default for volume
	"text_speed":speeds.mid
	# Add more defaults here as needed
}

# Dictionary containing active keyvaluepars, reflecting what's currently used by the system
var _live_settings: Dictionary = _default_values

# The Config file in memory 
@onready var storage = _get_or_create_config_file()

## dictionary to use for temporary storage while the player is changing settings in the menu. these won't have any effect yet, but they can be previewed later using sample sounds or timed signals briefly showing what the change would looke like 
#var _provisional_values : Dictionary = {}

func _get_or_create_config_file() -> ConfigFile:
	if false: #if any file is found on startup
		pass #get those values
		return null
	else:
		print('test! creating config file')
		return ConfigFile.new()

# Get the default value for a setting
func get_default_value(id : String) -> Variant:
	return _default_values[id]

## Get the provisional value for a setting
#func get_provisional_value(id : String) -> Variant:
#	if !_provisional_values.has(id):
#		_provisional_values[id] = get_live_value(id)
#	print("Provisional value for ", id, " received as :", _provisional_values[id]) # Debugging line
#	return _provisional_values[id]

## Set the provisional value for a setting
#func set_provisional_value(id : String, value : Variant):
#	print("Provisional value for ", id, " set to: ", _provisional_values[id]) # Debugging line
#	_provisional_values[id] = value

# Get the actual value of a setting
func get_live_value(id : String) -> Variant:
	print("Live value for ", id, " read as : ", _live_settings[id]) # Debugging line
	return _live_settings[id]# redundant since i won't work with a file of growing data adding to it every time and starting with null. that proved t convoluted  if _live_settings[id]!=null else _default_values[id] #return the stored if changed and default if no custom has been stored

## Set the active value for a setting to its default
#func _provisional_reset(id:String):
#	_provisional_values[id] = _default_values[id] #null would be cleaner, but also woudl require refresh on screen

#func _provisional_reset_all():
#	for setting in _provisional_values.keys:
#		_provisional_reset(setting)

# Apply the provisional value for a setting to be its live value
#func _apply_provisional_value(id : String):
#	_live_settings[id] = _provisional_values[id]
#	#signal to the game that the setting has changed
#	setting_changed.emit(id, _live_settings[id])

# Change a setting to a new value
func _change_live_value(id : String, value : Variant):
	_live_settings[id] = value
	#signal to the game that the setting has changed
	setting_changed.emit(id, _live_settings[id])

signal setting_changed(id : String, value : Variant)



#func _apply_provisional_value_all():
#	for setting in _provisional_values.keys:
#		_apply_provisional_value(setting)

# Revert the provisional value back to equal what the live value is
#func _revert_provisional_value(id : String):
#	_provisional_values[id] = get_live_value(id)

#func _revert_provisional_values_all():
#	for setting in _provisional_values.keys:
#		_revert_provisional_value(setting)
#	_provisional_values.clear() #TODO also call this when exiting the settings menu





func _register_setting(id : String):
	if _live_settings.has(id):
		return
	if DataManager.player_name in _live_settings:
		_live_settings[DataManager.player_name][id] = _default_values[id]
	else:
		_live_settings[DataManager.player_name] = {id : _default_values[id]}

func _register_all_settings():
	for setting in _live_settings.keys():
		_register_setting(setting)

func write_to_disk():
	#TODO call this when saved, register all settings, and give the config file as one of the files to be persisted
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
