extends ColorRect

var timer: Timer = Timer.new()


# Called when the node enters the scene tree for the first time.
func _ready():
	SignalBus.ink_func_fade_in.connect(fade_in)
	SignalBus.ink_func_fade_out.connect(fade_out)
	SignalBus.ink_func_fade_to_color.connect(fade_to_color)
	SignalBus.ink_func_effect.connect(fire_effect)


func fire_effect(prompt: String, duration: float=0.0):
	match prompt:
		"flash":
			flash()
		_:
			print("Effect not found")

func fade_out(black: bool=true, duration: float=0.0):
	if black:
		fade_to_color("black", duration)
	else:
		fade_to_color("white", duration)

func fade_in(duration: float=0.0):
	fade_to_color("transparent", duration)

func flash():
	fade_to_color("white", 0.1)
	fade_to_color("transparent", 0.1)

func fade_to_color(prompt: String, duration: float=0.0):
	var color = Color.from_string(prompt, Color.TRANSPARENT)
	assert(color!=null, "Color not valid")
	modulate = color





