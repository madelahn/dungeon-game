using Godot;
using System.Collections.Generic;

public partial class GameManager : Node
{
	private SaveManager saveManager;
	private Guild currentGuild;
	
	// You'll need to populate these with your game data
	private Dictionary<string, Race> races;
	private Dictionary<string, Trait> traits;
	private Dictionary<string, Class> classes;
	private Dictionary<string, Subclass> subclasses;
	
	public override void _Ready()
	{
		saveManager = new SaveManager();
		LoadGameData(); // Load races, traits, classes, etc.
	}
	
	// Start a new game
	public void StartNewGame(string guildName)
	{
		// Create new guild
		var appearance = new GuildAppearance("default");
		currentGuild = new Guild(System.Guid.NewGuid().ToString(), guildName, appearance);
		
		// Add starting resources
		currentGuild.Gold = 1000;
		currentGuild.Reputation = 0;
		
		// Maybe add some starting members or facilities
		AddStartingMembers();
		
		GD.Print($"New game started: {guildName}");
	}
	
	// Save current game
	public void SaveCurrentGame(string saveName)
	{
		if (currentGuild == null)
		{
			GD.PrintErr("No active guild to save");
			return;
		}
		
		bool success = saveManager.SaveGame(currentGuild, saveName);
		if (success)
		{
			GD.Print("Game saved successfully!");
		}
	}
	
	// Load a saved game
	public void LoadSavedGame(string saveId)
	{
		var saveData = saveManager.LoadGame(saveId);
		if (saveData == null)
		{
			GD.PrintErr("Failed to load save");
			return;
		}
		
		currentGuild = saveManager.ConvertSaveDataToGuild(
			saveData.Guild,
			races,
			traits,
			classes,
			subclasses
		);  
		
		GD.Print($"Game loaded: {currentGuild.Name}");
	}
	
	// Get all available saves for UI display
	public List<SaveData> GetAvailableSaves()
	{
		return saveManager.GetAllSaves();
	}
	
	private void LoadGameData()
	{
		// Initialize your game data here
		races = new Dictionary<string, Race>();
		traits = new Dictionary<string, Trait>();
		classes = new Dictionary<string, Class>();
		subclasses = new Dictionary<string, Subclass>();
		
		// Example: Load from JSON files or hardcode initial data
		// races["human"] = new Race { Id = "human", Name = "Human", BaseStats = ... };
	}
	
	private void AddStartingMembers()
	{
		// Add some default guild members when starting new game
		// This is just an example
	}
}
