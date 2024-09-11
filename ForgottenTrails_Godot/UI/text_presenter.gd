extends RichTextLabel

#@onready var label = $TextPresenter

var fulltext = "Hello World!"  # Set this to the text you want to display
var currenttext = "" 

var typingspeed = 0.01
@export var timer: Timer  


@onready var audio_player: AudioStreamPlayer =$AudioStreamPlayer

signal finished_typing
var typing: bool = false

func present_console_message(content: String, warning: bool = false) -> void:
	if warning:
		push_warning("Warning from INK Script: " + content)
	else:
		print("Message from INK Script: " + content)

func present_story(content: String) -> void:
	# Prep Textbox
	self.clear()
	currenttext = ""
	
	# Load Text
	fulltext=content
	
	# Type Text
	typing=true
	var level = 0
	for n in fulltext: #Go through each character. (Old Loops: # while(text.length() < fulltext.length()): #while(typing):
		#timer.wait_time = typingspeed
		
		# Evaluate 'n'
		if n=='[':
			level +=1 # we are one more layer into brackets
		elif n==']':
			level -=1 # we are one less layer into brackets
		
		currenttext += n # place the letter
		
		if level>0:
			continue #skip delay, go to next character
		elif level<0: 
			push_warning("bracket depth error")
		else: # level==0
			self.set_text(currenttext) # display the added letter(s)
			audio_player.play() # play some audio
			timer.start(typingspeed) # start the delay TODO:make dependent on 'n'
			await timer.timeout # wait for the typing delay
	
	# Finish Text
	finish_text()


func finish_text():
	#TODO: Add finish line sound?
	
	self.set_text(fulltext) # set text in case we came here by skipping
	
	# stop typing
	timer.stop()
	typing=false
	finished_typing.emit() #give signal




