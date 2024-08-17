using Godot;
using System;
using System.Linq;

public class Firewall : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }


    public Connection[] GetConnections() {
        var children = GetChildren();
        return children.Cast<Connection>().ToArray();
    }

    // will throw if contract does not exist
    public Connection GetConnectionByContractId(string id) {
        return GetConnections().First((x) => x.ContractId == id);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
