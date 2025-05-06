extends CharacterBody3D

@onready var pivot = $pivot

const SPEED = 3.0
const JUMP_VELOCITY = 2
const sens = .25

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
		pass

	###########################
	####### Camera Sway #######
	###########################
	
	
	move_and_slide()
