using Godot;
using System;

public class AvailableServers : ItemList
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private Game _game;
	public override void _Ready()
	{
		
		_game = GetNode<Game>("/root/Root");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
