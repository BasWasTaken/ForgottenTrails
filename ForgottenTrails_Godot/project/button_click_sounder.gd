extends Node

var button_click_sound: AudioStream = preload("res://project/system/button_click.mp3")
var button_release_sound: AudioStream = preload("res://project/system/button_release.mp3")

func _ready():
	# Connect the signal to the function
	# get the signal from godot's event system
	SignalBus.button_clicked.connect(_on_button_clicked)
	SignalBus.button_released.connect(_on_button_released)


func _on_button_clicked():
	# Play the button click sound
	AudioManager.play_audio(button_click_sound, "sfx")
	# Optionally, you can add more logic here if needed
func _on_button_released():
	# Play the button release sound
	AudioManager.play_audio(button_release_sound, "sfx")
	# Optionally, you can add more logic here if needed