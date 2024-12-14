extends Screen


# UI Objects in Scene
@export var reset_button:Button# =$"Global Buttons/Apply All Button" #reset to defaults
@export var revert_button:Button#=$"Global Buttons/Revert All Button" #undo changes
@export var apply_button:Button#=$"Global Buttons/Apply All Button" #confirm, save and apply changes
@export var close_button:Button#=$"Global Buttons/Close Button"

var settings_all:
	get:
		#print("getting children")
		var list = []
		for child in get_all_children(self):
			#print(child)
			if child is SettingButtons: 
				#print(child)
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
		if !is_visible_in_tree():
			return false
		var changes_pending = []
		for setting in settings_all:
			if setting.change_pending:
				changes_pending.append(setting)
		return changes_pending.size() >0

func _ready():
	_init()
	#_on_open_or_close() #is probably already called by itself..?

func _process(delta):
	open_or_close()

func _init():
	for child in settings_all:
		#child.ready.connect(child.init)
		child.checked_buttons.connect(check_buttons)

func _on_open_or_close(): #on visibility changed
	#TODO fix issue where tab menu opens the first child on startup
	if is_visible_in_tree():
		_on_open()
	else:
		_on_close()

func _on_open():
	for child in settings_all:
		child.init()
	#check_buttons()

func _on_close():
	if(changes_are_pending):
		revert()


func open_or_close():
	if Input.is_action_just_pressed("menu"):
		if is_visible_in_tree():
			close_try()
		else: 
			open()

func open():
	_on_open()
	show()

func close_try():
	if not changes_are_pending:
		close_confirm()
	else:
		#TODO prompt first
		close_confirm()

func close_confirm():
	_on_close()
	hide()


func check_buttons(): 
	if changes_are_pending:
		reset_button.disabled = false #could also be hidden and shown. same effect, different representation
		revert_button.disabled = false
		apply_button.disabled = false
		close_button.disabled = true
	else:
		reset_button.disabled = false
		revert_button.disabled = true
		apply_button.disabled = true
		close_button.disabled = false

#TODO decide whether to use this looping method, or the one from config hasndler. define here  what things need saving depensing on the change pending check, or get from the handler based on what's active?
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
