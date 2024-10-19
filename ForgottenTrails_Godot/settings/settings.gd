extends Control

@onready var settings_dictionary = {"opacity": find_child("Opacity Setting"), "speed": find_child("Speed Setting")}

var typing_speed: float:
	get:
		return settings_dictionary["speed"].saved_value

var box_opacity: float:
	get:
		return settings_dictionary["opacity"].saved_value

@export var reset_button:Button# =$"Global Buttons/Apply All Button" #reset to defaults
@export var revert_button:Button#=$"Global Buttons/Revert All Button" #undo changes
@export var apply_button:Button#=$"Global Buttons/Apply All Button" #confirm, save and apply changes
@export var close_button:Button#=$"Global Buttons/Close Button"

#func _ready():
	#settings_dictionary = {"opacity": find_child("Opacity Setting"), "speed": find_child("Speed Setting")}

signal settings_changed

func receive_changed_setting():#TODO connext checkbutton function to be fired when receiving signal from any changes from the buttons
	settings_changed.emit()
	check_buttons()

var changes_pending:
	get:
		var list = []
		for setting in settings_dictionary:
			if settings_dictionary[setting].change_pending:
				list+=setting
		return list

var change_pending: bool:
	get:
		#for setting in dic.values():
			#if setting.change_pending:
				#return true;
		#return false
		return changes_pending>0

func check_buttons():
	if change_pending:
		reset_button.disabled = false
		revert_button.disabled = false
		apply_button.disabled = false
		close_button.disabled = true
	else:
		reset_button.disabled = true
		revert_button.disabled = true
		apply_button.disabled = true
		close_button.disabled = false

func read_setting(key:String):
	return settings_dictionary[key].value

#const _typing_delay_base_default:float = 0.01
#@export_range(0,1) var _typing_delay_base:float = _typing_delay_base_default:
	#set(new_value):
		#if _typing_delay_base != new_value:
			#_typing_delay_base = new_value
			#settings_changed.emit()
	#get:
		#return _typing_delay_base
#
#
#signal settings_changed


func _get_values():
	pass # Replace with function body.

func reset():
	for setting in changes_pending:
		setting.reset()
	check_buttons()

func revert():
	for setting in changes_pending:
		setting.revert()
	check_buttons()

func apply():
	for setting in changes_pending:
		setting.apply()
	check_buttons()
