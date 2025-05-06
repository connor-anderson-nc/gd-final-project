extends Control

func _ready() -> void:
	write("Hello World!")

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
		$input_line.position.y += 20
