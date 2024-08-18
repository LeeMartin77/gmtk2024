using Godot;
using System;

public struct ContractCreationArgs {
	public float IncomePerTick;
	public float PacketsPerTick;
	public float PacketTimeoutTime;
	public string ContractId;
}

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
	public float PacketTimeoutTime = 20f;

	[Export]
	public string ContractId = "a832c168-a98f-475d-825b-9f5803dfa3de";

	private Connection _connection;
	private Game _game;
	private Label _detailsLabel;
	private int _lastTick;

	private float _pendingPackets = 1.0f;

	public int SentPackets = 0;
	public int ReceivedPackets = 0;

	public async override void _Ready()
	{
		_game = GetNode<Game>("/root/Root");
		_detailsLabel = GetNode<Label>("ContractDetails");

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
			while (_pendingPackets >= 1.0f && _connection != null) {
				_pendingPackets -= 1.0f;
				SentPackets += 1;
				_connection.CreatePacket(PacketTimeoutTime);
			}
		}

		_detailsLabel.Text = $"{PacketsPerTick} packets/tick\n{IncomePerTick} income/tick\nsent: {SentPackets}\nrecv: {ReceivedPackets}";
	}

	public void ReceivePacket() {
		ReceivedPackets += 1;
	}
}
