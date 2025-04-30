extends Node

@onready var vox_player = $VOXPlayer
@onready var sfx_player = $SFXPlayer
@onready var ambient_player = $AmbiencePlayer
@onready var music_player = $MusicPlayer
@onready var sysfx_player = $SystemPlayer

func play_audio(stream: AudioStream, source: String):
	var player: AudioStreamPlayer
	match source:
		"vox":
			player = vox_player
		"sfx":
			player = sfx_player
		"ambient":
			player = ambient_player
		"music":
			player = music_player
		"sysfx":
			player = sysfx_player
	# Stop any currently playing audio #TODO only sometimes, sometimes you'll want it overlapping
	player.stop()
	# Set the new stream
	player.stream = stream
	# Play the audio
	player.play()

var button_click_sound: AudioStream = preload("res://project/system/button_click.mp3")
var button_release_sound: AudioStream = preload("res://project/system/button_release.mp3")

func _ready():
	# Connect the signal to the function
	# get the signal from godot's event system
	SignalBus.ui_button_clicked.connect(_on_button_clicked)
	SignalBus.ui_button_released.connect(_on_button_released)


func _on_button_clicked():
	# Play the button click sound
	AudioManager.play_audio(button_click_sound, "sfx")
	# Optionally, you can add more logic here if needed
func _on_button_released():
	# Play the button release sound
	AudioManager.play_audio(button_release_sound, "sfx")
	# Optionally, you can add more logic here if needed