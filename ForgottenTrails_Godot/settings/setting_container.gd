extends Control
class_name setting_container
# this scripts acts as a broker between the Slider object and the Settings script

@export var setting_reference:UserSettings.Keys

@onready var input:Control = get_node("Value Input")


func _ready():
	init()

var values:
	get:
		return UserSettings.setting_items[setting_reference]
var default_value:
	get:
		return UserSettings.setting_items[setting_reference].default_value
var saved_value:
	get:
		return UserSettings.setting_items[setting_reference].saved_value
	set(value):
		UserSettings.setting_items[setting_reference].saved_value = value

var input_value:
	get:
		if input is Range:
			return input.value
		elif input is OptionButton:
			return input.get_selected_id()
	set(value):
		if input is Range:
			input.value = value
		elif input is OptionButton:
			input.select(input.get_item_index(value)) # get index by inputtin id, i.e. value

var change_pending: bool:
	get:
		print(saved_value!=input_value)
		return saved_value!=input_value
		#return slider.is_node_ready() && saved_value!=slider_value

@export var reset_button:Button# = get_node("H/Reset")
func reset():
	input_value=default_value
	check_buttons()

@export var revert_button:Button# = get_node("H/Revert")
func revert():
	input_value=saved_value
	check_buttons()

@export var apply_button:Button# = get_node("H/Apply")
func apply():
	saved_value=input_value
	change_applied.emit()
	check_buttons()

signal checked_buttons()

func check_buttons():
	if input_value != saved_value:
		revert_button.show()
		apply_button.show()
	else:
		revert_button.hide()
		apply_button.hide()
	if input_value != default_value:
		reset_button.show()
	else:
		reset_button.hide()
	checked_buttons.emit()

func check_input(new_value):
	check_buttons()

signal change_applied() 

#@export var subject: Node # the object affected by this setting, if any- to be redrawn after a change

func _on_change_applied():
	#subject.init()
	pass



func init():
	input_value = saved_value
	if input is Range:
		input.min_value = values.min_value
		input.max_value = values.max_value
	elif input is OptionButton:
		pass
	check_buttons()
