extends Control
# this scripts acts as a broker between the Slider object and the Settings script

@export var setting_reference:Settings.Keys

@onready var input:Control = get_node("Value Input")

var values:
	get:
		return Settings.setting_items[setting_reference]
var default_value:
	get:
		return Settings.setting_items[setting_reference].default_value
var saved_value:
	get:
		return Settings.setting_items[setting_reference].saved_value
	set(value):
		Settings.setting_items[setting_reference].saved_value = value

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

@export var revert_button:Button# = get_node("H/Revert")
func revert():
	input_value=saved_value

@export var apply_button:Button# = get_node("H/Apply")
func apply():
	log(input_value)
	saved_value=input_value
	change_applied.emit(input_value)
	check_buttons()

func check_buttons():
	print(saved_value)
	print(input_value)
	if saved_value != input_value:
		revert_button.show()
		apply_button.show()
	else:
		revert_button.hide()
		apply_button.hide()
	if saved_value != default_value:
		reset_button.show()
	else:
		reset_button.hide()

func _check_input():
	check_input(input_value)

func check_input(new_value):
	check_buttons()

signal change_applied(slider_value) 

func _on_change_applied():
	#TODO Room to add behaviour to see the new bheaviour from the changes, such as redrawing boxes with new opcaity
	pass


func _ready():
	input_value = saved_value
	if input is Range:
		input.min_value = values.min_value
		input.max_value = values.max_value
	elif input is OptionButton:
		pass
	check_buttons()
