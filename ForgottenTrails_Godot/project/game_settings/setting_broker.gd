#@tool
extends Control
class_name Setting_Broker
# Merged class handling both the UI elements and the settings logic.

@export var affected_setting: ConfigHandler.choose # The name of the setting, needed to fetch from config manager.
@onready var ref: String = ConfigHandler.choose.keys()[affected_setting] # The reference to the setting in the config manager, in string form
@export var input: Control # The input control for changing the setting live_value.
@export var reset_button: Button # Reset button to reset the live_value.
@export var revert_button: Button # Revert button to revert the provisional live_value.
@export var apply_button: Button # Apply button to apply the provisional live_value.

var input_value:
	get:
		assert(input != null, "Input control is null")
		if input is Range:
			return input.value
		elif input is OptionButton:
			return input.get_selected_id()
		elif input is CheckBox:
			return input.button_pressed
		else:
			assert(false, "Could not get unrecognised input type of" + str(input))
			return null
	set(value):
		assert(input != null, "Input control is null")
		if input is Range:
			input.value = value
		elif input is OptionButton:
			input.select(input.get_item_index(value)) # get index by inputting ID, i.e. live_value
		elif input is CheckBox:
			input.button_pressed = value
		else:
			assert(false, "Could not set unrecognised input type of" + str(input))

# # Getter for the current (live) live_value of the setting
# var live_value:
# 	get:
# 		print("Getting live value for: " + ref)
# 		return ConfigHandler.get_live_value(ref)
# 	set(value):
# 		ConfigHandler.set_live_value(ref, value)

# # Getter for the default live_value of the setting
# var default_value:
# 	get:
# 		return ConfigHandler.get_default_value(ref)

# Getter for the current (live) live_value of the setting
var live_value:
	get:
		assert(ref in ConfigHandler.setting_dictionary, "Invalid key: " + str(ref))
		return ConfigHandler.get_live_value(ref)
	set(value):
		assert(ref in ConfigHandler.setting_dictionary, "Invalid key: " + str(ref))
		ConfigHandler.set_live_value(ref, value)

# Getter for the default live_value of the setting
var default_value:
	get:
		assert(ref in ConfigHandler.setting_dictionary, "Invalid key: " + str(ref))
		return ConfigHandler.get_default_value(ref)

var using_default: bool:
	get:
		# first check if both values are set, if not, return false
		assert(input_value != null and default_value != null, "Input value or default value is null")
		# Check if the input_value is the same as the default_value.
		return input_value == default_value

var change_pending: bool:
	get:
		# first check if both values are set, if not, return false
		assert(input_value != null and live_value != null, "Input value or default value is null")

		# Check if the input_value is different from the live_value.
		return input_value != live_value

func _ready():
	if is_visible_in_tree():
		prepare_ui_element() # ja moet dit wel, of gewoon aangecalld vanaf boven?
		refresh_ui_element()

func prepare_ui_element(): # dit samenvoegen met eronder? gewoon in if block?
	if is_visible_in_tree():
		print("Preparing setting broker for: " + ref)
		# Set the label text to match the affected_setting name.
		print("Setting up setting broker for: " + ref)
		var label: Label = get_node("Button Handler/Label")
		label.text = str(ref)

		# Connect signals for the buttons.
		reset_button.pressed.connect(_on_reset_pressed)
		revert_button.pressed.connect(_on_revert_pressed)
		apply_button.pressed.connect(_on_apply_pressed)
		prepared = true
	refresh_ui_element()

var prepared: bool = false

func refresh_ui_element():
	if is_visible_in_tree():
		#print("Refreshing setting broker for: " + ref)
		if!prepared:
			prepare_ui_element()
		# Populate the input control with the setting's options.
		populate_input_options()

		# Set the input control to the live_value.
		input_value = live_value

		# Check the buttons to see if they should be visible.
		check_buttons()

func populate_input_options():
	if input is OptionButton:
		input.clear()
		var options: Dictionary = ConfigHandler.get_options(ref)
		for key in options.keys():
			#print("Adding option: " + key + " with value: " + str(options[key]))
			input.add_item(key, options[key])

	elif input is Range:
		var item = ConfigHandler.get_range(ref)
		#print("Setting range for: " + ref + " with min: " + str(item[0]) + " max: " + str(item[1]) + " step: " + str(item[2]))
		input.min_value = item[0]#.minimum_value
		input.max_value = item[1]#.maximum_value
		input.step = item[2]#.step_size
	elif input is CheckBox:
		# No additional setup needed for CheckBox.
		pass

func check_buttons():
	#check if we're active in the tree
	if is_visible_in_tree():
		if change_pending:
			revert_button.show()
			apply_button.show()
		else:
			revert_button.hide()
			apply_button.hide()
		if !using_default:
			reset_button.show()
		else:
			reset_button.hide()

func _on_reset_pressed():
	input_value = default_value
	check_buttons() #TODO: resolve fact you're creating many calls per framt to check the buttons

func _on_revert_pressed():
	input_value = live_value
	check_buttons()

func _on_apply_pressed():
	live_value = input_value
	check_buttons()

func check_input(_new_value):
	#???
	check_buttons()
