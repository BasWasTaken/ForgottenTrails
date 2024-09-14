@tool
class_name TypeEffect
extends RichTextEffect
# Syntax: [type delay=0.0 speed=20.0]


var bbcode = "type"
var counter: int = 0


func _process_custom_fx(char_fx: CharFXTransform) -> bool:
	var speed: float = char_fx.env.get("speed", 20.0)
	var delay: int = char_fx.env.get("delay", 0.0)

	char_fx.visible = false

	if char_fx.elapsed_time * speed - delay >= char_fx.relative_index:
		char_fx.visible = true

	return true
