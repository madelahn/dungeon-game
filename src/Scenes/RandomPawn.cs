using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class RandomPawn : Node2D
{
	private Pawn currentPawn;
	private DataLoader dataLoader;
	
	private Dictionary<string, Race> allRaces;
	private Dictionary<string, Trait> allTraits;
	private Dictionary<string, Class> allClasses;
	private Dictionary<string, Subclass> allSubclasses;
	private List<string> allNames = new List<string>();
	
	public override void _Ready()
	{
		// Get the DataLoader autoload
		dataLoader = GetNode<DataLoader>("/root/DataLoader");
		
		if (dataLoader == null)
		{
			GD.PrintErr("DataLoader not found!");
			return;
		}
		
		// Get data from DataLoader
		allRaces = dataLoader.GetRaces();
		allTraits = dataLoader.GetTraits();
		allClasses = dataLoader.GetClasses();
		allSubclasses = dataLoader.GetSubclasses();
		
		// Still load names from file
		LoadNames();
		
		GD.Print($"Loaded {allRaces.Count} races, {allTraits.Count} traits, {allClasses.Count} classes, {allSubclasses.Count} subclasses, {allNames.Count} names");
	}
	
	public override void _Process(double delta)
	{
		if (currentPawn != null)
		{
			GetNode<RichTextLabel>("generated_name").Text = currentPawn.Name;
			GetNode<RichTextLabel>("generated_trait").Text = currentPawn.Trait.Name;
			GetNode<RichTextLabel>("generated_race").Text = currentPawn.Race.Name;
			GetNode<RichTextLabel>("generated_class").Text = currentPawn.Subclass.Name;
		}
	}
	
	private void LoadNames()
	{
		var nameFile = Godot.FileAccess.Open("res://src/Data/names.txt", Godot.FileAccess.ModeFlags.Read);
		if (nameFile == null)
		{
			GD.PrintErr("Failed to load names.txt");
			return;
		}
		
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
	
	private void OnCreatePawn()
	{
		if (allRaces == null || allTraits == null || allClasses == null || allSubclasses == null || allNames.Count == 0)
		{
			GD.PrintErr("Data not loaded yet!");
			return;
		}
		
		var random = new Random();
		
		// Get random values from dictionaries
		var randomName = allNames[random.Next(allNames.Count)];
		var randomRace = allRaces.Values.ElementAt(random.Next(allRaces.Count));
		var randomTrait = allTraits.Values.ElementAt(random.Next(allTraits.Count));
		
		// Filter out "special" class if it exists
		var validClasses = allClasses.Values.Where(c => c.Id != "special").ToList();
		if (validClasses.Count == 0)
		{
			GD.PrintErr("No valid classes found!");
			return;
		}
		var randomClass = validClasses[random.Next(validClasses.Count)];
		
		// Get valid subclasses for the chosen class
		var validSubclasses = allSubclasses.Values.Where(s => s.ParentClassId == randomClass.Id).ToList();
		
		if (validSubclasses.Count == 0)
		{
			GD.PrintErr($"No subclasses found for class {randomClass.Name}");
			return;
		}
		
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
		
		var savePath = "user://pawns.json"; // Changed to user:// for writable directory
		var allPawns = new Godot.Collections.Dictionary();
		
		if (Godot.FileAccess.FileExists(savePath))
		{
			var loadFile = Godot.FileAccess.Open(savePath, Godot.FileAccess.ModeFlags.Read);
			if (loadFile != null)
			{
				var existingJson = loadFile.GetAsText();
				loadFile.Close();
				
				var parsed = Json.ParseString(existingJson);
				if (parsed.VariantType != Variant.Type.Nil)
				{
					allPawns = parsed.AsGodotDictionary();
				}
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
		var saveFile = Godot.FileAccess.Open(savePath, Godot.FileAccess.ModeFlags.Write);
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
