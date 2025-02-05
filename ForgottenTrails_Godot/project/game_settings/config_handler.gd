extends Node
# TEST 20250113190200
class SettingObject:
	var ref: choose
	pass

enum SettingType {
	OptionButton,
	Range,
	CheckBox
}

class Setting_OptionButton: 
	extends SettingObject
	var type = SettingType.OptionButton
	var value_options: Dictionary
	var default_value
	var live_value

	func _init(_ref:choose, default, options: Dictionary):
		ref = _ref
		self.value_options = options
		self.default_value = default
		self.live_value = default
		print(choose.keys()[ref], " created with default: ", default, " and options: ", options)

class Setting_Range:
	extends SettingObject
	var type = SettingType.Range
	var minimum_value: int
	var maximum_value: int
	var step_size: int
	var default_value: int:
		get:
			return default_value
		set(value):
			if value < minimum_value:
				default_value = minimum_value
			elif value > maximum_value:
				default_value = maximum_value
			else:
				default_value = value
	var live_value: int:
		get:
			return live_value
		set(value):
			if value < minimum_value:
				live_value = minimum_value
			elif value > maximum_value:
				live_value = maximum_value
			else:
				live_value = value

	func _init(_ref:choose, default: int, minimum: int= 0, maximum: int=100, step: int=1 ):
		ref = _ref
		self.minimum_value = minimum
		self.maximum_value = maximum
		self.step_size = step
		self.default_value = default
		self.live_value = default
		print(choose.keys()[ref], " created with default:", default, "min:", minimum, "max:", maximum, "step:", step)

class Setting_CheckBox:
	extends SettingObject
	var type = SettingType.CheckBox
	var default_value: bool
	var live_value: bool

	func _init(_ref:choose, default: bool):
		ref = _ref
		default_value = default
		live_value = default
		print(choose.keys()[ref], " created with default:", default)

enum choose{
	placeholder,
	text_speed,
	textbox_opacity,
	master_volume,
	full_screen
} 

# jezus wat zijn enums een gekut in godot
@export var setting_dictionary: Dictionary = {
	choose.keys()[choose.text_speed]: Setting_OptionButton.new(choose.text_speed, 50, {"slow":20, "mid":40, "fast":80}),
	choose.keys()[choose.textbox_opacity]: Setting_Range.new(choose.textbox_opacity, 50, 0, 100, 1),
	choose.keys()[choose.master_volume]: Setting_Range.new(choose.master_volume, 50, 0, 100, 1),
	choose.keys()[choose.full_screen]: Setting_CheckBox.new(choose.full_screen, false)
}


func get_default_value(id: String) -> Variant:
	assert(id in setting_dictionary, "Invalid option: " + id)
	return setting_dictionary[id].default_value

func get_range(id: String) -> Array:
	assert(id in setting_dictionary, "Invalid option: " + id)
	assert(setting_dictionary[id].type == SettingType.Range, "Key:"+ id+ "is not a range setting")
	return [setting_dictionary[id].minimum_value, setting_dictionary[id].maximum_value, setting_dictionary[id].step_size]


func get_options(id: String) -> Dictionary:
	assert(id in setting_dictionary, "Invalid option: " + id)
	assert(setting_dictionary[id].type == SettingType.OptionButton, "Key: " + id + " is not an option setting")
	return setting_dictionary[id].value_options

func get_live_value(id: String) -> Variant:
	#print("get_live_value called for the option:", id)
	assert(id in setting_dictionary, "Invalid option: " + id)
	#print("Valid choose:", id, "Value:", setting_dictionary[id].live_value)
	return setting_dictionary[id].live_value

signal setting_changed(id: String, value: Variant)

func set_live_value(id: String, value: Variant):
	assert(id in setting_dictionary, "Invalid option: " + id)
	setting_dictionary[id].live_value = value
	setting_changed.emit(id, value)

# The Config file in memory 
#@onready var storage = _get_or_create_config_file() # purely choose and value needed

func _get_or_create_config_file() -> ConfigFile:
	if false: #if any file is found on startup
		pass #get those values
		return null
	else:
		print('test! creating config file')
		return ConfigFile.new()

#register settings func..?

func write_to_disk():
	#TODO call this when saved, register all settings, and give the config file as one of the files to be persisted
	pass

func read_from_disk():
	#TODO
	pass
