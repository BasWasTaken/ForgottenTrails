extends TabContainer

func _ready():
	# Connect the signal to handle tab changes
	tab_changed.connect(_on_visibility_changed)
	visibility_changed.connect(_on_visibility_changed)
	current_tab = -1

func _on_visibility_changed():
	if current_tab == -1 || is_visible_in_tree(): # if no tab is selected, do nothing
		#TODO set free the state
		hide()
		mouse_filter = Control.MOUSE_FILTER_IGNORE
	else:
		#TODO lock the ink interaction by changing the vn state here.
		#NOTE left to do after I make some sort of GUI manager to control what's visible and active
		show()
		mouse_filter = Control.MOUSE_FILTER_PASS