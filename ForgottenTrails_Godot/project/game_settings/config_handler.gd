extends Node
# TEST 20250113190200
class SettingObject:
	extends Object

enum SettingType {
	OptionButton,
	Range,
	CheckBox
}

class Setting_OptionButton: 
	extends SettingObject
	var type = SettingType.OptionButton
	var value_options: Array
	var default_value
	var live_value

	func _init(default, options: Array):
		self.value_options = options
		self.default_value = default
		self.live_value = default

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

	func _init(default: int, minimum: int= 0, maximum: int=100, step: int=1 ):
		self.minimum_value = minimum
		self.maximum_value = maximum
		self.step_size = step
		self.default_value = default
		self.live_value = default

class Setting_CheckBox:
	extends SettingObject
	var type = SettingType.CheckBox
	var default_value: bool
	var live_value: bool

	func _init(default: bool):
		default_value = default
		live_value = default

enum text_speed_preset{
	slow=1,
	mid=50,
	fast=100
}

const temporary_list: Array[String] = ["text_speed", "textbox_opacity", "master_volume", "full_screen"]

@export var setting_dictionary = {
	"text_speed": Setting_OptionButton.new(text_speed_preset.mid, [text_speed_preset.slow, text_speed_preset.mid, text_speed_preset.fast]),
	"textbox_opacity": Setting_Range.new(50, 0, 100, 1),
	"master_volume": Setting_Range.new(50, 0, 100, 1),
	"full_screen": Setting_CheckBox.new(false)
}

func get_default_value(id: String) -> Variant:
	if id in setting_dictionary:
		return setting_dictionary[id].default_value
	else:
		print("Invalid key:", id)
		return null

func get_range(id: String) -> Array:
	if id in setting_dictionary:
		if setting_dictionary[id].type == SettingType.Range:
			return [setting_dictionary[id].minimum_value, setting_dictionary[id].maximum_value, setting_dictionary[id].step_size]
		else:
			print("Key:", id, "is not a range setting")
			return []
	else:
		print("Invalid key:", id)
		return []

func get_options(id: String) -> Array:
	if id in setting_dictionary:
		if setting_dictionary[id].type == SettingType.OptionButton:
			return setting_dictionary[id].value_options
		else:
			print("Key:", id, "is not an option setting")
			return []
	else:
		print("Invalid key:", id)
		return []

func get_live_value(id: String) -> Variant:
	print("get_live_value called with key:", id)
	if id in setting_dictionary:
		print("Valid key:", id, "Value:", setting_dictionary[id].live_value)
		return setting_dictionary[id].live_value
	else:
		print("Invalid key:", id)
		return null

signal setting_changed(id: String, value: Variant)

func set_live_value(id: String, value: Variant):
	if id in setting_dictionary:
		setting_dictionary[id].live_value = value
		setting_changed.emit(id, value)
	else:
		print("Invalid key:", id)

# The Config file in memory 
@onready var storage = _get_or_create_config_file() # purely key and value needed

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
