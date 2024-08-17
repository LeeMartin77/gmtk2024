using Godot;
using System;
using System.Linq;

public class Rack : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private PackedScene _serverScene;
    public override void _Ready()
    {
        
        _serverScene = GD.Load<PackedScene>("res://Server.tscn");
    }

    public Server[] GetServers() {
        var children = GetChildren();
        return children.Cast<Server>().ToArray();
    }


    public void AddServer(ServerCreationArgs args) {

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
