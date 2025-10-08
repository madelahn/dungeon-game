using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RandomPawn : Node2D
{
	private Pawn currentPawn;
	
	private List<Race> allRaces = new List<Race>();
	private List<Trait> allTraits = new List<Trait>();
	private List<Class> allClasses = new List<Class>();
	private List<Subclass> allSubclasses = new List<Subclass>();
	private List<string> allNames = new List<string>();
	
	public override void _Ready()
	{
		LoadNames();
		LoadTraits();
		LoadRaces();
		LoadClasses();
		LoadSubclasses();
	}
	
	public override void _Process(double delta)
	{
		// Update UI with current pawn info
		if (currentPawn != null)
		{
			GetNode<RichTextLabel>("generated_name").Text = currentPawn.Name;
			GetNode<RichTextLabel>("generated_trait").Text = currentPawn.Trait.Name;
			GetNode<RichTextLabel>("generated_race").Text = currentPawn.Race.Name;
		}
	}
	
	private void LoadNames()
	{
		var nameFile = FileAccess.Open("res://src/Data/names.txt", FileAccess.ModeFlags.Read);
		while (!nameFile.EofReached())
		{
			var line = nameFile.GetLine();
			if (!string.IsNullOrWhiteSpace(line))
			{
				allNames.Add(line.Trim());
			}
		}
		nameFile.Close();
		GD.Print($"Loaded {allNames.Count} names");
	}
	
	private void LoadTraits()
	{
		var traitFile = FileAccess.Open("res://src/Data/traits.json", FileAccess.ModeFlags.Read);
		var traitJson = traitFile.GetAsText();
		traitFile.Close();
		
		var traitDict = Json.ParseString(traitJson).AsGodotDictionary();
		
		foreach (var key in traitDict.Keys)
		{
			var data = traitDict[key].AsGodotDictionary();
			var trait = new Trait
			{
				Id = key.ToString(),
				Name = data["name"].ToString(),
				BaseStats = ParseStats(data["baseStats"].AsGodotDictionary())
			};
			allTraits.Add(trait);
		}
		GD.Print($"Loaded {allTraits.Count} traits");
	}
	
	private void LoadRaces()
	{
		// Similar to LoadTraits but for races
		var raceFile = FileAccess.Open("res://src/Data/races.json", FileAccess.ModeFlags.Read);
		var raceJson = raceFile.GetAsText();
		raceFile.Close();
		
		var raceDict = Json.ParseString(raceJson).AsGodotDictionary();
		
		foreach (var key in raceDict.Keys)
		{
			var data = raceDict[key].AsGodotDictionary();
			var race = new Race
			{
				Id = key.ToString(),
				Name = data["name"].ToString(),
				BaseStats = ParseStats(data["baseStats"].AsGodotDictionary())
			};
			allRaces.Add(race);
		}
		GD.Print($"Loaded {allRaces.Count} races");
	}
	
	private void LoadClasses()
	{
		var classFile = FileAccess.Open("res://src/Data/classes.json", FileAccess.ModeFlags.Read);
		var classJson = classFile.GetAsText();
		classFile.Close();
		
		var classDict = Json.ParseString(classJson).AsGodotDictionary();
		
		foreach (var key in classDict.Keys)
		{
			var data = classDict[key].AsGodotDictionary();
			var classData = new Class
			{
				Id = key.ToString(),
				Name = data["name"].ToString(),
				BaseStats = ParseStats(data["baseStats"].AsGodotDictionary())
			};
			allClasses.Add(classData);
		}
		GD.Print($"Loaded {allClasses.Count} classes");
	}
	
	private void LoadSubclasses()
	{
		var subclassFile = FileAccess.Open("res://src/Data/subclasses.json", FileAccess.ModeFlags.Read);
		var subclassJson = subclassFile.GetAsText();
		subclassFile.Close();
		
		var subclassDict = Json.ParseString(subclassJson).AsGodotDictionary();
		
		foreach (var key in subclassDict.Keys)
		{
			var data = subclassDict[key].AsGodotDictionary();
			var subclass = new Subclass
			{
				Id = key.ToString(),
				Name = data["name"].ToString(),
				ParentClassId = data["parentClass"].ToString(),
				StatModifiers = ParseStats(data["statModifiers"].AsGodotDictionary())
			};
			allSubclasses.Add(subclass);
		}
		GD.Print($"Loaded {allSubclasses.Count} subclasses");
	}
	
	private Stats ParseStats(Godot.Collections.Dictionary statsDict)
	{
		return new Stats
		{
			Atk = statsDict["atk"].AsInt32(),
			Def = statsDict["def"].AsInt32(),
			Mgc = statsDict["mgc"].AsInt32()
		};
	}
	
	private void OnCreatePawn()
	{
		var random = new Random();
		
		var randomName = allNames[random.Next(allNames.Count)];
		
		var randomRace = allRaces[random.Next(allRaces.Count)];
		
		var randomTrait = allTraits[random.Next(allTraits.Count)];
		
		var randomClass = allClasses[random.Next(allClasses.Count)];
		
		var validSubclasses = allSubclasses.Where(s => s.ParentClassId == randomClass.Id).ToList();
		var randomSubclass = validSubclasses[random.Next(validSubclasses.Count)];
		
		currentPawn = new Pawn(
			Guid.NewGuid().ToString(),
			randomName,
			randomRace,
			randomTrait,
			randomClass,
			randomSubclass,
			1
		);
		
		GD.Print($"Generated: {currentPawn.Name}");
		GD.Print($"Race: {currentPawn.Race.Name}, Class: {currentPawn.Class.Name} ({currentPawn.Subclass.Name})");
		GD.Print($"Trait: {currentPawn.Trait.Name}");
		GD.Print($"Stats - Atk: {currentPawn.Stats.Atk}, Def: {currentPawn.Stats.Def}, Mgc: {currentPawn.Stats.Mgc}");
	}
	
	private void OnSavePawn()
	{
		if (currentPawn == null)
		{
			GD.Print("No pawn to save!");
			return;
		}
		var savePath = "res://pawns.json";
		var allPawns = new Godot.Collections.Dictionary();
		
		if (FileAccess.FileExists(savePath))
		{
			var loadFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
			var existingJson = loadFile.GetAsText();
			loadFile.Close();
			
			var parsed = Json.ParseString(existingJson);
			if (parsed.VariantType != Variant.Type.Nil)
			{
				allPawns = parsed.AsGodotDictionary();
			}
		}
		
		var pawnData = new Godot.Collections.Dictionary
		{
			["id"] = currentPawn.Id,
			["name"] = currentPawn.Name,
			["raceId"] = currentPawn.Race.Id,
			["traitId"] = currentPawn.Trait.Id,
			["classId"] = currentPawn.Class.Id,
			["subclassId"] = currentPawn.Subclass.Id,
			["level"] = currentPawn.Level,
			["team"] = currentPawn.Team ?? "none",
			["status"] = currentPawn.Status.ToString()
		};
		allPawns[currentPawn.Id] = pawnData;
		
		var json = Json.Stringify(allPawns, "\t");
		var saveFile = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
		if (saveFile == null)
		{
			GD.PrintErr($"Failed to write to file: {savePath}");
			return;
		}
		saveFile.StoreString(json);
		saveFile.Close();
		GD.Print($"Saved pawn {currentPawn.Name} to {savePath}");
	}
}
