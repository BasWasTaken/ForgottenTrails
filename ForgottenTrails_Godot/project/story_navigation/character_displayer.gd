extends Panel


@export var prefab : PackedScene

func clear():
	# Clear the current character display
	for child in get_children():
		if child.is_in_group("character"):
			child.queue_free()

func add_or_change_character(character : String, variant: String="not_specified", coords: Vector2=Vector2(-1, -1)):
	# Check if the character is already displayed
	var subject:CharacterPortrait = null
	for child in get_children():
		if child is CharacterPortrait and child.character == character:
			subject = child
			break
	if !subject:
		subject = prefab.instantiate()
		subject.name = character
		subject.character = character
		add_child(subject)
	# we now have a character instance

	# set the position
	if coords != Vector2(-1, -1):
		# Check if the position is valid
		if coords.x < 0 or coords.y < 0:
			print("Invalid position provided. Using default position.")
			coords = (self.size / 2) - (subject.size / 2)
		subject.position = coords

	# set the variant
	if variant != "not_specified":
		# get the image path
		var image_path = "res://project/assets/images/" + character + "_" + variant + ".png" #TODO whenever referring in code to asset pahts, this should go through some sort of global path helper, so you only have to update what the pahts to various folders are in one location
		if !ResourceLoader.exists(image_path):
			print("Image not found at path: " + image_path)
		subject.texture = load(image_path)	
		subject.variant = variant

func _input(event):
	if event.is_action_pressed("test_event_1"):
		add_or_change_character("gabriel", "happy", Vector2(randi_range(0, 1200), randi_range(0, 800)))
	
	if event.is_action_pressed("test_event_2"):
		add_or_change_character("gabriel", "angry", Vector2(randi_range(0, 1200), randi_range(0, 800)))
	
	if event.is_action_pressed("test_event_3"):
		add_or_change_character("gabriel", "sad", Vector2(randi_range(0, 1200), randi_range(0, 800)))

	if event.is_action_pressed("test_event_4"):
		add_or_change_character("brian", "happy", Vector2(randi_range(0, 1200), randi_range(0, 800)))
	
	if event.is_action_pressed("test_event_5"):
		add_or_change_character("brian", "angry", Vector2(randi_range(0, 1200), randi_range(0, 800)))
	
	if event.is_action_pressed("test_event_6"):
		add_or_change_character("brian", "sad", Vector2(randi_range(0, 1200), randi_range(0, 800)))