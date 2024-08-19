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
	private PackedScene _cableLinkScene;

	private Area2D _source;

	private Destination _destination;

	private int _cableLength = 45;
	private Node2D _cable;

	private RigidBody2D _finalJoint;

	public async override void _Ready()
	{
		_packetScene = GD.Load<PackedScene>("res://Packet.tscn");
		_cableLinkScene = GD.Load<PackedScene>("res://CableLink.tscn");
		_game = GetNode<Game>("/root/Root");
		_packetsIn = GetNode<Node2D>("PacketsIn");
		_source = GetNode<Area2D>("Source");
		_destination = GetNode<Destination>("Destination");
		_cable = GetNode<Node2D>("Cable");
		// first one gets extreme weight to "lock" it in place + gravity turned off;

		var offset = 31.5f;
		var pinJointSoftness = 0.5f;
		var startJoint = new StaticBody2D();
		_cable.AddChild(startJoint);
		var pinJoint = new PinJoint2D();
		_cable.AddChild(pinJoint);
		pinJoint.Position = new Vector2(0, 0);
		pinJoint.Softness = 0;
		pinJoint.NodeA = pinJoint.GetPathTo(startJoint);
		// offset
		for(var i = 0; i < _cableLength; i++) {
			var joint = _cableLinkScene.Instance<RigidBody2D>();
			joint.Weight = 10.0f;
			joint.Mass = 10.0f;

			_cable.AddChild(joint);
			joint.Position = new Vector2(0, offset * i);
			pinJoint.NodeB = pinJoint.GetPathTo(joint);

			var pinJointIn = new PinJoint2D();
			pinJointIn.Position = new Vector2(0, offset * (i + 1));
			pinJointIn.Softness = pinJointSoftness;

			_cable.AddChild(pinJointIn);
			pinJointIn.NodeA = pinJointIn.GetPathTo(joint);

			pinJoint = pinJointIn;
		}

		_finalJoint = new RigidBody2D();
		_finalJoint.GravityScale = 0;
		_finalJoint.Mass = 600000;
		_finalJoint.Weight = 600000;
		_finalJoint.Position = new Vector2(0, offset * _cableLength + 1);

		_cable.AddChild(_finalJoint);
		pinJoint.NodeB = pinJoint.GetPathTo(_finalJoint);

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
		packet.Contract = _contract;
		_packetsIn.AddChild(packet);
	}
	

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		_finalJoint.LinearVelocity = _finalJoint.Position.DirectionTo(_destination.Position) * _finalJoint.Position.DistanceTo(_destination.Position) * 5f;
	}
}
