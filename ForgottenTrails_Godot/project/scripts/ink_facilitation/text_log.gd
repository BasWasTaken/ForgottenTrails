extends RichTextLabel
# TODO: make this a moveable window
# TODO: also create a text log on disk and write in that. expand to not only list the ink story there but also any significant console message, as an after action report for vugs
func _ready():
	clear()
	SignalBus.ink_sent_story.connect(log_story)
	SignalBus.ink_sent_choices.connect(log_choices)
	SignalBus.control_requests_choice.connect(log_decision)

func log_story(story: String):
	# Add the story text to the log
	append_text(story)

func log_choices(choices: Array):
	# Add the choices to the log
	for choice: InkChoice in choices:
		append_text("\t" + str(choice.Index) +") "+ choice.Text + "\n")  # Add a newline after each choice

func log_decision(index: int):
	append_text("\t (" + str(index)+")\n")
