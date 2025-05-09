extends CharacterBody3D

### Movement ###
const SPEED = 2.5
const JUMP_VELOCITY = 2

### Camera ###
@onready var pivot = $pivot
const sens = .25
const amp = .1
const vmod = .3
var sway_speed = .005

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
	if not is_on_floor():
		velocity += get_gravity() * delta
	elif Input.is_action_just_pressed("jump"):
		self.velocity.y = JUMP_VELOCITY

	var input_dir := Input.get_vector("left", "right", "forward", "back")
	var direction = (self.transform.basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	if direction:
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

	var object = $pivot/interaction_ray.get_collider()
	if object && object.is_in_group("interactable"):
		object.interacted()

	###########################
	####### Camera Sway #######
	###########################
	var x = $pivot/Camera3D.position.x

	if direction:
		x += sway_speed
		x = clamp(x, -amp, amp)
	else:
		x = move_toward(x, 0, abs(sway_speed))

	if is_equal_approx(x, -amp) or is_equal_approx(x, amp):
		sway_speed *= -1

	$pivot/Camera3D.position.x = x
	$pivot/Camera3D.position.y = -(x ** 2.0) * vmod


	move_and_slide()
