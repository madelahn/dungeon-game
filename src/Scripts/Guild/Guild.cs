using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Guild : Node
{
	public string Id { get; set; }
	public string Name { get; set; }
	public int Level { get; set; }
	public int Experience { get; set; }
	public GuildAppearance Appearance { get; set; }
	
	public int Gold { get; set; }
	public int Reputation { get; set; }
	
	private Dictionary<string, Pawn> Members;
	private Dictionary<string, Party> Parties;
	
	private Dictionary<string, Facility> Facilities;
	
	public RelationshipManager RelationshipManager { get; private set; }
	
	public Guild(string id, string name, GuildAppearance appearance) {
		Id = id;
		Name = name;
		Level = 1;
		Experience = 0;
		Appearance = appearance;
		Gold = 0;
		Reputation = 0;

		Members = new Dictionary<string, Pawn>();
		Parties = new Dictionary<string, Party>();
		Facilities = new Dictionary<string, Facility>();
		RelationshipManager = new RelationshipManager();
	}
	
	public List<Facility> GetAllFacilities() {
		return Facilities.Values.ToList();
	}
	
	public Facility GetFacility(string facilityId) {
		if (Facilities.ContainsKey(facilityId)) {
			return Facilities[facilityId];
		}
		GD.PrintErr($"Facility with ID {facilityId} not found.");
		return null;
	}
	
	public void AddFacility(Facility facility) {
		if (!Facilities.ContainsKey(facility.Id)) {
			Facilities[facility.Id] = facility;
		}
	}
	
	public void AddMember(Pawn pawn) {
		if (!Members.ContainsKey(pawn.Id)) {
			Members[pawn.Id] = pawn;
		}
	}
	
	public List<Pawn> GetAllMembers() {
		return Members.Values.ToList();
	}
	
	public List<Party> GetAllParties() {
		return Parties.Values.ToList();
	}
	
	public void CreateParty(string partyId, string partyName, int partySize) {
		if (!Parties.ContainsKey(partyId)) {
			Parties[partyId] = new Party(partyId, partyName, partySize);
		}
	}
	
	public Party GetParty(string partyId) {
		if (Parties.ContainsKey(partyId)) {
			return Parties[partyId];
		}
		GD.PrintErr($"Party with ID {partyId} not found.");
		return null;
	}
}

public class GuildAppearance
{
	public string Banner { get; set; }
	
	public GuildAppearance(string banner = "default")
	{
		Banner = banner;
	}
}
