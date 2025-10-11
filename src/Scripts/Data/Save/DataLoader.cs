using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class DataLoader : Node
{
	private const string DATA_PATH = "res://src/Data/";
	
	private Dictionary<string, Race> races;
	private Dictionary<string, Trait> traits;
	private Dictionary<string, Class> classes;
	private Dictionary<string, Subclass> subclasses;
	
	private bool isLoaded = false;
	
	public override void _Ready()
	{
		LoadAllData();
	}
	
	public void LoadAllData()
	{
		races = LoadRaces();	
		traits = LoadTraits();
		classes = LoadClasses();
		subclasses = LoadSubclasses();
		
		isLoaded = true;
	}
	
	private Dictionary<string, Race> LoadRaces()
	{
		var raceDict = new Dictionary<string, Race>();
		string filePath = DATA_PATH + "races.json";
		
		if (!Godot.FileAccess.FileExists(filePath))
		{
			GD.PrintErr($"Races file not found: {filePath}");
			return raceDict;
		}
		
		using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		
		var options = new JsonSerializerOptions 
		{ 
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true
		};
		
		var jsonDict = JsonSerializer.Deserialize<Dictionary<string, RaceData>>(json, options);
		
		foreach (var kvp in jsonDict)
		{
			raceDict[kvp.Key] = new Race
			{
				Id = kvp.Key,
				Name = kvp.Value.Name,
				BaseStats = new Stats
				{
					Atk = kvp.Value.BaseStats.Atk,
					Def = kvp.Value.BaseStats.Def,
					Mgc = kvp.Value.BaseStats.Mgc
				}
			};
		}
		
		return raceDict;
	}
	
	private Dictionary<string, Trait> LoadTraits()
	{
		var traitDict = new Dictionary<string, Trait>();
		string filePath = DATA_PATH + "traits.json";
		
		if (!Godot.FileAccess.FileExists(filePath))
		{
			GD.PrintErr($"Traits file not found: {filePath}");
			return traitDict;
		}
		
		using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		
		var options = new JsonSerializerOptions 
		{ 
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true
		};
		
		var jsonDict = JsonSerializer.Deserialize<Dictionary<string, TraitData>>(json, options);
		
		foreach (var kvp in jsonDict)
		{
			traitDict[kvp.Key] = new Trait
			{
				Id = kvp.Key,
				Name = kvp.Value.Name,
				BaseStats = new Stats
				{
					Atk = kvp.Value.BaseStats.Atk,
					Def = kvp.Value.BaseStats.Def,
					Mgc = kvp.Value.BaseStats.Mgc
				}
			};
		}
		
		return traitDict;
	}
	
	private Dictionary<string, Class> LoadClasses()
	{
		var classDict = new Dictionary<string, Class>();
		string filePath = DATA_PATH + "classes.json";
		
		if (!Godot.FileAccess.FileExists(filePath))
		{
			GD.PrintErr($"Classes file not found: {filePath}");
			return classDict;
		}
		
		using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		
		var options = new JsonSerializerOptions 
		{ 
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true
		};
		
		var jsonDict = JsonSerializer.Deserialize<Dictionary<string, ClassData>>(json, options);
		
		foreach (var kvp in jsonDict)
		{
			classDict[kvp.Key] = new Class
			{
				Id = kvp.Key,
				Name = kvp.Value.Name,
				BaseStats = new Stats
				{
					Atk = kvp.Value.BaseStats.Atk,
					Def = kvp.Value.BaseStats.Def,
					Mgc = kvp.Value.BaseStats.Mgc
				}
			};
		}
		
		return classDict;
	}
	
	private Dictionary<string, Subclass> LoadSubclasses()
	{
		var subclassDict = new Dictionary<string, Subclass>();
		string filePath = DATA_PATH + "subclasses.json";
		
		if (!Godot.FileAccess.FileExists(filePath))
		{
			GD.PrintErr($"Subclasses file not found: {filePath}");
			return subclassDict;
		}
		
		using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		
		var options = new JsonSerializerOptions 
		{ 
			PropertyNameCaseInsensitive = true,
			AllowTrailingCommas = true
		};
		
		var jsonDict = JsonSerializer.Deserialize<Dictionary<string, SubclassData>>(json, options);
		
		foreach (var kvp in jsonDict)
		{
			subclassDict[kvp.Key] = new Subclass
			{
				Id = kvp.Key,
				Name = kvp.Value.Name,
				ParentClassId = kvp.Value.ParentClassId,
				StatModifiers = new Stats
				{
					Atk = kvp.Value.StatModifiers.Atk,
					Def = kvp.Value.StatModifiers.Def,
					Mgc = kvp.Value.StatModifiers.Mgc
				}
			};
		}
		
		return subclassDict;
	}
	
	// Public getters
	public bool IsLoaded() => isLoaded;
	
	public Dictionary<string, Race> GetRaces() 
	{
		GD.Print($"GetRaces called: races is {(races == null ? "NULL" : $"not null with {races.Count} items")}");
		return races;
	}
	
	public Dictionary<string, Trait> GetTraits() 
	{
		GD.Print($"GetTraits called: traits is {(traits == null ? "NULL" : $"not null with {traits.Count} items")}");
		return traits;
	}
	
	public Dictionary<string, Class> GetClasses() 
	{
		GD.Print($"GetClasses called: classes is {(classes == null ? "NULL" : $"not null with {classes.Count} items")}");
		return classes;
	}
	
	public Dictionary<string, Subclass> GetSubclasses() 
	{
		GD.Print($"GetSubclasses called: subclasses is {(subclasses == null ? "NULL" : $"not null with {subclasses.Count} items")}");
		return subclasses;
	}
	
	public Race GetRace(string id) => races.ContainsKey(id) ? races[id] : null;
	public Trait GetTrait(string id) => traits.ContainsKey(id) ? traits[id] : null;
	public Class GetClass(string id) => classes.ContainsKey(id) ? classes[id] : null;
	public Subclass GetSubclass(string id) => subclasses.ContainsKey(id) ? subclasses[id] : null;
}

public class RaceData
{
	public string Name { get; set; }
	public StatsData BaseStats { get; set; }
}

public class TraitData
{
	public string Name { get; set; }
	public StatsData BaseStats { get; set; }
}

public class ClassData
{
	public string Name { get; set; }
	public StatsData BaseStats { get; set; }
}

public class SubclassData
{
	public string Name { get; set; }
	public string ParentClassId { get; set; }
	public StatsData StatModifiers { get; set; }
}

public class StatsData
{
	public int Atk { get; set; }
	public int Def { get; set; }
	public int Mgc { get; set; }
}
