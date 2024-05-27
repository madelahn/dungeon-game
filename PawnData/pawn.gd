class_name pawn

var _class = load("res://Classes/subclass.gd")
var _trait = load("res://traits.gd")

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
func _init(name, id):
	NAME = name
	ID = id


func get_base_stats():
	BASE_ATK = CLASS.get_atk_modifier() + TRAIT.ATK_MOD
	BASE_DEF = CLASS.get_def_modifier() + TRAIT.MGC_MOD
	BASE_MGC = CLASS.get_mgc_modifier() + TRAIT.MGC_MOD


