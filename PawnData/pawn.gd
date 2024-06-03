class_name pawn

var _class = load("res://Classes/subclass.gd")
var _trait = load("res://PawnData/Traits/traits.gd")

var NAME : String = ""
var ID : int
var CLASS : sub_class
var TRAIT : pawn_trait

var BASE_ATK : int
var BASE_MGC : int
var BASE_DEF : int

var level
var team
var relationships
var status
var equipment

# Class Functions
func _init(name, id, personality):
	NAME = name
	ID = id
	TRAIT = personality


func get_base_stats() -> void:
	BASE_ATK = CLASS.get_atk_modifier() + TRAIT.ATK_MOD
	BASE_DEF = CLASS.get_def_modifier() + TRAIT.MGC_MOD
	BASE_MGC = CLASS.get_mgc_modifier() + TRAIT.MGC_MOD


func save_to_json() -> void:
	var file
	if (FileAccess.file_exists("res://pawns.json")):
		file = FileAccess.open("res://pawns.json", FileAccess.READ_WRITE)
		print("Adding new pawn")
		file.seek_end(-3)
		file.store_line(
			get_pawn_string(self)
		)
		
	else:
		file = FileAccess.open("res://pawns.json", FileAccess.WRITE)
		print("Making new pawns file!")
		file.store_line(
			"{\n" \
			+ get_pawn_string(self)
			+ "\n}"
		)
	file.close()

func get_pawn_string(pawn) -> String:
	return ("\n\"" + str(ID) + "\": " \
			+ "{\"name\": " \
			+ "\"" + NAME + "\", " \
			+ "\"trait\": " \
			+ "\"" + TRAIT.TRAIT + "\"}," \
			+ "\n}")
