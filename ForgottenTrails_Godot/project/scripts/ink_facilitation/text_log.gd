extends RichTextLabel

func _ready():
	clear()
	SignalBus.ink_sent_story.connect(log_story)
	SignalBus.ink_sent_choices.connect(log_choices)

func log_story(story: String):
	# Add the story text to the log
	append_text(story)

func log_choices(choices: Array[InkChoice]):
	# Add the choices to the log
	for choice in choices:
		append_text("\t") 
		append_text(choice.text)
		append_text("\n")  # Add a newline after each choice
	# TODO: indicate which choice was selected
