extends CharacterBody3D

### Movement ###
const SPEED = 2.5
const JUMP_VELOCITY = 2
var can_move = true

### Camera ###
@onready var pivot = $pivot
const sens = .25
# sway #
const amp = .1
const vmod = .3
var sway_speed = .005

### hand terminal ###
const out_position = Vector3(0, 0.05, -0.58)
const away_position = Vector3(-1, 0, 0.6)
const HTspeed = 0.05
var HTactive = false

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	####################
	###### Camera ######
	####################
	if event is InputEventMouseMotion:
		pivot.rotate_x(-deg_to_rad(event.relative.y * sens))
		pivot.rotation_degrees.x = clamp(pivot.rotation_degrees.x, -90, 90)
		self.rotate_y(-deg_to_rad(event.relative.x * sens))

func _physics_process(delta: float) -> void:
	#########################
	####### Movement ########
	#########################
	var input_dir := Input.get_vector("left", "right", "forward", "back")
	var direction = (self.transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	
	if not is_on_floor():
		velocity += get_gravity() * delta
	elif Input.is_action_just_pressed("jump") && can_move:
		self.velocity.y = JUMP_VELOCITY

	if direction && can_move:
		velocity.x = direction.x * SPEED
		velocity.z = direction.z * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		velocity.z = move_toward(velocity.z, 0, SPEED)

	###################################
	####### Non-Movment Actions #######
	###################################
	
	if Input.is_action_just_pressed("ui_cancel"):
		get_tree().quit()

	### interact with objects ###
	var object = $pivot/interaction_ray.get_collider()
	if object && object.is_in_group("interactable") && Input.is_action_pressed("interact"):
		object.interacted()

	###################################
	########## Hand Terminal ##########
	###################################

	### toggle ###
	if Input.is_action_just_pressed("tab"):
		HTactive = !HTactive
		can_move = !HTactive

	match HTactive:
		true: 
			$Hand_Terminal.position = $Hand_Terminal.position.move_toward(out_position, HTspeed)
			$Hand_Terminal/LineEdit.grab_focus()
		false: 
			$Hand_Terminal.position = $Hand_Terminal.position.move_toward(away_position, HTspeed)
			$Hand_Terminal/LineEdit.release_focus()

	### send text ###
	$Hand_Terminal/SubViewport/CanvasLayer/AspectRatioContainer/TerminalFace.temp_text = $Hand_Terminal/LineEdit.text
	if Input.is_action_just_pressed("enter"):
		$Hand_Terminal/LineEdit.clear()

	###########################
	####### Camera Sway #######
	###########################
	
	var x = $pivot/Camera3D.position.x

	if direction && can_move:
		x += sway_speed
		x = clamp(x, -amp, amp)
	else:
		x = move_toward(x, 0, abs(sway_speed))

	if is_equal_approx(x, -amp) or is_equal_approx(x, amp):
		sway_speed *= -1

	$pivot/Camera3D.position.x = x
	$pivot/Camera3D.position.y = -(x ** 2.0) * vmod

	move_and_slide()
