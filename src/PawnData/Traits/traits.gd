class_name pawn_trait

var TRAIT : String
var ATK_MOD : int
var MGC_MOD : int
var DEF_MOD : int

# consider using weighted graph DS to better represent each trait & their relationships to others

func _init(name):
	TRAIT = name

func get_trait(selected_trait) -> pawn_trait:
	var file = FileAccess.open("res://src/PawnData/Traits/trait_instances.json", FileAccess.READ)
	var json_as_dict = JSON.parse_string(file)
	for key in json_as_dict:
		print(key)
		if json_as_dict[key].TRAIT == selected_trait:
			return pawn_trait.new(selected_trait)
	return null
