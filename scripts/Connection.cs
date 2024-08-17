using Godot;
using System;

public struct ConnectionCreationArgs {
    public string ContractId;
}

public class Connection : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    [Export]
    public string ContractId = "a832c168-a98f-475d-825b-9f5803dfa3de";

    private Contract _contract;
    private Game _game;

    private Node2D _packetsIn;

    private PackedScene _packetScene;

    private Area2D _source;

    public async override void _Ready()
    {
        _packetScene = GD.Load<PackedScene>("res://Packet.tscn");
        _game = GetNode<Game>("/root/Root");
        _packetsIn = GetNode<Node2D>("PacketsIn");
        _source = GetNode<Area2D>("Source");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        try {
		    _contract = _game.Contracts.GetContractById(ContractId);
        } catch {
            GD.Print("Contract doesn't exist!");
            QueueFree();
        }
    }

    public void _on_Source_body_entered(Node potentialPacket) {
        if (potentialPacket.GetType()  == typeof(Packet)) {
            if ((potentialPacket as Packet).Work <= 0.0f) {
                potentialPacket.QueueFree();
                _contract.ReceivePacket();
            }
        }
    }

    public void CreatePacket(float timeout) {
        var packet = _packetScene.Instance<Packet>();
        packet.TimeoutTicks = timeout;
        var pktpos = _source.Position;
        pktpos.y += 10;
        packet.Position = pktpos;
        _packetsIn.AddChild(packet);
    }

    

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
