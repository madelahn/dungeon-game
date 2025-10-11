using Godot;
using System;
using System.Collections.Generic;

public class SaveData
{
	public string SaveId { get; set; }
	public DateTime SaveDate { get; set; }
	public string SaveName { get; set; }
	
	// Guild Data
	public GuildSaveData Guild { get; set; }
	
	// Game Progress
	public int PlayTime { get; set; } // in seconds
	public string CurrentScene { get; set; }
	
	public SaveData()
	{
		SaveId = Guid.NewGuid().ToString();
		SaveDate = DateTime.Now;
	}
}

public class GuildSaveData
{
	public string Id { get; set; }
	public string Name { get; set; }
	public int Level { get; set; }
	public int Experience { get; set; }
	public GuildAppearanceSaveData Appearance { get; set; }
	
	public int Gold { get; set; }
	public int Reputation { get; set; }
	
	public List<PawnSaveData> Members { get; set; }
	public List<PartySaveData> Parties { get; set; }
	public List<FacilitySaveData> Facilities { get; set; }
	public List<RelationshipSaveData> Relationships { get; set; }
	
	public GuildSaveData()
	{
		Members = new List<PawnSaveData>();
		Parties = new List<PartySaveData>();
		Facilities = new List<FacilitySaveData>();
		Relationships = new List<RelationshipSaveData>();
	}
}

public class PawnSaveData
{
	public string Id { get; set; }
	public string Name { get; set; }
	public string RaceId { get; set; }
	public string TraitId { get; set; }
	public string ClassId { get; set; }
	public string SubclassId { get; set; }
	public int Level { get; set; }
	public string Team { get; set; }
	public PawnStatus Status { get; set; }
	public StatsSaveData Stats { get; set; }
}

public class StatsSaveData
{
	public int Atk { get; set; }
	public int Def { get; set; }
	public int Mgc { get; set; }
}

public class PartySaveData
{
	public string Id { get; set; }
	public string Name { get; set; }
	public List<string> MemberIds { get; set; }
	public PartyStatus Status { get; set; }
	public int MaxSize { get; set; }
}

public class FacilitySaveData
{
	public string Id { get; set; }
	public string Name { get; set; }
	public FacilityType Type { get; set; }
	public int Level { get; set; }
	public bool IsUnlocked { get; set; }
}

public class RelationshipSaveData
{
	public string Pawn1Id { get; set; }
	public string Pawn2Id { get; set; }
	public int Affinity { get; set; }
	public string Type { get; set; }
}

public class GuildAppearanceSaveData
{
	public string Banner { get; set; }
	public string ColorScheme { get; set; }
	public string Architecture { get; set; }
}
