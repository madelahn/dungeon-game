using Godot;
using System;

public partial class Pawn : Node
{
	public string Id { get; set; }
	public string Name { get; set; }
	public Race Race { get; set; }
	public Trait Trait { get; set; }
	public Class Class { get; set; }
	public Subclass Subclass { get; set; }
	public Stats Stats { get; set; }
	
	public int Level { get; set; }
	public string Team { get; set; }
	public PawnStatus Status { get; set; }
	//public List<Item> Equipment { get; set; }
	
	// Constructor
	public Pawn(
		string id, string name, Race race, Trait trait, Class classType, 
		Subclass subclass, int startingLevel = 1, string team = null,
		PawnStatus pawnStatus = PawnStatus.Neutral
	) {
		if (subclass.ParentClassId != classType.Id) {  // also changed to Id not id
			throw new ArgumentException($"{subclass.Name} is invalid for class {classType.Name}.");
		}
		
		Id = id;
		Name = name;
		Race = race;
		Trait = trait;
		Class = classType;
		Subclass = subclass;
		Level = startingLevel;
		Team = team;
		Status = pawnStatus;
		
		Stats = new Stats(race, trait, classType, subclass);
	}
}

public enum PawnStatus {
	Neutral,
	Happy,
	Sad
}

public struct Stats {
	public int Atk;
	public int Def;
	public int Mgc;
	
	// Constructor
	public Stats(Race race, Trait trait, Class classType, Subclass subclass)
	{
		Atk = race.BaseStats.Atk;
		Def = race.BaseStats.Def;
		Mgc = race.BaseStats.Mgc;
		
		Atk += trait.BaseStats.Atk;
		Def += trait.BaseStats.Def;
		Mgc += trait.BaseStats.Mgc;

		Atk += classType.BaseStats.Atk;
		Def += classType.BaseStats.Def;
		Mgc += classType.BaseStats.Mgc;

		Atk += subclass.StatModifiers.Atk;
		Def += subclass.StatModifiers.Def;
		Mgc += subclass.StatModifiers.Mgc;
	}
	
	public void ChangeStats(Stats change) {
		Atk += change.Atk;
		Def += change.Def;
		Mgc += change.Mgc;
	}
}

public class Race {
	public string Id { get; set; }
	public string Name { get; set; }
	public Stats BaseStats { get; set; }
}

public class Trait {
	public string Id { get; set; }
	public string Name { get; set; }
	public Stats BaseStats { get; set; }
}

public class Class {
	public string Id { get; set; }
	public string Name { get; set; }
	public Stats BaseStats { get; set; }
}

public class Subclass {
	public string Id { get; set; }
	public string Name { get; set; }
	public string ParentClassId { get; set; }
	public Stats StatModifiers { get; set; }
}
