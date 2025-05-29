extends RichTextLabel
# TODO: make this a moveable window
# TODO: also create a text log on disk and write in that. expand to not only list the ink story there but also any significant console message, as an after action report for vugs

var chosen: String = ""

func _ready():
	clear()
	SignalBus.ink_sent_story.connect(log_story)
	SignalBus.ink_sent_choices.connect(log_choices)
	#SignalBus.control_requests_choice.connect(log_decision) disabled, as this was creating ussues because this is not reliable caught before the next continue and choice presentation
# TODO: show choices in log again

func log_story(story: String):
	# Add the story text to the log
	#append_text(chosen)
	#chosen = ""
	append_text(story)

func log_choices(choices: Array):
	# Add the choices to the log
	append_text("\n")
	for choice: InkChoice in choices:
		append_text("\t" + str(choice.Index) +") "+ choice.Text + "\n") 
	append_text("\n")
	 
func log_decision(index: int):
	chosen = ("\t (" + str(index)+")\n")
