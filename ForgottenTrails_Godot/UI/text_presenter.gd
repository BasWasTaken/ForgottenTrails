extends RichTextLabel

#@onready var label = $TextPresenter

var fulltext = "Hello World!"  # Set this to the text you want to display
var currenttext = "" 
var currentindex = 0

var typingspeed = 0.1
@export var timer: Timer  


@onready var audio_player: AudioStreamPlayer =$AudioStreamPlayer

signal finished_typing
var busy: bool = false

func _ready():
	#load_text(text)
	pass

func _on_timer_timeout():
	# Handle the timeout signal to update text
	if currentindex < len(fulltext):
		
		audio_player.play() # play some audio
		
		currenttext += fulltext[currentindex] #display the next character
		self.set_text(currenttext)
		
		currentindex += 1 #move on to the next character
	else:
		finish_text()

func finish_text():
	#TODO: Add finish line sound?
	#currenttext=fulltext
	#currentindex=fulltext.length()-1
	#self.set_text(currenttext)
	
	self.set_text(fulltext)
	
	timer.stop()
	busy=false
	finished_typing.emit()

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
	busy=true








