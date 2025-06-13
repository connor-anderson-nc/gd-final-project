extends Node

func current():
	get_parent().write($computer.files.active_dir._name)

func cd(dir):
	var output = $computer.Change_dir(dir)
	get_parent().write(output)

func mkdir(name):
	var output = $computer.Make_dir(name)
	get_parent().write(output)

func rm(path):
	var output = $computer.Remove(path)
	get_parent().write(output)

func ls(dir = null):
	var output

	if dir != null:
		output = $computer.List(dir)
	else:
		output = $computer.List()

	for x in output:
		get_parent().write(x)
