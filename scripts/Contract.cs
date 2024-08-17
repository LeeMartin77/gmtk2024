using Godot;
using System;

public class Contract : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.


  	[Export]
	public float IncomePerTick = 100.0f;

    [Export]
    // TODO: We need this to be non-flat rates in the future
    public float PacketsPerTick = 0.2f;

    [Export]
    public float PacketTimeoutTime = 10f;

    [Export]
    public string ContractId = "a832c168-a98f-475d-825b-9f5803dfa3de";

    private Connection _connection;
    private Game _game;
    private int _lastTick;

    private float _pendingPackets = 0.0f;

    private int _sentPackets = 0;
    private int _receivedPackets = 0;

    public async override void _Ready()
    {
        _game = GetNode<Game>("/root/Root");
        await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        try {
		    _connection = GetNode<Firewall>("/root/Root/Game/Firewall").GetConnectionByContractId(ContractId);
        } catch {
            GD.Print("Connection doesn't exist!");
            QueueFree();
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_lastTick != _game.TickNumber) {
            _lastTick = _game.TickNumber;
            _pendingPackets += PacketsPerTick;
            while (_pendingPackets >= 1.0f) {
                _pendingPackets -= 1.0f;
                _sentPackets += 1;
                _connection.CreatePacket(PacketTimeoutTime);
            }
        }
    }

    public void ReceivePacket() {
        _receivedPackets += 1;
    }
}
