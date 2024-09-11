extends RichTextLabel

#@onready var label = $TextPresenter

var fulltext = "Hello World!"  # Set this to the text you want to display
var currenttext = "" 
var currentindex = 0

var typingspeed = 0.1
@export var timer: Timer  # Declare a variable to hold the timer

#@export var sfx: AudioStream

@onready var audio_player: AudioStreamPlayer =$AudioStreamPlayer

func _ready():
	load_text(text)

func _on_timer_timeout():
	# Handle the timeout signal to update text
	if currentindex < len(fulltext):
		currenttext += fulltext[currentindex]
		#var playback: AudioStreamPlayback = sfx.instantiate_playback()
		audio_player.play()
		self.set_text(currenttext)
		currentindex += 1
	else:
		timer.stop()  # Stop the timer when the text is fully typed

func present_story(content: String) -> void:
	load_text(content)

func present_console_message(content: String, warning: bool = false) -> void:
	if warning:
		push_warning("Warning from INK Script: " + content)
	else:
		print("Message from INK Script: " + content)

func load_text(content: String):
	clear()
	currenttext = ""
	fulltext=content
	currentindex = 0
	timer.wait_time = typingspeed
	timer.start()  # Start the timer





