public class Facility
{
	public string Id { get; set; }
	public string Name { get; set; }
	public FacilityType Type { get; set; }
	public int Level { get; set; }
	
	public Facility(string id, string name, FacilityType type)
	{
		Id = id;
		Name = name;
		Type = type;
		Level = 1;
	}
	
	public void Upgrade()
	{
		Level++;
	}
}

public enum FacilityType
{
	Store,
	Hospital,
	Training,
	Tavern,
	Bank,
	QuestBoard
}
