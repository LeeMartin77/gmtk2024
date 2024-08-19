using Godot;
using System;
using System.Collections.Generic;

public class Servers : Node
{
	private Game _game;
	
	private VBoxContainer _serversContainer;
	private List<ServerCreationArgs> _allServers = new List<ServerCreationArgs>(){
		new ServerCreationArgs(){
			Name = "Test",
			Cost = 100,
			ExpensesPerTick = 100,
			WorkPerTick =  0.4f,
			NumberOfCores = 1,
			NumberOfPorts = 1,
		}	
	};
	
	public override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_serversContainer = GetNode<VBoxContainer>("AllServers");
			
		PackedScene containerTemplate = GD.Load<PackedScene>("res://ServerCard.tscn");
		
		foreach(ServerCreationArgs serverCreationArgs in _allServers) {
			var instance = containerTemplate.Instance<ServerCard>();
			instance.CreationArgs = serverCreationArgs;
			_serversContainer.AddChild(instance);
		}
	}

	public override void _Process(float delta)
	{
	}
}
