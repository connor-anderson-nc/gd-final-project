extends Area3D
class_name Interact

@export_category("room change")
@export var active: bool
@export var room: PackedScene

var custom: Callable

func _ready() -> void:
	self.add_to_group("interactable")
	
	if !custom || !active:
		push_warning("interactable without action")

func interacted() -> void:
	if custom:
		custom.call()

	# put pickup func here
	
	if active:
		get_tree().change_scene_to_packed(room)
