extends Control
class_name setting_buttons
# this scripts acts as a broker between the object in scene and the script attached to it, containing a setting
# Contains both interface VIA THE EDITOR for devs to change defaults, (well, that's just in the setting scripts)
# as well as GUI for changing theplayerprefs through help of the config_handler. 


@export var setting: Setting:
	get:
		return get_parent()

@onready var input:Control = get_node("Value Input")

signal checked_buttons()

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
	set(value):
		if input is Range:
			input.value = value
		elif input is OptionButton:
			input.select(input.get_item_index(value)) # get index by inputtin id, i.e. value
		elif input is CheckBox:
			input.button_pressed=value
		else:
			print("Unrecognised setting type!")

var change_pending: bool:
	get:
		return setting.value!=input_value
		#return slider.is_node_ready() && saved_value!=slider_value

func _ready():
	init()

@export var reset_button:Button# = get_node("H/Reset")
func reset():
	input_value=setting.default_value
	check_buttons()

@export var revert_button:Button# = get_node("H/Revert")
func revert():
	input_value=setting.value
	check_buttons()

@export var apply_button:Button# = get_node("H/Apply")
func apply():
	ConfigHandler.save_setting_to_memory(setting,input_value)
	check_buttons()

func check_buttons():
	if input_value != setting.value:
		revert_button.show()
		apply_button.show()
	else:
		revert_button.hide()
		apply_button.hide()
	if input_value != setting.default_value:
		reset_button.show()
	else:
		reset_button.hide()
	checked_buttons.emit()

func check_input(new_value):
	check_buttons()

func init():
	input_value = setting.value
	check_buttons()
