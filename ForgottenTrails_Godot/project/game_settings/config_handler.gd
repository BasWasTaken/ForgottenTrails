extends Node
# Provides acces to user config in memory.
# Hands over config file to data manager for storage
# Creates an instance of default values for resetting and so that settings are never blank.

# THis should not be the primary way of getting setting values, because it is costly.
# So, only when the value is not present in the objects already. ANd mainly in the settings csreen probably.

# Not a pre-allocated file, but a growing file. Because I do not at this moment know all that should be listed on it.

var config = ConfigFile.new()


# get setting
# create if it does not yet exist
# with default based on..?


func get_setting():
	pass


func get_default_value(request:setting_name):
	return request.default_value

func get_value(request:setting_name):
	var result = _load_from_memory(request.name)
	if result != null:
		setting.stored_value = result #write retreived value as stored
		return result
	#else
	result = get_default_value(setting)
	if result!=null:
		setting.stored_value = result #write default value as stored because apperantly the player has not changed this
		return result
	else:
		print("CANNOT FIND SETTING" + setting.to_string())

func _load_from_memory(setting_name: String):
	if !config.get_section_keys(DataManager.player_name).has(setting_name):
		return null
	#TODO check (somehow) if you need to get from storage first
	return config.get_value(DataManager.player_name, setting_name)

func _read_file_from_storage():
	#TODO Get file from storage.
	#NOTE Use data_manager
	#NOTE also open the file up again on the name config
	pass

func save_to_memory(setting: Setting, value):
	config.set_value(DataManager.player_name, setting.name, value)
	setting._stored_value=value
	#NOTE should return some sucess code if no error

func _write_file_to_storage():
	#TODO Commit file to storage.
	#NOTE Use data_manager
	#NOTE also open the file up again on the name config
	pass

func _ready():
	config = _read_file_from_storage()
