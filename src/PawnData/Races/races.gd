extends Node

var RACE_NAME : String

var ATK_MOD : int
var MGC_MOD : int
var DEF_MOD : int

var RACIAL_INTERACTIONS = {
	Human = 0,
	Elf = 0
}

func _init(name):
	RACE_NAME = name
