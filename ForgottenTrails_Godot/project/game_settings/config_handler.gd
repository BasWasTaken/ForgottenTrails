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
		#print(choose.keys()[ref], " created with default: ", default, " and options: ", options)

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
		#print(choose.keys()[ref], " created with default:", default, "min:", minimum, "max:", maximum, "step:", step)

class Setting_CheckBox:
	extends SettingObject
	var type = SettingType.CheckBox
	var default_value: bool
	var live_value: bool

	func _init(_ref:choose, default: bool):
		ref = _ref
		default_value = default
		live_value = default
		#print(choose.keys()[ref], " created with default:", default)

# Note: this is where to list all the settings that are available in the game. not sure if this is a good idea in the long run
enum choose{
	placeholder,
	text_speed,
	textbox_opacity,
	master_volume,
	system_volume,
	music_volume,
	ambiant_volume,
	sfx_volume,
	voice_volume,
	full_screen
} 

# jezus wat zijn enums een gekut in godot
# fuck i don;t think this is sound, would be better to have defaults defined separate from this, due to issues later in reading. nvm for now
@export var setting_dictionary: Dictionary = {
	choose.keys()[choose.text_speed]: Setting_OptionButton.new(choose.text_speed, 50, {"literally 1 cpm":1,"so zetta slow":6, "v slow":12, "slow":25, "mid":50,"fast":100,"v fast":200, "wa-jow":1000, "instant":0}), #in cps
	choose.keys()[choose.textbox_opacity]: Setting_Range.new(choose.textbox_opacity, 50, 0, 100, 1),
	choose.keys()[choose.master_volume]: Setting_Range.new(choose.master_volume, 50, 0, 100, 1),
	choose.keys()[choose.system_volume]: Setting_Range.new(choose.system_volume, 50, 0, 100, 1),
	choose.keys()[choose.music_volume]: Setting_Range.new(choose.music_volume, 50, 0, 100, 1),
	choose.keys()[choose.ambiant_volume]: Setting_Range.new(choose.ambiant_volume, 50, 0, 100, 1),
	choose.keys()[choose.sfx_volume]: Setting_Range.new(choose.sfx_volume, 50, 0, 100, 1),
	choose.keys()[choose.voice_volume]: Setting_Range.new(choose.voice_volume, 50, 0, 100, 1),
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

func _ready(): #TODO make sure this happens before other things
	setting_changed.connect(_on_setting_changed)
	state = File_State.flagged_for_reading
	# print("Flagged for reading")
	read_from_disk()

enum File_State{
	none,
	flagged_for_writing,
	writing,
	flagged_for_reading,
	reading
} 
var state: File_State = File_State.none: 
	get:
		return state
	set(value):
		print("State changed to:", value)
		state = value

func _on_setting_changed(id: String, value: Variant):
	print("Setting changed for:", id, "to:", value)
	if state == File_State.none:
		state = File_State.flagged_for_writing
		# print("Flagged for writing")
	
func _process(_delta):
	#print(state)
	if state == File_State.flagged_for_writing:
		write_to_disk()
	elif state == File_State.flagged_for_reading:
		read_from_disk()


# NOTE: MAYBE this could be done more dynamically, make a broadly scoped var, create or open the file while in settings, and anny changed that are applied, are saved to here, and when closed, the file is saved to disk?
# but i genuinely don't know if that's ANY better at all, and it seems a lot more work to set up, so should definitely not do it now.



@onready var config_file: ConfigFile = ConfigFile.new()

# Save it to a file (overwrite if already exists).
func write_to_disk(): # Q: should this go via the data manager?
	assert(state == File_State.flagged_for_writing, "Cannot write to disk without being flagged for writing")
	state = File_State.writing

	# Create new ConfigFile object.
	var saving_config = config_file
	
	# Store some values.	
	for setting in setting_dictionary:
		saving_config.set_value(DataManager.player_name, setting, setting_dictionary[setting].live_value)
	
	saving_config.save("user://config.cfg") # ask for overwrite if changes
	print("Saved to disk")
	state = File_State.none

func read_from_disk(): # Q: should this go via the data manager?
	assert(state == File_State.flagged_for_reading, "Cannot read from disk without being flagged for reading")
	state = File_State.reading
	
	var loaded_config = config_file
	# Load data from a file.
	var err = loaded_config.load("user://config.cfg")

	# If the file didn't load, ignore it.
	if err != OK:
		state = File_State.none
		print("Failed to read from disk with error ", err)
		return

	# ok so here is where it feels really logical to create a default data table for settings on this blank file. which i'm not doing. isntead ddefault data is always provieded at the start by initialisation of this script. which feels weird. i need to think this over later.

	# Iterate over all sections.
	for player in loaded_config.get_sections():
		# Check if the section is the player's.
		if player == DataManager.player_name:
			# Fetch the data for each section.
			for setting in setting_dictionary:
				if loaded_config.has_section_key(player, setting): #NOTE creating new might be neater than filling a generated one, but it's fine for now 20250205201951
					setting_dictionary[setting].live_value = loaded_config.get_value(player, setting)
				else:
					print("No value found for", setting)
					setting_dictionary[setting].live_value = setting_dictionary[setting].default_value

	print("Readed from disk")

	state = File_State.none



