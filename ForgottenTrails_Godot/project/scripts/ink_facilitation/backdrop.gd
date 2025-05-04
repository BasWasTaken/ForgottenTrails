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
	var search = load("res://project/assets/images/" + prompt + ".jpg")
	assert(search, "Image not found")
	img = search