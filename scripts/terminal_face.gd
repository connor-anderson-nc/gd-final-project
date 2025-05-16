extends Control

@export var font_size: float

func _ready() -> void:
	write("Hello World!")
	$text_hist.add_theme_font_size_override("normal_font_size", font_size)
	$input_line/Label.add_theme_font_size_override("font_size", font_size)
	$input_line/LineEdit.add_theme_font_size_override("font_size", font_size)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if Input.is_action_just_pressed("tab"):
		$input_line/LineEdit.grab_focus()
	if Input.is_action_just_pressed("enter"):
		var text = $input_line/LineEdit.text
		write(text)
		
		#add proccessing the command here later


func write(text):
	$text_hist.text += text + "\n"
	if $input_line.position.y < 600:
		$input_line.position.y += font_size
