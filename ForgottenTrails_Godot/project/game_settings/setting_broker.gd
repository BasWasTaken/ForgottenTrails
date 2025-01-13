@tool
extends Control
class_name Setting_Broker
# Merged class handling both the UI elements and the settings logic.

@export_enum("text_speed", "textbox_opacity", "master_volume", "full_screen") var key: String # The name of the setting, needed to fetch from config manager.
@export var input: Control # The input control for changing the setting live_value.
@export var reset_button: Button # Reset button to reset the live_value.
@export var revert_button: Button # Revert button to revert the provisional live_value.
@export var apply_button: Button # Apply button to apply the provisional live_value.

var input_value:
	get:
		if input is Range:
			return input.value
		elif input is OptionButton:
			return input.get_selected_id()
		elif input is CheckBox:
			return input.button_pressed
		else:
			print("Unrecognised setting type!")
			return null
	set(value):
		if input is Range:
			input.value = value
		elif input is OptionButton:
			input.select(input.get_item_index(value)) # get index by inputting ID, i.e. live_value
		elif input is CheckBox:
			input.button_pressed = value
		else:
			print("Unrecognised setting type!")

# Getter for the current (live) live_value of the setting
var live_value:
	get:
		return ConfigHandler.get_live_value(key)
	set(value):
		ConfigHandler.set_live_value(key, value)

# Getter for the default live_value of the setting
var default_value:
	get:
		return ConfigHandler.get_default_value(key)

var using_default: bool:
	get:
		return input_value == default_value

var change_pending: bool:
	get:
		return input_value != live_value

func _ready():
	# Set the label text to match the key name.
	var label: Label = get_node("Button Handler/Label")
	label.text = str(key)

	# Connect signals for the buttons.
	reset_button.pressed.connect(_on_reset_pressed)
	revert_button.pressed.connect(_on_revert_pressed)
	apply_button.pressed.connect(_on_apply_pressed)

	_init()

func _init():
	# Populate the input control with the setting's options.
	populate_input_options()

	# Set the input control to the live_value.
	input_value = live_value

	# Check the buttons to see if they should be visible.
	check_buttons()

func populate_input_options():
	if input is OptionButton:
		var options = ConfigHandler.get_options(key)
		for option in options:
			input.add_item(str(option), option)
	elif input is Range:
		var item = ConfigHandler.get_range(key)
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
	check_buttons()

func _on_revert_pressed():
	input_value = live_value
	check_buttons()

func _on_apply_pressed():
	live_value = input_value
	check_buttons()

func check_input(new_value):
	#???
	check_buttons()
