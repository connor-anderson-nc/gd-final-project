extends Node
class_name computer

@export_category("system-info")
@export var sys_name: String
@export var os_name: String
@export var storage_space: int
@export var ram_name: String
@export var ram_capacity: int
@export var processor_name: String
@export var processor_capacity: int

#########################
####### file sys ########
#########################
func _ready() -> void:
	var root: dir = dir.new(null, "root")

class node:
	var parent: dir
	var f_name: String
	
	func _init(p, n) -> void:
		parent = p
		f_name = n

class dir extends node:
	var children = {}
	
	func add_child(child: node):
		children[child.f_name] = child

class file extends node:
	var data
