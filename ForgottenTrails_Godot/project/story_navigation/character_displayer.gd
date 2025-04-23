extends Panel


@export var prefab : PackedScene

func find_character(character : String) -> CharacterPortrait:
	# Find the character in the current display
	for child in get_children():
		if child is CharacterPortrait and child.character == character:
			return child
	return null

func remove_all_characters():
	# Clear all characters from the display
	for child in get_children():
		if child is CharacterPortrait:
			child.queue_free()

func remove_character(character : String):
	# Remove the character from the display
	var subject:CharacterPortrait = find_character(character)
	if subject:
		subject.queue_free()
	else:
		print("Character not found: " + character)

func add_or_change_character(character : String, variant: String="not_specified", coords: Vector2=Vector2(-1, -1)):
	# Check if the character is already displayed
	var subject:CharacterPortrait = find_character(character)
	# If the character is not displayed, create a new instance
	if !subject:
		subject = prefab.instantiate()
		subject.name = character
		subject.character = character
		add_child(subject)
	# we now have a character instance

	# set the position
	if coords != Vector2(-1, -1):
		var abs_pos = calc_abs_coords(coords)
		subject.position = abs_pos
		# add half the height and width to the position to center it?
		subject.position-= (subject.size / 2)

	# set the variant
	if variant != "not_specified":
		# get the image path
		var image_path = "res://project/assets/images/" + character + "_" + variant + ".png" #TODO whenever referring in code to asset pahts, this should go through some sort of global path helper, so you only have to update what the pahts to various folders are in one location
		if !ResourceLoader.exists(image_path):
			print("Image not found at path: " + image_path)
		subject.texture = load(image_path)	
		subject.variant = variant


func calc_abs_coords(rel_coords: Vector2) -> Vector2:
	# Calculate the relative coordinates based on the screen size

	# first check validity of input
	# Check if the position is valid
	if !(rel_coords.x >= 0  and rel_coords.x <= 100 and rel_coords.y >= 0  and rel_coords.y <= 100):
		print("Invalid position provided. Using default position.")
		rel_coords = Vector2(50, 50)

	var draw_area: Vector2 = self.size
	#var screen_size = get_viewport().get_visible_rect().size
	var abs_coords = draw_area * rel_coords / 100

	return abs_coords




func _input(event):
	if event.is_action_pressed("test_event_1"):
		add_or_change_character("gabriel", "happy", Vector2(0,0))
	
	if event.is_action_pressed("test_event_2"):
		add_or_change_character("gabriel", "angry", Vector2(100, 100))
	
	if event.is_action_pressed("test_event_3"):
		add_or_change_character("gabriel", "sad", Vector2(randi_range(0, 100), randi_range(0, 100)))

	if event.is_action_pressed("test_event_4"):
		add_or_change_character("brian", "happy", Vector2(randi_range(0, 100), randi_range(0, 100)))
	
	if event.is_action_pressed("test_event_5"):
		add_or_change_character("brian", "angry", Vector2(randi_range(0, 100), randi_range(0, 100)))
	
	if event.is_action_pressed("test_event_6"):
		add_or_change_character("brian", "sad", Vector2(randi_range(0, 100), randi_range(0, 100)))
	
	if event.is_action_pressed("test_event_7"):
		remove_character("gabriel")
	if event.is_action_pressed("test_event_8"):
		remove_character("brian")
	if event.is_action_pressed("test_event_9"):
		remove_all_characters()
