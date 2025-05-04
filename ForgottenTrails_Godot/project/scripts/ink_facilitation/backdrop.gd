extends TextureRect


# Called when the node enters the scene tree for the first time.
func _ready():
	img = get_texture()
	SignalBus.ink_func_backdrop_image.connect(fade_to_image)

var img: Texture2D = Texture2D.new():
	get:
		return get_texture()
	set(value):
		set_texture(value)

func fade_to_image(prompt: String, duration: float=0.0):
	var search = load("res://project/assets/visual/backgrounds/img_bg_" + prompt + ".jpg") #allowing different images would be a matter of rewriting so that it does not always look in background folder
	assert(search, "Image not found")
	img = search