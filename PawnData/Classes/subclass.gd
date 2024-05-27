extends main_class
class_name sub_class

var SUBCLASS_NAME : String = ""
var CLASS : main_class

func get_atk_modifier() -> int:
	return CLASS.ATK_MOD + ATK_MOD


func get_def_modifier() -> int:
	return CLASS.DEF_MOD + DEF_MOD


func get_mgc_modifier() -> int:
	return CLASS.MGC_MOD + MGC_MOD
