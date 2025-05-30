extends Node
class_name Item

var description: String
var mesh: Mesh
var ID: int

func _init(item_name: String, m: Mesh = Mesh.new(), id: int = randi_range(0, 9999)) -> void:
	self.name = item_name
	self.ID = id
