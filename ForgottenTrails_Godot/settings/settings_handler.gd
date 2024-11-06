extends Control


# UI Objects in Scene
@export var reset_button:Button# =$"Global Buttons/Apply All Button" #reset to defaults
@export var revert_button:Button#=$"Global Buttons/Revert All Button" #undo changes
@export var apply_button:Button#=$"Global Buttons/Apply All Button" #confirm, save and apply changes
@export var close_button:Button#=$"Global Buttons/Close Button"

func _ready():
	for child in settings_all:
		child.checked_buttons.connect(check_buttons)
	pass
	check_buttons()


var settings_all:
	get:
		print("getting children")
		var list = []
		for child in get_all_children(self):
			print(child)
			if child is setting_container: 
				print(child)
				list.append(child)
		return list

func get_all_children(node) -> Array:

	var nodes : Array = []

	for N in node.get_children():
		if N.get_child_count() > 0:
			nodes.append(N)
			nodes.append_array(get_all_children(N))
		else:
			nodes.append(N)
	return nodes
	
var changes_are_pending: bool:
	get:
		var changes_pending = []
		for setting in settings_all:
			if setting.change_pending:
				changes_pending.append(setting)
		return changes_pending.size() >0

func check_buttons(): 
	if changes_are_pending:
		reset_button.disabled = false #could also be hidden and shown. same effect, different representation
		revert_button.disabled = false
		apply_button.disabled = false
		close_button.disabled = true
	else:
		reset_button.disabled = true
		revert_button.disabled = true
		apply_button.disabled = true
		close_button.disabled = false



func reset():
	for setting in settings_all:
		setting.reset()
	check_buttons()

func revert():
	for setting in settings_all:
		setting.revert()
	check_buttons()

func apply():
	for setting in settings_all:
		setting.apply()
	check_buttons()
