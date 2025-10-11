using Godot;
using System;
using System.Text.Json;
using System.Collections.Generic;

public partial class SaveManager : Node
{
	private const string SAVE_DIRECTORY = "res://saves/";
	private const string SAVE_EXTENSION = ".json";
	
	public SaveManager()
	{
		// Create saves directory if it doesn't exist
		if (!DirAccess.DirExistsAbsolute(SAVE_DIRECTORY))
		{
			DirAccess.MakeDirAbsolute(SAVE_DIRECTORY);
		}
	}
	
	// Save the current game state
	public bool SaveGame(Guild guild, string saveName)
	{
		try
		{
			var saveData = new SaveData
			{
				SaveName = saveName,
				SaveDate = DateTime.Now,
				Guild = ConvertGuildToSaveData(guild),
				CurrentScene = GetTree().CurrentScene.Name
			};
			
			string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions 
			{ 
				WriteIndented = true 
			});
			
			string filePath = SAVE_DIRECTORY + saveData.SaveId + SAVE_EXTENSION;
			
			using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Write);
			if (file == null)
			{
				GD.PrintErr($"Failed to open file for writing: {filePath}");
				return false;
			}
			
			file.StoreString(json);
			GD.Print($"Game saved successfully: {filePath}");
			return true;
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error saving game: {e.Message}");
			return false;
		}
	}
	
	// Load a game from save file
	public SaveData LoadGame(string saveId)
	{
		try
		{
			string filePath = SAVE_DIRECTORY + saveId + SAVE_EXTENSION;
			
			if (!Godot.FileAccess.FileExists(filePath))
			{
				GD.PrintErr($"Save file does not exist: {filePath}");
				return null;
			}
			
			using var file = Godot.FileAccess.Open(filePath, Godot.FileAccess.ModeFlags.Read);
			if (file == null)
			{
				GD.PrintErr($"Failed to open file for reading: {filePath}");
				return null;
			}
			
			string json = file.GetAsText();
			var saveData = JsonSerializer.Deserialize<SaveData>(json);
			
			GD.Print($"Game loaded successfully: {filePath}");
			return saveData;
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error loading game: {e.Message}");
			return null;
		}
	}
	
	// Get all available save files
	public List<SaveData> GetAllSaves()
	{
		var saves = new List<SaveData>();
		
		using var dir = DirAccess.Open(SAVE_DIRECTORY);
		if (dir == null)
		{
			GD.PrintErr($"Failed to open save directory: {SAVE_DIRECTORY}");
			return saves;
		}
		
		dir.ListDirBegin();
		string fileName = dir.GetNext();
		
		while (fileName != "")
		{
			if (!dir.CurrentIsDir() && fileName.EndsWith(SAVE_EXTENSION))
			{
				string saveId = fileName.Replace(SAVE_EXTENSION, "");
				var saveData = LoadGame(saveId);
				if (saveData != null)
				{
					saves.Add(saveData);
				}
			}
			fileName = dir.GetNext();
		}
		
		dir.ListDirEnd();
		return saves;
	}
	
	// Delete a save file
	public bool DeleteSave(string saveId)
	{
		try
		{
			string filePath = SAVE_DIRECTORY + saveId + SAVE_EXTENSION;
			
			if (!Godot.FileAccess.FileExists(filePath))
			{
				GD.PrintErr($"Save file does not exist: {filePath}");
				return false;
			}
			
			DirAccess.RemoveAbsolute(filePath);
			GD.Print($"Save deleted successfully: {filePath}");
			return true;
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error deleting save: {e.Message}");
			return false;
		}
	}
	
	// Convert Guild to SaveData
	private GuildSaveData ConvertGuildToSaveData(Guild guild)
	{
		var guildSave = new GuildSaveData
		{
			Id = guild.Id,
			Name = guild.Name,
			Level = guild.Level,
			Experience = guild.Experience,
			Gold = guild.Gold,
			Reputation = guild.Reputation,
			Appearance = new GuildAppearanceSaveData
			{
				Banner = guild.Appearance.Banner
			}
		};
		
		// Save members
		foreach (var pawn in guild.GetAllMembers())
		{
			guildSave.Members.Add(new PawnSaveData
			{
				Id = pawn.Id,
				Name = pawn.Name,
				RaceId = pawn.Race.Id,
				TraitId = pawn.Trait.Id,
				ClassId = pawn.Class.Id,
				SubclassId = pawn.Subclass.Id,
				Level = pawn.Level,
				Team = pawn.Team,
				Status = pawn.Status,
				Stats = new StatsSaveData
				{
					Atk = pawn.Stats.Atk,
					Def = pawn.Stats.Def,
					Mgc = pawn.Stats.Mgc
				}
			});
		}
		
		// Save parties
		foreach (var party in guild.GetAllParties())
		{
			guildSave.Parties.Add(new PartySaveData
			{
				Id = party.Id,
				Name = party.Name,
				MemberIds = new List<string>(party.MemberIds),
				Status = party.Status,
				MaxSize = party.MaxSize
			});
		}
		
		// Save facilities
		foreach (var facility in guild.GetAllFacilities())
		{
			guildSave.Facilities.Add(new FacilitySaveData
			{
				Id = facility.Id,
				Name = facility.Name,
				Type = facility.Type,
				Level = facility.Level,
			});
		}
		
		// Save relationships - you'll need to add a method to get all relationships
		// This is a simplified version
		
		return guildSave;
	}
	
	// Reconstruct Guild from SaveData
	public Guild ConvertSaveDataToGuild(GuildSaveData guildSave, 
		Dictionary<string, Race> races,
		Dictionary<string, Trait> traits,
		Dictionary<string, Class> classes,
		Dictionary<string, Subclass> subclasses)
	{
		var appearance = new GuildAppearance(
			guildSave.Appearance.Banner
		);
		
		var guild = new Guild(guildSave.Id, guildSave.Name, appearance)
		{
			Level = guildSave.Level,
			Experience = guildSave.Experience,
			Gold = guildSave.Gold,
			Reputation = guildSave.Reputation
		};
		
		// Restore members
		foreach (var pawnSave in guildSave.Members)
		{
			var pawn = new Pawn(
				pawnSave.Id,
				pawnSave.Name,
				races[pawnSave.RaceId],
				traits[pawnSave.TraitId],
				classes[pawnSave.ClassId],
				subclasses[pawnSave.SubclassId],
				pawnSave.Level,
				pawnSave.Team,
				pawnSave.Status
			);
			
			// Override stats with saved values
			pawn.Stats = new Stats
			{
				Atk = pawnSave.Stats.Atk,
				Def = pawnSave.Stats.Def,
				Mgc = pawnSave.Stats.Mgc
			};
			
			guild.AddMember(pawn);
		}
		
		// Restore parties
		foreach (var partySave in guildSave.Parties)
		{
			guild.CreateParty(partySave.Id, partySave.Name, partySave.MaxSize);
			var party = guild.GetParty(partySave.Id);
			party.Status = partySave.Status;
			party.MaxSize = partySave.MaxSize;
		}
		
		// Restore facilities
		foreach (var facilitySave in guildSave.Facilities)
		{
			var facility = new Facility(facilitySave.Id, facilitySave.Name, facilitySave.Type)
			{
				Level = facilitySave.Level,
			};
			guild.AddFacility(facility);
		}
		
		// Restore relationships
		foreach (var relSave in guildSave.Relationships)
		{
			guild.RelationshipManager.CreateRelationship(
				relSave.Pawn1Id,
				relSave.Pawn2Id,
				relSave.Affinity,
				relSave.Type
			);
		}
		
		return guild;
	}

	public Guild ConvertSaveDataToGuild(GuildSaveData guildSave, DataLoader dataLoader)
	{
		return ConvertSaveDataToGuild(
			guildSave,
			dataLoader.GetRaces(),
			dataLoader.GetTraits(),
			dataLoader.GetClasses(),
			dataLoader.GetSubclasses()
		);
	}
}
