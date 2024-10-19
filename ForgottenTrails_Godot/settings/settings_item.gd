extends Control

#TODO: should this be an interface with a generic type parameter?

@onready var slider:Slider = get_node("Value Slider")  #TODO: make this more generic than only applying to sliders?



const default_value:float = 0.5
@onready var saved_value:float = default_value
var slider_value:
	get:
		return slider.value 

var change_pending: bool:
	get:
		return saved_value!=slider_value
		#return slider.is_node_ready() && saved_value!=slider_value

#TODO do this not on ready but when the settings screen is opened?
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
		

signal setting_changed(slider_value)
