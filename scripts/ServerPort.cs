using Godot;
using System;

public class ServerPort : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	public Destination ConnectedTo;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void _PortEntered(Node enteringBody) {
		if (enteringBody.GetType() == typeof(Destination) && ConnectedTo == null) {
			ConnectedTo = enteringBody as Destination;
			ConnectedTo.ConnectedTo = this;
		}
	}

	public void _PortLeft(Node enteringBody) {
		if (enteringBody.GetType() == typeof(Destination) && ConnectedTo == enteringBody) {
			ConnectedTo.ConnectedTo = null;
			ConnectedTo = null;
		}
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
