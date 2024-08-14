extends RichTextLabel

#@onready var label = $TextPresenter

func present_story(content: String) -> void:	
	text=content

func present_console_message(content: String, warning: bool = false) -> void:
	if warning:
		push_warning("Warning from INK Script: " + content)
	else:
		print("Message from INK Script: " + content)

