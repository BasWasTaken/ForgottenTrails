extends ColorRect

var timer: Timer = Timer.new()


# Called when the node enters the scene tree for the first time.
func _ready():
	SignalBus.ink_func_fade_in.connect(fade_in)
	SignalBus.ink_func_fade_out.connect(fade_out)
	SignalBus.ink_func_fade_to_color.connect(fade_to_color)
	SignalBus.ink_func_effect.connect(fire_effect)
	SignalBus.ink_func_flash.connect(flash)


func fire_effect(prompt: String, duration: float=0.0):
	match prompt:
		"flash":
			flash()
		_:
			print("Effect not found")

func fade_out(black: bool=true, duration: float=0.0):
	if black:
		await FadeUtils.fade_color(self, Color(0, 0, 0, 1), duration)

	else:
		await FadeUtils.fade_color(self, Color(1, 1, 1, 1), duration)

func fade_in(duration: float=0.0):
	fade_to_color("transparent", duration)

func flash(prompt: String="white", amount: int=1):
	var s_color = self.modulate
	var t_color = Color.from_string(prompt, Color.TRANSPARENT)	
	assert(t_color!=null, "Color not valid")

	for i in range(amount):
		await FadeUtils.fade_color(self, t_color, 0.1)
		await get_tree().create_timer(0.01).timeout
		await FadeUtils.fade_color(self, s_color, 0.1)
		await get_tree().create_timer(0.01).timeout

func fade_to_color(prompt: String, duration: float=0.0):
	var color = Color.from_string(prompt, Color.TRANSPARENT)	
	assert(color!=null, "Color not valid")
	await FadeUtils.fade_color(self, color, duration)





