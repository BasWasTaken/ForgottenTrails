@tool
extends Control
class_name Setting
# Merged class handling both the UI elements and the settings logic.

@export var key_name: String # The name of the setting, needed to fetch from config manager.
@export var input: Control # The input control for changing the setting live_value.
@export var reset_button: Button # Reset button to reset the live_value.
@export var revert_button: Button # Revert button to revert the provisional live_value.
@export var apply_button: Button # Apply button to apply the provisional live_value.

func _ready():

	# Set input options based on the setting values
	if input is Range:
		input.value = live_value  # Set the range (slider) value
	elif input is OptionButton:
		input.selected = input.get_item_index(live_value)  # Set the selected item in OptionButton
	elif input is CheckBox:
		input.button_pressed = (live_value != 0)  # Set the button pressed state (assuming live_value is 1 for checked, 0 for unchecked)
	else:
		print("Unsupported input control type!")
	init()

	# Connect signals for buttons
	reset_button.pressed.connect(_on_reset_pressed)
	revert_button.pressed.connect(_on_revert_pressed)
	apply_button.pressed.connect(_on_apply_pressed)

	# Set the label text to match the key name
	var label: Label = get_node("Button Handler/Label")
	label.text = key_name
func init():
	# Initialize input_value with the current setting live_value
	input_value = live_value
	check_buttons()

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
		return ConfigHandler.get_live_value(self.key_name)
	set(value):
		ConfigHandler._change_live_value(self.key_name, value)

# Getter for the default live_value of the setting
var default_value:
	get:
		return ConfigHandler.get_default_value(self.key_name)



func _on_reset_pressed():
	input_value = default_value
	check_buttons()

func _on_revert_pressed():
	input_value = live_value
	check_buttons()

func _on_apply_pressed():
	live_value = input_value
	check_buttons()

var using_default: bool:
	get:
		return input_value == default_value

var change_pending: bool:
	get:
		return input_value != live_value

func check_buttons():
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

func check_input(new_value):
	#???
	check_buttons()
