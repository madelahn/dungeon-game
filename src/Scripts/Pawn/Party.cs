using Godot;
using System.Collections.Generic;

public class Party
{
	public string Id { get; set; }
	public string Name { get; set; }
	public List<string> MemberIds { get; private set; }
	public PartyStatus Status { get; set; }
	public int MaxSize { get; set; }
	
	public Party(string id, string name, int maxSize = 4)
	{
		Id = id;
		Name = name;
		MemberIds = new List<string>();
		Status = PartyStatus.Idle;
		MaxSize = maxSize;
	}
	
	public void AddMember(string pawnId)
	{
		if (MemberIds.Count < MaxSize && !MemberIds.Contains(pawnId))
		{
			MemberIds.Add(pawnId);
		}
	}
	
	public void RemoveMember(string pawnId)
	{
		MemberIds.Remove(pawnId);
	}
	
	public bool HasMember(string pawnId)
	{
		return MemberIds.Contains(pawnId);
	}
	
	public bool IsFull()
	{
		return MemberIds.Count >= MaxSize;
	}
}

public enum PartyStatus
{
	Idle,
	OnQuest,
	Resting
}
