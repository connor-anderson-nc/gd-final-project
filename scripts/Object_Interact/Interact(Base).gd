extends Area3D
class_name Interact

@export var active: bool = true

@export_category("Room Change")
@export var change_active: bool
@export var room: PackedScene

@export_category("Pickup")
@export var Pickup_Active: bool
@export var Name: String = "item"
@export var Description: String
@export var Model: Mesh
@export var ID: int #4 digit ID, used mostly to identify keys and similar items

var custom: Callable

func _ready() -> void:
	self.add_to_group("interactable")

	match true:
		custom: pass
		Pickup_Active: pass
		change_active: if !room: push_error(self.name + ": scene change is active but no new scene is set")
		_: push_warning("interactable without action:" + self.name)

func interacted() -> void:
	if !active:
		return

	if Pickup_Active:
		pass

	if custom:
		custom.call()

	if change_active:
		get_tree().change_scene_to_packed(room)
