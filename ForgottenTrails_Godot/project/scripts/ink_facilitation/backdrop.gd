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
	var image_path_without_extension = "res://project/assets/visual/backgrounds/img_bg_" + prompt
	var image_path = image_path_without_extension + ".png" #TODO make this a helper function that checks for the file extension and returns the correct one, so that it can be used in other places as well
	if !ResourceLoader.exists(image_path):
		image_path = image_path_without_extension + ".jpg"
		if !ResourceLoader.exists(image_path):
			image_path = image_path_without_extension + ".jpeg"
			if !ResourceLoader.exists(image_path):
				image_path = image_path_without_extension + ".webp"
				if !ResourceLoader.exists(image_path):
					image_path = image_path_without_extension + ".bmp"
					if !ResourceLoader.exists(image_path):
						image_path = image_path_without_extension + ".tga"
						if !ResourceLoader.exists(image_path):
							push_error("Image not found at path: " + image_path_without_extension)
							img = null
							return
	# TODO make cleaner way to check multiple file extentions (and probably make in helper script)
	var image = load(image_path) #allowing different images would be a matter of rewriting so that it does not always look in background folder
	assert(image, "Image not found")
	img = image
