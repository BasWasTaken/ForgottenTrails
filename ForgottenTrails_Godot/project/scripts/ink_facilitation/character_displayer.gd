extends Panel


@export var prefab : PackedScene


func _ready():
	# Connect the signal to the function
	SignalBus.ink_func_sprite_present_by_string.connect(present_character_by_stringposition)
	#SignalBus.ink_func_sprite_move.connect(move_character)
	#SignalBus.ink_func_sprite_alter.connect(alter_character)
	SignalBus.ink_func_sprite_remove.connect(remove_character)
	SignalBus.ink_func_sprite_remove_all.connect(remove_all_characters)


func find_character(character : String) -> CharacterPortrait:
	# Find the character in the current display
	for child in get_children():
		if child is CharacterPortrait and child.character == character:
			return child
	return null

func remove_character(character : String):
	# Remove the character from the display
	var subject:CharacterPortrait = find_character(character)
	if subject:
		subject.queue_free()
	else:
		print("Character not found: " + character)

func remove_all_characters():
	# Clear all characters from the display
	for child in get_children():
		if child is CharacterPortrait:
			child.queue_free()

func present_character(character : String, variant: String="not_specified", coords: Vector2=Vector2(-1, -1)):
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
	if variant != "not_specified" && variant != "":
		# get the image path
		var image_path = "res://project/assets/visual/sprites/characters/img_char_" + character + "_" + variant + ".png" #TODO whenever referring in code to asset pahts, this should go through some sort of global path helper, so you only have to update what the pahts to various folders are in one location
		if !ResourceLoader.exists(image_path):
			print("Image not found at path: " + image_path)
		subject.texture = load(image_path)	
		subject.variant = variant

func present_character_by_stringposition(character : String, variant: String = "not_specified", location: String="not_specified"):
	# Present the character at a specified location
	var coords = parse_location(location)
	present_character(character, variant, coords)

# func move_character(character : String, coords: Vector2):
# 	# Move the character to a new position
# 	present_character(character, "not_specified", coords)

# func alter_character(character : String, variant: String):
# 	# Alter the character's variant
# 	present_character(character, variant, Vector2(-1, -1))

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

# misschien ooit zorgen dat bij plaatsen van nieuwe sprites er ruimte wordt gemaakt door andere dingen weg te scootchen, mar dat is even voor later.
# some location presets
var locations = {
	"top_left": Vector2(0, 0),
	"top_right": Vector2(100, 0),
	"bottom_left": Vector2(0, 100),
	"bottom_right": Vector2(100, 100),
	"center": Vector2(50, 50)
}

func parse_location(location: String) -> Vector2:
	# Parse the location string and return the corresponding coordinates
	if (location == "not_specified" || location == ""):
		return Vector2(-1, -1)
	elif location in locations:
		return locations[location]
	elif location == "random":
		return Vector2(randi_range(0, 100), randi_range(0, 100))
	else:
		# If the location is a valid Vector2 string, parse it
		var coords = location.split(",")
		if coords.size() == 2:
			return Vector2(coords[0].to_float(), coords[1].to_float())
		else:
			print("Invalid location format. Using default position.")
	return Vector2(50, 50)


# func _input(event):
# 	if event.is_action_pressed("test_event_1"):
# 		present_character("gabriel", "happy", Vector2(0,0))
	
# 	if event.is_action_pressed("test_event_2"):
# 		present_character("gabriel", "angry", Vector2(100, 100))
	
# 	if event.is_action_pressed("test_event_3"):
# 		present_character("gabriel", "sad", Vector2(randi_range(0, 100), randi_range(0, 100)))

# 	if event.is_action_pressed("test_event_4"):
# 		present_character("brian", "happy", Vector2(randi_range(0, 100), randi_range(0, 100)))
	
# 	if event.is_action_pressed("test_event_5"):
# 		present_character("brian", "angry", Vector2(randi_range(0, 100), randi_range(0, 100)))
	
# 	if event.is_action_pressed("test_event_6"):
# 		present_character("brian", "sad", Vector2(randi_range(0, 100), randi_range(0, 100)))
	
# 	if event.is_action_pressed("test_event_7"):
# 		remove_character("gabriel")
# 	if event.is_action_pressed("test_event_8"):
# 		remove_character("brian")
# 	if event.is_action_pressed("test_event_9"):
# 		remove_all_characters()
