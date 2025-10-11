using Godot;
using System;
using System.Collections.Generic;

public partial class NewGameManager : Node
{
	private SaveManager saveManager;
	private DataLoader dataLoader;
	private Guild currentGuild;
	
	private Dictionary<string, Race> races;
	private Dictionary<string, Trait> traits;
	private Dictionary<string, Class> classes;
	private Dictionary<string, Subclass> subclasses;
	
	public override void _Ready()
	{
		saveManager = GetNode<SaveManager>("/root/SaveManager");
		dataLoader = GetNode<DataLoader>("/root/DataLoader");
		
		CallDeferred(MethodName.LoadGameData);
	}
	
	// Create a brand new game
	public Guild CreateNewGame(string guildName, string bannerStyle = "default")
	{
		if (races == null || traits == null || classes == null || subclasses == null)
		{
			GD.PrintErr("Game data not loaded yet! Cannot create new game.");
			return null;
		}
		
		string guildId = Guid.NewGuid().ToString();
		var appearance = new GuildAppearance(bannerStyle);
		currentGuild = new Guild(guildId, guildName, appearance);
		
		currentGuild.Gold = 5000;
		currentGuild.Reputation = 0;
				
		AddStartingMembers();
		
		GD.Print($"New guild created: {guildName}");
		return currentGuild;
	}
	
	public bool SaveNewGame(string saveName)
	{
		if (currentGuild == null)
		{
			GD.PrintErr("No guild to save!");
			return false;
		}
		
		return saveManager.SaveGame(currentGuild, saveName);
	}
	
	private void AddStartingMembers()
	{
		if (races.ContainsKey("human") && 
			traits.ContainsKey("stinky") && 
			classes.ContainsKey("warrior") && 
			subclasses.ContainsKey("samurai"))
		{
			var starter = new Pawn(
				Guid.NewGuid().ToString(),
				"Aldric",
				races["human"],
				traits["stinky"],
				classes["warrior"],
				subclasses["samurai"],
				1,
				null,
				PawnStatus.Happy
			);
			
			currentGuild.AddMember(starter);
			GD.Print("Added starter member: Aldric");
		}
		else
		{
			GD.PrintErr("Could not find required data to create starter member");
		}
	}
	
	// Load game data from DataLoader
	private void LoadGameData()
	{		
		if (dataLoader == null)
		{
			GD.PrintErr("DataLoader is null, cannot load game data");
			return;
		}
		if (!dataLoader.IsLoaded())
		{
			CallDeferred(MethodName.LoadGameData);
			return;
		}
				
		races = dataLoader.GetRaces();
		traits = dataLoader.GetTraits();
		classes = dataLoader.GetClasses();
		subclasses = dataLoader.GetSubclasses();
		
		if (races == null || traits == null || classes == null || subclasses == null)
		{
			GD.PrintErr("Failed to load game data from DataLoader.");
			return;
		}
		
		GD.Print($"Game data loaded successfully: {races.Count} races, {traits.Count} traits, {classes.Count} classes, {subclasses.Count} subclasses");
	}
	
	// Helper method to create a new pawn
	public Pawn CreatePawn(string name, string raceId, string traitId, string classId, string subclassId)
	{
		var race = races.ContainsKey(raceId) ? races[raceId] : null;
		var trait = traits.ContainsKey(traitId) ? traits[traitId] : null;
		var classType = classes.ContainsKey(classId) ? classes[classId] : null;
		var subclass = subclasses.ContainsKey(subclassId) ? subclasses[subclassId] : null;
		
		if (race == null || trait == null || classType == null || subclass == null)
		{
			GD.PrintErr("Failed to create pawn: missing data");
			return null;
		}
		
		return new Pawn(
			Guid.NewGuid().ToString(),
			name,
			race,
			trait,
			classType,
			subclass
		);
	}
	
	// Getters for game data (useful for UI)
	public Dictionary<string, Race> GetRaces() => races;
	public Dictionary<string, Trait> GetTraits() => traits;
	public Dictionary<string, Class> GetClasses() => classes;
	public Dictionary<string, Subclass> GetSubclasses() => subclasses;
	public Guild GetCurrentGuild() => currentGuild;
}
