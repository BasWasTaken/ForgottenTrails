extends Control
# UI Objects in Scene
@export var reset_button:Button# =$"Global Buttons/Apply All Button" #reset to defaults
@export var revert_button:Button#=$"Global Buttons/Revert All Button" #undo changes
@export var apply_button:Button#=$"Global Buttons/Apply All Button" #confirm, save and apply changes
@export var close_button:Button#=$"Global Buttons/Close Button"

# NOTE | TODO (see 20250301143406 in obsidian): instead of is_visible_in_tree() is should probably make some state variable that tells me if it's open or close, active or inactive etc. low priority for now but might come up once I start making more screens. culd use a class to inherit from

var settings_all:
	get:
		#print("getting children")
		var list = []
		for child in get_all_children(self):
			#print(child)
			if child is Setting_Broker: 
				print(child)
				list.append(child)
		#print(list)
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


var changes_pending:
	get:
		if !is_visible_in_tree():
			return false
		var _pending = []
		for setting in settings_all:
			if setting.change_pending:
				_pending.append(setting)
		if _pending.size() > 0:
			return _pending
		else:
			return false

var deviations_from_defaults:
	get:
		if !is_visible_in_tree():
			return false
		var _deviations = []
		for setting in settings_all:
			if !setting.using_default:
				_deviations.append(setting)
		if _deviations.size() > 0:
			return _deviations
		else:
			return false

func _ready():
	pass
	#_on_open_or_close() #is probably already called by itself..?

func _process(_delta):
	open_or_close()


func open_or_close():
	if Input.is_action_just_pressed("menu"):
		if is_visible_in_tree():
			close_try()
		else: 
			open()

func open():
	show()
	_on_open()


func close_try():
	if not changes_pending:
		close_confirm()
	else:
		#TODO prompt first
		close_confirm()

func close_confirm():
	_on_close()
	hide()


func _on_open_or_close(): #on visibility changed
	#TODO fix issue where tab menu opens the first child on startup
	if is_visible_in_tree():
		_on_open()
	else:
		_on_close()



func _on_open():
	print("Settings Screen Opened")
	for child in settings_all:
		child.refresh_ui_element()
	check_buttons()

func _on_close():
	if(changes_pending):
		revert()


func _init():
	for child in settings_all:
		if child.is_ready():
			child.prepare_ui_element()


func check_buttons():
	if changes_pending:
		# allow reverts and applys
		revert_button.disabled = false
		apply_button.disabled = false
		# disallow close
		close_button.disabled = true
	else:
		# disallow reverts and applys
		revert_button.disabled = true
		apply_button.disabled = true
		
		# allow close
		close_button.disabled=false

	if deviations_from_defaults:
		reset_button.disabled = false 
	else:
		reset_button.disabled = true 

func reset():
	for setting in deviations_from_defaults:
		setting._on_reset_pressed() #NOTE: is it ok to use the on pressed or should this be own function, thus neseccisating second function per button?
	check_buttons()

func revert():
	for setting in changes_pending:
		setting._on_revert_pressed()
	check_buttons()

func apply():
	for setting in changes_pending:
		setting._on_apply_pressed()
	check_buttons()	
	#ConfigHandler.write_to_disk()
