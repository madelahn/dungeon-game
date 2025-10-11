using Godot;
using System;
using System.Collections.Generic;

public partial class RelationshipManager : Node
{
	private Dictionary<(string, string), RelationshipData> relationships;
		
	public RelationshipManager() {
		relationships = new Dictionary<(string, string), RelationshipData>();
	}
	
	private (string, string) MakeKey(string pawn1, string pawn2) {
		if (string.Compare(pawn1, pawn2) < 0) return (pawn1, pawn2);
		return (pawn2, pawn1);
	}
	
	public void CreateRelationship(string pawn1, string pawn2, int affinity, string type = "neutral") {
		var key = MakeKey(pawn1, pawn2);
		relationships[key] = new RelationshipData {
			Affinity = affinity,
			Type = type
		};
	}
	
	public void ChangeRelationship(string pawn1, string pawn2, int affinityChange) {
		var key = MakeKey(pawn1, pawn2);
		if (relationships.ContainsKey(key)) {
			var data = relationships[key];
			data.Affinity += affinityChange;
			relationships[key] = data;
		} else {
			CreateRelationship(pawn1, pawn2, affinityChange);
		}
	}
	
	public RelationshipData GetRelationship(string pawn1, string pawn2) {
		var key = MakeKey(pawn1, pawn2);
		return relationships.ContainsKey(key) ? relationships[key]
		: new RelationshipData { Affinity = 0, Type = "neutral" };
	}
}

public class RelationshipData {
	public int Affinity { get; set; }
	public string Type { get; set; }
}
