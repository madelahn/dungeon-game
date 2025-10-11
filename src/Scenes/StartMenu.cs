using Godot;
using System;

public partial class StartMenu : Node
{	
	private void _on_start_button() 
	{
		GD.Print("Start game");
		
		// Access the autoload singleton - use /root/ not a file path
		var newGameManager = GetNode<NewGameManager>("/root/NewGameManager");
		
		if (newGameManager == null)
		{
			GD.PrintErr("NewGameManager not found!");
			return;
		}
		
		var guild = newGameManager.CreateNewGame("Guild Name");
		
		if (guild != null)
		{
			// Optionally save the new game
			newGameManager.SaveNewGame("Guild Name - New Save");
			
			// Change to the main game scene
			// GetTree().ChangeSceneToFile("res://scenes/main_game.tscn");
			GD.Print("New game created successfully!");
		}
		else
		{
			GD.PrintErr("Failed to create new game!");
		}
	}
	
	private void _on_load_button() 
	{
		GD.Print("Load button");
		
		// Example of how to load a game later
		var saveManager = GetNode<SaveManager>("/root/SaveManager");
		var allSaves = saveManager.GetAllSaves();
		
		if (allSaves.Count > 0)
		{
			GD.Print($"Found {allSaves.Count} save(s)");
			// You'd show these in a UI and let the player pick one
		}
		else
		{
			GD.Print("No saves found");
		}
	}
}
