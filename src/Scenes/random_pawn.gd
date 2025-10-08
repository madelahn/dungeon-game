extends Node2D

var gen_pawn = load("res://src/PawnData/pawn.gd")

var char = load("res://src/PawnData/Traits/traits.gd")

var char_name = ""
var trait_name: String
var char_trait: pawn_trait

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	get_node("generated_name").text = char_name
	get_node("generated_trait").text = char_trait.TRAIT if (char_trait != null) else ""


func _on_create_pawn():
	var rand = RandomNumberGenerator.new().randi_range(0, 2939)
	var namefile = FileAccess.open("res://src/names.txt", FileAccess.READ)
	for i in range(0, rand - 1):
		namefile.get_line()
	char_name = namefile.get_line()
	
	var trait_file = FileAccess.get_file_as_string("res://src/PawnData/Traits/trait_instances.json")
	var json_as_dict = JSON.parse_string(trait_file)
	rand = randi_range(0, 1)
	for key in json_as_dict:
		if key == str(rand):
			char_trait = pawn_trait.new(json_as_dict[key].TRAIT)


func _on_save_pawn():
	pass # Replace with function body.
