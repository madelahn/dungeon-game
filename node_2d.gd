extends Node2D

var pawn = load("res://PawnData/pawn.gd")
var char = load("res://PawnData/Traits/traits.gd")

# Called when the node enters the scene tree for the first time.
func _ready():
	print("Create your pawn!")

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass


func _on_button_2_button_up():
	var input = get_node("intext").text
	# var _save_nodes = get_tree().get_nodes_in_group("Pawns")
	var selection = get_node("personality").get_selected_items()[0]
	var selected_trait = get_trait(selection)
	var id = get_new_id()
	var new_pawn = pawn.new(input, id, selected_trait)
	save_pawn(new_pawn)


func save_pawn(new_pawn):
	var file
	if (FileAccess.file_exists("res://pawns.json")):
		file = FileAccess.open("res://pawns.json", FileAccess.READ_WRITE)
		print("Adding new pawn")
		file.seek_end(-3)
		
		file.store_line(
			"\n\"" + str(new_pawn.ID) + "\": " \
			+ "{\"name\": " \
			+ "\"" + new_pawn.NAME + "\"," \
			+ "\"trait\": " \
			+ " \"" + new_pawn.TRAIT.TRAIT + "\"}," \
			+ "\n}"
		)
		
	else:
		file = FileAccess.open("res://pawns.json", FileAccess.WRITE)
		print("Making new pawns file!")
		file.store_line(
			"{\n" \
			+ "\"" + str(new_pawn.ID) + "\": " \
			+ "{\"name\": " \
			+ "\"" + new_pawn.NAME + "\"," \
			+ " \"trait\": " \
			+ "\"" + new_pawn.TRAIT.TRAIT + "\"}," \
			+ "\n}"
		)
	file.close()


func get_new_id():
	var json = FileAccess.get_file_as_string("res://pawns.json")
	if json == '':
		return 0
	else:
		var thing: Dictionary = JSON.parse_string(json)
		return thing.size()

func get_trait(selected_trait) -> pawn_trait:
	var file = FileAccess.get_file_as_string("res://PawnData/Traits/traitinstances.json")
	var json_as_dict = JSON.parse_string(file)
	for key in json_as_dict:
		if key == str(selected_trait):
			return pawn_trait.new(json_as_dict[key].TRAIT)
	return null

func _on_print_name():
	var input = get_node("intext").text
	if (len(input) < 1):
		print("Please enter at least one character.")
	else:
		print("Creating a new pawn.")
		print(input)

