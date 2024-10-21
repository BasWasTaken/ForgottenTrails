extends Control
# this scripts acts as a broker between the Slider object and the Settings script

@export var setting_reference:Settings.Keys

@onready var input:Control = get_node("Value Input")

var default_value:
	get:
		return Settings.setting_items[setting_reference].default_value
var saved_value:
	get:
		return Settings.setting_items[setting_reference].saved_value
var slider_value:
	get:
		return input.value

var change_pending: bool:
	get:
		return saved_value!=slider_value
		#return slider.is_node_ready() && saved_value!=slider_value

@export var reset_button:Button# = get_node("H/Reset")
func reset():
	slider_value=default_value

@export var revert_button:Button# = get_node("H/Revert")
func revert():
	slider_value=saved_value

@export var apply_button:Button# = get_node("H/Apply")
func apply():
	saved_value=slider_value
	setting_changed.emit(slider_value)

func _ready():
	slider_value = saved_value
	revert_button.hide()
	apply_button.hide()



func _on_value_changed(new_value):
	if saved_value != slider_value:
		revert_button.show()
		apply_button.show()
	else:
		revert_button.hide()
		apply_button.hide()
		

signal setting_changed(slider_value) #TODO Room to add behaviour to see the new bheaviour from the changes, such as redrawing boxes with new opcaity
