using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public struct ServerCreationArgs {
	public string Name;
	public float Cost;
	public float ExpensesPerTick;
	public float WorkPerTick;
	public int NumberOfCores;
	public int NumberOfPorts;
}

public class Server : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

  	[Export]
	public float ExpensesPerTick = 110.0f;

	[Export]
	public float WorkPerTick = 0.4f;

	[Export]
	public int NumberOfCores = 2;
	
	[Export]
	public int NumberOfPorts = 2;

	private Label _coresLabel;
	private Label _infoLabel;

	private Game _game;

	private (double, double)[] coreStates;

	private Stack<Packet> pending;

	private ServerPort[] _ports;

	private PackedScene _portScene;

	private Rack _rack;

	private Node2D _portContainer;

	public override void _Ready()
	{
		_coresLabel = GetNode<Label>("Cores/Label");
		_infoLabel = GetNode<Label>("Info/Label");

		_rack = GetParent<Rack>();

		_portScene = GD.Load<PackedScene>("res://ServerPort.tscn");

		_infoLabel.Text = $"CORE: {NumberOfCores}\nRATE: {WorkPerTick*60}RPM\nCOST: £{DayTime.ToDailyMoney(ExpensesPerTick)}PD";

		_game = GetNode<Game>("/root/Root");

		_portContainer = GetNode<Node2D>("Ports");

		var numports = NumberOfPorts;
		var offsetport = 0;
		var offsetporty = 0;
		var portwidth = 50;
		while (numports > 0) {
			numports -= 1;
			var port = _portScene.Instance<Area2D>();
			port.Position = new Vector2(-290 + offsetport * portwidth, -26 + offsetporty * 50);
			port.Connect("body_entered", port, "_PortEntered");
			port.Connect("body_exited", port, "_PortLeft");
			_portContainer.AddChild(port);
			offsetport += 1;
			if (offsetport > 2) {
				offsetport = 0;
				offsetporty +=1;
			}
		}

		_ports = _portContainer.GetChildren().Cast<ServerPort>().ToArray();

		var initialStates = new List<(double, double)>();
		var numcores = NumberOfCores;
		while (numcores > 0) {
			numcores -= 1;
			initialStates.Add((0.0d, 0.0d));
		}
		coreStates = initialStates.ToArray();
		pending = new Stack<Packet>();
	}

	public void _on_Retire_Server() {
		_rack.RemoveServer(this);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
			// work off "debt" on core
			if (state.Item1 >= 0.0f) {
				coreStates[i].Item1 = state.Item1 - (WorkPerTick * _game.TickRateSeconds * delta);
			}
		}
		foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
			if (state.Item1 <= 0.0f) {
				// core is available to work
				if (pending.Count > 0) {

					var pkt = pending.Pop();
					coreStates[i] = (pkt.Work, pkt.Work);
					pkt.WorkRate = WorkPerTick;
				}
			}
		}

		foreach(var port in _ports) {
			var overlapping = port.GetOverlappingBodies();
			// lolfuk

			foreach(var overlap in overlapping) {
				if (overlap.GetType()  == typeof(Packet)) {
					if ((overlap as Packet).Processable && (overlap as Packet).WorkRate <= 0.0f){
						pending.Push(overlap as Packet);
					}
				} else if (overlap != port.ConnectedTo && overlap.GetType()  == typeof(Destination) && !(overlap as Destination).AttachedToMouse) {
					// "eject"
					var thispos = (overlap as Destination).Position;
					thispos.y += 50;
					(overlap as Destination).Position = thispos;
				}
			}
		}

		_coresLabel.Text = "";

		foreach(var (state, i) in coreStates.Select((val, i) => (val, i))) {
			if (state.Item1 <= 0.0f) {
				_coresLabel.Text += $"CORE {i}: IDLE\n";
			} else {
				_coresLabel.Text += $"CORE {i}: {((state.Item1 / state.Item2) * 100).ToString("0")}%\n";
			}
		}
	}
}
