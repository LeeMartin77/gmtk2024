using Godot;
using System;

public class Connection : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public string ContractId = "a832c168-a98f-475d-825b-9f5803dfa3de";

    private Contract _contract;
    private Game _game;

    private PackedScene _packetScene;

    public async override void _Ready()
    {
        _packetScene = GD.Load<PackedScene>("res://Packet.tscn");
        _game = GetNode<Game>("/root/Root");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        try {
		    _contract = _game.Contracts.GetContractById(ContractId);
        } catch {
            GD.Print("Contract doesn't exist!");
            QueueFree();
        }
    }

    public void CreatePacket(float timeout) {
        var packet = _packetScene.Instance<Packet>();
        packet.TimeoutTime = timeout;
        AddChild(packet);
    }

    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
