using Godot;
using System;
using System.Linq;

public class Firewall : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private double _offsetArea = 15f;

    private PackedScene _connectionScene;
    public override void _Ready()
    {
        _connectionScene = GD.Load<PackedScene>("res://Connection.tscn");
    }

    public Connection[] GetConnections() {
        var children = GetChildren();
        return children.Cast<Connection>().ToArray();
    }

    // will throw if connection does not exist
    public Connection GetConnectionByContractId(string id) {
        return GetConnections().First((x) => x.ContractId == id);
    }

    public void AddConnection(ConnectionCreationArgs args) {
        var nc = _connectionScene.Instance() as Connection;
        nc.ContractId = args.ContractId;
        AddChild(nc);
        var pos = nc.Position;
        pos.x += (float)(new Random().NextDouble() * (_offsetArea - -_offsetArea) + -_offsetArea);
        pos.y += (float)(new Random().NextDouble() * (_offsetArea - -_offsetArea) + -_offsetArea);
        nc.Position = pos;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
