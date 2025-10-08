using Godot;
using System;

public partial class NewGame : Button
{
	private RelationshipManager relationshipManager;
	
	public override void _Ready() {
		relationshipManager = new RelationshipManager();
		GD.Print("Button ready");
	}
	
	private void _on_button_down() {
		GD.Print("Button pressed...");
	}
}
